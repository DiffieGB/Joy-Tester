using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

#if false
using ZenFulcrum.EmbeddedBrowser;

namespace Assets.Scripts
{
    class BrowserPopulator : MonoBehaviour
    {
        public AssetPopulator assetPopulator;
        public Button browserButton;
        AssetPackage activeAssetPackage;

        void Start()
        {
            //Browser browser = GetComponent<Browser>();
            //browser.Url = assetPopulator.activeAssetPackage.WebLink;
        }

        void Update()
        {
            if (activeAssetPackage == null)
            {
                Browser browser = GetComponent<Browser>();
                activeAssetPackage = assetPopulator.activeAssetPackage;
                browser.Url = activeAssetPackage.WebLink;

                browserButton.onClick.AddListener(delegate ()
                {
                    assetPopulator.navigation.GoToUrl(activeAssetPackage.WebLink);
                });
            }
        }
    }
}
#endif
