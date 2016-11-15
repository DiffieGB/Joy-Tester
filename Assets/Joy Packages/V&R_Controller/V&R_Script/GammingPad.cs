using UnityEngine;
using System.Collections;

[AddComponentMenu("V&R Controller/Controller/GammingPad")]
public class GammingPad : MonoBehaviour {
	public enum AXIS_TYPE {
		XY,
		XZ,
		YZ
	}
	public AXIS_TYPE axisType;
	float vertical;
	float horizontal;
	[Range(0.0f,1.0f)]
	public float sensivity;

	void Start () {
	
	}

	void Update () {

	}

	//return gamming pad’s axis in the form of Vector2
	public Vector2 GetAxisVector2(){
		Vector2 axis = new Vector2();

		return axis;
	}

	//return gamming pad’s axis in the form of Vector3 (subject to the axisType value)
	public Vector3 GetAxisVector3(){
		Vector3 axis = new Vector3();
		if(axisType == AXIS_TYPE.XY){
			axis.x = Input.GetAxis("Horizontal") * sensivity;
			axis.y = Input.GetAxis("Vertical") * sensivity;
			axis.z = 0;
		}
		else if(axisType == AXIS_TYPE.XZ){
			axis.x = Input.GetAxis("Horizontal") * sensivity;
			axis.y = 0;
			axis.z = Input.GetAxis("Vertical") * sensivity;
		}
		else if(axisType == AXIS_TYPE.YZ){
			axis.x = 0;
			axis.y = Input.GetAxis("Horizontal") * sensivity;
			axis.z = Input.GetAxis("Vertical") * sensivity;
		}
		return axis;		
	}
}
