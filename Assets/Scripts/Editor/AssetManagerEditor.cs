using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;


[CustomEditor(typeof(AssetManager))]
[CanEditMultipleObjects]
public class AssetManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        AssetManager assetManager = (AssetManager)target;

        DrawDefaultInspector();

        if (GUILayout.Button("UpdateAssets"))
        {
            assetManager.assets = new List<AssetPackage>(assetManager.GetComponents<AssetPackage>());

            foreach (AssetPackage asset in assetManager.assets)
            {
                if (!asset.enabled)
                {
                    assetManager.assets.Remove(asset);
                }
            }
        }
    }
}