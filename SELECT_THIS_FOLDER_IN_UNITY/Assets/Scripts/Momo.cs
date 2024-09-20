using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Momo : MonoBehaviour
{
	public AudioSource playerHearth;

	public GameObject black;

	public List<Sprite> flashbacksList;

	public AudioClip movingsound;

	public AudioClip chasingsound;

	public AudioClip foundsound;

	public AudioClip lostsound;

	public GameManager gameManager;

	public List<GameObject> boxes;

	public GameObject player;

	private CharacterController playerController;

	public List<GameObject> movePoints;

	private int pointTarget;

	private NavMeshAgent agent;

	public int state;

	public LockerEnter lockerEnterToChase;

	private Vector3[] playerPositions;

	private float playerPositionsTimer;

	private float playerPositionsIndex;

	public AudioSource audio;

	public float moveTime = 1f;

	public float moveSpeed;

	public float walkingSpeed = 40f;

	public float chasingSpeed = 30f;

	public GameObject GO_MamaText;

	public Text debugText;

	private Text MamaText;

	private bool isWaiting;

	public bool shouldCheck;

	private Vector3 prevPos;

	private Vector3 prevPosAnim;

	private bool isMoving;

	private bool isRunning;

	private int prevPosTime;

	public Animator mamaAnim;

	private Transform mamaPos;

	public int chanceToFollowPlayer = 20;

	public GameObject letstrythis;

	public GameObject iwonderwhatitdoes;

	private float temp_time;

	private bool hasReachedDestination()
	{
		if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance && (!agent.hasPath || agent.velocity.sqrMagnitude == 0f))
		{
			return true;
		}
		return false;
	}

	public void InstantlyTurn(Vector3 destination)
	{
		if (!((destination - base.transform.position).magnitude < 0.1f))
		{
			Vector3 normalized = (destination - base.transform.position).normalized;
			Quaternion b = Quaternion.LookRotation(normalized);
			base.transform.rotation = Quaternion.Slerp(base.transform.rotation, b, Time.deltaTime * 1000f);
		}
	}

	public void playerIsHiding()
	{
		if (state == 1)
		{
			Debug.Log("Has seen player");
		}
		else if (state == 2)
		{
			int num = Random.Range(0, 99);
			Debug.Log("Want " + (80f - playerPositionsIndex * 30f) + ". But got " + num);
			if ((float)num <= 80f - playerPositionsIndex * 30f)
			{
				Debug.Log("Yes");
				state = 4;
				StartCoroutine(waitToMove());
			}
			else
			{
				Debug.Log("NO");
				followPlayer();
				state = 1;
			}
		}
	}

	public void makeSleep(int time)
	{
		MamaText.text = "Mama is sleeping for " + time + " seconds!";
		GO_MamaText.SetActive(true);
		StartCoroutine(makeSleep1(time));
		makeWait(true);
		prevPos = mamaPos.position;
		if (!shouldCheck)
		{
			agent.enabled = false;
			mamaPos.position = new Vector3(0f, 0f, 0f);
		}
	}

	public IEnumerator makeSleep1(int time)
	{
		yield return new WaitForSeconds(4f);
		GO_MamaText.SetActive(false);
		yield return new WaitForSeconds((float)time - 4f);
		mamaPos.position = prevPos;
		agent.enabled = true;
		state = 0;
		chooseRandomPoint();
		temp_time = 0f;
		makeWait(false);
	}

	public void makeWait(bool shouldWait)
	{
		isWaiting = shouldWait;
		agent.enabled = !shouldWait;
	}

	public void chooseRandomPoint(bool shouldFollow = false)
	{
		temp_time = 0f;
		playerHearth.Stop();
		state = 0;
		if (!gameManager.isPlayerHidden || gameManager.ivedoneit)
		{
			if (!gameManager.amihere)
			{
				gameManager.friefasfasedqweqwrq231241();
			}
			int num = Random.Range(0, 25);
			if (num <= 2 || shouldFollow)
			{
				state = 0;
				agent.SetDestination(player.transform.position);
				StartCoroutine(gameManager.sentForPlayer());
			}
			else
			{
				int point = Random.Range(0, movePoints.Count);
				setPoint(point);
				prevPosTime = 0;
			}
		}
		else
		{
			int point2 = Random.Range(0, movePoints.Count);
			setPoint(point2);
			prevPosTime = 0;
		}
	}

	private void setPoint(int point)
	{
		state = 0;
		agent.SetDestination(movePoints[point].transform.position);
		InstantlyTurn(movePoints[point].transform.position);
		pointTarget = point;
	}

	public void setDestination(Vector3 dest)
	{
		agent.SetDestination(dest);
	}

	public void followPlayer()
	{
		agent.SetDestination(player.transform.position);
	}

	private void followFootsteps(int point)
	{
		state = 3;
		agent.SetDestination(playerPositions[point]);
		pointTarget = point;
	}

	private void updatePlayerPositions()
	{
		Debug.Log(playerPositionsIndex);
		if (playerPositionsIndex >= 0f)
		{
			playerPositionsIndex -= Time.deltaTime;
			Debug.Log(playerPositionsIndex);
		}
		else
		{
			mamaAnim.SetBool("isMoving", false);
			StartCoroutine(waitToMove());
			state = 5;
		}
		playerPositionsTimer = 0f;
	}

	private void Start()
	{
		playerPositions = new Vector3[3];
		mamaPos = base.transform;
		agent = GetComponent<NavMeshAgent>();
		agent.SetDestination(movePoints[0].transform.position);
		pointTarget = 0;
		playerController = player.GetComponent<CharacterController>();
		audio = GetComponent<AudioSource>();
		MamaText = GO_MamaText.GetComponent<Text>();
		prevPosAnim = base.transform.position;
		isMoving = false;
		shouldCheck = true;
	}

	private IEnumerator makeSound()
	{
		while (true)
		{
			yield return new WaitForSeconds(1f);
			agent.speed = moveSpeed;
			audio.PlayOneShot(movingsound);
			yield return new WaitForSeconds(0.2f);
			agent.speed = 0f;
		}
	}

	public void hsdfusdfh()
	{
		iwonderwhatitdoes.SetActive(false);
		letstrythis.SetActive(true);
	}

	private IEnumerator moveAndLook()
	{
		while (true)
		{
			yield return new WaitForSeconds(moveTime);
			if (isWaiting)
			{
				continue;
			}
			if (gameManager.ivedoneit)
			{
				followPlayer();
			}
			else
			{
				if ((prevPos - base.transform.position).magnitude < 0.1f)
				{
					prevPosTime++;
					if (prevPosTime >= 3)
					{
						chooseRandomPoint();
						Debug.Log("Мама застряла");
					}
				}
				if (state == 0)
				{
					moveSpeed = walkingSpeed;
					agent.speed = walkingSpeed;
					if (hasReachedDestination())
					{
						chooseRandomPoint();
					}
				}
				else if (state == 1)
				{
					audio.PlayOneShot(foundsound);
					followPlayer();
				}
				else if (state == 2)
				{
					audio.PlayOneShot(foundsound);
					followPlayer();
					playerPositionsTimer += Time.deltaTime;
					updatePlayerPositions();
				}
				else if (state == 4 && hasReachedDestination())
				{
					mamaAnim.SetBool("isRunning", false);
					moveSpeed = walkingSpeed;
					agent.speed = walkingSpeed;
					mamaAnim.SetBool("isMoving", false);
					lockerEnterToChase.onReaching();
				}
			}
			audio.PlayOneShot(movingsound);
			shouldCheck = true;
			yield return new WaitForSeconds(0.2f);
			if (state == 4)
			{
				InstantlyTurn(player.transform.position);
			}
			if (isWaiting)
			{
				agent.enabled = false;
				mamaPos.position = new Vector3(0f, 0f, 0f);
			}
		}
	}

	public void PlaySearchAnimation(Transform t, LockerEnter le)
	{
		Debug.Log("Anim is playing");
		state = 4;
		agent.SetDestination(t.position);
		moveSpeed = walkingSpeed;
		agent.speed = walkingSpeed;
		lockerEnterToChase = le;
	}

	private IEnumerator waitToMove()
	{
		mamaAnim.SetBool("isRunning", false);
		agent.isStopped = true;
		agent.ResetPath();
		Debug.Log("Lost");
		yield return new WaitForSeconds(1f);
		mamaAnim.SetBool("isRunning", false);
		moveSpeed = walkingSpeed;
		agent.speed = walkingSpeed;
		chooseRandomPoint();
	}

	private void Update()
	{
		temp_time += Time.deltaTime;
		Debug.Log(temp_time);
		if (temp_time >= 20f && state == 0)
		{
			chooseRandomPoint();
			temp_time = 0f;
		}
		if (prevPosAnim != mamaPos.position)
		{
			if (!isMoving)
			{
				isMoving = true;
				mamaAnim.SetBool("isMoving", true);
			}
		}
		else if (isMoving)
		{
			isMoving = false;
			mamaAnim.SetBool("isMoving", false);
		}
		prevPosAnim = mamaPos.position;
		if (!shouldCheck)
		{
			return;
		}
		Vector3 position = playerController.transform.position;
		position.y += playerController.height - 0.5f;
		Vector3 position2 = base.transform.position;
		position2.y += 1f;
		float num = Vector3.Distance(position, position2);
		if (num <= 2f && state != 4 && state != 0)
		{
			playerHearth.Stop();
			gameManager.gameEnd();
			return;
		}
		if (num <= 25f)
		{
			RaycastHit hitInfo;
			if (Physics.Linecast(position2, position, out hitInfo) && hitInfo.collider.tag == "Player")
			{
				if (state != 1)
				{
					float num2 = Vector3.Angle(base.transform.forward, player.transform.position - base.transform.position);
					if (num2 <= 120f)
					{
						moveSpeed = chasingSpeed;
						temp_time = 0f;
						state = 1;
						mamaAnim.SetBool("isRunning", true);
						playerHearth.Play(0uL);
						agent.speed = chasingSpeed;
						moveSpeed = chasingSpeed;
						Debug.Log("See");
					}
				}
			}
			else if (state == 1)
			{
				playerPositionsTimer = 0f;
				playerPositionsIndex = 3f;
				state = 2;
				moveSpeed = walkingSpeed;
				agent.speed = walkingSpeed;
				Debug.Log("Not see");
			}
		}
		if (gameManager.ivedoneit)
		{
			followPlayer();
			return;
		}
		if ((prevPos - base.transform.position).magnitude < 0.1f)
		{
			prevPosTime++;
			if (prevPosTime >= 3)
			{
				chooseRandomPoint();
				Debug.Log("Мама застряла");
			}
		}
		if (state == 0)
		{
			audio.clip = movingsound;
			if (!audio.isPlaying)
			{
				audio.Play(0uL);
			}
			moveSpeed = walkingSpeed;
			agent.speed = walkingSpeed;
			if (hasReachedDestination())
			{
				chooseRandomPoint();
			}
		}
		else if (state == 1)
		{
			audio.clip = foundsound;
			if (!audio.isPlaying)
			{
				audio.Play(0uL);
			}
			followPlayer();
		}
		else if (state == 2)
		{
			audio.clip = foundsound;
			if (!audio.isPlaying)
			{
				audio.Play(0uL);
			}
			followPlayer();
			playerPositionsTimer += Time.deltaTime;
			updatePlayerPositions();
		}
		else if (state == 4 && hasReachedDestination())
		{
			mamaAnim.SetBool("isRunning", false);
			moveSpeed = walkingSpeed;
			agent.speed = walkingSpeed;
			mamaAnim.SetBool("isMoving", false);
			lockerEnterToChase.onReaching();
			state = 5;
			audio.pitch = 0.3f;
		}
	}
}
