using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[AddComponentMenu("V&R Controller/Controller/VirtualJoystic")]
public class VirtualJoystick : MonoBehaviour {	
	public enum AXIS_TYPE {
		XY,
		XZ,
		YZ
	}
	public AXIS_TYPE axisType;
	public Texture joyCap_;
	public float joypadMaxRadius_ = 40f;
	
	[Range(0.0f,1.0f)]
	public float x1;
	
	[Range(0.0f,1.0f)]
	public float y1;
	
	[HideInInspector]
	public float pixelinsetValue;
	public float joyScale;
	
	private Vector2 joypadCapLocation_ = Vector2.zero;
	[Range(0.0f,1.0f)]
	public float sensivity;
	public float thumbOffset = 100;
	private float distance_;
	private float touchAngle_;
	
	private float horizontal_ = 0f;
	private float vertical_ = 0f;
	private bool input = false;

	public float GetHorizontal()
	{
		return horizontal_;
	}
	
	public float GetVertical()
	{
		return vertical_;
	}
	
	private float GetDistance()
	{
		if (distance_ == 0f)
			return 0f;
		else
			return distance_ / joypadMaxRadius_;
	}
	
	void Update () {
		Vector2 center = new Vector2(
			Screen.width * transform.position.x + GetComponent<GUITexture>().pixelInset.center.x/Screen.width,
			Screen.height * (1-transform.position.y) - GetComponent<GUITexture>().pixelInset.center.y/Screen.height);
		joypadCapLocation_ = center;
		pixelinsetValue = Screen.width * joyScale / 10;
		Rect boxforinset = new Rect (-pixelinsetValue / 2, -pixelinsetValue / 2, pixelinsetValue, pixelinsetValue);
		GetComponent<GUITexture>().pixelInset = boxforinset;
		
		if( Application.platform == RuntimePlatform.OSXEditor ||
		   Application.platform == RuntimePlatform.IPhonePlayer ||
		   Application.platform == RuntimePlatform.Android)
		{		
			if (!UpdateTouch())
				UpdateMouse();
//			UpdateTouch();
		}
		else
		{
			UpdateMouse();
		}
	}

	//for touch input
	bool UpdateTouch()
	{
		int n = 0;
		Vector2 center = new Vector2(
			Screen.width * transform.position.x + GetComponent<GUITexture>().pixelInset.center.x,
			Screen.height * (1-transform.position.y) - GetComponent<GUITexture>().pixelInset.center.y/Screen.height);
		
		Rect rect = new Rect(
			center.x - GetComponent<GUITexture>().pixelInset.width/2,
			center.y + GetComponent<GUITexture>().pixelInset.height/2,
			GetComponent<GUITexture>().pixelInset.width *2f,
			GetComponent<GUITexture>().pixelInset.height * 2f);
		
		if (Input.touchCount == 0)
			return false;
		else if (Input.touchCount==1)
		{
			if (!(Input.GetTouch(0).position.x >= rect.x - thumbOffset &&
			      Input.GetTouch(0).position.y >= rect.y - Screen.height &&
			      Input.GetTouch(0).position.x < rect.x + GetComponent<GUITexture>().pixelInset.width + thumbOffset &&
			      Input.GetTouch(0).position.y < rect.y + rect.height))
				input = false;
		}
		else if (Input.touchCount == 2)
		{		
			if (Input.GetTouch(1).position.x >= rect.x - thumbOffset&&
			    Input.GetTouch(1).position.y >= rect.y - Screen.height &&
			    Input.GetTouch(1).position.x < rect.x + GetComponent<GUITexture>().pixelInset.width + thumbOffset&&
			    Input.GetTouch(1).position.y < rect.y + rect.height)
			{
				n = 1;
			}
			else
			{
				n = 0;
			}
		}
		
		Vector2 mousePos = new Vector2(
			Input.GetTouch(n).position.x, 
			Screen.height - Input.GetTouch(n).position.y);
		
		
		if (Input.GetTouch(n).phase == TouchPhase.Began)
		{	
			if (mousePos.x >= rect.x &&
			    mousePos.y < rect.y &&
			    mousePos.x < rect.x + GetComponent<GUITexture>().pixelInset.width &&
			    mousePos.y >= rect.height)
			{
				input = true;
			}
			else
			{
				input = false;
			}
		}
		
		if (!input || (Input.GetTouch(n).position.x>  rect.x + GetComponent<GUITexture>().pixelInset.width + thumbOffset) || (Input.GetTouch(n).position.y>  rect.y + rect.height) || Input.GetTouch(n).phase == TouchPhase.Ended) {
			input = false;
			distance_ = 0f;
			touchAngle_ = 0f;
			horizontal_ = 0f;
			vertical_ = 0f;
			return false;
		}	

		float dx = center.x - mousePos.x;
		float dy = center.y - mousePos.y;
		distance_ = Mathf.Sqrt(dx * dx + dy * dy);
		
		if (distance_ > joypadMaxRadius_)
		{
			distance_ = joypadMaxRadius_;
			joypadCapLocation_ = new Vector2(
				center.x + horizontal_ * joypadMaxRadius_, 
				center.y - vertical_ * joypadMaxRadius_);
		}
		else
		{
			joypadCapLocation_ = mousePos;
		}
		touchAngle_ = Mathf.Atan2(dy, dx);
		horizontal_ = -Mathf.Cos(touchAngle_) * distance_ / joypadMaxRadius_;
		vertical_ = Mathf.Sin(touchAngle_) * distance_ / joypadMaxRadius_;
		return true;
	}

	//for mouse input
	bool UpdateMouse()
	{
		Vector2 center = new Vector2(
			Screen.width * transform.position.x + GetComponent<GUITexture>().pixelInset.center.x,
			Screen.height * (1-transform.position.y) - GetComponent<GUITexture>().pixelInset.center.y/Screen.height);
		Vector2 mousePos = new Vector2(
			Input.mousePosition.x, 
			Screen.height - Input.mousePosition.y);
		
		if (Input.GetMouseButtonDown(0)) {
			Rect rect = new Rect(
				center.x - GetComponent<GUITexture>().pixelInset.width/2,
				center.y + GetComponent<GUITexture>().pixelInset.height/2,
				GetComponent<GUITexture>().pixelInset.width * 2f,
				GetComponent<GUITexture>().pixelInset.height * 2f);

			if (mousePos.x >= rect.x &&
			    mousePos.y < rect.y &&
			    mousePos.x < rect.x + GetComponent<GUITexture>().pixelInset.width &&
			    mousePos.y >= rect.y - GetComponent<GUITexture>().pixelInset.height)
			{
				input = true;
			}
			else
			{
				input = false;
			}
		}
		
		if (!input || !Input.GetMouseButton(0)) {		
			input = false;
			distance_ = 0f;
			touchAngle_ = 0f;
			horizontal_ = 0f;
			vertical_ = 0f;
			return false;
		}
		
		float dx = (center.x - mousePos.x);
		float dy = (center.y - mousePos.y);
		
		distance_ = Mathf.Sqrt(dx * dx + dy * dy);
		
		if (distance_ > joypadMaxRadius_)
		{
			distance_ = joypadMaxRadius_;
			
			joypadCapLocation_ = new Vector2(
				center.x + horizontal_ * joypadMaxRadius_, 
				center.y - vertical_ * joypadMaxRadius_);
		}
		else
		{
			joypadCapLocation_ = mousePos;
		}
		touchAngle_ = Mathf.Atan2(dy, dx);
		horizontal_ = -Mathf.Cos(touchAngle_) * distance_ / joypadMaxRadius_;
		vertical_ = Mathf.Sin(touchAngle_) * distance_ / joypadMaxRadius_;
		return true;
	}
	//get Axis as Vector2
	public Vector2 GetAxisVector2(){
		Vector2 axis = new Vector2();
		axis.x = horizontal_ * sensivity;
		axis.y = vertical_ * sensivity;
		return axis;
	}
	//get Axis as Vector3
	public Vector3 GetAxisVector3(){
		Vector3 axis = new Vector3();
		if(axisType == AXIS_TYPE.XY){
			axis.x = horizontal_* sensivity;
			axis.y = vertical_ * sensivity;
			axis.z = 0;
		}
		else if(axisType == AXIS_TYPE.XZ){
			axis.x = horizontal_ * sensivity;
			axis.y = 0;
			axis.z = vertical_ * sensivity;
		}
		else if(axisType == AXIS_TYPE.YZ){
			axis.x = 0;
			axis.y = horizontal_ * sensivity;
			axis.z = vertical_ * sensivity;
		}
		return axis;
	}
	//draw joystick cap
	void OnGUI()
	{
		GUI.DrawTexture(new Rect(
			joypadCapLocation_.x + GetComponent<GUITexture>().pixelInset.x,
			joypadCapLocation_.y + GetComponent<GUITexture>().pixelInset.y, 
			GetComponent<GUITexture>().pixelInset.width, 
			GetComponent<GUITexture>().pixelInset.height), 
		                joyCap_);
	}
	
	public void adjustScale(){
		pixelinsetValue = GetMainGameViewSize().x * joyScale / 10;
		Rect boxforinset = new Rect (-pixelinsetValue / 2, -pixelinsetValue / 2, pixelinsetValue, pixelinsetValue);
		GetComponent<GUITexture>().pixelInset = boxforinset;
	}
	
	public void adjustPosition() {

		Vector3 pos = new Vector3 (x1, y1, 0);
		transform.position = pos;
	}
	
	public Vector2 GetMainGameViewSize()
	{
		System.Type T = System.Type.GetType("UnityEditor.GameView,UnityEditor");
		System.Reflection.MethodInfo GetSizeOfMainGameView = T.GetMethod("GetSizeOfMainGameView",System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
		System.Object Res = GetSizeOfMainGameView.Invoke(null,null);
		return (Vector2)Res;
	}
}

