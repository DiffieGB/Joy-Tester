using UnityEngine;
using System.Collections;

[AddComponentMenu("V&R Controller/Controller/INController")]
public class INController : MonoBehaviour {
	public enum AXISCONT_TYPE{
		VIRTUAL,
		GAMEPAD,
		BOTH
	}
	public enum BUTTON_STATE{
		UP,
		PRESS,
		PRESSING
	}
	public AXISCONT_TYPE axisconType;
	Vector3 axis;
	public INButton[] virtualButton;
	public BUTTON_STATE[] buttonState;
	public VirtualJoystick vj;
	public GammingPad gp;

	void Start () {
		print ("number of buttons : " + virtualButton.Length);
		buttonState = new BUTTON_STATE[virtualButton.Length];
	}

	void Update () {
		if(axisconType == AXISCONT_TYPE.VIRTUAL){
			axis = vj.GetAxisVector3();
			for (int a = 0; a <virtualButton.Length;a++){
				
				if (virtualButton[a].buttonPhase == INButton.ButtonPhase.Down){
					//					print (virtualButton[a].buttonName + "button down");
					buttonState[a] = BUTTON_STATE.PRESS;
				}
				else if (virtualButton[a].buttonPhase == INButton.ButtonPhase.Stationary){
					//					print (virtualButton[a].buttonName + "button stationary");
					buttonState[a] = BUTTON_STATE.PRESSING;
				}
				else if (virtualButton[a].buttonPhase == INButton.ButtonPhase.Up){
					//					print ("button up");
					buttonState[a] = BUTTON_STATE.UP;
				}
			}

		}
		else if(axisconType == AXISCONT_TYPE.GAMEPAD){
			axis = gp.GetAxisVector3();
			for (int a = 0; a <virtualButton.Length;a++){
				
				if ((Input.GetButtonDown(virtualButton[a].buttonName))){
					//					print (virtualButton[a].buttonName + "button down");
					buttonState[a] = BUTTON_STATE.PRESS;
				}
				else if ((Input.GetButton(virtualButton[a].buttonName))){
					//					print (virtualButton[a].buttonName + "button stationary");
					buttonState[a] = BUTTON_STATE.PRESSING;
				}
				else if ((Input.GetButtonUp(virtualButton[a].buttonName))){
					//					print ("button up");
					buttonState[a] = BUTTON_STATE.UP;
				}
			}
		}
		else{
			axis = vj.GetAxisVector3() + gp.GetAxisVector3();
			for (int a = 0; a <virtualButton.Length;a++){

				if (virtualButton[a].buttonPhase == INButton.ButtonPhase.Down || (Input.GetButtonDown(virtualButton[a].buttonName))){
//					print (virtualButton[a].buttonName + "button down");
					buttonState[a] = BUTTON_STATE.PRESS;
				}
				else if (virtualButton[a].buttonPhase == INButton.ButtonPhase.Stationary || (Input.GetButton(virtualButton[a].buttonName))){
//					print (virtualButton[a].buttonName + "button stationary");
					buttonState[a] = BUTTON_STATE.PRESSING;
				}
				else if (virtualButton[a].buttonPhase == INButton.ButtonPhase.Up || (Input.GetButtonUp(virtualButton[a].buttonName))){
//					print ("button up");
					buttonState[a] = BUTTON_STATE.UP;
				}
			}
		}
	}

	public Vector3 GetAxisVector3(){
		return axis;
	}

}
