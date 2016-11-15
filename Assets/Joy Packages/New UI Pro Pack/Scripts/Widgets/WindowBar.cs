
namespace NewUIProPack
{
	using UnityEngine;
	using UnityEngine.UI;
	using UnityEngine.Events;
	using UnityEngine.EventSystems;
	
	public class WindowBar : MonoBehaviour, IDragHandler
	{
		public void OnDrag (PointerEventData eventData)
		{
			transform.parent.position += (Vector3)eventData.delta;
		}
	}
}