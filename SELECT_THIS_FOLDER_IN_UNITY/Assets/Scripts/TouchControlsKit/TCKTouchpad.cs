using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TouchControlsKit
{
	public class TCKTouchpad : AxesBasedController, IPointerUpHandler, IPointerDownHandler, IDragHandler, IPointerEnterHandler, IEventSystemHandler
	{
		private GameObject prevPointerPressGO;

		protected override void OnApplyVisible()
		{
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
				if (axisX.enabled)
				{
					currentPosition.x = touchPos.x;
				}
				if (axisY.enabled)
				{
					currentPosition.y = touchPos.y;
				}
				currentDirection = currentPosition - defaultPosition;
				float num = Vector2.Distance(defaultPosition, currentPosition) * 2f;
				defaultPosition = currentPosition;
				SetAxes(currentDirection.normalized * num / 100f * sensitivity);
			}
			else
			{
				touchDown = true;
				touchPhase = ETouchPhase.Began;
				currentPosition = (defaultPosition = touchPos);
				UpdatePosition(touchPos);
				ResetAxes();
			}
		}

		public void OnPointerEnter(PointerEventData pointerData)
		{
			if (pointerData.pointerPress == null)
			{
				return;
			}
			if (pointerData.pointerPress == base.gameObject)
			{
				OnPointerDown(pointerData);
				return;
			}
			TCKButton component = pointerData.pointerPress.GetComponent<TCKButton>();
			if (component != null && component.swipeOut)
			{
				prevPointerPressGO = pointerData.pointerPress;
				pointerData.pointerDrag = base.gameObject;
				pointerData.pointerPress = base.gameObject;
				OnPointerDown(pointerData);
			}
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
				StopCoroutine("UpdateEndPosition");
				StartCoroutine("UpdateEndPosition", pointerData.position);
			}
		}

		private IEnumerator UpdateEndPosition(Vector2 position)
		{
			for (float el = 0f; el < 0.0025f; el += Time.deltaTime)
			{
				yield return null;
			}
			if (touchDown)
			{
				UpdatePosition(position);
			}
			else
			{
				ControlReset();
			}
		}

		public void OnPointerUp(PointerEventData pointerData)
		{
			if (prevPointerPressGO != null)
			{
				ExecuteEvents.Execute(prevPointerPressGO, pointerData, ExecuteEvents.pointerUpHandler);
				prevPointerPressGO = null;
			}
			ControlReset();
		}
	}
}
