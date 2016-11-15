using UnityEngine;

public class DraggableControlsExample : MonoBehaviour {
	public FastControls myFastControlsInstance;

	void OnEnable() {
		AddOrRemoveEventHandlers(true);
	}

	void OnDisable() {
		AddOrRemoveEventHandlers(false);
	}

	void AddOrRemoveEventHandlers(bool add) {
		foreach(FastControls.Button button in myFastControlsInstance.buttons) {
			if(add) {
				button.OnButtonDragged += DragButton;
			}
			else {
				button.OnButtonDragged -= DragButton;
			}
		}

		foreach(FastControls.ThumbStick thumbStick in myFastControlsInstance.thumbSticks) {
			if(add) {
				thumbStick.OnStickDragged += DragThumbStick;
			}
			else {
				thumbStick.OnStickDragged -= DragThumbStick;
			}
		}
	}
	
	void DragButton (FastControls.ButtonEventArgs args) {
		args.eventSender.Move((int)args.touchDeltaPosition.x, (int)args.touchDeltaPosition.y);	
	}

	void DragThumbStick (FastControls.ThumbStickEventArgs args) {
		args.eventSender.Move((int)args.touchDeltaPosition.x, (int)args.touchDeltaPosition.y);	
	}
}
