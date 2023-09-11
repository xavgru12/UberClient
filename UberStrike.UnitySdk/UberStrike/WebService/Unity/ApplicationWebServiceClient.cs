// Decompiled with JetBrains decompiler
// Type: UberStrike.WebService.Unity.ApplicationWebServiceClient
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using Cmune.Core.Models.Views;
using Cmune.DataCenter.Common.Entities;
using Cmune.Util;
using System;
using System.Collections.Generic;
using System.IO;
using UberStrike.Core.Models.Views;
using UberStrike.Core.Serialization;
using UberStrike.Core.Types;
using UberStrike.Core.ViewModel;
using UberStrike.DataCenter.Common.Entities;
using UnityEngine;

namespace UberStrike.WebService.Unity
{
  public static class ApplicationWebServiceClient
  {
    [Obsolete("Replaced by AuthenticateApplication")]
    public static Coroutine RegisterClientApplication(
      int cmuneId,
      string hashCode,
      ChannelType channel,
      int applicationId,
      Action<RegisterClientApplicationViewModel> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, cmuneId);
        StringProxy.Serialize((Stream) bytes, hashCode);
        EnumProxy<ChannelType>.Serialize((Stream) bytes, channel);
        Int32Proxy.Serialize((Stream) bytes, applicationId);
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IApplicationWebServiceContract", "ApplicationWebService", nameof (RegisterClientApplication), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(RegisterClientApplicationViewModelProxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    [Obsolete("")]
    public static Coroutine GetPhotonServers(
      ApplicationView applicationView,
      Action<List<PhotonView>> callback,
      Action<Exception> handler)
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        ApplicationViewProxy.Serialize((Stream) memoryStream, applicationView);
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IApplicationWebServiceContract", "ApplicationWebService", nameof (GetPhotonServers), Cryptography.RijndaelEncrypt(memoryStream.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ListProxy<PhotonView>.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector)), new ListProxy<PhotonView>.Deserializer<PhotonView>(PhotonViewProxy.Deserialize)));
        }), handler));
      }
    }

    [Obsolete("")]
    public static Coroutine GetMyIP(Action<string> callback, Action<Exception> handler)
    {
      using (MemoryStream memoryStream = new MemoryStream())
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IApplicationWebServiceContract", "ApplicationWebService", nameof (GetMyIP), Cryptography.RijndaelEncrypt(memoryStream.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(StringProxy.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector))));
        }), handler));
    }

    public static Coroutine AuthenticateApplication(
      string version,
      ChannelType channel,
      string publicKey,
      Action<AuthenticateApplicationView> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        StringProxy.Serialize((Stream) bytes, version);
        EnumProxy<ChannelType>.Serialize((Stream) bytes, channel);
        StringProxy.Serialize((Stream) bytes, publicKey);
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IApplicationWebServiceContract", "ApplicationWebService", nameof (AuthenticateApplication), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(AuthenticateApplicationViewProxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine RecordException(
      int cmid,
      BuildType buildType,
      ChannelType channelType,
      string buildNumber,
      string logString,
      string stackTrace,
      string exceptionData,
      Action callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, cmid);
        EnumProxy<BuildType>.Serialize((Stream) bytes, buildType);
        EnumProxy<ChannelType>.Serialize((Stream) bytes, channelType);
        StringProxy.Serialize((Stream) bytes, buildNumber);
        StringProxy.Serialize((Stream) bytes, logString);
        StringProxy.Serialize((Stream) bytes, stackTrace);
        StringProxy.Serialize((Stream) bytes, exceptionData);
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IApplicationWebServiceContract", "ApplicationWebService", nameof (RecordException), Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback();
        }), handler));
      }
    }

    public static Coroutine RecordExceptionUnencrypted(
      BuildType buildType,
      ChannelType channelType,
      string buildNumber,
      string errorType,
      string errorMessage,
      Action callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        EnumProxy<BuildType>.Serialize((Stream) bytes, buildType);
        EnumProxy<ChannelType>.Serialize((Stream) bytes, channelType);
        StringProxy.Serialize((Stream) bytes, buildNumber);
        StringProxy.Serialize((Stream) bytes, errorType);
        StringProxy.Serialize((Stream) bytes, errorMessage);
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IApplicationWebServiceContract", "ApplicationWebService", nameof (RecordExceptionUnencrypted), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback();
        }), handler));
      }
    }

    public static Coroutine RecordTutorialStep(
      int cmid,
      TutorialStepType step,
      Action callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, cmid);
        EnumProxy<TutorialStepType>.Serialize((Stream) bytes, step);
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IApplicationWebServiceContract", "ApplicationWebService", nameof (RecordTutorialStep), Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback();
        }), handler));
      }
    }

    public static Coroutine ReportBug(
      BugView bugView,
      Action<bool> callback,
      Action<Exception> handler)
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        BugViewProxy.Serialize((Stream) memoryStream, bugView);
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IApplicationWebServiceContract", "ApplicationWebService", nameof (ReportBug), Cryptography.RijndaelEncrypt(memoryStream.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(BooleanProxy.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector))));
        }), handler));
      }
    }

    public static Coroutine GetLiveFeed(
      Action<List<LiveFeedView>> callback,
      Action<Exception> handler)
    {
      using (MemoryStream memoryStream = new MemoryStream())
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IApplicationWebServiceContract", "ApplicationWebService", nameof (GetLiveFeed), Cryptography.RijndaelEncrypt(memoryStream.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ListProxy<LiveFeedView>.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector)), new ListProxy<LiveFeedView>.Deserializer<LiveFeedView>(LiveFeedViewProxy.Deserialize)));
        }), handler));
    }

    public static Coroutine GetMaps(
      string appVersion,
      LocaleType locale,
      DefinitionType definition,
      Action<List<MapView>> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        StringProxy.Serialize((Stream) bytes, appVersion);
        EnumProxy<LocaleType>.Serialize((Stream) bytes, locale);
        EnumProxy<DefinitionType>.Serialize((Stream) bytes, definition);
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IApplicationWebServiceContract", "ApplicationWebService", nameof (GetMaps), Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ListProxy<MapView>.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector)), new ListProxy<MapView>.Deserializer<MapView>(MapViewProxy.Deserialize)));
        }), handler));
      }
    }

    public static Coroutine GetItemAssetBundles(
      string appVersion,
      DefinitionType definition,
      Action<List<ItemAssetBundleView>> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        StringProxy.Serialize((Stream) bytes, appVersion);
        EnumProxy<DefinitionType>.Serialize((Stream) bytes, definition);
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IApplicationWebServiceContract", "ApplicationWebService", nameof (GetItemAssetBundles), Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ListProxy<ItemAssetBundleView>.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector)), new ListProxy<ItemAssetBundleView>.Deserializer<ItemAssetBundleView>(ItemAssetBundleViewProxy.Deserialize)));
        }), handler));
      }
    }

    public static Coroutine SetLevelVersion(
      int id,
      int version,
      string md5Hash,
      Action callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, id);
        Int32Proxy.Serialize((Stream) bytes, version);
        StringProxy.Serialize((Stream) bytes, md5Hash);
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IApplicationWebServiceContract", "ApplicationWebService", nameof (SetLevelVersion), Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback();
        }), handler));
      }
    }

    public static Coroutine GetPhotonServerName(
      string applicationVersion,
      string ipAddress,
      int port,
      Action<string> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        StringProxy.Serialize((Stream) bytes, applicationVersion);
        StringProxy.Serialize((Stream) bytes, ipAddress);
        Int32Proxy.Serialize((Stream) bytes, port);
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IApplicationWebServiceContract", "ApplicationWebService", nameof (GetPhotonServerName), Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(StringProxy.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector))));
        }), handler));
      }
    }

    public static Coroutine IsAlive(Action<string> callback, Action<Exception> handler)
    {
      using (MemoryStream memoryStream = new MemoryStream())
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IApplicationWebServiceContract", "ApplicationWebService", nameof (IsAlive), Cryptography.RijndaelEncrypt(memoryStream.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(StringProxy.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector))));
        }), handler));
    }
  }
}
