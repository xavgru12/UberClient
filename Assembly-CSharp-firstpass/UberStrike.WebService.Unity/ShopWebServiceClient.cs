using Cmune.DataCenter.Common.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using UberStrike.Core.Models.Views;
using UberStrike.Core.Serialization;
using UberStrike.Core.Types;
using UnityEngine;

namespace UberStrike.WebService.Unity
{
	public static class ShopWebServiceClient
	{
		public static Coroutine GetShop(Action<UberStrikeItemShopClientView> callback, Action<Exception> handler)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IShopWebServiceContract", "ShopWebService", "GetShop", memoryStream.ToArray(), delegate(byte[] data)
				{
					if (callback != null)
					{
						callback(UberStrikeItemShopClientViewProxy.Deserialize(new MemoryStream(data)));
					}
				}, handler));
			}
		}

		public static Coroutine BuyItem(int itemId, string authToken, UberStrikeCurrencyType currencyType, BuyingDurationType durationType, UberstrikeItemType itemType, BuyingLocationType marketLocation, BuyingRecommendationType recommendationType, Action<int> callback, Action<Exception> handler)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Int32Proxy.Serialize(memoryStream, itemId);
				StringProxy.Serialize(memoryStream, authToken);
				EnumProxy<UberStrikeCurrencyType>.Serialize(memoryStream, currencyType);
				EnumProxy<BuyingDurationType>.Serialize(memoryStream, durationType);
				EnumProxy<UberstrikeItemType>.Serialize(memoryStream, itemType);
				EnumProxy<BuyingLocationType>.Serialize(memoryStream, marketLocation);
				EnumProxy<BuyingRecommendationType>.Serialize(memoryStream, recommendationType);
				return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IShopWebServiceContract", "ShopWebService", "BuyItem", memoryStream.ToArray(), delegate(byte[] data)
				{
					if (callback != null)
					{
						callback(Int32Proxy.Deserialize(new MemoryStream(data)));
					}
				}, handler));
			}
		}

		public static Coroutine BuyPack(int itemId, string authToken, PackType packType, UberStrikeCurrencyType currencyType, UberstrikeItemType itemType, BuyingLocationType marketLocation, BuyingRecommendationType recommendationType, Action<int> callback, Action<Exception> handler)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Int32Proxy.Serialize(memoryStream, itemId);
				StringProxy.Serialize(memoryStream, authToken);
				EnumProxy<PackType>.Serialize(memoryStream, packType);
				EnumProxy<UberStrikeCurrencyType>.Serialize(memoryStream, currencyType);
				EnumProxy<UberstrikeItemType>.Serialize(memoryStream, itemType);
				EnumProxy<BuyingLocationType>.Serialize(memoryStream, marketLocation);
				EnumProxy<BuyingRecommendationType>.Serialize(memoryStream, recommendationType);
				return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IShopWebServiceContract", "ShopWebService", "BuyPack", memoryStream.ToArray(), delegate(byte[] data)
				{
					if (callback != null)
					{
						callback(Int32Proxy.Deserialize(new MemoryStream(data)));
					}
				}, handler));
			}
		}

		public static Coroutine GetBundles(ChannelType channel, Action<List<BundleView>> callback, Action<Exception> handler)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				EnumProxy<ChannelType>.Serialize(memoryStream, channel);
				return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IShopWebServiceContract", "ShopWebService", "GetBundles", memoryStream.ToArray(), delegate(byte[] data)
				{
					if (callback != null)
					{
						callback(ListProxy<BundleView>.Deserialize(new MemoryStream(data), BundleViewProxy.Deserialize));
					}
				}, handler));
			}
		}

		public static Coroutine BuyBundle(string authToken, int bundleId, ChannelType channel, string hashedReceipt, Action<bool> callback, Action<Exception> handler)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				StringProxy.Serialize(memoryStream, authToken);
				Int32Proxy.Serialize(memoryStream, bundleId);
				EnumProxy<ChannelType>.Serialize(memoryStream, channel);
				StringProxy.Serialize(memoryStream, hashedReceipt);
				return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IShopWebServiceContract", "ShopWebService", "BuyBundle", memoryStream.ToArray(), delegate(byte[] data)
				{
					if (callback != null)
					{
						callback(BooleanProxy.Deserialize(new MemoryStream(data)));
					}
				}, handler));
			}
		}

		public static Coroutine BuyBundleSteam(int bundleId, string steamId, string authToken, Action<bool> callback, Action<Exception> handler)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Int32Proxy.Serialize(memoryStream, bundleId);
				StringProxy.Serialize(memoryStream, steamId);
				StringProxy.Serialize(memoryStream, authToken);
				return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IShopWebServiceContract", "ShopWebService", "BuyBundleSteam", memoryStream.ToArray(), delegate(byte[] data)
				{
					if (callback != null)
					{
						callback(BooleanProxy.Deserialize(new MemoryStream(data)));
					}
				}, handler));
			}
		}

		public static Coroutine FinishBuyBundleSteam(string orderId, Action<bool> callback, Action<Exception> handler)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				StringProxy.Serialize(memoryStream, orderId);
				return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IShopWebServiceContract", "ShopWebService", "FinishBuyBundleSteam", memoryStream.ToArray(), delegate(byte[] data)
				{
					if (callback != null)
					{
						callback(BooleanProxy.Deserialize(new MemoryStream(data)));
					}
				}, handler));
			}
		}

		public static Coroutine VerifyReceipt(string hashedReceipt, Action<bool> callback, Action<Exception> handler)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				StringProxy.Serialize(memoryStream, hashedReceipt);
				return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IShopWebServiceContract", "ShopWebService", "VerifyReceipt", memoryStream.ToArray(), delegate(byte[] data)
				{
					if (callback != null)
					{
						callback(BooleanProxy.Deserialize(new MemoryStream(data)));
					}
				}, handler));
			}
		}

		public static Coroutine UseConsumableItem(string authToken, int itemId, Action<bool> callback, Action<Exception> handler)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				StringProxy.Serialize(memoryStream, authToken);
				Int32Proxy.Serialize(memoryStream, itemId);
				return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IShopWebServiceContract", "ShopWebService", "UseConsumableItem", memoryStream.ToArray(), delegate(byte[] data)
				{
					if (callback != null)
					{
						callback(BooleanProxy.Deserialize(new MemoryStream(data)));
					}
				}, handler));
			}
		}

		public static Coroutine GetAllMysteryBoxs(Action<List<MysteryBoxUnityView>> callback, Action<Exception> handler)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IShopWebServiceContract", "ShopWebService", "GetAllMysteryBoxs_1", memoryStream.ToArray(), delegate(byte[] data)
				{
					if (callback != null)
					{
						callback(ListProxy<MysteryBoxUnityView>.Deserialize(new MemoryStream(data), MysteryBoxUnityViewProxy.Deserialize));
					}
				}, handler));
			}
		}

		public static Coroutine GetAllMysteryBoxs(BundleCategoryType bundleCategoryType, Action<List<MysteryBoxUnityView>> callback, Action<Exception> handler)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				EnumProxy<BundleCategoryType>.Serialize(memoryStream, bundleCategoryType);
				return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IShopWebServiceContract", "ShopWebService", "GetAllMysteryBoxs_2", memoryStream.ToArray(), delegate(byte[] data)
				{
					if (callback != null)
					{
						callback(ListProxy<MysteryBoxUnityView>.Deserialize(new MemoryStream(data), MysteryBoxUnityViewProxy.Deserialize));
					}
				}, handler));
			}
		}

		public static Coroutine GetMysteryBox(int mysteryBoxId, Action<MysteryBoxUnityView> callback, Action<Exception> handler)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Int32Proxy.Serialize(memoryStream, mysteryBoxId);
				return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IShopWebServiceContract", "ShopWebService", "GetMysteryBox", memoryStream.ToArray(), delegate(byte[] data)
				{
					if (callback != null)
					{
						callback(MysteryBoxUnityViewProxy.Deserialize(new MemoryStream(data)));
					}
				}, handler));
			}
		}

		public static Coroutine RollMysteryBox(string authToken, int mysteryBoxId, ChannelType channel, Action<List<MysteryBoxWonItemUnityView>> callback, Action<Exception> handler)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				StringProxy.Serialize(memoryStream, authToken);
				Int32Proxy.Serialize(memoryStream, mysteryBoxId);
				EnumProxy<ChannelType>.Serialize(memoryStream, channel);
				return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IShopWebServiceContract", "ShopWebService", "RollMysteryBox", memoryStream.ToArray(), delegate(byte[] data)
				{
					if (callback != null)
					{
						callback(ListProxy<MysteryBoxWonItemUnityView>.Deserialize(new MemoryStream(data), MysteryBoxWonItemUnityViewProxy.Deserialize));
					}
				}, handler));
			}
		}

		public static Coroutine GetAllLuckyDraws(Action<List<LuckyDrawUnityView>> callback, Action<Exception> handler)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IShopWebServiceContract", "ShopWebService", "GetAllLuckyDraws_1", memoryStream.ToArray(), delegate(byte[] data)
				{
					if (callback != null)
					{
						callback(ListProxy<LuckyDrawUnityView>.Deserialize(new MemoryStream(data), LuckyDrawUnityViewProxy.Deserialize));
					}
				}, handler));
			}
		}

		public static Coroutine GetAllLuckyDraws(BundleCategoryType bundleCategoryType, Action<List<LuckyDrawUnityView>> callback, Action<Exception> handler)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				EnumProxy<BundleCategoryType>.Serialize(memoryStream, bundleCategoryType);
				return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IShopWebServiceContract", "ShopWebService", "GetAllLuckyDraws_2", memoryStream.ToArray(), delegate(byte[] data)
				{
					if (callback != null)
					{
						callback(ListProxy<LuckyDrawUnityView>.Deserialize(new MemoryStream(data), LuckyDrawUnityViewProxy.Deserialize));
					}
				}, handler));
			}
		}

		public static Coroutine GetLuckyDraw(int luckyDrawId, Action<LuckyDrawUnityView> callback, Action<Exception> handler)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Int32Proxy.Serialize(memoryStream, luckyDrawId);
				return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IShopWebServiceContract", "ShopWebService", "GetLuckyDraw", memoryStream.ToArray(), delegate(byte[] data)
				{
					if (callback != null)
					{
						callback(LuckyDrawUnityViewProxy.Deserialize(new MemoryStream(data)));
					}
				}, handler));
			}
		}

		public static Coroutine RollLuckyDraw(string authToken, int luckDrawId, ChannelType channel, Action<int> callback, Action<Exception> handler)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				StringProxy.Serialize(memoryStream, authToken);
				Int32Proxy.Serialize(memoryStream, luckDrawId);
				EnumProxy<ChannelType>.Serialize(memoryStream, channel);
				return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IShopWebServiceContract", "ShopWebService", "RollLuckyDraw", memoryStream.ToArray(), delegate(byte[] data)
				{
					if (callback != null)
					{
						callback(Int32Proxy.Deserialize(new MemoryStream(data)));
					}
				}, handler));
			}
		}
	}
}
