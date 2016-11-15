namespace NewUIProPack
{
	using UnityEngine;
	using UnityEngine.UI;
	using UnityEngine.Events;
	using UnityEngine.EventSystems;
	
	public class Joystick : Selectable, IDragHandler, IBeginDragHandler
	{
		[System.Serializable]
		public class JoystickHandler : UnityEvent<Vector2> { }
		
		[SerializeField]
		[Tooltip("Should it move when dragged beyond it's limit.")]
		bool movable;
		[SerializeField] RectTransform thumbRect;
		[SerializeField] JoystickHandler joystickChanged;
		Vector2 startPosition;

		public void OnBeginDrag(PointerEventData eventData)
		{
			startPosition = this.rectTransform().anchoredPosition;
		}

		public void OnDrag(PointerEventData eventData)
		{
			float radius = this.rectTransform().sizeDelta.x * 0.5f;
			Vector2 difference = this.rectTransform().anchoredPosition - startPosition;
			Vector2 thumbPosition = difference + eventData.position - eventData.pressPosition;
			if (thumbPosition.magnitude > radius)
			{
				thumbPosition = thumbPosition.normalized * radius;
			}

			thumbRect.anchoredPosition = thumbPosition;
			joystickChanged.Invoke(thumbPosition / radius);
		}
		public override void OnPointerUp (PointerEventData eventData)
		{
			base.OnPointerUp (eventData);
			thumbRect.anchoredPosition = Vector3.zero;
			joystickChanged.Invoke(Vector2.zero);
		}
	}
}