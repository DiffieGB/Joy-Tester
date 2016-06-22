using UnityEngine;

public class RuntimeButtonEventSubscribingExample : MonoBehaviour {

	public FastControlsRenderer myFastControlsInstance;

	public void ButtonDown(FastControls.ButtonEventArgs args) {
		Debug.Log ("Button down, touch position " + args.touchPosition);
	}
	
	public void ButtonUpInside(FastControls.ButtonEventArgs args) {
		Debug.Log ("Button up inside, touch position " + args.touchPosition);
	}
	
	public void ButtonUpOutside(FastControls.ButtonEventArgs args) {
		Debug.Log ("Button up outside, touch position " + args.touchPosition);
	}

	public void ButtonDragged(FastControls.ButtonEventArgs args) {
		Debug.Log ("Button dragged, touch position " + args.touchPosition + ", movement delta " + args.touchDeltaPosition);
	}
	
	void OnEnable() {
		FastControls.Button buttonA = myFastControlsInstance.GetButtonByName("ButtonA");
		
		if(buttonA != null) {
			buttonA.OnButtonDown += ButtonDown;
			buttonA.OnButtonUpInside += ButtonUpInside;
			buttonA.OnButtonUpOutside += ButtonUpOutside;
			buttonA.OnButtonDragged += ButtonDragged;
		}
	}

	void OnDisable() {
		FastControls.RemoveAllEventHandlersForObject(this);
	}
}
