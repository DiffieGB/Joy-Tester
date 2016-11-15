using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AttakPunchScript : MonoBehaviour {
	
	// Use this for initialization
	Animator m_Animator;
	
	void Start () {
		m_Animator = GetComponent<Animator>();
		}
	
	public void	AttakPunchPlay()
	{
		m_Animator.SetBool("PunchAttack", true);
	}
	
	public void	AttakPunchStop()
	{
		m_Animator.SetBool("PunchAttack", false);
	}
}