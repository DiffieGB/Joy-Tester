
using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
[AddComponentMenu("V&R Controller/Controller/INButton")]
public class INButton : MonoBehaviour {
	
	public enum ButtonPhase {
		Up,
		Down,
		Stationary
	}

	int fingerID;

	public Texture buttonDown;
	public Texture buttonUp;
	public string buttonName;
	Vector2 center = new Vector2();
	
	public ButtonPhase buttonPhase = ButtonPhase.Up;

	[Range(0.0f,1.0f)]
	public float x1;
	
	[Range(0.0f,1.0f)]
	public float y1;

	[HideInInspector]
	public float pixelinsetValue;
	public float joyScale;
	
	public ButtonPhase GetButton() { 
		return buttonPhase;	
	}

	void Start () {
		buttonUp = GetComponent<GUITexture>().texture;
		Input.multiTouchEnabled = true;
	}

	void Update () {
		pixelinsetValue = Screen.width * joyScale / 10;
		Rect boxforinset = new Rect (-pixelinsetValue / 2, -pixelinsetValue / 2, pixelinsetValue, pixelinsetValue);
		
		GetComponent<GUITexture>().pixelInset = boxforinset;

		if (Application.platform == RuntimePlatform.OSXEditor || 
			Application.platform == RuntimePlatform.IPhonePlayer||
			Application.platform == RuntimePlatform.Android)
		{
			if (!UpdateTouch())
				UpdateMouse();
//			UpdateTouch ();
		}
		else 
		{
			UpdateMouse();	
		}

		if (buttonPhase == ButtonPhase.Up)
			GetComponent<GUITexture>().texture = buttonUp;
		else
		{
			GetComponent<GUITexture>().texture = buttonDown;
		}
	}
	
	bool UpdateTouch() {
		center = new Vector2(Screen.width * transform.position.x,Screen.height * transform.position.y);

				Rect rect = new Rect(
				center.x + (GetComponent<GUITexture>().pixelInset.x ),
				center.y + (GetComponent<GUITexture>().pixelInset.y ),
				GetComponent<GUITexture>().pixelInset.width * 1.5f,
				GetComponent<GUITexture>().pixelInset.height * 1.5f);
		
		if (Input.touchCount > 0 ) {
			foreach(Touch touch in Input.touches)
			{
				if (touch.phase == TouchPhase.Began && (touch.position.x >= rect.x && touch.position.y >= rect.y && touch.position.x  < rect.x + rect.width/2 && touch.position.y < rect.y + rect.height/2)){					
					buttonPhase = ButtonPhase.Down;
					Debug.Log(buttonPhase);
					fingerID = touch.fingerId;
					return true;
				}
				else if (touch.fingerId == fingerID && touch.phase == TouchPhase.Stationary){
					if ((touch.position.x >= rect.x && touch.position.y >= rect.y && touch.position.x  < rect.x + rect.width/2 && touch.position.y < rect.y + rect.height/2)){
						buttonPhase = ButtonPhase.Stationary;
					}
					else
						buttonPhase = ButtonPhase.Up;
					return true;
				}
				else if (touch.phase == TouchPhase.Ended){
					buttonPhase = ButtonPhase.Up;
					return true;
				}
			}
		}
		return false;
	}
	
	bool UpdateMouse() {
		Vector2 center = new Vector2(
			Screen.width * transform.position.x,
			Screen.height * transform.position.y);
		
		Rect rect = new Rect(
			center.x + GetComponent<GUITexture>().pixelInset.x, center.y + GetComponent<GUITexture>().pixelInset.y,
			GetComponent<GUITexture>().pixelInset.width * 1.5f, GetComponent<GUITexture>().pixelInset.height * 1.5f);
		Vector2 mousePos = Input.mousePosition;
		
		if (Input.GetMouseButtonDown(0)) {
			if (mousePos.x >= rect.x && mousePos.y >= rect.y && mousePos.x < rect.x+rect.width/2 && mousePos.y < rect.y+rect.height/2)
			{
				buttonPhase = ButtonPhase.Down;
				return true;
			}
		}

		if (Input.GetMouseButton(0)) {
			if (buttonPhase == ButtonPhase.Down && (mousePos.x >= rect.x && mousePos.y >= rect.y && mousePos.x < rect.x+rect.width/2 && mousePos.y < rect.y+rect.height/2)){
				buttonPhase = ButtonPhase.Stationary;
				if (!(mousePos.x >= rect.x && mousePos.y >= rect.y && mousePos.x < rect.x+rect.width/2 && mousePos.y < rect.y+rect.height/2))
					buttonPhase = ButtonPhase.Up;
			}
			else if (buttonPhase == ButtonPhase.Down && !(mousePos.x >= rect.x && mousePos.y >= rect.y && mousePos.x < rect.x+rect.width/2 && mousePos.y < rect.y+rect.height/2)) {
				buttonPhase = ButtonPhase.Up;
			}
			return true;
		}
		else if (!Input.GetMouseButton(0)){
			buttonPhase = ButtonPhase.Up;
			return false;
		}
		return true;	
	}

	public void adjustPosition() {
		Vector3 pos = new Vector3 (x1, y1, 0);
		transform.position = pos;
	}
	
	public void adjustScale(){
		pixelinsetValue = GetMainGameViewSize().x * joyScale / 10;
		Rect boxforinset = new Rect (-pixelinsetValue / 2, -pixelinsetValue / 2, pixelinsetValue, pixelinsetValue);
		GetComponent<GUITexture>().pixelInset = boxforinset;
	}

	public Vector2 GetMainGameViewSize()
	{
		System.Type T = System.Type.GetType("UnityEditor.GameView,UnityEditor");
		System.Reflection.MethodInfo GetSizeOfMainGameView = T.GetMethod("GetSizeOfMainGameView",System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
		System.Object Res = GetSizeOfMainGameView.Invoke(null,null);
		return (Vector2)Res;
	}
}

