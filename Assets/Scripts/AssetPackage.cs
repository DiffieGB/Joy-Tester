using UnityEngine;
using System.Collections;

[System.Serializable]
public class AssetPackage : MonoBehaviour {

    public string name;
    public string sceneName;
    public string contentId;
    private string aId = "1011lGoa";
    private string baseLink = "https://www.assetstore.unity3d.com/#!/content";

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override string ToString()
    {
        return name;
    }

    public string WebLink
    {
        get { return baseLink + "/" + contentId + "?aid=" + aId; }
    }
}
