// Decompiled with JetBrains decompiler
// Type: UberStrike.WebService.Unity.ApplicationWebServiceClient
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
using UberStrike.DataCenter.Common.Entities;
using UnityEngine;

namespace UberStrike.WebService.Unity
{
  public static class ApplicationWebServiceClient
  {
    public static Coroutine AuthenticateApplication(
      string clientVersion,
      ChannelType channel,
      string publicKey,
      Action<AuthenticateApplicationView> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        StringProxy.Serialize((Stream) bytes, clientVersion);
        EnumProxy<ChannelType>.Serialize((Stream) bytes, channel);
        StringProxy.Serialize((Stream) bytes, publicKey);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IApplicationWebServiceContract", "ApplicationWebService", nameof (AuthenticateApplication), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(AuthenticateApplicationViewProxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine GetMaps(
      string clientVersion,
      DefinitionType clientType,
      Action<List<MapView>> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        StringProxy.Serialize((Stream) bytes, clientVersion);
        EnumProxy<DefinitionType>.Serialize((Stream) bytes, clientType);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IApplicationWebServiceContract", "ApplicationWebService", nameof (GetMaps), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ListProxy<MapView>.Deserialize((Stream) new MemoryStream(data), new ListProxy<MapView>.Deserializer<MapView>(MapViewProxy.Deserialize)));
        }), handler));
      }
    }

    public static Coroutine GetConfigurationData(
      string clientVersion,
      Action<ApplicationConfigurationView> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        StringProxy.Serialize((Stream) bytes, clientVersion);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IApplicationWebServiceContract", "ApplicationWebService", nameof (GetConfigurationData), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ApplicationConfigurationViewProxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine SetMatchScore(
      string clientVersion,
      MatchStats scoringView,
      string serverAuthentication,
      Action callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        StringProxy.Serialize((Stream) bytes, clientVersion);
        MatchStatsProxy.Serialize((Stream) bytes, scoringView);
        StringProxy.Serialize((Stream) bytes, serverAuthentication);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IApplicationWebServiceContract", "ApplicationWebService", nameof (SetMatchScore), bytes.ToArray(), (Action<byte[]>) (_param1 =>
        {
          if (callback == null)
            return;
          callback();
        }), handler));
      }
    }
  }
}
