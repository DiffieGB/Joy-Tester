using UnityEngine;
using System.Collections;

public class ThumbStickPollingExample : MonoBehaviour {

	public FastControlsRenderer myFastControlsInstance;

	FastControls.ThumbStick leftThumbStick = null;
	
	void Start() {
		leftThumbStick = myFastControlsInstance.GetThumbStickByName("LeftThumbStick");
	}
	
	void Update() {
		if(leftThumbStick != null) {
			if(leftThumbStick.isDown) {
				Debug.Log ("Thumb stick down, direction is " + leftThumbStick.direction + " and magnitude " + leftThumbStick.magnitude);
			}
		}
	}
}
