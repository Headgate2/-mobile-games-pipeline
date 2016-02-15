using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// How is a pool allowed to generate new instances of its IPoolables
/// </summary>
public enum PoolLoadType
{
	None,                       //cannot generate new instances
	CreateOnNewGameObject,      //generate instances on new empty gameobjects
	LoadedFromResources,        //created from a Resources prefab
}

public class Pool<T> where T : MonoBehaviour, IPoolable
{
	private List<T> checkedOut = new List<T>();
	public IEnumerable<T> CheckedOut { get { return checkedOut; } }

	private List<T> released = new List<T>();
	public IEnumerable<T> Released { get { return released; } }

	/// <summary>
	/// How large is the pool of items that are checked out
	/// </summary>
	public int CheckedOutCount { get { return checkedOut.Count; } }

	/// <summary>
	/// How large is the pool of items that are released and contained in the pool
	/// </summary>
	public int ReleasedCount
	{
		get { return released.Count; }
		set
		{
			if (value < 0)
				value = 0;
			while (released.Count > value)
			{
				UnityEngine.Object.Destroy(released.Last().gameObject);
				released.RemoveAt(released.Count - 1);
			}
			while (released.Count < value)
				GenerateInstance();
		}
	}

	CounterAction validatePoolAction = new CounterAction();
	
	public uint ValidateFrequency
	{
		get { return validatePoolAction.TargetCount; }
		set { validatePoolAction.TargetCount = value; }
	}

	public PoolLoadType LoadMethod { get; private set; }
	public string ResourcesLoadPath { get; private set; }

	private Pool() { }

	public Pool(PoolLoadType loadMethod, string resourcesLoadPath = "")
	{
		LoadMethod = loadMethod;
		ResourcesLoadPath = resourcesLoadPath;
		validatePoolAction.OnCount += ValidatePool;
		ValidateFrequency = 10;
	}

	/// <summary>
	/// Generates an instance of the poolable using the defined parameters
	/// </summary>
	private T GenerateInstance()
	{
		T ins = null;
		if (LoadMethod == PoolLoadType.CreateOnNewGameObject)
		{
			GameObject go = new GameObject(typeof(T).Name + " Instance");
			if (go == null)
			{
				Debug.LogError("Failed to create gameobject for instance of " + typeof(T).Name + ".  Please check your memory constraints.");
				return null;
			}
			ins = go.AddComponent<T>();
			if (ins == null)
			{
				Debug.LogError("Failed to add component of " + typeof(T).Name + "to new gameobject.  Please check your memory constraints.");
				UnityEngine.Object.Destroy(go);
				return null;
			}
		}
		else if (LoadMethod == PoolLoadType.LoadedFromResources)
		{
			if (string.IsNullOrEmpty(ResourcesLoadPath))
			{
				Debug.LogError("resourcesLoadPath is not a valid Resources location for " + typeof(T).Name);
				return null;
			}
			T pref = Resources.Load<T>(ResourcesLoadPath);
			if (pref == null)
			{
				Debug.LogError("Failed to load prefab with " + typeof(T).Name + " component attached to it from folder Resources/" + ResourcesLoadPath + ".  Please add a prefab with the component to that location, or update the location.");
				return null;
			}
			ins = UnityEngine.Object.Instantiate<T>(pref);
			if (ins == null)
			{
				Debug.LogError("Failed to create instance of prefab " + pref + " with component " + typeof(T).Name + ".  Please check your memory constraints");
				return null;
			}
		}
		else
		{
			return null;
		}

		ins.OnGenerate();
		return ins;
	}

	/// <summary>
	/// Checks out an instance of the poolable from the pool.
	/// If there all of the pool is checked out, a new instance is attempted to be generated.
	/// </summary>
	public T CheckOut()
	{
		//NOTE: if released contains invalid objects, RandomObject could return null, even if valid
		//			objects are available in the released pool
		T item = released.RandomObject();
		if (item == null)
		{
			item = GenerateInstance();
		}
		if (item != null)
		{
			item.OnCheckout();
			released.Remove(item);
			Add(item);
		}
		validatePoolAction.Increment();
		return item;
	}

	/// <summary>
	/// Releases the given instance (if it is from this pool and checked out)
	/// </summary>
	/// <param name="instance"></param>
	/// <returns>did the checked out items contain the given instance</returns>
	public bool Release(T instance)
	{
		validatePoolAction.Increment();

		int index = checkedOut.IndexOf(instance);
		if (index >= 0)
		{
			instance.OnRelease();
			released.Add(checkedOut[index]);
			checkedOut.RemoveAt(index);
			return true;
		}
		return false;
	}

	/// <summary>
	/// Add an instance of the poolable to this pool
	/// The instance is then considered Checked Out from the pool
	/// </summary>
	/// <param name="instance"></param>
	/// <returns>if instance was successfully added</returns>
	public bool Add(T instance)
	{
		if (checkedOut.Contains(instance) || released.Contains(instance))
			return false;
		checkedOut.Add(instance);
		return true;
	}

	/// <summary>
	/// Removes an instance of the poolable to this pool
	/// if the item is checked out, it is released
	/// </summary>
	/// <param name="instance"></param>
	/// <returns>if instance was successfully removed</returns>
	public bool Remove(T instance)
	{
		int rel = released.IndexOf(instance);
		int ch = checkedOut.IndexOf(instance);
		if (rel < 0 && ch < 0)
			return false;
		else if (rel >= 0)
		{
			released.RemoveAt(rel);
		}
		else
		{
			if (!Release(instance))
			{
				Debug.LogError("Failed to release instance of removed item");
				checkedOut.RemoveAt(ch);
			}
			else
				released.Remove(instance);
		}
		return true;
	}

	/// <summary>
	/// Validates the items in the pool (checked out and released) to ensure they have not been externally destroyed
	/// </summary>
	public void ValidatePool()
	{
		for (int x = 0; x < released.Count;)
		{
			if (released[x] == null)
				released.RemoveAt(x);
			else
				x++;
		}

		for (int x = 0; x < checkedOut.Count;)
		{
			if (checkedOut[x] == null)
				checkedOut.RemoveAt(x);
			else
				x++;
		}
	}
}