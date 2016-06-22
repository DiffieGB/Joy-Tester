/*
SetupScale.js

This script is used for auto-scaling while (remaining aspect ratio of the image) of GUITexture whose scale metrics are determined in iPhone or iPad on editor mode.

You should attach this script to the GUITexture and set the scale metric (note: you shouldn't give width or height in pixels! )
Then select your editor mode
Choose the fit options : Scalefit or Scalefill
See the affect after building and uploading your game to your device. Check in iPhone and iPad with same configurations! -> Aspect ratio will remain same!

*/


#pragma strict

@script RequireComponent( GUITexture )

//

enum AspectRatioExtended { NAN, iPhone, iPad} //In Landscape mode, scaling
enum ScaleModeExtended { NAN, ScaleToFit, ScaleToFill} 



public var aspectRatioTypeInEditorMode : AspectRatioExtended = AspectRatioExtended.NAN; //In editor mode, which is your editor screen ratio? 
private var aspectRatioTypeInCurrentDevice : AspectRatioExtended = AspectRatioExtended.NAN;

public var scalingMode : ScaleModeExtended = ScaleModeExtended.ScaleToFit;

function Awake(){
	//print("AWAKE Local Scale is " + transform.localScale + " ,Scaling Ratio : " + aspectRatioTypeInEditorMode + " , Screen W, H" + Screen.width + "x" + Screen.height);
	
	if(aspectRatioTypeInEditorMode != AspectRatioExtended.NAN){
		if(scalingMode != ScaleModeExtended.NAN){
			scaleTheTextureWithScreen();
		}
		else{
			//do nothing - no scaling mode selected
		}
	}
	else{
		Debug.LogError("Current Aspect Ratio should be set to use scaling!");
	}
}

function isAspectRatioSameWithEditor() : boolean{
	return true;
}

function scaleTheTextureWithScreen(){
	
	var aspectRatioInEditorMode : float;
	var aspectRatioInCurrentDevice : float = (Screen.width * 1.0) / Screen.height;
	var aspectRatioOfTransform : float = transform.localScale.x / transform.localScale.y;
	
	switch(aspectRatioTypeInEditorMode){
		case AspectRatioExtended.iPhone:
			aspectRatioInEditorMode = (480.0 / 320.0); 
			break;
		case AspectRatioExtended.iPad:
			aspectRatioInEditorMode = (1024.0 / 768.0); 
			break;
		default:
			aspectRatioInEditorMode = -1.0; 
	}
	
	//Debug.Log("Aspect Ratio in Editor Mode : " + aspectRatioInEditorMode + ", Current Device : " + aspectRatioInCurrentDevice + ", Transform : " + aspectRatioOfTransform);
	
	switch(aspectRatioInCurrentDevice){
		case (480.0 / 320.0):
		case (960.0 / 640.0):
			aspectRatioTypeInCurrentDevice = AspectRatioExtended.iPhone;
			break;
		case (1024.0 / 768.0):
		case (2048.0 / 1536.0):
			aspectRatioTypeInCurrentDevice = AspectRatioExtended.iPad;
			break;
		default:
			aspectRatioTypeInCurrentDevice = AspectRatioExtended.NAN;
			break;
	}
	
	Debug.Log("Aspect Ratio Type Current Device : " + aspectRatioTypeInCurrentDevice);
	
	if((aspectRatioTypeInEditorMode != aspectRatioTypeInCurrentDevice) && (aspectRatioTypeInEditorMode !=AspectRatioExtended.NAN && aspectRatioTypeInCurrentDevice != AspectRatioExtended.NAN)){
	
		if(aspectRatioInEditorMode > aspectRatioInCurrentDevice){ //If aspect ratio in editor mode is greater than current device
			switch(scalingMode){
				case ScaleModeExtended.ScaleToFit: 
					//transform.localScale.x = transform.localScale.x; // Same witdh scale
					transform.localScale.y =   transform.localScale.x * (aspectRatioInCurrentDevice) / (aspectRatioInEditorMode * aspectRatioOfTransform);
					break;
				case ScaleModeExtended.ScaleToFill: 
					transform.localScale.x =   transform.localScale.y * (aspectRatioInEditorMode * aspectRatioOfTransform) / (aspectRatioInCurrentDevice);
					//transform.localScale.y = transform.localScale.y; // Same witdh scale
					break;
				default: // do nothing in scaling
					break;
			
			}
		}
		else{ //If aspect ratio in editor mode is smaller than current device
		
		}
	
	}
	else{
		//do nothing, because aspect ratio values are equal with editor mode and device OR there is a NAN aspect ratio !
	}
	
	
}