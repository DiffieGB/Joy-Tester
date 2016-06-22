using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DigitalJoyStick : MonoBehaviour
{
    [SerializeField]
    bool useCircularBounds;
    [SerializeField]
    RectTransform attachBounds;
    [SerializeField]
    RectTransform reachBounds;
    [SerializeField]
    RectTransform joy;
    [SerializeField]
    bool rotateJoy = false;
    [SerializeField]
    float maxRotation = 45;
    [SerializeField]
    bool translateJoy = true;
    [SerializeField]
    AnimationCurve response = AnimationCurve.Linear(0, 0, 1, 1);
    [SerializeField]
    Vector2 virtualAxis;
    Vector2 realAxis;
    Vector2 offset;
    Vector3 joyStartRotation;

    int currentTouchId = -1;
    Touch CurrentTouch { 
        get {
            foreach (Touch touch in Input.touches)
            {
                if (touch.fingerId == currentTouchId)
                {
                    return touch;
                }
            }

            return Input.GetTouch(-1);
        }
        set { currentTouchId = value.fingerId; }
    }

    bool mouseActive =
        Application.platform != RuntimePlatform.Android &&
        Application.platform != RuntimePlatform.IPhonePlayer;
    bool mouseLatched = false;

    public void Start() 
    {
        joyStartRotation = joy.transform.localEulerAngles;
    }

    public void Update()
    {
        HandleTouchInput();
        HandleMouseInput();

        UpdateTransform();
    }

    void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            if (!TouchLatched)
            {
                foreach (Touch touch in Input.touches)
                {
                    if (this.Contains(touch.position))
                    {
                        LatchTouch(touch);
                    }
                }
            }
            else
            {
                if (CurrentTouch.phase == TouchPhase.Ended || CurrentTouch.phase == TouchPhase.Canceled)
                {
                    ReleaseTouch();
                }
            }
        }
    }

    void HandleMouseInput()
    {
        if (mouseActive)
        {
            if (!MouseLatched)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (this.Contains(Input.mousePosition))
                    {
                        LatchMouse();
                    }
                }
            }
            else
            {
                if (Input.GetMouseButtonUp(0))
                {
                    ReleaseMouse();
                }
            }
        }
    }

    void UpdateTransform()
    {
        if (Latched)
        {
            Vector2 localOffset = Vector2.zero;
            bool foundTouch = RectTransformUtility.ScreenPointToLocalPointInRectangle(
                reachBounds, LatchedPosition, GetComponentInParent<Canvas>().worldCamera, out localOffset);
            if (foundTouch)
            {
                offset = localOffset;
            }
        }
        else
        {
            offset = Vector2.zero;
        }

        Vector2 maxOffset = reachBounds.rect.size / 2;
                
        if (offset != Vector2.zero)
        {
            //float polyOffset = 0.0f;
            //int sides = 4;
            //float degreesPerSide = 360 / sides;
            //float radius_max = maxOffset.x;
            //float radius_min = radius_max / Mathf.Cos(Mathf.PI / sides);

            //float curentDistance = offset.magnitude;

            //if (offset.magnitude > radius_min)
            //{
            //    float angle = Vector2.Angle(Vector2.up, offset) + polyOffset * degreesPerSide;
            //    float lerp_factor = Mathf.Abs((angle % degreesPerSide - (degreesPerSide / 2)) / (degreesPerSide));
            //    curentDistance = Mathf.Lerp(radius_min, radius_max, lerp_factor);
            //}

            //offset *= curentDistance / offset.magnitude;

            if (useCircularBounds)
            {
                float maxDistance = maxOffset.x;
                float curentDistance = offset.magnitude;

                if (curentDistance > maxDistance)
                {
                    offset /= (curentDistance / maxDistance);
                }
            }
            else
            {
                offset.x = Mathf.Clamp(offset.x, -maxOffset.x, maxOffset.x);
                offset.y = Mathf.Clamp(offset.y, -maxOffset.y, maxOffset.y);
            }
        }

        realAxis.x = offset.x / maxOffset.x;
        realAxis.y = offset.y / maxOffset.y;
        
        float curentResponse = response.Evaluate(virtualAxis.magnitude);
        virtualAxis = realAxis * curentResponse;
        
        if (translateJoy)
        {
            joy.anchorMin = realAxis / 2;
            joy.anchorMax = realAxis / 2 + Vector2.one;
        }

        if (rotateJoy)
        {
            float xRotation = realAxis.y * maxRotation + joyStartRotation.x;
            float yRotation = -realAxis.x * maxRotation + joyStartRotation.y;

            joy.transform.localEulerAngles = new Vector3(xRotation, yRotation, joy.transform.rotation.z);
        }
    }

    public bool Contains(Vector2 position)
    {
        bool contains = false;

        if (useCircularBounds)
        {
            Vector2 mouseOffset = new Vector2();
            bool foundMouse = RectTransformUtility.ScreenPointToLocalPointInRectangle(
                attachBounds, position, GetComponentInParent<Canvas>().worldCamera, out mouseOffset);

            if (foundMouse && mouseOffset.magnitude < attachBounds.rect.size.x / 2)
            {
                contains = true;
            }
        }
        else
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(attachBounds, position, GetComponentInParent<Canvas>().worldCamera))
            {
                contains = true;
            }
        }

        return contains;
    }

    bool Latched
    {
        get { return MouseLatched || TouchLatched; }
    }

    Vector2 LatchedPosition
    {
        get 
        {
            Vector2 latchedPosition = Vector2.zero;

            if (TouchLatched)
            {
                latchedPosition = CurrentTouch.position;
            }
            else if (MouseLatched)
            {
                latchedPosition = Input.mousePosition;
            }

            return latchedPosition;
        }
    }

    public Vector2 VirtualAxis
    {
        get { return virtualAxis; }
    }

    bool TouchLatched
    {
        get { return currentTouchId != -1; }
    }

    void LatchTouch(Touch touch)
    {
        CurrentTouch = touch;
    }

    void ReleaseTouch()
    {
        currentTouchId = -1;
    }

    bool MouseLatched
    {
        get { return mouseLatched; }
    }

    void LatchMouse()
    {
        mouseLatched = true;
    }

    void ReleaseMouse()
    {
        mouseLatched = false;
    }
}
