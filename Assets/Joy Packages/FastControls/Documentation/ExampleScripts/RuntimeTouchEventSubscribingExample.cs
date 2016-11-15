using UnityEngine;
using System.Collections;

public class RuntimeTouchEventSubscribingExample : MonoBehaviour {

	public void TouchDown(FastControls.TouchEventArgs args) {
		Debug.Log ("Touch down at " + args.touchPosition);
	}
	
	public void TouchDragged(FastControls.TouchEventArgs args) {
		Debug.Log ("Touch dragged to " + args.touchPosition + ", movement delta " + args.touchDeltaPosition);
	}
	
	public void TouchUp(FastControls.TouchEventArgs args) {
		Debug.Log ("Touch up at " + args.touchPosition);
	}

	void OnEnable() {
		FastControls.OnTouchUp += TouchUp;
		FastControls.OnTouchDown += TouchDown;
		FastControls.OnTouchDragged += TouchDragged;
	}
	
	void OnDisable() {
		FastControls.OnTouchUp -= TouchUp;
		FastControls.OnTouchDown -= TouchDown;
		FastControls.OnTouchDragged -= TouchDragged;
	}
}
