using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using AdvancedInspector;

[AddComponentMenu("Event/Event Raycast Aimer")]
public class RaycastAimer : MonoBehaviour
{
	[Inspect]
	[SerializeField, Tooltip("What layers will this aimer aim at?  (Unchecked items are aimed through and ignored)")]
	LayerMask aimMask = int.MaxValue;
	public LayerMask AimMask { get { return aimMask; } }

	[Inspect]
	[SerializeField, Tooltip("How far does this aimer check for things to hit?")]
	float range = 1000;
	public float Range { get { return range; } set { range = value; } }

	private RaycastHit lastHit;
	//Raycast Hit information about the last aim update
	public RaycastHit LastHit { get { return lastHit; } }

	///Did we hit an object on the last aim update?
	public bool ValidLastHit { get { return LastHit.collider != null; } }

	[System.Serializable] public class Vector3Event : UnityEvent<Vector3> { }

	[Inspect]
	[SerializeField, Tooltip("Called whenever the aim is updated")]
	Vector3Event onNewAim;
	public Vector3Event OnNewAim { get { return onNewAim; } }

	[Inspect]
	[SerializeField, Tooltip("Called when the aim hits an object")]
	Vector3Event onAimHitObject;
	public Vector3Event OnAimHitObject { get { return onAimHitObject; } }

	[Inspect]
	[SerializeField, Tooltip("Rigidbodies whose colliders are ignored for the purposes of this aimer.")]
	List<Rigidbody> ignoreCollisions;
	public List<Rigidbody> IgnoreCollisions { get { return ignoreCollisions; } }

	/// <summary>
	/// If the aimer hit an object, this will be the position it hit, otherwise it will be the point at the maximum range of the aimer.
	/// </summary>
	public Vector3 ForwardHitPoint {
		get { return ValidLastHit ? LastHit.point : transform.position + transform.forward * Range; }
	}

	void Start() { }		//this exists so the designers can see the enable/disable checkbox

	public void UpdateAim()
	{
		if (!enabled)
			return;

		if (IgnoreCollisions.Count > 0)
		{
			RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, Range, AimMask);
			lastHit = new RaycastHit();

			if (hits.Length > 0)
			{
				foreach (RaycastHit hit in hits)
				{
					if (!IgnoreCollisions.Contains(hit.collider.attachedRigidbody) && (lastHit.collider == null || lastHit.distance > hit.distance))
						lastHit = hit;
				}
			}
		}
		else
			Physics.Raycast(transform.position, transform.forward, out lastHit, Range, AimMask);
		
		if (ValidLastHit)
			OnAimHitObject.Invoke(ForwardHitPoint);
		OnNewAim.Invoke(ForwardHitPoint);
    }
}