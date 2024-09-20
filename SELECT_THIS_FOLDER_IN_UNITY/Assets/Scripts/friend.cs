using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class friend : MonoBehaviour
{
	public NavMeshAgent agent;

	public Transform player;

	public Transform noteRoomSpawn;

	public AudioSource audio;

	private bool b;

	public void Update()
	{
		if (!b)
		{
			agent.SetDestination(player.position);
			if (Vector3.Distance(player.position, base.transform.position) <= 2f)
			{
				StartCoroutine(NoteRoomSpawn());
				b = true;
			}
		}
	}

	public IEnumerator NoteRoomSpawn()
	{
		audio.PlayOneShot(Resources.Load("intelligence rever") as AudioClip);
		player.position = noteRoomSpawn.position;
		yield return new WaitForSeconds(120f);
		SceneManager.LoadScene("Mama23");
	}
}
