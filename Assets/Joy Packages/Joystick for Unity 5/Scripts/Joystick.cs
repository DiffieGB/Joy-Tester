using UnityEngine;
using UnityEngine.EventSystems;

namespace jfu5
{
    public class Joystick : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IDragHandler
    {
        public Vector2 virtualAxes;

        [Range(0, 200)]
        public int joystickZone;

        private Vector2 startPosition;
        private Vector2 endPosition;

        public enum JoystickType
        {
            square,
            circle
        }

        public JoystickType JoystickZoneType;

        void Start()
        {
            startPosition = new Vector2(transform.position.x, transform.position.y);
        }

        public void OnDrag(PointerEventData data)
        {
            if (JoystickZoneType == JoystickType.square)
            {
                int deltaPositionX = (int)(data.position.x - startPosition.x);
                deltaPositionX = Mathf.Clamp(deltaPositionX, -joystickZone, joystickZone);

                int deltaPositionY = (int)(data.position.y - startPosition.y);
                deltaPositionY = Mathf.Clamp(deltaPositionY, -joystickZone, joystickZone);

                transform.position = new Vector3(startPosition.x + deltaPositionX, startPosition.y + deltaPositionY, 0f);
                virtualAxes = new Vector2((transform.position.x - startPosition.x) / joystickZone, (transform.position.y - startPosition.y) / joystickZone);
            }
            else
            {
                endPosition = (data.position - startPosition);
                endPosition = Vector2.ClampMagnitude(endPosition, joystickZone);

                transform.position = new Vector3(startPosition.x + endPosition.x, startPosition.y + endPosition.y, 0f);
                virtualAxes = new Vector2(endPosition.x / joystickZone, endPosition.y / joystickZone);
            }
        }

        public void OnPointerUp(PointerEventData data)
        {
            virtualAxes = Vector2.zero;
            transform.position = startPosition;
        }

        public void OnPointerDown(PointerEventData data)
        {

        }
    }
}