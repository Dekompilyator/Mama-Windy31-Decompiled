using TouchControlsKit;
using UnityEngine;

namespace Examples
{
	public class FirstPersonExample : MonoBehaviour
	{
		private bool binded;

		private Transform myTransform;

		private Transform cameraTransform;

		private CharacterController controller;

		private float rotation;

		private bool jump;

		private bool prevGrounded;

		private bool isPorjectileCube;

		private float weapReadyTime;

		private bool weapReady = true;

		private void Awake()
		{
			myTransform = base.transform;
			cameraTransform = Camera.main.transform;
			controller = GetComponent<CharacterController>();
		}

		private void Update()
		{
			if (!weapReady)
			{
				weapReadyTime += Time.deltaTime;
				if (weapReadyTime > 0.15f)
				{
					weapReady = true;
					weapReadyTime = 0f;
				}
			}
			if (TCKInput.GetAction("jumpBtn", EActionEvent.Down))
			{
				Jumping();
			}
			if (TCKInput.GetAction("fireBtn", EActionEvent.Press))
			{
				PlayerFiring();
			}
			Vector2 axis = TCKInput.GetAxis("Touchpad");
			PlayerRotation(axis.x, axis.y);
		}

		private void FixedUpdate()
		{
			Vector2 axis = TCKInput.GetAxis("Joystick");
			PlayerMovement(axis.x, axis.y);
		}

		private void Jumping()
		{
			if (controller.isGrounded)
			{
				jump = true;
			}
		}

		private void PlayerMovement(float horizontal, float vertical)
		{
			bool isGrounded = controller.isGrounded;
			Vector3 vector = myTransform.forward * vertical;
			vector += myTransform.right * horizontal;
			vector.y = -10f;
			if (jump)
			{
				jump = false;
				vector.y = 25f;
				isPorjectileCube = !isPorjectileCube;
			}
			if (isGrounded)
			{
				vector *= 7f;
			}
			controller.Move(vector * Time.fixedDeltaTime);
			if (!prevGrounded && isGrounded)
			{
				vector.y = 0f;
			}
			prevGrounded = isGrounded;
		}

		public void PlayerRotation(float horizontal, float vertical)
		{
			myTransform.Rotate(0f, horizontal * 12f, 0f);
			rotation += vertical * 12f;
			rotation = Mathf.Clamp(rotation, -60f, 60f);
			cameraTransform.localEulerAngles = new Vector3(0f - rotation, cameraTransform.localEulerAngles.y, 0f);
		}

		public void PlayerFiring()
		{
			if (weapReady)
			{
				weapReady = false;
				GameObject gameObject = GameObject.CreatePrimitive(isPorjectileCube ? PrimitiveType.Cube : PrimitiveType.Sphere);
				gameObject.transform.position = myTransform.position + myTransform.right;
				gameObject.transform.localScale = Vector3.one * 0.2f;
				Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();
				Transform transform = Camera.main.transform;
				rigidbody.AddForce(transform.forward * Random.Range(25f, 35f) + transform.right * Random.Range(-2f, 2f) + transform.up * Random.Range(-2f, 2f), ForceMode.Impulse);
				Object.Destroy(gameObject, 3.5f);
			}
		}

		public void PlayerClicked()
		{
		}
	}
}
