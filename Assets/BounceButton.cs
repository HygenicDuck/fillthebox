using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceButton : MonoBehaviour {

//	[SerializeField] private GameObject m_bouncingObject;
//	[SerializeField] private Transform m_boxCentre;
//	[SerializeField] private Vector2 m_bounceAmount;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnMouseDown()
	{
		GameController.Instance.BounceItem ();
//		Debug.Log ("Mouse down "+m_bouncingObject.transform.position.x+", "+m_boxCentre.position.x);
//		Rigidbody2D rb = m_bouncingObject.GetComponent<Rigidbody2D> ();
//		Vector2 v = rb.velocity;
//		v.y = m_bounceAmount.y;
//
//		if (m_bouncingObject.transform.position.x < m_boxCentre.position.x)
//		{
//			v.x = m_bounceAmount.x;
//		} 
//		else
//		{
//			v.x = -m_bounceAmount.x;
//		}
//
//		rb.velocity = v;
	}
}
