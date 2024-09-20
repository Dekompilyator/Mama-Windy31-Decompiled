using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
	public GameObject loading;

	public GameObject continueBtn;

	public GameObject menu;

	public GameObject menu2;

	public GameObject music;

	private bool shouldContinue;

	private AsyncOperation asyncLoad;

	public void OnStart()
	{
		Debug.Log("TEST");
		Time.timeScale = 1f;
		loading.SetActive(true);
		base.transform.position = new Vector2(3000f, 3000f);
		menu.SetActive(false);
		menu2.SetActive(false);
		asyncLoad = SceneManager.LoadSceneAsync("Start");
		asyncLoad.allowSceneActivation = false;
		StartCoroutine(LoadYourAsyncScene());
		Object.DontDestroyOnLoad(music);
		Advertisements.Instance.SetUserConsent(true);
		Advertisements.Instance.Initialize();
	}

	private IEnumerator LoadYourAsyncScene()
	{
		while (asyncLoad.progress < 0.9f)
		{
			Debug.Log(asyncLoad.progress);
			yield return null;
		}
		Debug.Log(asyncLoad.progress);
		continueBtn.SetActive(true);
	}

	public void onContinue()
	{
		asyncLoad.allowSceneActivation = true;
		AudioSource component = music.GetComponent<AudioSource>();
		component.pitch = 1f;
	}
}
