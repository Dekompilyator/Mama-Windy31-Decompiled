using UnityEngine;

namespace TouchControlsKit
{
	[RequireComponent(typeof(Camera))]
	public sealed class GuiCamera : MonoBehaviour
	{
		public static Camera getCamera { get; private set; }

		public static Transform getTransform { get; private set; }

		private void Awake()
		{
			getTransform = base.transform;
			getCamera = GetComponent<Camera>();
		}

		public static Vector2 ScreenToWorldPoint(Vector2 position)
		{
			return getCamera.ScreenToWorldPoint(position);
		}
	}
}
