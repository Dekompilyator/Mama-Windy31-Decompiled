using UnityEngine;

namespace SmartFPController
{
	public class CustomSurface : MonoBehaviour
	{
		[SerializeField]
		private string m_SurfaceName;

		public string surfaceName
		{
			get
			{
				return m_SurfaceName;
			}
		}
	}
}
