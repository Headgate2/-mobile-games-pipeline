using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Trigger3D class
/// 
/// A trigger is considered ACTIVE when there is at least one object inside of it.
/// 
/// Contains a list of objects inside the trigger, and if the trigger is active or not.
/// The user is responsible for setting up proper layering for collisions with this object.
/// </summary>
[AddComponentMenu("Event/Event Trigger3D")]
[RequireComponent( typeof(Collider) )]
public class Trigger3D : TriggerBase<Collider, Trigger3D.ColliderEvent>
{
	[Serializable] public class ColliderEvent : UnityEvent<Collider> { }

	void Awake()
	{
		ColliderComponent.isTrigger = true;
	}

	void OnTriggerEnter(Collider other)
	{
		Enter(other);
	}

	void OnTriggerExit(Collider other)
	{
		Exit(other);
	}
}
