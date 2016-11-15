using UnityEngine;
using System.Collections;
[AddComponentMenu("V&R Controller/Example/INSelfDestroy")]
public class INSelfDestroy : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (!this.GetComponent<ParticleSystem>().IsAlive ())
			Destroy (this.gameObject);
	}
}
