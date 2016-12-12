using UnityEngine;

#if false
using ZenFulcrum.EmbeddedBrowser;

public class BrowserController : MonoBehaviour {

    private Browser browser;
    private CanvasGroup canvasGroup;
    public string customJS;
    float delay = 1;
    bool hasJsRun = false;

	// Use this for initialization
	void Start ()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        browser = GetComponent<Browser>();

        canvasGroup.alpha = 0;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!hasJsRun && Time.timeSinceLevelLoad > delay)
        {
            hasJsRun = true;
            browser.EvalJS(customJS);
        }

        if (hasJsRun && canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += .01f;
        }
    }
}
#endif