using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Navigation : MonoBehaviour {

    public string mainMenu;
    List<AssetPackage> assetPackages;

    // Use this for initialization
    void Start () {
        assetPackages = GetComponent<AssetManager>().assets;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void GoToUrl(string url)
    {
#if UNITY_EDITOR
        Application.OpenURL(url);
#elif  UNITY_WEBGL
        Application.ExternalEval("window.open(\"" + url + "\")");
#else
        Application.OpenURL(url);
#endif
    }

    public void GoToHomeScene()
    {
        SceneManager.LoadScene(mainMenu);
    }

    public void GoToPreviousScene()
    {
        for (int i = 0; i < assetPackages.Count; i++)
        {
            if (assetPackages[i].sceneName.Equals(SceneManager.GetActiveScene().name))
            {
                int sceneIndexToload = i - 1;
                if (sceneIndexToload < 0)
                {
                    sceneIndexToload = assetPackages.Count - 1;
                }

                SceneManager.LoadScene(assetPackages[sceneIndexToload].sceneName);
            }
        }
    }

    public void GoToNextScene()
    {
        for (int i = 0; i < assetPackages.Count; i++)
        {
            if (assetPackages[i].sceneName.Equals(SceneManager.GetActiveScene().name))
            {
                int sceneIndexToload = i + 1;
                if (sceneIndexToload >= assetPackages.Count)
                {
                    sceneIndexToload = 0;
                }

                SceneManager.LoadScene(assetPackages[sceneIndexToload].sceneName);
            }
        }
    }

    public void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
