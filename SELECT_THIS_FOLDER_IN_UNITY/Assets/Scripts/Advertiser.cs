using System.Collections.Generic;
using GleyMobileAds;

public class Advertiser
{
	public ICustomAds advertiserScript;

	public SupportedAdvertisers advertiser;

	public MediationSettings mediationSettings;

	public List<PlatformSettings> platformSettings;

	public Advertiser(ICustomAds advertiserScript, MediationSettings mediationSettings, List<PlatformSettings> platformSettings)
	{
		this.advertiserScript = advertiserScript;
		this.mediationSettings = mediationSettings;
		this.platformSettings = platformSettings;
		advertiser = mediationSettings.advertiser;
	}
}
