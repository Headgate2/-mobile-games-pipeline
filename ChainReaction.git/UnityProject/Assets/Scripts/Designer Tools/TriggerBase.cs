using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;

public class TriggerBase<ColliderType, ColliderEventType> : MonoBehaviour where ColliderType : Component where ColliderEventType : UnityEvent<ColliderType>
{
	protected bool IsRunning() { return Application.isPlaying; }

	[AdvancedInspector.Inspect("IsRunning")]
	///This trigger is considered active when another object is inside of it.  It is inactive if no objects are inside of it
	public bool Active { get { return inside.Count > 0; } }

	private ColliderType col;
	/// The Collider for this trigger.
	public ColliderType ColliderComponent { get { return col ?? (col = GetComponent<ColliderType>()); } }

	///all the objects inside this trigger
	private List<ColliderType> inside = new List<ColliderType>();

	[AdvancedInspector.Inspect("IsRunning")]
	///A list of all the colliders currently inside the trigger
	public IEnumerable<ColliderType> Inside { get { return inside; } }

	//FIXME Someday Unity will handle generics correctly in the editor.  When they do, remove the ColliderEventType generic and replace it with this line
	//[Serializable] public class ColliderEvent : UnityEvent<ColliderType> { }

	[SerializeField, Tooltip("called when this trigger is activated by the first collider entering it")]
	protected ColliderEventType onActivate;
	public ColliderEventType OnActivate { get { return onActivate; } }

	[SerializeField, Tooltip("called when this trigger is deactivated by the last collider leaving it")]
	protected ColliderEventType onDeactivate;
	public ColliderEventType OnDeactivate { get { return onDeactivate; } }

	[SerializeField, Tooltip("called when a collider enters this trigger")]
	protected ColliderEventType onEnter;
	public ColliderEventType OnEnter { get { return onEnter; } }

	[SerializeField, Tooltip("called when a collider exists this trigger")]
	protected ColliderEventType onExit;
	public ColliderEventType OnExit { get { return onExit; } }

	protected void Enter(ColliderType collider)
	{
		bool wasActive = Active;

		inside.Add(collider);
		OnEnter.Invoke(collider);

		if (!wasActive)
			OnActivate.Invoke(collider);
	}

	protected void Exit(ColliderType collider)
	{
		bool wasActive = Active;

		inside.Remove(collider);

		OnExit.Invoke(collider);

		if (wasActive && !Active)
			OnDeactivate.Invoke(collider);
	}
}
