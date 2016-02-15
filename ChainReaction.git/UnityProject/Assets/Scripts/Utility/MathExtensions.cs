using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class MathExtensions 
{
	//Returns an angle between 0 and 360 clamped based on min and max angles
	public static float ClampAngle(this float angle, float min, float max)
	{
		angle = LimitAngle(angle);
		min = LimitAngle(min);
		max = LimitAngle(max);

		if(min <= max)
			return Mathf.Clamp(angle, min, max);
		else
		{
			if(angle > max && angle < min) 			//the angle is outside of the range
			{
				//return the closer of the two
				float maxDiff = angle - max;
				float minDiff = min - angle;
				if(maxDiff < minDiff)
					return max;
				else
					return min;
			}
			else 									//the angle is inside the range
				return angle;
		}
	}

	/// <returns>the difference between 2 angles in degrees.</returns>
	/// <param name="from">the angle to start from.</param>
	/// <param name="to">the angle to end at.</param>
	public static float AngleDiff(float from, float to)
	{
		if (Mathf.Approximately(from, to))
			return 0f;

		from = LimitAngle(from);
		to = LimitAngle(to);

		if (Mathf.Approximately(from, to)) 		//are the angles a full circle away from each other?
			return 360f;
		else if (from < to)
			return to - from;
		else
			return (360 - from) + to;
	}

	public static float ToRadians(this float degrees)
	{
		return degrees * Mathf.Deg2Rad;
	}

	public static float ToDegrees(this float radians)
	{
		return radians * Mathf.Rad2Deg;
	}

	public static float Normalize(this float val, float start, float end)
	{
		float width = end - start;
		float offsetValue = val - start;		//value relative to 0
		return (offsetValue - (Mathf.Floor(offsetValue / width) * width)) + start;
	}

	public static int Normalize(this int val, int start, int end)
	{
		int width = end - start;
		int offsetValue = val - start;			//value relative to 0
		return (offsetValue - ((offsetValue / width) * width)) + start;
	}

	///<returns>angle normalized between 0 and 360</returns>
	public static float LimitAngle(this float angle)
	{
		return Normalize(angle, 0f, 360f);
	}

	public static Vector3 GetClosestVertex(MeshFilter mf, Vector3 point)
	{
		Mesh mesh = mf.mesh;
		float minDistanceSqr = Mathf.Infinity;
		Vector3 nearestVertex = Vector3.zero;

		foreach(Vector3 vertex in mesh.vertices)
		{
			Vector3 diff = point - mf.transform.TransformPoint(vertex);
			float distSqr = diff.sqrMagnitude;
			if(distSqr < minDistanceSqr)
			{
				minDistanceSqr = distSqr;
				nearestVertex = mf.transform.TransformPoint(vertex);
			}
		}

		return nearestVertex;
	}

	public static List<Vector3> UniformPointsOnSphere(float N, float scale = 1f) 
	{
		List<Vector3> points = new List<Vector3>();
		float i = Mathf.PI * (3 - Mathf.Sqrt(5));
		float o = 2 / N;
		for(int k = 0; k < N; k++) {
			float y = k * o - 1 + (o / 2);
			float r = Mathf.Sqrt(1 - y*y);
			float phi = k * i;
			points.Add(new Vector3(Mathf.Cos(phi)*r, y, Mathf.Sin(phi)*r) * scale);
		}
		return points;
	}

	/// <summary>
	/// Returns a random direction in a cone. a spread of 0 is straight, 0.5 is 180*
	/// </summary>
	/// <param name="spread"></param>
	/// <param name="forward">must be unit</param>
	/// <returns></returns>
	public static Vector3 RandomDirection(float spread, Vector3 forward)
	{
		return Vector3.Slerp(forward, Random.onUnitSphere, spread);
	}

	public static bool Approximately(this Vector3 lhs, Vector3 rhs)
	{
		return Mathf.Approximately(lhs.x, rhs.x) && Mathf.Approximately(lhs.y, rhs.y) && Mathf.Approximately(lhs.z, rhs.z);
	}

	public static Vector3 RotatePointAroundPivot(this Vector3 point, Vector3 pivot, Vector3 angles)
	{
		Vector3 dir = point - pivot; // get point direction relative to pivot
		dir = Quaternion.Euler(angles) * dir; // rotate it
		point = dir + pivot; // calculate rotated point
		return point;
	}

	public static Vector2[,] Generate2DGrid(uint xCount, uint yCount, Vector2 padding)
	{
		Vector2[,] grid = new Vector2[xCount, yCount];

		for (int x = 0; x < xCount; x++)
		{
			for (int y = 0; y < yCount; y++)
			{
				grid[x, y] = new Vector2(x * padding.x, y * padding.y);
			}
		}

		return grid;
	}

	public static float FlipOver(this float t, float around)
	{
		return around + (around - t);
	}

	public static Vector3 FlipMagnitudeOver(this Vector3 v, float aroundMagnitude)
	{
		float magnitude = v.magnitude;
		magnitude = magnitude.FlipOver(aroundMagnitude);
		return v.normalized * magnitude;
	}

	public static Vector3 WithX(this Vector3 vec, float x) { return new Vector3(x, vec.y, vec.z); }
	public static Vector3 WithY(this Vector3 vec, float y) { return new Vector3(vec.x, y, vec.z); }
	public static Vector3 WithZ(this Vector3 vec, float z) { return new Vector3(vec.x, vec.y, z); }

	//Shorthand for converting Quaternions to their direction vector
	public static Vector3 Right(this Quaternion q) { return q.RotatePoint(Vector3.right); }
	public static Vector3 Up(this Quaternion q) { return q.RotatePoint(Vector3.up); }
	public static Vector3 Forward(this Quaternion q) { return q.RotatePoint(Vector3.forward); }
	public static Vector3 Back(this Quaternion q) { return q.RotatePoint(Vector3.back); }
	public static Vector3 Left(this Quaternion q) { return q.RotatePoint(Vector3.left); }
	public static Vector3 Down(this Quaternion q) { return q.RotatePoint(Vector3.down); }
	public static Vector3 RotatePoint(this Quaternion q, Vector3 point) { return q * point; }

	public static bool WithinRange<T>(this T f, T min, T max) where T : System.IComparable
	{
		return f.CompareTo(min) >= 0 && f.CompareTo(max) <= 0;
	}

	//Clamps a value between a minimum and maximum
	public static T ClampBetween<T>(this T val, T min, T max) where T : System.IComparable
	{
		if (val.CompareTo(min) < 0)
			return min;
		else if (val.CompareTo(max) > 0)
			return max;
		else
			return val;
	}

	public enum QuaternionInterpolationType { Lerp, Slerp, RotateTowards, LerpUnclamped, SlerpUnclamped }

	public static Quaternion Interpolate(QuaternionInterpolationType interpolateType, Quaternion from, Quaternion to, float t)
	{
		switch (interpolateType)
		{
			case QuaternionInterpolationType.Lerp:
				return Quaternion.Lerp(from, to, t);
			case QuaternionInterpolationType.Slerp:
				return Quaternion.Slerp(from, to, t);
			case QuaternionInterpolationType.RotateTowards:
				return Quaternion.RotateTowards(from, to, t);
			case QuaternionInterpolationType.LerpUnclamped:
				return Quaternion.LerpUnclamped(from, to, t);
			case QuaternionInterpolationType.SlerpUnclamped:
				return Quaternion.SlerpUnclamped(from, to, t);
		}
		return from;
	}

	public enum VectorInterpolationType { Lerp, Slerp, MoveTowards, LerpUnclamped, SlerpUnclamped }

	public static Vector3 Interpolate(VectorInterpolationType interpolateType, Vector3 from, Vector3 to, float t)
	{
		switch (interpolateType)
		{
			case VectorInterpolationType.Lerp:
				return Vector3.Lerp(from, to, t);
			case VectorInterpolationType.Slerp:
				return Vector3.Slerp(from, to, t);
			case VectorInterpolationType.MoveTowards:
				return Vector3.MoveTowards(from, to, t);
			case VectorInterpolationType.LerpUnclamped:
				return Vector3.LerpUnclamped(from, to, t);
			case VectorInterpolationType.SlerpUnclamped:
				return Vector3.SlerpUnclamped(from, to, t);
		}
		return from;
	}

	public enum FloatInterpolationType { Lerp, LerpUnclamped, MoveTowards, SmoothStep }

	public static float Interpolate(FloatInterpolationType interpolateType, float from, float to, float t)
	{
		switch (interpolateType)
		{
			case FloatInterpolationType.Lerp:
				return Mathf.Lerp(from, to, t);
			case FloatInterpolationType.LerpUnclamped:
				return Mathf.LerpUnclamped(from, to, t);
			case FloatInterpolationType.MoveTowards:
				return Mathf.MoveTowards(from, to, t);
			case FloatInterpolationType.SmoothStep:
				return Mathf.SmoothStep(from, to, t);
		}
		return from;
	}

	public enum AngleInterpolationType { Lerp, MoveTowards }

	public static float Interpolate(AngleInterpolationType interpolateType, float from, float to, float t)
	{
		switch (interpolateType)
		{
			case AngleInterpolationType.Lerp:
				return Mathf.LerpAngle(from, to, t);
			case AngleInterpolationType.MoveTowards:
				return Mathf.MoveTowardsAngle(from, to, t);
		}
		return from;
	}
}
