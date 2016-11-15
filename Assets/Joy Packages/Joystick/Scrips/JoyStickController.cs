using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JoyStickController : MonoBehaviour 
{
	public GameObject JoysticBase;
	public GameObject JoysticHandler;
	public bool isDaynamic;
	[Range(0,1)]
	public float x;
	[Range(0,1)]
	public float y;
	[Range(0,1)]
	public float ScreenWidth;
	[Range(0,1)]
	public float ScreenHeight;

	public float FadeOutSpeed;
	bool isZonePress;
	[HideInInspector]
	private Vector2 Axis;
	float[] RctaBoundary=new float[4];
	float width;
	float height;

	Color JoysticBaseColor;
	Color JoysticHandlerColor;
	int CurrentTochID;
	public float GetAxis(string axis)
	{
		if(axis=="Horizontal")
		{
			return Axis.x;
		}
		if(axis=="Vertical")
		{
			return Axis.y;
		}
		return 0;

	}


	void Start () 
	{
		isZonePress = false;

		width=JoysticBase.GetComponent<RectTransform> ().rect.width/2;
		height=JoysticBase.GetComponent<RectTransform> ().rect.height/2;

		if (isDaynamic)
		{
			JoysticBaseColor=JoysticBase.GetComponent<Image> ().color;
			JoysticHandlerColor=JoysticHandler.GetComponent<Image> ().color;
			JoysticBase.GetComponent<Image> ().color=new Color(JoysticBaseColor.r,JoysticBaseColor.g,JoysticBaseColor.b,0f);
			JoysticHandler.GetComponent<Image> ().color=new Color(JoysticHandlerColor.r,JoysticHandlerColor.g,JoysticHandlerColor.b,0f);
 			JoysticHandler.GetComponent<EventTrigger>().enabled=false;
		}
	}

	void Update () 
	{
		#if !UNITY_EDITOR
		if(isDaynamic)
		{

			if(Input.touchCount>0 && !isZonePress)
			{
				CurrentTochID=-1;
				foreach(Touch t in Input.touches)
				{
					if(t.position.x>x*Screen.width && t.position.y>y*Screen.height && t.position.x<ScreenWidth*Screen.width && t.position.y<ScreenHeight*Screen.height)
					{
						isZonePress=true;
						JoysticBase.transform.position=t.position;
						JoysticBase.GetComponent<Image> ().color=JoysticBaseColor;
						JoysticHandler.GetComponent<Image> ().color=JoysticHandlerColor;
						CurrentTochID++;
					}
				}
			}
			if(isZonePress)
			{
				if(Input.touches[CurrentTochID].phase==TouchPhase.Ended)
				{
					isZonePress=false;
					StartCoroutine(FadeOut());
					OnRealease();
					return;
				}
				OnDrag();
			}
		}

		#else
		if(isDaynamic)
		{
			if(Input.GetMouseButtonDown(0))
			{
				Vector3 t=Input.mousePosition;
				if(t.x>x*Screen.width && t.y>y*Screen.height && t.x<ScreenWidth*Screen.width && t.y<ScreenHeight*Screen.height)
				{
					isZonePress=true;
					JoysticBase.transform.position=Input.mousePosition;
					JoysticBase.GetComponent<Image> ().color=JoysticBaseColor;
					JoysticHandler.GetComponent<Image> ().color=JoysticHandlerColor;
				}
			}
			if(Input.GetMouseButtonUp(0))
			{
				isZonePress=false;
				StartCoroutine(FadeOut());
				OnRealease();
			}
			if(isZonePress)
			{
				OnDrag();
			}
		}
		#endif
	}

	public void OnDrag()
	{ 
		Vector3 josticBasePos= JoysticBase.transform.position;
		RctaBoundary [0] = josticBasePos.x+width/2;
		RctaBoundary [1] = josticBasePos.y+height/2;
		RctaBoundary [2] = josticBasePos.x-width/2;
		RctaBoundary [3] = josticBasePos.y-height/2;
		Vector3 JoysticHandlerPos = JoysticHandler.transform.position;
		if(JoysticHandlerPos.x<=RctaBoundary [0] && JoysticHandlerPos.y<=RctaBoundary [1] && JoysticHandlerPos.x>=RctaBoundary [2] && JoysticHandlerPos.y>=RctaBoundary [3])
		{
			Vector3 pos=Input.mousePosition; 
			pos.x=Mathf.Clamp(pos.x,RctaBoundary[2],RctaBoundary[0]);
			pos.y=Mathf.Clamp(pos.y,RctaBoundary[3],RctaBoundary[1]);
			JoysticHandler.transform.position = pos;
			Axis.x=(pos.x-JoysticBase.transform.position.x)/(width/2);
			Axis.y=(pos.y-JoysticBase.transform.position.y)/(height/2);
		}
	}

	public void OnRealease()
	{
		JoysticHandler.transform.position = JoysticBase.transform.position;
		Axis.x=0;
		Axis.y=0;
	}

	IEnumerator  FadeOut()
	{
		float alpha = JoysticBase.GetComponent<Image> ().color.a;
		while(alpha> 0)
		{
			alpha-=Time.deltaTime*FadeOutSpeed;
			JoysticBase.GetComponent<Image> ().color=new Color(JoysticBaseColor.r,JoysticBaseColor.g,JoysticBaseColor.b,alpha);
			JoysticHandler.GetComponent<Image> ().color=new Color(JoysticHandlerColor.r,JoysticHandlerColor.g,JoysticHandlerColor.b,alpha);
			yield return null;
		}

	}
}
