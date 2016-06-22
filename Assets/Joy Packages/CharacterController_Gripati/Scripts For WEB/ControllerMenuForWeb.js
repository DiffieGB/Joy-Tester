#pragma strict

private var marginFromLeftRatio : float = 0.10;
private var marginFromTopRatio : float = 0.09;
private var marginBetweenButtonRatio : float = 0.12;
private var widthOfButtonRatio : float = 0.8;
private var heightOfButtonRatio : float = 0.1;

private var isClickedOneOfThem : boolean = false;

public var skinIPhone3 : GUISkin;
public var skinIPhone4 : GUISkin;
private var isIPhone4 : boolean = false;

function Start () {
	isClickedOneOfThem = false;
	isIPhone4 = (Screen.width > 500) ? true: false;
	
}

function OnGUI(){
	GUI.skin = isIPhone4?skinIPhone4:skinIPhone3;
	if(isClickedOneOfThem){
		GUI.Label (Rect (Screen.width * 0.40, Screen.height * 0.45, Screen.width * 0.20, Screen.height * 0.1), "Loading...");
	}
	else{
		if (GUI.Button (new Rect (Screen.width * marginFromLeftRatio, Screen.height * marginFromTopRatio, Screen.width * widthOfButtonRatio, Screen.height * heightOfButtonRatio), "Static Joystick - Alwasy Visible - Linear - No Button")){
			isClickedOneOfThem = true;
			Application.LoadLevel("1 DemoSceneStaticJoystikNoExtraButton WEB");
		}
		
		if( GUI.Button (new Rect (Screen.width * marginFromLeftRatio, Screen.height * (marginFromTopRatio + marginBetweenButtonRatio), Screen.width * widthOfButtonRatio, Screen.height * heightOfButtonRatio), "Dynamic Joystick - Alwasy Visible - Linear - No Button")){
			isClickedOneOfThem = true;
			Application.LoadLevel("2 DemoSceneDynamicJoystikNoExtraButton WEB");
		}
		
		if(GUI.Button (new Rect (Screen.width * marginFromLeftRatio, Screen.height  * (marginFromTopRatio + 2 * marginBetweenButtonRatio), Screen.width * widthOfButtonRatio, Screen.height * heightOfButtonRatio), "Dynamic Joystick - Touch Visible - Cubic - No Button")){
			isClickedOneOfThem = true;
			Application.LoadLevel("3 DemoSceneDynamicJoystikTouchVisibleNoExtraButton WEB");
		}
		
		if(GUI.Button (new Rect (Screen.width * marginFromLeftRatio, Screen.height  * (marginFromTopRatio + 3 * marginBetweenButtonRatio), Screen.width * widthOfButtonRatio, Screen.height * heightOfButtonRatio), "Dynamic Joystick - Touch Visible - Exponential - No Button")){
			isClickedOneOfThem = true;
			Application.LoadLevel("4 DemoSceneDynamicJoystikTouchVisibleCubicNoExtraButtons WEB");
		}
		GUI.Label (new Rect (Screen.width * marginFromLeftRatio * 2, Screen.height  * (marginFromTopRatio + 5 * marginBetweenButtonRatio), Screen.width * widthOfButtonRatio, Screen.height * heightOfButtonRatio), " Sample Scenes for Web Player / PC");
		
	}

}