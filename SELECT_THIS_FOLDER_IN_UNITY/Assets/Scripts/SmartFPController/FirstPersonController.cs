using System.Collections;
using SmartFPController.Utils;
using UnityEngine;

namespace SmartFPController
{
	[RequireComponent(typeof(CameraHeadBob), typeof(FootstepSFXManager), typeof(CharacterController))]
	public class FirstPersonController : MonoBehaviour
	{
		[SerializeField]
		private GameObject hand;

		[SerializeField]
		public bool canWalk = true;

		[SerializeField]
		[Range(1f, 7f)]
		public float walkSpeed = 4.25f;

		[SerializeField]
		[Range(0f, 1f)]
		private float backwardsSpeed = 0.6f;

		[SerializeField]
		[Range(0f, 1f)]
		private float sidewaysSpeed = 0.7f;

		[SerializeField]
		[Range(0f, 1f)]
		private float inAirSpeed = 0.35f;

		[SerializeField]
		public bool canRun = true;

		[SerializeField]
		[Range(0f, 1f)]
		public float runSpeed = 8.75f;

		[SerializeField]
		private bool canCrouch = true;

		[SerializeField]
		[Range(0f, 1f)]
		private float crouchSpeed = 0.45f;

		[SerializeField]
		[Range(1f, 1.8f)]
		private float crouchHeight = 1.25f;

		[SerializeField]
		private bool canJump = true;

		[SerializeField]
		[Range(1f, 10f)]
		private float jumpForce = 5f;

		[SerializeField]
		private bool canClimb = true;

		[SerializeField]
		[Range(0f, 1f)]
		private float climbingSpeed = 0.8f;

		[SerializeField]
		private bool useHeadBob = true;

		[SerializeField]
		[Range(0f, 1f)]
		private float posForce = 0.65f;

		[SerializeField]
		[Range(0f, 1f)]
		private float tiltForce = 0.85f;

		[SerializeField]
		[Range(1f, 5f)]
		private float gravityMultiplier = 2f;

		[SerializeField]
		[Range(1f, 5f)]
		private float fallingDistanceToDamage = 3f;

		[SerializeField]
		[Range(1f, 10f)]
		private float fallingDamageMultiplier = 3.5f;

		[SerializeField]
		private string damageFunction = "TakeDamage";

		[SerializeField]
		[Range(0.1f, 1.5f)]
		private float stepInterval = 0.5f;

		[SerializeField]
		[Range(0.1f, 1f)]
		private float lookSmooth = 1f;

		[SerializeField]
		[Range(25f, 90f)]
		private float maxLookAngleY = 65f;

		[SerializeField]
		private Vector3 cameraOffset = Vector3.up;

		private Vector3 m_Velocity;

		private Vector2 m_DeltaPosition;

		private RaycastHit m_FloorHit;

		private bool prevGrounded;

		private bool jump;

		private bool crouching;

		private CharacterController m_Controller;

		private CollisionFlags collisionFlags;

		private Transform m_Transform;

		private Transform cameraTransform;

		private Vector3 moveDirection;

		private Vector3 crouchVelVec;

		private float nextStep;

		private float nativeCapsuleHeight;

		private float crouchVel;

		private float fallingStartPos;

		private float fallingDist;

		private float deltaSin;

		private float deltaCos;

		private Ladder currentLadder;

		private Vector3 nativeCapsuleCenter;

		private FootstepSFXManager m_FootstepSFXManager;

		private CameraHeadBob m_HeadBob;

		private Vector2 lookRotation = Vector2.zero;

		private Quaternion nativeRotation = Quaternion.identity;

		private Transform handTransform;

		private Vector3 handOffset;

		private SmartInputManager m_Input;

		public Transform getTransform
		{
			get
			{
				return m_Transform;
			}
		}

		public bool isGrounded { get; private set; }

		public bool isClimbing { get; private set; }

		public bool isMoving { get; private set; }

		public bool isMoveForward { get; private set; }

		public bool isRunning { get; private set; }

		public bool isCrouched { get; private set; }

		public bool isJumping { get; private set; }

		public bool isFalling { get; private set; }

		public Vector3 velocity
		{
			get
			{
				return m_Velocity;
			}
		}

		public float velocitySize { get; private set; }

		public Vector2 deltaPosition
		{
			get
			{
				return m_DeltaPosition;
			}
		}

		public float deltaAngle { get; private set; }

		public float speedPercent { get; private set; }

		public RaycastHit floorHit
		{
			get
			{
				return m_FloorHit;
			}
		}

		public float floorDistance { get; private set; }

		private void Awake()
		{
			m_Transform = base.transform;
			m_Input = GetComponent<SmartInputManager>();
			m_Controller = GetComponent<CharacterController>();
			nativeCapsuleHeight = m_Controller.height;
			nativeCapsuleCenter = m_Controller.center;
			nativeCapsuleCenter.y = nativeCapsuleHeight * 0.5f;
			m_Controller.center = nativeCapsuleCenter;
			m_FootstepSFXManager = GetComponent<FootstepSFXManager>();
			m_HeadBob = GetComponent<CameraHeadBob>();
		}

		private void Start()
		{
			cameraTransform = GetComponentInChildren<Camera>().transform;
			handTransform = hand.transform;
			handOffset = handTransform.localPosition;
			lookRotation.x = m_Transform.eulerAngles.y;
			lookRotation.y = cameraTransform.localEulerAngles.y;
			nativeRotation.eulerAngles = new Vector3(0f, lookRotation.y, 0f);
		}

		private void FixedUpdate()
		{
			UpdateStates();
		}

		private void Update()
		{
			CameraLook();
		}

		private void LateUpdate()
		{
			UpdateMoveAngle();
		}

		private void UpdateStates()
		{
			isGrounded = m_Controller.isGrounded;
			if (isGrounded)
			{
				if (isFalling)
				{
					isFalling = false;
					if (fallingDist > fallingDistanceToDamage)
					{
						int num = Mathf.RoundToInt(fallingDist * fallingDamageMultiplier);
						SendMessage(damageFunction, num, SendMessageOptions.DontRequireReceiver);
					}
					fallingDist = 0f;
				}
			}
			else if (isFalling)
			{
				fallingDist = fallingStartPos - m_Transform.position.y;
			}
			else if (!isClimbing)
			{
				isFalling = true;
				fallingStartPos = m_Transform.position.y;
			}
			Movement();
			PlayFootStepAudio();
			if (!isClimbing && !isGrounded && !isJumping && prevGrounded)
			{
				moveDirection.y = 0f;
			}
			prevGrounded = isGrounded;
		}

		private void UpdateMoveAngle()
		{
			m_DeltaPosition.x = Vector3.Dot(m_Transform.right, m_Velocity);
			m_DeltaPosition.y = Vector3.Dot(m_Transform.forward, m_Velocity);
			deltaAngle = Mathf.Atan2(m_DeltaPosition.x, m_DeltaPosition.y);
			deltaSin = ASKMath.SnapToZero(Mathf.Sin(deltaAngle));
			deltaCos = ASKMath.SnapToZero(Mathf.Cos(deltaAngle));
		}

		internal void Jump()
		{
			if (!isClimbing && !crouching && isGrounded)
			{
				if (isCrouched)
				{
					Crouch();
				}
				else if (canJump && canWalk && !jump && !isJumping)
				{
					jump = true;
				}
			}
		}

		internal void Crouch()
		{
			if (!canCrouch || isClimbing || crouching || !isGrounded)
			{
				return;
			}
			crouching = true;
			if (isCrouched)
			{
				if (Physics.SphereCast(m_Transform.position + Vector3.up * 0.75f, m_Controller.radius, Vector3.up, out m_FloorHit, nativeCapsuleHeight * 0.25f))
				{
					crouching = false;
				}
				else
				{
					StartCoroutine(StandUp());
				}
			}
			else
			{
				StartCoroutine(SitDown());
			}
		}

		private void Movement()
		{
			float x = m_Input.moveHorizontal * Time.timeScale;
			float num = m_Input.moveVertical * Time.timeScale;
			bool flag2 = (isMoveForward = num > -0.1f);
			bool flag3 = canRun && !isCrouched && flag2;
			isRunning = m_Input.runAction && flag3;
			float maxSpeed = GetMaxSpeed();
			Quaternion quaternion = Quaternion.Euler(0f, cameraTransform.eulerAngles.y, 0f);
			Vector3 vector = quaternion * new Vector3(0f, 0f, num);
			Vector3 vector2 = quaternion * new Vector3(x, 0f, 0f);
			Vector3 vector3 = vector + vector2;
			vector3 = vector3.normalized * Mathf.Clamp01(vector3.magnitude);
			if (isClimbing)
			{
				bool flag4 = cameraTransform.forward.y > -0.4f;
				if (flag2)
				{
					vector = currentLadder.m_Transform.up * ((!flag4) ? (0f - num) : num);
					vector3 = vector + vector2;
					if (isGrounded && !flag4)
					{
						vector3 += quaternion * Vector3.forward;
					}
					else if (!isGrounded && flag4)
					{
						vector3 += quaternion * Vector3.forward;
					}
				}
				moveDirection = vector3 * maxSpeed;
			}
			else if (isGrounded)
			{
				Physics.SphereCast(m_Transform.position + m_Controller.center, m_Controller.radius, Vector3.down, out m_FloorHit, m_Controller.height * 0.5f);
				moveDirection = vector3 * maxSpeed;
				moveDirection.y = -10f;
				if (jump)
				{
					m_FootstepSFXManager.PlayJumpingSound(m_FloorHit);
					isJumping = true;
					jump = false;
					moveDirection.y = jumpForce;
				}
				floorDistance = 0f;
			}
			else
			{
				moveDirection += Physics.gravity * gravityMultiplier * Time.fixedDeltaTime;
				if (Physics.Raycast(m_Transform.position, Vector3.down, out m_FloorHit))
				{
					floorDistance = ASKMath.SnapToZero(m_FloorHit.distance);
				}
			}
			if (canWalk)
			{
				collisionFlags = m_Controller.Move(moveDirection * Time.fixedDeltaTime);
			}
			m_Velocity = m_Controller.velocity;
			m_Velocity.y = ((!isClimbing) ? 0f : m_Velocity.y);
			velocitySize = m_Velocity.magnitude;
			speedPercent = velocitySize / maxSpeed;
			bool flag5 = velocitySize > 0.01f;
			isMoving = (isClimbing ? flag5 : (isGrounded && flag5));
		}

		private float GetMaxSpeed()
		{
			float num = ((!isRunning) ? walkSpeed : runSpeed);
			if (isClimbing)
			{
				num *= climbingSpeed;
			}
			else if (isCrouched)
			{
				num *= crouchSpeed;
			}
			else if (isFalling && !isJumping)
			{
				num *= inAirSpeed;
			}
			if (deltaCos < 0f)
			{
				num *= ASKMath.Persent01(backwardsSpeed, deltaCos);
			}
			return num * ASKMath.Persent01(sidewaysSpeed, deltaSin);
		}

		private void CameraLook()
		{
			float num = m_Input.lookHorizontal * Time.timeScale;
			float num2 = m_Input.lookVertical * Time.timeScale;
			lookRotation.x += num;
			lookRotation.y += num2;
			lookRotation.y = Mathf.Clamp(lookRotation.y, 0f - maxLookAngleY, maxLookAngleY);
			Quaternion b = nativeRotation * Quaternion.AngleAxis(lookRotation.y + ((!useHeadBob) ? 0f : (m_HeadBob.xTilt * tiltForce)), Vector3.left);
			Quaternion b2 = nativeRotation * Quaternion.AngleAxis(lookRotation.x + ((!useHeadBob) ? 0f : (m_HeadBob.yTilt * tiltForce)), Vector3.up);
			cameraTransform.localRotation = Quaternion.Slerp(cameraTransform.localRotation, b, lookSmooth);
			m_Transform.localRotation = Quaternion.Slerp(m_Transform.localRotation, b2, lookSmooth);
			cameraTransform.localPosition = new Vector3
			{
				x = m_Controller.center.x + cameraOffset.x + ((!useHeadBob) ? 0f : (m_HeadBob.xPos * posForce)),
				y = m_Controller.center.y * 2f + cameraOffset.y + ((!useHeadBob) ? 0f : (m_HeadBob.yPos * posForce)),
				z = m_Controller.center.z + cameraOffset.z
			};
			if (handTransform != null)
			{
				handTransform.localPosition = new Vector3
				{
					x = handOffset.x + ((!useHeadBob) ? 0f : (m_HeadBob.xPos * posForce)),
					y = handOffset.y + ((!useHeadBob) ? 0f : (m_HeadBob.yPos * posForce / 2f)),
					z = handOffset.z
				};
			}
		}

		private IEnumerator StandUp()
		{
			Vector3 targetCenter = nativeCapsuleCenter;
			isCrouched = false;
			while (PlayCrouchAnimation(targetCenter, nativeCapsuleHeight))
			{
				yield return null;
			}
			m_Controller.height = nativeCapsuleHeight;
			m_Controller.center = targetCenter;
			crouching = false;
		}

		private IEnumerator SitDown()
		{
			Vector3 targetCenter = nativeCapsuleCenter;
			targetCenter.y = crouchHeight * 0.5f;
			isCrouched = true;
			while (PlayCrouchAnimation(targetCenter, crouchHeight))
			{
				yield return null;
			}
			m_Controller.height = crouchHeight;
			m_Controller.center = targetCenter;
			crouching = false;
		}

		private bool PlayCrouchAnimation(Vector3 targetCenter, float targetHeight)
		{
			float smoothTime = Time.fixedDeltaTime * 5f;
			m_Controller.height = Mathf.SmoothDamp(m_Controller.height, targetHeight, ref crouchVel, smoothTime);
			m_Controller.center = Vector3.SmoothDamp(m_Controller.center, targetCenter, ref crouchVelVec, smoothTime);
			float num = ASKMath.Round(m_Controller.center.magnitude, 3);
			float num2 = ASKMath.Round(targetCenter.magnitude, 3);
			return num != num2;
		}

		private void PlayFootStepAudio()
		{
			if (!prevGrounded && isGrounded)
			{
				m_FootstepSFXManager.PlayLandingSound(m_FloorHit);
				nextStep = m_HeadBob.headBobCycle + stepInterval;
				isJumping = false;
				moveDirection.y = 0f;
			}
			else if (m_HeadBob.headBobCycle > nextStep)
			{
				nextStep = m_HeadBob.headBobCycle + stepInterval;
				if (isGrounded)
				{
					m_FootstepSFXManager.PlayFootStepSound(m_FloorHit);
				}
				else if (isClimbing)
				{
					currentLadder.PlayLadderFootstepSound();
				}
			}
		}

		private void OnControllerColliderHit(ControllerColliderHit hit)
		{
			PushObject(hit);
		}

		private void OnTriggerEnter(Collider collider)
		{
			OnLadderEnter(collider);
		}

		private void OnTriggerExit(Collider collider)
		{
			FromLadderExit();
		}

		private void PushObject(ControllerColliderHit hit)
		{
			if (collisionFlags != CollisionFlags.Below)
			{
				Rigidbody attachedRigidbody = hit.collider.attachedRigidbody;
				if (attachedRigidbody != null && !attachedRigidbody.isKinematic)
				{
					attachedRigidbody.AddForceAtPosition(hit.moveDirection * (m_Controller.velocity.magnitude * 1.25f / attachedRigidbody.mass), hit.point, ForceMode.VelocityChange);
				}
			}
		}

		private void OnLadderEnter(Collider collider)
		{
			if (!canClimb)
			{
				return;
			}
			currentLadder = collider.GetComponent<Ladder>();
			if (!(currentLadder == null))
			{
				if (isCrouched)
				{
					Crouch();
				}
				currentLadder.AssignAudioSource(GetComponent<AudioSource>());
				moveDirection = Vector3.zero;
				isClimbing = true;
				isFalling = false;
				fallingDist = 0f;
			}
		}

		private void FromLadderExit()
		{
			if (isClimbing)
			{
				isClimbing = false;
				currentLadder = null;
			}
		}

		internal void PlayerDie()
		{
			base.enabled = false;
			m_Controller.height = 0.1f;
			m_Controller.radius = 0.1f;
		}
	}
}
