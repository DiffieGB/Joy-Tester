using UnityEngine;
using System.Collections;

public class Example : MonoBehaviour {
	public JoyStickController JoyStickControllerLeft;
	public JoyStickController JoyStickControllerRight;

	public float walkSpeed;
	public float RotionSpeed;

	void Update () 
	{
		Vector3 targetPos = new Vector3 (0, JoyStickControllerLeft.GetAxis ("Vertical"),0 );
		Vector3 targetRot = new Vector3 (0, 0, -JoyStickControllerRight.GetAxis ("Horizontal"));
		transform.Translate (targetPos * Time.deltaTime*walkSpeed);
		transform.Rotate (targetRot*Time.deltaTime*RotionSpeed);
	}
}
