using UnityEngine;

public class ShitokScript : MonoBehaviour
{
	public GameObject key;

	public bool isOpen;

	public Animator anim;

	public GameObject lockedText;

	public GameObject activateText;

	public GameObject kletka;

	public void activate()
	{
		base.tag = "Untagged";
		kletka.GetComponent<Animator>().Play("GrillOpen");
	}
}
