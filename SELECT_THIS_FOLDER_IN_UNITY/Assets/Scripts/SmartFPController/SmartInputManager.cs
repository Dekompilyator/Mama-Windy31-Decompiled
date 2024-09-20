using System;
using TouchControlsKit;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SmartFPController
{
	public class SmartInputManager : MonoBehaviour
	{
		[Serializable]
		public sealed class Axes
		{
			public string moveX = "Move Horizontal";

			public string moveY = "Move Vertical";

			public string lookX = "Look Horizontal";

			public string lookY = "Look Vertical";

			public string moveJoystick = "Move Joystick";

			public string lookTouchpad = "Look Touchpad";
		}

		[Serializable]
		public sealed class Actions
		{
			public string run = "Run";

			public string jump = "Jump";

			public string crouch = "Crouch";

			public string pause = "Pause";

			public string blockCursor = "Block Cursor";

			public string unblockCursor = "Unblock Cursor";
		}

		public enum EInputType
		{
			Standalone = 0,
			TouchControlsKit = 1
		}

		[SerializeField]
		private EUpdateType updateType;

		[SerializeField]
		private EInputType inputType;

		[SerializeField]
		private TCKInput touchUIElements;

		[SerializeField]
		private MenuElements menuPrefab;

		[SerializeField]
		private Axes axes = new Axes();

		[SerializeField]
		private Actions actions = new Actions();

		private bool cursorIsBlocked = true;

		private FirstPersonController m_Controller;

		private MenuElements m_Menu;

		public bool gameIsPaused { get; private set; }

		internal float moveHorizontal { get; private set; }

		internal float moveVertical { get; private set; }

		internal float lookHorizontal { get; private set; }

		internal float lookVertical { get; private set; }

		internal bool runAction { get; private set; }

		private void SpawnUIElements()
		{
			m_Menu = SpawnSingleUIElement(menuPrefab);
			m_Menu.AwakeMENU(this);
			SpawnSingleUIElement(touchUIElements);
			if (UnityEngine.Object.FindObjectOfType<EventSystem>() == null)
			{
				new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
			}
		}

		private static T SpawnSingleUIElement<T>(T prefab) where T : MonoBehaviour
		{
			T[] array = UnityEngine.Object.FindObjectsOfType<T>();
			int num = array.Length;
			for (int i = 1; i < num; i++)
			{
				UnityEngine.Object.Destroy(array[i].gameObject);
			}
			T val = ((num <= 0) ? ((T)null) : array[0]);
			if (val == null)
			{
				if (prefab != null)
				{
					val = UnityEngine.Object.Instantiate(prefab);
				}
				else
				{
					Debug.LogError("Error: UI Prefab is not setup.");
				}
			}
			return val;
		}

		private void Awake()
		{
			m_Controller = GetComponent<FirstPersonController>();
			SpawnUIElements();
			InputSettings.BindAction(actions.jump, EActionEvent.Down, m_Controller.Jump);
			InputSettings.BindAction(actions.crouch, EActionEvent.Down, m_Controller.Crouch);
			InputSettings.BindAction(actions.blockCursor, EActionEvent.Down, BlockCursor);
			InputSettings.BindAction(actions.unblockCursor, EActionEvent.Down, UnblockCursor);
		}

		private void Start()
		{
			GameSettings.UpdateMixerVolumes();
			m_Menu.SetActive(false);
		}

		private void Update()
		{
			if (updateType == EUpdateType.Update)
			{
				InputsUpdate();
			}
		}

		private void LateUpdate()
		{
			if (updateType == EUpdateType.LateUpdate)
			{
				InputsUpdate();
			}
		}

		private void FixedUpdate()
		{
			if (updateType == EUpdateType.FixedUpdate)
			{
				InputsUpdate();
			}
		}

		private void InputsUpdate()
		{
			if (inputType == EInputType.TouchControlsKit)
			{
				TouchKitInput();
			}
			else
			{
				StandaloneInput();
			}
		}

		private void StandaloneInput()
		{
			if (InputSettings.GetAction(actions.pause, EActionEvent.Down))
			{
				Pause();
			}
			if (!gameIsPaused)
			{
				InputSettings.RunActions();
				InputSettings.RunActionAxis();
				InputSettings.RunAxis();
				if (cursorIsBlocked && Time.timeSinceLevelLoad > 0.1f)
				{
					Cursor.lockState = CursorLockMode.Locked;
					Cursor.visible = false;
				}
				runAction = InputSettings.GetAction(actions.run, EActionEvent.Press);
				moveHorizontal = InputSettings.GetAxis(axes.moveX);
				moveVertical = InputSettings.GetAxis(axes.moveY);
				lookHorizontal = InputSettings.GetAxis(axes.lookX) * GameSettings.GetLookSensitivityByInvert_X;
				lookVertical = InputSettings.GetAxis(axes.lookY) * GameSettings.GetLookSensitivityByInvert_Y;
			}
		}

		private void TouchKitInput()
		{
			if (TCKInput.CheckController(actions.pause) && TCKInput.GetAction(actions.pause, TouchControlsKit.EActionEvent.Down))
			{
				Pause();
			}
			if (!gameIsPaused)
			{
				runAction = TCKInput.CheckController(actions.run) && TCKInput.GetAction(actions.run, TouchControlsKit.EActionEvent.Press);
				if (TCKInput.CheckController(actions.jump) && TCKInput.GetAction(actions.jump, TouchControlsKit.EActionEvent.Down))
				{
					m_Controller.Jump();
				}
				if (TCKInput.CheckController(actions.crouch) && TCKInput.GetAction(actions.crouch, TouchControlsKit.EActionEvent.Down))
				{
					m_Controller.Crouch();
				}
				if (TCKInput.CheckController(axes.moveJoystick))
				{
					moveHorizontal = Mathf.Clamp(TCKInput.GetAxis(axes.moveJoystick, TouchControlsKit.EAxisType.Horizontal), -1f, 1f);
					moveVertical = ((!runAction) ? Mathf.Clamp(TCKInput.GetAxis(axes.moveJoystick, TouchControlsKit.EAxisType.Vertical), -1f, 1f) : 1f);
				}
				if (TCKInput.CheckController(axes.lookTouchpad))
				{
					lookHorizontal = TCKInput.GetAxis(axes.lookTouchpad, TouchControlsKit.EAxisType.Horizontal) * GameSettings.GetLookSensitivityByInvert_X;
					lookVertical = TCKInput.GetAxis(axes.lookTouchpad, TouchControlsKit.EAxisType.Vertical) * GameSettings.GetLookSensitivityByInvert_Y;
				}
			}
		}

		public static void BindAction(string m_Name, EActionEvent m_Event, ActionHandler m_Handler)
		{
			InputSettings.BindAction(m_Name, m_Event, m_Handler);
		}

		public static void UnbindAction(string m_Name, EActionEvent m_Event, ActionHandler m_Handler)
		{
			InputSettings.UnbindAction(m_Name, m_Event, m_Handler);
		}

		public static void BindActionByAxis(string actionName, EAxisState axisState, ActionHandler m_Handler)
		{
			InputSettings.BindActionAxis(actionName, axisState, m_Handler);
		}

		public static void UnbindActionByAxis(string actionName, EAxisState axisState, ActionHandler m_Handler)
		{
			InputSettings.UnbindActionAxis(actionName, axisState, m_Handler);
		}

		public static void BindAxis(string m_Name, AxisHandler m_Handler)
		{
			InputSettings.BindAxis(m_Name, m_Handler);
		}

		public static void UnbindAxis(string m_Name, AxisHandler m_Handler)
		{
			InputSettings.UnbindAxis(m_Name, m_Handler);
		}

		public static bool GetAction(string m_Name, EActionEvent m_Event)
		{
			return InputSettings.GetAction(m_Name, m_Event);
		}

		public static bool GetActionByAxis(string actionName, EAxisState axisState)
		{
			return InputSettings.GetActionAxis(actionName, axisState);
		}

		public static float GetAxis(string m_Name)
		{
			return InputSettings.GetAxis(m_Name);
		}

		public void BlockCursor()
		{
			cursorIsBlocked = true;
		}

		public void UnblockCursor()
		{
			cursorIsBlocked = false;
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}

		public void Pause()
		{
			gameIsPaused = !gameIsPaused;
			Time.timeScale = ((!gameIsPaused) ? 1f : 0f);
			m_Menu.SetActive(gameIsPaused);
			TCKInput.SetActive(!gameIsPaused);
		}

		public void PlayerDie()
		{
			m_Controller.PlayerDie();
			m_Menu.SetActive(true);
			TCKInput.SetActive(false);
		}
	}
}
