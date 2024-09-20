using System;

namespace GleyMobileAds
{
	[Serializable]
	public class AdvertiserId
	{
		public string id;

		public string displayName;

		public bool notRequired;

		public AdvertiserId(string displayName)
		{
			this.displayName = displayName;
			notRequired = false;
		}

		public AdvertiserId()
		{
			notRequired = true;
		}
	}
}
