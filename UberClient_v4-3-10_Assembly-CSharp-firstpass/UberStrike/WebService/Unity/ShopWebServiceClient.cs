// Decompiled with JetBrains decompiler
// Type: UberStrike.WebService.Unity.ShopWebServiceClient
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

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
    public static Coroutine GetShop(
      Action<UberStrikeItemShopClientView> callback,
      Action<Exception> handler)
    {
      using (MemoryStream memoryStream = new MemoryStream())
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IShopWebServiceContract", "ShopWebService", nameof (GetShop), memoryStream.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(UberStrikeItemShopClientViewProxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
    }

    public static Coroutine BuyItem(
      int itemId,
      string authToken,
      UberStrikeCurrencyType currencyType,
      BuyingDurationType durationType,
      UberstrikeItemType itemType,
      BuyingLocationType marketLocation,
      BuyingRecommendationType recommendationType,
      Action<int> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, itemId);
        StringProxy.Serialize((Stream) bytes, authToken);
        EnumProxy<UberStrikeCurrencyType>.Serialize((Stream) bytes, currencyType);
        EnumProxy<BuyingDurationType>.Serialize((Stream) bytes, durationType);
        EnumProxy<UberstrikeItemType>.Serialize((Stream) bytes, itemType);
        EnumProxy<BuyingLocationType>.Serialize((Stream) bytes, marketLocation);
        EnumProxy<BuyingRecommendationType>.Serialize((Stream) bytes, recommendationType);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IShopWebServiceContract", "ShopWebService", nameof (BuyItem), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(Int32Proxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine BuyPack(
      int itemId,
      string authToken,
      PackType packType,
      UberStrikeCurrencyType currencyType,
      UberstrikeItemType itemType,
      BuyingLocationType marketLocation,
      BuyingRecommendationType recommendationType,
      Action<int> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, itemId);
        StringProxy.Serialize((Stream) bytes, authToken);
        EnumProxy<PackType>.Serialize((Stream) bytes, packType);
        EnumProxy<UberStrikeCurrencyType>.Serialize((Stream) bytes, currencyType);
        EnumProxy<UberstrikeItemType>.Serialize((Stream) bytes, itemType);
        EnumProxy<BuyingLocationType>.Serialize((Stream) bytes, marketLocation);
        EnumProxy<BuyingRecommendationType>.Serialize((Stream) bytes, recommendationType);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IShopWebServiceContract", "ShopWebService", nameof (BuyPack), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(Int32Proxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine GetBundles(
      ChannelType channel,
      Action<List<BundleView>> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        EnumProxy<ChannelType>.Serialize((Stream) bytes, channel);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IShopWebServiceContract", "ShopWebService", nameof (GetBundles), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ListProxy<BundleView>.Deserialize((Stream) new MemoryStream(data), new ListProxy<BundleView>.Deserializer<BundleView>(BundleViewProxy.Deserialize)));
        }), handler));
      }
    }

    public static Coroutine BuyBundle(
      string authToken,
      int bundleId,
      ChannelType channel,
      string hashedReceipt,
      Action<bool> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        StringProxy.Serialize((Stream) bytes, authToken);
        Int32Proxy.Serialize((Stream) bytes, bundleId);
        EnumProxy<ChannelType>.Serialize((Stream) bytes, channel);
        StringProxy.Serialize((Stream) bytes, hashedReceipt);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IShopWebServiceContract", "ShopWebService", nameof (BuyBundle), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(BooleanProxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine BuyBundleSteam(
      int bundleId,
      string steamId,
      string authToken,
      Action<bool> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, bundleId);
        StringProxy.Serialize((Stream) bytes, steamId);
        StringProxy.Serialize((Stream) bytes, authToken);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IShopWebServiceContract", "ShopWebService", nameof (BuyBundleSteam), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(BooleanProxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine FinishBuyBundleSteam(
      string orderId,
      Action<bool> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        StringProxy.Serialize((Stream) bytes, orderId);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IShopWebServiceContract", "ShopWebService", nameof (FinishBuyBundleSteam), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(BooleanProxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine VerifyReceipt(
      string hashedReceipt,
      Action<bool> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        StringProxy.Serialize((Stream) bytes, hashedReceipt);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IShopWebServiceContract", "ShopWebService", nameof (VerifyReceipt), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(BooleanProxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine UseConsumableItem(
      string authToken,
      int itemId,
      Action<bool> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        StringProxy.Serialize((Stream) bytes, authToken);
        Int32Proxy.Serialize((Stream) bytes, itemId);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IShopWebServiceContract", "ShopWebService", nameof (UseConsumableItem), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(BooleanProxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine GetAllMysteryBoxs(
      Action<List<MysteryBoxUnityView>> callback,
      Action<Exception> handler)
    {
      using (MemoryStream memoryStream = new MemoryStream())
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IShopWebServiceContract", "ShopWebService", "GetAllMysteryBoxs_1", memoryStream.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ListProxy<MysteryBoxUnityView>.Deserialize((Stream) new MemoryStream(data), new ListProxy<MysteryBoxUnityView>.Deserializer<MysteryBoxUnityView>(MysteryBoxUnityViewProxy.Deserialize)));
        }), handler));
    }

    public static Coroutine GetAllMysteryBoxs(
      BundleCategoryType bundleCategoryType,
      Action<List<MysteryBoxUnityView>> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        EnumProxy<BundleCategoryType>.Serialize((Stream) bytes, bundleCategoryType);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IShopWebServiceContract", "ShopWebService", "GetAllMysteryBoxs_2", bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ListProxy<MysteryBoxUnityView>.Deserialize((Stream) new MemoryStream(data), new ListProxy<MysteryBoxUnityView>.Deserializer<MysteryBoxUnityView>(MysteryBoxUnityViewProxy.Deserialize)));
        }), handler));
      }
    }

    public static Coroutine GetMysteryBox(
      int mysteryBoxId,
      Action<MysteryBoxUnityView> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, mysteryBoxId);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IShopWebServiceContract", "ShopWebService", nameof (GetMysteryBox), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(MysteryBoxUnityViewProxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine RollMysteryBox(
      string authToken,
      int mysteryBoxId,
      ChannelType channel,
      Action<List<MysteryBoxWonItemUnityView>> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        StringProxy.Serialize((Stream) bytes, authToken);
        Int32Proxy.Serialize((Stream) bytes, mysteryBoxId);
        EnumProxy<ChannelType>.Serialize((Stream) bytes, channel);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IShopWebServiceContract", "ShopWebService", nameof (RollMysteryBox), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ListProxy<MysteryBoxWonItemUnityView>.Deserialize((Stream) new MemoryStream(data), new ListProxy<MysteryBoxWonItemUnityView>.Deserializer<MysteryBoxWonItemUnityView>(MysteryBoxWonItemUnityViewProxy.Deserialize)));
        }), handler));
      }
    }

    public static Coroutine GetAllLuckyDraws(
      Action<List<LuckyDrawUnityView>> callback,
      Action<Exception> handler)
    {
      using (MemoryStream memoryStream = new MemoryStream())
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IShopWebServiceContract", "ShopWebService", "GetAllLuckyDraws_1", memoryStream.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ListProxy<LuckyDrawUnityView>.Deserialize((Stream) new MemoryStream(data), new ListProxy<LuckyDrawUnityView>.Deserializer<LuckyDrawUnityView>(LuckyDrawUnityViewProxy.Deserialize)));
        }), handler));
    }

    public static Coroutine GetAllLuckyDraws(
      BundleCategoryType bundleCategoryType,
      Action<List<LuckyDrawUnityView>> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        EnumProxy<BundleCategoryType>.Serialize((Stream) bytes, bundleCategoryType);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IShopWebServiceContract", "ShopWebService", "GetAllLuckyDraws_2", bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ListProxy<LuckyDrawUnityView>.Deserialize((Stream) new MemoryStream(data), new ListProxy<LuckyDrawUnityView>.Deserializer<LuckyDrawUnityView>(LuckyDrawUnityViewProxy.Deserialize)));
        }), handler));
      }
    }

    public static Coroutine GetLuckyDraw(
      int luckyDrawId,
      Action<LuckyDrawUnityView> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, luckyDrawId);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IShopWebServiceContract", "ShopWebService", nameof (GetLuckyDraw), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(LuckyDrawUnityViewProxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine RollLuckyDraw(
      string authToken,
      int luckDrawId,
      ChannelType channel,
      Action<int> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        StringProxy.Serialize((Stream) bytes, authToken);
        Int32Proxy.Serialize((Stream) bytes, luckDrawId);
        EnumProxy<ChannelType>.Serialize((Stream) bytes, channel);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IShopWebServiceContract", "ShopWebService", nameof (RollLuckyDraw), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(Int32Proxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }
  }
}
