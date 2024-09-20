using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GleyMobileAds;
using UnityEngine;
using UnityEngine.Events;

public class Advertisements : MonoBehaviour
{
	private const string userConsent = "UserConsent";

	private const string removeAds = "RemoveAds";

	private static Advertisements instance;

	private static bool initialized;

	private static GameObject go;

	private List<Advertiser> allAdvertisers = new List<Advertiser>();

	private List<Advertiser> bannerAdvertisers = new List<Advertiser>();

	private List<Advertiser> interstitialAdvertisers = new List<Advertiser>();

	private List<Advertiser> rewardedAdvertisers = new List<Advertiser>();

	private SupportedMediation bannerMediation;

	private SupportedMediation interstitialMediation;

	private SupportedMediation rewardedMediation;

	internal bool debug;

	internal AdSettings adSettings;

	public static Advertisements Instance
	{
		get
		{
			if (instance == null)
			{
				go = new GameObject();
				go.name = "MobieAdsScripts";
				Object.DontDestroyOnLoad(go);
				instance = go.AddComponent<Advertisements>();
			}
			return instance;
		}
	}

	public void SetUserConsent(bool accept)
	{
		if (accept)
		{
			PlayerPrefs.SetInt("UserConsent", 1);
		}
		else
		{
			PlayerPrefs.SetInt("UserConsent", 2);
		}
		if (initialized)
		{
			UpdateUserConsent();
		}
	}

	public GDPRConsent GetConsent()
	{
		if (!UserConsentWasSet())
		{
			return GDPRConsent.Unset;
		}
		return (GDPRConsent)PlayerPrefs.GetInt("UserConsent");
	}

	public bool UserConsentWasSet()
	{
		return PlayerPrefs.HasKey("UserConsent");
	}

	public void RemoveAds(bool remove)
	{
		if (remove)
		{
			PlayerPrefs.SetInt("RemoveAds", 1);
			HideBanner();
		}
		else
		{
			PlayerPrefs.SetInt("RemoveAds", 0);
		}
	}

	public bool CanShowAds()
	{
		if (!PlayerPrefs.HasKey("RemoveAds"))
		{
			return true;
		}
		if (PlayerPrefs.GetInt("RemoveAds") == 0)
		{
			return true;
		}
		return false;
	}

	public void Initialize()
	{
		if (initialized)
		{
			return;
		}
		adSettings = Resources.Load<AdSettings>("AdSettingsData");
		if (adSettings == null)
		{
			Debug.LogError("Gley Ads Plugin is not properly configured. Go to Window->Gley->Ads to set up the plugin. See the documentation");
			return;
		}
		bannerMediation = adSettings.bannerMediation;
		interstitialMediation = adSettings.interstitialMediation;
		rewardedMediation = adSettings.rewardedMediation;
		debug = adSettings.debugMode;
		initialized = true;
		if (adSettings.advertiserSettings.First((AdvertiserSettings cond) => cond.advertiser == SupportedAdvertisers.Admob).useSDK)
		{
			allAdvertisers.Add(new Advertiser(go.AddComponent<CustomAdmob>(), adSettings.GetAdvertiserSettings(SupportedAdvertisers.Admob), adSettings.GetPlaftormSettings(SupportedAdvertisers.Admob)));
		}
		if (adSettings.advertiserSettings.First((AdvertiserSettings cond) => cond.advertiser == SupportedAdvertisers.Vungle).useSDK)
		{
			allAdvertisers.Add(new Advertiser(go.AddComponent<CustomVungle>(), adSettings.GetAdvertiserSettings(SupportedAdvertisers.Vungle), adSettings.GetPlaftormSettings(SupportedAdvertisers.Vungle)));
		}
		if (adSettings.advertiserSettings.First((AdvertiserSettings cond) => cond.advertiser == SupportedAdvertisers.AdColony).useSDK)
		{
			allAdvertisers.Add(new Advertiser(go.AddComponent<CustomAdColony>(), adSettings.GetAdvertiserSettings(SupportedAdvertisers.AdColony), adSettings.GetPlaftormSettings(SupportedAdvertisers.AdColony)));
		}
		if (adSettings.advertiserSettings.First((AdvertiserSettings cond) => cond.advertiser == SupportedAdvertisers.Chartboost).useSDK)
		{
			allAdvertisers.Add(new Advertiser(go.AddComponent<CustomChartboost>(), adSettings.GetAdvertiserSettings(SupportedAdvertisers.Chartboost), adSettings.GetPlaftormSettings(SupportedAdvertisers.Chartboost)));
		}
		if (adSettings.advertiserSettings.First((AdvertiserSettings cond) => cond.advertiser == SupportedAdvertisers.Unity).useSDK)
		{
			allAdvertisers.Add(new Advertiser(go.AddComponent<CustomUnityAds>(), adSettings.GetAdvertiserSettings(SupportedAdvertisers.Unity), adSettings.GetPlaftormSettings(SupportedAdvertisers.Unity)));
		}
		if (adSettings.advertiserSettings.First((AdvertiserSettings cond) => cond.advertiser == SupportedAdvertisers.Heyzap).useSDK)
		{
			allAdvertisers.Add(new Advertiser(go.AddComponent<CustomHeyzap>(), adSettings.GetAdvertiserSettings(SupportedAdvertisers.Heyzap), adSettings.GetPlaftormSettings(SupportedAdvertisers.Heyzap)));
		}
		if (adSettings.advertiserSettings.First((AdvertiserSettings cond) => cond.advertiser == SupportedAdvertisers.AppLovin).useSDK)
		{
			allAdvertisers.Add(new Advertiser(go.AddComponent<CustomAppLovin>(), adSettings.GetAdvertiserSettings(SupportedAdvertisers.AppLovin), adSettings.GetPlaftormSettings(SupportedAdvertisers.AppLovin)));
		}
		if (debug)
		{
			ScreenWriter.Write("User GDPR consent is set to: " + GetConsent());
		}
		for (int i = 0; i < allAdvertisers.Count; i++)
		{
			allAdvertisers[i].advertiserScript.InitializeAds(GetConsent(), allAdvertisers[i].platformSettings);
		}
		ApplySettings();
		LoadFile();
	}

	private void UpdateUserConsent()
	{
		for (int i = 0; i < allAdvertisers.Count; i++)
		{
			allAdvertisers[i].advertiserScript.UpdateConsent(GetConsent());
		}
	}

	public void ShowInterstitial(UnityAction InterstitialClosed = null)
	{
		if (!CanShowAds())
		{
			return;
		}
		ICustomAds interstitialAdvertiser = GetInterstitialAdvertiser();
		if (interstitialAdvertiser != null)
		{
			if (debug)
			{
				Debug.Log("Interstitial loaded from " + interstitialAdvertiser);
				ScreenWriter.Write("Interstitial loaded from " + interstitialAdvertiser);
			}
			interstitialAdvertiser.ShowInterstitial(InterstitialClosed);
		}
	}

	public void ShowInterstitial(UnityAction<string> InterstitialClosed)
	{
		if (!CanShowAds())
		{
			return;
		}
		ICustomAds interstitialAdvertiser = GetInterstitialAdvertiser();
		if (interstitialAdvertiser != null)
		{
			if (debug)
			{
				Debug.Log("Interstitial loaded from " + interstitialAdvertiser);
				ScreenWriter.Write("Interstitial loaded from " + interstitialAdvertiser);
			}
			interstitialAdvertiser.ShowInterstitial(InterstitialClosed);
		}
	}

	public void ShowInterstitial(SupportedAdvertisers advertiser, UnityAction InterstitialClosed = null)
	{
		if (!CanShowAds())
		{
			return;
		}
		Advertiser advertiser2 = GetInterstitialAdvertisers().First((Advertiser cond) => cond.advertiser == advertiser);
		if (advertiser2.advertiserScript.IsInterstitialAvailable())
		{
			if (debug)
			{
				Debug.Log(string.Concat("Interstitial from ", advertiser, " is available"));
				ScreenWriter.Write(string.Concat("Interstitial from ", advertiser, " is available"));
			}
			advertiser2.advertiserScript.ShowInterstitial(InterstitialClosed);
		}
		else
		{
			if (debug)
			{
				Debug.Log(string.Concat("Interstitial from ", advertiser, " is NOT available"));
				ScreenWriter.Write(string.Concat("Interstitial from ", advertiser, " is NOT available"));
			}
			ShowInterstitial(InterstitialClosed);
		}
	}

	private ICustomAds GetInterstitialAdvertiser()
	{
		if (interstitialMediation == SupportedMediation.OrderMediation)
		{
			return UseOrder(interstitialAdvertisers, SupportedAdTypes.Interstitial);
		}
		return UsePercent(interstitialAdvertisers, SupportedAdTypes.Interstitial);
	}

	public void ShowRewardedVideo(UnityAction<bool> CompleteMethod)
	{
		ICustomAds customAds = null;
		customAds = ((rewardedMediation != 0) ? UsePercent(rewardedAdvertisers, SupportedAdTypes.Rewarded) : UseOrder(rewardedAdvertisers, SupportedAdTypes.Rewarded));
		if (customAds != null)
		{
			if (debug)
			{
				Debug.Log("Rewarded video loaded from " + customAds);
				ScreenWriter.Write("Rewarded video loaded from " + customAds);
			}
			customAds.ShowRewardVideo(CompleteMethod);
		}
	}

	public void ShowRewardedVideo(UnityAction<bool, string> CompleteMethod)
	{
		ICustomAds customAds = null;
		customAds = ((rewardedMediation != 0) ? UsePercent(rewardedAdvertisers, SupportedAdTypes.Rewarded) : UseOrder(rewardedAdvertisers, SupportedAdTypes.Rewarded));
		if (customAds != null)
		{
			if (debug)
			{
				Debug.Log("Rewarded video loaded from " + customAds);
				ScreenWriter.Write("Rewarded video loaded from " + customAds);
			}
			customAds.ShowRewardVideo(CompleteMethod);
		}
	}

	public void ShowRewardedVideo(SupportedAdvertisers advertiser, UnityAction<bool> CompleteMethod)
	{
		Advertiser advertiser2 = GetRewardedAdvertisers().First((Advertiser cond) => cond.advertiser == advertiser);
		if (advertiser2.advertiserScript.IsRewardVideoAvailable())
		{
			if (debug)
			{
				Debug.Log(string.Concat("Rewarded Video from ", advertiser, " is available"));
				ScreenWriter.Write(string.Concat("Rewarded Video from ", advertiser, " is available"));
			}
			advertiser2.advertiserScript.ShowRewardVideo(CompleteMethod);
		}
		else
		{
			if (debug)
			{
				Debug.Log(string.Concat("Rewarded Video from ", advertiser, " is NOT available"));
				ScreenWriter.Write(string.Concat("Rewarded Video from ", advertiser, " is NOT available"));
			}
			ShowRewardedVideo(CompleteMethod);
		}
	}

	public void ShowBanner(BannerPosition position, BannerType bannerType = BannerType.SmartBanner)
	{
		if (CanShowAds())
		{
			for (int i = 0; i < bannerAdvertisers.Count; i++)
			{
				allAdvertisers[i].advertiserScript.ResetBannerUsage();
			}
			LoadBanner(position, bannerType);
		}
	}

	private void LoadBanner(BannerPosition position, BannerType bannerType)
	{
		ICustomAds customAds = null;
		customAds = ((bannerMediation != 0) ? UsePercent(bannerAdvertisers, SupportedAdTypes.Banner) : UseOrder(bannerAdvertisers, SupportedAdTypes.Banner));
		if (customAds != null)
		{
			if (debug)
			{
				Debug.Log("Banner loaded from " + customAds);
				ScreenWriter.Write("Banner loaded from " + customAds);
			}
			customAds.ShowBanner(position, bannerType, BannerDisplayedResult);
		}
		else if (debug)
		{
			Debug.Log("No Banners Available");
			ScreenWriter.Write("No Banners Available");
		}
	}

	private void BannerDisplayedResult(bool succesfullyDisplayed, BannerPosition position, BannerType bannerType)
	{
		if (!succesfullyDisplayed)
		{
			if (debug)
			{
				Debug.Log("Banner failed to load -> trying another advertiser");
				ScreenWriter.Write("Banner failed to load -> trying another advertiser");
			}
			LoadBanner(position, bannerType);
		}
		else if (debug)
		{
			Debug.Log("Banner is on screen");
			ScreenWriter.Write("Banner is on screen");
		}
	}

	public void HideBanner()
	{
		for (int i = 0; i < allAdvertisers.Count; i++)
		{
			allAdvertisers[i].advertiserScript.HideBanner();
		}
	}

	private ICustomAds UsePercent(List<Advertiser> advertisers, SupportedAdTypes adType)
	{
		List<Advertiser> list = new List<Advertiser>();
		List<int> list2 = new List<int>();
		int num = 0;
		for (int i = 0; i < advertisers.Count; i++)
		{
			switch (adType)
			{
			case SupportedAdTypes.Banner:
				if (advertisers[i].advertiserScript.IsBannerAvailable() && !advertisers[i].advertiserScript.BannerAlreadyUsed())
				{
					list.Add(advertisers[i]);
					num += advertisers[i].mediationSettings.bannerSettings.Weight;
					list2.Add(num);
				}
				break;
			case SupportedAdTypes.Interstitial:
				if (advertisers[i].advertiserScript.IsInterstitialAvailable())
				{
					list.Add(advertisers[i]);
					num += advertisers[i].mediationSettings.interstitialSettings.Weight;
					list2.Add(num);
				}
				break;
			case SupportedAdTypes.Rewarded:
				if (advertisers[i].advertiserScript.IsRewardVideoAvailable())
				{
					list.Add(advertisers[i]);
					num += advertisers[i].mediationSettings.rewardedSettings.Weight;
					list2.Add(num);
				}
				break;
			}
		}
		int num2 = Random.Range(0, num);
		if (debug)
		{
			for (int j = 0; j < list.Count; j++)
			{
				ScreenWriter.Write(string.Concat(list[j].advertiser, " weight ", list2[j]));
				Debug.Log(string.Concat(list[j].advertiser, " weight ", list2[j]));
			}
		}
		for (int k = 0; k < list2.Count; k++)
		{
			if (num2 < list2[k])
			{
				if (debug)
				{
					ScreenWriter.Write(string.Concat("SHOW AD FROM: ", list[k].advertiser, " weight ", num2));
					Debug.Log(string.Concat("SHOW AD FROM: ", list[k].advertiser, " weight ", num2));
				}
				return list[k].advertiserScript;
			}
		}
		return null;
	}

	private ICustomAds UseOrder(List<Advertiser> advertisers, SupportedAdTypes adType)
	{
		for (int i = 0; i < advertisers.Count; i++)
		{
			switch (adType)
			{
			case SupportedAdTypes.Banner:
				if (advertisers[i].advertiserScript.IsBannerAvailable() && !advertisers[i].advertiserScript.BannerAlreadyUsed())
				{
					return advertisers[i].advertiserScript;
				}
				break;
			case SupportedAdTypes.Interstitial:
				if (advertisers[i].advertiserScript.IsInterstitialAvailable())
				{
					return advertisers[i].advertiserScript;
				}
				break;
			case SupportedAdTypes.Rewarded:
				if (advertisers[i].advertiserScript.IsRewardVideoAvailable())
				{
					return advertisers[i].advertiserScript;
				}
				break;
			}
		}
		return null;
	}

	private void LoadFile()
	{
		if (adSettings.externalFileUrl != string.Empty && (adSettings.externalFileUrl.StartsWith("http") || adSettings.externalFileUrl.StartsWith("file")))
		{
			StartCoroutine(LoadFile(adSettings.externalFileUrl));
		}
	}

	private IEnumerator LoadFile(string url)
	{
		Debug.Log(url);
		WWW www = new WWW(url);
		yield return www;
		if (string.IsNullOrEmpty(www.error))
		{
			try
			{
				string message = www.text.Trim();
				Debug.Log(message);
				AdOrder adOrder = JsonUtility.FromJson<AdOrder>(www.text);
				UpdateSettings(adOrder);
				yield break;
			}
			catch
			{
				if (debug)
				{
					Debug.LogWarning("File was not in correct format");
					ScreenWriter.Write("File was not in correct format");
				}
				yield break;
			}
		}
		if (debug)
		{
			Debug.LogWarning("Could not download config file " + www.error);
			ScreenWriter.Write("Could not download config file " + www.error);
		}
	}

	private void UpdateSettings(AdOrder adOrder)
	{
		bannerMediation = adOrder.bannerMediation;
		interstitialMediation = adOrder.interstitialMediation;
		rewardedMediation = adOrder.rewardedMediation;
		for (int i = 0; i < adOrder.advertisers.Count; i++)
		{
			for (int j = 0; j < allAdvertisers.Count; j++)
			{
				if (allAdvertisers[j].mediationSettings.GetAdvertiser() == adOrder.advertisers[i].GetAdvertiser())
				{
					allAdvertisers[j].mediationSettings = adOrder.advertisers[i];
				}
			}
		}
		if (debug)
		{
			Debug.Log("File Config Loaded");
			ScreenWriter.Write("File Config Loaded");
		}
		ApplySettings();
	}

	private void ApplySettings()
	{
		if (debug)
		{
			Debug.Log("Banner mediation type: " + bannerMediation);
			ScreenWriter.Write("Banner mediation type: " + bannerMediation);
			Debug.Log("Interstitial mediation type: " + interstitialMediation);
			ScreenWriter.Write("Interstitial mediation type: " + interstitialMediation);
			Debug.Log("Rewarded mediation type: " + rewardedMediation);
			ScreenWriter.Write("Rewarded mediation type: " + rewardedMediation);
		}
		bannerAdvertisers = new List<Advertiser>();
		interstitialAdvertisers = new List<Advertiser>();
		rewardedAdvertisers = new List<Advertiser>();
		for (int i = 0; i < allAdvertisers.Count; i++)
		{
			if (bannerMediation == SupportedMediation.OrderMediation)
			{
				if (allAdvertisers[i].mediationSettings.bannerSettings.Order != 0)
				{
					bannerAdvertisers.Add(allAdvertisers[i]);
				}
			}
			else if (allAdvertisers[i].mediationSettings.bannerSettings.Weight != 0)
			{
				bannerAdvertisers.Add(allAdvertisers[i]);
			}
			if (interstitialMediation == SupportedMediation.OrderMediation)
			{
				if (allAdvertisers[i].mediationSettings.interstitialSettings.Order != 0)
				{
					interstitialAdvertisers.Add(allAdvertisers[i]);
				}
			}
			else if (allAdvertisers[i].mediationSettings.interstitialSettings.Weight != 0)
			{
				interstitialAdvertisers.Add(allAdvertisers[i]);
			}
			if (rewardedMediation == SupportedMediation.OrderMediation)
			{
				if (allAdvertisers[i].mediationSettings.rewardedSettings.Order != 0)
				{
					rewardedAdvertisers.Add(allAdvertisers[i]);
				}
			}
			else if (allAdvertisers[i].mediationSettings.rewardedSettings.Weight != 0)
			{
				rewardedAdvertisers.Add(allAdvertisers[i]);
			}
		}
		if (bannerMediation == SupportedMediation.OrderMediation)
		{
			bannerAdvertisers = bannerAdvertisers.OrderBy((Advertiser cond) => cond.mediationSettings.bannerSettings.Order).ToList();
		}
		else
		{
			bannerAdvertisers = bannerAdvertisers.OrderByDescending((Advertiser cond) => cond.mediationSettings.bannerSettings.Weight).ToList();
		}
		if (interstitialMediation == SupportedMediation.OrderMediation)
		{
			interstitialAdvertisers = interstitialAdvertisers.OrderBy((Advertiser cond) => cond.mediationSettings.interstitialSettings.Order).ToList();
		}
		else
		{
			interstitialAdvertisers = interstitialAdvertisers.OrderByDescending((Advertiser cond) => cond.mediationSettings.interstitialSettings.Weight).ToList();
		}
		if (rewardedMediation == SupportedMediation.OrderMediation)
		{
			rewardedAdvertisers = rewardedAdvertisers.OrderBy((Advertiser cond) => cond.mediationSettings.rewardedSettings.Order).ToList();
		}
		else
		{
			rewardedAdvertisers = rewardedAdvertisers.OrderByDescending((Advertiser cond) => cond.mediationSettings.rewardedSettings.Weight).ToList();
		}
	}

	public bool IsRewardVideoAvailable()
	{
		for (int i = 0; i < rewardedAdvertisers.Count; i++)
		{
			if (rewardedAdvertisers[i].advertiserScript.IsRewardVideoAvailable())
			{
				return true;
			}
		}
		return false;
	}

	public bool IsInterstitialAvailable()
	{
		if (!CanShowAds())
		{
			return false;
		}
		for (int i = 0; i < interstitialAdvertisers.Count; i++)
		{
			if (interstitialAdvertisers[i].advertiserScript.IsInterstitialAvailable())
			{
				return true;
			}
		}
		return false;
	}

	public bool IsBannerAvailable()
	{
		if (!CanShowAds())
		{
			return false;
		}
		for (int i = 0; i < bannerAdvertisers.Count; i++)
		{
			if (bannerAdvertisers[i].advertiserScript.IsBannerAvailable())
			{
				return true;
			}
		}
		return false;
	}

	private void DisplayAdvertisers(List<Advertiser> advertisers)
	{
		for (int i = 0; i < advertisers.Count; i++)
		{
			Debug.Log(string.Concat(advertisers[i].advertiser, " banner order ", advertisers[i].mediationSettings.bannerSettings.Order, " interstitial order ", advertisers[i].mediationSettings.interstitialSettings.Order, " rewarded order ", advertisers[i].mediationSettings.interstitialSettings.Order));
		}
	}

	public List<Advertiser> GetAllAdvertisers()
	{
		return allAdvertisers;
	}

	public List<Advertiser> GetBannerAdvertisers()
	{
		return bannerAdvertisers;
	}

	public List<Advertiser> GetInterstitialAdvertisers()
	{
		return interstitialAdvertisers;
	}

	public List<Advertiser> GetRewardedAdvertisers()
	{
		return rewardedAdvertisers;
	}
}
