using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(INButton))]
public class FixScaleButton : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		INButton myScript = (INButton)target;
		myScript.adjustPosition ();
		myScript.adjustScale ();
	}
}


