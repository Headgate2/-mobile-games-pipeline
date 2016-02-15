using UnityEngine;
using UnityEngine.Events;
using System.Collections;

/*
	Event that is used to add a delay between its trigger, and an event firing.

	There are optional other trigger functions, including:
	TriggerImmmediate - Fires the event with no delay.
	TriggerImmediateAlways - Fires the event with no delay, event if the script is disabled.
*/
[AddComponentMenu("Event/Delay Event")]
public class DelayEvent : MonoBehaviour
{
	[SerializeField, Tooltip("Should this event be triggered once before the first frame of this object?")]
	bool triggerOnStart = false;
	public bool TriggerOnStart { get { return triggerOnStart; } set { triggerOnStart = value; } }

	[SerializeField, Tooltip("Number of seconds after Trigger is called that the DelayedEvent is called")]
	float delaySeconds;
	public float DelaySeconds { get { return delaySeconds; } set { delaySeconds = value; } }

	[SerializeField, Tooltip("Called Delay Seconds after Trigger is called.")]
	UnityEvent delayedEvent;
	public UnityEvent DelayedEvent { get { return delayedEvent; } }

	[AdvancedInspector.Inspect]
	public void Trigger()
	{
#if UNITY_EDITOR
		if (!Application.isPlaying)
			return;
#endif
		if (!enabled || !gameObject.activeInHierarchy)
			return;

		this.InvokeAfter(delegate () { DelayedEvent.Invoke(); }, DelaySeconds);
	}

	[AdvancedInspector.Inspect]
	public void TriggerImmediate()
	{
#if UNITY_EDITOR
		if (!Application.isPlaying)
			return;
#endif
		if (!enabled || !gameObject.activeInHierarchy)
			return;

		DelayedEvent.Invoke();
	}

	[AdvancedInspector.Inspect]
	public void TriggerImmediateAlways()
	{
#if UNITY_EDITOR
		if (!Application.isPlaying)
			return;
#endif
		DelayedEvent.Invoke();
	}

	void Start()
	{
		if (TriggerOnStart)
			Trigger();
	}
}
