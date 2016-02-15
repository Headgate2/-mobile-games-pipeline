using UnityEngine;
using System.Collections;

public class TransformLocker : MonoBehaviour
{
	//TODO add position locking as well

#pragma warning disable 649
	[SerializeField]
	bool localRotation, setXRot, setYRot, setZRot;

	[SerializeField]
	Vector3 rotation;

	void Update()
	{
		if (localRotation)
			transform.localEulerAngles = ApplyRotation(transform.localEulerAngles);
		else
			transform.eulerAngles = ApplyRotation(transform.eulerAngles);
	}

	Vector3 ApplyRotation(Vector3 euler)
	{
		if (setXRot)
			euler.x = rotation.x;
		if (setYRot)
			euler.y = rotation.y;
		if (setZRot)
			euler.z = rotation.z;
		return euler;
	}
}
