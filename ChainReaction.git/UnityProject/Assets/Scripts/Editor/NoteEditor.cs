using UnityEditor;
using UnityEngine;
using System;
using System.Linq;

[CustomEditor(typeof(Note))]
public class NoteEditor : Editor
{
	private Note note;
	
	private Vector2 scrollPos;
	
	private void Init()
	{
		note = base.target as Note;        
	}
	
	public override void OnInspectorGUI()
	{
		Init();
		note.expand = GUILayout.Toggle(note.expand, "Expand text");
		float height = 100f;
		if(note.expand)
		{
			float unitsPerLine = 28f;
			height = note.text.Count(c => c == '\n') * unitsPerLine;
		}

		scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(height));
		GUIStyle myStyle = new GUIStyle();
		myStyle.wordWrap = true;
		myStyle.stretchWidth = false;
		myStyle.normal.textColor = Color.gray;
		note.text = EditorGUILayout.TextArea(note.text, myStyle, GUILayout.Width(200.0F), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
		GUILayout.EndScrollView();
		Repaint();       
	}          
}