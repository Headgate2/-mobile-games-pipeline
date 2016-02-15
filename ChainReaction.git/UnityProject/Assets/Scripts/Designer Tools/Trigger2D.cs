using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Trigger2D class
/// 
/// A trigger is considered ACTIVE when there is at least one object inside of it.
/// 
/// Contains a list of objects inside the trigger, and if the trigger is active or not.
/// The user is responsible for setting up proper layering for collisions with this object.
/// </summary>
[AddComponentMenu("Event/Event Trigger2D")]
[RequireComponent( typeof(Collider2D) )]
public class Trigger2D : TriggerBase<Collider2D, Trigger2D.ColliderEvent>
{
	[Serializable] public class ColliderEvent : UnityEvent<Collider2D> { }
	
	void Awake()
	{
		ColliderComponent.isTrigger = true;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		Enter(other);
	}

	void OnTriggerExit2D(Collider2D other)
	{
		Exit(other);
	}
}
