//////////////////////////////////////////////////////////////
// CameraRelativeControlExt.js
// Penelope iPhone Tutorial - Extended
//
// This script is an edited and simplified version of CameraRelativeControl.js.
// You can download and check CameraRelativeControl.js script from Penelope tutarial. ( see : http://unity3d.com/support/resources/tutorials/penelope )
//////////////////////////////////////////////////////////////

// This script must be attached to a GameObject that has a CharacterController
@script RequireComponent( CharacterController )

var joystickCircle : JoystickCircle;

var cameraPivot : Transform;						// The transform used for camera rotation
var cameraTransform : Transform;					// The actual transform of the camera

var speed : float = 5;								// Ground speed
var jumpSpeed : float = 8;
var inAirMultiplier : float = 0.25; 				// Limiter for ground speed while jumping
var rotationSpeed : Vector2 = Vector2( 50, 25 );	// Camera rotation speed for each axis

private var thisTransform : Transform;
private var character : CharacterController;
private var animationController : AnimationControllerExt;
private var velocity : Vector3;						// Used for continuing momentum while in air

function Start()
{
	// Cache component lookup at startup instead of doing this every frame	
	thisTransform = GetComponent( Transform );
	character = GetComponent( CharacterController );	
	animationController = GetComponent( AnimationControllerExt );
	
	// Set the maximum speed, so that the animation speed adjustment works
	animationController.maxForwardSpeed = speed;
	
	// Move the character to the correct start position in the level, if one exists
	var spawn = GameObject.Find( "PlayerSpawn" );
	if ( spawn )
		thisTransform.position = spawn.transform.position;	
}

function FaceMovementDirection()
{	
	var horizontalVelocity : Vector3 = character.velocity;
	horizontalVelocity.y = 0; // Ignore vertical movement
	
	// If moving significantly in a new direction, point that character in that direction
	if ( horizontalVelocity.magnitude > 0.1 )
		thisTransform.forward = horizontalVelocity.normalized;
}

function OnEndGame()
{
	// Disable joystick when the game ends	
	joystickCircle.Disable();
	
	// Don't allow any more control changes when the game ends
	this.enabled = false;
}

function Update()
{


//print("Character Angle =" +Mathf.Sin(joystickCircle.outputAngleArc) + " - " + Mathf.Cos(joystickCircle.outputAngleArc)  + " Power = " + joystickCircle.outputPower);
	var movement = cameraTransform.TransformDirection( Vector3( Mathf.Sin(joystickCircle.outputAngleRad) , 0, Mathf.Cos(joystickCircle.outputAngleRad) ) );
	
	
	// We only want the camera-space horizontal direction
	movement.y = 0;
	movement.Normalize(); // Adjust magnitude after ignoring vertical movement
	
	// Let's use the largest component of the joystick position for the speed.
	movement *= speed * joystickCircle.outputForce;
	//movement *= speed * moveJoystick.outputPower;
	
	// Check for jump
	if ( !character.isGrounded )
	{	
		// Apply gravity to our velocity to diminish it over time
		velocity.y += Physics.gravity.y * Time.deltaTime;
		
		// Adjust additional movement while in-air
		movement.x *= inAirMultiplier;
		movement.z *= inAirMultiplier;
	}
	
	movement += velocity;
	movement += Physics.gravity;
	movement *= Time.deltaTime;
	
	// Actually move the character
	character.Move( movement );
	
	
	if ( character.isGrounded )
		// Remove any persistent velocity after landing
		velocity = Vector3.zero;
	
	// Face the character to match with where she is moving
	FaceMovementDirection();	
	
	
	// Scale joystick input with rotation speed
	var camRotation = new Vector2(Mathf.Sin(joystickCircle.outputAngleRad) , Mathf.Cos(joystickCircle.outputAngleRad));
	camRotation.x *= rotationSpeed.x;
	camRotation.y *= rotationSpeed.y;
	camRotation *= Time.deltaTime;
	
	// Rotate around the character horizontally in world, but use local space
	// for vertical rotation
	cameraPivot.Rotate( 0, camRotation.x, 0, Space.World );
	cameraPivot.Rotate( camRotation.y * -5, 0, 0 );
	
	
}