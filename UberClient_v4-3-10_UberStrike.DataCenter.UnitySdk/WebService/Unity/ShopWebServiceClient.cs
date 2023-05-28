// Decompiled with JetBrains decompiler
// Type: UberStrike.WebService.Unity.ShopWebServiceClient
// Assembly: UberStrike.DataCenter.UnitySdk, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: 1925C691-C9DE-44B0-95F4-3171E7957DDD
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.DataCenter.UnitySdk.dll

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
      string applicationVersion,
      Action<UberStrikeItemShopClientView> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        StringProxy.Serialize((Stream) bytes, applicationVersion);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IShopWebServiceContract", "UberStrike.DataCenter.WebService.CWS.ShopWebServiceContract.svc", nameof (GetShop), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(UberStrikeItemShopClientViewProxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine BuyItem(
      int itemId,
      int buyerCmid,
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
        Int32Proxy.Serialize((Stream) bytes, buyerCmid);
        EnumProxy<UberStrikeCurrencyType>.Serialize((Stream) bytes, currencyType);
        EnumProxy<BuyingDurationType>.Serialize((Stream) bytes, durationType);
        EnumProxy<UberstrikeItemType>.Serialize((Stream) bytes, itemType);
        EnumProxy<BuyingLocationType>.Serialize((Stream) bytes, marketLocation);
        EnumProxy<BuyingRecommendationType>.Serialize((Stream) bytes, recommendationType);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IShopWebServiceContract", "UberStrike.DataCenter.WebService.CWS.ShopWebServiceContract.svc", nameof (BuyItem), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(Int32Proxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine BuyPack(
      int itemId,
      int buyerCmid,
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
        Int32Proxy.Serialize((Stream) bytes, buyerCmid);
        EnumProxy<PackType>.Serialize((Stream) bytes, packType);
        EnumProxy<UberStrikeCurrencyType>.Serialize((Stream) bytes, currencyType);
        EnumProxy<UberstrikeItemType>.Serialize((Stream) bytes, itemType);
        EnumProxy<BuyingLocationType>.Serialize((Stream) bytes, marketLocation);
        EnumProxy<BuyingRecommendationType>.Serialize((Stream) bytes, recommendationType);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IShopWebServiceContract", "UberStrike.DataCenter.WebService.CWS.ShopWebServiceContract.svc", nameof (BuyPack), bytes.ToArray(), (Action<byte[]>) (data =>
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
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IShopWebServiceContract", "UberStrike.DataCenter.WebService.CWS.ShopWebServiceContract.svc", nameof (GetBundles), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ListProxy<BundleView>.Deserialize((Stream) new MemoryStream(data), new ListProxy<BundleView>.Deserializer<BundleView>(BundleViewProxy.Deserialize)));
        }), handler));
      }
    }

    public static Coroutine BuyBundle(
      int cmid,
      int bundleId,
      ChannelType channel,
      string hashedReceipt,
      int applicationId,
      Action<bool> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, cmid);
        Int32Proxy.Serialize((Stream) bytes, bundleId);
        EnumProxy<ChannelType>.Serialize((Stream) bytes, channel);
        StringProxy.Serialize((Stream) bytes, hashedReceipt);
        Int32Proxy.Serialize((Stream) bytes, applicationId);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IShopWebServiceContract", "UberStrike.DataCenter.WebService.CWS.ShopWebServiceContract.svc", nameof (BuyBundle), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(BooleanProxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine UseConsumableItem(
      int cmid,
      int itemId,
      Action<bool> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, cmid);
        Int32Proxy.Serialize((Stream) bytes, itemId);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IShopWebServiceContract", "UberStrike.DataCenter.WebService.CWS.ShopWebServiceContract.svc", nameof (UseConsumableItem), bytes.ToArray(), (Action<byte[]>) (data =>
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
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IShopWebServiceContract", "UberStrike.DataCenter.WebService.CWS.ShopWebServiceContract.svc", "GetAllMysteryBoxs_1", memoryStream.ToArray(), (Action<byte[]>) (data =>
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
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IShopWebServiceContract", "UberStrike.DataCenter.WebService.CWS.ShopWebServiceContract.svc", "GetAllMysteryBoxs_2", bytes.ToArray(), (Action<byte[]>) (data =>
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
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IShopWebServiceContract", "UberStrike.DataCenter.WebService.CWS.ShopWebServiceContract.svc", nameof (GetMysteryBox), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(MysteryBoxUnityViewProxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine RollMysteryBox(
      int cmid,
      int mysteryBoxId,
      ChannelType channel,
      Action<List<MysteryBoxWonItemUnityView>> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, cmid);
        Int32Proxy.Serialize((Stream) bytes, mysteryBoxId);
        EnumProxy<ChannelType>.Serialize((Stream) bytes, channel);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IShopWebServiceContract", "UberStrike.DataCenter.WebService.CWS.ShopWebServiceContract.svc", nameof (RollMysteryBox), bytes.ToArray(), (Action<byte[]>) (data =>
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
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IShopWebServiceContract", "UberStrike.DataCenter.WebService.CWS.ShopWebServiceContract.svc", "GetAllLuckyDraws_1", memoryStream.ToArray(), (Action<byte[]>) (data =>
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
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IShopWebServiceContract", "UberStrike.DataCenter.WebService.CWS.ShopWebServiceContract.svc", "GetAllLuckyDraws_2", bytes.ToArray(), (Action<byte[]>) (data =>
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
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IShopWebServiceContract", "UberStrike.DataCenter.WebService.CWS.ShopWebServiceContract.svc", nameof (GetLuckyDraw), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(LuckyDrawUnityViewProxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine RollLuckyDraw(
      int cmid,
      int luckDrawId,
      ChannelType channel,
      Action<int> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, cmid);
        Int32Proxy.Serialize((Stream) bytes, luckDrawId);
        EnumProxy<ChannelType>.Serialize((Stream) bytes, channel);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IShopWebServiceContract", "UberStrike.DataCenter.WebService.CWS.ShopWebServiceContract.svc", nameof (RollLuckyDraw), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(Int32Proxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }
  }
}
