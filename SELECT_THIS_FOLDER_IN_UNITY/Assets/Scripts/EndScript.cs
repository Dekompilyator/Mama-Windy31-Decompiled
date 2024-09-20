using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScript : MonoBehaviour
{
	public void EndGame()
	{
		SceneManager.LoadScene("Menu");
	}
}
