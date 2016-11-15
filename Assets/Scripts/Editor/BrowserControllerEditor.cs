using UnityEngine;
using System.Collections;
using UnityEditor;
using ZenFulcrum.EmbeddedBrowser;


[CustomEditor(typeof(BrowserController))]
[CanEditMultipleObjects]
public class BrowserControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        BrowserController bc = (BrowserController)target;

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("JS", GUILayout.Width(20));
        bc.customJS = EditorGUILayout.TextArea(bc.customJS);
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Run JS Again"))
        {
            bc.GetComponent<Browser>().EvalJS(bc.customJS);
        }
    }
}
