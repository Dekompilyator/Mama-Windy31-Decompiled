using System;

namespace GleyMobileAds
{
	[Serializable]
	public class PlatformSettings
	{
		public SupportedPlatforms platform;

		public bool enabled;

		public AdvertiserId appId;

		public AdvertiserId idBanner;

		public AdvertiserId idInterstitial;

		public AdvertiserId idRewarded;

		public bool hasBanner;

		public bool hasInterstitial;

		public bool hasRewarded;

		public bool directedForChildren;

		public PlatformSettings(SupportedPlatforms platform, AdvertiserId appId, AdvertiserId idBanner, AdvertiserId idInterstitial, AdvertiserId idRewarded, bool hasBanner, bool hasInterstitial, bool hasRewarded)
		{
			this.platform = platform;
			this.appId = appId;
			this.idBanner = idBanner;
			this.idInterstitial = idInterstitial;
			this.idRewarded = idRewarded;
			this.hasBanner = hasBanner;
			this.hasInterstitial = hasInterstitial;
			this.hasRewarded = hasRewarded;
			enabled = false;
			directedForChildren = false;
		}
	}
}
