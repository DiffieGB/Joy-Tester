using UnityEngine;
using System.Collections;

// Example script for consuming a thumbstick and updating the position / rotation of an object based on input
public class csPlayerControllerDemo
: MonoBehaviour 
{
	// Public reference to thumbstick 
	public GameObject m_ThumbStickLeft;
	public GameObject m_ThumbStickRight;
	
	// Private reference to the actual class instances
	private csThumbStick m_stickControllerL;
	private csThumbStick m_stickControllerR;

	private Vector3 m_velocity = Vector3.zero;	
	
	void Start () 
	{
		// Do we have a reference to the left thumbstick gameobject?
		if (m_ThumbStickLeft)
		{
			// Get the code component
			m_stickControllerL = 
				(csThumbStick)m_ThumbStickLeft.GetComponent(typeof(csThumbStick));
			
			// Hook up an event subscriber
			m_stickControllerL.evThumbStickEvent += this.ThumbStickHandlerLeft;			
		}

		// Do we have a reference to the right thumbstick gameobject?
		if (m_ThumbStickRight)
		{
			// Get the code component
			m_stickControllerR = 
				(csThumbStick)m_ThumbStickRight.GetComponent(typeof(csThumbStick));
			
			// Hook up an event subscriber
			m_stickControllerR.evThumbStickEvent += this.ThumbStickHandlerRight;			
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		m_velocity = Vector3.Lerp(m_velocity, Vector3.zero, Time.deltaTime * 1.0f);
		transform.position = transform.position + m_velocity;			
	}		
	
	// Thumbstick Event Handler
	void ThumbStickHandlerLeft (csThumbStick.enThumbStickEvent tsE, Vector2 pos)
	{
		// NOTE: This is the other way of dealing with the thumbstick - subscribing to notifications.
		//	 	 Use whichever method suits your requirements best.

		switch (tsE)
		{
			case csThumbStick.enThumbStickEvent.MoveStart:	
				UpdatePlayerAngle();
				break;

			case csThumbStick.enThumbStickEvent.MoveHold:
				UpdatePlayerAngle();
				break;

			case csThumbStick.enThumbStickEvent.MoveChange:
				UpdatePlayerAngle();
				break;

			case csThumbStick.enThumbStickEvent.MoveEnd:
				UpdatePlayerAngle();
				break;

			case csThumbStick.enThumbStickEvent.Click:
				break;
		}		
	}

	// Thumbstick Event Handler
	void ThumbStickHandlerRight (csThumbStick.enThumbStickEvent tsE, Vector2 pos)
	{
		// NOTE: This is the other way of dealing with the thumbstick - subscribing to notifications.
		//	 	 Use whichever method suits your requirements best.

		switch (tsE)
		{
			case csThumbStick.enThumbStickEvent.MoveStart:	
				UpdatePlayerPosition();
				break;

			case csThumbStick.enThumbStickEvent.MoveHold:
				UpdatePlayerPosition();
				break;

			case csThumbStick.enThumbStickEvent.MoveChange:
				UpdatePlayerPosition();
				break;

			case csThumbStick.enThumbStickEvent.MoveEnd:
				UpdatePlayerPosition();
				break;

			case csThumbStick.enThumbStickEvent.Click:
				break;
		}		
	}	
	
	// Update player angle based on left thumbstick input
	private void UpdatePlayerAngle()
	{
		if (m_stickControllerL.Active)
		{
			transform.rotation = Quaternion.Euler(0.0f, m_stickControllerL.StickAngle, 0.0f);			
		}		
	}

	// Update player based on thumbstick input
	private void UpdatePlayerPosition()
	{
		if (m_stickControllerR.Active)
		{
			m_velocity.x += m_stickControllerR.StickVelocityX * Time.deltaTime * 0.01f;
			m_velocity.z -= m_stickControllerR.StickVelocityY * Time.deltaTime * 0.01f;
		}		
	}

}
