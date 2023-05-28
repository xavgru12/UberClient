// Decompiled with JetBrains decompiler
// Type: UberStrike.WebService.Unity.ApplicationWebServiceClient
// Assembly: UberStrike.DataCenter.UnitySdk, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: 1925C691-C9DE-44B0-95F4-3171E7957DDD
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.DataCenter.UnitySdk.dll

using Cmune.Core.Models.Views;
using Cmune.DataCenter.Common.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using UberStrike.Core.Models.Views;
using UberStrike.Core.Serialization;
using UberStrike.Core.Types;
using UberStrike.DataCenter.Common.Entities;
using UnityEngine;

namespace UberStrike.WebService.Unity
{
  public static class ApplicationWebServiceClient
  {
    [Obsolete("")]
    public static Coroutine GetPhotonServers(
      ApplicationView applicationView,
      Action<List<PhotonView>> callback,
      Action<Exception> handler)
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        ApplicationViewProxy.Serialize((Stream) memoryStream, applicationView);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IApplicationWebServiceContract", "UberStrike.DataCenter.WebService.CWS.ApplicationWebServiceContract.svc", nameof (GetPhotonServers), memoryStream.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ListProxy<PhotonView>.Deserialize((Stream) new MemoryStream(data), new ListProxy<PhotonView>.Deserializer<PhotonView>(PhotonViewProxy.Deserialize)));
        }), handler));
      }
    }

    public static Coroutine GetMyIP(Action<string> callback, Action<Exception> handler)
    {
      using (MemoryStream memoryStream = new MemoryStream())
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IApplicationWebServiceContract", "UberStrike.DataCenter.WebService.CWS.ApplicationWebServiceContract.svc", nameof (GetMyIP), memoryStream.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(StringProxy.Deserialize((Stream) new MemoryStream(data)));
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
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IApplicationWebServiceContract", "UberStrike.DataCenter.WebService.CWS.ApplicationWebServiceContract.svc", nameof (AuthenticateApplication), bytes.ToArray(), (Action<byte[]>) (data =>
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
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IApplicationWebServiceContract", "UberStrike.DataCenter.WebService.CWS.ApplicationWebServiceContract.svc", nameof (RecordException), bytes.ToArray(), (Action<byte[]>) (data =>
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
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IApplicationWebServiceContract", "UberStrike.DataCenter.WebService.CWS.ApplicationWebServiceContract.svc", nameof (RecordExceptionUnencrypted), bytes.ToArray(), (Action<byte[]>) (data =>
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
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IApplicationWebServiceContract", "UberStrike.DataCenter.WebService.CWS.ApplicationWebServiceContract.svc", nameof (RecordTutorialStep), bytes.ToArray(), (Action<byte[]>) (data =>
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
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IApplicationWebServiceContract", "UberStrike.DataCenter.WebService.CWS.ApplicationWebServiceContract.svc", nameof (ReportBug), memoryStream.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(BooleanProxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine GetLiveFeed(
      Action<List<LiveFeedView>> callback,
      Action<Exception> handler)
    {
      using (MemoryStream memoryStream = new MemoryStream())
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IApplicationWebServiceContract", "UberStrike.DataCenter.WebService.CWS.ApplicationWebServiceContract.svc", nameof (GetLiveFeed), memoryStream.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ListProxy<LiveFeedView>.Deserialize((Stream) new MemoryStream(data), new ListProxy<LiveFeedView>.Deserializer<LiveFeedView>(LiveFeedViewProxy.Deserialize)));
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
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IApplicationWebServiceContract", "UberStrike.DataCenter.WebService.CWS.ApplicationWebServiceContract.svc", nameof (GetMaps), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ListProxy<MapView>.Deserialize((Stream) new MemoryStream(data), new ListProxy<MapView>.Deserializer<MapView>(MapViewProxy.Deserialize)));
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
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IApplicationWebServiceContract", "UberStrike.DataCenter.WebService.CWS.ApplicationWebServiceContract.svc", nameof (GetItemAssetBundles), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ListProxy<ItemAssetBundleView>.Deserialize((Stream) new MemoryStream(data), new ListProxy<ItemAssetBundleView>.Deserializer<ItemAssetBundleView>(ItemAssetBundleViewProxy.Deserialize)));
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
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IApplicationWebServiceContract", "UberStrike.DataCenter.WebService.CWS.ApplicationWebServiceContract.svc", nameof (SetLevelVersion), bytes.ToArray(), (Action<byte[]>) (data =>
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
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IApplicationWebServiceContract", "UberStrike.DataCenter.WebService.CWS.ApplicationWebServiceContract.svc", nameof (GetPhotonServerName), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(StringProxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine IsAlive(Action<string> callback, Action<Exception> handler)
    {
      using (MemoryStream memoryStream = new MemoryStream())
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IApplicationWebServiceContract", "UberStrike.DataCenter.WebService.CWS.ApplicationWebServiceContract.svc", nameof (IsAlive), memoryStream.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(StringProxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
    }
  }
}
