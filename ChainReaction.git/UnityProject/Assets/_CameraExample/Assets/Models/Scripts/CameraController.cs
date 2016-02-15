using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
	
	public GameObject Camera1;
	public GameObject Camera2;
	
	private bool mMainCamera;

	private float mDragThreshold = 10;
	private Vector3 mMouseDownPosition;
	
	// Use this for initialization
	void Start ()
	{
		Camera1.SetActive(false);
		mMainCamera = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnMouseDown() {
		mMouseDownPosition = Input.mousePosition;
	}
	
	void OnMouseUp() {
		Vector3 moved = (Input.mousePosition - mMouseDownPosition);
		if (moved.magnitude < mDragThreshold) 
		{
			if (mMainCamera) 
			{
				Camera1.SetActive(false);
				Camera2.SetActive(true);
				mMainCamera = false;
			}
			else 
			{
				Camera1.SetActive(true);
				Camera2.SetActive(false);
				mMainCamera = true;
			}
		}
	}
}
