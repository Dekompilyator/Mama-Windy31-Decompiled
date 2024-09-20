using System;

namespace GleyMobileAds
{
	[Serializable]
	public class AdTypeSettings
	{
		public SupportedAdTypes adType;

		public int orderAndroid;

		public int orderiOS;

		public int orderWindows;

		public int weightAndroid;

		public int weightiOS;

		public int weightWindows;

		private int percentAndroid;

		private int percentiOS;

		private int percentWindows;

		public int Order
		{
			get
			{
				return orderAndroid;
			}
			set
			{
				orderAndroid = (orderiOS = (orderWindows = value));
			}
		}

		public int Percent
		{
			get
			{
				return percentAndroid;
			}
			set
			{
				percentAndroid = (percentiOS = (percentWindows = value));
			}
		}

		public int Weight
		{
			get
			{
				return weightAndroid;
			}
			set
			{
				weightAndroid = (weightiOS = (weightWindows = value));
			}
		}

		public AdTypeSettings(SupportedAdTypes type)
		{
			adType = type;
		}

		public AdTypeSettings(AdTypeSettings settings)
		{
			adType = settings.adType;
			orderAndroid = settings.orderAndroid;
			orderiOS = settings.orderiOS;
			orderWindows = settings.orderWindows;
			weightAndroid = settings.weightAndroid;
			weightiOS = settings.weightiOS;
			weightWindows = settings.weightWindows;
			percentAndroid = settings.percentAndroid;
			percentiOS = settings.percentiOS;
			percentWindows = settings.percentWindows;
		}
	}
}
