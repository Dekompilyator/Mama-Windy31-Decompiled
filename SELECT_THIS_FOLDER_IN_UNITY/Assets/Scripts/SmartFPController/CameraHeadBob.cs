using System;
using SmartFPController.Utils;
using UnityEngine;

namespace SmartFPController
{
	public class CameraHeadBob : MonoBehaviour
	{
		[SerializeField]
		[Range(1f, 3f)]
		private float headBobFrequency = 1.5f;

		[SerializeField]
		[Range(0.1f, 2f)]
		private float headBobHeight = 0.35f;

		[SerializeField]
		[Range(0.1f, 2f)]
		private float headBobSwayAngle = 0.5f;

		[SerializeField]
		[Range(0.01f, 0.1f)]
		private float headBobSideMovement = 0.075f;

		[SerializeField]
		[Range(0.1f, 2f)]
		private float bobHeightSpeedMultiplier = 0.35f;

		[SerializeField]
		[Range(0.1f, 2f)]
		private float bobStrideSpeedLengthen = 0.35f;

		[SerializeField]
		[Range(0.1f, 5f)]
		private float jumpLandMove = 2f;

		[SerializeField]
		[Range(10f, 100f)]
		private float jumpLandTilt = 35f;

		[SerializeField]
		[Range(0.1f, 4f)]
		private float springElastic = 1.25f;

		[SerializeField]
		[Range(0.1f, 2f)]
		private float springDampen = 0.77f;

		private float springPos;

		private float springVelocity;

		private float headBobFade;

		private Vector3 prevVelocity;

		private Vector3 prevPosition;

		private FirstPersonController m_Controller;

		private Transform m_Transform;

		public float headBobCycle { get; private set; }

		public float xPos { get; private set; }

		public float yPos { get; private set; }

		public float xTilt { get; private set; }

		public float yTilt { get; private set; }

		private void Awake()
		{
			m_Transform = base.transform;
			m_Controller = GetComponent<FirstPersonController>();
		}

		private void FixedUpdate()
		{
			UpdateValues(Time.fixedDeltaTime);
		}

		private void UpdateValues(float deltaTime)
		{
			Vector3 vector = (m_Transform.position - prevPosition) / deltaTime;
			Vector3 vector2 = vector - prevVelocity;
			prevPosition = m_Transform.position;
			prevVelocity = vector;
			if (!m_Controller.isClimbing)
			{
				vector.y = 0f;
			}
			springVelocity -= vector2.y;
			springVelocity -= springPos * springElastic;
			springVelocity *= springDampen;
			springPos += springVelocity * deltaTime;
			springPos = Mathf.Clamp(springPos, -0.32f, 0.32f);
			if (Mathf.Abs(springVelocity) < 0.05f && Mathf.Abs(springPos) < 0.05f)
			{
				springVelocity = (springPos = 0f);
			}
			float magnitude = vector.magnitude;
			float num = magnitude;
			if (m_Controller.isClimbing)
			{
				num *= 4f;
			}
			else if (!m_Controller.isClimbing && !m_Controller.isGrounded)
			{
				num /= 4f;
			}
			float num2 = 1f + num * bobStrideSpeedLengthen;
			headBobCycle += num / num2 * (deltaTime / headBobFrequency);
			float num3 = headBobCycle * ((float)Math.PI * 2f);
			float num4 = Mathf.Sin(num3);
			float num5 = Mathf.Sin(num3 + (float)Math.PI / 2f);
			num4 = 1f - (num4 * 0.5f + 1f);
			num4 *= num4;
			headBobFade = Mathf.Lerp(headBobFade, (!(magnitude < 0.1f)) ? 1f : 0f, deltaTime);
			headBobFade = ASKMath.SnapToZero(headBobFade);
			float num6 = 1f + num * bobHeightSpeedMultiplier;
			xPos = (0f - headBobSideMovement) * num5 * headBobFade;
			yPos = springPos * jumpLandMove + num4 * headBobHeight * num6;
			xTilt = springPos * jumpLandTilt;
			yTilt = num5 * headBobSwayAngle * headBobFade;
		}
	}
}
