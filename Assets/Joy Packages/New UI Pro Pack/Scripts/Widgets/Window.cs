
namespace NewUIProPack
{
	using UnityEngine;
	using UnityEngine.UI;
	using UnityEngine.Events;
	using UnityEngine.EventSystems;

	public class Window : Selectable
	{
		public Vector2 minSize;
		public override void OnPointerDown (PointerEventData eventData)
		{
			base.OnPointerDown (eventData);
			transform.SetAsLastSibling();
		}
	}
}