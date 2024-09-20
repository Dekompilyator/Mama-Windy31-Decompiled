using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TouchControlsKit
{
	public sealed class TCKInput : MonoBehaviour
	{
		private static TCKInput m_Instance;

		private static bool m_Active;

		private static ControllerBase[] m_AllControllers;

		private static AxesBasedController[] m_AbControllers;

		private static TCKButton[] m_Buttons;

		private static bool m_Inited;

		private static TCKInput instance
		{
			get
			{
				if (m_Instance == null)
				{
					m_Instance = UnityEngine.Object.FindObjectOfType<TCKInput>();
				}
				return m_Instance;
			}
		}

		public static bool isActive
		{
			get
			{
				return m_Active;
			}
		}

		private static ControllerBase[] allControllers
		{
			get
			{
				if (!m_Inited)
				{
					InitControllers();
				}
				return m_AllControllers;
			}
		}

		private static AxesBasedController[] abControllers
		{
			get
			{
				if (!m_Inited)
				{
					InitControllers();
				}
				return m_AbControllers;
			}
		}

		private static TCKButton[] buttons
		{
			get
			{
				if (!m_Inited)
				{
					InitControllers();
				}
				return m_Buttons;
			}
		}

		public static void CheckUIEventSystem()
		{
			if (!(UnityEngine.Object.FindObjectOfType<EventSystem>() != null))
			{
				Type typeFromHandle = typeof(EventSystem);
				new GameObject(typeFromHandle.Name, typeFromHandle, typeof(StandaloneInputModule));
			}
		}

		private void Awake()
		{
			CheckUIEventSystem();
			m_Instance = this;
			SetActive(true);
			InitControllers();
			Array.ForEach(m_AllControllers, delegate(ControllerBase c)
			{
				c.OnAwake();
			});
		}

		private void OnDisable()
		{
			OnReset();
		}

		private void OnDestroy()
		{
			OnReset();
		}

		private static void OnReset()
		{
			m_Inited = false;
			m_Instance = null;
		}

		private static void InitControllers()
		{
			m_Inited = true;
			m_AllControllers = instance.GetComponentsInChildren<ControllerBase>(true);
			m_AbControllers = GetControllers<AxesBasedController>();
			m_Buttons = GetControllers<TCKButton>();
		}

		private static T[] GetControllers<T>() where T : ControllerBase
		{
			return allControllers.Where((ControllerBase obj) => obj is T).Cast<T>().ToArray();
		}

		public static void SetActive(bool value)
		{
			m_Active = value;
			instance.enabled = value;
			instance.gameObject.SetActive(value);
		}

		public static bool CheckController(string controllerName)
		{
			if (!m_Active)
			{
				return false;
			}
			ControllerBase[] array = allControllers;
			foreach (ControllerBase controllerBase in array)
			{
				if (controllerBase.identifier == controllerName)
				{
					return true;
				}
			}
			return false;
		}

		public static bool GetControllerEnable(string controllerName)
		{
			if (!m_Active)
			{
				return false;
			}
			ControllerBase[] array = allControllers;
			foreach (ControllerBase controllerBase in array)
			{
				if (controllerBase.identifier == controllerName)
				{
					return controllerBase.isEnable;
				}
			}
			Debug.LogError("Controller: " + controllerName + " not found!");
			return false;
		}

		public static void SetControllerEnable(string controllerName, bool value)
		{
			if (!m_Active)
			{
				return;
			}
			ControllerBase[] array = allControllers;
			foreach (ControllerBase controllerBase in array)
			{
				if (controllerBase.identifier == controllerName)
				{
					controllerBase.isEnable = value;
					return;
				}
			}
			Debug.LogError("Controller: " + controllerName + " not found!");
		}

		public static bool GetControllerActive(string controllerName)
		{
			if (!m_Active)
			{
				return false;
			}
			ControllerBase[] array = allControllers;
			foreach (ControllerBase controllerBase in array)
			{
				if (controllerBase.identifier == controllerName)
				{
					return controllerBase.isActive;
				}
			}
			Debug.LogError("Controller: " + controllerName + " not found!");
			return false;
		}

		public static void SetControllerActive(string controllerName, bool value)
		{
			if (!m_Active)
			{
				return;
			}
			ControllerBase[] array = allControllers;
			foreach (ControllerBase controllerBase in array)
			{
				if (controllerBase.identifier == controllerName)
				{
					controllerBase.isActive = value;
					return;
				}
			}
			Debug.LogError("Controller: " + controllerName + " not found!");
		}

		public static bool GetControllerVisible(string controllerName)
		{
			if (!m_Active)
			{
				return false;
			}
			ControllerBase[] array = allControllers;
			foreach (ControllerBase controllerBase in array)
			{
				if (controllerBase.identifier == controllerName)
				{
					return controllerBase.isVisible;
				}
			}
			Debug.LogError("Controller: " + controllerName + " not found!");
			return false;
		}

		public static void SetControllerVisible(string controllerName, bool value)
		{
			if (!m_Active)
			{
				return;
			}
			ControllerBase[] array = allControllers;
			foreach (ControllerBase controllerBase in array)
			{
				if (controllerBase.identifier == controllerName)
				{
					controllerBase.isVisible = value;
					return;
				}
			}
			Debug.LogError("Controller: " + controllerName + " not found!");
		}

		public static float GetAxis(string controllerName, EAxisType axisType)
		{
			if (!m_Active)
			{
				return 0f;
			}
			for (int i = 0; i < abControllers.Length; i++)
			{
				if (abControllers[i].identifier == controllerName)
				{
					return (axisType != 0) ? abControllers[i].axisY.value : abControllers[i].axisX.value;
				}
			}
			Debug.LogError("Controller: " + controllerName + " not found!");
			return 0f;
		}

		public static Vector2 GetAxis(string controllerName)
		{
			for (int i = 0; i < abControllers.Length; i++)
			{
				if (abControllers[i].identifier == controllerName)
				{
					return new Vector2(abControllers[i].axisX.value, abControllers[i].axisY.value);
				}
			}
			Debug.LogError("Controller: " + controllerName + " not found!");
			return Vector2.zero;
		}

		public static bool GetAxisEnable(string controllerName, EAxisType axisType)
		{
			if (!m_Active)
			{
				return false;
			}
			AxesBasedController[] array = abControllers;
			foreach (AxesBasedController axesBasedController in array)
			{
				if (axesBasedController.identifier == controllerName)
				{
					return (axisType != 0) ? axesBasedController.axisY.enabled : axesBasedController.axisX.enabled;
				}
			}
			Debug.LogError("Controller: " + controllerName + " not found!");
			return false;
		}

		public static void SetAxisEnable(string controllerName, EAxisType axisType, bool value)
		{
			if (!m_Active)
			{
				return;
			}
			AxesBasedController[] array = abControllers;
			foreach (AxesBasedController axesBasedController in array)
			{
				if (axesBasedController.identifier == controllerName)
				{
					switch (axisType)
					{
					case EAxisType.Horizontal:
						axesBasedController.axisX.enabled = value;
						break;
					case EAxisType.Vertical:
						axesBasedController.axisY.enabled = value;
						break;
					default:
						Debug.LogError(string.Concat("Axis: ", axisType, " not found!"));
						break;
					}
					return;
				}
			}
			Debug.LogError("Controller: " + controllerName + " not found!");
		}

		public static bool GetAxisInverse(string controllerName, EAxisType axisType)
		{
			if (!m_Active)
			{
				return false;
			}
			AxesBasedController[] array = abControllers;
			foreach (AxesBasedController axesBasedController in array)
			{
				if (axesBasedController.identifier == controllerName)
				{
					return (axisType != 0) ? axesBasedController.axisY.inverse : axesBasedController.axisX.inverse;
				}
			}
			Debug.LogError("Controller: " + controllerName + " not found!");
			return false;
		}

		public static void SetAxisInverse(string controllerName, EAxisType axisType, bool value)
		{
			if (!m_Active)
			{
				return;
			}
			AxesBasedController[] array = abControllers;
			foreach (AxesBasedController axesBasedController in array)
			{
				if (axesBasedController.identifier == controllerName)
				{
					switch (axisType)
					{
					case EAxisType.Horizontal:
						axesBasedController.axisX.inverse = value;
						break;
					case EAxisType.Vertical:
						axesBasedController.axisY.inverse = value;
						break;
					default:
						Debug.LogError(string.Concat("Axis: ", axisType, " not found!"));
						break;
					}
					return;
				}
			}
			Debug.LogError("Controller: " + controllerName + " not found!");
		}

		public static float GetSensitivity(string controllerName)
		{
			if (!m_Active)
			{
				return 0f;
			}
			AxesBasedController[] array = abControllers;
			foreach (AxesBasedController axesBasedController in array)
			{
				if (axesBasedController.identifier == controllerName)
				{
					return axesBasedController.sensitivity;
				}
			}
			Debug.LogError("Controller: " + controllerName + " not found!");
			return 0f;
		}

		public static void SetSensitivity(string controllerName, float value)
		{
			if (!m_Active)
			{
				return;
			}
			AxesBasedController[] array = abControllers;
			foreach (AxesBasedController axesBasedController in array)
			{
				if (axesBasedController.identifier == controllerName)
				{
					axesBasedController.sensitivity = value;
					return;
				}
			}
			Debug.LogError("Controller: " + controllerName + " not found!");
		}

		public static void ShowingTouchZone(bool value)
		{
			if (m_Active)
			{
				Array.ForEach(abControllers, delegate(AxesBasedController c)
				{
					c.ShowTouchZone = value;
				});
			}
		}

		public static bool GetAction(string buttonName, EActionEvent m_Event)
		{
			if (!m_Active)
			{
				return false;
			}
			for (int i = 0; i < buttons.Length; i++)
			{
				if (buttons[i].identifier == buttonName)
				{
					switch (m_Event)
					{
					case EActionEvent.Down:
						return buttons[i].isDOWN;
					case EActionEvent.Press:
						return buttons[i].isPRESSED;
					case EActionEvent.Up:
						return buttons[i].isUP;
					case EActionEvent.Click:
						return buttons[i].isCLICK;
					}
				}
			}
			Debug.LogError("Button: " + buttonName + " not found!");
			return false;
		}

		[Obsolete("Please use 'GetAction' instead.")]
		public static bool GetButtonDown(string buttonName)
		{
			if (!m_Active)
			{
				return false;
			}
			TCKButton[] array = buttons;
			foreach (TCKButton tCKButton in array)
			{
				if (tCKButton.identifier == buttonName)
				{
					return tCKButton.isDOWN;
				}
			}
			Debug.LogError("Button: " + buttonName + " not found!");
			return false;
		}

		[Obsolete("Please use 'GetAction' instead.")]
		public static bool GetButton(string buttonName)
		{
			if (!m_Active)
			{
				return false;
			}
			TCKButton[] array = buttons;
			foreach (TCKButton tCKButton in array)
			{
				if (tCKButton.identifier == buttonName)
				{
					return tCKButton.isPRESSED;
				}
			}
			Debug.LogError("Button: " + buttonName + " not found!");
			return false;
		}

		[Obsolete("Please use 'GetAction' instead.")]
		public static bool GetButtonUp(string buttonName)
		{
			if (!m_Active)
			{
				return false;
			}
			TCKButton[] array = buttons;
			foreach (TCKButton tCKButton in array)
			{
				if (tCKButton.identifier == buttonName)
				{
					return tCKButton.isUP;
				}
			}
			Debug.LogError("Button: " + buttonName + " not found!");
			return false;
		}

		[Obsolete("Please use 'GetAction' instead.")]
		public static bool GetButtonClick(string buttonName)
		{
			if (!m_Active)
			{
				return false;
			}
			TCKButton[] array = buttons;
			foreach (TCKButton tCKButton in array)
			{
				if (tCKButton.identifier == buttonName)
				{
					return tCKButton.isCLICK;
				}
			}
			Debug.LogError("Button: " + buttonName + " not found!");
			return false;
		}

		public static EUpdateMode GetEUpdateType(string controllerName)
		{
			if (!m_Active)
			{
				return EUpdateMode.OFF;
			}
			ControllerBase[] array = allControllers;
			foreach (ControllerBase controllerBase in array)
			{
				if (controllerBase.identifier == controllerName)
				{
					return controllerBase.updateMode;
				}
			}
			Debug.LogError("Controller: " + controllerName + " not found!");
			return EUpdateMode.OFF;
		}

		public static void SetEUpdateType(string controllerName, EUpdateMode EUpdateType)
		{
			if (!m_Active)
			{
				return;
			}
			ControllerBase[] array = allControllers;
			foreach (ControllerBase controllerBase in array)
			{
				if (controllerBase.identifier == controllerName)
				{
					controllerBase.updateMode = EUpdateType;
				}
			}
			Debug.LogError("Controller: " + controllerName + " not found!");
		}

		public static ETouchPhase GetTouchPhase(string controllerName)
		{
			if (!m_Active)
			{
				return ETouchPhase.NoTouch;
			}
			ControllerBase[] array = allControllers;
			foreach (ControllerBase controllerBase in array)
			{
				if (controllerBase.identifier == controllerName)
				{
					return controllerBase.touchPhase;
				}
			}
			Debug.LogError("Controller: " + controllerName + " not found!");
			return ETouchPhase.NoTouch;
		}
	}
}
