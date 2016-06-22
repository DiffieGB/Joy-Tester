using UnityEngine;
using System.Collections;

public class Events : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void GoToUrl(string url) {
		Application.OpenURL (url);
	}

    public void GoToPreviousScene()
    {
        int curentLevelIndex = Application.loadedLevel;
        int nextLevel = (curentLevelIndex - 1 + Application.levelCount) % Application.levelCount;

        Application.LoadLevel(nextLevel);
    }

    public void GoToNextScene()
    {
        int curentLevelIndex = Application.loadedLevel;
        int nextLevel = (curentLevelIndex + 1) % Application.levelCount;

        Application.LoadLevel(nextLevel);
    }
}
