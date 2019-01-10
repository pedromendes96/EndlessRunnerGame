using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.UI;

public class Message : MonoBehaviour
{
	private System.Timers.Timer aTimer;
	private string[] messages;
	private System.Random _random;
	private Text text;
	private bool isActive;
	
	// Use this for initialization
	void Start ()
	{
		isActive = true;
		_random = new System.Random();
		messages = new[]
		{
			"Don't forget that we want remarkable recoveries from you!",
			"We are here to help you becoming great again!",
			"A anti-stroke game: let's destroy the damages from you!"
		};
		var textObj = gameObject.GetComponent<Text>();
		textObj.text = messages[_random.Next(messages.Length)];
		text = textObj;
		
		aTimer = new System.Timers.Timer(10000);
		// Hook up the Elapsed event for the timer. 
		aTimer.Elapsed += OnTimedEvent;
		aTimer.AutoReset = false;
		aTimer.Enabled = true;
	}

	private void OnTimedEvent(object sender, ElapsedEventArgs e)
	{
		// Debug.Log("Time elapsed!!!!!!!!");
		isActive = false;	
	}

	// Update is called once per frame
	void Update () {
		if (!isActive)
		{
			this.gameObject.transform.parent.gameObject.SetActive(false);
		}
	}
}
