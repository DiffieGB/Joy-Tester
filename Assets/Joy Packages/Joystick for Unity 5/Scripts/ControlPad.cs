using UnityEngine;
using System.Collections;

namespace jfu5
{
    public class ControlPad : MonoBehaviour
    {
        public TouchPad leftPad;
        public TouchPad rightPad;

        public float speed = 5f;
        public float speedRotate = 150f;

        void Update()
        {
            transform.Translate(0f, leftPad.virtualAxes.y * Time.deltaTime * speed, 0f);
            transform.Rotate(0f, 0f, -rightPad.virtualAxes.x * Time.deltaTime * speedRotate);
        }

    }
}