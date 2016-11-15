
namespace NewUIProPack
{
	using UnityEngine;
	using System.Collections;

	/// <summary>
	/// Knob UI Widget. Should be controlled by a slider whos value range
	/// from 0 to 1.
	/// </summary>
	public class KnobHelper : MonoBehaviour {

		public void SetRotationZ(float value)
		{
			Vector3 rotation = transform.localEulerAngles;
			rotation.z = value;
			transform.localEulerAngles = rotation;
		}
	}
}