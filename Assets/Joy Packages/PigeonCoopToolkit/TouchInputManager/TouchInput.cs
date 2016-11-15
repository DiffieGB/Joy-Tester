using PigeonCoopToolkit.TIM.Internal;
using UnityEngine;

namespace PigeonCoopToolkit.TIM
{
    public static class TouchInput 
    {
        private static TouchInputUpdater _instance;

        public static void SetAllInvisible()
        {
            if (PrerunCheck())
            {
                _instance.GetTIM().SetAllInvisible();
            }
        }
        public static void SetAllInactive()
        {
            if (PrerunCheck())
            {
                _instance.GetTIM().SetAllInactive();
            }
        }
        public static void SetAllVisible()
        {
            if (PrerunCheck())
            {
                _instance.GetTIM().SetAllVisible();
            }
        }
        public static void SetAllActive()
        {
            if (PrerunCheck())
            {
                _instance.GetTIM().SetAllActive();
            }
        }
        public static void SetVisible(string layerID)
        {
            if (PrerunCheck())
            {
                _instance.GetTIM().SetVisible(layerID);
            }
        }
        public static void SetInvisible(string layerID)
        {
            if (PrerunCheck())
            {
                _instance.GetTIM().SetInvisible(layerID);
            }
        }
        public static void SetActive(string layerID)
        {
            if (PrerunCheck())
            {
                _instance.GetTIM().SetActive(layerID);
            }
        }
        public static void SetInactive(string layerID)
        {
            if (PrerunCheck())
            {
                _instance.GetTIM().SetInactive(layerID);
            }
        }
        public static Vector2 JoystickValue(string layerID, string deviceID, bool normalized = false)
        {
            if (PrerunCheck())
            {
                return _instance.GetTIM().JoystickValue(layerID, deviceID, normalized);
            }

            return Vector2.zero;
        }
        public static bool GetJoystickUp(string layerID, string deviceID)
        {
            if (PrerunCheck())
            {
                return _instance.GetTIM().GetJoystickUp(layerID, deviceID);
            }

            return false;
        }
        public static bool GetJoystick(string layerID, string deviceID)
        {
            if (PrerunCheck())
            {
                return _instance.GetTIM().GetJoystick(layerID, deviceID);
            }

            return false;
        }
        public static bool GetJoystickDown(string layerID, string deviceID)
        {
            if (PrerunCheck())
            {
                return _instance.GetTIM().GetJoystickDown(layerID, deviceID);
            }

            return false;
        }
        public static bool GetButtonUp(string layerID, string deviceID)
        {
            if (PrerunCheck())
            {
                return _instance.GetTIM().GetButtonUp(layerID, deviceID);
            }

            return false;
        }
        public static bool GetButton(string layerID, string deviceID)
        {
            if (PrerunCheck())
            {
                return _instance.GetTIM().GetButton(layerID, deviceID);
            }

            return false;
        }
        public static bool GetButtonDown(string layerID, string deviceID)
        {
            if (PrerunCheck())
            {
                return _instance.GetTIM().GetButtonDown(layerID, deviceID);
            }

            return false;
        }
        static bool PrerunCheck()
        {
            if (Application.isPlaying == false)
                return false;

            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<TouchInputUpdater>();
                if(_instance == null)
                {
                    _instance = (new GameObject("Touch Input Updater")).AddComponent<TouchInputUpdater>();
                }
            }

            return true;
        }
    }
}