using UnityEditor;
using UnityEngine;
using System.Collections;
using PigeonCoopToolkit.Generic.Editor;

namespace PigeonCoopToolkit.TIM.Editor
{
    [InitializeOnLoad]
    public class InfoDialogueSpawner
    {
        static InfoDialogueSpawner()
        {
            EditorApplication.update += Update;
        }

        static void Update()
        {
            if (EditorPrefs.GetBool("PCTK/TouchInputManager/ShownInfoDialogue") == false)  
            {
                EditorPrefs.SetBool("PCTK/TouchInputManager/ShownInfoDialogue", true);

                InfoDialogue dialogue = EditorWindow.GetWindow<InfoDialogue>(true, "Thanks for your purchase!");
                dialogue.Init(Resources.Load("PCTK/TouchInputManager/banner") as Texture2D,
                new Generic.VersionInformation("Touch Input Manager", 3, 1, 2),
                Application.dataPath + "/PigeonCoopToolkit/__TouchInputManager Extras/Pigeon Coop Toolkit - TouchInputManager.pdf",
                "8984");
            } 
            else
            {
                EditorApplication.update -= Update; 
            }
        }
    }
}
 