	/* 
	 *   YOU CAN FIND AN IMAGE VERSION OF THIS TUTORIAL AND MORE DOCUMENTATION ON THE MIXAMO COMMUNITY
	 *                                
	 *											 COMMUNITY.MIXAMO.COM
	 * 
	 * 
	 * 																Happy creating!
	 * 
	 *																 - The Mixamo Team
	 * 
	 */


/*Intro to Animators in Unity
 * 
 * Animator controllers and control scripts are designed to work together.
 * Take a look at the "DemoAnimatorController" in the Mixamo > Demo folder.  To view the animator information open the
 * Window > Animator panel with the asset selected in the Project view.
 * 
 * Looking at the window you'll see several 'boxes' with different names, colors and lines connecting them.
 * 
 * Each box is a state, usually with one animation, but sometimes it could have multiples.  The orange box is the default
 * or starting state.  Blue hexagonal rectangles are sub-state machines, essentially logical layers that have their own states.
 * Each white line is a transition with rules about when to transition between states.
 * 
 * You can click on a state to see the animation that is played when the animator is in that state.  For example, clicking
 * on the "jump" state the inspector will update to show that a .anim called jump is assigned to the motion field.  You can
 * also see the transitions available to and from a state.  In the jump example, there is one transition listed as
 * "Jump -> IdleWalkingBlend".
 * 
 * To see the rules of a transition click on the arrow between states.  Click on the arrow between "Jump" and "IdleWalkingBlend".
 * You can see in the inspector that the "Conditions" for that transition is Exit Time of 0.88.  This means that it will play the
 * animation to 0.88 (or 88% complete) and then transition into the next animation.
 * 
 * You can also give custom conditions based on integer, float and boolean inputs.  On the bottom left of the Animator window
 * you'll see a Parameters list.  These are custom parameters we've added to control the animations in our Animator.  Jumping is a
 * boolean (true or false), while VSpeed is a float (decimal)
 * 
 * Another useful feature is the ability to watch your animation logic in the Animator window in real time.  If you have your
 * character selected in the hierarchy and press play the animator will show your current animation and transition information with a blue
 * playing line across the bottom of each state. 
 * 
 */




using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class DemoControlScript : MonoBehaviour {

    public Joystick moveJoystick;
    public Joystick lookJoystick;

    //First, we will create a reference called myAnimator so we can talk to the Animator component on the game object.
    //The Animator is what listens to our instructions and tells the mesh which animation to use.
    private Animator myAnimator;

	// The start method is called when the script is initalized, before other stuff in the scripts start happening.
	void Start () {
		//We have a reference called myAnimator but we need to fill that reference with an Animator component.
		//We can do that by 'getting' the animator that is on the same game object this script is appleid to.
		myAnimator = GetComponent<Animator>();
	
	}
	
	// Update is called once per frame so this is a great place to listen for input from the player to see if they have
	//interacted with the game since the LAST frame (such as a button press or mouse click).
	void Update () {

        //Set the VSpeed and HSpeed floats for our animator to control walking and strafing animations.
        myAnimator.SetFloat("VSpeed", moveJoystick.m_VerticalVirtualAxis.GetValue);
        myAnimator.SetFloat("HSpeed", moveJoystick.m_HorizontalVirtualAxis.GetValue);
        transform.Rotate(0, lookJoystick.m_HorizontalVirtualAxis.GetValue, 0);
        //myAnimator.SetFloat ("VSpeed", Input.GetAxis ("Vertical"));
		//myAnimator.SetFloat ("HSpeed", Input.GetAxis ("Horizontal"));

			
		
		//Set Jump Boolean to true to trigger jump animation, then wait a small time and set to false so we don't jump agani.
		if(Input.GetButtonDown ("Jump")){
			myAnimator.SetBool ("Jumping", true);
			Invoke ("StopJumping", 0.1f);
		}
		
		
		//Controlling an animation with procedural coded motion		
		//If the Q key is being held down AND no action is currently being performed (we'll go into more about the actions later).
		if(Input.GetKey("q") && (myAnimator.GetInteger ("CurrentAction") == 0)){
			
			//Rotate the character procedurally based on Time.deltaTime.  This will give the illusion of moving
			//Even though the animations don't have root motion
			transform.Rotate (Vector3.down * Time.deltaTime * 100.0f);
			
			//Also, IF we're currently standing still (both vertically and horizontally)
			if((Input.GetAxis ("Vertical") == 0f) && (Input.GetAxis ("Horizontal") == 0)){
				//change the animation to the 'inplace' animation
				myAnimator.SetBool ("TurningLeft", true);
			}

		} else {
			//Else here means if the Q key is not being held down
			//Then we make sure that we are not playing the turning animation
			myAnimator.SetBool ("TurningLeft", false);
		}
		
		//Same thing for E key, just rotating the other way!
		if(Input.GetKey("e") && (myAnimator.GetInteger ("CurrentAction") == 0)){
			transform.Rotate (Vector3.up * Time.deltaTime * 100.0f);
			if((Input.GetAxis ("Vertical") == 0f) && (Input.GetAxis ("Horizontal") == 0)){
				myAnimator.SetBool ("TurningRight", true);
			}

		} else {
			myAnimator.SetBool ("TurningRight", false);
		}

		
		//Something to keep in mind when controlling motion through code is that it can be difficult to 'match' the animation speed.
		//For example, our rotation speed is handled by the "Vector3 * Time.deltaTime * 100.0f" portion.  Changing that float
		//to another number will decrease or increase your rate of rotation and it may not match your animation anymore.
		
	
		
		
		//Toggleable Animations
		//Sometimes you may want to turn an animation on and off yourself.
		//Here we want the behaviour to be a little different.  We'll check if the input button is pressed and then check what the current state is.
		//If the CurrentAction is 0, we will change it to 2. If it is 2, we will change it to 0.  This creates a toggle effect when the button is pressed.
		//With this method we aren't looking for a button being held down OR being released, but just using the key down to listen for key presses.
		
		if(Input.GetKeyDown ("1")){
			if(myAnimator.GetInteger("CurrentAction") == 0){
				myAnimator.SetInteger("CurrentAction", 1);				
			} else if (myAnimator.GetInteger ("CurrentAction") == 1){
				myAnimator.SetInteger ("CurrentAction", 0);
			}
		}
		
		
		//Combining methods.
		//You can combine all of these methods (and much more advanced logic) in many ways!  Let's go over an example.  We want our character to kneel down
		//Stay in a kneeling loop idle, then stand up when we tell them to.  For this we'll need to combine a triggered 'transition' animation with looping
		//animations.  You can see in the Animator Controller that Action3 (kneeling) is actually comprised of 3 animations.  "kneeling down", "kneeling idle"
		//and "kneeling stand".
		
		//The first is "kneeling down".  The intro requirement is the same as before, CurrentAction = 3.  We can control this using the same "SetInteger" method
		//we used in our previous examples.  However, the exit transition is based on exit time.  So as soon as that intro "kneeling down" animation plays
		//The animation will transition into a kneeling idle.  Now we want the character to remain there until we tell it to get up, so the exit transition
		//From kneeling idle is "currentaction = 0".  This means we'll need to set up a toggle just like in Example #4.  When we toggle from 3 to 0, we'll transition
		//Into the kneeling stand animation, which will get our character back to their feet.  Finally the exit transition for kneeling stand is exit time, so
		//As soon as they are done standing up they will go the next state ("idle/walk").
		
		if(Input.GetKeyDown ("2")){
			if(myAnimator.GetInteger ("CurrentAction") == 0){
				myAnimator.SetInteger ("CurrentAction", 2);				
			} else if (myAnimator.GetInteger ("CurrentAction") == 2){
				myAnimator.SetInteger ("CurrentAction", 0);
			}
		}
		
		//Example #5:  Combining animations with Layers
		//Unity has a layering system inside of Mecanim that you can use to control animations on specific parts of your character.  For this example, we'll play a wave
		//animation but only on part of the body.  Our layer is set to override so it will completely replace the animation on the base layer.
		//First, we define the region we want to show by creating an avatar body mask. You can see our mask by going to the Mixamo > Demo folder and finding
		//the "UpperBodyMask" asset.  Click on it and the inspector will show the areas animation will play in green and where animations will not play in red.
		
		
		//We use the GetKeyDown instead of "GetKey".  GetKey means "while the button is held down".  GetKeyDown will only set once,
		//when the key is initially depressed.
		if(Input.GetKeyDown ("3")){
			//We want to turn the layer's weight up to 1.  You canconsider weight as influence and by default it is 0.
			myAnimator.SetLayerWeight (1, 1f);
			myAnimator.SetInteger("CurrentAction", 3);
		}
		
		//We also need to reset the CurrentAction integer so that it doesn't immediately transition back into the animation.
		if(Input.GetKeyUp ("3")){
			//Let's also turn the layer's weight back to 0 so it's not influencing the base layer at all.
			myAnimator.SetInteger ("CurrentAction", 0);
		}
		
		
		//If you are holding down the W button to walk and press the 3 button to wave you'll see that the body continues to walk and swing it's arm while just
		//the unmasked arm, head and torso are affected by the wave.
		
	}
	
	//This method is called after jumping is started to stop the jumping!
	void StopJumping(){
		myAnimator.SetBool ("Jumping", false);
	}
	
	//We've added some simple GUI labels for our controls to make it easier for you to test out.
	
	void OnGUI(){
		//GUI.Label (new Rect(0, 0, 200, 25), "Forward: W");
		//GUI.Label (new Rect(0, 25, 200, 25), "Strafe Left: A");
		//GUI.Label (new Rect(0, 50, 200, 25), "Strafe Right: D");
		//GUI.Label (new Rect(0, 75, 200, 25), "Turn Left: Q");
		//GUI.Label (new Rect(0, 100, 200, 25), "Turn Right: E");
		//GUI.Label (new Rect(0, 125, 200, 25), "Toggle Dance: 1");
		//GUI.Label (new Rect(0, 150, 200, 25), "Toggle Kneeling: 2");
		//GUI.Label (new Rect(0, 175, 200, 25), "Wave (Layer): 3");
	}
}

/*
 * We hope that this has been helpful to get you started with your own custom game logic and animation setup.  For any questions don't hesitate to reach us
 * at the Mixamo community!  community.mixamo.com
 */
