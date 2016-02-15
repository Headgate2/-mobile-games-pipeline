using UnityEngine;
using UnityEngine.Events;
using System.Collections;

/*
	Listener for our global events, that are fired from the GlobalEventManager
*/
[AddComponentMenu("Event/Global Message Listener Event")]
public class GlobalEvent : MonoBehaviour
{
	[SerializeField, Tooltip("What Global Event are we listening for?")]
	string eventName;
	public string EventName {
		get { return eventName; }
		set {
			UnRegister();
			eventName = value;
			Register();
		}
	}

	[SerializeField, Tooltip("Called when the Global event is triggered, and this script is enabled.")]
	UnityEvent onGlobalEvent;
	public UnityEvent OnGlobalEvent { get { return onGlobalEvent; } }

	void Awake()
	{
		Register();
	}

	void OnDestroy()
	{
		UnRegister();
	}

	public void Register()
	{
		Messenger.AddListener(EventName, Trigger);
	}

	public void UnRegister()
	{
		Messenger.RemoveListener(EventName, Trigger);
	}

	void Trigger()
	{
		if(enabled)
			OnGlobalEvent.Invoke();
	}
}
