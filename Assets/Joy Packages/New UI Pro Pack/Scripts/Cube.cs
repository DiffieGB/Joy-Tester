
namespace NewUIProPack
{
	using UnityEngine;
	using System.Collections;
	
	/// <summary>
	/// Demonstrates how to recieve input from Joystics, DPads, and buttons.
	/// </summary>
	public class Cube : MonoBehaviour {

		DPad.Direction direction = DPad.Direction.None;
		Vector2 joyInput;

		public void JoyInput(Vector2 input)
		{
			joyInput = input;
		}

		public void DPadInput(DPad.Direction dir)
		{
			direction = dir;
		}

		public void RotateCube()
		{
			transform.rotation = Random.rotation;
		}
		public void ScaleCube()
		{
			transform.localScale = Random.insideUnitSphere * 3f;
		}

		// Update is called once per frame
		void Update ()
		{
			transform.Translate(joyInput * Time.deltaTime * 5f, Space.World);

			if (direction == DPad.Direction.Up)
				transform.Translate(0, Time.deltaTime * 5f, 0, Space.World);
			else if (direction == DPad.Direction.Left)
				transform.Translate(-Time.deltaTime * 5f, 0, 0, Space.World);
			else if (direction == DPad.Direction.Right)
				transform.Translate(Time.deltaTime * 5f, 0, 0, Space.World);
			else if (direction == DPad.Direction.Down)
				transform.Translate(0, -Time.deltaTime * 5f, 0, Space.World);
		}
	}
}