//////////////////////////////////////////////////////////////
// JoystickSingle.js
// Circle Joystick For Web Platforms
//
// Joystick creates a movable joystick (via GUITexture) that 
// handles touch input and gives output as angles and power.
// The Angle gives the direction of joytick finger thumb while
// power gives the amount of [0.0,1.0] power that user makes.
// 
// support@gripati.com
//////////////////////////////////////////////////////////////

//enum OutputForceType { Linear, Cubic, Exponential} //Comment-outted becuase of already defined in Joystick Circle Script !

private var thumbComponents : Component[]; 
private var guiThumbArea : GUITexture;
private var guiThumbFinger : GUITexture;

private var transformThumbArea : Transform;
private var transformThumbFinger : Transform;

public var isDynamicLocation : boolean = true; //is location is dynamic or static of thumb area and finger

public var touchOffsetRatio : Vector2 = new Vector2(0.0, 0.0);				// Offset to apply to touch input Ratio
private var touchOffset : Vector2;

public var areaAvailableRatio : Rect = new Rect(0, 0, 0.5, 1); // Boundary where joystick usage is available (used in Dynamic Location!). The numbers should be [0.0,1.0]. (if x=0 and y=0 the bottom left corner is the starting point for boundary)
private var areaAvailable : Rect;

public var isThumbAreaAlwaysVisible : boolean = false;
public var isThumbFingerAlwaysVisible : boolean = false;

public var timeFadeInThumbArea : float = 0.4; // Fade in time for thumb area. If it is 0.0 or lower, the area will be shown instantly (if isThumbAreaAlwaysVisible = false)
public var timeFadeOutThumbArea : float = 0.4; 
public var timeFadeInThumbFinger : float = 0.4; // Fade in time for thumb finger. If it is 0.0 or lower, the finger will be shown instantly (if isThumbFingerAlwaysVisible = false)
public var timeFadeOutThumbFinger : float = 0.4; 
public var timeReturnBackOrigin : float = 0.2; //Time for returnin back of thumb finger. After finger touch, thumb finger is appear instantly but may return to the origin when relasing the touch. If time is 0.0 then thumb finger is appearing origin when releasing the touch.

public var radiusThumbAreaRatio : float;
private var radiusThumbArea : float;

private var positionInitialThumbArea : Vector2; //it is the same with Thumb Finger (center of them should be same)

public var forceOutputType : OutputForceType = OutputForceType.Linear;
public var outputAngleRad : float = 0.0; // Angle between finger and center of circle 
public var outputForce : float = 0.0; // [0.0, 1.0]

private var lastFingerId : int = -1; // last finger ID using to identify latching

private var isResetPreviously : boolean = true; //initially, system has already reset.

private var alphaInitialThumbArea : float;
private var alphaInitialThumbFinger : float;


function Start()
{
	// Cache this component at startup instead of looking up every frame	
	thumbComponents = GetComponentsInChildren(GUITexture);
	guiThumbArea = thumbComponents[0];
	guiThumbFinger = thumbComponents[1];
	
	alphaInitialThumbArea = guiThumbArea.color.a;
	alphaInitialThumbFinger = guiThumbFinger.color.a;
	
	transformThumbArea = guiThumbArea.transform;
	transformThumbFinger = guiThumbFinger.transform;
	
	areaAvailable.x = areaAvailableRatio.x * Screen.width;
	areaAvailable.y = areaAvailableRatio.y * Screen.height;
	areaAvailable.width = areaAvailableRatio.width * Screen.width;
	areaAvailable.height = areaAvailableRatio.height * Screen.height;
	
	touchOffset = new Vector2(touchOffsetRatio.x * Screen.width, touchOffsetRatio.y * Screen.height);
	
	radiusThumbArea = radiusThumbAreaRatio * Screen.width;
	if(radiusThumbArea<=0){ //if no radius is specified, then thumb area width/2 is used for radius
		radiusThumbArea = (transformThumbArea.localScale.x * Screen.width)/2.0;
	}
	
	positionInitialThumbArea = new Vector2(transformThumbArea.position.x * Screen.width, transformThumbArea.position.y * Screen.height);
	
	if(!isThumbAreaAlwaysVisible){
		guiThumbArea.color.a = 0.0;
	}
	if(!isThumbFingerAlwaysVisible){
		guiThumbFinger.color.a = 0.0;
	}
}


function Disable()
{
	gameObject.active = false;
}

function Reset()
{
	// Release the finger control and set the joystick back to the default position (added in v1.1)
	ReturnBackThumbFingerToOrigin(timeReturnBackOrigin);
	//guiThumbFinger.pixelInset.x = 0;
	//guiThumbFinger.pixelInset.y = 0;
	
	outputForce = 0;
	outputAngleRad = 0;
	lastFingerId = -1;
	
	if(!isThumbAreaAlwaysVisible){
		FadeOutCoroutine(guiThumbArea, timeFadeOutThumbArea, 0.0);
	}
	if(!isThumbFingerAlwaysVisible){
		FadeOutCoroutine(guiThumbFinger, timeFadeOutThumbFinger, 0.0);
	}
	
}

function ReturnBackThumbFingerToOrigin(timeToReturnBack : float){
	
	var initialPositionX : float = guiThumbFinger.pixelInset.x;
	var initialPositionY : float = guiThumbFinger.pixelInset.y;

	var time : float = 0;
    while(time <= timeToReturnBack)
    {
        time += Time.deltaTime;
        var timePercent : float =  Mathf.Clamp(time / timeToReturnBack, 0.0, 1.0);
   		
        guiThumbFinger.pixelInset.x = initialPositionX + ( -initialPositionX * timePercent);
        guiThumbFinger.pixelInset.y = initialPositionY + ( -initialPositionY * timePercent);
		
        yield;
    }
}

function showThumbArea(centerPositionThumbArea : Vector2){
//show thumb area dynamically
	var positionX : float = centerPositionThumbArea.x / Screen.width;
	var positionY : float = centerPositionThumbArea.y / Screen.height;
	
	transformThumbArea.position.x = positionX;
	transformThumbArea.position.y = positionY;
	
	transformThumbFinger.position.x = positionX;
	transformThumbFinger.position.y = positionY;
	
	positionInitialThumbArea = centerPositionThumbArea;
}


function showThumbFinger(touchPositionFinger : Vector2){

	var distanceBetweenTouchAndThumbCenter : float = Vector2.Distance(touchPositionFinger, positionInitialThumbArea);
	
	outputAngleRad = Mathf.Atan2(touchPositionFinger.x-positionInitialThumbArea.x, touchPositionFinger.y-positionInitialThumbArea.y) ;
	//var angelOfTouch : float = outputAngleRad * Mathf.Rad2Deg;
	
	if(distanceBetweenTouchAndThumbCenter <= radiusThumbArea){ //Touch is inside area defined by radius = thumb area width
		if(forceOutputType == OutputForceType.Linear){
			outputForce = distanceBetweenTouchAndThumbCenter / radiusThumbArea; 
		}
		else if(forceOutputType == OutputForceType.Cubic){
			outputForce = (distanceBetweenTouchAndThumbCenter * distanceBetweenTouchAndThumbCenter) / (radiusThumbArea * radiusThumbArea); 
		}
		else if(forceOutputType == OutputForceType.Exponential){
			outputForce = (Mathf.Exp(distanceBetweenTouchAndThumbCenter / radiusThumbArea) - 1) / (Mathf.Exp(1) - 1) ; 
		}
		else{
			print("Error - Unknown output force Type " + forceOutputType);
		}
		
		
		guiThumbFinger.pixelInset.x = touchPositionFinger.x - positionInitialThumbArea.x;
		guiThumbFinger.pixelInset.y = touchPositionFinger.y - positionInitialThumbArea.y;
	}
	else{
		outputForce = 1.0;
		guiThumbFinger.pixelInset.x = (radiusThumbArea * Mathf.Sin(outputAngleRad));//- positionInitialThumbArea.x;
		guiThumbFinger.pixelInset.y = (radiusThumbArea * Mathf.Cos(outputAngleRad));//- positionInitialThumbArea.y;
	}
	
	//print("Position =" +touchPositionFinger + "Distance= " + distanceBetweenTouchAndThumbCenter + " , Angle = " + outputAngleArc + " , Power = "+outputPower);
}

function FadeInCoroutine(guiTextureForFadeIn : GUITexture, timeFadeIn : float, alphaAfterFadeIn : float)
{
	var alphaInitial : float = guiTextureForFadeIn.color.a;
	var time : float = 0;
    while(time <= timeFadeIn)
    {
        time += Time.deltaTime;
        var timePercent : float = time / timeFadeIn;
        //print("Time =" + time + " Alpha =" + guiTextureForFadeIn.color.a + " After " + alphaAfterFadeIn + " Value"  + alphaInitial + ((alphaAfterFadeIn - alphaInitial) * timePercent)+ " TimePercent Fade In: " + timePercent + " FadeIn Time = " + timeFadeIn);
        guiTextureForFadeIn.color.a = alphaInitial + ((alphaAfterFadeIn - alphaInitial) * timePercent);
        //yield;
        yield WaitForSeconds (0.01);
        //yield 0.1
        //yield new WaitForFixedUpdate ();
    }
    guiTextureForFadeIn.color.a = alphaAfterFadeIn;
}

function FadeOutCoroutine(guiTextureForFadeOut : GUITexture, timeFadeOut : float, alphaAfterFadeOut : float)
{
	var alphaInitial : float = guiTextureForFadeOut.color.a;
	var time : float = 0;
    while(time <= timeFadeOut)
    {
        time += Time.deltaTime;
        var timePercent : float = time / timeFadeOut;
        guiTextureForFadeOut.color.a = alphaInitial - ((alphaInitial - alphaAfterFadeOut) * timePercent);
        yield;
    }
    guiTextureForFadeOut.color.a = alphaAfterFadeOut;
}

private var touch : Touch;
private var touchPositionFinger : Vector2;
private var isHitOnRect : boolean;
private var hasTouchOnButton : boolean;

function Update()
{	
	isResetPreviously = false;
	if (Input.GetMouseButton(0)){
		
		hasTouchOnButton = false;
			
		touchPositionFinger = Input.mousePosition - touchOffset;
		isHitOnRect = areaAvailable.Contains(touchPositionFinger);
		
		// this is a new touch
		if ( isHitOnRect && ( lastFingerId == -1 ) ){
			hasTouchOnButton = true;
			lastFingerId = 0;
			
			if(!isThumbAreaAlwaysVisible){
				FadeInCoroutine(guiThumbArea, timeFadeInThumbArea, alphaInitialThumbArea);
			}
			if(!isThumbFingerAlwaysVisible){
				FadeInCoroutine(guiThumbFinger, timeFadeInThumbFinger, alphaInitialThumbFinger);
			}
			
			if(isDynamicLocation){ //show thumb area dynamically
				showThumbArea(touchPositionFinger);
			}
			else{
			 	showThumbFinger(touchPositionFinger);	
			}
		}
		else if ( isHitOnRect && ( lastFingerId == 0 ) ){
			hasTouchOnButton = true;
			showThumbFinger(touchPositionFinger);
		}
		else if (lastFingerId == 0){ //touch canceled or ended
			Reset();
			isResetPreviously = true;
		}
		else{ //do nothing, touch is related with other finger touch
			
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
