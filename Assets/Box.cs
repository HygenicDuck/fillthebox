using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour 
{
	[SerializeField] private Animator m_animator;
	[SerializeField] private float m_offscreenOffset;
	[SerializeField] private GameObject m_itemDetector;
	[SerializeField] private ItemIndicator m_itemIndicator;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public void ShutTheBox()
	{
		m_animator.enabled = true;
		m_animator.Play ("shut lid");
	}

	public void MoveBoxOff()
	{
        int direction = (int)Mathf.Sign(transform.localScale.x);

		Mover mover = GetComponent<Mover> ();
		if (mover != null)
		{
			mover.MoveBy (new Vector3 (-direction*m_offscreenOffset, 0, 0), 0.25f);
		}
	}

	public void MoveBoxOn()
	{
        int direction = (int)Mathf.Sign(transform.localScale.x);

		Mover mover = GetComponent<Mover> ();
		if (mover != null)
		{
			mover.MoveBy (new Vector3 (direction*m_offscreenOffset, 0, 0), 0.25f);
		}

		m_animator.Play ("stay open");
	}

	public void StopDetectingItems()
	{
		m_itemDetector.SetActive (false);
	}

	public void ShowItemIndicator(int i)
	{
		m_itemIndicator.ShowItemIndicator (i);
	}
}
