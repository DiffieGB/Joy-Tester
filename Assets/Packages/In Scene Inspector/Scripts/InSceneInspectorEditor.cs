#if UNITY_EDITOR

using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace TP_InSceneInspector
{
    [ExecuteInEditMode]
    [CustomEditor(typeof(InSceneInspector))]
    public class InSceneInspectorEditor : Editor
    {
        public static bool useCustomInspector = true;

        //private int defaultComponentIndex = 0;
        private static List<string> componentChoices = null;


        public override void OnInspectorGUI()
        {
            if (componentChoices == null)
            {
                UpdateComponentChoices();
            }

            if (!useCustomInspector)
            {
                DrawDefaultInspector();
                EditorGUILayout.HelpBox("For more intuive editing check Use Custom Inspector", MessageType.Info);
            }
            else
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Defualt Object");
                DefaultObject = (GameObject)EditorGUILayout.ObjectField(DefaultObject, typeof(GameObject), true);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Defualt Component");
                if (DefaultObject != null)
                {
                    DefaultComponentIndex = EditorGUILayout.Popup(DefaultComponentIndex, componentChoices.ToArray());
                }
                else
                {
                    GUI.enabled = false;
                    DefaultComponentIndex = EditorGUILayout.Popup(0, new string[] { "" });
                    GUI.enabled = true;
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Min Height");
                Height = EditorGUILayout.FloatField(Height);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Hide");
                InSceneInspectorInstance.Hide = EditorGUILayout.Toggle(InSceneInspectorInstance.Hide);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Start Open");
                InSceneInspectorInstance.startOpened = EditorGUILayout.Toggle(InSceneInspectorInstance.startOpened);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Alignment");
                if (GUILayout.Button("Left"))
                {
                    InSceneInspectorInstance.AlignLeft = true;
                }
                if (GUILayout.Button("Right"))
                {
                    InSceneInspectorInstance.AlignRight = true;
                }
                EditorGUILayout.EndHorizontal();

                GUIContent runButtonText = new GUIContent("Run");
                var runButtonRect = GUILayoutUtility.GetRect(runButtonText, GUI.skin.button, GUILayout.ExpandWidth(false));
                runButtonRect.width = 150;
                runButtonRect.center = new Vector2(EditorGUIUtility.currentViewWidth / 2, runButtonRect.center.y);
                if (GUI.Button(runButtonRect, runButtonText, GUI.skin.button))
                {
                    InSceneInspectorInstance.Initialize();
                }

                GUIContent resetButtonText = new GUIContent("Reset");
                var resetButtonRect = GUILayoutUtility.GetRect(resetButtonText, GUI.skin.button, GUILayout.ExpandWidth(false));
                resetButtonRect.width = 150;
                resetButtonRect.center = new Vector2(EditorGUIUtility.currentViewWidth / 2, resetButtonRect.center.y);
                if (GUI.Button(resetButtonRect, resetButtonText, GUI.skin.button))
                {
                    InSceneInspectorInstance.Reset();
                }

                if (GUI.changed)
                {
                    EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
                }

                EditorGUILayout.HelpBox("For full editing capabilities un-check Use Custom Inspector", MessageType.Info);
            }

            useCustomInspector = EditorGUILayout.Toggle("Use Custom Inspector", useCustomInspector);
        }

        private InSceneInspector InSceneInspectorInstance
        {
            get { return (InSceneInspector)target; }
        }

        private void UpdateComponentChoices()
        {
            if (DefaultObject != null)
            {
                Component[] components = DefaultObject.GetComponents<Component>();

                componentChoices = new List<string>(components.Where(x => x != null).Select(x => x.GetType().Name).ToArray());
                componentChoices.Insert(0, "Select Component");
                DefaultComponentIndex = 0;
            }
            else
            {
                componentChoices = new List<string>() {};
                componentChoices.Insert(0, ""); 
                DefaultComponentIndex = 0;
            }
        }

        private GameObject DefaultObject
        {
            get { return InSceneInspectorInstance.defaultObject; }
            set
            {
                if (InSceneInspectorInstance.defaultObject != value)
                {
                    InSceneInspectorInstance.defaultObject = value;
                    UpdateComponentChoices();
                }
            }
        }

        public int DefaultComponentIndex
        {
            get
            {
                return InSceneInspectorInstance.defaultComponentIndex;
            }

            set
            {
                InSceneInspectorInstance.defaultComponentIndex = value;
                InSceneInspectorInstance.defaultComponent = componentChoices[InSceneInspectorInstance.defaultComponentIndex];
            }
        }

        public float Height
        {
            get
            {
                return InSceneInspectorInstance.contentPanel.GetComponent<LayoutElement>().minHeight;
            }

            set
            {
                float height = value;
                InSceneInspectorInstance.contentPanel.GetComponent<LayoutElement>().minHeight = height;
                RectTransform objectPickerRect = InSceneInspectorInstance.objectPicker.GetComponent<RectTransform>();
                objectPickerRect.sizeDelta = new Vector2(objectPickerRect.sizeDelta.x, height + objectPickerRect.anchoredPosition.y - 5);
                RectTransform componentPickerRect = InSceneInspectorInstance.componentPicker.GetComponent<RectTransform>();
                componentPickerRect.sizeDelta = new Vector2(componentPickerRect.sizeDelta.x, height + componentPickerRect.anchoredPosition.y - 5);
            }
        }
    }
}
#endif