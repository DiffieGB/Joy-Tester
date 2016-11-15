using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(VirtualJoystick))]
public class FixScalePad : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		
		VirtualJoystick myScript = (VirtualJoystick)target;
		myScript.adjustPosition ();
		myScript.adjustScale ();
	}
}


