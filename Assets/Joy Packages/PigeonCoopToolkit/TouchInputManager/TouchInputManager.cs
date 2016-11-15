using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PigeonCoopToolkit.TIM.Internal
{
    public class TouchInputManager : ScriptableObject
    {
        public List<Layer> Layers = new List<Layer>();

        private List<string> _visibleLayers = new List<string>();
        private List<string> _activeLayers = new List<string>();

        public void SetAllInvisible()
        {
            _visibleLayers.Clear();
        }
        public void SetAllInactive()
        {
            _activeLayers.Clear();
        }
        public void SetAllVisible()
        {
            foreach(var layer in Layers)
            {
                _visibleLayers.Add(layer.LayerID);
            }
        }
        public void SetAllActive()
        {
            foreach (var layer in Layers)
            {
                _activeLayers.Add(layer.LayerID);
            }
        }
        public void SetVisible(string layerID)
        {
            if (_visibleLayers.Contains(layerID) == false)
                _visibleLayers.Add(layerID);
        }
        public void SetInvisible(string layerID)
        {
            if (_visibleLayers.Contains(layerID))
                _visibleLayers.Remove(layerID);
        }
        public void SetActive(string layerID)
        {
            if (_activeLayers.Contains(layerID) == false)
            {
                _activeLayers.Add(layerID);
            }
        }
        public void SetInactive(string layerID)
        {
            if (_activeLayers.Contains(layerID))
                _activeLayers.Remove(layerID);
        }
        public Vector2 JoystickValue(string layerID, string deviceID, bool normalized = false)
        {
            var stick = FindJoystick(layerID, deviceID);
            if(stick != null)
            {
                if (normalized)
                    return stick.GetInputNormalized();
                else
                    return stick.GetInput();
            }
            else
                return Vector2.zero;
        }
        public bool GetJoystickUp(string layerID, string deviceID)
        {
            var stick = FindJoystick(layerID, deviceID);
            if (stick != null)
            {
                return stick.WasUp;
            }
            else
                return false;
        }
        public bool GetJoystick(string layerID, string deviceID)
        {
            var stick = FindJoystick(layerID, deviceID);
            if (stick != null)
            {
                return stick.IsDown;
            }
            else
                return false;
        }
        public bool GetJoystickDown(string layerID, string deviceID)
        {
            var stick = FindJoystick(layerID, deviceID);
            if (stick != null)
            {
                return stick.WasDown;
            }
            else
                return false;
        }
        public bool GetButtonUp(string layerID, string deviceID)
        {
            var button = FindButton(layerID, deviceID);
            if (button != null)
            {
                return button.WasUp;
            }
            else
                return false;
        }
        public bool GetButton(string layerID, string deviceID)
        {
            var button = FindButton(layerID, deviceID);
            if (button != null)
            {
                return button.IsDown;
            }
            else
                return false;
        }
        public bool GetButtonDown(string layerID, string deviceID)
        {
            var button = FindButton(layerID, deviceID);
            if (button != null)
            {
                return button.WasDown;
            }
            else
                return false;
        }

        public void Init()
        {
            foreach (Layer layer in Layers)
            {
                layer.Init();
            }
        }

        public void SetDrawArea(DrawArea drawArea)
        {
            foreach (Layer layer in Layers)
            {
                layer.SetDrawArea(drawArea);
            }
        }

        public void UpdateManager(float deltatime)
        {
            foreach (Layer layer in Layers)
            {
                layer.UpdateLayer(deltatime);
            }
        }

        public void Render()
        {
            foreach (Layer layer in Layers)
            {
                if (_visibleLayers.Contains(layer.LayerID))
                    layer.Render();
            }
        }

        public bool AddTouch(TouchData touch)
        {
            foreach (Layer layer in Layers)
            {
                if (_activeLayers.Contains(layer.LayerID) == false)
                    continue;

                if (layer.AddTouch(touch))
                    return true;
            }

            return false;
        }

        private TouchJoystick FindJoystick(string layerID, string deviceID)
        {
            foreach (var layer in Layers)
            {
                if (layer.LayerID == layerID)
                {
                    foreach (var joystick in layer.TouchJoysticks)
                    {
                        if (joystick.DeviceID == deviceID)
                        {
                            return joystick;
                        }
                    }
                }
            }

            return null;
        }

        private TouchButton FindButton(string layerID, string deviceID)
        {
            foreach (var layer in Layers)
            {
                if (layer.LayerID == layerID)
                {
                    foreach (var button in layer.TouchButtons)
                    {
                        if (button.DeviceID == deviceID)
                        {
                            return button;
                        }
                    }
                }
            }

            return null;
        }
    }

    [System.Serializable]
    public class Layer
    {
        public string LayerID = string.Empty;
        public List<TouchButton> TouchButtons = new List<TouchButton>();
        public List<TouchJoystick> TouchJoysticks = new List<TouchJoystick>();

        public void Init()
        {
            foreach (TouchButton device in TouchButtons)
            {
                device.Init();
            }

            foreach (TouchJoystick device in TouchJoysticks)
            {
                device.Init();
            }
        }

        public void SetDrawArea(DrawArea drawArea)
        {
            foreach (TouchButton device in TouchButtons)
            {
                device.SetDrawArea(drawArea);
            }

            foreach (TouchJoystick device in TouchJoysticks)
            {
                device.SetDrawArea(drawArea);
            }

        }

        public void UpdateLayer(float deltatime)
        {
            foreach (TouchButton device in TouchButtons)
            {
                device.UpdateDevice(deltatime);
            }

            foreach (TouchJoystick device in TouchJoysticks)
            {
                device.UpdateDevice(deltatime);
            }
        }

        public void Render()
        {
            foreach (TouchButton device in TouchButtons)
            {
                device.Render();
            }

            foreach (TouchJoystick device in TouchJoysticks)
            {
                device.Render();
            }

        }

        public bool AddTouch(TouchData touch)
        {
            foreach (TouchButton device in TouchButtons)
            {
                if (device.AddTouch(touch))
                    return true;
            }

            foreach (TouchJoystick device in TouchJoysticks)
            {
                if (device.AddTouch(touch))
                    return true;
            }

            return false;
        }
    }

    [System.Serializable]
    public class TouchJoystick : TouchDevice
    {
        public float Deadzone;

        protected override void OnUpdate(float deltatime)
        {
            if (IsDown)
            {
                float _scale = Style.BottomPressed.Size;
                if (_scale <= 0)
                    _scale = Mathf.Epsilon;

                _bottomPosition = _drawArea.GetTransformedMouse(_drawArea.ToScreenSpace(_currentTouch.StartPosition));

                Vector2 touchPos = _drawArea.GetTransformedMouse(_drawArea.ToScreenSpace(_currentTouch.Position));
                Vector2 firstInitScreenPos = _bottomPosition;
                Debug.Log((Mathf.Abs(touchPos.x - firstInitScreenPos.x)));
                float scaleX = Mathf.Clamp(Remap(Mathf.Abs(touchPos.x - firstInitScreenPos.x), Deadzone * (_scale / 2), _scale / 2, 0, _scale / 2), 0, _scale / 2);
                float scaleY = Mathf.Clamp(Remap(Mathf.Abs(touchPos.y - firstInitScreenPos.y), Deadzone * (_scale / 2), _scale / 2, 0, _scale / 2), 0, _scale / 2 * _drawArea.GetRatio());
                _topPosition = new Vector3(firstInitScreenPos.x + (((GetTouchPositionNormalized().x * scaleX))), firstInitScreenPos.y + (((GetTouchPositionNormalized().y * scaleY))));
            }
            else
            {
                LerpBackToIdle();// _topPosition = _bottomPosition = new Vector2(ActiveRegion.x + (ActiveRegion.width * ActiveRegionOffset.x), ActiveRegion.y + (ActiveRegion.height * ActiveRegionOffset.y));
            }
        }

    
        private Vector2 GetTouchPositionNormalized()
        {
            if (IsDown)
            {
                Vector2 touchPos = _drawArea.GetTransformedMouse(_drawArea.ToScreenSpace(_currentTouch.Position)) - _drawArea.GetTransformedMouse(_drawArea.ToScreenSpace(_currentTouch.StartPosition));
                return touchPos.normalized;
            }

            return Vector2.zero;
        }

        public Vector2 GetInputNormalized()
        {
            if (IsDown)
            {
                Vector2 input = (_currentTouch.Position - _currentTouch.StartPosition);
                //input = new Vector2(disableX ? 0 : input.x, disableY ? 0 : input.y);
                return input.normalized;
            }
            else
                return Vector2.zero;
        }

        public Vector2 GetInput()
        {
            if (IsDown)
            {
                float _scale = Style.BottomPressed.Size;
                if (_scale <= 0)
                    _scale = Mathf.Epsilon;

                _scale /= 2;

                Vector2 normalizedSize = GetInputNormalized();
                Vector2 touchPos = _drawArea.GetTransformedMouse(_drawArea.ToScreenSpace(_currentTouch.Position));
                Vector2 firstInitScreenPos = _drawArea.GetTransformedMouse(_drawArea.ToScreenSpace(_currentTouch.StartPosition));
                float scaleX = Mathf.Clamp(Remap(Mathf.Abs(touchPos.x - firstInitScreenPos.x), 0, 1, Deadzone, 1), 0, _scale) / _scale;
                float scaleY = Mathf.Clamp(Remap(Mathf.Abs(touchPos.y - firstInitScreenPos.y), 0, 1, Deadzone, 1), 0, _scale) / _scale;
                return new Vector2(normalizedSize.x * scaleX, normalizedSize.y * -scaleY);
            }
            else
                return Vector2.zero;
        }

        public float Remap(float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }
    }

    [System.Serializable]
    public class TouchButton : TouchDevice
    {
        protected override void OnUpdate(float deltatime)
        {
            if (IsDown)
            {
                _topPosition = _bottomPosition = _drawArea.GetTransformedMouse(_drawArea.ToScreenSpace(_currentTouch.StartPosition));
            }
            else if(!IsDown)
            {
                LerpBackToIdle();// _topPosition = _bottomPosition = new Vector2(ActiveRegion.x + (ActiveRegion.width * ActiveRegionOffset.x), ActiveRegion.y + (ActiveRegion.height * ActiveRegionOffset.y));
            }
        }
    }

    [System.Serializable]
    public class TouchDevice
    {
        public string DeviceID = string.Empty;
        public Rect ActiveRegion = new Rect(0.25f, 0.25f, 0.5f,0.5f);
        public Vector2 ActiveRegionOffset = new Vector2(0.5f,0.5f);
        public TouchStyle Style = new TouchStyle();

        public bool WasDown
        { get { return _wasDown; } }
        public bool WasUp
        { get { return _wasUp; } }
        public bool IsDown
        { get { return _isDown; } }

        private bool _wasDown, _wasUp, _isDown;
        private float _animationLerpTime;
        private float _timeDown;
        private float _timeUp;

        [System.NonSerialized]
        protected Vector2 _bottomPosition;
        [System.NonSerialized]
        protected Vector2 _topPosition;
        [System.NonSerialized]
        protected TouchData _currentTouch;
        [System.NonSerialized]
        protected Vector2 _lastTouchStartPosition;
        [System.NonSerialized]
        protected DrawArea _drawArea; 

        public void SetDrawArea(DrawArea drawArea)
        {
            if (_drawArea == null)
                Init();

            _drawArea = drawArea;
        }

        public void Init()
        {
            _lastTouchStartPosition = _topPosition = _bottomPosition = GetIdlePosition();
            _timeUp = Style.ReturnOnReleaseTime;
        }

        public void UpdateDevice(float deltatime) 
        {
            if (_drawArea == null)
                return;

            CheckTouch(deltatime);
            UpdateAnimationLerpTime(deltatime);
            OnUpdate(deltatime);
        }

        public void Render()
        {
            if (_drawArea == null)
                return;

            TouchStyleDefinition bottomLerpedStyle = IsDown ? Style.GetBottomPressedStyleAtTime(_animationLerpTime) : Style.GetBottomReleasedStyleAtTime(_animationLerpTime);
            TouchStyleDefinition topLerpedStyle = IsDown ? Style.GetTopPressedStyleAtTime(_animationLerpTime) : Style.GetTopReleasedStyleAtTime(_animationLerpTime);
        
            _drawArea.DrawQuad(_bottomPosition, bottomLerpedStyle);

            _drawArea.DrawQuad(_topPosition, topLerpedStyle);
        }

        public bool AddTouch(TouchData touch)
        {
            if (_drawArea == null)
                return false;

            if (!HasValidTouch())
            {
                Vector2 correctPosition = _drawArea.GetTransformedMouse(_drawArea.ToScreenSpace(touch.Position));
                if (ActiveRegion.Contains(correctPosition))
                {
                    _currentTouch = touch;
                    return true;
                }
            }

            return false;
        }

        private void CheckTouch(float deltatime)
        {
            if (HasValidTouch())
            {
                if (WasDown)
                {
                    _wasDown = false;
                }
                else if (!IsDown)
                {
                    _isDown = true;
                    _wasDown = true;
                    _lastTouchStartPosition = _currentTouch.StartPosition;
                }
            }
            else
            {
                if (IsDown)
                {
                    _isDown = false;
                    _wasDown = false;
                    _wasUp = true;
                }
                else if (WasUp)
                {
                    _wasUp = false;
                }
            }
        }

        private void UpdateAnimationLerpTime(float deltatime)
        {
            if (HasValidTouch())
            {
                _animationLerpTime += deltatime;

                _timeUp = 0;
                _timeDown += deltatime; 
            } 
            else
            {
                _animationLerpTime -= deltatime;

                _timeUp += deltatime;
                _timeDown = 0;
            }

            _animationLerpTime = Mathf.Clamp(_animationLerpTime, 0, Style.AnimationDuration);
        }
        private bool HasValidTouch()
        {
            bool valid = _currentTouch != null && _currentTouch.Phase != TouchPhase.Canceled && _currentTouch.Phase != TouchPhase.Ended;
            if (!valid)
                _currentTouch = null;

            return valid;
        }
        protected Vector2 GetIdlePosition()
        {
            return new Vector2(ActiveRegion.x + (ActiveRegion.width * ActiveRegionOffset.x), ActiveRegion.y + (ActiveRegion.height * ActiveRegionOffset.y));
        }
        protected void LerpBackToIdle()
        {
            if (Style.ReturnOnRelease)
            {
                Vector2 lastTouchStartPos = _drawArea.GetTransformedMouse(_drawArea.ToScreenSpace(_lastTouchStartPosition));
                Vector2 idlePos = GetIdlePosition();
                float returnTime =  (_timeUp / (Mathf.Clamp01(Vector2.Distance(lastTouchStartPos,idlePos)) * Style.ReturnOnReleaseTime));
                _topPosition = _bottomPosition = Vector2.Lerp(lastTouchStartPos, idlePos, Style.ReturnAnimationCurve.Evaluate(returnTime));
            }
            else
            {
                _topPosition = _bottomPosition;
            }
        }
        protected virtual void OnUpdate(float deltatime) { }
        protected virtual void TouchBegin() { }
        protected virtual void TouchEnd() { }
        protected virtual void TouchStay() { }
    }

    [System.Serializable]
    public class TouchStyle
    {
        public TouchStyleDefinition BottomPressed = new TouchStyleDefinition();
        public TouchStyleDefinition BottomReleased = new TouchStyleDefinition();
        public TouchStyleDefinition TopPressed = new TouchStyleDefinition();
        public TouchStyleDefinition TopReleased = new TouchStyleDefinition();

        public float AnimationDuration = 1;
        public AnimationCurve AnimationCurve = new AnimationCurve(new Keyframe(0,0),new Keyframe(1,1));

        public bool ReturnOnRelease = true;
        public float ReturnOnReleaseTime = 1;
        public AnimationCurve ReturnAnimationCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));

        public TouchStyleDefinition GetTopPressedStyleAtTime(float t)
        {
            t = t / AnimationDuration;
            return LerpStyles(TopReleased, TopPressed, t);
        }

        public TouchStyleDefinition GetTopReleasedStyleAtTime(float t)
        {
            t = 1-(t / AnimationDuration);
            return LerpStyles(TopPressed, TopReleased, t);
        }

        public TouchStyleDefinition GetBottomPressedStyleAtTime(float t)
        {
            t = t / AnimationDuration;
            return LerpStyles(BottomReleased, BottomPressed, t);
        }

        public TouchStyleDefinition GetBottomReleasedStyleAtTime(float t)
        {
            t = 1 - (t / AnimationDuration);
            return LerpStyles(BottomPressed, BottomReleased, t);
        }

        private TouchStyleDefinition LerpStyles(TouchStyleDefinition from, TouchStyleDefinition to, float t)
        {
            TouchStyleDefinition result = new TouchStyleDefinition();
            result.TouchMaterial = to.TouchMaterial;
            float sampledValue = AnimationCurve.Evaluate(t);
            result.Color = Color.Lerp(from.Color, to.Color, sampledValue);
            result.Size = Mathf.Lerp(from.Size, to.Size, sampledValue);

            return result;
        }
    }

    [System.Serializable]
    public class TouchStyleDefinition
    {
        public Material TouchMaterial = null;
        public Color Color = Color.white;
        public float Size = 0.15f;
    }
    [System.Serializable]
    public class TouchData
    {
        public TouchPhase Phase;
        public Vector2 StartPosition;
        public Vector2 Position;
    }

    public enum TouchKey
    {
        Mouse2 = -2,
        Mouse1,
        Touch0,
        Touch1,
        Touch2,
        Touch3,
        Touch4,
        Touch5,
        Touch6,
        Touch7,
        Touch8,
        Touch9,
        Touch10
    }

    public class DrawArea
        {
            public static readonly Rect ValidRange = new Rect(0, 0, 1, 1);
            private float _ratio;
            private Rect _drawRect;

            public Rect GetRect()
            {
                return _drawRect;
            }

            public float GetRatio()
            {
                return _ratio;
            }

            public void SetDrawRect(Rect rect)
            {
                _drawRect = rect;
                _ratio = _drawRect.width / _drawRect.height;
                _drawRect = ToScreenSpace(_drawRect);
            }

            public Vector2 ToScreenSpace(Vector2 targetVector)
            {
                return new Vector2(targetVector.x / (float)Screen.width, targetVector.y / (float)Screen.height);
            }

            public Rect ToScreenSpace(Rect targetRect)
            {
                return new Rect(targetRect.x / (float)Screen.width, targetRect.y / (float)Screen.height, targetRect.width / (float)Screen.width, targetRect.height / (float)Screen.height);
            }

            public Vector2 ScreenSpaceToDrawAreaSpace(Vector2 target)
            {
                return new Vector2(target.x * _drawRect.width, target.y * _drawRect.height);
            }

            public Vector2 DrawAreaSpaceSquare(float size)
            {
                Vector2 square = new Vector2(size * _drawRect.width, (size * _drawRect.width) / (_drawRect.width / _drawRect.height));
                square.y *= _ratio;
                return square;
            }

            public Vector2 GetTransformedMouse(Vector2 mouse)
            {
                mouse.x -= _drawRect.x;
                mouse.y -= _drawRect.y;
                return new Vector2(mouse.x / _drawRect.width, (mouse.y) / _drawRect.height);
            }

            public void DrawQuad(Vector2 position, TouchStyleDefinition style)
            {
                if (style == null || style.TouchMaterial == null)
                    return;

                Color revertColor = Color.white;
                Color revertTint = Color.white;
                if (style.TouchMaterial.HasProperty("_Color"))
                    revertColor = style.TouchMaterial.color;
                if (style.TouchMaterial.HasProperty("_TintColor"))
                    revertTint = style.TouchMaterial.GetColor("_TintColor");

                GL.PushMatrix();

                if (style.TouchMaterial.HasProperty("_Color"))
                    style.TouchMaterial.color = style.Color;
                if (style.TouchMaterial.HasProperty("_TintColor"))
                    style.TouchMaterial.SetColor("_TintColor", style.Color);

                style.TouchMaterial.SetPass(0);

                float drawRectxMinPixelSpace = Screen.width * _drawRect.xMin;
                float drawRectyMaxPixelSpace = Screen.height * _drawRect.yMax;
                float drawRectHeightPixelSpace = Screen.height * _drawRect.height;
                float drawRectWidthPixelSpace = Screen.width * _drawRect.width;
                GL.LoadPixelMatrix(0, 1, 1, 0);

                GL.Viewport(new Rect(drawRectxMinPixelSpace, (Screen.height - drawRectyMaxPixelSpace), drawRectWidthPixelSpace, drawRectHeightPixelSpace));

                float scaleX = Screen.width / (float)drawRectWidthPixelSpace;
                float scaleY = (Screen.height) / (float)drawRectHeightPixelSpace;

                GL.Begin(GL.QUADS);

                position.x -= _drawRect.xMin;
                position.y -= _drawRect.yMin;

                Vector2 size = DrawAreaSpaceSquare(style.Size);
                size.x *= scaleX;
                size.y *= scaleY;


                Rect square = new Rect(position.x - (size.x / 2), position.y - (size.y / 2), size.x, size.y);
            

                GL.TexCoord(new Vector3(0, 1, 0));
                GL.Vertex3(_drawRect.xMin + (square.xMin), _drawRect.yMin + (square.yMin), 0);
                GL.TexCoord(new Vector3(1, 1, 0));
                GL.Vertex3(_drawRect.xMin + (square.xMin) + (square.width), _drawRect.yMin + (square.yMin), 0);
                GL.TexCoord(new Vector3(1, 0, 0));
                GL.Vertex3(_drawRect.xMin + (square.xMin) + (square.width), _drawRect.yMin + (square.yMin) + (square.height), 0);
                GL.TexCoord(new Vector3(0, 0, 0));
                GL.Vertex3(_drawRect.xMin + (square.xMin), _drawRect.yMin + (square.yMin) + (square.height), 0);

                GL.End();

                GL.PopMatrix();

                if (style.TouchMaterial.HasProperty("_Color"))
                    style.TouchMaterial.color = revertColor;
                if (style.TouchMaterial.HasProperty("_TintColor"))
                    style.TouchMaterial.SetColor("_TintColor", revertTint);
            }
        }

}
