using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayButtonHandler : MonoBehaviour
{
	public SceneLoader SceneLoader;
	
	public void OnClick()
	{
//		if (TCPClient.Instance.IsConnected)
//		{
			var objects = GameObject.FindGameObjectsWithTag("Static");
			foreach (var gameObject in objects)
			{
				gameObject.SetActive(false);
			}
			SceneLoader.LoadSceneAsync("Main");
//		}
	}
}
