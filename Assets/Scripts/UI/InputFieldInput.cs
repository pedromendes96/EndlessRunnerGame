using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldInput : MonoBehaviour {

	public Text Warning;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnEndEdit(Text text){
		string ip = text.text;
		if(ValidateIPv4(ip)){
			Warning.color = Color.white;
			Warning.text = "Valid IP!";
		}else{
			Warning.color = Color.red;
			Warning.text = "This isn't a valid IP.";
		}
	}

	public bool ValidateIPv4(string ipString)
	{
		if (String.IsNullOrEmpty(ipString.Trim()))
		{
			return false;
		}

		string[] splitValues = ipString.Trim().Split('.');
		if (splitValues.Length != 4)
		{
			return false;
		}

		byte tempForParsing;

		return splitValues.All(r => byte.TryParse(r, out tempForParsing));
	}
}
