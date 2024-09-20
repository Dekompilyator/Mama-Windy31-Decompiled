using System;
using System.Collections.Generic;

namespace GleyMobileAds
{
	[Serializable]
	public class AdvertiserSettings
	{
		public SupportedAdvertisers advertiser;

		public bool useSDK;

		public string preprocessorDirective;

		public string sdkLink;

		public List<PlatformSettings> platformSettings;

		public AdvertiserSettings(SupportedAdvertisers advertiser, string sdkLink, string preprocessorDirective)
		{
			this.advertiser = advertiser;
			this.sdkLink = sdkLink;
			this.preprocessorDirective = preprocessorDirective;
			useSDK = false;
			platformSettings = new List<PlatformSettings>();
		}
	}
}
