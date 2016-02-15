using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using AdvancedInspector;

public class ForceApplier2D : MonoBehaviour
{
	public enum DirectionType { Forward,  }
	[SerializeField]
	private DirectionType direction;
	public DirectionType Direction { get { return direction; } set { direction = value; } }

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

	[System.Serializable] public class Rigidbody2DEvent : UnityEvent<Rigidbody2D> { }

	[SerializeField, Tooltip("Called whenever we apply a force to a rigidbody.")]
	Rigidbody2DEvent onForceApplied;
	public Rigidbody2DEvent OnForceApplied { get { return onForceApplied; } }

	void Start() { }	//include this function so the designers can see the checkbox to disable this component

	public void ApplyForceToObject(Object o)
	{
		if (!enabled || o == null)
			return;

		GameObject go = MonoExtensions.GetGameObject(o);
		if (go != null)
		{
			Collider2D c = go.GetComponent<Collider2D>();
			if (c != null)
				ApplyForceToCollider(c);
			else
				ApplyForceToRigidbody(go.GetComponentInParent<Rigidbody2D>());
		}
	}

	public void ApplyForceToCollider(Collider2D c)
	{
		if (!enabled || c == null)
			return;
		
		ApplyForceToRigidbody(c.attachedRigidbody);		
	}

	public void ApplyForceToRigidbody(Rigidbody2D rb)
	{
		if (!enabled || rb == null)
			return;

		if (ForceApplierType == Type.Single)
			ApplyForce(rb);
		else if (ForceApplierType == Type.OverTime)
			StartCoroutine(ApplyContinuous(rb));

		OnForceApplied.Invoke(rb);
	}

	IEnumerator ApplyContinuous(Rigidbody2D rb)
	{
		uint numberFixedUpdates = (uint)(SecondsToApplyOver / Time.fixedDeltaTime);
		for (int x = 0; x < numberFixedUpdates; x++)
		{
			yield return new WaitForFixedUpdate();
			ApplyForce(rb, overTimeMultiplier.Evaluate(x * Time.fixedDeltaTime / SecondsToApplyOver));
		}
	}

	private void ApplyForce(Rigidbody2D rb, float forceMultiplier = 1f)
	{
		if (Direction == DirectionType.Forward)
			rb.AddForce2D(Force * forceMultiplier, ForceMode, Space.World);
	}
}