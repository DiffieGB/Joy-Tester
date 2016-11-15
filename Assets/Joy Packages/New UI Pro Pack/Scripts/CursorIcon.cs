
namespace NewUIProPack
{
	using UnityEngine;
	using UnityEngine.UI;
	using UnityEngine.Events;
	using UnityEngine.EventSystems;

	/// <summary>
	/// Place onto a UI Element to allow it to control the cursors image (hot spot).
	/// </summary>
	public class CursorIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		public Texture2D hoverSprite;
		public Vector2 hotSpot;
		public CursorMode cursorMode;

		void IPointerEnterHandler.OnPointerEnter (PointerEventData eventData)
		{
			Cursor.SetCursor(hoverSprite, hotSpot, cursorMode);
		}

		void IPointerExitHandler.OnPointerExit (PointerEventData eventData)
		{
			Cursor.SetCursor(null, Vector2.zero, cursorMode);
		}
	}
}