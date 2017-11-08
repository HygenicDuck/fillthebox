using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemIndicator : MonoBehaviour {

	[SerializeField] 
	private GameObject[] m_items;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ShowItemIndicator(int number)
	{
		for (int i = 0; i < m_items.Length; i++)
		{
			m_items[i].SetActive (i == number);
		}
	}
}
