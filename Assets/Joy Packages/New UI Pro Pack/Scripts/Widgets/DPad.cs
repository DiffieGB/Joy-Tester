namespace NewUIProPack
{
	using UnityEngine;
	using UnityEngine.UI;
	using UnityEngine.Events;
	using UnityEngine.EventSystems;
	
	/// <summary>
	/// D-Pad (Directional Pad) UI widget.
	/// </summary>
	public class DPad : Selectable, IDragHandler
	{
		[System.Serializable]
		public class DPadHandler : UnityEvent<Direction> { }

		public enum Direction
		{
			Up,
			Down,
			Left,
			Right,
			None
		}

		[SerializeField] DPadHandler onPadPress;
		Direction currentDirection;

		public override void OnPointerDown (PointerEventData eventData)
		{
			base.OnPointerDown (eventData);
			CalcDirection(eventData);
		}

		public void OnDrag (PointerEventData eventData)
		{
			CalcDirection(eventData);
		}
		public override void OnPointerUp (PointerEventData eventData)
		{
			base.OnPointerUp (eventData);
			currentDirection = Direction.None;
			onPadPress.Invoke(currentDirection);
		}

		void CalcDirection(PointerEventData eventData)
		{
			Vector3 direction = (eventData.position - (Vector2)transform.position).normalized;
			
			if (Vector3.Angle(direction, transform.up) < 45)
				currentDirection = Direction.Up;
			else if (Vector3.Angle(direction, transform.right) < 45)
				currentDirection = Direction.Right;
			else if (Vector3.Angle(direction, -transform.right) < 45)
				currentDirection = Direction.Left;
			else
				currentDirection = Direction.Down;
			onPadPress.Invoke(currentDirection);
		}
	}
}