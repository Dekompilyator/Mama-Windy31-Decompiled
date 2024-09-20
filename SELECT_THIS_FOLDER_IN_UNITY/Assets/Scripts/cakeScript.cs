using System.Collections;
using System.Collections.Generic;
using TouchControlsKit;
using UnityEngine;
using UnityEngine.UI;

public class cakeScript : MonoBehaviour
{
	public GameObject hand;

	public List<Sprite> sprites;

	public SpriteRenderer rend;

	public int index;

	public List<Vector2> positions;

	public List<Sprite> noiseSprites;

	public AudioSource audio;

	public AudioSource audioScr;

	public AudioSource audioScr2;

	public AudioClip noisesound;

	public Animator ending;

	public movingScript player;

	public Image noiseObject;

	public GameObject wall;

	private int noiseIndex;

	public void OnTriggerEnter2D(Collider2D collision)
	{
		hand.SetActive(true);
	}

	public void OnTriggerExit2D(Collider2D collision)
	{
		hand.SetActive(false);
	}

	public IEnumerator noise()
	{
		bool b = true;
		float time = 0f;
		while (b)
		{
			yield return new WaitForSeconds(Time.deltaTime);
			time += Time.deltaTime;
			noiseIndex++;
			if (noiseIndex >= noiseSprites.Count)
			{
				noiseIndex = 0;
			}
			noiseObject.sprite = noiseSprites[noiseIndex];
			if (time >= 0.5f)
			{
				b = false;
			}
		}
		noiseObject.gameObject.SetActive(false);
	}

	public IEnumerator scr()
	{
		bool b = true;
		int i = 0;
		while (b)
		{
			yield return new WaitForSeconds(0.5f);
			audioScr.volume += 0.04f;
			i++;
			if (i >= 10)
			{
				b = false;
			}
		}
	}

	public IEnumerator scr2()
	{
		bool b = true;
		int i = 0;
		while (b)
		{
			yield return new WaitForSeconds(1f);
			audioScr2.volume += 0.01f;
			i++;
			if (i >= 5)
			{
				b = false;
			}
		}
	}

	public void Update()
	{
		if (!TCKInput.GetAction("handBtn", EActionEvent.Down) && !Input.GetKeyDown(KeyCode.E))
		{
			return;
		}
		if (index == 3)
		{
			ending.gameObject.SetActive(true);
			ending.Play("endinganim");
			base.gameObject.SetActive(false);
			player.gameObject.SetActive(false);
			return;
		}
		StartCoroutine(noise());
		noiseObject.gameObject.SetActive(true);
		audio.PlayOneShot(noisesound);
		wall.SetActive(true);
		rend.sprite = sprites[index];
		base.transform.position = positions[index];
		index++;
		player.speed /= 1.35f;
		if (index == 2 && audioScr2.volume <= 0f)
		{
			audioScr2.volume = 0.02f;
			StartCoroutine(scr2());
		}
		if (index == 3 && audioScr.volume <= 0f)
		{
			audioScr.volume = 0.02f;
			StartCoroutine(scr());
		}
	}
}
