
namespace NewUIProPack
{
	using UnityEngine;
	using UnityEngine.UI;
	using UnityEngine.Events;
	using UnityEngine.EventSystems;

	public class WindowSizer : Selectable, IDragHandler
	{
		public int minWidth = 300;
		public int minHeight = 300;

		public void OnDrag (PointerEventData eventData)
		{
			Vector2 sizeDelta = transform.parent.rectTransform().sizeDelta;
			sizeDelta += new Vector2(eventData.delta.x, -eventData.delta.y);
			if (sizeDelta.x < minWidth)
				sizeDelta.x = minWidth;
			if (sizeDelta.y < minHeight)
				sizeDelta.y = minHeight;
			transform.parent.rectTransform().sizeDelta = sizeDelta;
		}
	}
}