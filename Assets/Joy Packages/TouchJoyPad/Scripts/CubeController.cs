using UnityEngine;
using System.Collections;

public class CubeController : MonoBehaviour {

	float FadeTimer;
	public Material CubeMat;
	Vector3 CubeRotate, KickDirection;

	// Use this for initialization
	void Start () {
		FadeTimer = 0.0f;
		CubeMat.color = Color.clear;
	}
	
	// Update is called once per frame
	void Update () {

		if (transform.position.y < -10.0f) {
			FadeTimer = 0.0f;
			GetComponent<Rigidbody> ().velocity = Vector3.zero;
			transform.position = new Vector3 (0.0f, 2.0f, 0.0f);

		}

		CubeMat.color = Color.Lerp (Color.clear, Color.red, FadeTimer * 2.0f);

		if (FadeTimer < 10.0f) {
			FadeTimer += Time.deltaTime;
		}
	}
}
