using System;
using UnityEngine;

namespace SmartFPController.Utils
{
	public static class ASKMath
	{
		public const float HALF_PI = (float)Math.PI / 2f;

		public const float DOUBLE_PI = (float)Math.PI * 2f;

		public static float SnapToZero(float value, float epsilon = 0.0001f)
		{
			return (!(Mathf.Abs(value) < epsilon)) ? value : 0f;
		}

		public static double Round(double value, int digits)
		{
			return Math.Round(value, digits);
		}

		public static float Round(float value, int digits)
		{
			return (float)Math.Round(value, digits);
		}

		public static float Persent01(float src, float percent)
		{
			return 1f - (1f - src) * Math.Abs(percent);
		}
	}
}
