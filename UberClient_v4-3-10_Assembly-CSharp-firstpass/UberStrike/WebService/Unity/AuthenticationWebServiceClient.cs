// Decompiled with JetBrains decompiler
// Type: UberStrike.WebService.Unity.AuthenticationWebServiceClient
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using Cmune.DataCenter.Common.Entities;
using System;
using System.IO;
using UberStrike.Core.Serialization;
using UberStrike.Core.ViewModel;
using UberStrike.DataCenter.Common.Entities;
using UnityEngine;

namespace UberStrike.WebService.Unity
{
  public static class AuthenticationWebServiceClient
  {
    public static Coroutine CreateUser(
      string emailAddress,
      string password,
      ChannelType channel,
      string locale,
      string machineId,
      Action<MemberRegistrationResult> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        StringProxy.Serialize((Stream) bytes, emailAddress);
        StringProxy.Serialize((Stream) bytes, password);
        EnumProxy<ChannelType>.Serialize((Stream) bytes, channel);
        StringProxy.Serialize((Stream) bytes, locale);
        StringProxy.Serialize((Stream) bytes, machineId);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IAuthenticationWebServiceContract", "AuthenticationWebService", nameof (CreateUser), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(EnumProxy<MemberRegistrationResult>.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine CompleteAccount(
      int cmid,
      string name,
      ChannelType channel,
      string locale,
      string machineId,
      Action<AccountCompletionResultView> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, cmid);
        StringProxy.Serialize((Stream) bytes, name);
        EnumProxy<ChannelType>.Serialize((Stream) bytes, channel);
        StringProxy.Serialize((Stream) bytes, locale);
        StringProxy.Serialize((Stream) bytes, machineId);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IAuthenticationWebServiceContract", "AuthenticationWebService", nameof (CompleteAccount), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(AccountCompletionResultViewProxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine LoginMemberEmail(
      string email,
      string password,
      ChannelType channelType,
      string machineId,
      Action<MemberAuthenticationResultView> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        StringProxy.Serialize((Stream) bytes, email);
        StringProxy.Serialize((Stream) bytes, password);
        EnumProxy<ChannelType>.Serialize((Stream) bytes, channelType);
        StringProxy.Serialize((Stream) bytes, machineId);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IAuthenticationWebServiceContract", "AuthenticationWebService", nameof (LoginMemberEmail), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(MemberAuthenticationResultViewProxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine LoginMemberFacebookUnitySdk(
      string facebookPlayerAccessToken,
      ChannelType channelType,
      string machineId,
      Action<MemberAuthenticationResultView> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        StringProxy.Serialize((Stream) bytes, facebookPlayerAccessToken);
        EnumProxy<ChannelType>.Serialize((Stream) bytes, channelType);
        StringProxy.Serialize((Stream) bytes, machineId);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IAuthenticationWebServiceContract", "AuthenticationWebService", nameof (LoginMemberFacebookUnitySdk), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(MemberAuthenticationResultViewProxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine LoginSteam(
      string clientVersion,
      string steamId,
      string authToken,
      string machineId,
      string hwid,
      bool isMac,
      Action<MemberAuthenticationResultView> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        StringProxy.Serialize((Stream) bytes, clientVersion);
        StringProxy.Serialize((Stream) bytes, steamId);
        StringProxy.Serialize((Stream) bytes, authToken);
        StringProxy.Serialize((Stream) bytes, machineId);
        StringProxy.Serialize((Stream) bytes, hwid);
        BooleanProxy.Serialize((Stream) bytes, isMac);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IAuthenticationWebServiceContract", "AuthenticationWebService", nameof (LoginSteam), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(MemberAuthenticationResultViewProxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine LoginMemberPortal(
      int cmid,
      string hash,
      string machineId,
      Action<MemberAuthenticationResultView> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, cmid);
        StringProxy.Serialize((Stream) bytes, hash);
        StringProxy.Serialize((Stream) bytes, machineId);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IAuthenticationWebServiceContract", "AuthenticationWebService", nameof (LoginMemberPortal), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(MemberAuthenticationResultViewProxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine LinkSteamMember(
      string email,
      string password,
      string steamId,
      string machineId,
      Action<MemberAuthenticationResultView> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        StringProxy.Serialize((Stream) bytes, email);
        StringProxy.Serialize((Stream) bytes, password);
        StringProxy.Serialize((Stream) bytes, steamId);
        StringProxy.Serialize((Stream) bytes, machineId);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IAuthenticationWebServiceContract", "AuthenticationWebService", nameof (LinkSteamMember), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(MemberAuthenticationResultViewProxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }
  }
}
