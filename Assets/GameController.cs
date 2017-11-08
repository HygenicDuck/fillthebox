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
	[SerializeField] List<Item> m_currentItems;
	[SerializeField] private GameObject[] m_itemPrefabs;
	[SerializeField] private GameObject[] m_boxes;
	[SerializeField] private Transform[] m_boxSpawnPositions;
	[SerializeField] private GameObject[] m_boxPrefabs;
	[SerializeField] private Transform m_itemSpawnParent;
	[SerializeField] private Vector2 m_bounceAmount;

	const float INITIAL_TIME = 10f;
	const float TIME_ADDED_PER_ITEM = 6f;
	const int NUM_ITEMS_AT_ONCE = 1;

	int m_currentBoxIndex = 0;
	int m_currentItemIndex = 0;

	// Use this for initialization
	void Start () 
	{
		Timer.Instance.SetTime (INITIAL_TIME);
		m_currentItems = new List<Item> ();
		StartCoroutine (NextBoxSequence (NUM_ITEMS_AT_ONCE));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnItemEntersBox(Item item)
	{
		StartCoroutine (ItemInBoxSequence (item));
	}

	void NextBox()
	{
		if (m_currentBox != null)
		{
			Destroy (m_currentBox.gameObject);
		}

		m_currentBoxIndex++;
		if (m_currentBoxIndex >= m_boxSpawnPositions.Length)
		{
			m_currentBoxIndex = 0;
		}
	}

	void MakeBox()
	{
		m_currentBox = Instantiate (m_boxPrefabs[m_currentItemIndex], m_boxSpawnPositions[m_currentBoxIndex]).GetComponent<Box>();
		m_currentBox.transform.localPosition = Vector3.zero;
		Vector3 scale = m_currentBox.transform.localScale;
		if (m_currentBoxIndex == 1)
		{
			scale.x = -scale.x;
		}
		m_currentBox.transform.localScale = scale;

		Box box = m_currentBox.GetComponent<Box> ();
		box.ShowItemIndicator (Random.Range(0,7));
	}

	void NextItem()
	{
		m_currentItemIndex++;
		if (m_currentItemIndex >= m_itemPrefabs.Length)
		{
			m_currentItemIndex = 0;
		}
	}

	void SpawnItem()
	{
		Item item = Instantiate (m_itemPrefabs [m_currentItemIndex], m_itemSpawnParent).GetComponent<Item>();
		item.type = m_currentItemIndex;
		m_currentItems.Add (item);
		item.transform.localPosition = Vector3.zero;
	}

	IEnumerator ItemInBoxSequence(Item item)
	{
		m_currentBox.StopDetectingItems ();
		Rigidbody2D rb = item.gameObject.GetComponent<Rigidbody2D>();
		rb.velocity = Vector3.zero;
		yield return new WaitForSeconds (0.2f);
		m_currentBox.ShutTheBox ();
		yield return new WaitForSeconds (0.3f);
		m_currentItems.Remove (item);
		Destroy (item.gameObject);
		m_currentBox.MoveBoxOff ((m_currentBoxIndex * 2)-1);
		yield return new WaitForSeconds (0.2f);
		Timer.Instance.IncreaseTime (TIME_ADDED_PER_ITEM);

		int numItemsToSpawn = (m_currentItems.Count == 0) ? NUM_ITEMS_AT_ONCE : 0;
			
		yield return NextBoxSequence(numItemsToSpawn);
	}


	IEnumerator NextBoxSequence(int numItems)
	{
		NextBox ();
		MakeBox ();
		m_currentBox.MoveBoxOn ((m_currentBoxIndex * 2)-1);

		for (int i = 0; i < numItems; i++)
		{
			yield return new WaitForSeconds (0.2f);
			NextItem ();
			SpawnItem ();
		}
	}

	public void BounceItem()
	{
		if (!Timer.Instance.TimeUp ())
		{
			foreach (Item item in m_currentItems)
			{
				//Debug.Log ("Mouse down "+m_bouncingObject.transform.position.x+", "+m_boxCentre.position.x);
				Rigidbody2D rb = item.GetComponent<Rigidbody2D> ();
				Vector2 v = rb.velocity;
				v.y = m_bounceAmount.y;

				float hysteresisRange = 0.5f;

				if (item.transform.position.x < (m_currentBox.transform.position.x - hysteresisRange))
				{
					item.m_bounceDirection = 1f;
				}
				else if (item.transform.position.x > (m_currentBox.transform.position.x + hysteresisRange))
				{
					item.m_bounceDirection = -1f;
				}

				v.x = m_bounceAmount.x * item.m_bounceDirection;

				rb.velocity = v;

				float av = rb.angularVelocity;
				av += (200.1f * item.m_bounceDirection);
				rb.angularVelocity = av;

			}
		}
	}
}
