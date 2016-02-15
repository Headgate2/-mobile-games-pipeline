using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using AdvancedInspector;

public class ForceApplier3D : MonoBehaviour
{
	public enum DirectionType { Forward, Explosive }
	[SerializeField]
	private DirectionType direction;
	public DirectionType Direction { get { return direction; } set { direction = value; } }
	private bool IsExplosive() { return Direction == DirectionType.Explosive; }

	public enum Type { Single, OverTime }
	[SerializeField]
	Type forceApplierType;
	public Type ForceApplierType { get { return forceApplierType; } set { forceApplierType = value; } }
	private bool IsOverTime() { return ForceApplierType == Type.OverTime; }

	[SerializeField, Tooltip("Type of force to apply")]
	private ForceMode forceMode = ForceMode.Impulse;
	public ForceMode ForceMode { get { return forceMode; } set { forceMode = value; } }

	[SerializeField, Tooltip("Amount of force to apply")]
	private float power = 10f;
	public float Power { get { return power; } set { power = value; } }

	[SerializeField, Tooltip("(OPTIONAL) \nFORWARD DIRECTION: What direction should the force be applied in?\nEmpty will apply in the forward of this transform.\n\nEXPLOSIVE DIRECTION: What position should the explosion be based from?\nEmpty will apply from this transform's position.")]
	Transform forceTransform;
	public Transform ForceTransform { get { return forceTransform == null ? transform : forceTransform; } set { forceTransform = value; } }

	public Vector3 Force { get { return ForceTransform.forward * Power; } }

	[Inspect("IsOverTime")]
	[SerializeField, Tooltip("Number of seconds to apply a force over for a continuously applied force")]
	private float secondsToApplyOver;
	public float SecondsToApplyOver { get { return secondsToApplyOver; } set { secondsToApplyOver = value; } }

	[Inspect("IsOverTime")]
	[SerializeField, Tooltip("Multiplier applied to the force applied to the rigidbody over the course of Seconds To Apply Over.")]
	private AnimationCurve overTimeMultiplier = AnimationCurve.Linear(0f, 1f, 1f, 1f);
	public AnimationCurve OverTimeMultiplier { get { return overTimeMultiplier; } }

	[Inspect("IsExplosive")]
	[SerializeField, Tooltip("The explosion is modelled as a sphere with a certain centre position and radius in world space; normally, anything outside the sphere is not affected by the explosion and the force decreases in proportion to distance from the centre. However, if a value of zero is passed for the radius then the full force will be applied regardless of how far the centre is from the rigidbody")]
	private float explosionRadius;
	public float ExplosionRadius { get { return explosionRadius; } set { explosionRadius = value; } }

	[Inspect("IsExplosive")]
	[SerializeField, Tooltip("non-zero value for the upwardsModifier parameter, the direction will be modified by subtracting that value from the Y component of the centre point. For example, if you pass a value of 2.0 for upwardsModifier, the explosion will appear to be centred 2.0 units below its actual position for purposes of calculating the force direction")]
	private float explosionUpwardsModifier;
	public float ExplosionUpwardsModifier { get { return explosionUpwardsModifier; } set { explosionUpwardsModifier = value; } }

	[System.Serializable] public class RigidbodyEvent : UnityEvent<Rigidbody> { }

	[SerializeField, Tooltip("Called whenever we apply a force to a rigidbody.")]
	RigidbodyEvent onForceApplied;
	public RigidbodyEvent OnForceApplied { get { return onForceApplied; } }

	void Start() { }	//include this function so the designers can see the checkbox to disable this component

	public void ApplyForceToObject(Object o)
	{
		if (!enabled || o == null)
			return;

		GameObject go = MonoExtensions.GetGameObject(o);
		if (go != null)
		{
			Collider c = go.GetComponent<Collider>();
			if (c != null)
				ApplyForceToCollider(c);
			else
				ApplyForceToRigidbody(go.GetComponentInParent<Rigidbody>());
		}
	}

	public void ApplyForceToCollider(Collider c)
	{
		if (!enabled || c == null)
			return;
		
		ApplyForceToRigidbody(c.attachedRigidbody);		
	}

	public void ApplyForceToRigidbody(Rigidbody rb)
	{
		if (!enabled || rb == null)
			return;

		if (ForceApplierType == Type.Single)
			ApplyForce(rb);
		else if (ForceApplierType == Type.OverTime)
			StartCoroutine(ApplyContinuous(rb));

		OnForceApplied.Invoke(rb);
	}

	IEnumerator ApplyContinuous(Rigidbody rb)
	{
		uint numberFixedUpdates = (uint)(SecondsToApplyOver / Time.fixedDeltaTime);
		for (int x = 0; x < numberFixedUpdates; x++)
		{
			yield return new WaitForFixedUpdate();
			ApplyForce(rb, overTimeMultiplier.Evaluate(x * Time.fixedDeltaTime / SecondsToApplyOver));
		}
	}

	private void ApplyForce(Rigidbody rb, float forceMultiplier = 1f)
	{
		if (Direction == DirectionType.Forward)
			rb.AddForce(Force * forceMultiplier, ForceMode);
		else
			rb.AddExplosionForce(Power * forceMultiplier, ForceTransform.position, ExplosionRadius, ExplosionUpwardsModifier, ForceMode);
	}

	void OnValidate()
	{
		ExplosionRadius = Mathf.Clamp(ExplosionRadius, 0f, float.MaxValue);
	}

	void OnDrawGizmos()
	{
		if(IsExplosive())
		{
			Color c = Color.red;
			c.a = 0.2f;
			Gizmos.DrawSphere(ForceTransform.position, ExplosionRadius);
		}
	}
}