using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts
{
    class BrowserImagePopulator : MonoBehaviour
    {
        public AssetPopulator assetPopulator;
        public Button imageButton;
        public string imageBankPath = "Joy Browser Images";

        private bool ready = false;

        AssetPackage activeAssetPackage;

        void Update()
        {
            if (!ready)
            {
                activeAssetPackage = assetPopulator.activeAssetPackage;
                Sprite[] sprites = Resources.LoadAll<Sprite>(imageBankPath);
                Image image = GetComponent<Image>();

                if (activeAssetPackage != null)
                {
                    foreach (Sprite sprite in sprites)
                    {
                        if (sprite.name.Equals(activeAssetPackage.name))
                        {
                            image.sprite = sprite;
                            image.SetNativeSize();
                            
                            imageButton.onClick.AddListener(delegate ()
                            {
                                assetPopulator.navigation.GoToUrl(activeAssetPackage.WebLink);
                            });

                            ready = true; 

                            break;
                        }
                    }
                }
                else
                {
                    if (SceneManager.GetActiveScene().name.Equals("Main"))
                    {
                        string parentName = gameObject.transform.parent.name;

                        foreach (Sprite sprite in sprites)
                        {
                            if (sprite.name.Equals(parentName))
                            {
                                image.sprite = sprite;
                                image.SetNativeSize();

                                imageButton.onClick.AddListener(delegate ()
                                {
                                    assetPopulator.navigation.GoToScene(parentName);
                                });

                                ready = true;

                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}
