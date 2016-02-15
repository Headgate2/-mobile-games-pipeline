using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class TriggerWhileInsideBase<TriggerType, ColliderType, ColliderEventType> : MonoBehaviour where TriggerType : TriggerBase<ColliderType, ColliderEventType> where ColliderType : Component where ColliderEventType : UnityEvent<ColliderType>
{
	private TriggerType trigger;
	public TriggerType Trigger { get { return trigger ?? (trigger = GetComponent<TriggerType>()); } }

	private IntervalEvent interval;
	public IntervalEvent Interval { get { return interval ?? (interval = GetComponent<IntervalEvent>()); } }

	[SerializeField]
	private ColliderEventType onColliderInside;
	public ColliderEventType OnColliderInside { get { return onColliderInside; } }

	protected virtual void Awake()
	{
		Interval.OnIntervalEvent.AddListener(OnInterval);
	}

	protected virtual void OnInterval()
	{
		foreach (ColliderType c in Trigger.Inside)
			OnColliderInside.Invoke(c);
	}
}
