using UnityEngine;

public class roomScript : MonoBehaviour
{
	public AudioSource audio;

	public void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			audio.PlayOneShot(Resources.Load("laugh") as AudioClip);
			Object.Destroy(this);
		}
	}
}
