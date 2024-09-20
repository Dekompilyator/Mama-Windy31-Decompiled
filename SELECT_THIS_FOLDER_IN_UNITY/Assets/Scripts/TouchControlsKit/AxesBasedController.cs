using System;
using System.Collections;
using UnityEngine;

namespace TouchControlsKit
{
	public abstract class AxesBasedController : ControllerBase
	{
		[Range(1f, 10f)]
		public float sensitivity = 1f;

		[Label("Lag")]
		public bool axesLag;

		[Range(5f, 25f)]
		public float axesLagSpeed = 10f;

		[Label("X - [ Horizontal ]")]
		public Axis axisX = new Axis();

		[Label("Y - [ Vertical ]")]
		public Axis axisY = new Axis();

		[SerializeField]
		private bool showTouchZone = true;

		protected Vector2 defaultPosition;

		protected Vector2 currentPosition;

		protected Vector2 currentDirection;

		public bool ShowTouchZone
		{
			get
			{
				return showTouchZone;
			}
			set
			{
				if (showTouchZone != value)
				{
					showTouchZone = value;
					OnApplyShowTouchZone();
				}
			}
		}

		[ApplyMethod]
		protected virtual void OnApplyShowTouchZone()
		{
			baseImage.color = ((!showTouchZone || !visible) ? Color.clear : GetActiveColor(baseImageColor));
		}

		protected override void OnApplyActiveColors()
		{
			if (showTouchZone)
			{
				base.OnApplyActiveColors();
			}
		}

		protected override void OnApplyVisible()
		{
			OnApplyShowTouchZone();
		}

		protected void ResetAxes()
		{
			SetAxes(0f, 0f);
		}

		protected void SetAxes(Vector2 axes)
		{
			SetAxes(axes.x, axes.y);
		}

		protected void SetAxes(float x, float y)
		{
			x = ((!axisX.inverse) ? x : (0f - x));
			y = ((!axisY.inverse) ? y : (0f - y));
			if (axesLag)
			{
				if (axisX.enabled)
				{
					StopCoroutine("SmoothAxisX");
					StartCoroutine("SmoothAxisX", x);
				}
				else
				{
					axisX.SetValue(0f);
				}
				if (axisY.enabled)
				{
					StopCoroutine("SmoothAxisY");
					StartCoroutine("SmoothAxisY", y);
				}
				else
				{
					axisY.SetValue(0f);
				}
			}
			else
			{
				axisX.SetValue(x);
				axisY.SetValue(y);
			}
		}

		private IEnumerator SmoothAxisX(float targetValue)
		{
			while (Math.Round(axisX.value, 2) != Math.Round(targetValue, 2))
			{
				axisX.SetValue(Mathf.Lerp(axisX.value, targetValue, Time.smoothDeltaTime * axesLagSpeed));
				yield return null;
			}
			axisX.SetValue(targetValue);
		}

		private IEnumerator SmoothAxisY(float targetValue)
		{
			while (Math.Round(axisY.value, 2) != Math.Round(targetValue, 2))
			{
				axisY.SetValue(Mathf.Lerp(axisY.value, targetValue, Time.smoothDeltaTime * axesLagSpeed));
				yield return null;
			}
			axisY.SetValue(targetValue);
		}

		protected override void ControlReset()
		{
			base.ControlReset();
			ResetAxes();
		}
	}
}
