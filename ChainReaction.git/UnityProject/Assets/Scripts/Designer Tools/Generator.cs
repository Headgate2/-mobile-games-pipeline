using UnityEngine;
using UnityEngine.Events;
using System.Collections;

/*
	Event class for generating prefabs from events.
*/
[AddComponentMenu("Event/Prefab Generator")]
public class Generator : MonoBehaviour 
{
	[SerializeField]
	GameObject prefab;

	[System.Serializable] public class GenerateEvent : UnityEvent<GameObject> { }

	[SerializeField, Tooltip("Called when this generator creates an object")]
	GenerateEvent onGenerate;
	public GenerateEvent OnGenerate { get { return onGenerate; } }

	//This is here so designers can disable the generator with the checkbox in the inspector
	void Start() { }

	[AdvancedInspector.Inspect]
	public void Generate()
	{
#if UNITY_EDITOR
		if (!Application.isPlaying)
			return;
#endif

		if (!enabled)
			return;
		TryGenerate();
	}

	[AdvancedInspector.Inspect]
	public void GenerateAlways()
	{
#if UNITY_EDITOR
		if (!Application.isPlaying)
			return;
#endif
		TryGenerate();
    }

	private void TryGenerate()
	{
		if (prefab == null)
		{
			Debug.LogWarning("Prefab is NOT set in Generator on " + name);
			return;
		}

		OnGenerate.Invoke((GameObject)Instantiate(prefab, transform.position, transform.rotation));
	}
}
