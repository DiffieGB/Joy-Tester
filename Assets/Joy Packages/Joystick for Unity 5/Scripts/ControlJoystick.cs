using UnityEngine;
using System.Collections;

namespace jfu5
{
    public class ControlJoystick : MonoBehaviour
    {

        public Joystick leftJoystick;
        public Joystick rightJoystick;

        public float speed = 5f;
        public float speedRotate = 150f;

        void Update()
        {
            transform.Translate(0f, leftJoystick.virtualAxes.y * Time.deltaTime * speed, 0f);
            transform.Rotate(0f, 0f, -rightJoystick.virtualAxes.x * Time.deltaTime * speedRotate);
        }
    }
}
