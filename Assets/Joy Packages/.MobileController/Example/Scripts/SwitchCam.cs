using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SwitchCam : MonoBehaviour {

	public	GameObject TargetCamera; 

	public void	Newcam(){
//		if (TargetCamera == null)
//		{
//			Debug.LogWarning("Connect to the PlayerCamera");
//			return;
//		}  

		//print ("Rotate camera");
		TargetCamera.transform.Rotate (new Vector3 (0, 90, 0));
	}
}
