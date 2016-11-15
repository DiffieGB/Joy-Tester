using System.IO;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using PigeonCoopToolkit.Generic.Editor;
using PigeonCoopToolkit.TIM.Internal;

namespace PigeonCoopToolkit.TIM.Editor
{
    public class TouchInputManagerEditor : EditorWindow
    {
        public TouchInputManager TouchInputManager;
        public TouchData MB1 = new TouchData(), MB2 = new TouchData();
        private float _prevTime;

        [System.Serializable]
        public class Selector
        {
            public string ID = string.Empty;
            public void SetSelection(string ID)
            {
                this.ID = ID;
            }
            public void ClearSelection()
            {
                ID = string.Empty;
            }
            public bool IsSelected(string ID)
            {
                return this.ID == ID;
            }
            public bool HasSelection()
            {
                return string.IsNullOrEmpty(ID) == false;
            }
            public string GetSelection()
            {
                return ID;
            }
        }

        [System.Serializable]
        public class AspectManager
        {
            [System.Serializable]
            public class Aspect
            {
                public int X, Y;
                public float GetRatio()
                {
                    return (float)X / (float)Y;
                }

                public Orientation GetOrientation()
                {
                    return X > Y ? Orientation.Landscape : Orientation.Portrate;
                }

                public string GetID()
                {
                    return X + ":" + Y;
                }
            }

            public enum Orientation
            {
                Portrate,
                Landscape
            }

            public List<Aspect> PreviewAspects;

            public AspectManager()
            {
                PreviewAspects = new List<Aspect>();
                PreviewAspects.Add(new Aspect { X = 4, Y = 3 });
                PreviewAspects.Add(new Aspect { X = 3, Y = 2 });
                PreviewAspects.Add(new Aspect { X = 16, Y = 10 });
                PreviewAspects.Add(new Aspect { X = 5, Y = 3 });
                PreviewAspects.Add(new Aspect { X = 16, Y = 9 });

                PreviewAspects.Add(new Aspect { X = 3, Y = 4 });
                PreviewAspects.Add(new Aspect { X = 2, Y = 3 });
                PreviewAspects.Add(new Aspect { X = 10, Y = 16 });
                PreviewAspects.Add(new Aspect { X = 3, Y = 5 });
                PreviewAspects.Add(new Aspect { X = 9, Y = 16 });
            }

            public Aspect GetAspect(string ID)
            {
                return PreviewAspects.FirstOrDefault(a => a.GetID() == ID);
            }
        }

        public Selector TouchLayerSelection = new Selector();
        public Selector TouchDeviceSelection = new Selector();
        public Selector TouchDeviceRegionResizeSelection = new Selector();
        public Selector TouchDeviceRegionTranslateSelection = new Selector();
        public Selector DevicePreviewerAspectSelection = new Selector();

        private class PreviewDrawData
        {
            public string TargetLayer;
            public Rect DrawRect;
            public bool IsMainPreview;
        }

        private class TouchInputManagerLayerPreview 
        {
            public string LayerID;
            public TouchInputManager TouchInputManager;
        }

        private List<TouchInputManagerLayerPreview> _touchInputManagerLayerPreviews;

        private List<PreviewDrawData> _previewDrawData;

        public Panel TouchLayerPanel;
        public Panel TouchInspectorPanel;

        public AspectManager Aspects;

        [MenuItem("Window/Pigeon Coop Toolkit/Touch Input Manager/Editor")]
        public static void Init()
        {
            TouchInputManagerEditor win = EditorWindow.GetWindow<TouchInputManagerEditor>() as TouchInputManagerEditor;
            win.title = "TIM 3";
            win.minSize = new Vector2(1000, 350);

        }

        [MenuItem("Window/Pigeon Coop Toolkit/Touch Input Manager/About")]
        public static void About()
        {
            InfoDialogue dialogue = EditorWindow.GetWindow<InfoDialogue>(true, "Touch Input Manager");
            dialogue.Init(Resources.Load("PCTK/TouchInputManager/banner") as Texture2D,
                new Generic.VersionInformation("Touch Input Manager", 3, 1, 2),
                Application.dataPath + "/PigeonCoopToolkit/__TouchInputManager Extras/Pigeon Coop Toolkit - TouchInputManager.pdf",
                "8984",true);
        }

        void Update()
        {
            EditorApplication.playmodeStateChanged -= PlayModeStateChange;
            EditorApplication.playmodeStateChanged += PlayModeStateChange;

            if (TouchInputManager == null)
                SaveAndRefreshTIM();

            if (TouchLayerPanel == null)
                TouchLayerPanel = new Panel("Touch Layers", PCUIHelper.Icon.Folder, PCUIHelper.ResizeSide.None);
            if (TouchInspectorPanel == null)
                TouchInspectorPanel = new Panel("Touch Inspector", PCUIHelper.Icon.Inspector, PCUIHelper.ResizeSide.None);

            if (_previewDrawData == null)
                _previewDrawData = new List<PreviewDrawData>();

            if (Aspects == null)
            {
                Aspects = new AspectManager();
                DevicePreviewerAspectSelection.SetSelection("16:9");
            }

            if (_touchInputManagerLayerPreviews == null)
            {
                RefreshPreviews();
            }
        }

        void PlayModeStateChange()
        {
            SaveAndRefreshTIM();
        }

        void OnGUI()
        {
            Repaint();

            if (TouchInputManager == null ||
                TouchLayerPanel == null ||
                TouchInspectorPanel == null ||
                _previewDrawData == null ||
                _touchInputManagerLayerPreviews == null ||
                Aspects == null)
                return;

            _previewDrawData.Clear();

            GUILayout.BeginHorizontal();
            TouchLayerPanel.Draw(DrawLayerList, DrawLayerListHeader);
            DrawPreviewArea();
            TouchInspectorPanel.Draw(DrawInspector, DrawInspectorHeader);
            GUILayout.EndHorizontal();

            foreach (PreviewDrawData pda in _previewDrawData)
            {
                TouchInputManager activeInstance = null;

                if (pda.IsMainPreview)
                    activeInstance = TouchInputManager;
                else if (_touchInputManagerLayerPreviews != null && _touchInputManagerLayerPreviews.Count != 0)
                    activeInstance = _touchInputManagerLayerPreviews.First(a => a.LayerID == pda.TargetLayer).TouchInputManager;

                activeInstance.SetAllInvisible();
                activeInstance.SetAllInactive();
                activeInstance.SetActive(pda.TargetLayer);
                activeInstance.SetVisible(pda.TargetLayer);

                if ((Event.current.isMouse && Event.current.button == 0) || (Event.current.rawType == EventType.mouseUp && Event.current.button == 0))
                {
                    TouchData which = MB1;//? MB1 : MB2;
                    if (Event.current.rawType == EventType.mouseDown)
                    {
                        which.Phase = TouchPhase.Began;
                        which.Position = Event.current.mousePosition;
                        which.Position.y += 22;
                        which.StartPosition = which.Position;

                        activeInstance.AddTouch(which);
                        GUIUtility.hotControl = 1;
                    }
                    else if (Event.current.rawType == EventType.mouseUp)
                    {
                        which.Phase = TouchPhase.Ended;
                        which.Position = Event.current.mousePosition;
                        which.Position.y += 22;
                        GUIUtility.hotControl = 0;
                    }
                    else if (Event.current.rawType == EventType.mouseDrag)
                    {
                        which.Phase = TouchPhase.Moved;
                        which.Position = Event.current.mousePosition;
                        which.Position.y += 22;
                    }
                }

                if (Event.current.type == EventType.repaint)
                {
                    Rect renderArea = pda.DrawRect;

                    renderArea.y += 22;
                    DrawArea DA = new DrawArea();
                    DA.SetDrawRect(renderArea);
                    activeInstance.SetDrawArea(DA);

                    float delta = (float)EditorApplication.timeSinceStartup - _prevTime;

                    activeInstance.UpdateManager(delta);
                    activeInstance.Render();
                }
            }

            if (Event.current.type == EventType.repaint)
                _prevTime = (float)EditorApplication.timeSinceStartup;

            GUI.SetNextControlName("");

            if (GUI.changed)
            {
                SaveAndRefreshTIM();
            }
        }

        private void DrawLayerListHeader()
        {
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(new GUIContent("New Layer"), PCUIHelper.GetStyle("MiniButton")))
            {
                AddNewLayer();
            }
        }

        private void DrawInspectorHeader()
        {
            GUILayout.FlexibleSpace();
            GUI.enabled = EditorApplication.isCompiling == false;

            if (GUILayout.Button(new GUIContent(EditorApplication.isCompiling ? "Compiling..." : "Build IDs"), PCUIHelper.GetStyle("MiniButton")))
            {
                RebuildIDs();
            }

            GUI.enabled = true;
        }

        private void DrawLayerList()
        {
            for (int i = 0; i < TouchInputManager.Layers.Count; i++)
            {
                Layer currentLayer = TouchInputManager.Layers[i];

                DrawPanelBlock(currentLayer.LayerID, null, () =>
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(5);



                    GUILayout.BeginVertical(PCUIHelper.GetStyle("DarkBack"), GUILayout.Height(TouchLayerPanel.GetWidth() * (9f / 16f)));
                    GUILayout.FlexibleSpace();
                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();

                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    GUILayout.FlexibleSpace();

                    GUILayout.EndVertical();
                    Rect drawRect = GUILayoutUtility.GetLastRect();

                    PCUIHelper.GetStyle("GridBackLayerPanelDisabled").border.left = (int)(drawRect.width);
                    PCUIHelper.GetStyle("GridBackLayerPanelDisabled").border.top = (int)(drawRect.height);
                    PCUIHelper.GetStyle("GridBackLayerPanel").border.left = (int)(drawRect.width);
                    PCUIHelper.GetStyle("GridBackLayerPanel").border.top = (int)(drawRect.height);

                    if (TouchLayerSelection.IsSelected(currentLayer.LayerID) == false)
                    {
                        GUI.Box(drawRect, "", PCUIHelper.GetStyle("GridBackLayerPanel"));

                        Rect previewDrawRect = drawRect;
                        Vector2 offset = TouchLayerPanel.GetScrollOffset();
                        offset.x -= 5;
                        offset.y -= 28;

                        previewDrawRect.x -= offset.x;
                        previewDrawRect.y -= offset.y;

                        previewDrawRect.yMin += 3;
                        previewDrawRect.xMax -= 3;

                        if (previewDrawRect.yMin < 28 || previewDrawRect.yMax > Screen.height - 28)
                        {
                            GUI.Box(drawRect, "", PCUIHelper.GetStyle("GridBackLayerPanelDisabled"));
                        }
                        else
                        {
                            GUI.Box(drawRect, "", PCUIHelper.GetStyle("GridBackLayerPanel"));
                            _previewDrawData.Add(new PreviewDrawData { DrawRect = previewDrawRect, TargetLayer = currentLayer.LayerID });
                        }
                    }
                    else
                    {

                        GUI.Box(drawRect, "", PCUIHelper.GetStyle("GridBackLayerPanelDisabled"));

                        Rect closeRect = new Rect(drawRect.x + drawRect.width / 2f - 50, drawRect.y + (drawRect.height / 2f) - 15, 100, 31);
                        if (GUI.Button(closeRect, "Close", PCUIHelper.GetStyle("CloseButton")))
                        {
                            ClearLayerSelection();
                        }
                    }

                    GUI.Box(drawRect, "", PCUIHelper.GetStyle("InnerShadow"));


                    GUILayout.Space(5);
                    GUILayout.EndHorizontal();
                }, TouchLayerSelection.IsSelected(currentLayer.LayerID), () =>
                {
                    GUILayout.FlexibleSpace();
                    if (TouchLayerSelection.IsSelected(currentLayer.LayerID) == false)
                    {
                        if (GUILayout.Button("Edit", PCUIHelper.GetStyle("MiniButton")))
                        {
                            ChangeLayerSelection(currentLayer.LayerID);
                        }
                    }

                    if (GUILayout.Button("Delete", PCUIHelper.GetStyle("MiniButton")))
                    {
                        if (EditorUtility.DisplayDialog("Delete Layer", "Are you sure? You will loose everything on this layer and this cannot be undone.", "Yes", "No"))
                        {
                            if (currentLayer.LayerID == TouchLayerSelection.ID)
                                ClearLayerSelection();

                            TouchInputManager.Layers.RemoveAt(i);
                            i--;
                        }
                    }
                });


                if (i != TouchInputManager.Layers.Count - 1)
                    GUILayout.Space(5);
            }
        }
        private void DrawInspector()
        {
            if (TouchDeviceSelection.HasSelection())
            {
                DrawDeviceInspector();
            }
            else if (TouchLayerSelection.HasSelection())
            {
                DrawLayerInspector();
            }
        }
        private void DrawLayerInspector()
        {
            Layer selectedLayer = TouchInputManager.Layers.First(a => a.LayerID == TouchLayerSelection.GetSelection());

            DrawPanelBlock("Layer Configuration", PCUIHelper.Icon.ConfigGears, () =>
            {
                PreviewDrawData pda = _previewDrawData.First(a => a.TargetLayer == selectedLayer.LayerID);
                selectedLayer.LayerID = EditorGUILayout.TextField(new GUIContent("ID"), selectedLayer.LayerID);
                selectedLayer.LayerID = FormatLayerID(selectedLayer.LayerID);
                TouchLayerSelection.ID = selectedLayer.LayerID;
                pda.TargetLayer = selectedLayer.LayerID;
            });

            GUILayout.Space(5);

            DrawPanelBlock("Joysticks", PCUIHelper.Icon.DeviceJoystick, () =>
            {
                for (int i = 0; i < selectedLayer.TouchJoysticks.Count; i++)
                {
                    TouchJoystick joystick = selectedLayer.TouchJoysticks[i];
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(new GUIContent(joystick.DeviceID), PCUIHelper.GetStyle("label"));
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Edit", PCUIHelper.GetStyle("MiniButton")))
                    {
                        ChangeDeviceSelection(joystick.DeviceID);
                    }
                    if (GUILayout.Button("Delete", PCUIHelper.GetStyle("MiniButton")))
                    {
                        if (EditorUtility.DisplayDialog("Delete Joystick", "Are you sure? This cannot be undone.", "Yes", "No"))
                        {
                            selectedLayer.TouchJoysticks.RemoveAt(i);
                            i--;
                        }
                    }
                    GUILayout.EndHorizontal();
                }
            });

            GUILayout.Space(5);

            DrawPanelBlock("Buttons", PCUIHelper.Icon.DeviceButton, () =>
            {
                for (int i = 0; i < selectedLayer.TouchButtons.Count; i++)
                {
                    TouchButton button = selectedLayer.TouchButtons[i];

                    GUILayout.BeginHorizontal();
                    GUILayout.Label(new GUIContent(button.DeviceID), PCUIHelper.GetStyle("label"));
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Edit", PCUIHelper.GetStyle("MiniButton")))
                    {
                        ChangeDeviceSelection(button.DeviceID);
                    }
                    if (GUILayout.Button("Delete", PCUIHelper.GetStyle("MiniButton")))
                    {
                        if (EditorUtility.DisplayDialog("Delete Button", "Are you sure? This cannot be undone.", "Yes", "No"))
                        {
                            selectedLayer.TouchButtons.RemoveAt(i);
                            i--;
                        }
                    }
                    GUILayout.EndHorizontal();
                }
            });
        }
        private void DrawDeviceInspector()
        {
            ///////////
            TouchDevice selectedDevice = null;

            if (TouchInputManager.Layers.First(a => a.LayerID == TouchLayerSelection.GetSelection()).TouchButtons.Any(b => b.DeviceID == TouchDeviceSelection.GetSelection()))
            {
                selectedDevice = TouchInputManager.Layers.First(a => a.LayerID == TouchLayerSelection.GetSelection()).TouchButtons.First(b => b.DeviceID == TouchDeviceSelection.GetSelection());
            }
            else if (TouchInputManager.Layers.First(a => a.LayerID == TouchLayerSelection.GetSelection()).TouchJoysticks.Any(b => b.DeviceID == TouchDeviceSelection.GetSelection()))
            {
                selectedDevice = TouchInputManager.Layers.First(a => a.LayerID == TouchLayerSelection.GetSelection()).TouchJoysticks.First(b => b.DeviceID == TouchDeviceSelection.GetSelection());
            }

            if (selectedDevice == null)
            {
                ClearDeviceSelection();
                return;
            }


            ///////////

            DrawPanelBlock("Device Configuration", PCUIHelper.Icon.ConfigGears, () =>
            {
                selectedDevice.DeviceID = EditorGUILayout.TextField(new GUIContent("ID"), selectedDevice.DeviceID);
                selectedDevice.DeviceID = FormatDeviceID(selectedDevice.DeviceID);
                TouchDeviceSelection.ID = selectedDevice.DeviceID;

                GUILayout.Space(5);
                if (GUILayout.Button("Back to " + TouchLayerSelection.GetSelection(), PCUIHelper.GetStyle("MiniButton"), GUILayout.ExpandWidth(true)))
                    ClearDeviceSelection();

            });

            GUILayout.Space(5);


            ////////////

            if (selectedDevice is TouchJoystick)
            {
                var js = selectedDevice as TouchJoystick;
                DrawPanelBlock("Joystick Configuration", PCUIHelper.Icon.ConfigGears, () =>
                {
                    js.Deadzone = EditorGUILayout.FloatField(new GUIContent("Deadzone"), js.Deadzone);
                    js.Deadzone = Mathf.Clamp01(js.Deadzone);
                });

                GUILayout.Space(5);
            }

            ///////////

            DrawPanelBlock("Transform", PCUIHelper.Icon.Transform, () =>
            {
                selectedDevice.ActiveRegion = EditorGUILayout.RectField(new GUIContent("Active Region"), selectedDevice.ActiveRegion);

                selectedDevice.ActiveRegion = ClampDeviceRegion(selectedDevice.ActiveRegion);

                selectedDevice.ActiveRegionOffset = EditorGUILayout.Vector2Field(new GUIContent("Region Offset"), selectedDevice.ActiveRegionOffset);
                selectedDevice.ActiveRegionOffset.x = Mathf.Clamp01(selectedDevice.ActiveRegionOffset.x);
                selectedDevice.ActiveRegionOffset.y = Mathf.Clamp01(selectedDevice.ActiveRegionOffset.y);
            });

            GUILayout.Space(5);
            ////////////

            

            DrawPanelBlock("Pressed Style", PCUIHelper.Icon.Pressed, () =>
            {
                EditorGUILayout.LabelField(new GUIContent("Top"), PCUIHelper.GetStyle("label"));
                selectedDevice.Style.TopPressed.TouchMaterial = EditorGUILayout.ObjectField(new GUIContent("Material"), selectedDevice.Style.TopPressed.TouchMaterial, typeof(Material), false) as Material;
                selectedDevice.Style.TopPressed.Color = EditorGUILayout.ColorField(new GUIContent("Color"), selectedDevice.Style.TopPressed.Color);
                selectedDevice.Style.TopPressed.Size = EditorGUILayout.FloatField(new GUIContent("Size"), selectedDevice.Style.TopPressed.Size);

                EditorGUILayout.LabelField(new GUIContent("Bottom"), PCUIHelper.GetStyle("label"));
                selectedDevice.Style.BottomPressed.TouchMaterial = EditorGUILayout.ObjectField(new GUIContent("Material"), selectedDevice.Style.BottomPressed.TouchMaterial, typeof(Material), false) as Material;
                selectedDevice.Style.BottomPressed.Color = EditorGUILayout.ColorField(new GUIContent("Color"), selectedDevice.Style.BottomPressed.Color);
                selectedDevice.Style.BottomPressed.Size = EditorGUILayout.FloatField(new GUIContent("Size"), selectedDevice.Style.BottomPressed.Size);
            });

            GUILayout.Space(5);

            DrawPanelBlock("Released Style", PCUIHelper.Icon.Released, () =>
            {
                EditorGUILayout.LabelField(new GUIContent("Top"), PCUIHelper.GetStyle("label"));
                selectedDevice.Style.TopReleased.TouchMaterial = EditorGUILayout.ObjectField(new GUIContent("Material"), selectedDevice.Style.TopReleased.TouchMaterial, typeof(Material), false) as Material;
                selectedDevice.Style.TopReleased.Color = EditorGUILayout.ColorField(new GUIContent("Color"), selectedDevice.Style.TopReleased.Color);
                selectedDevice.Style.TopReleased.Size = EditorGUILayout.FloatField(new GUIContent("Size"), selectedDevice.Style.TopReleased.Size);

                EditorGUILayout.LabelField(new GUIContent("Bottom"), PCUIHelper.GetStyle("label"));
                selectedDevice.Style.BottomReleased.TouchMaterial = EditorGUILayout.ObjectField(new GUIContent("Material"), selectedDevice.Style.BottomReleased.TouchMaterial, typeof(Material), false) as Material;
                selectedDevice.Style.BottomReleased.Color = EditorGUILayout.ColorField(new GUIContent("Color"), selectedDevice.Style.BottomReleased.Color);
                selectedDevice.Style.BottomReleased.Size = EditorGUILayout.FloatField(new GUIContent("Size"), selectedDevice.Style.BottomReleased.Size);
            });


            GUILayout.Space(5);

            //////////////////

            DrawPanelBlock("Style Animation", PCUIHelper.Icon.ConfigAnim, () =>
            {
                EditorGUILayout.LabelField("Interpolation");

                selectedDevice.Style.AnimationCurve = EditorGUILayout.CurveField(selectedDevice.Style.AnimationCurve, GUILayout.Height(60));
                selectedDevice.Style.AnimationDuration = EditorGUILayout.FloatField(new GUIContent("Duration"), selectedDevice.Style.AnimationDuration);
                
            }); 

            GUILayout.Space(5);

            //////////////////
            DrawPanelBlock("Return On Release", PCUIHelper.Icon.ConfigAnim, () =>
            {
                selectedDevice.Style.ReturnOnRelease = EditorGUILayout.Toggle(new GUIContent("Return on release"), selectedDevice.Style.ReturnOnRelease);
                if (selectedDevice.Style.ReturnOnRelease == true)
                {
                    selectedDevice.Style.ReturnAnimationCurve = EditorGUILayout.CurveField(selectedDevice.Style.ReturnAnimationCurve, GUILayout.Height(60));
                    selectedDevice.Style.ReturnOnReleaseTime = EditorGUILayout.FloatField(new GUIContent("Duration"), selectedDevice.Style.ReturnOnReleaseTime);
                    selectedDevice.Style.ReturnOnReleaseTime = Mathf.Clamp(selectedDevice.Style.ReturnOnReleaseTime, 0, 100);
                }


            });

            //////////
        }

        private void DrawDevicePreviewDropdownButton()
        {
            string current = null;

            if (DevicePreviewerAspectSelection.HasSelection())
            {
                current = DevicePreviewerAspectSelection.GetSelection();
            }

            if (GUILayout.Button(current == null ? "Preview screen" : "Previewing " + current, PCUIHelper.GetStyle("MiniButton")))
            {
                GenericMenu gm = new GenericMenu();
                foreach (var aspect in Aspects.PreviewAspects)
                {
                    if (aspect.GetOrientation() == AspectManager.Orientation.Portrate)
                        continue;

                    gm.AddItem(new GUIContent(aspect.GetID()), DevicePreviewerAspectSelection.GetSelection() == aspect.GetID(), DropdownDevicePreviewChanged, aspect.GetID());
                }
                gm.AddSeparator("");


                foreach (var aspect in Aspects.PreviewAspects)
                {
                    if (aspect.GetOrientation() == AspectManager.Orientation.Landscape)
                        continue;

                    gm.AddItem(new GUIContent(aspect.GetID()), DevicePreviewerAspectSelection.GetSelection() == aspect.GetID(), DropdownDevicePreviewChanged, aspect.GetID());
                }

                if (DevicePreviewerAspectSelection.HasSelection())
                {
                    gm.AddSeparator("");
                    gm.AddItem(new GUIContent("Clear"), false, DropdownDevicePreviewChanged, null);
                }
                gm.ShowAsContext();
            }

        }

        private void DropdownDevicePreviewChanged(object obj)
        {
            if (obj == null)
            {
                DevicePreviewerAspectSelection.ClearSelection();
            }
            else
            {
                DevicePreviewerAspectSelection.SetSelection(obj as string);
            }
        }

        private Rect ClampDeviceRegion(Rect rect)
        {
            rect.x = Mathf.Clamp01(rect.x);
            rect.y = Mathf.Clamp01(rect.y);

            if (rect.xMax > 1)
                rect.width = 1 - rect.x;
            if (rect.yMax > 1)
                rect.height = 1 - rect.y;

            if (rect.height < 0)
                rect.height = 0;
            if (rect.width < 0)
                rect.width = 0;

            return rect;
        }

        private void DrawPanelBlock(string title, string icon, System.Action drawContentFn, bool selected = false, System.Action headerExtras = null)
        {
            GUILayout.BeginVertical(PCUIHelper.GetStyle("PanelItem"));

            GUILayout.BeginHorizontal(PCUIHelper.GetStyle(selected ? "PanelHeaderSelected" : "PanelHeader"));
            if (string.IsNullOrEmpty(icon) == false)
                PCUIHelper.DrawIcon(icon);
            GUILayout.Label(title, PCUIHelper.GetStyle(selected ? "LabelSelected" : "label"));

            /*if(selected)
            {
                GUILayout.BeginHorizontal(PCUIHelper.GetStyle("PanelHeaderDecorator"));
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }*/

            if (headerExtras != null)
                headerExtras();

            GUILayout.EndHorizontal();

            GUILayout.Space(5);
            drawContentFn();
            GUILayout.Space(5);

            GUILayout.EndVertical();
        }
        private void DrawPreviewArea()
        {

            GUILayout.BeginVertical();
            if (TouchLayerSelection.HasSelection())
            {
                GUILayout.BeginHorizontal(PCUIHelper.GetStyle("SceneHeader"));
                PCUIHelper.DrawIcon(PCUIHelper.Icon.Preview);
                if (GUILayout.Button("Preview", PCUIHelper.GetStyle("LabelButton")))
                {
                    ClearLayerSelection();
                }

                PCUIHelper.DrawIcon(PCUIHelper.Icon.PointRight);

                PCUIHelper.DrawIcon(PCUIHelper.Icon.Layer);

                if (TouchDeviceSelection.HasSelection())
                {
                    if (GUILayout.Button(TouchLayerSelection.GetSelection(), PCUIHelper.GetStyle("LabelButton")))
                    {
                        ClearDeviceSelection();
                    }

                    PCUIHelper.DrawIcon(PCUIHelper.Icon.PointRight);
                    GUILayout.Label(TouchDeviceSelection.GetSelection(), PCUIHelper.GetStyle("label"));
                }
                else
                {
                    GUILayout.Label(TouchLayerSelection.GetSelection(), PCUIHelper.GetStyle("label"));
                }

                GUILayout.FlexibleSpace();

                if (GUILayout.Button("New Joystick", PCUIHelper.GetStyle("MiniButton")))
                {
                    AddNewJoystick();
                }

                if (GUILayout.Button("New Button", PCUIHelper.GetStyle("MiniButton")))
                {
                    AddNewButton();
                }

                DrawDevicePreviewDropdownButton();
                GUILayout.EndHorizontal();
            }
            else
            {
                GUILayout.BeginHorizontal(PCUIHelper.GetStyle("SceneHeader"));

                PCUIHelper.DrawIcon(PCUIHelper.Icon.Preview);
                GUILayout.Label("Preview", PCUIHelper.GetStyle("label"));

                GUILayout.FlexibleSpace();
                DrawDevicePreviewDropdownButton();
                GUILayout.EndHorizontal();
            }

            AspectManager.Aspect activeAspect = null;
            if (DevicePreviewerAspectSelection.HasSelection())
                activeAspect = Aspects.GetAspect(DevicePreviewerAspectSelection.GetSelection());

            if (DevicePreviewerAspectSelection.HasSelection())
            {
                PCUIHelper.GetStyle("GridBackLayerPanelDisabled").border.left = Mathf.RoundToInt(Screen.width - TouchInspectorPanel.GetWidth() - TouchLayerPanel.GetWidth());
                PCUIHelper.GetStyle("GridBackLayerPanelDisabled").border.top = Screen.height;
                GUILayout.BeginVertical(PCUIHelper.GetStyle("GridBackLayerPanelDisabled"));
            }
            else
            {
                PCUIHelper.GetStyle("GridBack").border.left = Mathf.RoundToInt(Screen.width - TouchInspectorPanel.GetWidth() - TouchLayerPanel.GetWidth());
                PCUIHelper.GetStyle("GridBack").border.top = Screen.height;
                GUILayout.BeginVertical(PCUIHelper.GetStyle("GridBack"));
            }

            GUILayout.BeginVertical(PCUIHelper.GetStyle("InnerShadow"));

            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();

            Rect previewRect = GUILayoutUtility.GetLastRect();

            if (DevicePreviewerAspectSelection.HasSelection())
            {
                float ratio = activeAspect.GetRatio();
                Vector2 previewDeviceSize = new Vector2(previewRect.width, previewRect.width / ratio);

                if (previewDeviceSize.y > previewRect.height)
                {
                    float scaleDownBy = previewDeviceSize.y - previewRect.height;
                    previewDeviceSize.y -= scaleDownBy;
                    previewDeviceSize.x -= scaleDownBy * ratio;
                }

                if (previewDeviceSize.x > previewRect.width)
                {
                    float scaleDownBy = previewDeviceSize.x - previewRect.width;
                    previewDeviceSize.x -= scaleDownBy;
                    previewDeviceSize.y -= scaleDownBy * ratio;
                }

                previewDeviceSize /= 1.5f;

                previewRect = new Rect(previewRect.center.x - (previewDeviceSize.x / 2f), previewRect.center.y - (previewDeviceSize.y / 2f), previewDeviceSize.x, previewDeviceSize.y);

                Rect previewDeviceRect = previewRect;

                if (activeAspect.GetOrientation() == AspectManager.Orientation.Landscape)
                {
                    previewDeviceRect.x -= 35;
                    previewDeviceRect.width += 70;
                    previewDeviceRect.y -= 10;
                    previewDeviceRect.height += 20;
                    GUI.Box(previewDeviceRect, "", PCUIHelper.GetStyle("DevicePreviewLandscape"));
                }
                else
                {
                    previewDeviceRect.x -= 10;
                    previewDeviceRect.width += 20;
                    previewDeviceRect.y -= 35;
                    previewDeviceRect.height += 70;
                    GUI.Box(previewDeviceRect, "", PCUIHelper.GetStyle("DevicePreviewPortrate"));
                }

                PCUIHelper.GetStyle("GridBack").border.left = Mathf.RoundToInt(previewRect.width);
                PCUIHelper.GetStyle("GridBack").border.top = Mathf.RoundToInt(previewRect.height);
                GUI.Box(previewRect, "", PCUIHelper.GetStyle("GridBack"));
            }

            GUILayout.EndVertical();
            GUILayout.EndVertical();
            GUILayout.EndVertical();

            if (TouchLayerSelection.HasSelection())
            {
                Layer selectedLayer = TouchInputManager.Layers.First(a => a.LayerID == TouchLayerSelection.GetSelection());
                List<TouchDevice> layerDevices = new List<TouchDevice>();
                if (selectedLayer.TouchJoysticks.Count != 0)
                    layerDevices.AddRange(selectedLayer.TouchJoysticks.Cast<TouchDevice>());
                if (selectedLayer.TouchButtons.Count != 0)
                    layerDevices.AddRange(selectedLayer.TouchButtons.Cast<TouchDevice>());

                foreach (TouchDevice device in layerDevices)
                {
                    Rect deviceRegion = device.ActiveRegion;
                    deviceRegion.x *= previewRect.width;
                    deviceRegion.x += previewRect.x;
                    deviceRegion.y *= previewRect.height;
                    deviceRegion.y += previewRect.y;
                    deviceRegion.width *= previewRect.width;
                    deviceRegion.height *= previewRect.height;

                    if (device.DeviceID == TouchDeviceSelection.GetSelection())
                        GUI.Box(deviceRegion, "", PCUIHelper.GetStyle("GridPreviewRegionSelected"));
                    else
                        GUI.Box(deviceRegion, "", PCUIHelper.GetStyle("GridPreviewRegion"));


                    Rect resizeRegion = deviceRegion;
                    resizeRegion.xMin = resizeRegion.xMax - 20;
                    resizeRegion.yMin = resizeRegion.yMax - 20;
                    resizeRegion.width = 20;
                    resizeRegion.height = 20;

                    EditorGUIUtility.AddCursorRect(resizeRegion, MouseCursor.ResizeUpLeft);

                    GUI.Box(resizeRegion, "", PCUIHelper.GetStyle("GridResizeRegion"));

                    if ((Event.current.isMouse || Event.current.rawType == EventType.mouseUp) && Event.current.button == 0)
                    {
                        if (TouchDeviceRegionResizeSelection.GetSelection() == device.DeviceID)
                        {
                            device.Init();

                            if (Event.current.rawType == EventType.mouseUp)
                            {
                                TouchDeviceRegionResizeSelection.ClearSelection();
                                Event.current.Use();
                                GUIUtility.hotControl = 0;
                            }
                            else if (Event.current.rawType == EventType.mouseDrag)
                            {
                                device.ActiveRegion.width += Event.current.delta.x / previewRect.width;
                                device.ActiveRegion.height += Event.current.delta.y / previewRect.height;

                                device.ActiveRegion = ClampDeviceRegion(device.ActiveRegion);
                                if (device.ActiveRegion.width * previewRect.width < 20)
                                    device.ActiveRegion.width = 20 / previewRect.width;
                                if (device.ActiveRegion.height * previewRect.height < 20)
                                    device.ActiveRegion.height = 20 / previewRect.height;

                                Event.current.Use();
                            }
                        }
                        else if (TouchDeviceRegionResizeSelection.HasSelection() == false && resizeRegion.Contains(Event.current.mousePosition))
                        {
                            if (Event.current.rawType == EventType.mouseDown && GUIUtility.hotControl == 0)
                            {
                                TouchDeviceSelection.SetSelection(device.DeviceID);
                                TouchDeviceRegionResizeSelection.SetSelection(device.DeviceID);
                                Event.current.Use();
                                GUIUtility.hotControl = 1;
                            }

                        }
                    }


                    Rect translateRegion = deviceRegion;
                    translateRegion.xMin = deviceRegion.x + (deviceRegion.width / 2f) - 20;
                    translateRegion.yMin = deviceRegion.yMin;
                    translateRegion.width = 40;
                    translateRegion.height = 20;

                    EditorGUIUtility.AddCursorRect(translateRegion, MouseCursor.MoveArrow);

                    GUI.Box(translateRegion, "", PCUIHelper.GetStyle("GridTranslateRegion"));

                    if ((Event.current.isMouse || Event.current.rawType == EventType.mouseUp) && Event.current.button == 0)
                    {
                        if (TouchDeviceRegionTranslateSelection.GetSelection() == device.DeviceID)
                        {
                            device.Init();

                            if (Event.current.rawType == EventType.mouseUp)
                            {
                                TouchDeviceRegionTranslateSelection.ClearSelection();
                                Event.current.Use();
                                GUIUtility.hotControl = 0;
                            }
                            else if (Event.current.rawType == EventType.mouseDrag)
                            {
                                device.ActiveRegion.x += Event.current.delta.x / previewRect.width;
                                device.ActiveRegion.y += Event.current.delta.y / previewRect.height;

                                if (device.ActiveRegion.xMax > 1)
                                {
                                    float difference = 1 - device.ActiveRegion.xMax;
                                    device.ActiveRegion.xMax = 1;
                                    device.ActiveRegion.xMin += difference;
                                }

                                if (device.ActiveRegion.yMax > 1)
                                {
                                    float difference = 1 - device.ActiveRegion.yMax;
                                    device.ActiveRegion.yMax = 1;
                                    device.ActiveRegion.yMin += difference;
                                }

                                device.ActiveRegion = ClampDeviceRegion(device.ActiveRegion);
                                Event.current.Use();
                            }
                        }
                        else if (TouchDeviceRegionTranslateSelection.HasSelection() == false && translateRegion.Contains(Event.current.mousePosition))
                        {
                            if (Event.current.rawType == EventType.mouseDown && GUIUtility.hotControl == 0)
                            {
                                TouchDeviceSelection.SetSelection(device.DeviceID);
                                TouchDeviceRegionTranslateSelection.SetSelection(device.DeviceID);
                                Event.current.Use();
                                GUIUtility.hotControl = 1;
                            }

                        }
                    }
                    TextAnchor restore = PCUIHelper.GetStyle("LabelButton").alignment;
                    PCUIHelper.GetStyle("LabelButton").alignment = TextAnchor.MiddleCenter;
                    if (GUI.Button(new Rect(deviceRegion.x, deviceRegion.y + 25, deviceRegion.width, 22), new GUIContent(device.DeviceID), PCUIHelper.GetStyle("LabelButton")))
                    {
                        ChangeDeviceSelection(device.DeviceID);
                    }
                    PCUIHelper.GetStyle("LabelButton").alignment = restore;
                }


                _previewDrawData.Add(new PreviewDrawData { DrawRect = previewRect, TargetLayer = TouchLayerSelection.GetSelection(), IsMainPreview = true });
            }
        }

        private void AddNewLayer(bool autoSelect = true)
        {
            Layer newLayer = new Layer();
            newLayer.LayerID = GenereateUniqueLayerName();
            TouchInputManager.Layers.Add(newLayer);
            if (autoSelect) ChangeLayerSelection(newLayer.LayerID);
        }
        private void AddNewButton(bool autoSelect = true)
        {
            TouchButton newButton = GenerateDefaultButton();
            TouchInputManager.Layers.First(a => a.LayerID == TouchLayerSelection.GetSelection()).TouchButtons.Add(newButton);
            if (autoSelect) ChangeDeviceSelection(newButton.DeviceID);
        }
        private void AddNewJoystick(bool autoSelect = true)
        {
            TouchJoystick newJoystick = GenerateDefaultJoystick();
            TouchInputManager.Layers.First(a => a.LayerID == TouchLayerSelection.GetSelection()).TouchJoysticks.Add(newJoystick);
            if (autoSelect) ChangeDeviceSelection(newJoystick.DeviceID);
        }

        private TouchJoystick GenerateDefaultJoystick()
        {
            TouchJoystick newJoystick = new TouchJoystick();
            newJoystick.DeviceID = GenerateUniqueDeviceName();
            newJoystick.Style = GenerateDefaultTouchStyle();
            return newJoystick;
        }
        private TouchButton GenerateDefaultButton()
        {
            TouchButton newButton = new TouchButton();
            newButton.DeviceID = GenerateUniqueDeviceName();
            newButton.Style = GenerateDefaultTouchStyle();
            return newButton;
        }
        private TouchStyle GenerateDefaultTouchStyle()
        {
            TouchStyle newTouchStyle = new TouchStyle();
            newTouchStyle.BottomPressed.TouchMaterial = newTouchStyle.BottomReleased.TouchMaterial = Resources.Load("PCTK/TouchInputManager/Default/DefaultDeviceBottom") as Material;
            if (newTouchStyle.BottomPressed.TouchMaterial == null || newTouchStyle.BottomReleased.TouchMaterial == null)
            {
                Debug.LogError("TouchInputManager: You are missing some files - missing a default material. You should reimport TouchInputManager");
            }
            newTouchStyle.TopReleased.TouchMaterial = newTouchStyle.TopPressed.TouchMaterial = Resources.Load("PCTK/TouchInputManager/Default/DefaultDeviceTop") as Material;
            if (newTouchStyle.TopReleased.TouchMaterial == null || newTouchStyle.TopPressed.TouchMaterial == null)
            {
                Debug.LogError("TouchInputManager: You are missing some files - missing a default material. You should reimport TouchInputManager");
            }

            return newTouchStyle;
        }
        private string FormatLayerID(string ID)
        {
            if (string.IsNullOrEmpty(ID))
                return GenereateUniqueLayerName();

            ID = Regex.Replace(ID, "[^a-zA-Z0-9]", string.Empty);
            ID = Regex.Replace(ID, "^[^a-zA-Z]*", string.Empty);

            if (string.IsNullOrEmpty(ID))
                return GenereateUniqueLayerName();

            ID = char.ToUpper(ID[0]) + ID.Substring(1);
            if (TouchInputManager.Layers.Count(a => a.LayerID == ID) > 1)
            {
                ID = GenereateUniqueLayerName();
            }

            return ID;
        }
        private string FormatDeviceID(string ID)
        {
            if (string.IsNullOrEmpty(ID))
                return GenerateUniqueDeviceName();

            ID = Regex.Replace(ID, "[^a-zA-Z0-9]", string.Empty);
            ID = Regex.Replace(ID, "^[^a-zA-Z]*", string.Empty);

            if (string.IsNullOrEmpty(ID))
                return GenerateUniqueDeviceName();

            ID = char.ToUpper(ID[0]) + ID.Substring(1);
            if (TouchInputManager.Layers.First(a => a.LayerID == TouchLayerSelection.GetSelection()).TouchButtons.Count(a => a.DeviceID == ID) +
                TouchInputManager.Layers.First(a => a.LayerID == TouchLayerSelection.GetSelection()).TouchJoysticks.Count(a => a.DeviceID == ID) > 1)
            {
                ID = GenerateUniqueDeviceName();
            }

            return ID;
        }
        private string GenereateUniqueLayerName()
        {
            string uniqueName = "NewLayer";
            int tryCount = 0;
            do
            {
                uniqueName = "NewLayer" + tryCount;
                tryCount++;
            } while (TouchInputManager.Layers.Any(a => a.LayerID == uniqueName));

            return uniqueName;
        }
        private string GenerateUniqueDeviceName()
        {
            string uniqueName = "NewDevice";
            int tryCount = 0;
            do
            {
                uniqueName = "NewDevice" + tryCount;
                tryCount++;
            } while (TouchInputManager.Layers.First(a => a.LayerID == TouchLayerSelection.GetSelection()).TouchJoysticks.Any(b => b.DeviceID == uniqueName) ||
                TouchInputManager.Layers.First(a => a.LayerID == TouchLayerSelection.GetSelection()).TouchButtons.Any(b => b.DeviceID == uniqueName));

            return uniqueName;
        }

        private void ClearLayerSelection()
        {
            ChangeLayerSelection(string.Empty);
        }
        private void ClearDeviceSelection()
        {
            ChangeDeviceSelection(string.Empty);
        }
        private void ChangeLayerSelection(string layer)
        {
            RefreshPreviews();
            TouchLayerSelection.SetSelection(layer);
            TouchDeviceSelection.ClearSelection();
            TouchDeviceRegionResizeSelection.ClearSelection();
            TouchDeviceRegionTranslateSelection.ClearSelection();
            GUI.FocusControl("");
        }
        private void ChangeDeviceSelection(string device)
        {
            TouchDeviceSelection.SetSelection(device);
            TouchDeviceRegionResizeSelection.ClearSelection();
            TouchDeviceRegionTranslateSelection.ClearSelection();
            GUI.FocusControl("");
        }

        private void SaveAndRefreshTIM()
        {
            MB1 = new TouchData(); MB2 = new TouchData();

            if (TouchInputManager != null)
            {
                EditorUtility.SetDirty(TouchInputManager);
                TouchInputManager.Init();
            }
            else
            {
                EnsureDatafileExists();
                TouchInputManager = Resources.Load("PCTK/TouchInputManager/TouchInputManagerData") as TouchInputManager;
            }
        }
        private void RefreshPreviews()
        {
            if (_touchInputManagerLayerPreviews != null && _touchInputManagerLayerPreviews.Count != 0)
                _touchInputManagerLayerPreviews.ForEach(a => { if(a.TouchInputManager != null) DestroyImmediate(a.TouchInputManager); });

            _touchInputManagerLayerPreviews = new List<TouchInputManagerLayerPreview>();
            foreach (Layer layer in TouchInputManager.Layers)
            {
                TouchInputManager instance = Instantiate(TouchInputManager) as TouchInputManager;
                instance.hideFlags = HideFlags.HideAndDontSave;

                instance.SetAllInactive();
                instance.SetAllInvisible();
                instance.SetVisible(layer.LayerID);
                instance.SetActive(layer.LayerID);
                _touchInputManagerLayerPreviews.Add(new TouchInputManagerLayerPreview { TouchInputManager = instance, LayerID = layer.LayerID });
            }
        }

        private void RebuildIDs()
        {
            string[] found = AssetDatabase.FindAssets("TouchIDAutogenerated");
            string targetPath = AssetDatabase.GUIDToAssetPath(found[0]);
            targetPath = targetPath.Replace("Assets", "");

            List<string> UniqueDeviceIDs = new List<string>();
            List<string> UniqueLayerIDs = new List<string>();

            foreach (var layer in TouchInputManager.Layers)
            {
                UniqueLayerIDs.Add(layer.LayerID);

                foreach (var joystickDevice in layer.TouchJoysticks)
                {
                    if (UniqueDeviceIDs.Contains(joystickDevice.DeviceID) == false)
                        UniqueDeviceIDs.Add(joystickDevice.DeviceID);
                }

                foreach (var buttonDevice in layer.TouchButtons)
                {
                    if (UniqueDeviceIDs.Contains(buttonDevice.DeviceID) == false)
                        UniqueDeviceIDs.Add(buttonDevice.DeviceID);
                }

            }

            AssetDatabase.DeleteAsset(Application.dataPath + targetPath);

            string generatedCSharp = "";


            
            generatedCSharp += "namespace PigeonCoopToolkit.TIM {\n";
            generatedCSharp += "public static class LayerID {\n";

            foreach (var layerID in UniqueLayerIDs)
            {
                generatedCSharp += string.Format("public static readonly string {0} = \"{0}\";\n", layerID);
            }

            generatedCSharp += "}\n";

            generatedCSharp += "public static class DeviceID {\n";

            foreach (var deviceID in UniqueDeviceIDs)
            {
                generatedCSharp += string.Format("public static readonly string {0} = \"{0}\";\n", deviceID);
            }

            generatedCSharp += "}\n";
            generatedCSharp += "}\n";



            System.IO.File.WriteAllText(Application.dataPath + targetPath, generatedCSharp);
            AssetDatabase.Refresh();
        }

        public static void EnsureDatafileExists()
        {
            TouchInputManager existingTIMSave = Resources.Load<TouchInputManager>("PCTK/TouchInputManager/TouchInputManagerData");
            if (existingTIMSave != null)
                return;

            TouchInputManager asset = ScriptableObject.CreateInstance<TouchInputManager>();

            string path = AssetDatabase.GetAssetPath(Resources.Load("PCTK/TouchInputManager/banner"));
            string fileName = Path.GetFileName(path);
            if (string.IsNullOrEmpty(fileName) == false)
                path = path.Replace(fileName, "");

            if (path.StartsWith("Assets/"))
                path = "a" + path.Substring(1);

            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/TouchInputManagerData.asset");

            AssetDatabase.CreateAsset(asset, assetPathAndName);
        }
    }

}

