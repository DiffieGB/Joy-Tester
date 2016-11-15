using UnityEngine;
using System.Collections;

[AddComponentMenu("V&R Controller/Example/INPlayerBasic")]
public class INPlayerBasic : MonoBehaviour {
	public INController inCon;
	public float speed = 1f;
	Vector3 preDir;
	CharacterController cCon;

	void Start () {
		cCon = gameObject.GetComponent<CharacterController>();
		Time.timeScale = 1; 
	}

	void Update () {
		//moving charactor
		if (!(Time.timeScale == 0))
			cCon.Move(inCon.GetAxisVector3() * speed * Time.smoothDeltaTime*10);
		//adjust direction
		if (inCon.GetAxisVector3() != Vector3.zero && !(Time.timeScale == 0)){
			transform.LookAt(Vector3.Lerp(inCon.GetAxisVector3() * 10000 , preDir, 0.005f));
			preDir = inCon.GetAxisVector3() * 10000;
		}
		//button1 first press
		if (inCon.buttonState[0] == INController.BUTTON_STATE.PRESS){
			print("pressed button1");
		}
		//button1 go on pressed
		else if (inCon.buttonState[0] == INController.BUTTON_STATE.PRESSING){
		//	print("pressing button1");
		}
		//button1 end pressed
		else if (inCon.buttonState[0] == INController.BUTTON_STATE.UP){
		//	print("unpressed button1");
		}

		//button2 first press
		if (inCon.buttonState[1] == INController.BUTTON_STATE.PRESS){
			print ("pressed button2");
		}
		//button2 go on pressed
		else if (inCon.buttonState[1] == INController.BUTTON_STATE.PRESSING){
		//	print ("pressed button2");
		}
		//button2 end pressed
		else if (inCon.buttonState[1] == INController.BUTTON_STATE.UP){
		//	print("unpressed button2");
		}
	}
}
