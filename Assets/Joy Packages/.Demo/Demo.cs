//using UnityEngine;
//using System.Collections;
//using CodX.VJoy;

//public class Demo : MonoBehaviour {
//	public Transform cameraPivot;
//	public VirtualJoystick joy;
//	public GameObject ball;
//	public Transform ballSpawn;
	
//	public float moveSpeed = 500.0f;
	
//	private CharacterController controller;
//	private Transform cam;
//	private float lastBall = -1.5f,
//				ballInterval = 1.5f;
//	private bool showLabel = false;
//	private SwitchCallback swListener;
	
//	void Start(){
//		this.controller = GetComponent<CharacterController>();
//		this.cam = cameraPivot.FindChild("Camera");
//		this.swListener = new SwitchCallback(SwitchHandler);
//		this.joy.OnToggleChanged += this.swListener;
//	}
//	void OnDestroy(){
//		this.joy.OnToggleChanged -= this.swListener;
//	}
//	void Update () {
//		// Allow the player to exit the app
//		if(Input.GetKeyDown(KeyCode.Escape))	Application.Quit();
//		if(this.controller){	// Handling player movement
//			Vector3 moveDir;
//			if(this.cameraPivot)
//				moveDir = (joy.GetAxis("Move", VJAxis.Horizontal) * cameraPivot.right) + (joy.GetAxis("Move", VJAxis.Vertical) * cameraPivot.forward);
//			else
//				moveDir = new Vector3(joy.GetAxis("Move", VJAxis.Horizontal), 0, joy.GetAxis("Move", VJAxis.Vertical));
//			if(moveDir.sqrMagnitude > 0.15f){
//				this.controller.SimpleMove(moveDir * this.moveSpeed * Time.deltaTime);
//				transform.rotation = Quaternion.LookRotation(moveDir);
//			}
//		}
//		if(this.cameraPivot){	// Handling camera rotation
//			float tmp = joy.GetAxis("Cam", VJAxis.Horizontal);
//			if(Mathf.Abs(tmp) > 0.2f)	cameraPivot.RotateAroundLocal(cameraPivot.up, tmp * Time.deltaTime);
//			if(this.cam){	// Handling camera distance
//				tmp = joy.GetAxis("Cam", VJAxis.Vertical);
//				if(Mathf.Abs(tmp) > 0.2f)	cam.Translate(new Vector3(0, 0, tmp * Time.deltaTime * 50.0f), Space.Self);
//				// Clamp distance
//				Vector3 tmpPos = cam.localPosition;
//				if(tmpPos.z > -3.0f)	cam.localPosition = new Vector3(tmpPos.x, tmpPos.y, -3.0f);
//				if(tmpPos.z < -12.0f)	cam.localPosition = new Vector3(tmpPos.x, tmpPos.y, -12.0f);
//				if(tmpPos.y > 5.15f)	cam.localPosition = new Vector3(tmpPos.x, 5.15f, tmpPos.z);
//				if(tmpPos.y < 2.4f)	cam.localPosition = new Vector3(tmpPos.x, 2.4f, tmpPos.z);
//			}
//		}
//		if(Time.time > this.lastBall + this.ballInterval && this.joy.GetButtonUp("Fire")){	// Handling ball spawning
//			this.lastBall = Time.time;
//			if(this.ball != null && this.ballSpawn != null){
//				Rigidbody r = Instantiate(this.ball, this.ballSpawn.position, this.ballSpawn.rotation) as Rigidbody;
//				r.AddForce(this.ballSpawn.forward * 100.0f * Time.deltaTime, ForceMode.Impulse);
//			}
//		}
//	}
//	void OnGUI(){
//		if(this.showLabel){
//			GUI.Label(new Rect(95, 57, Screen.width - ((Screen.width * 0.1f) + 256 + 20), 25), "Hide info");
//			GUI.Box(new Rect(0, (Screen.height * 0.05f) + 128, Screen.width, Screen.height - ((Screen.height * 0.05f) + 128)), "");
//			GUI.Label(new Rect(25, (Screen.height * 0.05f) + 128 + 25, Screen.width - 50, Screen.height - ((Screen.height * 0.05f) + 103)), "This demo has a control system with many flaws but it's not the goal. The goal is to illustrate how to use the VirtualJoystick plugin and it's largely demonstrated in really few lines of code. Basically you have to add the VirtualJoystick component to a GameObject, reference that component in the scripts you want and finally use the methods related to that reference to get information from user input. Each control can be referenced by its index or by its tag wich you can spicify where you define it. For further information please visit http://codx.altervista.org/ and contact me.");
//		}else{
//			GUI.Label(new Rect(95, 57, Screen.width - ((Screen.width * 0.1f) + 256 + 20), 25), "Show info");
//		}
//	}
//	void SwitchHandler(string tag, float time, bool state){
//		switch(tag){
//			case "swInfo":
//				this.showLabel = state;
//				break;
//			default:
//				break;
//		}
//	}
//}
