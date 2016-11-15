using UnityEditor;

namespace PigeonCoopToolkit.TIM.Editor
{
    [InitializeOnLoad]
    class TIMScriptableAssetInitializer
    {
        static TIMScriptableAssetInitializer ()
        {
            TouchInputManagerEditor.EnsureDatafileExists();
        }
    }
} 