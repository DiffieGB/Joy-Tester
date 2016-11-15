using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TouchJoyPad : MonoBehaviour {

	//Variables that can be referenced from outside
	static public float JoyKeyAttenuationValue = 0.3f, TouchDragAttenuationValue = 0.3f, JoyKeyDeadRate = 0.2f;
	static public Vector2 JoyKeyAxis, TouchDragAxis;
	static public bool JoyKey, JoyKeyDown, JoyKeyUp, TouchDrag, TouchDragDown, TouchDragUp,
	Button01, Button01Down, Button01Up, Button02, Button02Down, Button02Up, 
	Button03, Button03Down, Button03Up, Button04, Button04Down, Button04Up;

	//Variable for internal processing
	int ScreenWidth;
	bool PreJoyKey, PreTouchDrag, PreButton01, PreButton02, PreButton03, PreButton04;
	Vector2[] convertedTouchPos;
	Vector2 JoyKeyPos2D, DragArea2D, Button01Pos2D, Button02Pos2D, Button03Pos2D, Button04Pos2D;
	RectTransform rectJoyKey, rectDragArea, rectButton01, rectButton02, rectButton03, rectButton04;

	// Use this for initialization
	void Start () {
		//Detect the rect of Joykey and buttons
		rectJoyKey = GameObject.Find ("Image_JoyKey").GetComponent<RectTransform> ();
		rectDragArea = GameObject.Find ("Image_DragArea").GetComponent<RectTransform> ();
		rectButton01 = GameObject.Find ("Image_Button01").GetComponent<RectTransform> ();
		rectButton02 = GameObject.Find ("Image_Button02").GetComponent<RectTransform> ();
		rectButton03 = GameObject.Find ("Image_Button03").GetComponent<RectTransform> ();
		rectButton04 = GameObject.Find ("Image_Button04").GetComponent<RectTransform> ();

		JoyKeyPos2D = new Vector2 (rectJoyKey.position.x, rectJoyKey.position.y);
		DragArea2D = new Vector2 (rectDragArea.position.x, rectDragArea.position.y);
		Button01Pos2D = new Vector2 (rectButton01.position.x, rectButton01.position.y);
		Button02Pos2D = new Vector2 (rectButton02.position.x, rectButton02.position.y);
		Button03Pos2D = new Vector2 (rectButton03.position.x, rectButton03.position.y);
		Button04Pos2D = new Vector2 (rectButton04.position.x, rectButton04.position.y);

		ScreenWidth = Screen.width;
	}
	
	// Update is called once per frame
	void Update () {

		if (ScreenWidth != Screen.width) {
			JoyKeyPos2D = new Vector2 (rectJoyKey.position.x, rectJoyKey.position.y);
			DragArea2D = new Vector2 (rectDragArea.position.x, rectDragArea.position.y);
			Button01Pos2D = new Vector2 (rectButton01.position.x, rectButton01.position.y);
			Button02Pos2D = new Vector2 (rectButton02.position.x, rectButton02.position.y);
			Button03Pos2D = new Vector2 (rectButton03.position.x, rectButton03.position.y);
			Button04Pos2D = new Vector2 (rectButton04.position.x, rectButton04.position.y);

			ScreenWidth = Screen.width;
		}

		//Boolean value initialization for the state detection
		JoyKey = false;
		TouchDrag = false;
		Button01 = false;
		Button02 = false;
		Button03 = false;
		Button04 = false;

//		Detect touches
		if (Input.touchCount > 0) {
			convertedTouchPos = new Vector2[Input.touchCount];
			for (int i = 0; i < Input.touchCount; i++) {
				convertedTouchPos [i] = Input.GetTouch (i).position;

//			The depressed key detection by the touch position
				if (convertedTouchPos[i].x > JoyKeyPos2D.x - rectJoyKey.rect.width * 0.55f
					&& convertedTouchPos[i].x < JoyKeyPos2D.x + rectJoyKey.rect.width * 0.55f
					&& convertedTouchPos[i].y > JoyKeyPos2D.y - rectJoyKey.rect.height * 0.55f
					&& convertedTouchPos[i].y < JoyKeyPos2D.y + rectJoyKey.rect.height * 0.55f) {
//			Processing of the Joykey
					JoyKey = true;
					JoyKeyAxis = new Vector2 ((convertedTouchPos [i].x - JoyKeyPos2D.x) / (rectJoyKey.rect.width * 0.50f),
						(convertedTouchPos [i].y - JoyKeyPos2D.y) / (rectJoyKey.rect.width * 0.50f));
						
//			Dead position setting
					if (JoyKeyAxis.x < JoyKeyDeadRate && JoyKeyAxis.x > 0.0f) {
						JoyKeyAxis = new Vector2 (0.0f, JoyKeyAxis.y);
					}

					if (JoyKeyAxis.y < JoyKeyDeadRate && JoyKeyAxis.y > 0.0f) {
						JoyKeyAxis = new Vector2 (JoyKeyAxis.x, 0.0f);
					}
//			Joykey value cutoff
					if (JoyKeyAxis.x > 1.0f) {
						JoyKeyAxis.x = 1.0f;
					} 
					if (JoyKeyAxis.x < -1.0f) {
						JoyKeyAxis.x = -1.0f;
					} 
					if (JoyKeyAxis.y > 1.0f) {
						JoyKeyAxis.y = 1.0f;
					} 
					if (JoyKeyAxis.y < -1.0f) {
						JoyKeyAxis.y = -1.0f;
					} 
				} else if (convertedTouchPos[i].x > Button01Pos2D.x - rectButton01.rect.width * 0.50f
					&& convertedTouchPos[i].x < Button01Pos2D.x + rectButton01.rect.width * 0.50f
					&& convertedTouchPos[i].y > Button01Pos2D.y - rectButton01.rect.height * 0.50f
					&& convertedTouchPos[i].y < Button01Pos2D.y + rectButton01.rect.height * 0.50f) {
//			Processing of the Button01
					Button01 = true;
				} else if (convertedTouchPos[i].x > Button02Pos2D.x - rectButton02.rect.width * 0.50f
					&& convertedTouchPos[i].x < Button02Pos2D.x + rectButton02.rect.width * 0.50f
					&& convertedTouchPos[i].y > Button02Pos2D.y - rectButton02.rect.height * 0.50f
					&& convertedTouchPos[i].y < Button02Pos2D.y + rectButton02.rect.height * 0.50f) {
//			Processing of the Button02
					Button02 = true;
				} else if (convertedTouchPos[i].x > Button03Pos2D.x - rectButton03.rect.width * 0.50f
					&& convertedTouchPos[i].x < Button03Pos2D.x + rectButton03.rect.width * 0.50f
					&& convertedTouchPos[i].y > Button03Pos2D.y - rectButton03.rect.height * 0.50f
					&& convertedTouchPos[i].y < Button03Pos2D.y + rectButton03.rect.height * 0.50f) {
//			Processing of the Button03
					Button03 = true;
				} else if (convertedTouchPos[i].x > Button04Pos2D.x - rectButton04.rect.width * 0.5f
					&& convertedTouchPos[i].x < Button04Pos2D.x + rectButton04.rect.width * 0.5f
					&& convertedTouchPos[i].y > Button04Pos2D.y - rectButton04.rect.height * 0.5f
					&& convertedTouchPos[i].y < Button04Pos2D.y + rectButton04.rect.height * 0.5f) {
//			Processing of the Button04
					Button04 = true;
				} else if (convertedTouchPos[i].x > DragArea2D.x - rectDragArea.rect.width * 0.5f
					&& convertedTouchPos[i].x < DragArea2D.x + rectDragArea.rect.width * 0.5f
					&& convertedTouchPos[i].y > DragArea2D.y - rectDragArea.rect.height * 0.5f
					&& convertedTouchPos[i].y < DragArea2D.y + rectDragArea.rect.height * 0.5f) {
//			Processing of the other screen position touch and drag
					TouchDrag = true;
					TouchDragAxis = Input.GetTouch (i).deltaPosition;
				}
			}
		} 

//		Attenuation of the axis value
		if (!JoyKey) {
			JoyKeyAxis = Vector2.Lerp (JoyKeyAxis, Vector2.zero, JoyKeyAttenuationValue);
		}

		if (!TouchDrag) {
			TouchDragAxis = Vector2.Lerp (TouchDragAxis, Vector2.zero, TouchDragAttenuationValue);
		}

//		Detect each key up and down
		if (JoyKey != PreJoyKey) {
			if (JoyKey) {
				JoyKeyDown = true;
			} else {
				JoyKeyUp = true;
			}
		} else {
			JoyKeyDown = false;
			JoyKeyUp = false;
		}

		if (TouchDrag != PreTouchDrag) {
			if (TouchDrag) {
				TouchDragDown = true;
			} else {
				TouchDragUp = true;
			}
		} else {
			TouchDragDown = false;
			TouchDragUp = false;
		}

		if (Button01 != PreButton01) {
			if (Button01) {
				Button01Down = true;
			} else {
				Button01Up = true;
			}
		} else {
			Button01Down = false;
			Button01Up = false;
		}

		if (Button02 != PreButton02) {
			if (Button02) {
				Button02Down = true;
			} else {
				Button02Up = true;
			}
		} else {
			Button02Down = false;
			Button02Up = false;
		}

		if (Button03 != PreButton03) {
			if (Button03) {
				Button03Down = true;
			} else {
				Button03Up = true;
			}
		} else {
			Button03Down = false;
			Button03Up = false;
		}

		if (Button04 != PreButton04) {
			if (Button04) {
				Button04Down = true;
			} else {
				Button04Up = true;
			}
		} else {
			Button04Down = false;
			Button04Up = false;
		}

//		To record the state value of this frame
		PreJoyKey = JoyKey;
		PreTouchDrag = TouchDrag;
		PreButton01 = Button01;
		PreButton02 = Button02;
		PreButton03 = Button03;
		PreButton04 = Button04;
	}
}
