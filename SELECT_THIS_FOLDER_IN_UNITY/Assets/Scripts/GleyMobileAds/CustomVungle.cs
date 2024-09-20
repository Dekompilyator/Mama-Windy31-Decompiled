using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GleyMobileAds
{
	public class CustomVungle : MonoBehaviour, ICustomAds
	{
		public void HideBanner()
		{
		}

		public void InitializeAds(GDPRConsent consent, List<PlatformSettings> platformSettings)
		{
		}

		public void ResetBannerUsage()
		{
		}

		public bool BannerAlreadyUsed()
		{
			return false;
		}

		public bool IsBannerAvailable()
		{
			return false;
		}

		public bool IsInterstitialAvailable()
		{
			return false;
		}

		public bool IsRewardVideoAvailable()
		{
			return false;
		}

		public void ShowBanner(BannerPosition position, BannerType type, UnityAction<bool, BannerPosition, BannerType> DisplayResult)
		{
		}

		public void ShowInterstitial(UnityAction InterstitialClosed = null)
		{
		}

		public void ShowInterstitial(UnityAction<string> InterstitialClosed)
		{
		}

		public void ShowRewardVideo(UnityAction<bool> CompleteMethod)
		{
		}

		public void ShowRewardVideo(UnityAction<bool, string> CompleteMethod)
		{
		}

		public void UpdateConsent(GDPRConsent consent)
		{
		}
	}
}
