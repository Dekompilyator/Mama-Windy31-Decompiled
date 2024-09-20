using UnityEngine;

namespace GleyMobileAds
{
	public class ScreenWriter : MonoBehaviour
	{
		private static string logMessage;

		private static ScreenWriter instance;

		public static void Write(object message)
		{
			if (Advertisements.Instance.debug)
			{
				if (instance == null)
				{
					GameObject gameObject = new GameObject();
					gameObject.name = "DebugMessagesHolder";
					instance = gameObject.AddComponent<ScreenWriter>();
					logMessage += "\nDebugMessages instance created on DebugMessagesHolder";
				}
				logMessage = logMessage + "\n" + message.ToString();
			}
		}

		private void OnGUI()
		{
			if (Advertisements.Instance.debug && logMessage != null)
			{
				GUI.Label(new Rect(0f, 0f, Screen.width, Screen.height), logMessage);
				if (GUI.Button(new Rect(Screen.width - 100, Screen.height - 100, 100f, 100f), "Clear"))
				{
					logMessage = null;
				}
			}
		}
	}
}
