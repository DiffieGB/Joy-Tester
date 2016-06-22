//////////////////////////////////////////////////////////////
// FollowTransformExt.js
// Penelope iPhone Tutorial - Extended
//
// You can control camera with joystick 
//
// FollowTransform will follow any assigned Transform and 
// optionally face the forward vector to match for the Transform
// where this script is attached.
//////////////////////////////////////////////////////////////

var targetTransform : Transform;		// Transform to follow
var faceForward : boolean = false;		// Match forward vector?
private var thisTransform : Transform;

private var offsetLocation : Vector3;

public var joystickToControl : JoystickCircle;
public var joystickSensitivity : Vector3 = Vector3.one;
public var joystickAngleLimits : Vector3 = Vector3(360, 60, 0);

function Start()
{
	// Cache component lookup at startup instead of doing this every frame
	thisTransform = transform;
	offsetLocation = thisTransform.position;
	
}

private var cameraRotation : Vector3 ;
function Update () 
{
	thisTransform.position = targetTransform.position + offsetLocation;
	
	thisTransform.Rotate(Mathf.Cos(joystickToControl.outputAngleRad) * joystickSensitivity.x * joystickToControl.outputForce  * Time.deltaTime, -1 * Mathf.Sin(joystickToControl.outputAngleRad) * joystickSensitivity.y * joystickToControl.outputForce  * Time.deltaTime, 0, Space.Self);
	
	thisTransform.rotation.eulerAngles.x =limitAngle(thisTransform.rotation.eulerAngles.x, joystickAngleLimits.x);
	thisTransform.rotation.eulerAngles.y =limitAngle(thisTransform.rotation.eulerAngles.y, joystickAngleLimits.y);
	thisTransform.rotation.eulerAngles.z = 0;
	
	if ( faceForward )
		thisTransform.forward = targetTransform.forward;
}

function limitAngle ( angleEulerValue : float, angleMinMax : float) : float{
	var returnValue : float = 0;
	if(angleEulerValue > angleMinMax && angleEulerValue < 180){
		returnValue= angleMinMax;
	}
	else if(angleEulerValue < 360 - angleMinMax && angleEulerValue > 180){
		returnValue= 360 - angleMinMax;
	}
	else{
		//inside limit
		returnValue = angleEulerValue;
	}
	return returnValue;
}

