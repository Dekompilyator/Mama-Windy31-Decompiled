using GleyMobileAds;
using UnityEngine;

public class TestAds : MonoBehaviour
{
	private float buttonWidth = Screen.width / 4;

	private float buttonHeight = Screen.height / 13;

	private int nrOfButtons = 4;

	private bool showDetails;

	private bool bottom = true;

	private void Start()
	{
		if (Advertisements.Instance.UserConsentWasSet())
		{
			Advertisements.Instance.Initialize();
		}
	}

	private void OnGUI()
	{
		if (!Advertisements.Instance.UserConsentWasSet())
		{
			GUI.Label(new Rect(0f, 0f, Screen.width, Screen.height), "Do you prefer random ads in your app or ads relevant to you? If you choose Random no personalized data will be collected. If you choose personal all data collected will be used only to serve ads relevant to you.");
			if (GUI.Button(new Rect(buttonWidth, (float)Screen.height - 5f * buttonHeight, buttonWidth, buttonHeight), "Personalized"))
			{
				Advertisements.Instance.SetUserConsent(true);
				Advertisements.Instance.Initialize();
			}
			if (GUI.Button(new Rect(2f * buttonWidth, (float)Screen.height - 5f * buttonHeight, buttonWidth, buttonHeight), "Random"))
			{
				Advertisements.Instance.SetUserConsent(false);
				Advertisements.Instance.Initialize();
			}
			return;
		}
		if (GUI.Button(new Rect(0f, (float)Screen.height - buttonHeight, buttonWidth, buttonHeight), "Show Details"))
		{
			showDetails = !showDetails;
		}
		if (GUI.Button(new Rect(buttonWidth, (float)Screen.height - buttonHeight, buttonWidth, buttonHeight), "Consent:\nTrue"))
		{
			Advertisements.Instance.SetUserConsent(true);
		}
		if (GUI.Button(new Rect(2f * buttonWidth, (float)Screen.height - buttonHeight, buttonWidth, buttonHeight), "Consent:\nFalse"))
		{
			Advertisements.Instance.SetUserConsent(false);
		}
		if (GUI.Button(new Rect(buttonWidth, (float)Screen.height - 2f * buttonHeight, buttonWidth, buttonHeight), "Show Ads"))
		{
			Advertisements.Instance.RemoveAds(false);
		}
		if (GUI.Button(new Rect(2f * buttonWidth, (float)Screen.height - 2f * buttonHeight, buttonWidth, buttonHeight), "Remove Ads"))
		{
			Advertisements.Instance.RemoveAds(true);
		}
		if (Advertisements.Instance.IsRewardVideoAvailable())
		{
			Debug.Log("AVAIBLE");
			if (GUI.Button(new Rect(0f, 0f, buttonWidth, buttonHeight), "Show Rewarded"))
			{
				Advertisements.Instance.ShowRewardedVideo(CompleteMethod);
			}
		}
		if (Advertisements.Instance.IsInterstitialAvailable() && GUI.Button(new Rect(1f * buttonWidth, 0f, buttonWidth, buttonHeight), "Show Interstitial"))
		{
			Advertisements.Instance.ShowInterstitial(InterstitialClosed);
		}
		if (Advertisements.Instance.IsBannerAvailable())
		{
			if (GUI.Button(new Rect(2f * buttonWidth, 0f, buttonWidth, buttonHeight), "Show Banner"))
			{
				Advertisements.Instance.ShowBanner(BannerPosition.BOTTOM);
			}
			if (GUI.Button(new Rect(3f * buttonWidth, 0f, buttonWidth, buttonHeight), "Hide Banner"))
			{
				Advertisements.Instance.HideBanner();
			}
			if (GUI.Button(new Rect(3f * buttonWidth, buttonHeight, buttonWidth, buttonHeight), "Switch Banner"))
			{
				if (bottom)
				{
					Advertisements.Instance.ShowBanner(BannerPosition.BOTTOM);
				}
				else
				{
					Advertisements.Instance.ShowBanner(BannerPosition.TOP);
				}
				bottom = !bottom;
			}
		}
		if (!showDetails)
		{
			return;
		}
		int num = 0;
		for (int i = 0; i < Advertisements.Instance.GetRewardedAdvertisers().Count; i++)
		{
			if (Advertisements.Instance.GetRewardedAdvertisers()[i].advertiserScript.IsRewardVideoAvailable())
			{
				if (GUI.Button(new Rect((float)(num % nrOfButtons) * buttonWidth, (float)(2 + num / nrOfButtons) * buttonHeight, buttonWidth, buttonHeight), string.Concat(Advertisements.Instance.GetRewardedAdvertisers()[i].advertiser, " Rewarded")))
				{
					Advertisements.Instance.GetRewardedAdvertisers()[i].advertiserScript.ShowRewardVideo(CompleteMethod);
				}
				num++;
			}
		}
		num = 0;
		for (int j = 0; j < Advertisements.Instance.GetInterstitialAdvertisers().Count; j++)
		{
			if (Advertisements.Instance.GetInterstitialAdvertisers()[j].advertiserScript.IsInterstitialAvailable() && Advertisements.Instance.CanShowAds())
			{
				if (GUI.Button(new Rect((float)(num % nrOfButtons) * buttonWidth, (float)(5 + num / nrOfButtons) * buttonHeight, buttonWidth, buttonHeight), string.Concat(Advertisements.Instance.GetInterstitialAdvertisers()[j].advertiser, " Interstitial")))
				{
					Advertisements.Instance.GetInterstitialAdvertisers()[j].advertiserScript.ShowInterstitial(InterstitialClosed);
				}
				num++;
			}
		}
		num = 0;
		for (int k = 0; k < Advertisements.Instance.GetBannerAdvertisers().Count; k++)
		{
			if (Advertisements.Instance.GetBannerAdvertisers()[k].advertiserScript.IsBannerAvailable() && Advertisements.Instance.CanShowAds())
			{
				if (GUI.Button(new Rect((float)(num % nrOfButtons) * buttonWidth, (float)(8 + num / nrOfButtons) * buttonHeight, buttonWidth, buttonHeight), string.Concat(Advertisements.Instance.GetBannerAdvertisers()[k].advertiser, " Banner")))
				{
					Advertisements.Instance.GetBannerAdvertisers()[k].advertiserScript.ShowBanner(BannerPosition.BOTTOM, BannerType.Banner, null);
				}
				num++;
			}
		}
	}

	private void InterstitialClosed(string advertiser)
	{
		if (Advertisements.Instance.debug)
		{
			Debug.Log("Interstitial closed from: " + advertiser + " -> Resume Game ");
			ScreenWriter.Write("Interstitial closed from: " + advertiser + " -> Resume Game ");
		}
	}

	private void CompleteMethod(bool completed, string advertiser)
	{
		if (Advertisements.Instance.debug)
		{
			Debug.Log("Closed rewarded from: " + advertiser + " -> Completed " + completed);
			ScreenWriter.Write("Closed rewarded from: " + advertiser + " -> Completed " + completed);
			if (!completed)
			{
			}
		}
	}
}
