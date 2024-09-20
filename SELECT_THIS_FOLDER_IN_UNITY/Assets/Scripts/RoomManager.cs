using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
	[Serializable]
	public class Room
	{
		public GameObject key;

		public List<Transform> locationsForKey;
	}

	public List<Room> rooms;

	public List<Transform> randomInFree;

	public GameObject finalKey;

	public void setPos(Transform t1, Transform t2)
	{
		t1.position = t2.position;
		t1.rotation = t2.rotation;
	}

	public void generateKeys()
	{
		if (rooms.Count > 0)
		{
			int num = UnityEngine.Random.Range(0, rooms.Count);
			int num2 = UnityEngine.Random.Range(0, randomInFree.Count);
			setPos(rooms[num].key.transform, randomInFree[num2]);
			Debug.Log("Заспавнил " + num + " в " + num2);
			Room room = rooms[num];
			rooms.Remove(room);
			while (rooms.Count > 0)
			{
				num = UnityEngine.Random.Range(0, rooms.Count);
				num2 = UnityEngine.Random.Range(0, room.locationsForKey.Count);
				Debug.Log("Заспавнил " + num + " в " + num2);
				setPos(rooms[num].key.transform, room.locationsForKey[num2]);
				room = rooms[num];
				rooms.Remove(room);
			}
			num2 = UnityEngine.Random.Range(0, room.locationsForKey.Count);
			setPos(finalKey.transform, room.locationsForKey[num2]);
			Debug.Log("Заспавнил финалку в " + num2);
		}
	}
}
