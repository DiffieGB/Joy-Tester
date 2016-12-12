using UnityEngine;
using System.Collections;

public class Follower : MonoBehaviour {
	public Transform whoToFollow;
	
	void Update () {
		if(whoToFollow){
			transform.position = whoToFollow.position;
		}
	}
}
