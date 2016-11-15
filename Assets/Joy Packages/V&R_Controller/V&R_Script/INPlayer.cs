using UnityEngine;
using System.Collections;

[AddComponentMenu("V&R Controller/Example/INPlayer")]
public class INPlayer : MonoBehaviour {
	public INController inCon;
	public Transform bullet1;
	public Transform bullet2;
	public Transform boom;
	public Transform gunPortPos;
//	public int hp;
//	public int maxHp;
//	public int lv;

	public float speed = 1f;
	public float normalSpeed = 1f;
	public float shootDelay = 1f;
	public Transform target;
	bool moveLock_f;
	Vector3 preDir;
	bool shoot_f;
	CharacterController cCon;

	void Start () {
		cCon = gameObject.GetComponent<CharacterController>();
		StartCoroutine("ShootDelay");
		Time.timeScale = 1;
	}

	void Update () {
		if (!moveLock_f && !(Time.timeScale == 0))
			cCon.Move(inCon.GetAxisVector3() * speed * Time.smoothDeltaTime*10);
		
		if (inCon.GetAxisVector3() != Vector3.zero && !(Time.timeScale == 0)){
			transform.LookAt(Vector3.Lerp(inCon.GetAxisVector3() * 10000 , preDir, 0.005f));
			preDir = inCon.GetAxisVector3() * 10000;
		}
		//first press
		if (inCon.buttonState[0] == INController.BUTTON_STATE.PRESS){
			print("pressed button1");
			FireBoom1();
		}
		//go on press
		else if (inCon.buttonState[0] == INController.BUTTON_STATE.PRESSING){
		//	print("pressing button1");
			if (shoot_f)
				FireBoom2();
		}
		//end pressed
		else if (inCon.buttonState[0] == INController.BUTTON_STATE.UP){
		//	print("unpressed button1");
		}

		if (inCon.buttonState[1] == INController.BUTTON_STATE.PRESS){
			print ("pressed button2");
			FireBoom3();
		}
		else if (inCon.buttonState[1] == INController.BUTTON_STATE.PRESSING){
		//	print ("pressed button2");
		}
		else if (inCon.buttonState[1] == INController.BUTTON_STATE.UP){
		//	print("unpressed button2");
		}
	}

	public void FireBoom1(){
		Instantiate(bullet1, gunPortPos.transform.position, Quaternion.Euler(transform.eulerAngles));
		shoot_f = false;
	}
	public void FireBoom2(){
		Instantiate(bullet2, gunPortPos.transform.position, Quaternion.Euler(transform.eulerAngles));
		shoot_f = false;
	}
	public void FireBoom3(){
		Instantiate(boom,gunPortPos.transform.position + transform.forward * 5f, Quaternion.LookRotation(-gunPortPos.transform.forward));
	}
	
	IEnumerator ShootDelay(){
		while(true){
			yield return new WaitForSeconds(shootDelay);
			shoot_f = true;
		}
	}
}
