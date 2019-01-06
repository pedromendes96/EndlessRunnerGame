using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {
	
	public Canvas SliderCanvas;
	public Slider Slider;

	public void LoadSceneAsync(string name)
	{
		SliderCanvas.gameObject.SetActive(true);
		StartCoroutine(LoadYourAsyncScene(name));
	}
	
	IEnumerator LoadYourAsyncScene(string name)
	{
		// The Application loads the Scene in the background as the current Scene runs.
		// This is particularly good for creating loading screens.
		// You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
		// a sceneBuildIndex of 1 as shown in Build Settings.

		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(name);

		// Wait until the asynchronous scene fully loads
		while (!asyncLoad.isDone)
		{
			float progress = Mathf.Clamp01(asyncLoad.progress/.9f);
			
			Slider.value = progress;
			yield return null;
		}
	}
}
