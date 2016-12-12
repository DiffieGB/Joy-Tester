using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class AssetPopulator : MonoBehaviour {

    private bool desktop;
    public GameObject BrowserView;
    public GameObject StaticView;
    public List<Button> buttonLinks;
    
    public AssetPackage activeAssetPackage;
    public Navigation navigation;
    private AssetManager assetManager;

    // Use this for initialization
    void Start () {
        GameObject runtimeScripts = GameObject.Find("Runtime Scripts");
        navigation = runtimeScripts.GetComponent<Navigation>();
        assetManager = runtimeScripts.GetComponent<AssetManager>();

        string activeScene = SceneManager.GetActiveScene().name;

        foreach(AssetPackage assetPackage in assetManager.assets)
        {
            if (assetPackage.sceneName == activeScene)
            {
                activeAssetPackage = assetPackage;
                //Debug.Log("Active asset pack: " + activeAssetPackage);
            }
            //Debug.Log("Asset pack: " + activeScene + ", Scene Name: " + assetPackage.sceneName);
        }

        if (activeAssetPackage != null)
        {
            foreach (Button button in buttonLinks)
            {
                button.onClick.AddListener(delegate ()
                {
                    navigation.GoToUrl(activeAssetPackage.WebLink);
                });
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
