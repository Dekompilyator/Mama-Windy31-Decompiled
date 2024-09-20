using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GleyMobileAds
{
	public class AdSettings : ScriptableObject
	{
		public List<MediationSettings> mediationSettings = new List<MediationSettings>();

		public List<AdvertiserSettings> advertiserSettings = new List<AdvertiserSettings>();

		public bool debugMode;

		public bool usePlaymaker;

		public SupportedMediation bannerMediation;

		public SupportedMediation interstitialMediation;

		public SupportedMediation rewardedMediation;

		public string externalFileUrl = "Paste your external config file url here";

		public MediationSettings GetAdvertiserSettings(SupportedAdvertisers advertiser)
		{
			return mediationSettings.FirstOrDefault((MediationSettings cond) => cond.advertiser == advertiser);
		}

		public List<PlatformSettings> GetPlaftormSettings(SupportedAdvertisers advertiser)
		{
			return advertiserSettings.FirstOrDefault((AdvertiserSettings cond) => cond.advertiser == advertiser).platformSettings;
		}
	}
}
