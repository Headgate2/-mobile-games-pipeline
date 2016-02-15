using UnityEngine;
using UnityEngine.Events;
using System.Collections;

/*
	Event that exposes specific MonoBehaviour messages to the editor.
*/
[AddComponentMenu("Event/Event MonoBehaviour")]
public class MonoBehaviourEvent : MonoBehaviour
{
	[SerializeField, Tooltip("This function is always called before any Start functions and also just after a prefab is instantiated. (If a GameObject is inactive during start up Awake is not called until it is made active, or a function in any script attached to it is called.)")]
	UnityEvent onAwake;
	public UnityEvent OnAwake { get { return onAwake; } }

	[SerializeField, Tooltip("Start is called before the first frame update only if the script instance is enabled.")]
	UnityEvent onStart;
	public UnityEvent OnStart { get { return onStart; } }

	[SerializeField, Tooltip("(only called if the Object is active): This function is called just after the object is enabled. This happens when a MonoBehaviour instance is created, such as when a level is loaded or a GameObject with the script component is instantiated.")]
	UnityEvent onEnable;
	public UnityEvent OnEnableEvent { get { return onEnable; } }

	[SerializeField, Tooltip("This function is called when the behaviour becomes disabled or inactive.")]
	UnityEvent onDisable;
	public UnityEvent OnDisableEvent { get { return onDisable; } }

	[SerializeField, Tooltip("This function is called after all frame updates for the last frame of the object’s existence (the object might be destroyed in response to Object.Destroy or at the closure of a scene).")]
	UnityEvent onDestroy;
	public UnityEvent OnDestroyEvent { get { return onDestroy; } }
	
	void Awake()
	{
		OnAwake.Invoke();
	}

	void Start()
	{
		OnStart.Invoke();
	}

	void OnEnable()
	{
		OnEnableEvent.Invoke();
	}

	void OnDisable()
	{
		OnDisableEvent.Invoke();
    }

	void OnDestroy()
	{
		OnDestroyEvent.Invoke();
	}
}
