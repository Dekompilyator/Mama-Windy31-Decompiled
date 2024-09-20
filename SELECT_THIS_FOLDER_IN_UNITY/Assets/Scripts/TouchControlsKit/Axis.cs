using System;

namespace TouchControlsKit
{
	[Serializable]
	public sealed class Axis
	{
		public const int DIGITS = 2;

		public bool enabled = true;

		public bool inverse;

		public float value { get; private set; }

		public void SetValue(float value)
		{
			this.value = ((!enabled) ? 0f : ((float)Math.Round(value, 3)));
		}
	}
}
