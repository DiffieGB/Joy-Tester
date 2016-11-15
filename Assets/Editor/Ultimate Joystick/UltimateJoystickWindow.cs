/* Written by Kaz Crowe */
/* UltimateJoystickWindow.cs */
using UnityEngine;
using UnityEditor;

public class UltimateJoystickWindow : EditorWindow
{
	static string ujVersionNumber = "2.1.2";// ALWAYS UDPATE
	static int importantChanges = 1;// UPDATE ON IMPORTANT CHANGES

	GUILayoutOption[] buttonSize = new GUILayoutOption[] { GUILayout.Width( 200 ), GUILayout.Height( 35 ) }; 
	GUILayoutOption[] docSize = new GUILayoutOption[] { GUILayout.Width( 300 ), GUILayout.Height( 330 ) };

	GUISkin style;

	GUIStyle wordWrapped, headerText, sectionHeader;
	GUIStyle boldTitle, versionNumber, windowTitle, backButton;

	enum CurrentMenu
	{
		MainMenu,
		HowTo,
		Overview,
		Documentation,
		Extras,
		OtherProducts,
		Feedback,
		ThankYou,
		VersionChanges
	}
	static CurrentMenu currentMenu;
	static string menuTitle = "Main Menu";

	Texture2D scriptReference;
	Texture2D positionVisual;

	Texture2D ubPromo, usbPromo, fstpPromo;

	Vector2 scroll_HowTo = Vector2.zero, scroll_Overview = Vector2.zero, scroll_Docs = Vector2.zero, scroll_Extras = Vector2.zero;
	Vector2 scroll_OtherProd = Vector2.zero, scroll_Feedback = Vector2.zero, scroll_Thanks = Vector2.zero;

	int smallSpace = 8;
	int mediumSpace = 12;
	int largeSpace = 20;


	[MenuItem( "Window/Tank and Healer Studio/Ultimate Joystick", false, 0 )]
	static void InitializeWindow ()
	{
		EditorWindow window = GetWindow<UltimateJoystickWindow>( true, "Tank and Healer Studio Asset Window", true );
		window.maxSize = new Vector2( 500, 500 );
		window.minSize = new Vector2( 500, 500 );

		window.Show();
	}

	void OnEnable ()
	{
		style = ( GUISkin ) EditorGUIUtility.Load( "Ultimate Joystick/UltimateJoystickEditorSkin.guiskin" );

		scriptReference = ( Texture2D )EditorGUIUtility.Load( "Ultimate Joystick/UJ_ScriptRef.jpg" );
		positionVisual = ( Texture2D )EditorGUIUtility.Load( "Ultimate Joystick/UJ_PosVisual.png" );
		ubPromo = ( Texture2D ) EditorGUIUtility.Load( "Ultimate UI/UB_Promo.png" );
		usbPromo = ( Texture2D ) EditorGUIUtility.Load( "Ultimate UI/USB_Promo.png" );
		fstpPromo = ( Texture2D ) EditorGUIUtility.Load( "Ultimate UI/FSTP_Promo.png" );

		if( style != null )
		{
			wordWrapped = style.GetStyle( "NormalWordWrapped" );
			headerText = style.GetStyle( "HeaderText" );
			sectionHeader = style.GetStyle( "SectionHeader" );
			boldTitle = style.GetStyle( "BoldTitle" );
			versionNumber = style.GetStyle( "VersionNumber" );
			windowTitle = style.GetStyle( "WindowTitle" );
			backButton = style.GetStyle( "BackButton" );
		}
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
		
		EditorGUILayout.LabelField( "Ultimate Joystick", windowTitle );

		GUILayout.Space( 3 );

		EditorGUILayout.LabelField( " Version " + ujVersionNumber, versionNumber );

		GUILayout.Space( 12 );

		EditorGUILayout.BeginHorizontal();
		GUILayout.Space( smallSpace );
		if( currentMenu != CurrentMenu.MainMenu && currentMenu != CurrentMenu.ThankYou && currentMenu != CurrentMenu.VersionChanges )
		{
			EditorGUILayout.BeginVertical();
			GUILayout.Space( smallSpace );
			if( GUILayout.Button( "", backButton, GUILayout.Width( 80 ), GUILayout.Height( 40 ) ) )
				BackToMainMenu();
			EditorGUILayout.EndVertical();
		}
		else
			GUILayout.Space( 80 );

		GUILayout.Space( 15 );
		EditorGUILayout.BeginVertical();
		GUILayout.Space( 14 );
		EditorGUILayout.LabelField( menuTitle, headerText );
		EditorGUILayout.EndVertical();
		GUILayout.FlexibleSpace();
		GUILayout.Space( 80 );
		EditorGUILayout.EndHorizontal();

		GUILayout.Space( 10 );

		if( currentMenu == CurrentMenu.VersionChanges )
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
			case CurrentMenu.VersionChanges:
			{
				VersionChanges();
			}break;
			default:
			{
				MainMenu();
			}break;
		}
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

		GUILayout.FlexibleSpace();

		GUILayout.Space( largeSpace );
		EditorGUILayout.EndVertical();
	}

	void ErrorScreen ()
	{
		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUIStyle errorStyle = new GUIStyle( GUI.skin.label );
		errorStyle.fixedHeight = 55;
		errorStyle.fixedWidth = 175;
		errorStyle.fontSize = 48;
		errorStyle.normal.textColor = new Color( 1.0f, 0.0f, 0.0f, 1.0f );
		EditorGUILayout.LabelField( "ERROR", errorStyle );
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

		GUILayout.Space( 50 );

		EditorGUILayout.BeginHorizontal();
		GUILayout.Space( 50 );
		EditorGUILayout.LabelField( "Could not find the needed GUISkin located in the Editor Default Resources folder. Please ensure that the correct GUISkin, UltimateJoystickEditorSkin, is in the right folder( Editor Default Resources/Ultimate Joystick ) before trying to access the Ultimate Joystick Window.", EditorStyles.wordWrappedLabel );
		GUILayout.Space( 50 );
		EditorGUILayout.EndHorizontal();
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
		scroll_HowTo = EditorGUILayout.BeginScrollView( scroll_HowTo, false, false, docSize );

		GUI.color = Color.white;

		EditorGUILayout.LabelField( "How To Create", sectionHeader );

		EditorGUILayout.LabelField( "   To create a Ultimate Joystick in your scene, simply go up to GameObject / UI / Ultimate UI / Ultimate Joystick. What this does is locates the Ultimate Joystick prefab that is located within the Editor Default Resources folder, and creates an Ultimate Joystick within the scene.\n\nThis method of adding an Ultimate Joystick to your scene ensures that the joystick will have a Canvas and an EventSystem so that it can work correctly.", wordWrapped );

		GUILayout.Space( largeSpace );

		EditorGUILayout.LabelField( "How To Reference", sectionHeader );
		EditorGUILayout.LabelField( "   One of the great things about the Ultimate Joystick is how easy it is to reference to other scripts. The first thing that you will want to make sure to do is to name the joystick in the Script Reference section. After this is complete, you will be able to reference that particular joystick by it's name from a static function within the Ultimate Joystick script.\n\nAfter the joystick has been given a name in the Script Reference section, we can get that joystick's position by creating a Vector2 variable at run time and storing the result from the static function: 'GetPosition'. This Vector2 will be the joystick's position, and will contain float values between -1, and 1, with 0 being at the center.\n\nKeep in mind that the joystick's left and right ( horizontal ) movement is translated into this Vector2's X value, while the up and down ( vertical ) is the Vector2's Y value. This will be important when applying the Ultimate Joystick's position to your scripts.", wordWrapped );
			
		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.Space( 10 );
		GUILayout.Label( positionVisual, GUILayout.Width( 200 ), GUILayout.Height( 200 ) );
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

		GUILayout.Space( largeSpace );

		EditorGUILayout.LabelField( "Example", sectionHeader );

		EditorGUILayout.LabelField( "   Let's assume that we want to use a joystick for a characters movement. The first thing to do is to assign the name \"Movement\" in the Joystick Name variable located in the Script Reference section of the Ultimate Joystick.", wordWrapped );
		
		GUILayout.Space( 10 );

		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.Label( scriptReference );
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

		GUILayout.Space( 10 );

		EditorGUILayout.LabelField( "After that, we need to create a Vector2 variable to store the result of the joystick's position returned by the 'GetPosition' function. In order to get the \"Movement\" joystick's position, we need to pass in the name \"Movement\" as the argument.", wordWrapped );

		GUILayout.Space( 10 );

		EditorGUILayout.LabelField( "C# Example:", boldTitle );
		EditorGUILayout.TextArea( "Vector2 joystickPosition = UltimateJoystick.GetPosition( \"Movement\" );", GUI.skin.GetStyle( "TextArea" ) );

		GUILayout.Space( 10 );

		EditorGUILayout.LabelField( "Javascript Example:", boldTitle );
		EditorGUILayout.TextArea( "var joystickPosition : Vector2 = UltimateJoystick.GetPosition( \"Movement\" );", GUI.skin.GetStyle( "TextArea" ) );

		GUILayout.Space( 10 );

		EditorGUILayout.LabelField( "The joystickPosition variable now contains the values of the position that the Movement joystick was in at the movement it was referenced. Now we can use this information in any way that is desired. For example, if you are wanting to put the joystick's position into a character movement script, you would create a Vector3 variable for movement direction, and put in the appropriate values of the Ultimate Joystick's position.", wordWrapped );

		GUILayout.Space( 10 );

		EditorGUILayout.LabelField( "C# Example:", boldTitle );
		EditorGUILayout.TextArea( "Vector3 movementDirection = new Vector3( joystickPosition.x, 0, joystickPosition.y );", GUI.skin.GetStyle( "TextArea" ) );

		GUILayout.Space( 10 );

		EditorGUILayout.LabelField( "Javascript Example:", boldTitle );
		EditorGUILayout.TextArea( "var movementDirection : Vector3 = new Vector3( joystickPosition.x, 0, joystickPosition.y );", GUI.skin.GetStyle( "TextArea" ) );

		GUILayout.Space( 10 );

		EditorGUILayout.LabelField( "In the above example, the joystickPosition variable is used to give the movement direction values in the X and Z directions. This is because you generally don't want your character to move in the Y direction unless the user jumps. That is why we put the joystickPosition.y value into the Z value of the movementDirection variable.", wordWrapped );

		GUILayout.Space( 10 );

		EditorGUILayout.LabelField( "Understanding how to use the values from any input is important when creating character controllers, so experiment with the values and try to understand how the mobile input can be used in different ways.", wordWrapped );

		GUILayout.Space( mediumSpace );

		EditorGUILayout.EndScrollView();
	}
	#endregion
	
	#region Overview
	void Overview ()
	{
		scroll_Overview = EditorGUILayout.BeginScrollView( scroll_Overview, false, false, docSize );

		EditorGUILayout.LabelField( "Assigned Variables", sectionHeader );
		EditorGUILayout.LabelField( "   In the Assigned Variables section, there are a few components that should already be assigned if you are using one of the Prefabs that has been provided. If not, you will see error messages on the Ultimate Joystick inspector that will help you to see if any of these variables are left unassigned. Please note that these need to be assigned in order for the Ultimate Joystick to work properly.", wordWrapped );

		GUILayout.Space( largeSpace );
		
		/* //// --------------------------- < SIZE AND PLACEMENT > --------------------------- \\\\ */
		EditorGUILayout.LabelField( "Size And Placement", sectionHeader );
		EditorGUILayout.LabelField( "   The Size and Placement section allows you to customize the joystick's size and placement on the screen, as well as determine where the user's touch can be processed for the selected joystick.", wordWrapped );

		GUILayout.Space( smallSpace );

		// Scaling Axis
		EditorGUILayout.LabelField( "« Scaling Axis »", boldTitle );
		EditorGUILayout.LabelField( "Determines which axis the joystick will be scaled from. If Height is chosen, then the joystick will scale itself proportionately to the Height of the screen.", wordWrapped );

		GUILayout.Space( smallSpace );

		// Anchor
		EditorGUILayout.LabelField( "« Anchor »", boldTitle );
		EditorGUILayout.LabelField( "Determines which side of the screen that the joystick will be anchored to.", wordWrapped );
		
		GUILayout.Space( smallSpace );

		// Touch Size
		EditorGUILayout.LabelField( "« Touch Size »", boldTitle );
		EditorGUILayout.LabelField( "Touch Size configures the size of the area where the user can touch. You have the options of either 'Default','Medium', 'Large' or 'Custom'. When the option 'Custom' is selected, an additional box will be displayed that allows for a more adjustable touch area.", wordWrapped );
				
		GUILayout.Space( smallSpace );

		// Touch Size Customization
		EditorGUILayout.LabelField( "« Touch Size Customization »", boldTitle );
		EditorGUILayout.LabelField( "If the 'Custom' option of the Touch Size is selected, then you will be presented with the Touch Size Customization box. Inside this box are settings for the Width and Height of the touch area, as well as the X and Y position of the touch area on the screen.", wordWrapped );
				
		GUILayout.Space( smallSpace );

		// Dynamic Positioning
		EditorGUILayout.LabelField( "« Dynamic Positioning »", boldTitle );
		EditorGUILayout.LabelField( "Dynamic Positioning will make the joystick snap to where the user touches, instead of the user having to touch a direct position to get the joystick. The Touch Size option will directly affect the area where the joystick can snap to.", wordWrapped );
				
		GUILayout.Space( smallSpace );

		// Joystick Size
		EditorGUILayout.LabelField( "« Joystick Size »", boldTitle );
		EditorGUILayout.LabelField( "Joystick Size will change the scale of the joystick. Since everything is calculated out according to screen size, your joystick Touch Size and other properties will scale proportionately with the joystick's size along your specified Scaling Axis.", wordWrapped );
				
		GUILayout.Space( smallSpace );

		// Radius
		EditorGUILayout.LabelField( "« Radius »", boldTitle );
		EditorGUILayout.LabelField( "Radius determines how far away the joystick will move from center when it is being used, and will scale proportionately with the joystick.", wordWrapped );
				
		GUILayout.Space( smallSpace );

		// Joystick Position
		EditorGUILayout.LabelField( "« Joystick Position »", boldTitle );
		EditorGUILayout.LabelField( "Joystick Position will present you with two sliders. The X value will determine how far the Joystick is away from the Left and Right sides of the screen, and the Y value from the Top and Bottom. This will encompass 50% of your screen width, relevant to your Anchor selection.", wordWrapped );
		/* \\\\ -------------------------- < END SIZE AND PLACEMENT > --------------------------- //// */

		GUILayout.Space( largeSpace );

		/* //// ----------------------------- < STYLE AND OPTIONS > ----------------------------- \\\\ */
		EditorGUILayout.LabelField( "Style And Options", sectionHeader );
		EditorGUILayout.LabelField( "   The Style and Options section contains options that affect how the joystick handles and is visually presented to the user.", wordWrapped );
		
		GUILayout.Space( smallSpace );

		// -----< VISUAL DISPLAY >----- //

		// Disable Visuals
		EditorGUILayout.LabelField( "« Disable Visuals »", boldTitle );
		EditorGUILayout.LabelField( "Disable Visuals presents you with the option to disable the visuals of the joystick, whilst keeping all functionality. When paired with Dynamic Positioning and Throwable, this option can give you a very smooth camera control.", wordWrapped );
		
		GUILayout.Space( smallSpace );

		// Use Fade
		EditorGUILayout.LabelField( "« Use Fade »", boldTitle );
		EditorGUILayout.LabelField( "The Use Fade option will present you with settings for the targeted alpha for the touched and untouched states, as well as the duration for the fade between the targeted alpha settings.", wordWrapped );

		GUILayout.Space( smallSpace );

		// Use Animation
		EditorGUILayout.LabelField( "« Use Animation »", boldTitle );
		EditorGUILayout.LabelField( "If you would like the joystick to play an animation when being interacted with, then you will want to enable the Use Animation option.", wordWrapped );

		GUILayout.Space( smallSpace );

		// Show Highlight
		EditorGUILayout.LabelField( "« Show Highlight »", boldTitle );
		EditorGUILayout.LabelField( "Show Highlight will allow you to customize the set highlight images with a custom color. With this option, you will also be able to customize and set the highlight color at runtime using the UpdateHighlightColor function.", wordWrapped );

		GUILayout.Space( smallSpace );

		// Show Tension
		EditorGUILayout.LabelField( "« Show Tension »", boldTitle );
		EditorGUILayout.LabelField( "With Show Tension enabled, the joystick will display it's position visually. This is done using custom colors and images that will display the direction and intensity of the joystick's current position. With this option enabled, you will be able to update the tension colors at runtime using the UpdateTensionColors function.", wordWrapped );
		
		GUILayout.Space( smallSpace );

		// -----< FUNCTIONALITY >----- //

		// Throwable
		EditorGUILayout.LabelField( "« Throwable »", boldTitle );
		EditorGUILayout.LabelField( "The Throwable option allows the joystick to smoothly transition back to center after being released. This can be used to give your input a smoother feel.", wordWrapped );
		
		GUILayout.Space( smallSpace );
		
		// Draggable
		EditorGUILayout.LabelField( "« Draggable »", boldTitle );
		EditorGUILayout.LabelField( "The Draggable option will allow the joystick to move from it's default position when the user's input exceeds the set radius.", wordWrapped );

		GUILayout.Space( smallSpace );

		// Axis
		EditorGUILayout.LabelField( "« Axis »", boldTitle );
		EditorGUILayout.LabelField( "Axis determines which axis the joystick will snap to. By default it is set to Both, which means the joystick will use both the X and Y axis for movement. If either the X or Y option is selected, then the joystick will lock to the corresponding axis.", wordWrapped );
		
		GUILayout.Space( smallSpace );

		// Boundary
		EditorGUILayout.LabelField( "« Boundary »", boldTitle );
		EditorGUILayout.LabelField( "Boundary will allow you to decide if you want to have a square or circular edge to your joystick.", wordWrapped );
		
		GUILayout.Space( smallSpace );

		// Dead Zone
		EditorGUILayout.LabelField( "« Dead Zone »", boldTitle );
		EditorGUILayout.LabelField( "Dead Zone gives the option of setting one or two values that the joystick is constrained by. When selected, the joystick will be forced to a maximum value when it has past the set dead zone.", wordWrapped );

		GUILayout.Space( smallSpace );

		// Tap Count
		EditorGUILayout.LabelField( "« Tap Count »", boldTitle );
		EditorGUILayout.LabelField( "The Tap Count option allows you to decide if you want to store the amount of taps that the joystick recieves. The options provided with the Tap Count will allow you to customize the target amount of taps and the amount of time the user will have to accumulate these taps.", wordWrapped );
		/* //// --------------------------- < END STYLE AND OPTIONS > --------------------------- \\\\ */

		GUILayout.Space( largeSpace );

		/* //// ----------------------------- < SCRIPT REFERENCE > ------------------------------ \\\\ */
		EditorGUILayout.LabelField( "Script Reference", sectionHeader );
		EditorGUILayout.LabelField( "   The Script Reference section contains fields for naming and helpful code snippets that you can copy and paste into your scripts. This section can also expose the horizontal and vertical values so that they can be referenced by certain game making plugins.", wordWrapped );
		
		GUILayout.Space( smallSpace );
		
		// Joystick Name
		EditorGUILayout.LabelField( "« Joystick Name »", boldTitle );
		EditorGUILayout.LabelField( "The unique name of your Ultimate Joystick. This name is what will be used to reference this particular joystick from the public static functions.", wordWrapped );
		
		GUILayout.Space( smallSpace );

		// Joystick Use
		EditorGUILayout.LabelField( "« Joystick Use »", boldTitle );
		EditorGUILayout.LabelField( "This option will present you with a code snippet that is determined by your selection. This code can be copy and pasted into your custom scripts. Please note that the Joystick Use option does not actually determine what the joystick can do. Instead it only provides example code for you to use in your scripts.", wordWrapped );

		GUILayout.Space( smallSpace );

		// Expose Values
		EditorGUILayout.LabelField( "« Expose Values »", boldTitle );
		EditorGUILayout.LabelField( "The Expose Values option will expose the horizontal and vertical values into public variables that can be accessed by certain game making pugins. This option can also be used for debugging the Ultimate Joystick to see what the values are when you are using them.", wordWrapped );

		GUILayout.Space( mediumSpace );

		EditorGUILayout.EndScrollView();
	}
	#endregion
	
	#region Documentation
	void Documentation ()
	{
		scroll_Docs = EditorGUILayout.BeginScrollView( scroll_Docs, false, false, docSize );
		
		/* //// --------------------------- < PUBLIC FUNCTIONS > --------------------------- \\\\ */
		EditorGUILayout.LabelField( "Public Functions", sectionHeader );

		GUILayout.Space( smallSpace );

		// Vector2 GetPosition
		EditorGUILayout.LabelField( "Vector2 GetPosition()", boldTitle );
		EditorGUILayout.LabelField( "Returns the Ultimate Joystick's position in a Vector2 variable. The X value that is returned represents the Left and Right( Horizontal ) position of the joystick, whereas the Y value represents the Up and Down( Vertical ) position of the joystick. The values returned will always be between -1 and 1, with 0 being the center.", wordWrapped );

		GUILayout.Space( smallSpace );

		// float GetDistance
		EditorGUILayout.LabelField( "float GetDistance()", boldTitle );
		EditorGUILayout.LabelField( "Returns the distance of the joystick from it's center in a float value. The value returned will always be a value between 0 and 1.", wordWrapped );

		GUILayout.Space( smallSpace );

		// UpdatePositioning()
		EditorGUILayout.LabelField( "UpdatePositioning()", boldTitle );
		EditorGUILayout.LabelField( "Updates the size and positioning of the Ultimate Joystick. This function can be used to update any options that may have been changed prior to Start().", wordWrapped );
		
		GUILayout.Space( smallSpace );

		// ResetJoystick()
		EditorGUILayout.LabelField( "ResetJoystick()", boldTitle );
		EditorGUILayout.LabelField( "Resets the joystick back to it's neutral state.", wordWrapped );
						
		GUILayout.Space( smallSpace );

		// UpdateHighlightColor()
		EditorGUILayout.LabelField( "UpdateHighlightColor( Color targetColor )", boldTitle );
		EditorGUILayout.LabelField( "Updates the colors of the assigned highlight images with the targeted color if the showHighlight variable is set to true. The targetColor variable will overwrite the current color setting for highlightColor and apply immediately to the highlight images.", wordWrapped );
				
		GUILayout.Space( smallSpace );

		// UpdateTensionColors()
		EditorGUILayout.LabelField( "UpdateTensionColors( Color targetTensionNone, Color targetTensionFull )", boldTitle );
		EditorGUILayout.LabelField( "Updates the tension accent image colors with the targeted colors if the showTension variable is true. The tension colors will be set to the targeted colors, and will be applied when the joystick is next used.", wordWrapped );

		GUILayout.Space( smallSpace );

		// bool GetTapCount
		EditorGUILayout.LabelField( "bool GetTapCount", boldTitle );
		EditorGUILayout.LabelField( "Returns the current state of the joystick's Tap Count according to the options set. The boolean returned will be true only after the Tap Count options have been achieved from the users input.", wordWrapped );
		
		GUILayout.Space( smallSpace );

		// float GetHorizontalAxis
		EditorGUILayout.LabelField( "float GetHorizontalAxis", boldTitle );
		EditorGUILayout.LabelField( "Returns the current horizontal value of the joystick's position. The value returned will always be between -1 and 1, with 0 being the neutral position.", wordWrapped );

		GUILayout.Space( smallSpace );

		// float GetVerticalAxis
		EditorGUILayout.LabelField( "float GetVerticalAxis", boldTitle );
		EditorGUILayout.LabelField( "Returns the current vertical value of the joystick's position. The value returned will always be between -1 and 1, with 0 being the neutral position.", wordWrapped );
		
		GUILayout.Space( smallSpace );

		// bool GetJoystickState
		EditorGUILayout.LabelField( "bool GetJoystickState", boldTitle );
		EditorGUILayout.LabelField( "Returns the state that the joystick is currently in. This function will return true when the joystick is being interacted with, and false when not.", wordWrapped );

		GUILayout.Space( largeSpace );
		
		/* //// --------------------------- < STATIC FUNCTIONS > --------------------------- \\\\ */
		EditorGUILayout.LabelField( "Static Functions", sectionHeader );

		GUILayout.Space( smallSpace );

		// UltimateJoystick.GetPosition
		EditorGUILayout.LabelField( "Vector2 UltimateJoystick.GetPosition( string joystickName )", boldTitle );
		EditorGUILayout.LabelField( "Returns the Ultimate Joystick's position in a Vector2 variable. This static function will return the same exact value as the local GetPosition function. See GetPosition for more information.", wordWrapped );
		GUILayout.Space( smallSpace );

		// UltimateJoystick.GetDistance
		EditorGUILayout.LabelField( "float UltimateJoystick.GetDistance( string joystickName )", boldTitle );
		EditorGUILayout.LabelField( "Returns the distance of the joystick from it's center in a float value. This static function will return the same value as the local GetDistance function. See GetDistance for more information", wordWrapped );
		
		GUILayout.Space( smallSpace );

		// UltimateJoystick.GetJoystickState
		EditorGUILayout.LabelField( "bool UltimateJoystick.GetJoystickState( string joystickName )", boldTitle );
		EditorGUILayout.LabelField( "Returns the state that the targeted joystick is currently in. This function will return true when the joystick is being interacted with, and false when not.", wordWrapped );
				
		GUILayout.Space( smallSpace );

		// UltimateJoystick.GetJoystick()
		EditorGUILayout.LabelField( "UltimateJoystick UltimateJoystick.GetJoystick( string joystickName )", boldTitle );
		EditorGUILayout.LabelField( "Returns the instance of the Ultimate Joystick in the current scene with the targeted name. This function allows the user to call any public functions and modify any public variables of the returned Ultimate Joystick.", wordWrapped );

		GUILayout.Space( smallSpace );

		// UltimateJoystick.GetHorizontalAxis()
		EditorGUILayout.LabelField( "float UltimateJoystick.GetHorizontalAxis( string joystickName )", boldTitle );
		EditorGUILayout.LabelField( "Returns the current horizontal value of the targeted joystick's position. The value returned will always be between -1 and 1, with 0 being the neutral position.", wordWrapped );

		GUILayout.Space( smallSpace );

		// UltimateJoystick.GetVerticalAxis()
		EditorGUILayout.LabelField( "float UltimateJoystick.GetVerticalAxis( string joystickName )", boldTitle );
		EditorGUILayout.LabelField( "Returns the current vertical value of the targeted joystick's position. The value returned will always be between -1 and 1, with 0 being the neutral position.", wordWrapped );

		GUILayout.Space( smallSpace );

		// UltimateJoystick.GetTapCount()
		EditorGUILayout.LabelField( "bool UltimateJoystick.GetTapCount( string joystickName )", boldTitle );
		EditorGUILayout.LabelField( "Returns the current state of the targeted joystick's Tap Count according to the options set. The boolean returned will be true only after the Tap Count options have been achieved from the users input.", wordWrapped );

		GUILayout.Space( mediumSpace );

		EditorGUILayout.EndScrollView();
	}
	#endregion
	
	#region Extras
	void Extras ()
	{
		scroll_Extras = EditorGUILayout.BeginScrollView( scroll_Extras, false, false, docSize );
		EditorGUILayout.LabelField( "Videos", sectionHeader );
		EditorGUILayout.LabelField( "   The links below are to the collection of videos that we have made in connection with the Ultimate Joystick. The Tutorial Videos are designed to get the Ultimate Joystick implemented into your project as fast as possible, and give you a good understanding of what you can achieve using it in your projects, whereas the demonstrations are videos showing how we, and others in the Unity community, have used assets created by Tank & Healer Studio in our projects.", wordWrapped );

		GUILayout.Space( 10 );

		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if( GUILayout.Button( "Tutorials", buttonSize ) )
			Application.OpenURL( "https://www.youtube.com/playlist?list=PL7crd9xMJ9TmWdbR_bklluPeElJ_xUdO9" );
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

		EditorGUILayout.LabelField( "Example Scripts", sectionHeader );
		EditorGUILayout.LabelField( "   Below is a link to a list of free example scripts that we have made available on our support website. Please feel free to use these as an example of how to get started on your own scripts. The scripts provided are fully commented to help you to grasp the concept behind the code. These scripts are by no means a complete solution, and are not intended to be used in finished projects.", wordWrapped );

		GUILayout.Space( 10 );

		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if( GUILayout.Button( "Example Scripts", buttonSize ) )
			Application.OpenURL( "http://www.tankandhealerstudio.com/uj-example-scripts.html" );
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.EndScrollView();
	}
	#endregion
	
	#region OtherProducts
	void OtherProducts ()
	{
		scroll_OtherProd = EditorGUILayout.BeginScrollView( scroll_OtherProd, false, false, docSize );

		/* -------------- < ULTIMATE BUTTON > -------------- */
		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.Space( 15 );
		GUILayout.Label( ubPromo, GUILayout.Width( 250 ), GUILayout.Height( 125 ) );
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

		GUILayout.Space( smallSpace );

		EditorGUILayout.LabelField( "Ultimate Button", sectionHeader );

		EditorGUILayout.LabelField( "   Buttons are a core element of UI, and as such they should be easy to customize and implement. The Ultimate Button is the embodiment of that very idea. This code package takes the best of Unity's Input and UnityEvent methods and pairs it with exceptional customization to give you the most versatile button for your mobile project. Are you in need of a button for attacking, jumping, shooting, or all of the above? With Ultimate Button's easy size and placement options, style options, script reference and button events, you'll have everything you need to create your custom buttons, whether they are simple or complex.", wordWrapped );

		GUILayout.Space( 10 );

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
		GUILayout.Space( 15 );
		GUILayout.Label( usbPromo, GUILayout.Width( 250 ), GUILayout.Height( 125 ) );
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

		GUILayout.Space( smallSpace );

		EditorGUILayout.LabelField( "Ultimate Status Bar", sectionHeader );

		EditorGUILayout.LabelField( "   The Ultimate Status Bar is a complete solution for displaying health, mana, energy, stamina, experience, or virtually any condition that you'd like like your player to be aware of. It can also be used to show a selected target's health, the progress of loading or casting, and even interacting with objects. Whatever type of progress display that you need, the Ultimate Status Bar can make it visually happen. Additionally, you have the option of using the many \"Ultimate\" textures provided, or you can easily use your own! If you are looking for a way to neatly display any type of status for your game, then consider the Ultimate Status Bar!", wordWrapped );

		GUILayout.Space( 10 );

		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if( GUILayout.Button( "More Info", buttonSize ) )
			Application.OpenURL( "http://www.tankandhealerstudio.com/ultimate-status-bar.html" );
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();
		/* -------------- < END STATUS BAR > --------------- */

		GUILayout.Space( 25 );

		/* -------------- < FROST STONE > -------------- */
		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.Space( 15 );
		GUILayout.Label( fstpPromo, GUILayout.Width( 250 ), GUILayout.Height( 125 ) );
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

		GUILayout.Space( smallSpace );

		EditorGUILayout.LabelField( "Frost Stone: UI Texture Pack", sectionHeader );

		EditorGUILayout.LabelField( "   This package is made to compliment Ultimate Joystick, Ultimate Button and Ultimate Status Bar. The Frost Stone: UI Texture Pack is an inspiring new look for your Ultimate Joystick, Ultimate Button and Ultimate Status Bar. These Frost Stone Textures will flawlessly blend with your current Ultimate UI code to give your game an incredible new look.", wordWrapped );

		GUILayout.Space( 10 );

		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if( GUILayout.Button( "More Info", buttonSize ) )
			Application.OpenURL( "http://www.tankandhealerstudio.com/frost-stone-texture-pack.html" );
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();
		/* ------------ < END FROST STONE > ------------ */

		EditorGUILayout.EndScrollView();
	}
	#endregion

	#region Feedback
	void Feedback ()
	{
		scroll_Feedback = EditorGUILayout.BeginScrollView( scroll_Feedback, false, false, docSize );

		EditorGUILayout.LabelField( "Having Problems?", sectionHeader );

		EditorGUILayout.LabelField( "   If you experience any issues with the Ultimate Joystick, please contact us right away. We will lend any assistance that we can to resolve any issues that you have. Either contact us at tankandhealerstudio@outlook.com or complete the Contact Form on our website.", wordWrapped );

		GUILayout.Space( smallSpace );

		//EditorGUILayout.LabelField( "Support Email:\n    tankandhealerstudio@outlook.com", boldTitle, GUILayout.Height( 30 ) );
		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		EditorGUILayout.LabelField( "tankandhealerstudio@outlook.com", boldTitle, GUILayout.Width( 230 ) );
		GUILayout.Space( 10 );
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

		GUILayout.Space( smallSpace );

		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if( GUILayout.Button( "Contact Form", buttonSize ) )
			Application.OpenURL( "http://www.tankandhealerstudio.com/contact-us.html" );
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

		GUILayout.Space( 25 );

		EditorGUILayout.LabelField( "Good Experiences?", sectionHeader );

		EditorGUILayout.LabelField( "   If you have appreciated how easy the Ultimate Joystick is to get into your project, leave us a comment and rating on the Unity Asset Store. We are very grateful for all positive feedback that we get.", wordWrapped );

		GUILayout.Space( 10 );

		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if( GUILayout.Button( "Rate Us", buttonSize ) )
			Application.OpenURL( "https://www.assetstore.unity3d.com/en/#!/content/27695" );
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

		GUILayout.Space( 25 );

		EditorGUILayout.LabelField( "Show Us What You've Done!", sectionHeader );

		EditorGUILayout.LabelField( "   If you have used any of the assets created by Tank & Healer Studio in your project, we would love to see what you have done. Contact us with any information on your game and we will be happy to support you in any way that we can!", wordWrapped );

		GUILayout.Space( smallSpace );

		EditorGUILayout.LabelField( "Contact Us:\n    tankandhealerstudio@outlook.com" , boldTitle, GUILayout.Height( 30 ) );

		GUILayout.Space( 10 );

		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		EditorGUILayout.LabelField( "Happy Game Making,\n	-Tank & Healer Studio", GUILayout.Height( 30 ) );
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

		GUILayout.Space( 25 );

		EditorGUILayout.EndScrollView();
	}
	#endregion

	#region ThankYou
	void ThankYou ()
	{
		scroll_Thanks = EditorGUILayout.BeginScrollView( scroll_Thanks, false, false, docSize );

		GUILayout.Space( 10 );

		EditorGUILayout.LabelField( "    The two of us at Tank & Healer Studio would like to thank you for purchasing the Ultimate Joystick asset package from the Unity Asset Store. If you have any questions about the Ultimate Joystick that are not covered in this Documentation Window, please don't hesitate to contact us at: ", wordWrapped );

		GUILayout.Space( 10 );

		EditorGUILayout.LabelField( "       tankandhealerstudio@outlook.com" , boldTitle );

		GUILayout.Space( 10 );

		EditorGUILayout.LabelField( "    We hope that the Ultimate Joystick will be a great help to you in the development of your game. After pressing the continue button below, you will be presented with helpful information on this asset to assist you in implementing Ultimate Joystick into your project.\n", wordWrapped );

		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		EditorGUILayout.LabelField( "Happy Game Making,\n	-Tank & Healer Studio", GUILayout.Height( 30 ) );
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

		GUILayout.Space( 15 );

		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if( GUILayout.Button( "Continue", buttonSize ) )
			BackToMainMenu();
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.EndScrollView();
	}
	#endregion

	#region VersionChanges
	void VersionChanges ()
	{
		scroll_Thanks = EditorGUILayout.BeginScrollView( scroll_Thanks, false, false, docSize );

		GUILayout.Space( 10 );
		
		EditorGUILayout.LabelField( "  Thank you for downloading the most recent version of the Ultimate Joystick. There is some exciting new functionality as well as some changes that could affect any existing reference of the Ultimate Joystick. Please check out the sections below to see all the important changes that have been made. As always, if you run into any issues with the Ultimate Joystick, please contact us at:", wordWrapped );

		GUILayout.Space( 3 );
		EditorGUILayout.LabelField( "  TankAndHealerStudio@outlook.com" , boldTitle );
		GUILayout.Space( 15 );

		EditorGUILayout.LabelField( "GENERAL CHANGES", sectionHeader );
		EditorGUILayout.LabelField( "  Some folder structure and existing functionality has been updated and improved. None of these changes should affect any existing use of the Ultimate Joystick.", wordWrapped );
		EditorGUILayout.LabelField( "  • Removed example files from the Plugins folder. All example files will now be in the folder named: Ultimate Joystick Examples.", wordWrapped );
		EditorGUILayout.LabelField( "  • Added new example scene: Asteroids.", wordWrapped );
		EditorGUILayout.LabelField( "  • Modified the Third Person Example scene to better implement the Ultimate Joystick and show more of its potential.", wordWrapped );
		EditorGUILayout.LabelField( "  • Added four new Ultimate Joystick textures that can be used in your projects.", wordWrapped );
		EditorGUILayout.LabelField( "  • Modified the DisplayTensionAccents function to accept a limited number of tension directions. Now up, down, left, or right can be used as the only tension accents without causing any unwanted behavior.", wordWrapped );
		EditorGUILayout.LabelField( "  • Added helpful text to the Ultimate Joystick Editor to help bring attention to variables that have not been assigned.", wordWrapped );
		EditorGUILayout.LabelField( "  • Removed the Ultimate Joystick PSD from the project files.", wordWrapped );

		GUILayout.Space( 10 );

		EditorGUILayout.LabelField( "NEW FUNCTIONS", sectionHeader );
		EditorGUILayout.LabelField( "  Some new functions have been added to help reference the Ultimate Joystick more efficiently. For information on what each new function does, please refer to the Documentation section of this help window.", wordWrapped );
		EditorGUILayout.LabelField( "  • float GetHorizontalAxis()", wordWrapped );
		EditorGUILayout.LabelField( "  • float GetVerticalAxis()", wordWrapped );
		EditorGUILayout.LabelField( "  • UltimateJoystick GetJoystick()", wordWrapped );
		EditorGUILayout.LabelField( "  • bool GetTapCount()", wordWrapped );

		GUILayout.Space( 10 );
		
		EditorGUILayout.LabelField( "REMOVED FUNCTIONS", sectionHeader );
		EditorGUILayout.LabelField( "  Only the public static functions have been removed. All functionality remains the same for the public version of each function. To replace any existing code that uses these removed functions, please use the GetJoystick function.", wordWrapped );
		EditorGUILayout.LabelField( "  • UpdatePositioning()", wordWrapped );
		EditorGUILayout.LabelField( "  • ResetJoystick()", wordWrapped );
		EditorGUILayout.LabelField( "  • UpdateHighlightColor()", wordWrapped );
		EditorGUILayout.LabelField( "  • UpdateTensionColors()", wordWrapped );
		
		GUILayout.Space( 10 );
		
		EditorGUILayout.LabelField( "RENAMED FUNCTIONS", sectionHeader );
		EditorGUILayout.LabelField( "  Some of the public functions have been renamed to match the public static functions used more commonly. If you are using any of these public functions, please be sure to update them to the corresponding renamed function.", wordWrapped );
		EditorGUILayout.LabelField( "  • JoystickPosition has been renamed to GetPosition.", wordWrapped );
		EditorGUILayout.LabelField( "  • JoystickDistance has been renamed to GetDistance.", wordWrapped );
		EditorGUILayout.LabelField( "  • JoystickState has been renamed to GetJoystickState", wordWrapped );

		GUILayout.Space( 15 );

		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if( GUILayout.Button( "Got it!", buttonSize ) )
			BackToMainMenu();
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.EndScrollView();
	}
	#endregion
	
	[InitializeOnLoad]
	class UltimateJoystickInitialLoad
	{
		static UltimateJoystickInitialLoad ()
		{
			// If the user has a older version of UJ that used the bool for startup...
			if( EditorPrefs.HasKey( "UltimateJoystickStartup" ) && !EditorPrefs.HasKey( "UltimateJoystickVersion" ) )
			{
				// Set the new pref to 0 so that the pref will exist and the version changes will be shown.
				EditorPrefs.SetInt( "UltimateJoystickVersion", 0 );
			}

			// If this is the first time that the user has downloaded the Ultimate Joystick...
			if( !EditorPrefs.HasKey( "UltimateJoystickVersion" ) )
			{
				// Set the current menu to the thank you page.
				currentMenu = CurrentMenu.ThankYou;
				menuTitle = "Thank You";

				// Set the version to current so they won't see these version changes.
				EditorPrefs.SetInt( "UltimateJoystickVersion", importantChanges );

				EditorApplication.update += WaitForCompile;
			}
			else if( EditorPrefs.GetInt( "UltimateJoystickVersion" ) < importantChanges )
			{
				// Set the current menu to the version changes page.
				currentMenu = CurrentMenu.VersionChanges;
				menuTitle = "Version Changes";

				// Set the version to current so they won't see this page again.
				EditorPrefs.SetInt( "UltimateJoystickVersion", importantChanges );

				EditorApplication.update += WaitForCompile;
			}
		}

		static void WaitForCompile ()
		{
			if( EditorApplication.isCompiling )
				return;

			EditorApplication.update -= WaitForCompile;

			InitializeWindow();
		}
	}
}