using UnityEngine;
using UnityEngine.EventSystems;

namespace TouchControlsKit
{
	public class TCKButton : ControllerBase, IPointerExitHandler, IPointerDownHandler, IDragHandler, IPointerUpHandler, IPointerClickHandler, IEventSystemHandler
	{
		public bool swipeOut;

		[Label("Normal Button")]
		public Sprite normalSprite;

		[Label("Pressed Button")]
		public Sprite pressedSprite;

		public Color32 pressedColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 165);

		private int pressedFrame = -1;

		private int releasedFrame = -1;

		private int clickedFrame = -1;

		internal bool isPRESSED
		{
			get
			{
				return touchDown;
			}
		}

		internal bool isDOWN
		{
			get
			{
				return pressedFrame == Time.frameCount - 1;
			}
		}

		internal bool isUP
		{
			get
			{
				return releasedFrame == Time.frameCount - 1;
			}
		}

		internal bool isCLICK
		{
			get
			{
				return clickedFrame == Time.frameCount - 1;
			}
		}

		protected override void UpdatePosition(Vector2 touchPos)
		{
			base.UpdatePosition(touchPos);
			if (!touchDown)
			{
				touchDown = true;
				touchPhase = ETouchPhase.Began;
				pressedFrame = Time.frameCount;
				ButtonDown();
			}
		}

		protected void ButtonDown()
		{
			baseImage.sprite = pressedSprite;
			baseImage.color = ((!visible) ? ((Color32)Color.clear) : pressedColor);
		}

		protected void ButtonUp()
		{
			baseImage.sprite = normalSprite;
			baseImage.color = ((!visible) ? ((Color32)Color.clear) : baseImageColor);
		}

		protected override void ControlReset()
		{
			base.ControlReset();
			releasedFrame = Time.frameCount;
			ButtonUp();
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

		public void OnPointerExit(PointerEventData pointerData)
		{
			if (!swipeOut)
			{
				OnPointerUp(pointerData);
			}
		}

		public void OnPointerUp(PointerEventData pointerData)
		{
			ControlReset();
		}

		public void OnPointerClick(PointerEventData pointerData)
		{
			clickedFrame = Time.frameCount;
		}
	}
}
