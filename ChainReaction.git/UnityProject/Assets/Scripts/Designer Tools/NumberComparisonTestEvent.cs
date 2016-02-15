using UnityEngine;
using System.Collections;
using UnityEngine.Events;

[AddComponentMenu("Event/Number Comparison Test Event")]
public class NumberComparisonTestEvent : MonoBehaviour 
{
	[SerializeField, Tooltip("If the test does not accept a second parameter, this value will be used for the comparison.")]
	private float toCompareTo;
	public float ToCompareTo { get { return toCompareTo; } set { toCompareTo = value; } }

	[System.Serializable] public class ComparisonDifferenceEvent : UnityEvent<float> { }

	[SerializeField, Tooltip("Called when a compare happens, and the value is less than the compared value")]
	private ComparisonDifferenceEvent onLessThan;
	public ComparisonDifferenceEvent OnLessThan { get { return onLessThan; } }

	[SerializeField, Tooltip("Called when a compare happens, and the value is (approximately) equal to the compared value")]
	private ComparisonDifferenceEvent onEqual;
	public ComparisonDifferenceEvent OnEqual { get { return onEqual; } }

	[SerializeField, Tooltip("Called when a compare happens, and the value is greater than the compared value")]
	private ComparisonDifferenceEvent onGreaterThan;
	public ComparisonDifferenceEvent OnGreaterThan { get { return onGreaterThan; } }

	public void Compare(int value)
	{
		Compare((float)value, ToCompareTo);
	}

	public void Compare(int val1, int val2)
	{
		Compare((float)val1, (float)val2);
	}

	/// <summary>
	/// Tests the given value against the ToCompareTo value and returns the difference
	/// </summary>
	/// <param name="toCompare"></param>
	/// <returns></returns>
	public void Compare(float value)
	{
		Compare(value, ToCompareTo);
    }

	/// <summary>
	/// Tests the given values for a difference and fires the appropriate event for that.
	/// 
	/// IGNORES THE ToCompareTo VALUE
	/// </summary>
	/// <param name="val1"></param>
	/// <param name="val2"></param>
	/// <returns>difference between the values</returns>
	public void Compare(float val1, float val2)
	{
		float diff = val1 - val2;

		if (Mathf.Approximately(diff, 0f))
			OnEqual.Invoke(diff);
		else if (diff < 0f)
			OnLessThan.Invoke(diff);
		else
			OnGreaterThan.Invoke(diff);
	}
}
