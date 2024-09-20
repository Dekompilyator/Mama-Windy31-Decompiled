using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GleyMobileAds
{
	public class CustomAdmob : MonoBehaviour, ICustomAds
	{
		public void InitializeAds(GDPRConsent consent, List<PlatformSettings> platformSettings)
		{
		}

		public bool IsInterstitialAvailable()
		{
			return false;
		}

		public bool IsRewardVideoAvailable()
		{
			return false;
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

		public void HideBanner()
		{
		}

		public bool IsBannerAvailable()
		{
			return false;
		}

		public void ResetBannerUsage()
		{
		}

		public bool BannerAlreadyUsed()
		{
			return false;
		}

		public void ShowBanner(BannerPosition position, BannerType type, UnityAction<bool, BannerPosition, BannerType> DisplayResult)
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
