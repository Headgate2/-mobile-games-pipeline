using UnityEngine;
using UnityEngine.Events;
using System.Collections;

/*
	Playable Event component.

	This event has 2 states, play and stop.

	Recommended use would be turning things on and off with the two different states (e.g. turn on a light while this event is playing).

	This event must be manually stopped by default, however can be configured to automatically stop after a given amount of seconds (if the SecondsToAutoStop is > 0).

	If the gameobject this script is attached to is disabled before stop is called using the auto-stop, it will NOT be auto-stopped.  (This is due to the nature of how coroutines are connected to gameobjects)
*/
[AddComponentMenu("Event/Playable Event")]
public class PlayableEvent : MonoBehaviour 
{
	[SerializeField, Tooltip("How many seconds until this playable automatically stops?  0 will never auto stop.")]
	float secondsToAutoStop = 0f;
	public float SecondsToAutoStop { get { return secondsToAutoStop; } set { secondsToAutoStop = value; } }

	[SerializeField, Tooltip("Should we allow the playable event to fire if we are already playing?")]
	private bool allowMultiPlay = true;
	public bool AllowMultiPlay { get { return allowMultiPlay; } }

	[SerializeField, Tooltip("Called when this playable begins playing?")]
	UnityEvent onPlay;
	public UnityEvent OnPlay { get { return onPlay; } }

	[SerializeField, Tooltip("Called when this playable stops playing?")]
	UnityEvent onStop;
	public UnityEvent OnStop { get { return onStop; } }

	[AdvancedInspector.Inspect, AdvancedInspector.ReadOnly]
	public bool Playing { get; private set; }       //is this event in the playing state?  False is in the stopped state.

	[AdvancedInspector.Inspect]
	public void Play()
	{
#if UNITY_EDITOR
		if (!Application.isPlaying)
			return;
#endif
		if (Playing && !AllowMultiPlay)
			return;

		Playing = true;
		OnPlay.Invoke();
		if (secondsToAutoStop > 0)
			this.InvokeAfter(Stop, secondsToAutoStop);
	}

	[AdvancedInspector.Inspect]
	public void Stop()
	{
#if UNITY_EDITOR
		if (!Application.isPlaying)
			return;
#endif
		if (!Playing && !AllowMultiPlay)
			return;

		Playing = false;
		OnStop.Invoke();
	}
}
