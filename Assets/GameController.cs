using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
	[SerializeField] List<Item> m_activeItems;
	[SerializeField] List<int> m_currentItemTypes;
	[SerializeField] private GameObject[] m_itemPrefabs;
	[SerializeField] private GameObject[] m_boxes;
	[SerializeField] private Transform[] m_boxSpawnPositions;
	[SerializeField] private GameObject[] m_boxPrefabs;
	[SerializeField] private GameObject m_gameOverText;
	[SerializeField] private Transform m_itemSpawnParent;
    [SerializeField] private Vector2 m_bounceAmount;
    [SerializeField] private string[] m_boxSequence;

	const float INITIAL_TIME = 10f;
	const float TIME_ADDED_PER_ITEM = 6f;
	const int NUM_ITEMS_AT_ONCE = 3;

	int m_currentBoxIndex = 0;
	int m_currentItemIndex = 0;
    int m_sequenceIndex = 0;

	// Use this for initialization
	void Start () 
	{
		m_gameOverText.SetActive (false);
		Timer.Instance.SetTime (INITIAL_TIME);
		m_activeItems = new List<Item> ();
		StartCoroutine (MainGameLoop());
		StartCoroutine (NextBoxSequence (NUM_ITEMS_AT_ONCE));
	}

	IEnumerator MainGameLoop()
	{
		do
		{
			while (!Timer.Instance.TimeUp ())
			{
				yield return null;
			}

			// wait a bit to see if something is going to fall in the box after the timer has expired.
			yield return new WaitForSeconds (2f);
		} while (!Timer.Instance.TimeUp ());

		// it is game over
		StartCoroutine (GameOverSequence ());
	}

	IEnumerator GameOverSequence()
	{
//		for (int i = 0; i < 5; i++)
//		{
//			m_gameOverText.SetActive (true);
//			yield return new WaitForSeconds (0.5f);
//			m_gameOverText.SetActive (false);
//			yield return new WaitForSeconds (0.25f);
//		}

		m_gameOverText.SetActive (true);
		yield return new WaitForSeconds (2f);

		SceneManager.LoadScene ("main");
	}
	
	// Update is called once per frame
	void Update () 
	{
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

        m_sequenceIndex++;
        if (m_sequenceIndex >= m_boxSequence.Length)
        {
            m_sequenceIndex = 0;
        }

		m_currentBoxIndex++;
		if (m_currentBoxIndex >= m_boxSpawnPositions.Length)
		{
			m_currentBoxIndex = 0;
		}
	}

    void MakeBoxes()
    {
        // introduce all boxes specified at this step in the sequence
        string s = m_boxSequence[m_sequenceIndex];
        Debug.Log("s = " + s);
        string[] pairs = s.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
        foreach(string pair in pairs)
        {
            Debug.Log("pair = " + pair);
            string[] p = pair.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
            Debug.Log("p0 = " + p[0]);
            Debug.Log("p1 = " + p[1]);
            int boxIndex = System.Int32.Parse(p[0]);
            int posIndex = System.Int32.Parse(p[1]);

            m_currentBox = Instantiate(m_boxPrefabs[boxIndex], m_boxSpawnPositions[posIndex]).GetComponent<Box>();
            m_currentBox.transform.localPosition = Vector3.zero;
            Vector3 scale = m_currentBox.transform.localScale;
            if (posIndex == 1)
            {
                scale.x = -scale.x;
            }
            m_currentBox.transform.localScale = scale;

            Box box = m_currentBox.GetComponent<Box>();
            box.ShowItemIndicator(m_currentItemTypes[Random.Range(0, m_currentItemTypes.Count)]);
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
		box.ShowItemIndicator (m_currentItemTypes[Random.Range(0,m_currentItemTypes.Count)]);
	}

	void NextItem()
	{
		m_currentItemIndex++;
		if (m_currentItemIndex >= m_itemPrefabs.Length)
		{
			m_currentItemIndex = 0;
		}
	}

	void SpawnItem(int type)
	{
		Item item = Instantiate (m_itemPrefabs [type], m_itemSpawnParent).GetComponent<Item>();
		item.type = type;
		m_activeItems.Add (item);
		item.transform.localPosition = Vector3.zero;
		Vector3 r = new Vector3 (0f, 0f, Random.Range (0f, 360f));
		item.transform.eulerAngles = r;
	}

	IEnumerator ItemInBoxSequence(Item item)
	{
		m_currentBox.StopDetectingItems ();
		Rigidbody2D rb = item.gameObject.GetComponent<Rigidbody2D>();
		rb.velocity = Vector3.zero;
		yield return new WaitForSeconds (0.2f);
		m_currentBox.ShutTheBox ();
		yield return new WaitForSeconds (0.3f);
		//Debug.Log ("Before removing "+item.type+" : "+m_currentItemTypes.Count);
		m_currentItemTypes.Remove (item.type);
		//Debug.Log ("After removing "+item.type+" : "+m_currentItemTypes.Count);
		m_activeItems.Remove (item);
		Destroy (item.gameObject);
		m_currentBox.MoveBoxOff ();
		yield return new WaitForSeconds (0.2f);
		Timer.Instance.IncreaseTime (TIME_ADDED_PER_ITEM);

		Debug.Log ("m_currentItemTypes.Count "+m_currentItemTypes.Count);
		int numItemsToSpawn = (m_currentItemTypes.Count == 0) ? NUM_ITEMS_AT_ONCE : 0;
			
		yield return NextBoxSequence(numItemsToSpawn);
	}


	IEnumerator NextBoxSequence(int numItems)
	{
		for (int i = 0; i < numItems; i++)
		{
			m_currentItemTypes.Add (Random.Range(0,m_itemPrefabs.Length));
			Debug.Log ("Adding a random item "+i);

		}
		
		NextBox ();
		MakeBoxes ();

        int direction = (int)Mathf.Sign(m_currentBox.transform.localScale.x);
        m_currentBox.MoveBoxOn();
        //m_currentBox.MoveBoxOn((m_currentBoxIndex * 2) - 1);

		Debug.Log ("m_currentItemTypes.Count = "+m_currentItemTypes.Count);
		for (int i = m_currentItemTypes.Count-numItems; i < m_currentItemTypes.Count; i++)
		{
			yield return new WaitForSeconds (0.4f);
			NextItem ();
			Debug.Log ("m_currentItemTypes[i] = "+m_currentItemTypes[i]);
			SpawnItem (m_currentItemTypes[i]);
		}
	}

	public void BounceItem()
	{
		if (!Timer.Instance.TimeUp ())
		{
			foreach (Item item in m_activeItems)
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

				bool belowBox = (item.transform.position.y < m_currentBox.transform.position.y);

				v.x = m_bounceAmount.x * item.m_bounceDirection * (belowBox ? 2f : 1f);

				rb.velocity = v;

				float av = rb.angularVelocity;
				av += (200.1f * item.m_bounceDirection);
				rb.angularVelocity = av;

			}
		}
	}

	
}
