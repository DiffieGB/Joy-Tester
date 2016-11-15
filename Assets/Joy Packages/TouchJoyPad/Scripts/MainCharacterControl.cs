using UnityEngine;
using System.Collections;

public class MainCharacterControl : MonoBehaviour {

	public float MoveSpeed = 5.0f, JumpPower = 1.0f, Gravity = 9.81f;

	public GameObject Bullet;

	float cameraTilt;
	CharacterController MainCharaCon;
	Vector3 MCVTemp, MainCharaVelocity;

	// Use this for initialization
	void Start () {
		MainCharaCon = GameObject.FindWithTag ("Player").GetComponent<CharacterController> ();
	}
	
	// Update is called once per frame
	void Update () {
		//Move value by Joy key
		MCVTemp = transform.forward * TouchJoyPad.JoyKeyAxis.y * Time.deltaTime * MoveSpeed;
		MCVTemp += transform.right * TouchJoyPad.JoyKeyAxis.x * Time.deltaTime * MoveSpeed;

		//Dash Effect
		if (TouchJoyPad.Button02) {
			MCVTemp *= 2.0f;
		}

		//Inheritance of Y-axis values
		if (MainCharaCon.isGrounded) {
			MCVTemp.y = 0.0f;
		} else {
			MCVTemp.y += MainCharaVelocity.y;
		}

		//Update of the velocity values
		MainCharaVelocity = MCVTemp;

		//Camera move by touch and drag
		MainCharaCon.transform.Rotate (transform.up * TouchJoyPad.TouchDragAxis.x * 2.0f * 800.0f / (float)Screen.width);
		cameraTilt -= TouchJoyPad.TouchDragAxis.y * 2.0f * 800.0f / (float)Screen.width;

		if (cameraTilt > 60.0f) {
			cameraTilt = 60.0f;
		}

		if (cameraTilt < -60.0f) {
			cameraTilt = -60.0f;
		}

		Camera.main.transform.localRotation = Quaternion.Euler (cameraTilt, 0.0f, 0.0f);

		//Jump button
		if (MainCharaCon.isGrounded && TouchJoyPad.Button01Down) {
			MainCharaVelocity.y = JumpPower;
		}

		//Jump cancel
		if (!MainCharaCon.isGrounded && MainCharaVelocity.y > 0.0f && TouchJoyPad.Button01Up) {
			MainCharaVelocity.y /= 5.0f;
		}

		//Small shot
		if (TouchJoyPad.Button03Down) {
			GameObject BulletFire = (GameObject)Instantiate (Bullet, Camera.main.transform.position + Camera.main.transform.forward * 1.0f, Quaternion.identity);
			BulletFire.transform.localScale *= 0.8f;
			BulletFire.GetComponent<Rigidbody> ().mass *= 0.5f;
			BulletFire.GetComponent<Rigidbody> ().velocity = (BulletFire.transform.position - Camera.main.transform.position) * 30.0f;
		}

		//Large shot
		if (TouchJoyPad.Button04Down) {
			GameObject BulletFire = (GameObject)Instantiate (Bullet, Camera.main.transform.position + Camera.main.transform.forward * 1.0f, Quaternion.identity);
			BulletFire.transform.localScale *= 2.0f;
			BulletFire.GetComponent<Rigidbody> ().mass *= 8.0f;
			BulletFire.GetComponent<Rigidbody> ().velocity = (BulletFire.transform.position - Camera.main.transform.position) * 15.0f;
		}

		//Add gravity
		if (MainCharaVelocity.y > -Gravity * 0.5f) {
			MainCharaVelocity.y -= Gravity * Time.deltaTime * 0.2f;
		} else {
			MainCharaVelocity.y = -Gravity;
		}
		//Application of the velocity
		MainCharaCon.Move (MainCharaVelocity);

		//Loop if player fall	
		if (transform.position.y < -20.0f) {
			transform.position = new Vector3 (0.0f, 5.0f, -2.0f);
		}
	}
}
