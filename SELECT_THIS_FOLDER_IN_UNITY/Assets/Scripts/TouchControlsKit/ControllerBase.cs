using UnityEngine;
using UnityEngine.UI;

namespace TouchControlsKit
{
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Image))]
	public abstract class ControllerBase : MonoBehaviour
	{
		public EUpdateMode updateMode;

		internal ETouchPhase touchPhase = ETouchPhase.NoTouch;

		public RectTransform baseRect;

		public Image baseImage;

		public string identifier = "NONAME_Controller";

		[SerializeField]
		protected Color32 baseImageColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 165);

		protected int touchId = -1;

		protected bool touchDown;

		[SerializeField]
		protected bool enable = true;

		[SerializeField]
		protected bool active = true;

		[SerializeField]
		protected bool visible = true;

		private float touchPosMag;

		private float prevTouchPosMag;

		public bool isEnable
		{
			get
			{
				return enable;
			}
			set
			{
				if (enable != value)
				{
					enable = value;
					OnApplyEnable();
				}
			}
		}

		public bool isActive
		{
			get
			{
				return active;
			}
			set
			{
				if (active != value)
				{
					active = value;
					OnApplyActive();
				}
			}
		}

		public bool isVisible
		{
			get
			{
				return visible;
			}
			set
			{
				if (visible != value)
				{
					visible = value;
					OnApplyVisible();
				}
			}
		}

		protected Color GetActiveColor(Color color)
		{
			return (!active) ? (color * 0.5f) : color;
		}

		[ApplyMethod]
		protected virtual void OnApplyEnable()
		{
			base.enabled = enable && active;
			baseImage.enabled = enable;
		}

		[ApplyMethod]
		protected virtual void OnApplyActive()
		{
			base.enabled = enable && active;
			if (visible)
			{
				OnApplyActiveColors();
			}
		}

		protected virtual void OnApplyActiveColors()
		{
			baseImage.color = GetActiveColor(baseImageColor);
		}

		[ApplyMethod]
		protected virtual void OnApplyVisible()
		{
			baseImage.color = ((!visible) ? Color.clear : GetActiveColor(baseImageColor));
		}

		public virtual void OnAwake()
		{
			baseImage = GetComponent<Image>();
			baseRect = baseImage.rectTransform;
			OnApplyActive();
		}

		protected virtual void Update()
		{
			if (updateMode == EUpdateMode.Normal)
			{
				UpdateTouchPhase();
			}
		}

		protected virtual void LateUpdate()
		{
			if (updateMode == EUpdateMode.Late)
			{
				UpdateTouchPhase();
			}
		}

		protected virtual void FixedUpdate()
		{
			if (updateMode == EUpdateMode.Fixed)
			{
				UpdateTouchPhase();
			}
		}

		private void OnDisable()
		{
			if (Application.isPlaying && touchDown)
			{
				ControlReset();
			}
		}

		private void UpdateTouchPhase()
		{
			if (touchDown)
			{
				if (touchPosMag == prevTouchPosMag)
				{
					touchPhase = ETouchPhase.Stationary;
				}
				else
				{
					touchPhase = ETouchPhase.Moved;
				}
			}
			else
			{
				touchPhase = ETouchPhase.NoTouch;
			}
			prevTouchPosMag = touchPosMag;
		}

		protected virtual void UpdatePosition(Vector2 touchPos)
		{
			touchPosMag = touchPos.magnitude;
		}

		protected virtual void ControlReset()
		{
			touchPhase = ETouchPhase.Ended;
			touchId = -1;
			touchDown = false;
		}
	}
}
