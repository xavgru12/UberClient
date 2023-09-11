
using Cmune.DataCenter.Common.Entities;
using Cmune.Util;
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
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IShopWebServiceContract", "ShopWebService", nameof (GetShop), Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(UberStrikeItemShopClientViewProxy.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector))));
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
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IShopWebServiceContract", "ShopWebService", nameof (BuyItem), Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(Int32Proxy.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector))));
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
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IShopWebServiceContract", "ShopWebService", nameof (BuyPack), Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(Int32Proxy.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector))));
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
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IShopWebServiceContract", "ShopWebService", nameof (GetBundles), Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ListProxy<BundleView>.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector)), new ListProxy<BundleView>.Deserializer<BundleView>(BundleViewProxy.Deserialize)));
        }), handler));
      }
    }

    public static Coroutine BuyMasBundle(
      int cmid,
      int bundleId,
      string hashedReceipt,
      int applicationId,
      Action<bool> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, cmid);
        Int32Proxy.Serialize((Stream) bytes, bundleId);
        StringProxy.Serialize((Stream) bytes, hashedReceipt);
        Int32Proxy.Serialize((Stream) bytes, applicationId);
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IShopWebServiceContract", "ShopWebService", nameof (BuyMasBundle), Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(BooleanProxy.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector))));
        }), handler));
      }
    }

    public static Coroutine BuyiPadBundle(
      int cmid,
      int bundleId,
      string hashedReceipt,
      int applicationId,
      Action<bool> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, cmid);
        Int32Proxy.Serialize((Stream) bytes, bundleId);
        StringProxy.Serialize((Stream) bytes, hashedReceipt);
        Int32Proxy.Serialize((Stream) bytes, applicationId);
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IShopWebServiceContract", "ShopWebService", nameof (BuyiPadBundle), Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(BooleanProxy.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector))));
        }), handler));
      }
    }

    public static Coroutine BuyiPhoneBundle(
      int cmid,
      int bundleId,
      string hashedReceipt,
      int applicationId,
      Action<bool> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, cmid);
        Int32Proxy.Serialize((Stream) bytes, bundleId);
        StringProxy.Serialize((Stream) bytes, hashedReceipt);
        Int32Proxy.Serialize((Stream) bytes, applicationId);
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IShopWebServiceContract", "ShopWebService", nameof (BuyiPhoneBundle), Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(BooleanProxy.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector))));
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
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IShopWebServiceContract", "ShopWebService", nameof (UseConsumableItem), Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(BooleanProxy.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector))));
        }), handler));
      }
    }

    public static Coroutine GetAllMysteryBoxs(
      Action<List<MysteryBoxUnityView>> callback,
      Action<Exception> handler)
    {
      using (MemoryStream memoryStream = new MemoryStream())
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IShopWebServiceContract", "ShopWebService", "GetAllMysteryBoxs_1", Cryptography.RijndaelEncrypt(memoryStream.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ListProxy<MysteryBoxUnityView>.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector)), new ListProxy<MysteryBoxUnityView>.Deserializer<MysteryBoxUnityView>(MysteryBoxUnityViewProxy.Deserialize)));
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
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IShopWebServiceContract", "ShopWebService", "GetAllMysteryBoxs_2", Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ListProxy<MysteryBoxUnityView>.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector)), new ListProxy<MysteryBoxUnityView>.Deserializer<MysteryBoxUnityView>(MysteryBoxUnityViewProxy.Deserialize)));
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
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IShopWebServiceContract", "ShopWebService", nameof (GetMysteryBox), Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(MysteryBoxUnityViewProxy.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector))));
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
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IShopWebServiceContract", "ShopWebService", nameof (RollMysteryBox), Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ListProxy<MysteryBoxWonItemUnityView>.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector)), new ListProxy<MysteryBoxWonItemUnityView>.Deserializer<MysteryBoxWonItemUnityView>(MysteryBoxWonItemUnityViewProxy.Deserialize)));
        }), handler));
      }
    }

    public static Coroutine GetAllLuckyDraws(
      Action<List<LuckyDrawUnityView>> callback,
      Action<Exception> handler)
    {
      using (MemoryStream memoryStream = new MemoryStream())
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IShopWebServiceContract", "ShopWebService", "GetAllLuckyDraws_1", Cryptography.RijndaelEncrypt(memoryStream.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ListProxy<LuckyDrawUnityView>.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector)), new ListProxy<LuckyDrawUnityView>.Deserializer<LuckyDrawUnityView>(LuckyDrawUnityViewProxy.Deserialize)));
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
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IShopWebServiceContract", "ShopWebService", "GetAllLuckyDraws_2", Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ListProxy<LuckyDrawUnityView>.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector)), new ListProxy<LuckyDrawUnityView>.Deserializer<LuckyDrawUnityView>(LuckyDrawUnityViewProxy.Deserialize)));
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
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IShopWebServiceContract", "ShopWebService", nameof (GetLuckyDraw), Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(LuckyDrawUnityViewProxy.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector))));
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
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IShopWebServiceContract", "ShopWebService", nameof (RollLuckyDraw), Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(Int32Proxy.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector))));
        }), handler));
      }
    }
  }
}
