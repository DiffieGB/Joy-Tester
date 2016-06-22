#pragma strict

public var transformPenelope : Transform;

public var camera1FollowWithZoom : Camera;
public var camera2TopView : Camera;


private var characterControllerPenelope : CharacterController;

function Start () {
	characterControllerPenelope = transformPenelope.GetComponent(CharacterController);
	
	gameObjectCamera1FollowWithZoom = camera1FollowWithZoom.gameObject;
	gameObjectCamera2TopView = camera2TopView.gameObject;
}

function Update () {

}

function ButtonATouchedDown(){ //Simple Jump Button
	if(characterControllerPenelope.isGrounded){
		characterControllerPenelope.Move(Vector3(0, 3, 0));
	}
}

private var isCamera1 : boolean = true;
private var gameObjectCamera1FollowWithZoom : GameObject;
private var gameObjectCamera2TopView : GameObject;

function ButtonBTouchedDown(){ //Camera Change Button
	if(isCamera1){
		gameObjectCamera2TopView.active = false;
		gameObjectCamera1FollowWithZoom.active = true;
	}
	else{
		gameObjectCamera1FollowWithZoom.active = false;
		gameObjectCamera2TopView.active = true;
	}
	isCamera1 = !isCamera1;
}

function ButtonCTouchedDownAndContinue(){ //Move Penelope Backward
	characterControllerPenelope.Move(transformPenelope.forward * -0.01);
}


private var generatedSphere : GameObject;
private var scaleConstantStart : float = 0.1;
private var scaleConstantEnd : float = 0.5;
private var scalingSmoothness : float = 0.1;

function ButtonDTouchedDown(){ //Create Sphere when touched Down
	generatedSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
	generatedSphere.AddComponent(Rigidbody);
	generatedSphere.GetComponent.<Rigidbody>().useGravity = false;
    generatedSphere.transform.position = transformPenelope.position + transformPenelope.rotation * Vector3(0, 1.9, 0.5);
    generatedSphere.transform.localScale = Vector3(scaleConstantStart,scaleConstantStart,scaleConstantStart); //.(scaleConstant, scaleConstant, scaleConstant);
}

function ButtonDTouchedDownAndContinue(){ //Change Sphere scale
	generatedSphere.transform.position = transformPenelope.position + (transformPenelope.rotation * Vector3(0, 2.8, 0.5));
	
	generatedSphere.transform.localScale = Vector3.Lerp(generatedSphere.transform.localScale, Vector3(scaleConstantStart,scaleConstantStart,scaleConstantStart) + Vector3(scaleConstantEnd, scaleConstantEnd, scaleConstantEnd) * Mathf.PingPong(Time.time,scaleConstantEnd), scalingSmoothness);
}

function ButtonDRelease(){ //Release Sphere with force
	generatedSphere.GetComponent.<Rigidbody>().AddForce(transformPenelope.rotation * Vector3(0, 0, 15),ForceMode.Impulse);
}