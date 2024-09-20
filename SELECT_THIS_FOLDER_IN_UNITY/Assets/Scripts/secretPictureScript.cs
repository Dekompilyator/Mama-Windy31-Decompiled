using System.Collections.Generic;
using UnityEngine;

public class secretPictureScript : MonoBehaviour
{
	public Animator anim;

	public bool isFixed;

	public bool prev;

	public bool isFirst;

	public bool isLast;

	public secretPictureScript next;

	public List<secretPictureScript> picList;

	public Material matToChange;

	private void unfixAll()
	{
		foreach (secretPictureScript pic in picList)
		{
			pic.prev = false;
		}
	}

	private void fixAll()
	{
		foreach (secretPictureScript pic in picList)
		{
			pic.GetComponent<MeshRenderer>().material = matToChange;
			pic.gameObject.tag = "Untagged";
		}
	}

	public void onClick(GameManager gameManager)
	{
		if (anim == null)
		{
			anim = GetComponent<Animator>();
		}
		if (!isFixed)
		{
			isFixed = true;
			anim.Play("fix");
			if (isFirst || prev)
			{
				if (!isLast)
				{
					next.prev = true;
					return;
				}
				gameManager.startNoise();
				fixAll();
			}
			else
			{
				unfixAll();
			}
		}
		else
		{
			isFixed = false;
			anim.Play("unfix");
			unfixAll();
		}
	}
}
