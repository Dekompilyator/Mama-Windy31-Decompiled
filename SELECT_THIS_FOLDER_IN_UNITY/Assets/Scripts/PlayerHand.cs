using System.Collections;
using System.Collections.Generic;
using SmartFPController;
using TouchControlsKit;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHand : MonoBehaviour
{
	public Camera PlayerCamera;

	public GameObject PickupText;

	public GameObject hand;

	public GameObject handObject;

	public GameObject dropObject;

	public Text itemText;

	private float itemTextTime;

	private bool isItemTextActive;

	public GameObject manager;

	public GameObject momo;

	public List<AudioClip> sounds;

	public float runEnergy = 3f;

	public GameObject runBtn;

	public GameObject runEnergySliderGO;

	public Slider runEnergySlider;

	public Image runEnergyImage;

	public Color runEnergyColor;

	public AudioClip buttonBeep;

	public AudioClip doorIsClosed;

	public AudioClip closeDoor;

	public AudioClip openDoor;

	private GameManager gameManager;

	private Collider itemInHand;

	private Transform cameraTransform;

	private Transform handTransform;

	private Vector3 cameraOffset;

	private Momo momoScript;

	private AudioSource audio;

	public FirstPersonController fpsController;

	private string prevTag;

	private int layerMask;

	private int layerMask2;

	private int handlayer;

	private int itemLayer;

	private void Start()
	{
		cameraTransform = PlayerCamera.transform;
		handTransform = hand.transform;
		layerMask = LayerMask.GetMask("RaycastLayer");
		layerMask2 = LayerMask.GetMask("RaycastItems");
		handlayer = LayerMask.NameToLayer("hand");
		itemLayer = LayerMask.NameToLayer("RaycastItems");
		gameManager = manager.GetComponent<GameManager>();
		momoScript = momo.GetComponent<Momo>();
		cameraOffset = new Vector3(3f, 0f, 0f);
	}

	private void dropItem()
	{
		if (itemInHand != null)
		{
			Rigidbody component = itemInHand.GetComponent<Rigidbody>();
			component.isKinematic = false;
			Transform transform = itemInHand.transform;
			transform.gameObject.layer = itemLayer;
			transform.parent = null;
			transform.position = cameraTransform.position;
			component.velocity = cameraTransform.forward * 10f;
			itemInHand.tag = prevTag;
			prevTag = string.Empty;
			itemInHand = null;
		}
	}

	public void SetItemText(string text, float time)
	{
		itemText.text = text;
		itemTextTime = time;
		isItemTextActive = true;
	}

	private void removeItem()
	{
		if (itemInHand != null)
		{
			prevTag = string.Empty;
			Object.Destroy(itemInHand.gameObject);
		}
	}

	private void playAudio(AudioClip clip)
	{
		if (audio == null)
		{
			audio = GetComponent<AudioSource>();
		}
		audio.PlayOneShot(clip);
	}

	private void FixedUpdate()
	{
		if (gameManager.isStopped)
		{
			return;
		}
		if (TCKInput.GetAction("RunBtn", TouchControlsKit.EActionEvent.Press) || Input.GetKey(KeyCode.LeftShift))
		{
			runEnergy -= 0.01f;
			runEnergySlider.value = runEnergy;
			if (!runEnergySliderGO.activeSelf)
			{
				runEnergySliderGO.SetActive(true);
			}
			if (runEnergy <= 0f)
			{
				runBtn.SetActive(false);
				runEnergyImage.color = Color.red;
				fpsController.canRun = false;
			}
		}
		else if (runEnergy <= 3f)
		{
			if (fpsController.isMoving)
			{
				runEnergy += 0.004f;
				runEnergySlider.value = runEnergy;
			}
			else
			{
				runEnergy += 0.01f;
				runEnergySlider.value = runEnergy;
			}
			if (runEnergy >= 1f && !runBtn.activeSelf)
			{
				runBtn.SetActive(true);
				fpsController.canRun = true;
				runEnergyImage.color = runEnergyColor;
			}
		}
		else if (runEnergySliderGO.activeSelf)
		{
			runEnergySliderGO.SetActive(false);
		}
	}

	private IEnumerator removeWood(GameObject wood)
	{
		yield return new WaitForSeconds(5f);
		Object.Destroy(wood);
	}

	private void Update()
	{
		Vector3 origin = PlayerCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
		int num = layerMask | layerMask2;
		if (isItemTextActive)
		{
			if (itemTextTime > 0f)
			{
				itemTextTime -= Time.deltaTime;
			}
			else
			{
				itemText.text = string.Empty;
				isItemTextActive = false;
			}
		}
		if (gameManager.isStopped)
		{
			return;
		}
		RaycastHit hitInfo;
		if (Physics.Raycast(origin, cameraTransform.forward, out hitInfo, 3f, num))
		{
			string text = hitInfo.collider.tag;
			if (!handObject.activeSelf)
			{
				handObject.SetActive(true);
			}
			if (!TCKInput.GetAction("UseBTN", TouchControlsKit.EActionEvent.Down) && !Input.GetKeyDown(KeyCode.E))
			{
				return;
			}
			switch (text)
			{
			case "Pickable":
			case "Key":
			case "Kukla":
			case "Knife":
			case "Hammer":
			case "Box":
			case "Kraska":
			case "Kist":
			{
				dropItem();
				itemInHand = hitInfo.collider;
				hitInfo.collider.gameObject.layer = handlayer;
				Transform transform3 = itemInHand.transform;
				ItemProps component16 = itemInHand.GetComponent<ItemProps>();
				itemInHand.GetComponent<Rigidbody>().isKinematic = true;
				transform3.parent = handTransform;
				transform3.localPosition = component16.position;
				Vector3 rotation = component16.rotation;
				transform3.localRotation = Quaternion.Euler(rotation);
				prevTag = hitInfo.collider.tag;
				hitInfo.collider.tag = "Untagged";
				SetItemText(component16.descr, 2f);
				break;
			}
			case "Lock":
			case "FinalLock":
			{
				Lock component4 = hitInfo.collider.GetComponent<Lock>();
				if (itemInHand != null && component4.tag == prevTag && (component4.key == null || itemInHand.transform == component4.key.transform))
				{
					component4.removeLock();
				}
				break;
			}
			case "Openable":
			case "FinalDoor":
			{
				if (text == "FinalDoor")
				{
					Locks component7 = hitInfo.collider.GetComponent<Locks>();
					if (component7 != null)
					{
						if (component7.blocks.Count > 0)
						{
							Animator component8 = hitInfo.collider.GetComponent<Animator>();
							component8.Play("ExitShake");
							playAudio(doorIsClosed);
						}
						else if (prevTag == "Box" && gameManager.bodyState >= 4)
						{
							removeItem();
							gameManager.chestIsRemoved();
						}
						else
						{
							SetItemText("Перед уходом нужно собрать подарки", 2f);
						}
					}
					break;
				}
				DoorScript component9 = hitInfo.collider.GetComponent<DoorScript>();
				Locks component10 = hitInfo.collider.GetComponent<Locks>();
				if (!(component10 != null))
				{
					break;
				}
				if (component10.blocks.Count > 0)
				{
					Animator component11 = hitInfo.collider.GetComponent<Animator>();
					if (component9.isFliped)
					{
						component11.Play("Shake2");
						playAudio(doorIsClosed);
					}
					else
					{
						component11.Play("Shake");
						playAudio(doorIsClosed);
					}
					break;
				}
				if (!component9.isOpen)
				{
					playAudio(sounds[4]);
					Animator component12 = hitInfo.collider.GetComponent<Animator>();
					component12.Play("Open");
					playAudio(openDoor);
				}
				else
				{
					playAudio(sounds[3]);
					Animator component13 = hitInfo.collider.GetComponent<Animator>();
					component13.Play("Close");
					playAudio(closeDoor);
				}
				component9.isOpen = !component9.isOpen;
				break;
			}
			case "Body":
				if (prevTag == "Box")
				{
					Debug.Log(itemLayer);
					fpsController.walkSpeed += 0.4f;
					fpsController.runSpeed += 0.4f;
					itemInHand.gameObject.layer = itemLayer;
					gameManager.changeState(itemInHand.transform);
					itemInHand.gameObject.layer = itemLayer;
					itemInHand = null;
					prevTag = string.Empty;
				}
				else
				{
					SetItemText("Нужна коробка", 2f);
				}
				break;
			case "Locker":
			{
				LockerInfo component15 = hitInfo.collider.GetComponent<LockerInfo>();
				Animator animator2 = component15.animator;
				if (component15.isOpen)
				{
					playAudio(sounds[0]);
					component15.isOpen = false;
					animator2.Play("Close");
					component15.lockerEnter.onClose();
				}
				else
				{
					playAudio(sounds[1]);
					component15.isOpen = true;
					animator2.Play("Open");
				}
				break;
			}
			case "lockerBack":
			{
				LockerBack component = hitInfo.collider.GetComponent<LockerBack>();
				LockerInfo lockerInfo = ((!(component == null)) ? component.locker : hitInfo.collider.GetComponent<LockerInfo>());
				Animator animator = lockerInfo.animator;
				if (lockerInfo.isOpen)
				{
					playAudio(sounds[0]);
					lockerInfo.isOpen = false;
					animator.Play("Close");
					lockerInfo.lockerEnter.onClose();
				}
				else
				{
					playAudio(sounds[1]);
					lockerInfo.isOpen = true;
					animator.Play("Open");
				}
				break;
			}
			case "Altar":
				if (prevTag == "Kukla")
				{
					Transform transform = itemInHand.transform;
					transform.parent = hitInfo.collider.transform;
					transform.localPosition = new Vector3(0f, 0f, 1.28f);
					transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 138f));
					altarScript component5 = hitInfo.collider.GetComponent<altarScript>();
					component5.isToyInPlace = true;
					itemInHand.gameObject.layer = LayerMask.GetMask("Default");
					itemInHand = null;
					prevTag = string.Empty;
				}
				else if (prevTag == "Knife")
				{
					altarScript component6 = hitInfo.collider.GetComponent<altarScript>();
					if (component6.isToyInPlace)
					{
						Transform transform2 = itemInHand.transform;
						transform2.parent = hitInfo.collider.transform;
						transform2.localPosition = new Vector3(0f, 0f, 1.5f);
						transform2.localRotation = Quaternion.Euler(new Vector3(0f, 0f, -64.302f));
						itemInHand.gameObject.layer = LayerMask.GetMask("Default");
						itemInHand = null;
						prevTag = string.Empty;
					}
				}
				break;
			case "Doski":
				if (prevTag == "Hammer")
				{
					playAudio(sounds[2]);
					hitInfo.collider.GetComponent<Rigidbody>().isKinematic = false;
					hitInfo.collider.GetComponent<Rigidbody>().velocity = hitInfo.collider.transform.right * -10f;
					hitInfo.collider.tag = "Untagged";
					StartCoroutine(removeWood(hitInfo.collider.gameObject));
				}
				break;
			case "Shitok":
			{
				ShitokScript component14 = hitInfo.collider.GetComponent<ShitokScript>();
				if (!component14.isOpen)
				{
					if (itemInHand != null && component14.key.transform == itemInHand.transform)
					{
						component14.isOpen = true;
						component14.anim.Play("Open");
					}
				}
				else
				{
					component14.activate();
				}
				break;
			}
			case "Kholst":
				if (prevTag == "Kraska")
				{
					KartinaScript component2 = hitInfo.collider.GetComponent<KartinaScript>();
					component2.putPaint(itemInHand);
					itemInHand.gameObject.layer = itemLayer;
					itemInHand = null;
					prevTag = string.Empty;
				}
				else if (prevTag == "Kist")
				{
					KartinaScript component3 = hitInfo.collider.GetComponent<KartinaScript>();
					if (!component3.isPainted && component3.isPaintThere)
					{
						component3.Paint();
					}
				}
				break;
			case "Otkrytka":
				momoScript.makeSleep(5);
				hitInfo.collider.gameObject.SetActive(false);
				break;
			case "Phone":
				if (gameManager.isPhoneRinging)
				{
					playAudio(buttonBeep);
					gameManager.PhoneIsTaken();
				}
				break;
			case "specialDoor":
				gameManager.tryOpenSpecialDoor();
				break;
			case "secretPicture":
				hitInfo.collider.GetComponent<secretPictureScript>().onClick(gameManager);
				break;
			case "friendEnd":
				SceneManager.LoadScene("Mama23");
				break;
			case "Button":
				playAudio(buttonBeep);
				break;
			}
		}
		else
		{
			if (handObject.activeSelf)
			{
				handObject.SetActive(false);
			}
			if (itemInHand != null)
			{
				dropObject.SetActive(true);
			}
			else
			{
				dropObject.SetActive(false);
			}
			if (TCKInput.GetAction("DropBTN", TouchControlsKit.EActionEvent.Down))
			{
				dropItem();
			}
		}
	}
}
