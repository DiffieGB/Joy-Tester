using UnityEngine;

public class RuntimeThumbStickEventSubscribingExample : MonoBehaviour {

	public FastControlsRenderer myFastControlsInstance;

	public void ThumbStickDown(FastControls.ThumbStickEventArgs args) {
		Debug.Log ("Thumb stick down, touch position " + args.touchPosition);
	}
	
	public void ThumbStickDragged(FastControls.ThumbStickEventArgs args) {
		Debug.Log ("Thumb stick dragged, touch position " + args.touchPosition + ", movement delta " + args.touchDeltaPosition);
	}
	
	public void ThumbStickUp(FastControls.ThumbStickEventArgs args) {
		Debug.Log ("Thumb stick up, touch position " + args.touchPosition);
	}

	public void ThumbStickValuesChanged(FastControls.ThumbStickEventArgs args) {
		Debug.Log ("Thumb stick values changed, direction is " + args.eventSender.direction + " and magnitude " + args.eventSender.magnitude);
	}

	void OnEnable() {
		FastControls.ThumbStick leftThumbStick = myFastControlsInstance.GetThumbStickByName("LeftThumbStick");
		
		if(leftThumbStick != null) {
			leftThumbStick.OnStickDown += ThumbStickDown;
			leftThumbStick.OnStickDragged += ThumbStickDragged;
			leftThumbStick.OnStickUp += ThumbStickUp;
			leftThumbStick.OnValuesChanged += ThumbStickValuesChanged;
		}
	}
	
	void OnDisable() {
		FastControls.RemoveAllEventHandlersForObject(this);
	}
}
