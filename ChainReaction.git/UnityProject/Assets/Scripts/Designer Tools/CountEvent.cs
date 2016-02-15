using UnityEngine;
using System.Collections;
using UnityEngine.Events;

/*
	Count Event component

	This event is used to trigger an event when a count reaches a certain number.
*/
[AddComponentMenu("Event/Counter Event")]
public class CountEvent : MonoBehaviour
{
	[HideInInspector]
	[SerializeField, Tooltip("What is the starting count, and the current count during gameplay.")]
	int count = 0;

	[AdvancedInspector.Inspect]
	public int Count {
		get { return count; }
		set {
			count = value;
#if UNITY_EDITOR
			if (!Application.isPlaying)
				return;
#endif
			OnCountChanged.Invoke(count);
			if (count == EndCount)
			{
				OnCountEnd.Invoke(count);
				if (ResetOnEnd)
					count = 0;
			}
		}
	}

	[SerializeField, Tooltip("At what count do we fire this event?")]
	int endCount = 0;
	public int EndCount { get { return endCount; } set { endCount = value; } }

	[SerializeField, Tooltip("Does the counter reset to 0 after firing an event?")]
	bool resetOnEnd = false;
	public bool ResetOnEnd { get { return resetOnEnd; } set { resetOnEnd = value; } }

	[System.Serializable] public class IntEvent : UnityEvent<int> { }

	[SerializeField, Tooltip("Called when the count changes")]
	IntEvent onCountChanged;
	public IntEvent OnCountChanged { get { return onCountChanged; } }

	[SerializeField, Tooltip("Called when the count reaches the end count")]
	IntEvent onCountEnd;
	public IntEvent OnCountEnd { get { return onCountEnd; } }

#if ADVANCED_INSPECTOR
	[AdvancedInspector.Inspect]
#endif
	public void Increment() { Count++; }

#if ADVANCED_INSPECTOR
	[AdvancedInspector.Inspect]
#endif
	public void Decrement() { Count--; }
}
