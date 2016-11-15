using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PigeonCoopToolkit.TIM.Internal
{
    public class TouchInputUpdater : MonoBehaviour
    {
        private TouchInputManager _touchInputManager;
        private List<TouchData> _activeTouches;
        private float _prevTime;

        void Awake()
        {
            _prevTime = Time.realtimeSinceStartup;

            _touchInputManager = Instantiate(Resources.Load("PCTK/TouchInputManager/TouchInputManagerData")) as TouchInputManager;
            _touchInputManager.Init();
            _touchInputManager.SetAllInactive();
            _touchInputManager.SetAllInvisible();

            DrawArea DA = new DrawArea();
            DA.SetDrawRect(new Rect(0, 0, Screen.width, Screen.height));
            _touchInputManager.SetDrawArea(DA);

            _activeTouches = new List<TouchData>();
            for (int i = 0; i < 10; i++)
            {
                _activeTouches.Add(new TouchData());
            }
        }

        void Update()
        {

#if  UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER

            if (Input.GetMouseButtonDown(0))
            {
                _activeTouches[0].Phase = TouchPhase.Began;

                Vector3 flippedPos = Input.mousePosition;
                flippedPos.y = Screen.height - flippedPos.y;

                _activeTouches[0].StartPosition = flippedPos;
                _activeTouches[0].Position = flippedPos;


                _touchInputManager.AddTouch(_activeTouches[0]);
            } 
            else if (Input.GetMouseButtonUp(0))
            {
                _activeTouches[0].Phase = TouchPhase.Ended;

                Vector3 flippedPos = Input.mousePosition;
                flippedPos.y = Screen.height - flippedPos.y;

                _activeTouches[0].Position = flippedPos;
            }
            else if (Input.GetMouseButton(0))
            {
                _activeTouches[0].Phase = TouchPhase.Moved;

                Vector3 flippedPos = Input.mousePosition;
                flippedPos.y = Screen.height - flippedPos.y;

                _activeTouches[0].Position = flippedPos;
            }

#else


            foreach (Touch touch in Input.touches)
            {
                if(touch.phase == TouchPhase.Began)
                {
                    _activeTouches[touch.fingerId].Phase = TouchPhase.Began;

                    Vector3 flippedPos = touch.position;
                    flippedPos.y = Screen.height - flippedPos.y;

                    _activeTouches[touch.fingerId].StartPosition = flippedPos;
                    _activeTouches[touch.fingerId].Position = flippedPos;

                    _touchInputManager.AddTouch(_activeTouches[touch.fingerId]);
                }
                if(touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                {
                    _activeTouches[touch.fingerId].Phase = TouchPhase.Moved;

                    Vector3 flippedPos = touch.position;
                    flippedPos.y = Screen.height - flippedPos.y;

                    _activeTouches[touch.fingerId].Position = flippedPos;
                }
                if(touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    _activeTouches[touch.fingerId].Phase = TouchPhase.Ended;

                    Vector3 flippedPos = touch.position;
                    flippedPos.y = Screen.height - flippedPos.y;

                    _activeTouches[touch.fingerId].Position = flippedPos;
                }

            }

#endif

            float delta = Time.realtimeSinceStartup - _prevTime;
            _prevTime = Time.realtimeSinceStartup;

            if (delta >= 0)
                _touchInputManager.UpdateManager(delta);
        }

        void OnGUI()
        {
            DrawArea DA = new DrawArea();
            DA.SetDrawRect(new Rect(0, 0, Screen.width, Screen.height));
            _touchInputManager.SetDrawArea(DA);

            _touchInputManager.Render();
        }

        public TouchInputManager GetTIM()
        {
            return _touchInputManager;
        }
    }
}
