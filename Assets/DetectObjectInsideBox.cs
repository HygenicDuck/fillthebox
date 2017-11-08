using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectObjectInsideBox : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D coll) 
	{
		Debug.Log ("thing inside box!");
		Item item = coll.gameObject.GetComponent<Item> ();
		if (item == null)
		{
			item = coll.gameObject.transform.parent.gameObject.GetComponent<Item> ();
		}
		GameController.Instance.OnItemEntersBox (item);
	}
}
