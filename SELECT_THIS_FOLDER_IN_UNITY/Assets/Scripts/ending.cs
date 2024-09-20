using UnityEngine;

public class ending : MonoBehaviour
{
	public AudioSource audio;

	public AudioClip f;

	public Animator r;

	public GameObject cake;

	public GameObject player;

	public void OnTriggerEnter2D(Collider2D collision)
	{
		r.gameObject.SetActive(true);
		r.Play("endinganim");
		cake.gameObject.SetActive(false);
		player.gameObject.SetActive(false);
	}
}
