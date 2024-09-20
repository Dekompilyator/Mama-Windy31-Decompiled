using System.Collections;
using UnityEngine;

public class LockerEnter : MonoBehaviour
{
	public GameObject lockerDoor;

	public LockerInfo lockerInfo;

	public Transform creepyPoint;

	public GameManager gameManager;

	public Momo mama;

	private bool isInZone;

	private bool isPlayingAnimation;

	private bool hasSeen;

	public void OnTriggerEnter(Collider other)
	{
		isInZone = true;
		gameManager.isPlayerHidden = true;
		Debug.Log("is Open = " + lockerInfo.isOpen);
		if (!lockerInfo.isOpen)
		{
			Debug.Log(mama.state);
			if (mama.state != 0)
			{
				PlayAnimation();
			}
		}
	}

	public bool hasMamaSeenDoor()
	{
		RaycastHit hitInfo;
		if (Physics.Linecast(mama.transform.position, lockerDoor.transform.position, out hitInfo) && hitInfo.collider.tag == "Locker")
		{
			Debug.Log("Has seen");
			return true;
		}
		if (hitInfo.collider.tag == "Player")
		{
			Debug.Log("Has seen");
			return true;
		}
		Debug.Log(hitInfo.collider.tag);
		Debug.Log("Hasn't seen");
		return false;
	}

	public void onClose()
	{
		hasSeen = hasMamaSeenDoor();
		Debug.Log(isInZone);
		if (isInZone)
		{
			Debug.Log(mama.state);
			if (mama.state != 0)
			{
				PlayAnimation();
			}
		}
	}

	public void PlayAnimation()
	{
		mama.PlaySearchAnimation(creepyPoint, this);
		isPlayingAnimation = true;
	}

	public IEnumerator isWaiting()
	{
		yield return new WaitForSeconds(4f);
		if (hasSeen)
		{
			lockerInfo.animator.Play("Open");
			mama.followPlayer();
			mama.state = 0;
		}
		else
		{
			yield return new WaitForSeconds(1f);
			mama.chooseRandomPoint();
			mama.state = 0;
		}
		mama.GetComponent<Momo>().audio.pitch = 0.64f;
	}

	public void onReaching()
	{
		StartCoroutine(isWaiting());
		mama.InstantlyTurn(mama.player.transform.position);
	}

	public IEnumerator waitBeforeMoving()
	{
		yield return new WaitForSeconds(1f);
		Debug.Log("moving");
	}

	public void OnTriggerExit(Collider other)
	{
		gameManager.isPlayerHidden = false;
		isInZone = false;
		mama.state = 0;
		isPlayingAnimation = false;
		hasSeen = false;
	}
}
