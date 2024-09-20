using System.Collections;
using UnityEngine;

public class seen : MonoBehaviour
{
	public GameObject player;

	public CharacterController playerController;

	public Transform self;

	public SpriteRenderer sprite;

	public Animator anim;

	public Transform newloc;

	public GameManager gameManager;

	private bool b;

	private IEnumerator start()
	{
		b = true;
		gameManager.sdgsdhhdfugj();
		yield return new WaitForSeconds(1f);
		Debug.Log("TEST");
		self.position = newloc.position;
		anim.enabled = false;
	}

	public void Update()
	{
		if (sprite.isVisible)
		{
			if (!anim.isActiveAndEnabled)
			{
				anim.enabled = true;
			}
			if (playerController == null)
			{
				playerController = player.GetComponent<CharacterController>();
			}
			Vector3 position = playerController.transform.position;
			position.y += playerController.height - 1f;
			Debug.Log(Vector3.Distance(position, self.position));
			RaycastHit hitInfo;
			if (Vector3.Distance(position, self.position) <= 12f && Physics.Linecast(self.position, position, out hitInfo) && hitInfo.collider.tag == "Player" && !b)
			{
				StartCoroutine(start());
			}
		}
	}
}
