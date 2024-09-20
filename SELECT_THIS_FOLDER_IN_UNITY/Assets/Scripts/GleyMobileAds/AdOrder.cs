using System;
using System.Collections.Generic;

namespace GleyMobileAds
{
	[Serializable]
	public class AdOrder
	{
		public SupportedMediation bannerMediation;

		public SupportedMediation interstitialMediation;

		public SupportedMediation rewardedMediation;

		public List<MediationSettings> advertisers = new List<MediationSettings>();
	}
}
