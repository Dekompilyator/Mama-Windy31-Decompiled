using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScript : MonoBehaviour
{
	public GameObject loading;

	public Text text;

	public Animator anim;

	private bool shouldContinue;

	private AsyncOperation asyncLoad;

	public void OnStart()
	{
		Debug.Log("TEST");
		Time.timeScale = 1f;
		loading.SetActive(true);
		anim.enabled = false;
		base.transform.position = new Vector2(3000f, 3000f);
		asyncLoad = SceneManager.LoadSceneAsync("Mama23");
		asyncLoad.allowSceneActivation = false;
		StartCoroutine(LoadYourAsyncScene());
	}

	private IEnumerator LoadYourAsyncScene()
	{
		while (asyncLoad.progress < 0.9f)
		{
			Debug.Log(asyncLoad.progress);
			yield return null;
		}
		Debug.Log(asyncLoad.progress);
		text.text = "Продолжить";
	}

	public void onContinue()
	{
		if (asyncLoad.progress >= 0.9f)
		{
			asyncLoad.allowSceneActivation = true;
			Object.Destroy(GameObject.Find("Музыка"));
		}
	}
}
