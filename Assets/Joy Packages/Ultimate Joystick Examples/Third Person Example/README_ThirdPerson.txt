									  -----------------------------------
									  |	THIRD PERSON CHARACTER EXAMPLE  |
									  -----------------------------------

	The Third Person Example files are an adaptation of Unity's default Third Person Controller available in the
Standard Assets download with your version of Unity. If you do not have them already, then you can download them
from the link at the bottom of this text file. For more information about how to customize the Third Person
Example to fit your game's needs, please refer to the text file included with the Standard Assets for the Third
Person Character. The text file is named: ThirdPersonCharacterGuidelines.txt

In order to have the character move and look around using the Ultimate Joystick's input, first we will need to
create two Ultimate Joystick's with our scene. For this example, I created an Ultimate Joystick and anchored it
to the left side of the screen. I then named this joystick "Move" because it will be used to move the character
around the level. Then I created a second Ultimate Joystick within the scene and anchored it to the right side of
the screen. Then, in the Script Reference section, I assigned the name "Look" to the joystick, since this joystick
will be used for making the character look around.

------------
| MOVEMENT |
------------
		Inside the ThirdPersonUserControl.cs script, the read inputs of h and v were previously using Unity's input
	system. We need to replace that input with the Ultimate Joystick. So let's replace the horizontal and vertical
	input with the Ultimate Joystick's horizontal and vertical input.

		float h = UltimateJoystick.GetHorizontalAxis( "Move" );
		float v = UltimateJoystick.GetVerticalAxis( "Move" );
		
	After these floats have been modified to the Ultimate Joystick's input, the character will now be moving using the
	Ultimate Joystick's input.

	BONUS
	-----
	As a bonus feature, we have adapted the character's jump ability to work with the Ultimate Joystick's tap count
	functionality. In order to do this, we have changed the GetButtonDown check from Unity's CrossPlatformInputManager
	to:

		m_Jump = UltimateJoystick.GetTapCount( "Look" );

	The above code will change m_Jump to true only when the user has achieved the tap count determined by the Tap Count
	options.

-----------
| LOOKING |
-----------
		In order to make the player's camera move around the player, we are using Unity's existing code for their
	Pivot Based Camera Rig. The code previously used the CrossPlatformInputManager to get the mouse position and moved 
	the camera accordingly. All we have to do is replace the input with the Ultimate Joystick's input for the "Look"
	joystick.

		var x = UltimateJoystick.GetHorizontalAxis( "Look" );
        var y = UltimateJoystick.GetVerticalAxis( "Look" ) * 0.5f;

	In the above code, I multiply the vertical value by 0.5 to slow the up and down movement within the scene. Feel
	free to mess with this value until it feels comfortable for you.
		
--------------
| CONCLUSION |
--------------
	If you are having any problems with the code, or need any assistance getting the Ultimate Joystick implemented into your
	project, then please contact us at TankAndHealerStudio@outlook.com and we will try to help you out as much as we can!

----------------
| HELPFUL INFO |
----------------
	Support Email:
		TankAndHealerStudio@outlook.com

	Standard Assets Package:
		https://www.assetstore.unity3d.com/en/#!/content/32351

	Support Website:
		http://www.tankandhealerstudio.com/assets.html