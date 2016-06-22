using UnityEngine;
using System.Collections;

public class SimpleFastControlsDemo_Character : MonoBehaviour {
	public FastControlsRenderer fastControlsRenderer;

	public float speed = 5.0f;
	public float jumpSpeed = 4.0f;
	public float gravity = 9.81f;
	
	private CharacterController mCharacterController;
	private Transform mTransform;

	private FastControls.ThumbStick mLeftThumbStick;
	private FastControls.ThumbStick mRightThumbStick;
	private FastControls.Button mButtonA;
	private FastControls.Button mButtonB;
	
	private Vector3 movingForce = Vector3.zero;

	void Awake() {
		// Add touch drag handler
		FastControls.OnTouchDragged += RotateCamera;

		mTransform = GetComponent<Transform>();
		mCharacterController = GetComponent<CharacterController>();

		// Get references to our thumb sticks and buttons
		if(fastControlsRenderer != null) {
			mLeftThumbStick = fastControlsRenderer.GetThumbStickByName("LeftThumbStick");
			mRightThumbStick = fastControlsRenderer.GetThumbStickByName("RightThumbStick");
			mButtonA = fastControlsRenderer.GetButtonByName("ButtonA");
			mButtonB = fastControlsRenderer.GetButtonByName("ButtonB");
		}
	}

	void Update() {
		if(mButtonA == null || mButtonB == null || mLeftThumbStick == null || mCharacterController == null) {
			return;
		}

		Quaternion screenMovementSpace = Quaternion.Euler(0, Camera.main.GetComponent<Transform>().eulerAngles.y, 0);
		Vector3 screenMovementForward = screenMovementSpace * Vector3.forward;
		Vector3 screenMovementRight = screenMovementSpace * Vector3.right;	

		if(mCharacterController.isGrounded) {
			// Use LeftThumbStick direction for steering and magnitude for speed
			movingForce = mLeftThumbStick.direction.x * screenMovementRight + mLeftThumbStick.direction.y * screenMovementForward;
			movingForce *= speed * mLeftThumbStick.magnitude;

			// Jump if ButtonA or ButtonB is down and we are grounded
			if(mButtonA.isDown || mButtonB.isDown) {
				movingForce.y = jumpSpeed;
			}
		}

		movingForce.y -= gravity * Time.deltaTime;

		mCharacterController.Move(movingForce * Time.deltaTime);
	}

	void OnTriggerEnter(Collider other) {
		if(mButtonA == null || mButtonB == null || mRightThumbStick == null) {
			return;
		}

		mRightThumbStick.hidden = !mRightThumbStick.hidden;
		mButtonA.hidden = !mButtonA.hidden;
		mButtonB.hidden = !mButtonB.hidden;
	}

	private void RotateCamera(FastControls.TouchEventArgs args) {
		Camera.main.GetComponent<Transform>().RotateAround(mTransform.position, Vector3.up, args.touchDeltaPosition.x);
	}
}
