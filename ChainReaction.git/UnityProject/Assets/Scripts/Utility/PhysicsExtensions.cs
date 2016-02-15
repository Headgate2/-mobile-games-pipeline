using UnityEngine;
using System.Collections;

public static class PhysicsExtensions
{
	/// <summary>
	/// Returns an array containing the 2 points that represent this capsule
	/// </summary>
	/// <param name="capsule"></param>
	/// <returns></returns>
	public static Vector3[] Points(this CapsuleCollider capsule)
	{
		Vector3[] points = new Vector3[2];

		Vector3 alongCapsuleDirection = Vector3.zero;
		if (capsule.direction == 0)         //x
			alongCapsuleDirection = capsule.transform.right;
		else if (capsule.direction == 1)    //y
			alongCapsuleDirection = capsule.transform.up;
		else if (capsule.direction == 2)    //z
			alongCapsuleDirection = capsule.transform.forward;

		points[0] = capsule.transform.position + capsule.center + -alongCapsuleDirection * capsule.height * 0.5f;
		points[1] = points[0] + alongCapsuleDirection * capsule.height;
		return points;
	}

	public static bool Cast(CapsuleCollider capsule, Vector3 direction, out RaycastHit hitInfo, float maxDistance, LayerMask mask)
	{
		Vector3[] points = capsule.Points();
		return Physics.CapsuleCast(points[0], points[1], capsule.radius, direction, out hitInfo, maxDistance, mask);
	}

	public static RaycastHit[] CastAll(CapsuleCollider capsule, Vector3 direction, float maxDistance, LayerMask mask)
	{
		Vector3[] points = capsule.Points();
		return Physics.CapsuleCastAll(points[0], points[1], capsule.radius, direction, maxDistance, mask);
	}

	public static bool Cast(SphereCollider sphere, Vector3 direction, out RaycastHit hitInfo, float maxDistance, LayerMask mask)
	{
		return Physics.SphereCast(sphere.transform.position + sphere.center, sphere.radius, direction, out hitInfo, mask);
	}

	public static RaycastHit[] CastAll(SphereCollider sphere, Vector3 direction, float maxDistance, LayerMask mask)
	{
		return Physics.SphereCastAll(sphere.transform.position + sphere.center, sphere.radius, direction, maxDistance, mask);
	}

	/// <summary>
	/// Function for applying a specific type of force to a Rigidbody2D (since the default functionality is incomplete)
	/// </summary>
	/// <param name="rb2d">Rigidbody2D to apply the force to</param>
	/// <param name="force">The amount of force to apply</param>
	/// <param name="mode">What type of force to apply</param>
	/// <param name="relativeTo">Should the force be applied in the rigidbody's local space, or world space?</param>
	public static void AddForce2D(this Rigidbody2D rb2d, Vector2 force, ForceMode mode = ForceMode.Force, Space relativeTo = Space.World)
	{
		ForceMode2D mode2D = ForceMode2D.Force;
		Vector2 forceApplied = force;
		switch (mode)
		{
			case ForceMode.Impulse:
				mode2D = ForceMode2D.Impulse;
				break;
			case ForceMode.Acceleration:
				forceApplied *= rb2d.mass;
				break;
			case ForceMode.VelocityChange:
				forceApplied = force * rb2d.mass / Time.fixedDeltaTime;
				break;
			case ForceMode.Force:
				//nothing special
				break;
		}

		if (relativeTo == Space.Self)
			rb2d.AddRelativeForce(forceApplied, mode2D);
		else if (relativeTo == Space.World)
			rb2d.AddForce(forceApplied, mode2D);
	}
}
