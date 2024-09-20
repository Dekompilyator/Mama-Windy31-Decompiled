using System;
using UnityEngine;

namespace SmartFPController
{
	public sealed class SurfaceDetector : ScriptableObject
	{
		[Serializable]
		private struct SData
		{
			public string surfaceName;

			public UnityEngine.Object targetObject;

			public int GetHash()
			{
				return (targetObject != null) ? targetObject.GetHashCode() : 0;
			}
		}

		private struct SInfo
		{
			public readonly string surfaceName;

			public readonly int hashCode;

			public SInfo(string surfaceName, int hashCode)
			{
				this.surfaceName = surfaceName;
				this.hashCode = hashCode;
			}
		}

		[SerializeField]
		private SData[] m_Materials = new SData[0];

		[SerializeField]
		private SData[] m_Textures = new SData[0];

		[NonSerialized]
		private bool inited;

		private static SInfo[] materialsInfo;

		private static SInfo[] texturesInfo;

		[SerializeField]
		private string[] m_Names = new string[0];

		public const string UNKNOWN = "UNKNOWN";

		private static SurfaceDetector rawInstance;

		public static string[] allNames
		{
			get
			{
				return instance.m_Names;
			}
		}

		public static int count
		{
			get
			{
				return instance.m_Names.Length;
			}
		}

		private static SurfaceDetector instance
		{
			get
			{
				if (rawInstance == null)
				{
					rawInstance = Resources.Load<SurfaceDetector>(typeof(SurfaceDetector).Name);
				}
				return rawInstance;
			}
		}

		private static bool isValid
		{
			get
			{
				return instance != null;
			}
		}

		private static void PrintInvalidMessage()
		{
			Debug.LogError("SurfaceDetector instance not found. Please create it from [Tools->VictorsAssets->SmartFPController->Settings]");
		}

		private static void CheckInit()
		{
			if (!instance.inited)
			{
				instance.inited = true;
				WhriteInfo(instance.m_Materials, out materialsInfo);
				WhriteInfo(instance.m_Textures, out texturesInfo);
			}
		}

		private static void WhriteInfo(SData[] source, out SInfo[] target)
		{
			target = new SInfo[source.Length];
			for (int i = 0; i < source.Length; i++)
			{
				target[i] = new SInfo(source[i].surfaceName, source[i].GetHash());
			}
			Array.Sort(target, (SInfo x, SInfo y) => x.hashCode.CompareTo(y.hashCode));
		}

		public static string GetSurface(RaycastHit hit)
		{
			if (!isValid)
			{
				PrintInvalidMessage();
				return "UNKNOWN";
			}
			if (hit.collider == null || hit.collider.isTrigger)
			{
				return "UNKNOWN";
			}
			CheckInit();
			int? num = FindIndex(materialsInfo, hit.GetMaterial());
			int num2 = ((!num.HasValue) ? (-1) : num.Value);
			if (num2 >= 0)
			{
				return materialsInfo[num2].surfaceName;
			}
			int? num3 = FindIndex(texturesInfo, hit.GetTerrainTexture());
			num2 = ((!num3.HasValue) ? (-1) : num3.Value);
			if (num2 >= 0)
			{
				return texturesInfo[num2].surfaceName;
			}
			CustomSurface component = hit.collider.GetComponent<CustomSurface>();
			if (component != null)
			{
				return component.surfaceName;
			}
			return "UNKNOWN";
		}

		public static string GetSurface(Material meshMaterial)
		{
			if (!isValid)
			{
				PrintInvalidMessage();
				return "UNKNOWN";
			}
			CheckInit();
			int? num = FindIndex(materialsInfo, meshMaterial);
			int num2 = ((!num.HasValue) ? (-1) : num.Value);
			if (num2 >= 0)
			{
				return materialsInfo[num2].surfaceName;
			}
			return "UNKNOWN";
		}

		public static string GetSurface(Texture terrainTexture)
		{
			if (!isValid)
			{
				PrintInvalidMessage();
				return "UNKNOWN";
			}
			CheckInit();
			int? num = FindIndex(texturesInfo, terrainTexture);
			int num2 = ((!num.HasValue) ? (-1) : num.Value);
			if (num2 >= 0)
			{
				return texturesInfo[num2].surfaceName;
			}
			return "UNKNOWN";
		}

		public static bool TryGetSurface(RaycastHit hit, out string surface)
		{
			surface = GetSurface(hit);
			return surface != "UNKNOWN";
		}

		public static bool TryGetSurface(Material meshMaterial, out string surface)
		{
			surface = GetSurface(meshMaterial);
			return surface != "UNKNOWN";
		}

		public static bool TryGetSurface(Texture terrainTexture, out string surface)
		{
			surface = GetSurface(terrainTexture);
			return surface != "UNKNOWN";
		}

		private static int? FindIndex(SInfo[] array, UnityEngine.Object item)
		{
			if (item == null || array.Length == 0)
			{
				return null;
			}
			int num = 0;
			int num2 = array.Length;
			int hashCode = item.GetHashCode();
			if (hashCode >= array[num].hashCode && hashCode <= array[num2 - 1].hashCode)
			{
				while (num < num2)
				{
					int num3 = num + (num2 - num) / 2;
					if (hashCode <= array[num3].hashCode)
					{
						num2 = num3;
					}
					else
					{
						num = num3 + 1;
					}
				}
				if (array[num2].hashCode == hashCode)
				{
					return num2;
				}
			}
			return null;
		}
	}
}
