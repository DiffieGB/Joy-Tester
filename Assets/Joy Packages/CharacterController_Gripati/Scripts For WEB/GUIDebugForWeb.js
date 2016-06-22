#pragma strict

public var joystickCircleToWriteOutput : JoystickCircleForWeb;
public var guiTextureTransformButtonA : Transform;
public var guiTextureTransformButtonB : Transform;
public var guiTextureTransformButtonC : Transform;
public var guiTextureTransformButtonD : Transform;

public var skinIPhone3 : GUISkin;
public var skinIPhone4 : GUISkin;
private var isIPhone4 : boolean = false;

function Awake(){
	isIPhone4 = (Screen.width > 500) ? true : false;
}

function OnGUI () {
	GUI.skin = isIPhone4?skinIPhone4:skinIPhone3;

 
 
	//GUI.Label (Rect (Screen.width * 0.02, Screen.height * 0.90, Screen.width * 0.5, Screen.height * 0.1), "Angle (in Degree) = " + joystickSingleToWriteOutput.outputAngleRad * Mathf.Rad2Deg);
	GUI.Label (Rect (Screen.width * 0.02, Screen.height * 0.90, Screen.width * 0.5, Screen.height * 0.1), String.Format("Angle (in Degree) =  {0:F2} ", joystickCircleToWriteOutput.outputAngleRad * Mathf.Rad2Deg));
	//GUI.Label (Rect (Screen.width * 0.45, Screen.height * 0.90, Screen.width * 0.5, Screen.height * 0.1), "Force = " + joystickSingleToWriteOutput.outputForce);
	GUI.Label (Rect (Screen.width * 0.45, Screen.height * 0.90, Screen.width * 0.5, Screen.height * 0.1), String.Format("Force =  {0:F2}", joystickCircleToWriteOutput.outputForce));
	
	
	if(guiTextureTransformButtonA){
		GUI.Label (Rect (Screen.width * 0.73, Screen.height * 0.50, Screen.width * 0.27, Screen.height * 0.05), "Button A = " + getStatusOfButton(guiTextureTransformButtonA));
	}
	if(guiTextureTransformButtonB){
		GUI.Label (Rect (Screen.width * 0.73, Screen.height * 0.45, Screen.width * 0.27, Screen.height * 0.05), "Button B = " + getStatusOfButton(guiTextureTransformButtonB));
	}
	if(guiTextureTransformButtonC){
		GUI.Label (Rect (Screen.width * 0.73, Screen.height * 0.40, Screen.width * 0.27, Screen.height * 0.05), "Button C = " + getStatusOfButton(guiTextureTransformButtonC));
	}
	if(guiTextureTransformButtonD){
		GUI.Label (Rect (Screen.width * 0.73, Screen.height * 0.35, Screen.width * 0.27, Screen.height * 0.05), "Button D = " + getStatusOfButton(guiTextureTransformButtonD));
	}
	
	if(GUI.Button(Rect(Screen.width * 0.90, Screen.height * 0.01, Screen.width * 0.09, Screen.height * 0.1), "EXIT")){
		Application.LoadLevel("0 DemoIntro WEB");
	}
}

function getStatusOfButton(guiTextureTransform : Transform) : String{
	var returnValue : String= "None";
	
	if(guiTextureTransform){
		var scriptControllerGui : ControllerGUIButton = guiTextureTransform.GetComponent (ControllerGUIButton);
		if(scriptControllerGui.isOverImage){
			returnValue = "Pressed";
		}
		else{
			returnValue = " - ";
		}
		
	}
	else{
		returnValue = "None";
	}
	
	return returnValue;
}
