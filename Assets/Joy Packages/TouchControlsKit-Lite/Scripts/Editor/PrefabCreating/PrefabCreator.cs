/*******************************************************
 * 													   *
 * Asset:		 Touch Controls Kit         		   *
 * Script:		 TCKInput.cs                           *
 * 													   *
 * Copyright(c): Victor Klepikov					   *
 * Support: 	 http://bit.ly/vk-Support			   *
 * 													   *
 * mySite:       http://vkdemos.ucoz.org			   *
 * myAssets:     http://u3d.as/5Fb                     *
 * myTwitter:	 http://twitter.com/VictorKlepikov	   *
 * 													   *
 *******************************************************/


using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TouchControlsKit.Utils;

namespace TouchControlsKit.Inspector
{
    public sealed class PrefabCreator : Editor
    {
        // 
        private const string mainGOName = "_TCKCanvas";
        private const string menuAbbrev = "GameObject/UI/Touch Controls Kit/";
        private const string nameAbbrev = "TouchControlsKit";

        //
        private static GameObject tckGUIobj;
        private static GameObject button, touchpad, steeringWheel;
        private static GameObject joystickMain, joystickBackgr, joystickImage;
        private static GameObject dpadMain, dpadArrowUP, dpadArrowDOWN, dpadArrowLEFT, dpadArrowRIGHT;

        private static Color32 defaultColor = new Color32( 255, 255, 255, 150 );


        // CreateTCKInput [MenuItem]
        [MenuItem( menuAbbrev + "TCK Canvas" )]
        private static void CreateTouchManager()
        {
            if( tckGUIobj == null )
            {
                TCKInput tckInputObj = FindObjectOfType<TCKInput>();
                tckGUIobj = ( tckInputObj != null ) ? tckInputObj.gameObject : null;
            }

            if( tckGUIobj != null ) 
                return;

            tckGUIobj = new GameObject( mainGOName, typeof( Canvas ), typeof( GraphicRaycaster ), typeof( CanvasScaler ), typeof( TCKInput ) );
            tckGUIobj.layer = LayerMask.NameToLayer( "UI" );

            Transform camTransform = new GameObject( "tckUICamera", typeof( GuiCamera ) ).transform;
            camTransform.parent = tckGUIobj.transform;
            camTransform.localPosition = Vector3.zero;

            Camera tmpCamera = camTransform.GetComponent<Camera>();
            //
            float maxCameraDepth = -1f;
            Camera[] tmpCameras = FindObjectsOfType<Camera>();
            foreach( Camera cam in tmpCameras )
                maxCameraDepth = Mathf.Max( cam.depth, maxCameraDepth );
            maxCameraDepth++;
            tmpCamera.depth = maxCameraDepth;
            //
            tmpCamera.clearFlags = CameraClearFlags.Depth;
            tmpCamera.cullingMask = 32;
            tmpCamera.orthographic = true;
            tmpCamera.orthographicSize = 100f;
            tmpCamera.nearClipPlane = -.25f;
            tmpCamera.farClipPlane = .25f;
            tmpCamera.useOcclusionCulling = false;
            tmpCamera.hdr = false;

            tckGUIobj.GetComponent<Canvas>().worldCamera = tmpCamera;
            tckGUIobj.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera; 
            tckGUIobj.GetComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        }

        [MenuItem( menuAbbrev + "TCK Canvas", true )]
        private static bool ValidateCreateTouchManager()
        {
            return FindObjectOfType<TCKInput>() == null;
        }


        // CreateButton [MenuItem]
        [MenuItem( menuAbbrev + "Button" )]
        private static void CreateButton()
        {
            CreateTouchManager();
            SetupController<TCKButton>( ref button, tckGUIobj.transform, "Button" + FindObjectsOfType<TCKButton>().Length.ToString(), true );

            TCKButton btnTemp = button.GetComponent<TCKButton>();
            btnTemp.baseImage = button.GetComponent<Image>();
            btnTemp.baseRect = button.GetComponent<RectTransform>();
            btnTemp.normalSprite = AssetDatabase.LoadAssetAtPath<Sprite>( "Assets/" + nameAbbrev + "/Content/Sprites/ButtonNormal.png" );
            btnTemp.pressedSprite = AssetDatabase.LoadAssetAtPath<Sprite>( "Assets/" + nameAbbrev + "/Content/Sprites/ButtonPressed.png" );            
            btnTemp.normalColor = btnTemp.pressedColor = defaultColor;
            btnTemp.MyName = button.name;
            btnTemp.baseRect.sizeDelta = Vector2.one * 45f;            
            button.transform.localScale = Vector3.one;
            btnTemp.baseRect.anchoredPosition = RandomPos;
        }

        // CreateJoystick [MenuItem]
        [MenuItem( menuAbbrev + "Joystick" )]
        private static void CreateJoystick()
        {
            CreateTouchManager();
            SetupController<TCKJoystick>( ref joystickMain, tckGUIobj.transform, "Joystick" + FindObjectsOfType<TCKJoystick>().Length.ToString(), true );

            TCKJoystick joyTemp = joystickMain.GetComponent<TCKJoystick>();            
            joyTemp.baseImage = joystickMain.GetComponent<Image>();
            joyTemp.baseRect = joystickMain.GetComponent<RectTransform>();
            joyTemp.baseImage.sprite = AssetDatabase.LoadAssetAtPath<Sprite>( "Assets/" + nameAbbrev + "/Content/Sprites/Touchzone.png" );

            SetupController<TCKJoystick>( ref joystickBackgr, joystickMain.transform, "JoystickBack", false );
            SetupController<TCKJoystick>( ref joystickImage, joystickBackgr.transform, "Joystick", false );

            joyTemp.joystickBackgroundImage = joystickBackgr.GetComponent<Image>();
            joyTemp.joystickBackgroundRT = joystickBackgr.GetComponent<RectTransform>();
            joyTemp.joystickBackgroundRT.sizeDelta = Vector2.one * 75f; 

            joyTemp.joystickImage = joystickImage.GetComponent<Image>();            
            joyTemp.joystickRT = joystickImage.GetComponent<RectTransform>();
            joyTemp.joystickRT.anchorMin = Vector2.zero;
            joyTemp.joystickRT.anchorMax = Vector2.one;
            joyTemp.joystickRT.sizeDelta = Vector2.zero;

            joyTemp.joystickBackgroundImage.sprite = AssetDatabase.LoadAssetAtPath<Sprite>( "Assets/" + nameAbbrev + "/Content/Sprites/JoystickBack.png" );
            joyTemp.joystickImage.sprite = AssetDatabase.LoadAssetAtPath<Sprite>( "Assets/" + nameAbbrev + "/Content/Sprites/Joystick.png" );

            joyTemp.baseRect.sizeDelta = new Vector2( 180f, 160f );
            
            joyTemp.MyName = joystickMain.name;

            joystickMain.transform.localScale = Vector3.one;
            joyTemp.baseRect.anchoredPosition = RandomPos;
        }

        // CreateTouchpad [MenuItem]
        [MenuItem( menuAbbrev + "Touchpad" )]
        private static void CreateTouchpad()
        {
            CreateTouchManager();
            SetupController<TCKTouchpad>( ref touchpad, tckGUIobj.transform, "Touchpad" + FindObjectsOfType<TCKTouchpad>().Length.ToString(), true );

            TCKTouchpad tpTemp = touchpad.GetComponent<TCKTouchpad>();
            tpTemp.baseImage = touchpad.GetComponent<Image>();
            tpTemp.baseRect = touchpad.GetComponent<RectTransform>();
            tpTemp.baseImage.sprite = AssetDatabase.LoadAssetAtPath<Sprite>( "Assets/" + nameAbbrev + "/Content/Sprites/Touchzone.png" );
            tpTemp.MyName = touchpad.name;
            tpTemp.baseRect.sizeDelta = new Vector2( 270f, 170f );

            touchpad.transform.localScale = Vector3.one;
            tpTemp.baseRect.anchoredPosition = RandomPos;
        }

        // SetupController<Generic>
        private static void SetupController<TComp>(
            ref GameObject myGO, Transform myParent, string myName, bool needMyComponent ) where TComp : MonoBehaviour
        {
            myGO = new GameObject( myName, typeof( Image ) );
            myGO.GetComponent<Image>().color = defaultColor;
            myGO.layer = LayerMask.NameToLayer( "UI" );
            myGO.transform.localScale = Vector3.one;
            myGO.transform.SetParent( myParent );

            if( needMyComponent )
                myGO.AddComponent<TComp>();
        }

        // RandomPos
        private static Vector2 RandomPos { get { return new Vector2( Random.Range( -200f, 200f ), Random.Range( -120f, 120f ) ); } }
    }
}