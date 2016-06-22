//////////////////////////////////////////////////////////////
// FollowTransform.js
// Penelope iPhone Tutorial
//
// FollowTransform will follow any assigned Transform and 
// optionally face the forward vector to match for the Transform
// where this script is attached.
//////////////////////////////////////////////////////////////

var targetTransform : Transform;		// Transform to follow
var faceForward : boolean = false;		// Match forward vector?
private var thisTransform : Transform;

private var offsetLocation : Vector3;

function Start()
{
	// Cache component lookup at startup instead of doing this every frame
	thisTransform = transform;
	offsetLocation = thisTransform.position;
	
}

function Update () 
{
	thisTransform.position = targetTransform.position + offsetLocation;
	
	if ( faceForward )
		thisTransform.forward = targetTransform.forward;
}