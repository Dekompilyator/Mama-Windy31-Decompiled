using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GleyMobileAds
{
	public class CustomAppLovin : MonoBehaviour, ICustomAds
	{
		public void InitializeAds(GDPRConsent consent, List<PlatformSettings> platformSettings)
		{
		}

		public void UpdateConsent(GDPRConsent consent)
		{
		}

		public bool IsBannerAvailable()
		{
			return false;
		}

		public void ShowBanner(BannerPosition position, BannerType type, UnityAction<bool, BannerPosition, BannerType> DisplayResult)
		{
		}

		public void HideBanner()
		{
		}

		public void ResetBannerUsage()
		{
		}

		public bool BannerAlreadyUsed()
		{
			return false;
		}

		public bool IsInterstitialAvailable()
		{
			return false;
		}

		public void ShowInterstitial(UnityAction InterstitialClosed)
		{
		}

		public void ShowInterstitial(UnityAction<string> InterstitialClosed)
		{
		}

		public bool IsRewardVideoAvailable()
		{
			return false;
		}

		public void ShowRewardVideo(UnityAction<bool> CompleteMethod)
		{
		}

		public void ShowRewardVideo(UnityAction<bool, string> CompleteMethod)
		{
		}
	}
}
