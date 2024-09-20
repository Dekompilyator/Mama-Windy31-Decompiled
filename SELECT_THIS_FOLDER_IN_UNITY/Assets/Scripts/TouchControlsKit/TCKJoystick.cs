using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TouchControlsKit
{
	public class TCKJoystick : AxesBasedController, IPointerUpHandler, IPointerDownHandler, IDragHandler, IEventSystemHandler
	{
		public Image joystickImage;

		public Image backgroundImage;

		public RectTransform joystickRT;

		public RectTransform backgroundRT;

		[SerializeField]
		[Label("Mode")]
		private bool isStatic = true;

		[Range(1f, 9f)]
		public float borderSize = 5.85f;

		[LogicLabel("isStatic", "Smooth Return", "Fadeout")]
		public bool smoothReturn;

		[Range(1f, 20f)]
		public float smoothFactor = 7f;

		[SerializeField]
		private Color32 joystickImageColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 165);

		[SerializeField]
		private Color32 backgroundImageColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 165);

		public bool IsStatic
		{
			get
			{
				return isStatic;
			}
			set
			{
				if (isStatic != value)
				{
					isStatic = value;
				}
			}
		}

		public override void OnAwake()
		{
			backgroundRT = base.transform.GetChild(0) as RectTransform;
			joystickRT = backgroundRT.GetChild(0) as RectTransform;
			backgroundImage = backgroundRT.GetComponent<Image>();
			joystickImage = joystickRT.GetComponent<Image>();
			if (Application.isPlaying)
			{
				Image image = joystickImage;
				bool flag = isStatic;
				backgroundImage.enabled = flag;
				image.enabled = flag;
			}
			base.OnAwake();
		}

		protected override void OnApplyEnable()
		{
			base.OnApplyEnable();
			Image image = backgroundImage;
			bool flag = enable;
			joystickImage.enabled = flag;
			image.enabled = flag;
		}

		protected override void OnApplyActiveColors()
		{
			base.OnApplyActiveColors();
			joystickImage.color = GetActiveColor(joystickImageColor);
			backgroundImage.color = GetActiveColor(backgroundImageColor);
		}

		protected override void OnApplyVisible()
		{
			base.OnApplyVisible();
			joystickImage.color = ((!visible) ? Color.clear : GetActiveColor(joystickImageColor));
			backgroundImage.color = ((!visible) ? Color.clear : GetActiveColor(backgroundImageColor));
		}

		protected override void UpdatePosition(Vector2 touchPos)
		{
			if (!axisX.enabled && !axisY.enabled)
			{
				return;
			}
			base.UpdatePosition(touchPos);
			if (touchDown)
			{
				UpdateCurrentPosition(touchPos);
				currentDirection = currentPosition - defaultPosition;
				float num = Vector2.Distance(defaultPosition, currentPosition);
				float num2 = 100f;
				float num3 = backgroundRT.sizeDelta.magnitude / 2f * borderSize / 16f;
				if (num > num3)
				{
					currentPosition = defaultPosition + currentDirection.normalized * num3;
				}
				else
				{
					num2 = num / num3 * 100f;
				}
				UpdateJoystickPosition();
				SetAxes(currentDirection.normalized * num2 / 100f * sensitivity);
			}
			else
			{
				touchDown = true;
				touchPhase = ETouchPhase.Began;
				if (!isStatic)
				{
					UpdateTransparencyAndPosition(touchPos);
				}
				UpdateCurrentPosition(touchPos);
				UpdatePosition(touchPos);
				ResetAxes();
			}
		}

		private void UpdateCurrentPosition(Vector2 touchPos)
		{
			defaultPosition = (currentPosition = backgroundRT.position);
			Vector2 vector = GuiCamera.ScreenToWorldPoint(touchPos);
			if (axisX.enabled)
			{
				currentPosition.x = vector.x;
			}
			if (axisY.enabled)
			{
				currentPosition.y = vector.y;
			}
		}

		private void UpdateJoystickPosition()
		{
			joystickRT.position = currentPosition;
		}

		private void UpdateTransparencyAndPosition(Vector2 touchPos)
		{
			OnApplyVisible();
			Image image = joystickImage;
			bool flag = true;
			backgroundImage.enabled = flag;
			image.enabled = flag;
			RectTransform rectTransform = joystickRT;
			Vector3 position = GuiCamera.ScreenToWorldPoint(touchPos);
			backgroundRT.position = position;
			rectTransform.position = position;
		}

		private IEnumerator SmoothReturnRun()
		{
			bool smoothReturnIsRun = true;
			int defPosMagnitude = Mathf.RoundToInt(defaultPosition.sqrMagnitude);
			while (smoothReturnIsRun && !touchDown)
			{
				float smoothTime = smoothFactor * Time.smoothDeltaTime;
				currentPosition = Vector2.Lerp(currentPosition, defaultPosition, smoothTime);
				if (!isStatic)
				{
					joystickImage.color = Color.Lerp(joystickImage.color, Color.clear, smoothTime);
					backgroundImage.color = Color.Lerp(backgroundImage.color, Color.clear, smoothTime);
				}
				if (Mathf.RoundToInt(currentPosition.sqrMagnitude) == defPosMagnitude)
				{
					currentPosition = defaultPosition;
					smoothReturnIsRun = false;
					if (!isStatic)
					{
						Image image = joystickImage;
						bool flag = false;
						backgroundImage.enabled = flag;
						image.enabled = flag;
					}
				}
				UpdateJoystickPosition();
				yield return null;
			}
		}

		protected override void ControlReset()
		{
			base.ControlReset();
			if (smoothReturn)
			{
				StopCoroutine("SmoothReturnRun");
				StartCoroutine("SmoothReturnRun");
				return;
			}
			Image image = joystickImage;
			bool flag = isStatic;
			backgroundImage.enabled = flag;
			image.enabled = flag;
			currentPosition = defaultPosition;
			UpdateJoystickPosition();
		}

		public void OnPointerDown(PointerEventData pointerData)
		{
			if (!touchDown)
			{
				touchId = pointerData.pointerId;
				UpdatePosition(pointerData.position);
			}
		}

		public void OnDrag(PointerEventData pointerData)
		{
			if (Input.touchCount >= touchId && touchDown)
			{
				UpdatePosition(pointerData.position);
			}
		}

		public void OnPointerUp(PointerEventData pointerData)
		{
			UpdatePosition(pointerData.position);
			ControlReset();
		}
	}
}
