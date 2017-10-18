using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour 
{
	[SerializeField] private Animator m_animator;
	[SerializeField] private float m_offscreenOffset;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public void ShutTheBox()
	{
		m_animator.enabled = true;
	}

	public void MoveBoxOff()
	{
		Mover mover = GetComponent<Mover> ();
		if (mover != null)
		{
			mover.MoveBy (new Vector3 (m_offscreenOffset, 0, 0), 0.25f);
		}
	}

	public void MoveBoxOn()
	{
		Mover mover = GetComponent<Mover> ();
		if (mover != null)
		{
			mover.MoveBy (new Vector3 (-m_offscreenOffset, 0, 0), 0.25f);
		}
	}
}
