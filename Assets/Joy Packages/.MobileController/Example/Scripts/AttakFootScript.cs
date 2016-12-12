using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AttakFootScript : MonoBehaviour {

	// Use this for initialization
	Animator m_Animator;
	
	void Start () {
		m_Animator = GetComponent<Animator>();

	}
	
	public void	AttakFootPlay()
	{
		m_Animator.SetBool("FootAttack", true);
	}

	public void	AttakFootStop()
	{
		m_Animator.SetBool("FootAttack", false);

	}
}
