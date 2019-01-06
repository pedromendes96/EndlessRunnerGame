using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ButtonHandler : MonoBehaviour {

	public Text Warning;
	public InputField inputField;
	// Use this for initialization
	public void OnClick(){
		var ip = inputField.text;
		if(ValidateIPv4(ip)){
			IPAddress iPAddress;
			var parsed = IPAddress.TryParse(ip.Replace(".",""), out iPAddress);
			Warning.color = Color.white;
			Warning.text = "Starting the connection...";
			TCPClient.Instance.StartClient(iPAddress);
		}else{
			Warning.color = Color.red;
			Warning.text = "Can't connect since it's not a valid IP.";
		}
	}

	public bool ValidateIPv4(string ipString)
	{
		if (String.IsNullOrEmpty(ipString.Trim()))
		{
			return false;
		}

		string[] splitValues = ipString.Split('.');
		if (splitValues.Length != 4)
		{
			return false;
		}

		byte tempForParsing;

		return splitValues.All(r => byte.TryParse(r, out tempForParsing));
	}
}
