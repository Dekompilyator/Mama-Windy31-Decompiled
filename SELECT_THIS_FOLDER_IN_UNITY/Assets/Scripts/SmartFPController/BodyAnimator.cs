using System;
using SmartFPController.Utils;
using UnityEngine;

namespace SmartFPController
{
	[RequireComponent(typeof(Animator))]
	public class BodyAnimator : MonoBehaviour
	{
		public bool isMoving;

		public bool isMovedForward;

		public bool isCrouched;

		public bool isClimbing;

		public bool isFalling;

		public float normalizedSpeed;

		public float radians;

		public float floorDistance;

		public float turn;

		private Transform m_Root;

		private Animator m_Animator;

		private FirstPersonController m_Controller;

		private float bodyYaw;

		private float prevBodyYaw;

		private int m_IsMovingHash;

		private int m_IsCrouchedHash;

		private int m_IsClimbingHash;

		private int m_IsFallingHash;

		private int m_RadiansHash;

		private int m_NormalizedSpeedHash;

		private int m_FloorDistanceHash;

		private int m_TurnHash;

		private void Start()
		{
			m_Root = base.transform.root;
			m_Controller = m_Root.GetComponent<FirstPersonController>();
			InitAnimator();
			InitHashIDs();
		}

		private void LateUpdate()
		{
			UpdateAnimationValues();
			UpdateRadiansAndSpeed();
			UpdateAnimator();
		}

		private void InitAnimator()
		{
			m_Animator = GetComponent<Animator>();
			m_Animator.applyRootMotion = false;
			m_Animator.cullingMode = AnimatorCullingMode.CullUpdateTransforms;
			m_Animator.updateMode = AnimatorUpdateMode.Normal;
			m_Animator.GetBoneTransform(HumanBodyBones.RightUpperArm).localScale = Vector3.zero;
			m_Animator.GetBoneTransform(HumanBodyBones.LeftUpperArm).localScale = Vector3.zero;
		}

		private void InitHashIDs()
		{
			m_IsMovingHash = Animator.StringToHash("IsMoving");
			m_IsCrouchedHash = Animator.StringToHash("IsCrouched");
			m_IsClimbingHash = Animator.StringToHash("IsClimbing");
			m_IsFallingHash = Animator.StringToHash("IsFalling");
			m_TurnHash = Animator.StringToHash("Turn");
			m_RadiansHash = Animator.StringToHash("Radians");
			m_NormalizedSpeedHash = Animator.StringToHash("NormalizedSpeed");
			m_FloorDistanceHash = Animator.StringToHash("FloorDistance");
		}

		private void UpdateAnimationValues()
		{
			isMoving = m_Controller.isMoving;
			isMovedForward = m_Controller.isMoveForward;
			isCrouched = m_Controller.isCrouched;
			isClimbing = m_Controller.isClimbing;
			isFalling = m_Controller.isFalling;
			floorDistance = m_Controller.floorDistance;
			if (isMoving)
			{
				prevBodyYaw = (bodyYaw = (turn = 0f));
				return;
			}
			prevBodyYaw = bodyYaw;
			bodyYaw = m_Root.eulerAngles.y;
			float b = Mathf.DeltaAngle(prevBodyYaw, bodyYaw);
			turn = Mathf.Lerp(turn, b, Time.smoothDeltaTime * 10f);
			turn = Mathf.Clamp(ASKMath.SnapToZero(turn, 0.01f), -2f, 2f);
		}

		private void UpdateRadiansAndSpeed()
		{
			float num = 0f;
			float t = Time.smoothDeltaTime * 10f;
			if (m_Controller.isGrounded && !m_Controller.isClimbing)
			{
				num = m_Controller.speedPercent;
				num = ((!m_Controller.isRunning) ? num : (num * 2f));
				num = ((!isMovedForward) ? (0f - num) : num);
				normalizedSpeed = Mathf.Lerp(normalizedSpeed, num, t);
			}
			if (num != 0f)
			{
				float num2 = m_Controller.deltaAngle;
				if (isMovedForward)
				{
					num2 = Mathf.Clamp(num2, -(float)Math.PI / 2f, (float)Math.PI / 2f);
				}
				else
				{
					if (num2 > (float)Math.PI / 2f)
					{
						num2 -= (float)Math.PI;
					}
					else if (num2 < -(float)Math.PI / 2f)
					{
						num2 += (float)Math.PI;
					}
					num2 = 0f - num2;
				}
				radians = Mathf.Lerp(radians, num2, t);
			}
			else
			{
				radians = Mathf.Lerp(radians, 0f, t);
				normalizedSpeed = Mathf.Lerp(normalizedSpeed, 0f, t);
			}
			radians = ASKMath.SnapToZero(radians);
			normalizedSpeed = ASKMath.SnapToZero(normalizedSpeed);
		}

		private void UpdateAnimator()
		{
			m_Animator.SetBool(m_IsMovingHash, isMoving);
			m_Animator.SetBool(m_IsCrouchedHash, isCrouched);
			m_Animator.SetBool(m_IsClimbingHash, isClimbing);
			m_Animator.SetBool(m_IsFallingHash, isFalling);
			m_Animator.SetFloat(m_TurnHash, turn);
			m_Animator.SetFloat(m_RadiansHash, radians);
			m_Animator.SetFloat(m_FloorDistanceHash, floorDistance);
			m_Animator.SetFloat(m_NormalizedSpeedHash, normalizedSpeed);
		}
	}
}
