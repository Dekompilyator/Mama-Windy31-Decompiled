using System.Collections.Generic;
using UnityEngine;

public class RandomGeneration : MonoBehaviour
{
	public List<Transform> spawnPositions;

	public void Start()
	{
		SpawnInRandomPositions();
	}

	public void SpawnInRandomPositions()
	{
		int num = Random.Range(0, spawnPositions.Count);
		base.transform.position = spawnPositions[num].position;
		Debug.Log("Spawned in" + num);
	}
}
