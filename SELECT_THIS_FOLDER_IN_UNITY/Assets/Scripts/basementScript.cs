using System.Collections;
using UnityEngine;

public class basementScript : MonoBehaviour
{
	public GameObject basementPicture;

	public void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			StartCoroutine(show());
		}
	}

	private IEnumerator show()
	{
		basementPicture.SetActive(true);
		yield return new WaitForSeconds(0.1f);
		basementPicture.SetActive(false);
		Object.Destroy(this);
	}
}
