#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections;

public class EditorExtensions : MonoBehaviour 
{
	[MenuItem ("Tools/Revert to Prefab %r")]
	public static void Revert() 
	{
		GameObject[] selection = Selection.gameObjects;
		
		for (int i = 0; i < selection.Length; i++) 
		{
			PrefabUtility.RevertPrefabInstance(selection[i]);
		}
	}
}

#endif