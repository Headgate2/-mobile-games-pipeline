using UnityEngine;
using UnityEngine.Events;
using System.Collections;

/*
	Event that happens regularly over some defined interval
*/
[AddComponentMenu("Event/Event Interval")]
public class IntervalEvent : MonoBehaviour
{
	public enum UpdateType
	{
		Update,
		FixedUpdate,
		LateUpdate,
		Seconds
	}

	[SerializeField, Tooltip("What kind of update loop should this interval run on?")]
	UpdateType updateEvery = UpdateType.Seconds;
	public UpdateType UpdateEvery { get { return updateEvery; } set { updateEvery = value; } }
	private bool UpdateEverySeconds() { return UpdateEvery == UpdateType.Seconds; }

	[AdvancedInspector.Inspect("UpdateEverySeconds")]
	[SerializeField, Tooltip("Number of seconds between calling this event")]
	float intervalSeconds = 1f;

	[SerializeField, Tooltip("Called before the first frame and every Interval Seconds after")]
	UnityEvent onIntervalEvent;
	public UnityEvent OnIntervalEvent { get { return onIntervalEvent; } }

	public bool Running { get; private set; }

	void Awake()
	{
		Running = false;
	}

	void OnEnable()
	{
		if (!Running)
			StartCoroutine("IntervalCoroutine");
	}

	void OnDisable()
	{
		if (Running)
		{
			StopCoroutine("IntervalCoroutine");
			Running = false;
		}
	}

	void Update()
	{
		if (UpdateEvery == UpdateType.Update)
			OnIntervalEvent.Invoke();
	}

	void FixedUpdate()
	{
		if (UpdateEvery == UpdateType.FixedUpdate)
			OnIntervalEvent.Invoke();
	}

	void LateUpdate()
	{
		if (UpdateEvery == UpdateType.LateUpdate)
			OnIntervalEvent.Invoke();
	}

	IEnumerator IntervalCoroutine()
	{
		Running = true;
		while (true)
		{
			while(UpdateEvery == UpdateType.Seconds)
			{
				OnIntervalEvent.Invoke();
				yield return new WaitForSeconds(intervalSeconds);
			}
			yield return null;		//stops the infinite loop from locking up the interval event
		}
    }
}
