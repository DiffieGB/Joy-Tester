using UnityEngine;
using System.Collections;

[AddComponentMenu("V&R Controller/Example/TestBullet")]
public class TestBullet : MonoBehaviour {
	public float speed = 50;
	// Use this for initialization
	void Start () {
		StartCoroutine ("DestroySelf", 2);
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(Vector3.forward * Time.deltaTime * speed);
	}

	IEnumerator DestroySelf(float time){
		Destroy (this.gameObject, time);
		yield return null;
	}
}
