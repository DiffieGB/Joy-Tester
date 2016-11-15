using UnityEngine;

public class ThumbStickBackgroundHidingExample : MonoBehaviour {
	public FastControlsRenderer myFastControlsInstance;

	FastControls.ThumbStick leftThumbStick;
	Color thumbStickColor;

	void Start() {
		leftThumbStick = myFastControlsInstance.GetThumbStickByName("LeftThumbStick");

		if(leftThumbStick != null) {
			thumbStickColor = leftThumbStick.backgroundTint;

			SetThumbStickBackgroundAlpha(0.0f);
		}
	}

	void OnEnable() {
		if(leftThumbStick != null) {
			leftThumbStick.OnStickDown += UpdateThumbstickBackgroundVisibility;
			leftThumbStick.OnStickUp += UpdateThumbstickBackgroundVisibility;
		}
	}

	void OnDisable() {
		FastControls.RemoveAllEventHandlersForObject(this);
	}

	void UpdateThumbstickBackgroundVisibility(FastControls.ThumbStickEventArgs args) {
		SetThumbStickBackgroundAlpha((args.eventType == FastControls.ThumbStickEventType.OnStickDown) ? 1.0f : 0.0f);
	}

	void SetThumbStickBackgroundAlpha(float alpha) {
		if(leftThumbStick != null) {
			thumbStickColor.a = alpha;
			leftThumbStick.backgroundTint = thumbStickColor;
		}
	}
}
