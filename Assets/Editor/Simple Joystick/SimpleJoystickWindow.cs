/* Written by Kaz Crowe */
/* SimpleJoystickWindow.cs */
using UnityEngine;
using UnityEditor;

public class SimpleJoystickWindow : EditorWindow
{
	GUILayoutOption[] buttonSize = new GUILayoutOption[] { GUILayout.Width( 200 ), GUILayout.Height( 35 ) }; 

	GUILayoutOption[] docSize = new GUILayoutOption[] { GUILayout.Width( 300 ), GUILayout.Height( 325 ) };

	GUISkin style;

	enum CurrentMenu
	{
		MainMenu,
		HowTo,
		Overview,
		Documentation,
		Extras,
		OtherProducts,
		Feedback,
		ThankYou
	}
	static CurrentMenu currentMenu;
	static string menuTitle = "Main Menu";

	Vector2 scrollPos;

	Texture2D scriptReference;
	Texture2D positionVisual;

	Texture2D ujPromo, ubPromo, usbPromo;


	[MenuItem( "Window/Tank and Healer Studio/Simple Joystick", false, 1 )]
	static void Init ()
	{
		currentMenu = CurrentMenu.MainMenu;
		InitializeWindow();
	}

	static void InitializeWindow ()
	{
		EditorWindow window = GetWindow<SimpleJoystickWindow>( true, "Tank and Healer Studio Asset Window", true );
		window.maxSize = new Vector2( 500, 500 );
		window.minSize = new Vector2( 500, 500 );
		window.Show();
	}

	void OnEnable ()
	{
		style = ( GUISkin )EditorGUIUtility.Load( "Simple Joystick/SimpleJoystickEditorSkin.guiskin" );

		scriptReference = ( Texture2D ) EditorGUIUtility.Load( "Simple Joystick/ScriptReference.jpg" );
		positionVisual = ( Texture2D )EditorGUIUtility.Load( "Simple Joystick/SJ_PosVisual.png" );

		ujPromo = ( Texture2D )EditorGUIUtility.Load( "Ultimate UI/UJ_Promo.png" );
		ubPromo = ( Texture2D )EditorGUIUtility.Load( "Ultimate UI/UB_Promo.png" );
		usbPromo = ( Texture2D )EditorGUIUtility.Load( "Ultimate UI/USB_Promo.png" );
	}

	void ErrorScreen ()
	{
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		EditorGUILayout.LabelField( "ERROR", EditorStyles.boldLabel, GUILayout.Width( 45 ) );
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		EditorGUILayout.LabelField( "There is no SimpleJoystickEditorSkin.guiskin located within the Editor Default Resources folder. Please make sure to import all of the needed assets before using the Simple Joystick.", EditorStyles.wordWrappedLabel, GUILayout.Width( 300 ) );
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
	}
	
	void OnGUI ()
	{
		if( style == null )
		{
			GUILayout.BeginVertical( "Box" );
			GUILayout.FlexibleSpace();
			
			ErrorScreen();
			
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndVertical();
			return;
		}

		GUI.skin = style;

		EditorGUILayout.Space();

		GUILayout.BeginVertical( "Box" );
		
		EditorGUILayout.LabelField( "Simple Joystick", GUI.skin.GetStyle( "WindowTitle" ) );

		GUILayout.Space( 3 );

		EditorGUILayout.LabelField( "Version 1.2.2", EditorStyles.whiteMiniLabel );//< ---- ALWAYS UPDATE

		GUILayout.Space( 12 );

		EditorGUILayout.BeginHorizontal();
		GUILayout.Space( 5 );
		if( currentMenu != CurrentMenu.MainMenu && currentMenu != CurrentMenu.ThankYou )
		{
			EditorGUILayout.BeginVertical();
			GUILayout.Space( 5 );
			if( GUILayout.Button( "", GUI.skin.GetStyle( "BackButton" ), GUILayout.Width( 80 ), GUILayout.Height( 40 ) ) )
				BackToMainMenu();
			EditorGUILayout.EndVertical();
		}
		else
			GUILayout.Space( 80 );

		GUILayout.Space( 15 );
		EditorGUILayout.BeginVertical();
		GUILayout.Space( 14 );
		EditorGUILayout.LabelField( menuTitle, GUI.skin.GetStyle( "HeaderText" ) );
		EditorGUILayout.EndVertical();
		GUILayout.FlexibleSpace();
		GUILayout.Space( 80 );
		EditorGUILayout.EndHorizontal();

		GUILayout.Space( 10 );

		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		switch( currentMenu )
		{
			case CurrentMenu.MainMenu:
			{
				MainMenu();
			}break;
			case CurrentMenu.HowTo:
			{
				HowTo();
			}break;
			case CurrentMenu.Overview:
			{
				Overview();
			}break;
			case CurrentMenu.Documentation:
			{
				Documentation();
			}break;
			case CurrentMenu.Extras:
			{
				Extras();
			}break;
			case CurrentMenu.OtherProducts:
			{
				OtherProducts();
			}break;
			case CurrentMenu.Feedback:
			{
				Feedback();
			}break;
			case CurrentMenu.ThankYou:
			{
				ThankYou();
			}break;
			default:
			{
				MainMenu();
			}break;
		}
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

		GUILayout.FlexibleSpace();
		
		GUILayout.Space( 20 );
		EditorGUILayout.EndVertical();
	}

	void BackToMainMenu ()
	{
		currentMenu = CurrentMenu.MainMenu;
		menuTitle = "Main Menu";
	}
	
	#region MainMenu
	void MainMenu ()
	{
		EditorGUILayout.BeginVertical();
		GUILayout.Space( 25 );

		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if( GUILayout.Button( "How To", buttonSize ) )
		{
			currentMenu = CurrentMenu.HowTo;
			scrollPos = Vector2.zero;
			menuTitle = "How To";
		}
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

		GUILayout.FlexibleSpace();

		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if( GUILayout.Button( "Overview", buttonSize ) )
		{
			currentMenu = CurrentMenu.Overview;
			scrollPos = Vector2.zero;
			menuTitle = "Overview";
		}
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

		GUILayout.FlexibleSpace();

		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if( GUILayout.Button( "Documentation", buttonSize ) )
		{
			currentMenu = CurrentMenu.Documentation;
			scrollPos = Vector2.zero;
			menuTitle = "Documentation";
		}
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

		GUILayout.FlexibleSpace();

		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if( GUILayout.Button( "Extras", buttonSize ) )
		{
			currentMenu = CurrentMenu.Extras;
			scrollPos = Vector2.zero;
			menuTitle = "Extras";
		}
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();
		
		GUILayout.FlexibleSpace();

		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if( GUILayout.Button( "Other Products", buttonSize ) )
		{
			currentMenu = CurrentMenu.OtherProducts;
			scrollPos = Vector2.zero;
			menuTitle = "Other Products";
		}
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

		GUILayout.FlexibleSpace();

		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if( GUILayout.Button( "Feedback", buttonSize ) )
		{
			currentMenu = CurrentMenu.Feedback;
			scrollPos = Vector2.zero;
			menuTitle = "Feedback";
		}
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

		GUILayout.FlexibleSpace();
		EditorGUILayout.EndVertical();
	}
	#endregion

	#region HowTo
	void HowTo ()
	{
		scrollPos = EditorGUILayout.BeginScrollView( scrollPos, false, false, docSize );

		EditorGUILayout.LabelField( "How To Create", GUI.skin.GetStyle( "SectionHeader" ) );

		EditorGUILayout.LabelField( "   To create a Simple Joystick in your scene, just go up to GameObject / UI / Ultimate UI / Simple Joystick. What this does is locates the Simple Joystick prefab that is located within the Editor Default Resources folder, and creates a Simple Joystick within the scene.\n\nThis method of adding a Simple Joystick to your scene ensures that the Joystick will have a Canvas and an EventSystem so that it can work correctly.", EditorStyles.wordWrappedLabel );

		GUILayout.Space( 20 );

		EditorGUILayout.LabelField( "How To Reference", GUI.skin.GetStyle( "SectionHeader" ) );
		EditorGUILayout.LabelField( "   One of the great things about the Simple Joystick is how easy it is to reference to other scripts. The first thing that you will want to make sure to do is to name the joystick in the Script Reference section. After this is complete, you will be able to reference that particular joystick by it's name from a static function within the Simple Joystick script.\n\nAfter the joystick has been given a name in the Script Reference section, we can get that joystick's position by creating a Vector2 variable at run time and storing the result from the static function: 'GetPosition'. This Vector2 will be the joystick's position, and will contain values between -1, and 1, with 0 being at the center.\n\nKeep in mind that the joystick's left and right ( horizontal ) movement is translated into this Vector2's X value, while the up and down ( vertical ) is the Vector2's Y value. This will be important when applying the Simple Joystick's position to your scripts.", EditorStyles.wordWrappedLabel );

		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.Space( 10 );
		GUILayout.Label( positionVisual, GUILayout.Width( 200 ), GUILayout.Height( 200 ) );
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

		GUILayout.Space( 20 );

		EditorGUILayout.LabelField( "Example", GUI.skin.GetStyle( "SectionHeader" ) );

		EditorGUILayout.LabelField( "   Let's assume that we want to use a joystick for a characters movement. The first thing to do is to assign the name \"Movement\" in the Joystick Name variable located in the Script Reference section of the Simple Joystick.", EditorStyles.wordWrappedLabel );
		
		GUILayout.Space( 10 );

		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.Label( scriptReference );
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

		GUILayout.Space( 10 );

		EditorGUILayout.LabelField( "After that, we need to create a Vector2 variable to store the result of the joystick's position returned by the 'GetPosition' function. In order to get the \"Movement\" joystick's position, we need to pass in the name \"Movement\" as the argument.", EditorStyles.wordWrappedLabel );

		GUILayout.Space( 10 );

		EditorGUILayout.TextArea( "Vector2 joystickPosition = SimpleJoystick.GetPosition( \"Movement\" );", GUI.skin.GetStyle( "TextArea" ) );

		GUILayout.Space( 10 );

		EditorGUILayout.LabelField( "After this, the joystickPosition variable can be used in anything that needs joystick input. For example, if you are wanting to put the joystick's position into a character movement script, you would create a Vector3 variable for movement direction, and put in the appropriate value of the Simple Joystick's position.", EditorStyles.wordWrappedLabel );

		GUILayout.Space( 10 );

		EditorGUILayout.TextArea( "Vector3 movementDirection = new Vector3( joystickPosition.x, 0, joystickPosition.y );", GUI.skin.GetStyle( "TextArea" ) );

		GUILayout.Space( 10 );

		EditorGUILayout.LabelField( "In the above example, the joystickPosition variable is used to give the movement direction values in the X and Z directions. This is because you generally don't want your character to move in the Y direction unless the user jumps. That is why we put the joystickPosition.y into the Z value of the movementDirection variable.", EditorStyles.wordWrappedLabel );

		GUILayout.Space( 10 );

		EditorGUILayout.LabelField( "Understanding how to use the values from any input is important when creating character controllers, so experiment with the values and try to understand how the mobile input can be used in different ways.", EditorStyles.wordWrappedLabel );

		GUILayout.FlexibleSpace();

		EditorGUILayout.EndScrollView();
	}
	#endregion
	
	#region Overview
	void Overview ()
	{
		scrollPos = EditorGUILayout.BeginScrollView( scrollPos, false, false, docSize );

		EditorGUILayout.LabelField( "Assigned Variables", GUI.skin.GetStyle( "SectionHeader" ) );
		EditorGUILayout.LabelField( "   In the Assigned Variables section, there are three components that should already be assigned if you are using one of the Prefabs that has been provided. If not, you will see error messages on the Simple Joystick inspector that will help you to see if any of these variables are left unassigned. Please note that these need to be assigned in order for the Simple Joystick to work properly.", EditorStyles.wordWrappedLabel );

		GUILayout.Space( 20 );
		
		/* //// --------------------------- < SIZE AND PLACEMENT > --------------------------- \\\\ */
		EditorGUILayout.LabelField( "Size And Placement", GUI.skin.GetStyle( "SectionHeader" ) );
		EditorGUILayout.LabelField( "   The Size and Placement section allows you to customize the joystick's size and placement on the screen, as well as determine where the user's touch can be processed for the selected joystick.", EditorStyles.wordWrappedLabel );

		GUILayout.Space( 5 );

		// Scaling Axis
		EditorGUILayout.LabelField( "Scaling Axis", EditorStyles.boldLabel );
		EditorGUILayout.LabelField( "Determines which axis the joystick will be scaled from. If Height is chosen, then the joystick will scale itself proportionately to the Height of the screen.", EditorStyles.wordWrappedLabel );

		GUILayout.Space( 5 );

		// Anchor
		EditorGUILayout.LabelField( "Anchor", EditorStyles.boldLabel );
		EditorGUILayout.LabelField( "Determines which side of the screen that the joystick will be anchored to.", EditorStyles.wordWrappedLabel );
		
		GUILayout.Space( 5 );

		// Touch Size
		EditorGUILayout.LabelField( "Touch Size", EditorStyles.boldLabel );
		EditorGUILayout.LabelField( "Touch Size configures the size of the area where the user can touch. You have the options of either 'Default','Medium', 'Large' or 'Custom'. When the option 'Custom' is selected, an additional box will be displayed that allows for a more adjustable touch area.", EditorStyles.wordWrappedLabel );
				
		GUILayout.Space( 5 );

		// Touch Size Customization
		EditorGUILayout.LabelField( "Touch Size Customization", EditorStyles.boldLabel );
		EditorGUILayout.LabelField( "If the 'Custom' option of the Touch Size is selected, then you will be presented with the Touch Size Customization box. Inside this box are settings for the Width and Height of the touch area, as well as the X and Y position of the touch area on the screen.", EditorStyles.wordWrappedLabel );
				
		GUILayout.Space( 5 );

		// Dynamic Positioning
		EditorGUILayout.LabelField( "Dynamic Positioning", EditorStyles.boldLabel );
		EditorGUILayout.LabelField( "Dynamic Positioning will make the joystick snap to where the user touches, instead of the user having to touch a direct position to get the joystick. The Touch Size option will directly affect the area where the joystick can snap to.", EditorStyles.wordWrappedLabel );
				
		GUILayout.Space( 5 );

		// Joystick Size
		EditorGUILayout.LabelField( "Joystick Size", EditorStyles.boldLabel );
		EditorGUILayout.LabelField( "Joystick Size will change the scale of the joystick. Since everything is calculated out according to screen size, your joystick Touch Size and other properties will scale proportionately with the joystick's size along your specified Scaling Axis.", EditorStyles.wordWrappedLabel );
				
		GUILayout.Space( 5 );

		// Radius
		EditorGUILayout.LabelField( "Radius", EditorStyles.boldLabel );
		EditorGUILayout.LabelField( "Radius determines how far away the joystick will move from center when it is being used, and will scale proportionately with the joystick.", EditorStyles.wordWrappedLabel );
				
		GUILayout.Space( 5 );

		// Joystick Position
		EditorGUILayout.LabelField( "Joystick Position", EditorStyles.boldLabel );
		EditorGUILayout.LabelField( "Joystick Position will present you with two sliders. The X value will determine how far the Joystick is away from the Left and Right sides of the screen, and the Y value from the Top and Bottom. This will encompass 50% of your screen, relevant to your Anchor selection.", EditorStyles.wordWrappedLabel );
		/* \\\\ -------------------------- < END SIZE AND PLACEMENT > --------------------------- //// */

		GUILayout.Space( 20 );

		/* //// ----------------------------- < STYLE AND OPTIONS > ----------------------------- \\\\ */
		EditorGUILayout.LabelField( "Style And Options", GUI.skin.GetStyle( "SectionHeader" ) );
		EditorGUILayout.LabelField( "   The Style and Options section contains options that effect how the joystick handles and is visually presented to the user.", EditorStyles.wordWrappedLabel );
		
		GUILayout.Space( 5 );

		// Touch Pad
		EditorGUILayout.LabelField( "Touch Pad", EditorStyles.boldLabel );
		EditorGUILayout.LabelField( "Touch Pad presents you with the option to disable the visuals of the joystick, whilst keeping all functionality. When paired with Dynamic Positioning and Throwable, this option can give you a very smooth camera control.", EditorStyles.wordWrappedLabel );
		
		GUILayout.Space( 5 );

		// Axis
		EditorGUILayout.LabelField( "Axis", EditorStyles.boldLabel );
		EditorGUILayout.LabelField( "Axis determines which axis the joystick will snap to. By default it is set to Both, which means the joystick will use both the X and Y axis for movement. If either the X or Y option is selected, then the joystick will snap to the corresponding axis.", EditorStyles.wordWrappedLabel );
		
		GUILayout.Space( 5 );

		// Boundary
		EditorGUILayout.LabelField( "Boundary", EditorStyles.boldLabel );
		EditorGUILayout.LabelField( "Boundary will allow you to decide if you want to have a square or circular edge to your joystick.", EditorStyles.wordWrappedLabel );
		
		GUILayout.Space( 5 );

		// Dead Zone
		EditorGUILayout.LabelField( "Dead Zone", EditorStyles.boldLabel );
		EditorGUILayout.LabelField( "Dead Zone gives the option of setting one or two values that the joystick is constrained by. When selected, the joystick will be forced to a maximum value when it has past the set dead zone.", EditorStyles.wordWrappedLabel );
		/* //// --------------------------- < END STYLE AND OPTIONS > --------------------------- \\\\ */

		EditorGUILayout.EndScrollView();
	}
	#endregion
	
	#region Documentation
	void Documentation ()
	{
		scrollPos = EditorGUILayout.BeginScrollView( scrollPos, false, false, docSize );
		
		/* //// --------------------------- < PUBLIC FUNCTIONS > --------------------------- \\\\ */
		EditorGUILayout.LabelField( "Public Functions", GUI.skin.GetStyle( "SectionHeader" ) );

		GUILayout.Space( 5 );

		// Vector2 JoystickPosition
		EditorGUILayout.LabelField( "Vector2 JoystickPosition", GUI.skin.GetStyle( "ItemHeader" ) );
		EditorGUILayout.LabelField( "Returns the Simple Joystick's position in a Vector2 variable. The X value that is returned represents the Left and Right( Horizontal ) movement of the joystick, whereas the Y value represents the Up and Down( Vertical ) movement of the joystick. The values returned will always be between -1 and 1, with 0 being the center.", EditorStyles.wordWrappedLabel );

		GUILayout.Space( 5 );

		// float JoystickDistance
		EditorGUILayout.LabelField( "float JoystickDistance", GUI.skin.GetStyle( "ItemHeader" ) );
		EditorGUILayout.LabelField( "Returns the distance of the joystick from it's center in a float value. The value returned will always be a value between 0 and 1.", EditorStyles.wordWrappedLabel );

		GUILayout.Space( 5 );

		// UpdatePositioning()
		EditorGUILayout.LabelField( "UpdatePositioning()", GUI.skin.GetStyle( "ItemHeader" ) );
		EditorGUILayout.LabelField( "Updates the size and positioning of the Simple Joystick. This function can be used to update any options that may have been changed prior to Start().", EditorStyles.wordWrappedLabel );
		
		GUILayout.Space( 5 );

		// ResetJoystick()
		EditorGUILayout.LabelField( "ResetJoystick()", GUI.skin.GetStyle( "ItemHeader" ) );
		EditorGUILayout.LabelField( "Resets the joystick back to it's neutral state.", EditorStyles.wordWrappedLabel );
				
		GUILayout.Space( 5 );

		// ResetJoystick()
		EditorGUILayout.LabelField( "bool JoystickState", GUI.skin.GetStyle( "ItemHeader" ) );
		EditorGUILayout.LabelField( "Returns the state that the joystick is currently in. This function will return true when the joystick is being interacted with, and false when not.", EditorStyles.wordWrappedLabel );

		GUILayout.Space( 20 );
		
		/* //// --------------------------- < STATIC FUNCTIONS > --------------------------- \\\\ */
		EditorGUILayout.LabelField( "Static Functions", GUI.skin.GetStyle( "SectionHeader" ) );

		GUILayout.Space( 5 );

		// SimpleJoystick.GetPosition
		EditorGUILayout.LabelField( "Vector2 SimpleJoystick.GetPosition( string joystickName )", GUI.skin.GetStyle( "ItemHeader" ) );
		EditorGUILayout.LabelField( "Returns the Simple Joystick's position in a Vector2 variable. This static function will return the same exact value as the JoystickPosition function. See JoystickPosition for more information.", EditorStyles.wordWrappedLabel );
		GUILayout.Space( 5 );

		// SimpleJoystick.GetDistance
		EditorGUILayout.LabelField( "float SimpleJoystick.GetDistance( string joystickName )", GUI.skin.GetStyle( "ItemHeader" ) );
		EditorGUILayout.LabelField( "Returns the distance of the joystick from it's center in a float value. This static function will return the same value as the JoystickDistance function. See JoystickDistance for more information", EditorStyles.wordWrappedLabel );

		GUILayout.Space( 5 );

		// SimpleJoystick.UpdatePositioning
		EditorGUILayout.LabelField( "SimpleJoystick.UpdatePositioning( string joystickName )", GUI.skin.GetStyle( "ItemHeader" ) );
		EditorGUILayout.LabelField( "Updates the size and positioning of the Simple Joystick. This static function will call the public UpdatePositioning function of the referenced joystick. See UpdatePositioning for more information.", EditorStyles.wordWrappedLabel );
		
		GUILayout.Space( 5 );

		// SimpleJoystick.ResetJoystick
		EditorGUILayout.LabelField( "SimpleJoystick.ResetJoystick( string joystickName )", GUI.skin.GetStyle( "ItemHeader" ) );
		EditorGUILayout.LabelField( "Resets the joystick back to it's neutral state.", EditorStyles.wordWrappedLabel );
						
		GUILayout.Space( 5 );

		// SimpleJoystick.GetJoystickState
		EditorGUILayout.LabelField( "bool SimpleJoystick.GetJoystickState( string joystickName )", GUI.skin.GetStyle( "ItemHeader" ) );
		EditorGUILayout.LabelField( "Returns the state that the joystick is currently in. This function will return true when the joystick is being interacted with, and false when not.", EditorStyles.wordWrappedLabel );

		GUILayout.Space( 20 );

		/* //// --------------------------- < VARIABLES > --------------------------- \\\\ */
		EditorGUILayout.LabelField( "Variables", GUI.skin.GetStyle( "SectionHeader" ) );

		GUILayout.Space( 5 );
		
		// joystick
		EditorGUILayout.LabelField( "joystick ( typeof( RectTransform ) )", GUI.skin.GetStyle( "ItemHeader" ) );
		EditorGUILayout.LabelField( "The Joystick graphic that will move.", EditorStyles.wordWrappedLabel );
				
		GUILayout.Space( 5 );

		// joystickSizeFolder
		EditorGUILayout.LabelField( "joystickSizeFolder ( typeof( RectTransform ) )", GUI.skin.GetStyle( "ItemHeader" ) );
		EditorGUILayout.LabelField( "The parent RectTransform that is used to size all of the joystick components within it.", EditorStyles.wordWrappedLabel );
				
		GUILayout.Space( 5 );

		// joystickBase
		EditorGUILayout.LabelField( "joystickBase ( typeof( RectTransform ) )", GUI.skin.GetStyle( "ItemHeader" ) );
		EditorGUILayout.LabelField( "The Joystick Base RectTransform. This variable is used to determine the reset position of the joystick when not in use, as well as the default position if the Dynamic Positioning option is selected.", EditorStyles.wordWrappedLabel );
								
		GUILayout.Space( 5 );

		// scalingAxis
		EditorGUILayout.LabelField( "scalingAxis ( typeof( SimpleJoystick.ScalingAxis ) )", GUI.skin.GetStyle( "ItemHeader" ) );
		EditorGUILayout.LabelField( "This option will determine which axis the joystick will stick to. If Height is selected, then the joystick will maintain a size that is relevant to the height of the screen. This option is useful to maintain the look of the joystick among different screen resolutions.", EditorStyles.wordWrappedLabel );

		GUILayout.Space( 5 );

		// anchor
		EditorGUILayout.LabelField( "anchor ( typeof( SimpleJoystick.Anchor ) )", GUI.skin.GetStyle( "ItemHeader" ) );
		EditorGUILayout.LabelField( "The anchor option will anchor the joystick to either the Left or Right side of the screen.", EditorStyles.wordWrappedLabel );
						
		GUILayout.Space( 5 );

		// joystickTouchSize
		EditorGUILayout.LabelField( "joystickTouchSize ( typeof( SimpleJoystick.JoystickTouchSize ) )", GUI.skin.GetStyle( "ItemHeader" ) );
		EditorGUILayout.LabelField( "The options provided with the joystickTouchSize variable will determine how much of an area the user can hit to initiate the touch on the joystick.", EditorStyles.wordWrappedLabel );
		
		GUILayout.Space( 5 );

		// joystickSize
		EditorGUILayout.LabelField( "joystickSize ( typeof( float ) )", GUI.skin.GetStyle( "ItemHeader" ) );
		EditorGUILayout.LabelField( "This option determines the overall size of the joystick.", EditorStyles.wordWrappedLabel );
				
		GUILayout.Space( 5 );

		// radiusModifier
		EditorGUILayout.LabelField( "radiusModifier ( typeof( float ) )", GUI.skin.GetStyle( "ItemHeader" ) );
		EditorGUILayout.LabelField( "The radiusModifier variable will be used to calculate how far the joystick graphic can travel from the center of the touch area.", EditorStyles.wordWrappedLabel );
				
		GUILayout.Space( 5 );

		// dynamicPositioning
		EditorGUILayout.LabelField( "dynamicPositioning ( typeof( boolean ) )", GUI.skin.GetStyle( "ItemHeader" ) );
		EditorGUILayout.LabelField( "Dynamic Positioning will make the joystick centered on the position of the user's initial touch on the joystick.", EditorStyles.wordWrappedLabel );
				
		GUILayout.Space( 5 );

		// customTouchSize_X
		EditorGUILayout.LabelField( "customTouchSize_X ( typeof( float ) )", GUI.skin.GetStyle( "ItemHeader" ) );
		EditorGUILayout.LabelField( "This option is available when the joystickTouchSize option is set to 'Custom'. This option will determine how wide the custom touch area is.", EditorStyles.wordWrappedLabel );
				
		GUILayout.Space( 5 );

		// customTouchSize_Y
		EditorGUILayout.LabelField( "customTouchSize_Y ( typeof( float ) )", GUI.skin.GetStyle( "ItemHeader" ) );
		EditorGUILayout.LabelField( "This option is available when the joystickTouchSize option is set to 'Custom'. This option will determine the height of the custom touch area.", EditorStyles.wordWrappedLabel );
						
		GUILayout.Space( 5 );

		// customTouchSizePos_X
		EditorGUILayout.LabelField( "customTouchSizePos_X ( typeof( float ) )", GUI.skin.GetStyle( "ItemHeader" ) );
		EditorGUILayout.LabelField( "This option is available when the joystickTouchSize option is set to 'Custom'. This option will determine the horizontal position of the joystick's touch area.", EditorStyles.wordWrappedLabel );
						
		GUILayout.Space( 5 );

		// customTouchSizePos_Y
		EditorGUILayout.LabelField( "customTouchSizePos_Y ( typeof( float ) )", GUI.skin.GetStyle( "ItemHeader" ) );
		EditorGUILayout.LabelField( "This option is available when the joystickTouchSize option is set to 'Custom'. This option will determine the vertical position of the joystick's touch area.", EditorStyles.wordWrappedLabel );
						
		GUILayout.Space( 5 );

		// customSpacing_X
		EditorGUILayout.LabelField( "customSpacing_X ( typeof( float ) )", GUI.skin.GetStyle( "ItemHeader" ) );
		EditorGUILayout.LabelField( "This option determines the horizontal position of the joystick.", EditorStyles.wordWrappedLabel );
						
		GUILayout.Space( 5 );

		// customSpacing_Y
		EditorGUILayout.LabelField( "customSpacing_Y ( typeof( float ) )", GUI.skin.GetStyle( "ItemHeader" ) );
		EditorGUILayout.LabelField( "The option determines the vertical position of the joystick.", EditorStyles.wordWrappedLabel );
						
		GUILayout.Space( 5 );

		// touchPad
		EditorGUILayout.LabelField( "touchPad ( typeof( bool ) )", GUI.skin.GetStyle( "ItemHeader" ) );
		EditorGUILayout.LabelField( "The Touch Pad option chooses if the joystick should be visible or not.", EditorStyles.wordWrappedLabel );
						
		GUILayout.Space( 5 );

		// axis
		EditorGUILayout.LabelField( "axis ( typeof( SimpleJoystick.Axis ) )", GUI.skin.GetStyle( "ItemHeader" ) );
		EditorGUILayout.LabelField( "The axis variable will allow the joystick to be clamped along the X, Y, or both axis.", EditorStyles.wordWrappedLabel );
						
		GUILayout.Space( 5 );

		// boundary
		EditorGUILayout.LabelField( "boundary ( typeof( SimpleJoystick.Boundary ) )", GUI.skin.GetStyle( "ItemHeader" ) );
		EditorGUILayout.LabelField( "Boundary determines if the joystick's radius should be clamped to either a square or circular boundary.", EditorStyles.wordWrappedLabel );
						
		GUILayout.Space( 5 );

		// deadZoneOption
		EditorGUILayout.LabelField( "deadZoneOption ( typeof( SimpleJoystick.DeadZoneOption ) )", GUI.skin.GetStyle( "ItemHeader" ) );
		EditorGUILayout.LabelField( "The deadZoneOption will send a modified joystick's position that is determined by the Dead Zone position variables below. If the deadZoneOption is used, then the joystick's position will be either -1, 0, or 1 on each axis used.", EditorStyles.wordWrappedLabel );
								
		GUILayout.Space( 5 );

		// xDeadZone
		EditorGUILayout.LabelField( "xDeadZone ( typeof( float ) )", GUI.skin.GetStyle( "ItemHeader" ) );
		EditorGUILayout.LabelField( "The horizontal distance before the joystick's position will be changed from 0 to either -1 or 1.", EditorStyles.wordWrappedLabel );
								
		GUILayout.Space( 5 );

		// yDeadZone
		EditorGUILayout.LabelField( "yDeadZone ( typeof( float ) )", GUI.skin.GetStyle( "ItemHeader" ) );
		EditorGUILayout.LabelField( "The vertical distance before the joystick's position will be changed from 0 to either -1 or 1.", EditorStyles.wordWrappedLabel );
								
		GUILayout.Space( 5 );

		// exposeValues
		EditorGUILayout.LabelField( "exposeValues ( typeof( boolean ) )", GUI.skin.GetStyle( "ItemHeader" ) );
		EditorGUILayout.LabelField( "Determines if the Simple Joystick should expose the position values.", EditorStyles.wordWrappedLabel );
								
		GUILayout.Space( 5 );

		// horizontalValue
		EditorGUILayout.LabelField( "horizontalValue ( typeof( float ) )", GUI.skin.GetStyle( "ItemHeader" ) );
		EditorGUILayout.LabelField( "The current horizontal position of the joystick exposed in the editor for certain game making plugins.", EditorStyles.wordWrappedLabel );
										
		GUILayout.Space( 5 );

		// verticalValue
		EditorGUILayout.LabelField( "verticalValue ( typeof( float ) )", GUI.skin.GetStyle( "ItemHeader" ) );
		EditorGUILayout.LabelField( "The current vertical position of the joystick exposed in the editor for certain game making plugins", EditorStyles.wordWrappedLabel );
										
		GUILayout.Space( 5 );

		// joystickPreset
		EditorGUILayout.LabelField( "joystickPreset ( typeof( SimpleJoystick.JoystickPreset ) )", GUI.skin.GetStyle( "ItemHeader" ) );
		EditorGUILayout.LabelField( "The current preset of the joystick. This option helps to determine what options the user is presented with.", EditorStyles.wordWrappedLabel );
										
		GUILayout.Space( 5 );

		// joystickState
		EditorGUILayout.LabelField( "joystickState ( typeof( boolean ) )", GUI.skin.GetStyle( "ItemHeader" ) );
		EditorGUILayout.LabelField( "The current state of the joystick. This option helps reference when the joystick is being interacted with.", EditorStyles.wordWrappedLabel );

		EditorGUILayout.EndScrollView();
	}
	#endregion
	
	#region Extras
	void Extras ()
	{
		scrollPos = EditorGUILayout.BeginScrollView( scrollPos, false, false, docSize );
		EditorGUILayout.LabelField( "Videos", GUI.skin.GetStyle( "SectionHeader" ) );
		EditorGUILayout.LabelField( "   The links below are to the collection of videos that we have made in connection with the Simple Joystick. The Tutorial Videos are designed to get the Simple Joystick implemented into your project as fast as possible, and give you a good understanding of what you can achieve using it in your projects, whereas the demonstrations are videos showing how we, and others in the Unity community, have used assets created by Tank and Healer Studio in our projects.", EditorStyles.wordWrappedLabel );

		GUILayout.Space( 10 );

		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if( GUILayout.Button( "Tutorials", buttonSize ) )
			Application.OpenURL( "https://www.youtube.com/playlist?list=PL7crd9xMJ9TnIHHL85cwLDIw5O85JMCh1" );
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

		GUILayout.Space( 10 );

		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if( GUILayout.Button( "Demonstrations", buttonSize ) )
			Application.OpenURL( "https://www.youtube.com/playlist?list=PL7crd9xMJ9TlkjepDAY_GnpA1CX-rFltz" );
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

		GUILayout.Space( 25 );

		EditorGUILayout.LabelField( "Example Scripts", GUI.skin.GetStyle( "SectionHeader" ) );
		EditorGUILayout.LabelField( "   Below is a link to a list of free example script that we have made available on our support website. Please feel free to use these as an example of how to get started on your own scripts. The scripts provided are fully commented to help you to grasp the concept behind the code. These script are by no means a complete solution, and are not intended to be used in finished projects.", EditorStyles.wordWrappedLabel );

		GUILayout.Space( 10 );

		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if( GUILayout.Button( "Example Scripts", buttonSize ) )
			Application.OpenURL( "http://www.tankandhealerstudio.com/sj-example-scripts.html" );
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.EndScrollView();
	}
	#endregion
	
	#region OtherProducts
	void OtherProducts ()
	{
		scrollPos = EditorGUILayout.BeginScrollView( scrollPos, false, false, docSize );

		/* -------------- < ULTIMATE BUTTON > -------------- */
		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.Space( 15 );
		GUILayout.Label( ubPromo, GUILayout.Width( 250 ), GUILayout.Height( 125 ) );
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.LabelField( "Ultimate Button", GUI.skin.GetStyle( "SectionHeader" ) );

		EditorGUILayout.LabelField( "   Buttons are a core element of UI, and as such they should be easy to customize and implement. The Ultimate Button is the embodiment of that very idea. This code package takes the best of Unity's Input and UnityEvent methods and pairs it with exceptional customization to give you the most versatile button for your mobile project. Are you in need of a button for attacking, jumping, shooting, or all of the above? With Ultimate Button's easy size and placement options, style options, and touch actions, you'll have everything you need to create your custom buttons, whether they are simple or complex.", EditorStyles.wordWrappedLabel );

		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if( GUILayout.Button( "More Info", buttonSize ) )
			Application.OpenURL( "http://www.tankandhealerstudio.com/ultimate-button.html" );
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();
		/* ------------ < END ULTIMATE BUTTON > ------------ */

		GUILayout.Space( 25 );

		/* ------------ < ULTIMATE STATUS BAR > ------------ */
		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.Space( 20 );
		GUILayout.Label( usbPromo, GUILayout.Width( 250 ), GUILayout.Height( 125 ) );
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.LabelField( "Ultimate Status Bar", GUI.skin.GetStyle( "SectionHeader" ) );

		EditorGUILayout.LabelField( "   The Ultimate Status Bar is a complete solution for displaying your character's current health, mana, energy, stamina, experience, or virtually any condition that you'd like your player to be aware of. It can also be used to show a selected target's health, the progress of loading or casting, and even interacting with objects. Whatever type of progress display that you need, the Ultimate Status Bar can make it visually happen. Additionally, you have the option of using the many \"Ultimate\" textures provided, or you can easily use your own. If you are looking for a way to neatly display any type of status for your game, then consider the Ultimate Status Bar.", EditorStyles.wordWrappedLabel );

		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if( GUILayout.Button( "More Info", buttonSize ) )
			Application.OpenURL( "http://www.tankandhealerstudio.com/ultimate-status-bar.html" );
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();
		/* -------------- < END STATUS BAR > --------------- */

		GUILayout.Space( 25 );

		/* ------------- < ULTIMATE JOYSTICK > ------------- */
		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.Space( 20 );
		GUILayout.Label( ujPromo, GUILayout.Width( 250 ), GUILayout.Height( 125 ) );
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.LabelField( "Ultimate Joystick", GUI.skin.GetStyle( "SectionHeader" ) );

		EditorGUILayout.LabelField( "   The Ultimate Joystick is a simple, yet powerful tool for the development of your mobile games. The Ultimate Joystick was created with the goal of giving Developers an incredibly versatile joystick solution, while being extremely easy to implement into existing, or new scripts. You don't need to be a programmer to work with the Ultimate Joystick, and it is very easy to implement into any type of character controller that you need. Additionally, Ultimate Joystick's source code is extremely well commented, easy to modify, and features a complete in-engine documentation window, making it ideal for game-specific adjustments. In its entirety, Ultimate Joystick is an elegant and easy to utilize mobile joystick solution.", EditorStyles.wordWrappedLabel );

		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if( GUILayout.Button( "More Info", buttonSize ) )
			Application.OpenURL( "http://www.tankandhealerstudio.com/ultimate-joystick.html" );
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();
		/* ----------- < END ULTIMATE JOYSTICK > ----------- */

		EditorGUILayout.EndScrollView();
	}
	#endregion

	#region Feedback
	void Feedback ()
	{
		scrollPos = EditorGUILayout.BeginScrollView( scrollPos, false, false, docSize );

		EditorGUILayout.LabelField( "Having Problems?", GUI.skin.GetStyle( "SectionHeader" ) );

		EditorGUILayout.LabelField( "   If you experience any issues with the Simple Joystick, please contact us right away. We will lend any assistance that we can to resolve any issues that you have.", EditorStyles.wordWrappedLabel );

		GUILayout.Space( 10 );

		EditorGUILayout.LabelField( "Support Email:\n    tankandhealerstudio@outlook.com" , EditorStyles.boldLabel, GUILayout.Height( 30 ) );

		GUILayout.Space( 25 );

		EditorGUILayout.LabelField( "Good Experiences?", GUI.skin.GetStyle( "SectionHeader" ) );

		EditorGUILayout.LabelField( "   If you have appreciated how easy the Simple Joystick is to get into your project, leave us a comment and rating on the Unity Asset Store. We are very grateful for all positive feedback that we get.", EditorStyles.wordWrappedLabel );

		GUILayout.Space( 10 );

		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if( GUILayout.Button( "Rate Us", buttonSize ) )
			Application.OpenURL( "https://www.assetstore.unity3d.com/#!/content/28685" );
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

		GUILayout.Space( 25 );

		EditorGUILayout.LabelField( "Show Us What You've Done!", GUI.skin.GetStyle( "SectionHeader" ) );

		EditorGUILayout.LabelField( "   If you have used any of the assets created by Tank & Healer Studio in your project, we would love to see what you have done. Contact us with any information on your game and we will be happy to support you in any way that we can!", EditorStyles.wordWrappedLabel );

		GUILayout.Space( 10 );

		EditorGUILayout.LabelField( "Contact Us:\n    tankandhealerstudio@outlook.com" , EditorStyles.boldLabel, GUILayout.Height( 30 ) );

		GUILayout.Space( 10 );

		EditorGUILayout.LabelField( "Happy Game Making,\n	-Tank & Healer Studio", GUILayout.Height( 30 ) );

		GUILayout.Space( 25 );

		EditorGUILayout.EndScrollView();
	}
	#endregion

	#region ThankYou
	void ThankYou ()
	{
		scrollPos = EditorGUILayout.BeginScrollView( scrollPos, false, false, docSize );

		EditorGUILayout.LabelField( "    The two of us at Tank & Healer Studio would like to thank you for purchasing the Simple Joystick asset package from the Unity Asset Store. If you have any questions about the Simple Joystick that are not covered in this Documentation Window, please don't hesitate to contact us at: ", EditorStyles.wordWrappedLabel );

		GUILayout.Space( 10 );

		EditorGUILayout.LabelField( "       tankandhealerstudio@outlook.com" , EditorStyles.boldLabel );

		GUILayout.Space( 10 );

		EditorGUILayout.LabelField( "    We hope that the Simple Joystick will be a great help to you in the development of your game. After pressing the continue button below, you will be presented with helpful information on this asset to assist you in implementing Simple Joystick into your project.\n", EditorStyles.wordWrappedLabel );

		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		EditorGUILayout.LabelField( "Happy Game Making,\n	-Tank & Healer Studio", GUILayout.Height( 30 ) );
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

		GUILayout.Space( 15 );

		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if( GUILayout.Button( "Continue", buttonSize ) )
		{
			EditorPrefs.SetBool( "SimpleJoystickStartup", true );
			Selection.activeObject = AssetDatabase.LoadMainAssetAtPath( "Assets/Plugins/Simple Joystick/README.txt" );
			BackToMainMenu();
		}
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.EndScrollView();
	}
	#endregion
	
	[InitializeOnLoad]
	class InitialLoad
	{
		static InitialLoad ()
		{
			if( EditorPrefs.GetBool( "SimpleJoystickStartup" ) == false )
				EditorApplication.update += WaitForCompile;
		}

		static void WaitForCompile ()
		{
			if( EditorApplication.isCompiling )
				return;

			EditorApplication.update -= WaitForCompile;
				
			currentMenu = CurrentMenu.ThankYou;
			menuTitle = "Thank You!";

			InitializeWindow();

			Selection.activeObject = AssetDatabase.LoadMainAssetAtPath( "Assets/Plugins/Simple Joystick/README.txt" );
		}
	}
}