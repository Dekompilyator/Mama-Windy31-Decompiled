using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Events;
using UnityEngine.Monetization;

namespace GleyMobileAds
{
	public class CustomUnityAds : MonoBehaviour, ICustomAds
	{
		private UnityAction<bool> OnCompleteMethod;

		private UnityAction<bool, string> OnCompleteMethodWithAdvertiser;

		private UnityAction OnInterstitialClosed;

		private UnityAction<string> OnInterstitialClosedWithAdvertiser;

		private string unityAdsId;

		private string bannerPlacement;

		private string videoAdPlacement;

		private string rewardedVideoAdPlacement;

		private bool debug;

		private bool bannerUsed;

		private BannerPosition position;

		private BannerType bannerType;

		private UnityAction<bool, BannerPosition, BannerType> DisplayResult;

		public void InitializeAds(GDPRConsent consent, List<PlatformSettings> platformSettings)
		{
			debug = Advertisements.Instance.debug;
			PlatformSettings platformSettings2 = platformSettings.First((PlatformSettings cond) => cond.platform == SupportedPlatforms.Android);
			unityAdsId = platformSettings2.appId.id;
			bannerPlacement = platformSettings2.idBanner.id;
			videoAdPlacement = platformSettings2.idInterstitial.id;
			rewardedVideoAdPlacement = platformSettings2.idRewarded.id;
			if (debug)
			{
				Debug.Log(string.Concat(this, " Initialization Started"));
				ScreenWriter.Write(string.Concat(this, " Initialization Started"));
				Debug.Log(string.Concat(this, " App ID: ", unityAdsId));
				ScreenWriter.Write(string.Concat(this, " App ID: ", unityAdsId));
				Debug.Log(string.Concat(this, " Banner placement ID: ", bannerPlacement));
				ScreenWriter.Write(string.Concat(this, " Banner Placement ID: ", bannerPlacement));
				Debug.Log(string.Concat(this, " Interstitial Placement ID: ", videoAdPlacement));
				ScreenWriter.Write(string.Concat(this, " Interstitial Placement ID: ", videoAdPlacement));
				Debug.Log(string.Concat(this, " Rewarded Video Placement ID: ", rewardedVideoAdPlacement));
				ScreenWriter.Write(string.Concat(this, " Rewarded Video Placement ID: ", rewardedVideoAdPlacement));
			}
			if (consent != 0)
			{
				UnityEngine.Monetization.MetaData metaData = new UnityEngine.Monetization.MetaData("gdpr");
				if (consent == GDPRConsent.Accept)
				{
					metaData.Set("consent", "true");
				}
				else
				{
					metaData.Set("consent", "false");
				}
				Monetization.SetMetaData(metaData);
			}
			Monetization.Initialize(unityAdsId, false);
			Advertisement.Initialize(unityAdsId, false);
		}

		public void UpdateConsent(GDPRConsent consent)
		{
			if (consent != 0)
			{
				UnityEngine.Monetization.MetaData metaData = new UnityEngine.Monetization.MetaData("gdpr");
				if (consent == GDPRConsent.Accept)
				{
					metaData.Set("consent", "true");
				}
				else
				{
					metaData.Set("consent", "false");
				}
				Monetization.SetMetaData(metaData);
			}
			Debug.Log(string.Concat(this, " Update consent to ", consent));
			ScreenWriter.Write(string.Concat(this, " Update consent to ", consent));
		}

		public bool IsInterstitialAvailable()
		{
			return Monetization.IsReady(videoAdPlacement);
		}

		public void ShowInterstitial(UnityAction InterstitialClosed)
		{
			if (IsInterstitialAvailable())
			{
				OnInterstitialClosed = InterstitialClosed;
				ShowAdPlacementContent showAdPlacementContent = null;
				showAdPlacementContent = Monetization.GetPlacementContent(videoAdPlacement) as ShowAdPlacementContent;
				if (showAdPlacementContent != null)
				{
					showAdPlacementContent.Show(InterstitialCallback);
				}
			}
		}

		public void ShowInterstitial(UnityAction<string> InterstitialClosed)
		{
			if (IsInterstitialAvailable())
			{
				OnInterstitialClosedWithAdvertiser = InterstitialClosed;
				ShowAdPlacementContent showAdPlacementContent = null;
				showAdPlacementContent = Monetization.GetPlacementContent(videoAdPlacement) as ShowAdPlacementContent;
				if (showAdPlacementContent != null)
				{
					showAdPlacementContent.Show(InterstitialCallback);
				}
			}
		}

		private void InterstitialCallback(UnityEngine.Monetization.ShowResult result)
		{
			if (debug)
			{
				Debug.Log(string.Concat(this, "Interstitial result: ", result.ToString()));
				ScreenWriter.Write(string.Concat(this, "Interstitial result: ", result.ToString()));
			}
			if (OnInterstitialClosed != null)
			{
				OnInterstitialClosed();
				OnInterstitialClosed = null;
			}
			if (OnInterstitialClosedWithAdvertiser != null)
			{
				OnInterstitialClosedWithAdvertiser(SupportedAdvertisers.Unity.ToString());
				OnInterstitialClosedWithAdvertiser = null;
			}
		}

		public bool IsRewardVideoAvailable()
		{
			return Monetization.IsReady(rewardedVideoAdPlacement);
		}

		public void ShowRewardVideo(UnityAction<bool> CompleteMethod)
		{
			if (IsRewardVideoAvailable())
			{
				OnCompleteMethod = CompleteMethod;
				ShowAdPlacementContent showAdPlacementContent = null;
				showAdPlacementContent = Monetization.GetPlacementContent(rewardedVideoAdPlacement) as ShowAdPlacementContent;
				if (showAdPlacementContent != null)
				{
					showAdPlacementContent.Show(RewardedCallback);
				}
			}
		}

		public void ShowRewardVideo(UnityAction<bool, string> CompleteMethod)
		{
			if (IsRewardVideoAvailable())
			{
				OnCompleteMethodWithAdvertiser = CompleteMethod;
				ShowAdPlacementContent showAdPlacementContent = null;
				showAdPlacementContent = Monetization.GetPlacementContent(rewardedVideoAdPlacement) as ShowAdPlacementContent;
				if (showAdPlacementContent != null)
				{
					showAdPlacementContent.Show(RewardedCallback);
				}
			}
		}

		private void RewardedCallback(UnityEngine.Monetization.ShowResult result)
		{
			if (debug)
			{
				Debug.Log(string.Concat(this, "Complete method result: ", result.ToString()));
				ScreenWriter.Write(string.Concat(this, "Complete method result: ", result.ToString()));
			}
			if (result == UnityEngine.Monetization.ShowResult.Finished)
			{
				if (OnCompleteMethod != null)
				{
					OnCompleteMethod(true);
					OnCompleteMethod = null;
				}
				if (OnCompleteMethodWithAdvertiser != null)
				{
					OnCompleteMethodWithAdvertiser(true, SupportedAdvertisers.Unity.ToString());
					OnCompleteMethodWithAdvertiser = null;
				}
			}
			else
			{
				if (OnCompleteMethod != null)
				{
					OnCompleteMethod(false);
					OnCompleteMethod = null;
				}
				if (OnCompleteMethodWithAdvertiser != null)
				{
					OnCompleteMethodWithAdvertiser(false, SupportedAdvertisers.Unity.ToString());
					OnCompleteMethodWithAdvertiser = null;
				}
			}
		}

		public bool IsBannerAvailable()
		{
			return Advertisement.IsReady(bannerPlacement);
		}

		public void ResetBannerUsage()
		{
			bannerUsed = false;
		}

		public bool BannerAlreadyUsed()
		{
			return bannerUsed;
		}

		public void ShowBanner(BannerPosition position, BannerType bannerType, UnityAction<bool, BannerPosition, BannerType> DisplayResult)
		{
			if (IsBannerAvailable())
			{
				bannerUsed = true;
				this.position = position;
				this.bannerType = bannerType;
				this.DisplayResult = DisplayResult;
				BannerLoadOptions bannerLoadOptions = new BannerLoadOptions();
				bannerLoadOptions.errorCallback = BannerLoadFailed;
				bannerLoadOptions.loadCallback = BannerLoadSuccess;
				BannerLoadOptions options = bannerLoadOptions;
				if (debug)
				{
					Debug.Log(string.Concat(this, "Start Loading Placement:", bannerPlacement, " is ready ", Advertisement.IsReady(bannerPlacement)));
					ScreenWriter.Write(string.Concat(this, "Start Loading Placement:", bannerPlacement, " is ready ", Advertisement.IsReady(bannerPlacement)));
				}
				Advertisement.Banner.Load(bannerPlacement, options);
			}
		}

		private void BannerLoadSuccess()
		{
			if (debug)
			{
				Debug.Log(string.Concat(this, "Banner Load Success"));
				ScreenWriter.Write(string.Concat(this, " Banner Load Success"));
			}
			BannerOptions bannerOptions = new BannerOptions();
			bannerOptions.showCallback = BanerDisplayed;
			bannerOptions.hideCallback = BannerHidded;
			BannerOptions options = bannerOptions;
			Advertisement.Banner.Show(bannerPlacement, options);
		}

		private void BannerLoadFailed(string message)
		{
			if (debug)
			{
				Debug.Log(string.Concat(this, "Banner Load Failed ", message));
				ScreenWriter.Write(string.Concat(this, " Banner Load Failed ", message));
			}
			if (DisplayResult != null)
			{
				DisplayResult(false, position, bannerType);
				DisplayResult = null;
			}
		}

		private void BanerDisplayed()
		{
			if (debug)
			{
				Debug.Log(string.Concat(this, "Baner Displayed"));
				ScreenWriter.Write(string.Concat(this, "Baner Displayed"));
			}
			if (DisplayResult != null)
			{
				DisplayResult(true, position, bannerType);
				DisplayResult = null;
			}
		}

		private void BannerHidded()
		{
			if (debug)
			{
				Debug.Log(string.Concat(this, "Baner Hidden"));
				ScreenWriter.Write(string.Concat(this, "Baner Hidden"));
			}
		}

		public void HideBanner()
		{
			Advertisement.Banner.Hide(true);
		}
	}
}
