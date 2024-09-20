using TouchControlsKit;
using UnityEngine;

public class movingScript : MonoBehaviour
{
	public Animator anim;

	public float speed = 1f;

	public Rigidbody2D p;

	public bool canMove;

	private void Update()
	{
		if (canMove)
		{
			if (TCKInput.GetAction("right", EActionEvent.Press) || Input.GetKey(KeyCode.D))
			{
				Vector2 velocity = new Vector2(speed, 0f);
				p.velocity = velocity;
				anim.Play("moving");
			}
			else if (TCKInput.GetAction("left", EActionEvent.Press) || Input.GetKey(KeyCode.A))
			{
				Vector2 velocity = new Vector2(0f - speed, 0f);
				p.velocity = velocity;
				anim.Play("movingleft");
			}
			else
			{
				p.velocity = Vector2.zero;
				anim.Play("stand");
			}
		}
	}
}
