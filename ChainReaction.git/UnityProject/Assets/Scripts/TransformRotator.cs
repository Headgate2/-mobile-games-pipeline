using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

public class TransformRotator : MonoBehaviour 
{
	public enum Rotation { Pitch, Roll, Yaw }

	[SerializeField, FormerlySerializedAs("xTransform")]
	private Transform yawTransform;
	public Transform YawTransform { get { return yawTransform != null ? yawTransform : transform; } }

	[SerializeField, FormerlySerializedAs("yTransform")]
	private Transform pitchTransform;
	public Transform PitchTransform { get { return pitchTransform != null ? pitchTransform : transform; } }

	[SerializeField, FormerlySerializedAs("verticalOffset"), Tooltip("How many degrees from vertically up/down are we allowed to look?")]
	private float pitchOffset = 3f;
    public float PitchOffset { get { return pitchOffset; } set { pitchOffset = value; } }

	[SerializeField]
	private Transform rollTransform;
	public Transform RollTransform { get { return rollTransform != null ? rollTransform : transform; } }

	public float Yaw
	{
		get { return YawTransform.localEulerAngles.y; }
		set { YawTransform.RotateAround(YawTransform.position, YawTransform.up, value - Yaw); }
	}

	public float Pitch
	{
		get { return PitchTransform.localEulerAngles.x; }
		set { SetTransformVerticalAngle(PitchTransform, value, pitchOffset); }
	}

	public float Roll
	{
		get { return RollTransform.localEulerAngles.z; }
		set { RollTransform.RotateAround(RollTransform.position, RollTransform.forward, value - Roll); }
	}

	public Vector3 RepresentativeEuler
	{
		get { return new Vector3(Pitch, Yaw, Roll); }
		set
		{
			Pitch = value.x;
			Yaw = value.y;
			Roll = value.z;
		}
	}

	//Returns the rotation this rotator represents, as a combination of the sub-transforms' rotations
	public Quaternion RepresentativeRotation { get { return Quaternion.Euler(RepresentativeEuler); } }

	public void Reset(bool yaw = true, bool pitch = true, bool roll = true)
	{
		if(yaw)
			Yaw = 0f;
		if(pitch)
			Pitch = 0f;
		if (roll)
			Roll = 0f;
	}

	public Transform GetTransform(Rotation rot)
	{
		switch(rot)
		{
			case Rotation.Pitch: return PitchTransform;
			case Rotation.Yaw: return YawTransform;
			case Rotation.Roll: return RollTransform;
			default: return null;
		}
	}

	public float Get(Rotation rot)
	{
		switch (rot)
		{
			case Rotation.Pitch: return Pitch;
			case Rotation.Yaw: return Yaw;
			case Rotation.Roll: return Roll;
			default: return float.NaN;
		}
	}

	public bool Set(Rotation rot, float value)
	{
		switch (rot)
		{
			case Rotation.Pitch:
				Pitch = value;
				return true;
			case Rotation.Yaw:
				Yaw = value;
				return true;
			case Rotation.Roll:
				Roll = value;
				return true;
			default:
				return false;
        }
	}

	public Vector3 GetRotationPlaneNormal(Rotation rot)
	{
		switch (rot)
		{
			case Rotation.Pitch: return PitchTransform.right;
			case Rotation.Yaw: return YawTransform.up;
			case Rotation.Roll: return RollTransform.forward;
			default: return Vector3.zero;
		}
	}

	public void LookAt(Vector3 position, Rotation rotationType, Vector3? up = null)
	{
		GetTransform(rotationType).rotation = GetFacingDirectionOnPlane(rotationType, position, up);
    }

	public void LookAt(Vector3 position, Rotation priority1, Rotation priority2, Rotation? priority3, Vector3? up = null)
	{
		if (priority1 == priority2 || priority1 == priority3 || priority2 == priority3)
		{
			Debug.LogError("Priorities must be unique.");
			return;
		}
		LookAt(position, priority1, up);
		LookAt(position, priority2, up);
		if(priority3 != null)
			LookAt(position, priority3.Value, up);
	}

	/// <summary>
	/// Turns the selected Rotation toward a given point using the provided interpolation settings
	/// </summary>
	/// <param name="position">end desired position to LookAt</param>
	/// <param name="rotationType">Rotation (Yaw/Pitch/Roll) which will turn</param>
	/// <param name="interpolateType">Rotation interpolation (Lerp/Slerp/RotateTowards/etc.) type used</param>
	/// <param name="interpolateRate">Rate of the interpolation (desired ranges based on selected interpolation type)</param>
	/// <param name="up">Up direction to maintain (null will try to maintain the transform's current up direction)</param>
	public void TurnToward(Vector3 position, Rotation rotationType, MathExtensions.QuaternionInterpolationType interpolateType, float interpolateRate, Vector3? up = null)
	{
		Transform trans = GetTransform(rotationType);
		Quaternion desiredFacing = GetFacingDirectionOnPlane(rotationType, position, up);
		trans.rotation = MathExtensions.Interpolate(interpolateType, trans.rotation, desiredFacing, interpolateRate);
	}

	public void TurnToward(Vector3 position, Rotation priority1, Rotation priority2, Rotation? priority3, MathExtensions.QuaternionInterpolationType interpolateType, float interpolateRate, Vector3? up = null)
	{
		if (priority1 == priority2 || priority1 == priority3 || priority2 == priority3)
		{
			Debug.LogError("Priorities must be unique.");
			return;
		}
		TurnToward(position, priority1, interpolateType, interpolateRate, up);
		TurnToward(position, priority2, interpolateType, interpolateRate, up);
		if (priority3 != null)
			TurnToward(position, priority3.Value, interpolateType, interpolateRate, up);
	}

	/// <summary>
	/// Finds the desired facing direction of one of the rotations toward a point (on their respective rotaion plane)
	/// </summary>
	/// <param name="position">position to look at</param>
	/// <param name="rotationType">Rotation (Yaw/Pitch/Roll) desired to aim</param>
	/// <param name="up">Up direction to maintain (null will try to maintain the transform's current up direction)</param>
	/// <returns></returns>
	Quaternion GetFacingDirectionOnPlane(Rotation rotationType, Vector3 position, Vector3? up = null)
	{
		Transform t = GetTransform(rotationType);
		Vector3 planeNormal = GetRotationPlaneNormal(rotationType);
		Vector3 projectedDirectionToward = Vector3.ProjectOnPlane(position - t.position, planeNormal);
		return Quaternion.LookRotation(projectedDirectionToward, up ?? t.up);
	}

	static void SetTransformVerticalAngle(Transform trans, float newAngle, float maxLookOffset = 3f)
	{
		Vector3 localRotation = trans.localEulerAngles;
		localRotation.x = MathExtensions.ClampAngle(newAngle, -90f + maxLookOffset, 90f - maxLookOffset);
		trans.localEulerAngles = localRotation;
	}
}
