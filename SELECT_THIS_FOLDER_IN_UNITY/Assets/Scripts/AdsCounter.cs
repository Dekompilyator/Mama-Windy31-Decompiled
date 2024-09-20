using UnityEngine;

public class AdsCounter : MonoBehaviour
{
	public int adsCount;

	private void Start()
	{
		Object.DontDestroyOnLoad(this);
	}
}
