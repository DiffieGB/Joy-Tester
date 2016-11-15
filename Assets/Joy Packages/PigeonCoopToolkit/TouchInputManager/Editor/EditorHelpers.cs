using UnityEditor;
using UnityEngine;

namespace PigeonCoopToolkit.TIM.Editor
{
    [System.Serializable]
    public class Panel
    {
        public string Title, Icon;
        public PCUIHelper.ResizeSide ResizeSide;
        private Vector2 _scrollPosition;

        [SerializeField]
        private float _width = 280;
        [SerializeField]
        private float _minWidth = 280;

        private bool _dragged = false;

        public Vector2 GetScrollOffset()
        {
            return _scrollPosition;
        }

        public float GetWidth()
        {
            return _width;
        }

        public Panel(string title, string icon, PCUIHelper.ResizeSide resize)
        {
            Title = title;
            Icon = icon;
            ResizeSide = resize;
        }

        public void Draw(System.Action DrawFn, System.Action HeaderExtras = null)
        {
            GUILayout.BeginVertical();
            GUILayout.BeginVertical(PCUIHelper.GetStyle("PanelItem"), GUILayout.Width(_width));

            GUILayout.BeginHorizontal(PCUIHelper.GetStyle("LightBack"));
            if (string.IsNullOrEmpty(Icon) == false)
                PCUIHelper.DrawIcon(Icon);
            GUILayout.Label(Title, PCUIHelper.GetStyle("label"));

            if (HeaderExtras != null)
                HeaderExtras();

            /*
            GUILayout.BeginHorizontal(PCUIHelper.GetStyle("PanelHeaderDecorator"));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            */

            /*
            GUILayout.FlexibleSpace();
            GUILayout.Button("New Layout", style.GetStyle("MiniButton"));
            */

            GUILayout.EndHorizontal();

            GUILayout.BeginVertical(PCUIHelper.GetStyle("MidBack"));
            GUILayout.BeginVertical(PCUIHelper.GetStyle("Padder"));

            _scrollPosition = PCUIHelper.BeginScrollView(_scrollPosition);

            DrawFn();

            GUILayout.FlexibleSpace();
            PCUIHelper.EndScrollView();


            GUILayout.EndVertical();
            GUILayout.EndVertical();

            GUILayout.EndVertical();
            GUILayout.EndVertical();

            if (ResizeSide == PCUIHelper.ResizeSide.None)
                return;

            Rect draggableRect = GUILayoutUtility.GetLastRect();

            if (ResizeSide == PCUIHelper.ResizeSide.Right)
                draggableRect = new Rect(draggableRect.xMax - 5, draggableRect.yMin, 10, draggableRect.height);
            else
                draggableRect = new Rect(draggableRect.xMin - 5, draggableRect.yMin, 10, draggableRect.height);

            EditorGUIUtility.AddCursorRect(draggableRect, MouseCursor.ResizeHorizontal);
            if (Event.current.isMouse && Event.current.button == 0)
            {
                if (Event.current.rawType == EventType.mouseDown && draggableRect.Contains(Event.current.mousePosition))
                {
                    _dragged = true;
                    Event.current.Use();
                }
                else if (Event.current.rawType == EventType.mouseDrag && _dragged)
                {
                    if (ResizeSide == PCUIHelper.ResizeSide.Right)
                        _width += Event.current.delta.x;
                    else
                        _width -= Event.current.delta.x;

                    Event.current.Use();
                }
                else
                {
                    _dragged = false;
                }
            }

            if (_width < _minWidth)
                _width = _minWidth;
        }
    }

    public static class PCUIHelper
    {
        public enum ResizeSide
        {
            Left,
            Right,
            None
        }

        public static class Icon
        {
            public static readonly string Pressed = "a";
            public static readonly string Released = "b";
            public static readonly string Inspector = "c";
            public static readonly string Layer = "d";
            public static readonly string ConfigAnim = "e";
            public static readonly string ConfigGears = "f";
            public static readonly string Folder = "g";
            public static readonly string PointRight = "h";
            public static readonly string DeviceButton = "i";
            public static readonly string DeviceJoystick = "j";
            public static readonly string Transform = "k";
            public static readonly string Preview = "l";
        }

        public static GUISkin Style
        {
            get
            {
                if (_proSkin == null)
                {
                    _proSkin = Resources.Load("PCTK/TouchInputManager/Editor/ProSkin") as GUISkin;
                }
                if (_lightSkin == null)
                {
                    _lightSkin = Resources.Load("PCTK/TouchInputManager/Editor/LightSkin") as GUISkin;
                }

                return EditorGUIUtility.isProSkin ? _proSkin : _lightSkin;
            }
        }
        private static GUISkin _proSkin;
        private static GUISkin _lightSkin;
        public static GUIStyle GetStyle(string id)
        {
            return Style.GetStyle(id);
        }
        public static Vector2 BeginScrollView(Vector2 position)
        {
            GUISkin temp = GUI.skin;
            GUI.skin = Style;
            position = GUILayout.BeginScrollView(position);
            GUI.skin = temp;
            return position;
        }
        public static void EndScrollView()
        {
            GUILayout.EndScrollView();
        }
        public static void DrawIcon(string iconID)
        {
            GUILayout.Label(iconID, PCUIHelper.GetStyle("FontelloIcon"));
        }
    }
}

