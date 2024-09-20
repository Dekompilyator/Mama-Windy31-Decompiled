using System;
using UnityEngine;

namespace SmartFPController
{
	public class InputSettings : ScriptableObject
	{
		[Serializable]
		public sealed class ActionData
		{
			[Serializable]
			public sealed class ActionAxis
			{
				public string axisName = string.Empty;

				[SerializeField]
				private EAxisSource axisSource;

				[SerializeField]
				[Range(0f, 1f)]
				private float threshold;

				[SerializeField]
				private EAxisEventsClamp axisStateClamp;

				private EAxisState axisState;

				private bool touchDown;

				private int pressedFrame = -1;

				private int releasedFrame = -1;

				public EAxisState getAxisState
				{
					get
					{
						float num = ((axisSource != 0) ? Input.GetAxis(axisName) : GetAxis(axisName));
						bool flag = axisStateClamp == EAxisEventsClamp.OnlyPositive || axisStateClamp == EAxisEventsClamp.All;
						bool flag2 = axisStateClamp == EAxisEventsClamp.OnlyNegative || axisStateClamp == EAxisEventsClamp.All;
						int num2 = Time.frameCount - 1;
						if (flag && num > threshold)
						{
							if (pressedFrame == num2)
							{
								touchDown = true;
							}
							if (touchDown)
							{
								axisState = EAxisState.PositivePress;
							}
							else
							{
								pressedFrame = Time.frameCount;
								axisState = EAxisState.PositiveDown;
							}
						}
						else if (flag2 && num < 0f - threshold)
						{
							if (pressedFrame == num2)
							{
								touchDown = true;
							}
							if (touchDown)
							{
								axisState = EAxisState.NegativePress;
							}
							else
							{
								pressedFrame = Time.frameCount;
								axisState = EAxisState.NegativeDown;
							}
						}
						else if (num <= threshold || num >= 0f - threshold)
						{
							bool flag3 = axisState == EAxisState.PositiveDown || axisState == EAxisState.PositivePress;
							bool flag4 = axisState == EAxisState.NegativeDown || axisState == EAxisState.NegativePress;
							if (touchDown && (flag3 || flag4))
							{
								if (flag && flag3)
								{
									axisState = EAxisState.PositiveUp;
								}
								else if (flag2 && flag4)
								{
									axisState = EAxisState.NegativeUp;
								}
								releasedFrame = Time.frameCount;
							}
							else
							{
								if (releasedFrame == num2)
								{
									touchDown = false;
								}
								if (!touchDown)
								{
									axisState = EAxisState.NONE;
								}
							}
						}
						return axisState;
					}
				}
			}

			internal struct ActionEvents
			{
				internal bool useDown;

				internal bool usePress;

				internal bool useUp;

				internal ActionHandler downHandler;

				internal ActionHandler pressHandler;

				internal ActionHandler upHandler;
			}

			public string name = string.Empty;

			public EActionType type;

			public KeyCode[] keys;

			public ActionAxis[] actionAxes;

			internal ActionEvents actionEvents;

			internal ActionEvents axisPositiveEvents;

			internal ActionEvents axisNegativeEvents;
		}

		[Serializable]
		public sealed class AxisData
		{
			[Serializable]
			public struct CustomKeys
			{
				public KeyCode negativeKey;

				public KeyCode positiveKey;
			}

			public string name = string.Empty;

			public EAxisType type;

			public string[] unityAxes;

			public CustomKeys[] customKeys;

			public bool normalize;

			[NonSerialized]
			public bool useIt;

			[NonSerialized]
			public AxisHandler axisHandler;
		}

		[SerializeField]
		private ActionData[] actionDatabase;

		[SerializeField]
		private AxisData[] axesDatabase;

		private static InputSettings instance;

		public static ActionData[] ActionDB
		{
			get
			{
				return m_Instance.actionDatabase;
			}
		}

		public static AxisData[] AxesDB
		{
			get
			{
				return m_Instance.axesDatabase;
			}
		}

		private static InputSettings m_Instance
		{
			get
			{
				if (instance == null)
				{
					instance = Resources.Load<InputSettings>("InputSettings");
				}
				return instance;
			}
		}

		internal static void BindAction(string m_Name, EActionEvent m_Event, ActionHandler m_Handler)
		{
			ActionData[] actionDB = ActionDB;
			foreach (ActionData actionData in actionDB)
			{
				if (!(actionData.name == m_Name))
				{
					continue;
				}
				switch (m_Event)
				{
				case EActionEvent.Down:
					actionData.actionEvents.useDown = true;
					if (actionData.actionEvents.downHandler != m_Handler)
					{
						ref ActionData.ActionEvents actionEvents2 = ref actionData.actionEvents;
						actionEvents2.downHandler = (ActionHandler)Delegate.Combine(actionEvents2.downHandler, m_Handler);
					}
					break;
				case EActionEvent.Press:
					actionData.actionEvents.usePress = true;
					if (actionData.actionEvents.pressHandler != m_Handler)
					{
						ref ActionData.ActionEvents actionEvents3 = ref actionData.actionEvents;
						actionEvents3.pressHandler = (ActionHandler)Delegate.Combine(actionEvents3.pressHandler, m_Handler);
					}
					break;
				case EActionEvent.Up:
					actionData.actionEvents.useUp = true;
					if (actionData.actionEvents.upHandler != m_Handler)
					{
						ref ActionData.ActionEvents actionEvents = ref actionData.actionEvents;
						actionEvents.upHandler = (ActionHandler)Delegate.Combine(actionEvents.upHandler, m_Handler);
					}
					break;
				}
				return;
			}
			Debug.LogError("Action " + m_Name + " Not Found.");
		}

		internal static void UnbindAction(string m_Name, EActionEvent m_Event, ActionHandler m_Handler)
		{
			ActionData[] actionDB = ActionDB;
			foreach (ActionData actionData in actionDB)
			{
				if (!(actionData.name == m_Name))
				{
					continue;
				}
				switch (m_Event)
				{
				case EActionEvent.Down:
					if (actionData.actionEvents.downHandler == m_Handler)
					{
						ref ActionData.ActionEvents actionEvents2 = ref actionData.actionEvents;
						actionEvents2.downHandler = (ActionHandler)Delegate.Remove(actionEvents2.downHandler, m_Handler);
						actionData.actionEvents.useDown = actionData.actionEvents.downHandler != null;
					}
					break;
				case EActionEvent.Press:
					if (actionData.actionEvents.pressHandler == m_Handler)
					{
						ref ActionData.ActionEvents actionEvents3 = ref actionData.actionEvents;
						actionEvents3.pressHandler = (ActionHandler)Delegate.Remove(actionEvents3.pressHandler, m_Handler);
						actionData.actionEvents.usePress = actionData.actionEvents.pressHandler != null;
					}
					break;
				case EActionEvent.Up:
					if (actionData.actionEvents.upHandler == m_Handler)
					{
						ref ActionData.ActionEvents actionEvents = ref actionData.actionEvents;
						actionEvents.upHandler = (ActionHandler)Delegate.Remove(actionEvents.upHandler, m_Handler);
						actionData.actionEvents.useUp = actionData.actionEvents.upHandler != null;
					}
					break;
				}
				return;
			}
			Debug.LogError("Action " + m_Name + " Not Found.");
		}

		internal static void BindActionAxis(string m_Name, EAxisState m_State, ActionHandler m_Handler)
		{
			ActionData[] actionDB = ActionDB;
			foreach (ActionData actionData in actionDB)
			{
				if (!(actionData.name == m_Name))
				{
					continue;
				}
				switch (m_State)
				{
				case EAxisState.PositiveDown:
					actionData.axisPositiveEvents.useDown = true;
					if (actionData.axisPositiveEvents.downHandler != m_Handler)
					{
						ref ActionData.ActionEvents axisPositiveEvents2 = ref actionData.axisPositiveEvents;
						axisPositiveEvents2.downHandler = (ActionHandler)Delegate.Combine(axisPositiveEvents2.downHandler, m_Handler);
					}
					break;
				case EAxisState.PositivePress:
					actionData.axisPositiveEvents.usePress = true;
					if (actionData.axisPositiveEvents.pressHandler != m_Handler)
					{
						ref ActionData.ActionEvents axisPositiveEvents3 = ref actionData.axisPositiveEvents;
						axisPositiveEvents3.pressHandler = (ActionHandler)Delegate.Combine(axisPositiveEvents3.pressHandler, m_Handler);
					}
					break;
				case EAxisState.PositiveUp:
					actionData.axisPositiveEvents.useUp = true;
					if (actionData.axisPositiveEvents.upHandler != m_Handler)
					{
						ref ActionData.ActionEvents axisPositiveEvents = ref actionData.axisPositiveEvents;
						axisPositiveEvents.upHandler = (ActionHandler)Delegate.Combine(axisPositiveEvents.upHandler, m_Handler);
					}
					break;
				case EAxisState.NegativeDown:
					actionData.axisNegativeEvents.useDown = true;
					if (actionData.axisNegativeEvents.downHandler != m_Handler)
					{
						ref ActionData.ActionEvents axisNegativeEvents3 = ref actionData.axisNegativeEvents;
						axisNegativeEvents3.downHandler = (ActionHandler)Delegate.Combine(axisNegativeEvents3.downHandler, m_Handler);
					}
					break;
				case EAxisState.NegativePress:
					actionData.axisNegativeEvents.usePress = true;
					if (actionData.axisNegativeEvents.pressHandler != m_Handler)
					{
						ref ActionData.ActionEvents axisNegativeEvents2 = ref actionData.axisNegativeEvents;
						axisNegativeEvents2.pressHandler = (ActionHandler)Delegate.Combine(axisNegativeEvents2.pressHandler, m_Handler);
					}
					break;
				case EAxisState.NegativeUp:
					actionData.axisNegativeEvents.useUp = true;
					if (actionData.axisNegativeEvents.upHandler != m_Handler)
					{
						ref ActionData.ActionEvents axisNegativeEvents = ref actionData.axisNegativeEvents;
						axisNegativeEvents.upHandler = (ActionHandler)Delegate.Combine(axisNegativeEvents.upHandler, m_Handler);
					}
					break;
				}
				return;
			}
			Debug.LogError("Action " + m_Name + " Not Found.");
		}

		internal static void UnbindActionAxis(string m_Name, EAxisState m_State, ActionHandler m_Handler)
		{
			ActionData[] actionDB = ActionDB;
			foreach (ActionData actionData in actionDB)
			{
				if (!(actionData.name == m_Name))
				{
					continue;
				}
				switch (m_State)
				{
				case EAxisState.PositiveDown:
					if (actionData.axisPositiveEvents.downHandler == m_Handler)
					{
						ref ActionData.ActionEvents axisPositiveEvents2 = ref actionData.axisPositiveEvents;
						axisPositiveEvents2.downHandler = (ActionHandler)Delegate.Remove(axisPositiveEvents2.downHandler, m_Handler);
						actionData.axisPositiveEvents.useDown = actionData.axisPositiveEvents.downHandler != null;
					}
					break;
				case EAxisState.PositivePress:
					if (actionData.axisPositiveEvents.pressHandler == m_Handler)
					{
						ref ActionData.ActionEvents axisPositiveEvents3 = ref actionData.axisPositiveEvents;
						axisPositiveEvents3.pressHandler = (ActionHandler)Delegate.Remove(axisPositiveEvents3.pressHandler, m_Handler);
						actionData.axisPositiveEvents.usePress = actionData.axisPositiveEvents.pressHandler != null;
					}
					break;
				case EAxisState.PositiveUp:
					if (actionData.axisPositiveEvents.upHandler == m_Handler)
					{
						ref ActionData.ActionEvents axisPositiveEvents = ref actionData.axisPositiveEvents;
						axisPositiveEvents.upHandler = (ActionHandler)Delegate.Remove(axisPositiveEvents.upHandler, m_Handler);
						actionData.axisPositiveEvents.useUp = actionData.axisPositiveEvents.upHandler != null;
					}
					break;
				case EAxisState.NegativeDown:
					if (actionData.axisNegativeEvents.downHandler == m_Handler)
					{
						ref ActionData.ActionEvents axisNegativeEvents3 = ref actionData.axisNegativeEvents;
						axisNegativeEvents3.downHandler = (ActionHandler)Delegate.Remove(axisNegativeEvents3.downHandler, m_Handler);
						actionData.axisNegativeEvents.useDown = actionData.axisNegativeEvents.downHandler != null;
					}
					break;
				case EAxisState.NegativePress:
					if (actionData.axisNegativeEvents.pressHandler == m_Handler)
					{
						ref ActionData.ActionEvents axisNegativeEvents2 = ref actionData.axisNegativeEvents;
						axisNegativeEvents2.pressHandler = (ActionHandler)Delegate.Remove(axisNegativeEvents2.pressHandler, m_Handler);
						actionData.axisNegativeEvents.usePress = actionData.axisNegativeEvents.pressHandler != null;
					}
					break;
				case EAxisState.NegativeUp:
					if (actionData.axisNegativeEvents.upHandler == m_Handler)
					{
						ref ActionData.ActionEvents axisNegativeEvents = ref actionData.axisNegativeEvents;
						axisNegativeEvents.upHandler = (ActionHandler)Delegate.Remove(axisNegativeEvents.upHandler, m_Handler);
						actionData.axisNegativeEvents.useUp = actionData.axisNegativeEvents.upHandler != null;
					}
					break;
				}
				return;
			}
			Debug.LogError("Action " + m_Name + " Not Found.");
		}

		internal static void BindAxis(string m_Name, AxisHandler m_Handler)
		{
			AxisData[] axesDB = AxesDB;
			foreach (AxisData axisData in axesDB)
			{
				if (axisData.name == m_Name)
				{
					axisData.useIt = true;
					if (axisData.axisHandler != m_Handler)
					{
						axisData.axisHandler = (AxisHandler)Delegate.Combine(axisData.axisHandler, m_Handler);
					}
					return;
				}
			}
			Debug.LogError("Axis " + m_Name + " Not Found.");
		}

		internal static void UnbindAxis(string m_Name, AxisHandler m_Handler)
		{
			AxisData[] axesDB = AxesDB;
			foreach (AxisData axisData in axesDB)
			{
				if (axisData.name == m_Name)
				{
					if (axisData.axisHandler == m_Handler)
					{
						axisData.axisHandler = (AxisHandler)Delegate.Remove(axisData.axisHandler, m_Handler);
						axisData.useIt = axisData.axisHandler != null;
					}
					return;
				}
			}
			Debug.LogError("Axis " + m_Name + " Not Found.");
		}

		internal static void RunActions()
		{
			ActionData[] actionDB = ActionDB;
			foreach (ActionData actionData in actionDB)
			{
				if (actionData.actionEvents.useDown)
				{
					if (actionData.type == EActionType.KeyCode || actionData.type == EActionType.Mixed)
					{
						KeyCode[] keys = actionData.keys;
						foreach (KeyCode key in keys)
						{
							if (Input.GetKeyDown(key))
							{
								actionData.actionEvents.downHandler();
								break;
							}
						}
					}
					if (actionData.type == EActionType.Axis || actionData.type == EActionType.Mixed)
					{
						ActionData.ActionAxis[] actionAxes = actionData.actionAxes;
						foreach (ActionData.ActionAxis actionAxis in actionAxes)
						{
							EAxisState getAxisState = actionAxis.getAxisState;
							if (getAxisState == EAxisState.NegativeDown || getAxisState == EAxisState.PositiveDown)
							{
								actionData.actionEvents.downHandler();
								break;
							}
						}
					}
				}
				if (actionData.actionEvents.usePress)
				{
					if (actionData.type == EActionType.KeyCode || actionData.type == EActionType.Mixed)
					{
						KeyCode[] keys2 = actionData.keys;
						foreach (KeyCode key2 in keys2)
						{
							if (Input.GetKeyDown(key2))
							{
								actionData.actionEvents.pressHandler();
								break;
							}
						}
					}
					if (actionData.type == EActionType.Axis || actionData.type == EActionType.Mixed)
					{
						ActionData.ActionAxis[] actionAxes2 = actionData.actionAxes;
						foreach (ActionData.ActionAxis actionAxis2 in actionAxes2)
						{
							EAxisState getAxisState2 = actionAxis2.getAxisState;
							if (getAxisState2 == EAxisState.NegativeDown || getAxisState2 == EAxisState.PositiveDown)
							{
								actionData.actionEvents.pressHandler();
								break;
							}
						}
					}
				}
				if (!actionData.actionEvents.useUp)
				{
					continue;
				}
				if (actionData.type == EActionType.KeyCode || actionData.type == EActionType.Mixed)
				{
					KeyCode[] keys3 = actionData.keys;
					foreach (KeyCode key3 in keys3)
					{
						if (Input.GetKeyDown(key3))
						{
							actionData.actionEvents.upHandler();
							break;
						}
					}
				}
				if (actionData.type != EActionType.Axis && actionData.type != EActionType.Mixed)
				{
					continue;
				}
				ActionData.ActionAxis[] actionAxes3 = actionData.actionAxes;
				foreach (ActionData.ActionAxis actionAxis3 in actionAxes3)
				{
					EAxisState getAxisState3 = actionAxis3.getAxisState;
					if (getAxisState3 == EAxisState.NegativeDown || getAxisState3 == EAxisState.PositiveDown)
					{
						actionData.actionEvents.upHandler();
						break;
					}
				}
			}
		}

		internal static void RunActionAxis()
		{
			ActionData[] actionDB = ActionDB;
			foreach (ActionData actionData in actionDB)
			{
				if (actionData.type == EActionType.KeyCode)
				{
					continue;
				}
				ActionData.ActionAxis[] actionAxes = actionData.actionAxes;
				foreach (ActionData.ActionAxis actionAxis in actionAxes)
				{
					EAxisState getAxisState = actionAxis.getAxisState;
					if (actionData.axisPositiveEvents.useDown && getAxisState == EAxisState.PositiveDown)
					{
						actionData.axisPositiveEvents.downHandler();
						break;
					}
					if (actionData.axisPositiveEvents.usePress && getAxisState == EAxisState.PositivePress)
					{
						actionData.axisPositiveEvents.pressHandler();
						break;
					}
					if (actionData.axisPositiveEvents.useUp && getAxisState == EAxisState.PositiveUp)
					{
						actionData.axisPositiveEvents.upHandler();
						break;
					}
					if (actionData.axisNegativeEvents.useDown && getAxisState == EAxisState.NegativeDown)
					{
						actionData.axisNegativeEvents.downHandler();
						break;
					}
					if (actionData.axisNegativeEvents.usePress && getAxisState == EAxisState.NegativePress)
					{
						actionData.axisNegativeEvents.pressHandler();
						break;
					}
					if (actionData.axisNegativeEvents.useUp && getAxisState == EAxisState.NegativeUp)
					{
						actionData.axisNegativeEvents.upHandler();
						break;
					}
				}
			}
		}

		internal static void RunAxis()
		{
			AxisData[] axesDB = AxesDB;
			foreach (AxisData axisData in axesDB)
			{
				if (axisData.useIt)
				{
					axisData.axisHandler(GetAxisValue(axisData));
				}
			}
		}

		internal static bool GetAction(string m_Name, EActionEvent m_Event)
		{
			ActionData[] actionDB = ActionDB;
			foreach (ActionData actionData in actionDB)
			{
				if (!(actionData.name == m_Name))
				{
					continue;
				}
				switch (m_Event)
				{
				case EActionEvent.Down:
				{
					if (actionData.type == EActionType.KeyCode || actionData.type == EActionType.Mixed)
					{
						KeyCode[] keys3 = actionData.keys;
						foreach (KeyCode key3 in keys3)
						{
							if (Input.GetKeyDown(key3))
							{
								return true;
							}
						}
					}
					if (actionData.type != EActionType.Axis && actionData.type != EActionType.Mixed)
					{
						break;
					}
					ActionData.ActionAxis[] actionAxes3 = actionData.actionAxes;
					foreach (ActionData.ActionAxis actionAxis3 in actionAxes3)
					{
						EAxisState getAxisState3 = actionAxis3.getAxisState;
						if (getAxisState3 == EAxisState.NegativeDown || getAxisState3 == EAxisState.PositiveDown)
						{
							return true;
						}
					}
					break;
				}
				case EActionEvent.Press:
				{
					if (actionData.type == EActionType.KeyCode || actionData.type == EActionType.Mixed)
					{
						KeyCode[] keys2 = actionData.keys;
						foreach (KeyCode key2 in keys2)
						{
							if (Input.GetKey(key2))
							{
								return true;
							}
						}
					}
					if (actionData.type != EActionType.Axis && actionData.type != EActionType.Mixed)
					{
						break;
					}
					ActionData.ActionAxis[] actionAxes2 = actionData.actionAxes;
					foreach (ActionData.ActionAxis actionAxis2 in actionAxes2)
					{
						EAxisState getAxisState2 = actionAxis2.getAxisState;
						if (getAxisState2 == EAxisState.NegativePress || getAxisState2 == EAxisState.PositivePress)
						{
							return true;
						}
					}
					break;
				}
				case EActionEvent.Up:
				{
					if (actionData.type == EActionType.KeyCode || actionData.type == EActionType.Mixed)
					{
						KeyCode[] keys = actionData.keys;
						foreach (KeyCode key in keys)
						{
							if (Input.GetKeyUp(key))
							{
								return true;
							}
						}
					}
					if (actionData.type != EActionType.Axis && actionData.type != EActionType.Mixed)
					{
						break;
					}
					ActionData.ActionAxis[] actionAxes = actionData.actionAxes;
					foreach (ActionData.ActionAxis actionAxis in actionAxes)
					{
						EAxisState getAxisState = actionAxis.getAxisState;
						if (getAxisState == EAxisState.NegativeUp || getAxisState == EAxisState.PositiveUp)
						{
							return true;
						}
					}
					break;
				}
				}
				return false;
			}
			Debug.LogError("Action " + m_Name + " Not Found!");
			return false;
		}

		internal static bool GetActionAxis(string m_Name, EAxisState m_State)
		{
			ActionData[] actionDB = ActionDB;
			foreach (ActionData actionData in actionDB)
			{
				if (!(actionData.name == m_Name))
				{
					continue;
				}
				if (actionData.type != 0)
				{
					ActionData.ActionAxis[] actionAxes = actionData.actionAxes;
					foreach (ActionData.ActionAxis actionAxis in actionAxes)
					{
						if (actionAxis.getAxisState == m_State)
						{
							return true;
						}
					}
				}
				return false;
			}
			Debug.LogError("ActionAxis " + m_Name + " Not Found!");
			return false;
		}

		internal static float GetAxis(string axisName)
		{
			AxisData[] axesDB = AxesDB;
			foreach (AxisData axisData in axesDB)
			{
				if (axisData.name == axisName)
				{
					return GetAxisValue(axisData);
				}
			}
			Debug.LogError("Axis " + axisName + " Not Found.");
			return 0f;
		}

		private static float GetAxisValue(AxisData aData)
		{
			float num = 0f;
			if (aData.type == EAxisType.Unity || aData.type == EAxisType.Mixed)
			{
				string[] unityAxes = aData.unityAxes;
				foreach (string axisName in unityAxes)
				{
					num += Input.GetAxis(axisName);
				}
			}
			if (aData.type == EAxisType.Custom || aData.type == EAxisType.Mixed)
			{
				AxisData.CustomKeys[] customKeys = aData.customKeys;
				for (int j = 0; j < customKeys.Length; j++)
				{
					AxisData.CustomKeys customKeys2 = customKeys[j];
					if (Input.GetKey(customKeys2.negativeKey))
					{
						num += -1f;
					}
					else if (Input.GetKey(customKeys2.positiveKey))
					{
						num += 1f;
					}
				}
			}
			return (!aData.normalize) ? num : Mathf.Clamp(num, -1f, 1f);
		}
	}
}
