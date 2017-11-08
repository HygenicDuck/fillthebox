using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour 
{
	static Timer m_instance = null;

	Timer()
	{
		m_instance = this;
	}

	public static Timer Instance
	{
		get
		{
			return m_instance;
		}
	}

	[SerializeField] Text m_timerText;

	float m_timeLeft = 0f;
	SpriteScaler scaler = null;

	// Use this for initialization
	void Start () 
	{
		scaler = GetComponent<SpriteScaler> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		float timeBefore = m_timeLeft;
		m_timeLeft -= Time.deltaTime;

		if ((m_timeLeft < 5f) && ((int)timeBefore != (int)m_timeLeft))
		{
			//scaler.ScaleBackFrom (2f, 0.1f);
			scaler.ScaleUsingCurve (1.0f);
		}

		if (m_timeLeft <= 0f)
		{
			m_timeLeft = 0f;
		}
		DisplayTime ();
	}

	public void SetTime(float time)
	{
		m_timeLeft = time;
		DisplayTime ();
	}

	public void IncreaseTime(float time)
	{
		m_timeLeft += time;
		DisplayTime ();
	}

	void DisplayTime()
	{
		m_timerText.text = m_timeLeft.ToString ("0.0");
	}

	public float Value()
	{
		return m_timeLeft;
	}

	public bool TimeUp()
	{
		return (m_timeLeft == 0f);
	}
}
