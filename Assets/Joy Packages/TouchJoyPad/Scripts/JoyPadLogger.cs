using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class JoyPadLogger : MonoBehaviour {

	Text textJoypadLog;

	// Use this for initialization
	void Start () {
		textJoypadLog = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		textJoypadLog.text = "<Joypad Log>";

		if (TouchJoyPad.JoyKey) {
			textJoypadLog.text += "\nJoykey " + TouchJoyPad.JoyKeyAxis;
		}
		if (TouchJoyPad.JoyKeyDown) {
			textJoypadLog.text += "\nJoykey Down";
		}
		if (TouchJoyPad.JoyKeyUp) {
			textJoypadLog.text += "\nJoykey Up";
		}

		if (TouchJoyPad.TouchDrag) {
			textJoypadLog.text += "\nTouchdrag " + TouchJoyPad.TouchDragAxis;
		}
		if (TouchJoyPad.TouchDragDown) {
			textJoypadLog.text += "\nTouchdrag Down";
		}
		if (TouchJoyPad.TouchDragUp) {
			textJoypadLog.text += "\nTouchdrag Up";
		}

		if (TouchJoyPad.Button01) {
			textJoypadLog.text += "\nButton01 0n";
		}
		if (TouchJoyPad.Button01Down) {
			textJoypadLog.text += "\nButton01 Down";
		}
		if (TouchJoyPad.Button01Up) {
			textJoypadLog.text += "\nButton01 Up";
		}

		if (TouchJoyPad.Button02) {
			textJoypadLog.text += "\nButton02 0n";
		}
		if (TouchJoyPad.Button02Down) {
			textJoypadLog.text += "\nButton02 Down";
		}
		if (TouchJoyPad.Button02Up) {
			textJoypadLog.text += "\nButton02 Up";
		}

		if (TouchJoyPad.Button03) {
			textJoypadLog.text += "\nButton03 0n";
		}
		if (TouchJoyPad.Button03Down) {
			textJoypadLog.text += "\nButton03 Down";
		}
		if (TouchJoyPad.Button03Up) {
			textJoypadLog.text += "\nButton03 Up";
		}

		if (TouchJoyPad.Button04) {
			textJoypadLog.text += "\nButton04 0n";
		}
		if (TouchJoyPad.Button04Down) {
			textJoypadLog.text += "\nButton04 Down";
		}
		if (TouchJoyPad.Button04Up) {
			textJoypadLog.text += "\nButton04 Up";
		}
	}
}
