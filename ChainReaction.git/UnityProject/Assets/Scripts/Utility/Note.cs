using UnityEngine;
using System.Collections;

public class Note : MonoBehaviour 
{
#if UNITY_EDITOR
	public bool expand = false;
	public string text = "Type your note here.";
#endif
}
