//var scriptControllerGUI : ControllerGUI ;
var scriptControllerGUI : MonoBehaviour;
var functionTouchDown : String = null;
var functionTouchedDownAndContinue : String = null;
var functionTouchUpInside : String = null;
var functionTouchUpOutside : String = null;

private var imageNormal : Texture;
var imageOver : Texture;

var isOverImage : boolean = false;

private var lastFingerId : int = -1;
private var guiTextureCurrent : GUITexture;

function Start(){
	guiTextureCurrent = this.GetComponent.<GUITexture>();
	imageNormal = guiTextureCurrent.texture;
}

function OnGUI () {
	TouchControl();
}

function Reset()
{
	//nothing is touched
	lastFingerId = -1;
	if(imageNormal){ //Change back to normal image if there is already over image!
		this.GetComponent.<GUITexture>().texture = imageNormal; //normal image
		isOverImage = false;
	}
}

private var touch : Touch;
private var touchPositionFinger : Vector2;
private var isHitOnGui : boolean;
private var hasTouchOnGui: boolean;
private var isResetPreviously : boolean = true; //initially, system has already reset.

function TouchControl(){
	var touchCount : int = Input.touchCount;
	if ( touchCount > 0 ){
		isResetPreviously = false;
		hasTouchOnGui = false;
		for(var touchIndex:int = 0; touchIndex < touchCount; touchIndex++){
				touch = Input.GetTouch(touchIndex);			
				touchPositionFinger = touch.position;
				isHitOnGui = guiTextureCurrent.HitTest(touchPositionFinger);
				
				if( isHitOnGui && touch.phase == TouchPhase.Began && ( lastFingerId == -1 ) ){ 	 //Started Touch
					lastFingerId = touch.fingerId;
					hasTouchOnGui = true;
			
					if(functionTouchDown){
						scriptControllerGUI.SendMessage(functionTouchDown);
					}
					
					if(imageOver){ //over image
						this.GetComponent.<GUITexture>().texture = imageOver; //over image
						isOverImage = true;
					}
				}
				else if ( isHitOnGui && (touch.phase == TouchPhase.Began ||touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary) && ( lastFingerId == touch.fingerId ) ){ //Touched Previously and still inside the button
					hasTouchOnGui = true;
					if(functionTouchedDownAndContinue){  
						scriptControllerGUI.SendMessage(functionTouchedDownAndContinue);
					}
				}
				else if ( !isHitOnGui && (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary) && ( lastFingerId == touch.fingerId ) ){ //Touched Previously and now touch is outside the button
					Reset();
					isResetPreviously = true;
					if(functionTouchUpOutside){
						scriptControllerGUI.SendMessage(functionTouchUpOutside);
					}
					/* If you would like to use button after going outside, uncomment this part and delete other statements in this scope
					if(isOverImage && imageNormal){ // did not change over image to normal
						this.guiTexture.texture = imageNormal; //normal image
						isOverImage = false;
					}
					else{	// changed before the over image to normal - do nothing
						// do nothing
					}
					*/
				}
				else if ( isHitOnGui &&  (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) && ( lastFingerId == touch.fingerId ) ){ //Touch Up Inside
					Reset();
					isResetPreviously = true;
					if(functionTouchUpInside){
						scriptControllerGUI.SendMessage(functionTouchUpInside);
					}
					
				}
				else if ( !isHitOnGui &&  (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) && ( lastFingerId == touch.fingerId ) ){ //Touch Up Outside
					Reset();
					isResetPreviously = true;
					if(functionTouchUpOutside){
						scriptControllerGUI.SendMessage(functionTouchUpOutside);
					}
					
				}
				else{ // There is a touch but not on that button or Gui element
					
				}		
		}
			
		if(!hasTouchOnGui && !isResetPreviously){ // If there isn't touch or there was touch but not with given fingerid then we release the touched button 
			Reset();
			isResetPreviously = true;
		}
		else{ //There is touch on this button
			//do nothing
		}
	}
	else if(!isResetPreviously) //There is no touch, so we reset the button config (for performence optimization it is done once for each touch by flag isResetPreviously)
	{
		Reset();
		isResetPreviously = true;
	}
	else{
		//do nothing -> Already Reset
	}
}