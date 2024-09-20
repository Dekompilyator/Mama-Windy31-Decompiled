using System.Collections;
using System.Collections.Generic;
using GleyMobileAds;
using SmartFPController;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public Text debugText;

	public bool secretsMadeToBeSeen;

	public Image deathPicture;

	public List<Sprite> deathPictures;

	public AudioSource buzz;

	public Animator flashbackdoor;

	public Camera PlayerCamera;

	public List<Transform> spawns = new List<Transform>();

	public List<Transform> momSpawns = new List<Transform>();

	public List<Transform> chestSpawns = new List<Transform>();

	public List<Transform> chests = new List<Transform>();

	public AdsCounter adsCounter;

	public GameObject phone;

	public GameObject player;

	public GameObject momSprite;

	public GameObject DOGE;

	public GameObject tort;

	public List<Transform> tortStates;

	public int bodyState = -1;

	public List<Sprite> noiseSprites;

	public List<Sprite> secretSprites;

	public Transform assets;

	private int assetsCount;

	public GameObject screamer;

	public GameObject noise;

	public GameObject bodyTimer;

	public GameObject bodyText;

	public GameObject bodyText2;

	public GameObject winText;

	public GameObject winText2;

	public GameObject finalDoorText;

	public GameObject specialDoorText1;

	public GameObject specialDoorText2;

	public GameObject reviveBtn;

	public GameObject letgoBtn;

	public float alpha = 0.5f;

	public AudioSource tickAudio;

	public AudioClip noiseAudio;

	public AudioClip finalAudio;

	public AudioClip naughtyAudio;

	public AudioClip ringingAudio;

	public AudioClip music;

	public AudioClip screamerSound;

	public AudioClip noiseSound;

	public AudioClip helloSound;

	public bool isPhoneRinging;

	public GameObject phonePlotImage;

	public RoomManager roomManager;

	public bool isStopped;

	public bool isFlickering;

	public bool isPlayerHidden;

	public Transform secret1Spawn;

	public Transform secret2Spawn;

	public Color secretNoiseColor;

	public Transform iamhere;

	public GameObject fri;

	public Animator end;

	public bool ivedoneit;

	public bool amihere;

	public List<GameObject> enters;

	public GameObject gameEndFriend;

	private int specialDoorCount;

	public FirstPersonController fpc;

	private int chestCount;

	private int secretInt;

	private AudioSource audioSource;

	private AudioSource phoneAudio;

	private int noiseIndex;

	private Color c;

	private Image img;

	private Transform body;

	private Vector3 bodyOrigin;

	private Text bodyTimerText;

	private chestPlacing chestplacing;

	private bool isFinished;

	public GameObject blackSquare;

	private bool makeSound = true;

	public Momo momoScript;

	private DOGE dogeScript;

	private int randomNum;

	private List<Transform> placedGifts = new List<Transform>();

	private int removedGiftsCount;

	private bool hasRevived;

	private Transform tutorialPlacing;

	private float buttonWidth = Screen.width / 4;

	private float buttonHeight = Screen.height / 13;

	private bool isSecretImage;

	public AudioSource phoneSource;

	private void Start()
	{
		secretInt = 2;
		phoneAudio = phone.GetComponent<AudioSource>();
		int index = (randomNum = Random.Range(0, 2));
		Transform transform = tort.transform;
		chestplacing = spawns[index].gameObject.GetComponent<chestPlacing>();
		tutorialPlacing = chestplacing.tutorialPlacing;
		body = transform;
		transform.parent = spawns[index];
		transform.localPosition = new Vector3(0f, 0f, 0f);
		transform.localRotation = Quaternion.Euler(chestplacing.tortAngle);
		bodyOrigin = transform.position;
		noise.SetActive(true);
		isFlickering = true;
		bodyTimerText = bodyTimer.GetComponent<Text>();
		audioSource = player.GetComponent<AudioSource>();
		StartCoroutine(timerTick());
		c = Color.white;
		hasRevived = false;
		generateChest();
	}

	public IEnumerator Secret1Spawn()
	{
		blackSquare.SetActive(false);
		phoneAudio.Stop();
		isPhoneRinging = false;
		buzz.volume = 0.3f;
		buzz.clip = Resources.Load("basement") as AudioClip;
		buzz.Play(0uL);
		screamer.SetActive(false);
		player.transform.position = secret1Spawn.position;
		isFlickering = true;
		alpha = 0.3f;
		noise.SetActive(true);
		player.GetComponent<FirstPersonController>().walkSpeed = 0.2f;
		player.GetComponent<FirstPersonController>().runSpeed = 0.5f;
		yield return new WaitForSeconds(7f);
		flashbackdoor.Play("tryopenanim1");
		yield return new WaitForSeconds(1f);
		flashbackdoor.Play("tryopenanim2");
		yield return new WaitForSeconds(3f);
		blackSquare.SetActive(true);
		buzz.volume = 0.6f;
		yield return new WaitForSeconds(2f);
		SceneManager.LoadScene("Mama23");
	}

	public IEnumerator Secret2Spawn()
	{
		blackSquare.SetActive(false);
		phoneAudio.Stop();
		isPhoneRinging = false;
		buzz.clip = Resources.Load("cry") as AudioClip;
		buzz.Play(0uL);
		screamer.SetActive(false);
		player.transform.position = secret2Spawn.position;
		isFlickering = true;
		alpha = 0.2f;
		noise.SetActive(true);
		player.GetComponent<FirstPersonController>().walkSpeed = 0.2f;
		player.GetComponent<FirstPersonController>().runSpeed = 0.5f;
		yield return new WaitForSeconds(15f);
		blackSquare.SetActive(true);
		buzz.volume = 0.4f;
		yield return new WaitForSeconds(2f);
		SceneManager.LoadScene("Mama23");
	}

	public IEnumerator treeterwwette()
	{
		yield return new WaitForSeconds(1f);
		fri.transform.position = iamhere.position;
		end.enabled = false;
	}

	public IEnumerator gottafindawaytocontrol()
	{
		yield return new WaitForSeconds(3f);
		momoScript.walkingSpeed = 10f;
		momoScript.chasingSpeed = 10f;
		momoScript.moveSpeed = 10f;
	}

	public void sdgsdhhdfugj()
	{
		audioSource.PlayOneShot(helloSound);
		momoScript.audio.clip = noiseAudio;
		momoScript.audio.Play(0uL);
		momoScript.audio.loop = true;
		momoScript.audio.pitch = 0.5f;
		ivedoneit = true;
		momoScript.hsdfusdfh();
		momoScript.walkingSpeed = 3f;
		momoScript.chasingSpeed = 3f;
		momoScript.moveSpeed = 3f;
		momoScript.shouldCheck = false;
		StartCoroutine(gottafindawaytocontrol());
	}

	public void friefasfasedqweqwrq231241()
	{
		int num = Random.Range(0, 100);
		if (num <= 1)
		{
			Debug.Log("TEST");
			audioSource.PlayOneShot(helloSound);
			end.enabled = true;
			fri.transform.position = player.transform.position - player.transform.forward * 2f;
			StartCoroutine(treeterwwette());
		}
	}

	public void onReviveBtn()
	{
		reviveBtn.SetActive(false);
		letgoBtn.SetActive(false);
		Debug.Log(adsCounter.adsCount);
		if (adsCounter.adsCount == 0 || adsCounter.adsCount > 4)
		{
			if (Advertisements.Instance.IsRewardVideoAvailable())
			{
				Advertisements.Instance.ShowRewardedVideo(CompleteMethod);
				adsCounter.adsCount = 1;
			}
			else
			{
				CompleteMethod(true, string.Empty);
			}
		}
		else
		{
			adsCounter.adsCount++;
			CompleteMethod(true, string.Empty);
		}
	}

	private void CompleteMethod(bool completed, string advertiser)
	{
		if (Advertisements.Instance.debug)
		{
			Debug.Log("Closed rewarded from: " + advertiser + " -> Completed " + completed);
			ScreenWriter.Write("Closed rewarded from: " + advertiser + " -> Completed " + completed);
		}
		if (completed)
		{
			spawnMom();
			momoScript.shouldCheck = true;
			momoScript.makeWait(false);
			screamer.SetActive(false);
			momoScript.chooseRandomPoint();
			blackSquare.SetActive(false);
			momoScript.audio.Play(0uL);
			hasRevived = true;
		}
		else
		{
			StartCoroutine(showSecretEnding());
		}
	}

	public void onLetgoBtn()
	{
		reviveBtn.SetActive(false);
		letgoBtn.SetActive(false);
		Debug.Log(adsCounter.adsCount);
		if (adsCounter.adsCount != 0)
		{
			if (adsCounter.adsCount < 4)
			{
				adsCounter.adsCount++;
			}
			else
			{
				adsCounter.adsCount = 0;
			}
		}
		Debug.Log(adsCounter.adsCount);
		if (adsCounter.adsCount == 0)
		{
			if (Advertisements.Instance.IsInterstitialAvailable())
			{
				Advertisements.Instance.ShowInterstitial(InterstitialClosed);
				adsCounter.adsCount = 1;
			}
			else
			{
				StartCoroutine(showSecretEnding());
			}
		}
		else
		{
			StartCoroutine(showSecretEnding());
		}
	}

	private void InterstitialClosed(string advertiser)
	{
		if (Advertisements.Instance.debug)
		{
			Debug.Log("Interstitial closed from: " + advertiser + " -> Resume Game ");
			ScreenWriter.Write("Interstitial closed from: " + advertiser + " -> Resume Game ");
		}
		adsCounter.adsCount = 1;
		StartCoroutine(showSecretEnding());
	}

	public void tryOpenSpecialDoor()
	{
		specialDoorCount++;
		if (specialDoorCount == 1)
		{
			isStopped = true;
			specialDoorText1.SetActive(true);
			noise.SetActive(true);
			isFlickering = true;
			alpha = 1f;
			fpc.canWalk = false;
			StartCoroutine(specialDoorWait1());
		}
		else if (specialDoorCount == 2)
		{
			isStopped = true;
			specialDoorText2.SetActive(true);
			noise.SetActive(true);
			isFlickering = true;
			alpha = 1f;
			fpc.canWalk = false;
			StartCoroutine(specialDoorWait2());
		}
		else if (specialDoorCount == 3)
		{
			gameEnd();
		}
	}

	private IEnumerator specialDoorWait1()
	{
		yield return new WaitForSeconds(0.5f);
		specialDoorText1.SetActive(false);
		isFlickering = false;
		noise.SetActive(false);
		fpc.canWalk = true;
		isStopped = false;
	}

	private IEnumerator specialDoorWait2()
	{
		yield return new WaitForSeconds(0.5f);
		specialDoorText2.SetActive(false);
		isFlickering = false;
		noise.SetActive(false);
		fpc.canWalk = true;
		isStopped = false;
	}

	private void generateChest()
	{
		Transform transform = chests[0];
		int index = Random.Range(0, chestSpawns.Count);
		transform.position = chestSpawns[index].position;
		transform.rotation = chestSpawns[index].rotation;
		chestSpawns.Remove(chestSpawns[index]);
		chests.Remove(transform);
	}

	private void generatePhone()
	{
		generateChest();
	}

	public void changeState(Transform chest)
	{
		bodyState++;
		chest.transform.parent = spawns[randomNum];
		chest.transform.position = chestplacing.placing[bodyState].position;
		chest.transform.rotation = Quaternion.Euler(new Vector3(-90f, -90f, 0f));
		placedGifts.Add(chest);
		audioSource.PlayOneShot(noiseSound);
		if (bodyState != 0)
		{
			if (bodyState == 1)
			{
				momoScript.chanceToFollowPlayer = 3;
			}
			if (bodyState <= 2)
			{
				momoScript.moveTime -= 0.1f;
			}
			else
			{
				momoScript.moveTime -= 0.05f;
			}
		}
		momoScript.walkingSpeed += 0.5f;
		momoScript.moveSpeed += 0.5f;
		momoScript.chasingSpeed += 0.5f;
		momoScript.chooseRandomPoint(true);
		if (bodyState < 4)
		{
			body.parent = null;
			body.transform.position = Vector3.zero;
			body = tortStates[bodyState];
			body.parent = spawns[randomNum];
			body.localPosition = Vector3.zero;
			body.rotation = Quaternion.Euler(chestplacing.tortAngle);
		}
		else
		{
			body.parent = null;
			body.transform.position = Vector3.zero;
		}
		if (bodyState == 1)
		{
			startNoise();
			Debug.Log("Started");
			isPhoneRinging = true;
			StartCoroutine(phoneRinging());
		}
		else if (bodyState == 4)
		{
			isStopped = true;
			winText.SetActive(true);
			noise.SetActive(true);
			isFlickering = true;
			alpha = 1f;
			fpc.canWalk = false;
			StartCoroutine(fiveChests());
		}
		else
		{
			startNoise();
			generateChest();
		}
	}

	public void chestIsRemoved()
	{
		removedGiftsCount++;
		startNoise();
		audioSource.PlayOneShot(noiseSound);
		momoScript.chooseRandomPoint(true);
		if (removedGiftsCount == 3)
		{
			momoScript.enabled = false;
			gameEndFriend.SetActive(true);
		}
	}

	public void startNoise()
	{
		isFlickering = true;
		StartCoroutine(noiseStop());
		noise.SetActive(true);
		alpha = 1f;
	}

	public IEnumerator noiseStop()
	{
		yield return new WaitForSeconds(0.5f);
		isFlickering = false;
		noise.SetActive(false);
	}

	public IEnumerator fiveChests()
	{
		yield return new WaitForSeconds(1f);
		winText.SetActive(false);
		isFlickering = false;
		noise.SetActive(false);
		fpc.canWalk = true;
		isStopped = false;
		foreach (Transform placedGift in placedGifts)
		{
			placedGift.tag = "Box";
			placedGift.gameObject.layer = LayerMask.NameToLayer("RaycastItems");
		}
	}

	public IEnumerator phoneRinging()
	{
		while (isPhoneRinging)
		{
			phoneAudio.PlayOneShot(ringingAudio);
			yield return new WaitForSeconds(4f);
		}
	}

	public IEnumerator showPhonePlotImage()
	{
		isSecretImage = true;
		alpha = 0.8f;
		noise.SetActive(true);
		audioSource.PlayOneShot(noiseSound);
		yield return new WaitForSeconds(0.5f);
		isSecretImage = false;
		phonePlotImage.SetActive(false);
		noise.SetActive(false);
		generateChest();
	}

	public void PhoneIsTaken()
	{
		if (secretsMadeToBeSeen)
		{
			int num = Random.Range(0, 1000);
			isPhoneRinging = false;
			phoneAudio.Stop();
			Debug.Log(num);
			if (!PlayerPrefs.HasKey("herCall"))
			{
				PlayerPrefs.SetInt("herCall", 0);
				audioSource.volume = 0.5f;
				audioSource.PlayOneShot(Resources.Load("она") as AudioClip);
			}
			else
			{
				int @int = PlayerPrefs.GetInt("herCall");
				Debug.Log(@int);
				if (num <= 200)
				{
					audioSource.volume = 1f;
					audioSource.PlayOneShot(finalAudio);
					PlayerPrefs.SetInt("herCall", @int + 1);
				}
				else if (num <= 500)
				{
					PlayerPrefs.SetInt("herCall", @int + 1);
					StartCoroutine(showPhonePlotImage());
				}
				else if (num <= 950)
				{
					if (PlayerPrefs.HasKey("PhonesFromMinigame"))
					{
						int int2 = PlayerPrefs.GetInt("PhonesFromMinigame");
						Debug.Log(int2);
						if (int2 >= 10)
						{
							SceneManager.LoadScene("Minigame");
							PlayerPrefs.SetInt("PhonesFromMinigame", 0);
							PlayerPrefs.SetInt("herCall", @int + 1);
						}
						else if (@int >= 15)
						{
							if (Random.Range(0, 2) == 0)
							{
								PlayerPrefs.SetInt("herCall", 0);
								phoneSource.volume = 0.5f;
								phoneSource.PlayOneShot(Resources.Load("она") as AudioClip);
							}
							else
							{
								PlayerPrefs.SetInt("PhonesFromMinigame", int2 + 1);
								PlayerPrefs.SetInt("herCall", @int + 1);
								audioSource.PlayOneShot(noiseSound);
							}
						}
						else
						{
							PlayerPrefs.SetInt("PhonesFromMinigame", int2 + 1);
							PlayerPrefs.SetInt("herCall", @int + 1);
							audioSource.PlayOneShot(noiseSound);
						}
					}
					else
					{
						PlayerPrefs.SetInt("PhonesFromMinigame", 0);
						PlayerPrefs.SetInt("herCall", @int + 1);
						SceneManager.LoadScene("Minigame");
					}
				}
				else
				{
					audioSource.PlayOneShot(noiseSound);
				}
			}
		}
		else
		{
			isPhoneRinging = false;
			phoneAudio.Stop();
			int num2 = Random.Range(0, 100);
			if (!PlayerPrefs.HasKey("herCall"))
			{
				PlayerPrefs.SetInt("herCall", 0);
				audioSource.volume = 0.5f;
				audioSource.PlayOneShot(Resources.Load("она") as AudioClip);
			}
			else
			{
				int int3 = PlayerPrefs.GetInt("herCall");
				int int4 = PlayerPrefs.GetInt("PhonesFromMinigame");
				if (num2 <= 20)
				{
					if (PlayerPrefs.HasKey("PhonesFromMinigame"))
					{
						Debug.Log("Phones are " + int4);
						if (int4 >= 10)
						{
							SceneManager.LoadScene("Minigame");
							PlayerPrefs.SetInt("PhonesFromMinigame", 0);
							PlayerPrefs.SetInt("herCall", int3 + 1);
						}
						else
						{
							PlayerPrefs.SetInt("PhonesFromMinigame", int4 + 1);
							PlayerPrefs.SetInt("herCall", int3 + 1);
							audioSource.PlayOneShot(noiseSound);
						}
					}
					else
					{
						PlayerPrefs.SetInt("PhonesFromMinigame", 0);
						PlayerPrefs.SetInt("herCall", int3 + 1);
						SceneManager.LoadScene("Minigame");
					}
				}
				else if (num2 <= 40)
				{
					PlayerPrefs.SetInt("herCall", int3 + 1);
					PlayerPrefs.SetInt("PhonesFromMinigame", int4 + 1);
					StartCoroutine(showPhonePlotImage());
				}
				else if (num2 <= 50)
				{
					audioSource.volume = 1f;
					PlayerPrefs.SetInt("herCall", int3 + 1);
					PlayerPrefs.SetInt("PhonesFromMinigame", int4 + 1);
					audioSource.PlayOneShot(finalAudio);
				}
				else
				{
					PlayerPrefs.SetInt("PhonesFromMinigame", int4 + 1);
					if (int3 >= 15)
					{
						if (Random.Range(0, 2) == 0)
						{
							PlayerPrefs.SetInt("herCall", 0);
							audioSource.volume = 0.5f;
							audioSource.PlayOneShot(Resources.Load("она") as AudioClip);
						}
						else
						{
							PlayerPrefs.SetInt("herCall", int3 + 1);
							audioSource.PlayOneShot(noiseSound);
						}
					}
					else
					{
						PlayerPrefs.SetInt("herCall", int3 + 1);
						audioSource.PlayOneShot(noiseSound);
					}
				}
			}
		}
		generateChest();
		PlayerPrefs.Save();
	}

	private void spawnMom()
	{
		momoScript.shouldCheck = false;
		Vector3 position = player.transform.position;
		int num = 0;
		float num2 = Vector3.Distance(position, momSpawns[0].transform.position);
		float num3 = Vector3.Distance(position, momSpawns[1].transform.position);
		if (num2 < num3)
		{
			num2 = num3;
			num = 1;
		}
		num3 = Vector3.Distance(position, momSpawns[2].transform.position);
		if (num2 < num3)
		{
			num2 = num3;
			num = 2;
		}
		momSprite.transform.position = momSpawns[num].transform.position;
		Debug.Log("Place is " + num);
		Debug.Log(momSpawns[num].transform.position);
		Debug.Log(momSprite.transform.position);
		momoScript.state = 0;
		momoScript.mamaAnim.SetBool("isRunning", false);
		momoScript.shouldCheck = true;
	}

	private IEnumerator timerTick()
	{
		int i = 5;
		int prevTick = 0;
		while (i > 0)
		{
			if (makeSound)
			{
				if (prevTick == 0)
				{
					if (i <= 5)
					{
						audioSource.PlayOneShot(finalAudio);
					}
					else
					{
						audioSource.PlayOneShot(noiseAudio);
					}
				}
				prevTick++;
				if (prevTick == 5)
				{
					prevTick = 0;
				}
			}
			else
			{
				prevTick = 0;
			}
			yield return new WaitForSeconds(1f);
			i--;
			Debug.Log("I IS " + i);
			bodyTimerText.text = i.ToString();
		}
		momoScript = momSprite.GetComponent<Momo>();
		isFinished = true;
		isFlickering = false;
		noise.SetActive(false);
		Object.Destroy(bodyText);
		Object.Destroy(bodyTimerText);
		audioSource.volume = 1f;
		audioSource.PlayOneShot(naughtyAudio);
		roomManager.generateKeys();
		yield return new WaitForSeconds(3f);
		momSprite.SetActive(true);
		audioSource.PlayOneShot(music);
		didifindit();
		StartCoroutine(musicTimer());
	}

	public IEnumerator sentForPlayer()
	{
		tickAudio.pitch = 2f;
		tickAudio.volume = 0.08f;
		yield return new WaitForSeconds(5f);
		tickAudio.volume = 0.025f;
		tickAudio.pitch = 1f;
	}

	public void didifindit()
	{
		int num = Random.Range(0, 1000);
		if (secretsMadeToBeSeen)
		{
			num = Random.Range(0, 10);
		}
		if (num <= 50)
		{
			if (PlayerPrefs.HasKey("FriendBeenHere"))
			{
				int @int = PlayerPrefs.GetInt("FriendBeenHere");
				Debug.Log("FriendBeenHere is " + @int);
				if (@int >= 20)
				{
					PlayerPrefs.SetInt("FriendBeenHere", 0);
					amihere = true;
					num = Random.Range(0, enters.Count);
					GameObject gameObject = enters[num];
					foreach (GameObject enter in enters)
					{
						if (enter.transform != gameObject.transform)
						{
							Object.Destroy(enter);
						}
					}
					enters.Clear();
					gameObject.SetActive(true);
				}
				else
				{
					PlayerPrefs.SetInt("FriendBeenHere", @int + 1);
					foreach (GameObject enter2 in enters)
					{
						Object.Destroy(enter2);
					}
					enters.Clear();
				}
			}
			else
			{
				PlayerPrefs.SetInt("FriendBeenHere", 0);
				amihere = true;
				num = Random.Range(0, enters.Count);
				GameObject gameObject2 = enters[num];
				foreach (GameObject enter3 in enters)
				{
					if (enter3.transform != gameObject2.transform)
					{
						Object.Destroy(enter3);
					}
				}
				enters.Clear();
				gameObject2.SetActive(true);
			}
		}
		else
		{
			foreach (GameObject enter4 in enters)
			{
				Object.Destroy(enter4);
			}
			enters.Clear();
		}
		PlayerPrefs.Save();
	}

	public IEnumerator picture()
	{
		int r = Random.Range(0, deathPictures.Count);
		deathPicture.gameObject.SetActive(true);
		deathPicture.sprite = deathPictures[r];
		audioSource.PlayOneShot(noiseAudio);
		yield return new WaitForSeconds(3f);
		SceneManager.LoadScene("Mama23");
	}

	public IEnumerator showSecretEnding()
	{
		yield return new WaitForSeconds(1f);
		int r5 = Random.Range(0, 500);
		if (secretsMadeToBeSeen)
		{
			r5 = Random.Range(0, 10);
		}
		if (r5 <= 30)
		{
			if (PlayerPrefs.HasKey("DeathsWithoutMinigames"))
			{
				int d = PlayerPrefs.GetInt("DeathsWithoutMinigames");
				Debug.Log("Deathsfromminigames are " + d);
				yield return new WaitForSeconds(1f);
				if (d >= 10)
				{
					PlayerPrefs.SetInt("DeathsWithoutMinigames", 0);
					r5 = Random.Range(0, 2);
					Debug.Log("R = " + r5);
					if (r5 == 0)
					{
						Debug.Log("R1 = " + r5);
						StartCoroutine(Secret1Spawn());
					}
					else
					{
						Debug.Log("R2 = " + r5);
						StartCoroutine(Secret2Spawn());
					}
				}
				else
				{
					PlayerPrefs.SetInt("DeathsWithoutMinigames", d + 1);
					int r6 = Random.Range(0, 10);
					Debug.Log("TEST " + r6);
					if (r6 >= 6)
					{
						r5 = Random.Range(0, deathPictures.Count);
						deathPicture.gameObject.SetActive(true);
						deathPicture.sprite = deathPictures[r5];
						audioSource.PlayOneShot(noiseAudio);
						yield return new WaitForSeconds(3f);
						PlayerPrefs.Save();
						SceneManager.LoadScene("Mama23");
					}
					else
					{
						PlayerPrefs.Save();
						SceneManager.LoadScene("Mama23");
					}
				}
			}
			else
			{
				yield return new WaitForSeconds(1f);
				PlayerPrefs.SetInt("DeathsWithoutMinigames", 0);
				r5 = Random.Range(0, 2);
				Debug.Log("R = " + r5);
				if (r5 == 0)
				{
					Debug.Log("R1 = " + r5);
					StartCoroutine(Secret1Spawn());
				}
				else
				{
					Debug.Log("R2 = " + r5);
					StartCoroutine(Secret2Spawn());
				}
			}
		}
		else
		{
			int r7 = Random.Range(0, 10);
			Debug.Log("TEST " + r7);
			if (r7 >= 6)
			{
				r5 = Random.Range(0, deathPictures.Count);
				deathPicture.gameObject.SetActive(true);
				deathPicture.sprite = deathPictures[r5];
				audioSource.PlayOneShot(noiseAudio);
				yield return new WaitForSeconds(3f);
				PlayerPrefs.Save();
				SceneManager.LoadScene("Mama23");
			}
			else
			{
				PlayerPrefs.Save();
				SceneManager.LoadScene("Mama23");
			}
		}
		PlayerPrefs.Save();
	}

	public IEnumerator waitToEndTheGame()
	{
		momoScript.audio.Stop();
		Debug.Log(momoScript);
		Debug.Log("ENDGAME");
		yield return new WaitForSeconds(1.7f);
		blackSquare.SetActive(true);
		adsCounter = GameObject.Find("AdsCount").GetComponent<AdsCounter>();
		if (!hasRevived)
		{
			reviveBtn.SetActive(true);
			letgoBtn.SetActive(true);
			yield break;
		}
		if (adsCounter.adsCount != 0)
		{
			if (adsCounter.adsCount < 4)
			{
				adsCounter.adsCount++;
			}
			else
			{
				adsCounter.adsCount = 0;
			}
		}
		if (Advertisements.Instance.IsInterstitialAvailable())
		{
			if (adsCounter.adsCount == 0)
			{
				Advertisements.Instance.ShowInterstitial(InterstitialClosed);
				adsCounter.adsCount = 1;
			}
			else
			{
				StartCoroutine(showSecretEnding());
			}
		}
		else
		{
			StartCoroutine(showSecretEnding());
		}
	}

	public void gameEnd()
	{
		StartCoroutine(waitToEndTheGame());
		screamer.SetActive(true);
		audioSource.PlayOneShot(screamerSound);
		momoScript.shouldCheck = false;
		momoScript.makeWait(true);
	}

	private IEnumerator musicTimer()
	{
		while (true)
		{
			yield return new WaitForSeconds(110f);
			audioSource.PlayOneShot(music);
			if (momoScript.state != 1)
			{
				spawnMom();
			}
		}
	}

	private void Update()
	{
		if (ivedoneit)
		{
			assetsCount = assets.childCount;
			if (assetsCount > 0)
			{
				int index = Random.Range(0, assetsCount - 1);
				Transform child = assets.GetChild(index);
				MeshRenderer component = child.gameObject.GetComponent<MeshRenderer>();
				if (component != null)
				{
					component.material = null;
				}
				child.parent = null;
				assetsCount--;
			}
		}
		if (!isFinished)
		{
			if (img == null)
			{
				img = noise.GetComponent<Image>();
			}
			Vector3 from = bodyOrigin - PlayerCamera.transform.position;
			Vector3 forward = PlayerCamera.transform.forward;
			float num = Vector3.Angle(from, forward);
			if (num <= 45f)
			{
				alpha = (45f - num) / 45f;
				if (alpha <= 0.2f)
				{
					alpha = 0.2f;
				}
				c.a = alpha;
				img.color = c;
			}
			audioSource.volume = alpha + 0.2f;
			img.sprite = noiseSprites[noiseIndex];
			if (noiseIndex == 2)
			{
				noiseIndex = 0;
			}
			else
			{
				noiseIndex++;
			}
		}
		else if (isFlickering)
		{
			c.a = alpha;
			img.color = c;
			audioSource.volume = alpha - 0.5f;
			if (noiseIndex == 2)
			{
				noiseIndex = 0;
			}
			else
			{
				noiseIndex++;
			}
			img.sprite = noiseSprites[noiseIndex];
		}
		if (isSecretImage)
		{
			c.a = alpha;
			img.color = c;
			audioSource.volume = alpha - 0.5f;
			if (noiseIndex == 2)
			{
				noiseIndex = 0;
			}
			else
			{
				noiseIndex++;
			}
			img.sprite = noiseSprites[noiseIndex];
			int num2 = Random.Range(0, 100);
			if (num2 <= 5)
			{
				img.sprite = phonePlotImage.GetComponent<Image>().sprite;
			}
		}
	}
}
