using UnityEngine;

public class roomsMovement : MonoBehaviour
{
	public Transform camera;

	public Transform player;

	public Vector2 newPos;

	public Vector3 newCamPos;

	public void OnTriggerEnter2D(Collider2D collision)
	{
		Vector2 vector = newPos;
		vector.y = player.transform.position.y;
		player.position = vector;
		camera.position = newCamPos;
	}
}
