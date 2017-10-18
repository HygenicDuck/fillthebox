using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour 
{
	static GameController m_instance = null;

	GameController()
	{
		m_instance = this;
	}

	public static GameController Instance
	{
		get
		{
			return m_instance;
		}
	}


	[SerializeField] Box m_currentBox;
	[SerializeField] Item m_currentItem;
	[SerializeField] private GameObject[] m_itemPrefabs;
	[SerializeField] private GameObject[] m_boxes;
	[SerializeField] private Transform m_itemSpawnParent;
	[SerializeField] private Vector2 m_bounceAmount;

	int m_currentBoxIndex = 0;
	int m_currentItemIndex = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnItemEntersBox()
	{
		StartCoroutine (ItemInBoxSequence ());
	}

	void NextBox()
	{
		m_currentBoxIndex++;
		if (m_currentBoxIndex >= m_boxes.Length)
		{
			m_currentBoxIndex = 0;
		}
		m_currentBox = m_boxes [m_currentBoxIndex].GetComponent<Box>();
	}

	void SpawnNextItem()
	{
		m_currentItemIndex++;
		if (m_currentItemIndex >= m_itemPrefabs.Length)
		{
			m_currentItemIndex = 0;
		}

		m_currentItem = Instantiate (m_itemPrefabs [m_currentItemIndex], m_itemSpawnParent).GetComponent<Item>();
		m_currentItem.transform.localPosition = Vector3.zero;
	}

	IEnumerator ItemInBoxSequence()
	{
		yield return new WaitForSeconds(0.2f);
		m_currentBox.ShutTheBox ();
		yield return new WaitForSeconds(0.5f);
		Destroy (m_currentItem.gameObject);
		m_currentBox.MoveBoxOff ();
		yield return new WaitForSeconds(0.2f);

		NextBox ();
		m_currentBox.MoveBoxOn ();

		yield return new WaitForSeconds(0.2f);
		SpawnNextItem ();
	}

	public void BounceItem()
	{
		//Debug.Log ("Mouse down "+m_bouncingObject.transform.position.x+", "+m_boxCentre.position.x);
		Rigidbody2D rb = m_currentItem.GetComponent<Rigidbody2D> ();
		Vector2 v = rb.velocity;
		v.y = m_bounceAmount.y;

		if (m_currentItem.transform.position.x < m_currentBox.transform.position.x)
		{
			v.x = m_bounceAmount.x;
		} 
		else
		{
			v.x = -m_bounceAmount.x;
		}

		rb.velocity = v;
	}
}
