using UnityEngine;
using UnityEditor;
using System.Collections;

public static class EditorHelper
{
	//returns a position in front of the editor scene view camera
	public static Vector3 GetSceneViewLookPosition()
	{
		Transform sceneViewCam = SceneView.lastActiveSceneView.camera.transform;
		RaycastHit hitInfo;
		if( Physics.Raycast(sceneViewCam.position, sceneViewCam.forward, out hitInfo) )
		{
			//return a point almost all of the way along this ray, but not inside the object we hit
			return sceneViewCam.position + (hitInfo.point - sceneViewCam.position) * 0.8f;
		}
		else
			return SceneView.lastActiveSceneView.pivot;
	}

	public static GameObject SpawnGameObjectInHierarchy(string name, string prefabName)
	{
		//Always load the new prefab every time, in case the user modified the prefab between creating these objects
		GameObject prefab = Resources.Load<GameObject>(prefabName);
		if(prefab == null)
		{
			Debug.LogError("Cannot create " + name + " object.  Please check the " + prefabName + " prefab exists, and is inside the Resources folder.");
			return null;
		}

		//Create the copy from the prefab and 
		GameObject obj = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
		obj.name = name;
		obj.transform.SetAsFirstSibling();
		obj.transform.position = EditorHelper.GetSceneViewLookPosition();

		return obj;
	}
}
