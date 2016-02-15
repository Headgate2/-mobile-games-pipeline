using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;

public static class MonoExtensions
{
	///Calls an action on the next frame
	public static void InvokeNextFrame(this MonoBehaviour mb, UnityAction action)
	{
		mb.InvokeAfter(action, null);
	}

	///Calls an action at the end of the frame
	public static void InvokeEndOfFrame(this MonoBehaviour mb, UnityAction action)
	{
		mb.InvokeAfter(action, new WaitForEndOfFrame());
	}

	///Calls an action at the next FixedUpdate
	public static void InvokeFixedUpdate(this MonoBehaviour mb, UnityAction action)
	{
		mb.InvokeAfter(action, new WaitForFixedUpdate());
	}

	///Calls an action after a seconds within the given range of (fromSeconds, toSeconds)
	public static void InvokeAfterRandom(this MonoBehaviour mb, UnityAction action, float fromSeconds, float toSeconds)
	{
		mb.InvokeAfter(action, UnityEngine.Random.Range(fromSeconds, toSeconds));
	}

	//Calls an action after a given amount of seconds
	public static void InvokeAfter(this MonoBehaviour mb, UnityAction action, float seconds)
	{
		mb.InvokeAfter(action, new WaitForSeconds(seconds));
	}

	//Calls an action after a yield instruction has completed
	public static void InvokeAfter(this MonoBehaviour mb, UnityAction action, YieldInstruction yieldInstruction)
	{
		mb.StartCoroutine(WaitBefore(yieldInstruction, action));
	}

	//Coroutine that waits for the yield instruction to complete, then calls an action
	public static IEnumerator WaitBefore(YieldInstruction ins, UnityAction action)
	{
		yield return ins;
		action.Invoke();
	}

	//Repeats an action every frame starting on the next frame
	public static void InvokeFrameRepeat(this MonoBehaviour mb, UnityAction action, uint numTimes)
	{
		mb.StartCoroutine(RepeatCount(null, action, numTimes));
	}

	public static IEnumerator RepeatCount(YieldInstruction ins, UnityAction action, uint numTimes)
	{
		for(uint x = 0; x < numTimes; x++)
		{
			yield return ins;
			action.Invoke();
		}
	}

	public static void InvokeFrameRepeatForever(this MonoBehaviour mb, UnityAction action)
	{
		mb.StartCoroutine(RepeatForever(null, action));
	}

	//Repeats an action forever with a number of seconds between each call
	public static void InvokeRepeatForeverSeconds(this MonoBehaviour mb, UnityAction action, float secondsBetween)
	{
		mb.StartCoroutine(RepeatForever(new WaitForSeconds(secondsBetween), action));
	}

	public static IEnumerator RepeatForever(YieldInstruction ins, UnityAction action)
	{
		while (true)
		{
			yield return ins;
			action.Invoke();
		}
	}

	public static void InvokeWhenTrue<T>(this MonoBehaviour mb, T source, System.Func<T, bool> boolSelector, UnityAction action)
	{
		mb.StartCoroutine(WaitForValue(null, source, boolSelector, true, null, action));
	}

	public static void InvokeWhenFalse<T>(this MonoBehaviour mb, T source, System.Func<T, bool> boolSelector, UnityAction action)
	{
		mb.StartCoroutine(WaitForValue(null, source, boolSelector, false, null, action));
	}

	public static IEnumerator WaitForValue<T, K>(YieldInstruction waitTicker, T source, System.Func<T, K> selector, K desiredVal, UnityAction tick, UnityAction finished) where K : System.IComparable
	{
		while (selector(source).CompareTo(desiredVal) != 0)
		{
			yield return waitTicker;
			if (tick != null)
				tick.Invoke();
		}
		if (finished != null)
			finished.Invoke();
	}

	//Extension for monobehaviour and all other components that allow you to get/add a component to/from it's gameobject
	public static T GetOrAddComponent<T>(this Component c) where T : Component
	{
		T item = c.GetComponent<T>();
		if(item == null)
			item = c.gameObject.AddComponent<T>();
		return item;
	}

	public static T GetOrAddComponent<T>(this GameObject g) where T : Component
	{
		return g.transform.GetOrAddComponent<T>();
	}

	public static T GetAbstract<T>(this Component comp) where T : Component
	{
		return comp.GetComponent<T>();
	}

	public static T GetAbstract<T>(this GameObject inObj) where T : Component
	{
        return inObj.transform.GetComponent<T>();
    }

	public static T GetInterface<T>(this Component comp) where T : class
	{
		if (!typeof(T).IsInterface)
		{
			Debug.LogError(typeof(T).ToString() + ": is not an actual interface!");
			return null;
		}
		return comp.GetComponent(typeof(T)) as T;
	}

    public static T GetInterface<T>(this GameObject inObj) where T : class
    {
		return inObj.transform.GetInterface<T>();
    }

    public static T[] GetInterfaces<T>(this GameObject inObj) where T : class
	{
		return inObj.transform.GetInterfaces<T>();
	}

	public static T[] GetInterfaces<T>(this Component inObj) where T : class
	{
		if (!typeof(T).IsInterface)
		{
			Debug.LogError(typeof(T).ToString() + ": is not an actual interface!");
			return new T[] { };
		}
		return inObj.GetComponents(typeof(T)) as T[];
	}

	/// <summary>
	/// Accepts a gameobject or component and returns the gameobject
	/// </summary>
	/// <param name="o"></param>
	/// <returns></returns>
	public static GameObject GetGameObject(UnityEngine.Object o)
	{
		GameObject go = o as GameObject;
		if (go != null)
			return go;
		
		Component c = o as Component;
		if (c != null)
			return c.gameObject;

		return null;
	}

	/// <summary>
	/// Accepts a gameobject or component
	/// If a gameObject, sets its active state
	/// If a component, sets its enabled state
	/// </summary>
	/// <param name="o"></param>
	/// <returns></returns>
	public static void SetActive(UnityEngine.Object o, bool active)
    {
        GameObject go = o as GameObject;
        if (go != null)
        {
            go.SetActive(active);
            return;
        }

        Behaviour c = o as Behaviour;
        if (c != null)
        {
            c.enabled = active;
            return;
        }
    }

	/// <summary>
	/// Clones the prefab
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="prefab"></param>
	/// <returns></returns>
	public static T Clone<T>(this T prefab) where T : UnityEngine.Object
	{
		return UnityEngine.Object.Instantiate<T>(prefab);
	}

	/// <summary>
	/// Clones the prefab and set's the clone's position
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="prefab"></param>
	/// <param name="position"></param>
	/// <returns></returns>
	public static T Clone<T>(this T prefab, Vector3 position) where T : UnityEngine.Object
	{
		T instance = prefab.Clone<T>();
		GameObject go = GetGameObject(instance);
		if (go)
			go.transform.position = position;
		return instance;
	}

	/// <summary>
	/// Clones the prefab and set's the clone's position and rotation
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="prefab"></param>
	/// <param name="position"></param>
	/// <param name="rotation"></param>
	/// <returns></returns>
	public static T Clone<T>(this T prefab, Vector3 position, Quaternion rotation) where T : UnityEngine.Object
	{
		T instance = prefab.Clone<T>(position);
		GameObject go = GetGameObject(instance);
		if (go)
			go.transform.rotation = rotation;
		return instance;
	}

	public static void Destroy<T>(this T obj, bool immediate = false) where T : UnityEngine.Object
	{
		if(immediate)
			UnityEngine.Object.DestroyImmediate(obj);
		else
			UnityEngine.Object.Destroy(obj);
	}

	//Copies a component's values to another component of the same type
	public static bool CopyComponent<T>(this T source, T target) where T : Component
	{
		BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
		PropertyInfo[] pinfos = source.GetType().GetProperties(flags);
		foreach (var pinfo in pinfos)
		{
			if (pinfo.CanWrite)
			{
				try
				{
					pinfo.SetValue(target, pinfo.GetValue(source, null), null);
				}
				catch { } // In case of NotImplementedException being thrown. For some reason specifying that exception didn't seem to catch it, so I didn't catch anything specific.
			}
		}
		FieldInfo[] finfos = source.GetType().GetFields(flags);
		foreach (var finfo in finfos)
		{
			finfo.SetValue(target, finfo.GetValue(source));
		}
		return true;
	}

	public static T AddComponent<T>(this GameObject go, T componentToCopyFrom) where T : Component
	{
		T comp = go.AddComponent<T>();
		if(componentToCopyFrom.CopyComponent(comp))
			return comp;
		else
		{
			UnityEngine.Object.Destroy(comp);
			return null;
		}
	}

	public static bool Contains(this LayerMask mask, GameObject go)
	{
		return ( mask & (1 << go.layer) ) != 0;
	}

	public static bool Contains(this LayerMask mask, Component c)
	{
		return mask.Contains(c.gameObject);
	}
}
