using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonReload : MonoBehaviour {

	public void	ReloadGame(){
		Application.LoadLevel(Application.loadedLevel);
	}
}
