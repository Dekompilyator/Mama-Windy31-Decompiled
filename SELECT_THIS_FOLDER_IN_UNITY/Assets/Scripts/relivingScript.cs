using UnityEngine;
using UnityEngine.SceneManagement;

public class relivingScript : MonoBehaviour
{
	public movingScript player;

	public AudioSource audio;

	public AudioClip noise;

	public void onAnimFinish1()
	{
		player.canMove = true;
		base.gameObject.SetActive(false);
	}

	public void onNoiseStart1()
	{
		audio.PlayOneShot(noise);
	}

	public void onAnimFinish2()
	{
		Debug.Log("finished");
		SceneManager.LoadScene("Mama23");
	}
}
