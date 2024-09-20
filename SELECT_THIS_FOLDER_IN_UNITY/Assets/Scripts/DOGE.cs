using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DOGE : MonoBehaviour
{
	public GameObject player;

	private CharacterController playerController;

	public List<GameObject> movePoints;

	private int pointTarget;

	private NavMeshAgent agent;

	private AudioSource audio;

	public int state;

	public bool isRunningFromPlayer;

	public AudioClip movingSound;

	public AudioClip rickSound;

	public GameObject momo;

	private bool isWaiting;

	private Momo momoScript;

	public int woofCount;

	private bool hasReachedDestination()
	{
		if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance && (!agent.hasPath || agent.velocity.sqrMagnitude == 0f))
		{
			return true;
		}
		return false;
	}

	private void chooseRandomPoint()
	{
		int num = Random.Range(0, movePoints.Count);
		setPoint(num);
		Debug.Log("Choosed " + num);
	}

	private void setPoint(int point)
	{
		state = 0;
		agent.SetDestination(movePoints[point].transform.position);
		pointTarget = point;
	}

	private void followPlayer()
	{
		agent.SetDestination(player.transform.position);
	}

	private void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		agent.SetDestination(movePoints[0].transform.position);
		pointTarget = 0;
		playerController = player.GetComponent<CharacterController>();
		audio = GetComponent<AudioSource>();
		momoScript = momo.GetComponent<Momo>();
		StartCoroutine(moveAndLook());
	}

	private IEnumerator moveAndLook()
	{
		while (true)
		{
			yield return new WaitForSeconds(0.5f);
			if (isWaiting)
			{
				continue;
			}
			Vector3 position = playerController.transform.position;
			position.y += playerController.height - 0.5f;
			Vector3 position2 = base.transform.position;
			position2.y += 1f;
			RaycastHit hitInfo;
			if (Vector3.Distance(position, position2) <= 10f && Physics.Linecast(position2, position, out hitInfo) && hitInfo.collider.tag == "Player" && state == 0 && !isRunningFromPlayer && Vector3.Angle(base.transform.forward, player.transform.position - base.transform.position) <= 80f)
			{
				agent.speed = 6f;
				state = 1;
				Debug.Log("DOG See");
				woofCount = 1;
				StartCoroutine(woof());
			}
			if (state == 0)
			{
				if (hasReachedDestination())
				{
					chooseRandomPoint();
				}
				audio.PlayOneShot(movingSound);
			}
			else if (state == 1)
			{
				followPlayer();
				audio.PlayOneShot(rickSound);
			}
		}
	}

	private IEnumerator woof()
	{
		yield return new WaitForSeconds(1f);
		woofCount++;
		momoScript.setDestination(player.transform.position);
		yield return new WaitForSeconds(1f);
		woofCount++;
		momoScript.setDestination(player.transform.position);
		yield return new WaitForSeconds(1f);
		woofCount++;
		momoScript.setDestination(player.transform.position);
		yield return new WaitForSeconds(1f);
		woofCount++;
		momoScript.setDestination(player.transform.position);
		agent.ResetPath();
		state = 0;
		isRunningFromPlayer = true;
		agent.speed = 8f;
		yield return new WaitForSeconds(10f);
		isRunningFromPlayer = false;
	}

	public void makeWait(bool shouldWait)
	{
		isWaiting = shouldWait;
	}

	private void Update()
	{
		Vector3 position = playerController.transform.position;
		position.y += playerController.height - 0.5f;
		Vector3 position2 = base.transform.position;
		position2.y += 1f;
		Debug.DrawLine(position2, position);
	}
}
