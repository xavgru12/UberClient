// Decompiled with JetBrains decompiler
// Type: UberStrike.WebService.Unity.AuthenticationWebServiceClient
// Assembly: UberStrike.DataCenter.UnitySdk, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: 1925C691-C9DE-44B0-95F4-3171E7957DDD
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.DataCenter.UnitySdk.dll

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
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IAuthenticationWebServiceContract", "UberStrike.DataCenter.WebService.CWS.AuthenticationWebServiceContract.svc", nameof (CreateUser), bytes.ToArray(), (Action<byte[]>) (data =>
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
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IAuthenticationWebServiceContract", "UberStrike.DataCenter.WebService.CWS.AuthenticationWebServiceContract.svc", nameof (CompleteAccount), bytes.ToArray(), (Action<byte[]>) (data =>
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
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IAuthenticationWebServiceContract", "UberStrike.DataCenter.WebService.CWS.AuthenticationWebServiceContract.svc", nameof (LoginMemberEmail), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(MemberAuthenticationResultViewProxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine LoginMemberCookie(
      int cmid,
      DateTime expirationTime,
      string encryptedContent,
      string hash,
      ChannelType channelType,
      string machineId,
      Action<MemberAuthenticationResultView> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, cmid);
        DateTimeProxy.Serialize((Stream) bytes, expirationTime);
        StringProxy.Serialize((Stream) bytes, encryptedContent);
        StringProxy.Serialize((Stream) bytes, hash);
        EnumProxy<ChannelType>.Serialize((Stream) bytes, channelType);
        StringProxy.Serialize((Stream) bytes, machineId);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IAuthenticationWebServiceContract", "UberStrike.DataCenter.WebService.CWS.AuthenticationWebServiceContract.svc", nameof (LoginMemberCookie), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(MemberAuthenticationResultViewProxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine LoginMemberFacebook(
      string facebookID,
      string hash,
      string machineId,
      Action<MemberAuthenticationResultView> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        StringProxy.Serialize((Stream) bytes, facebookID);
        StringProxy.Serialize((Stream) bytes, hash);
        StringProxy.Serialize((Stream) bytes, machineId);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IAuthenticationWebServiceContract", "UberStrike.DataCenter.WebService.CWS.AuthenticationWebServiceContract.svc", nameof (LoginMemberFacebook), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(MemberAuthenticationResultViewProxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine FacebookSingleSignOn(
      string authToken,
      ChannelType channelType,
      string machineId,
      Action<MemberAuthenticationResultView> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        StringProxy.Serialize((Stream) bytes, authToken);
        EnumProxy<ChannelType>.Serialize((Stream) bytes, channelType);
        StringProxy.Serialize((Stream) bytes, machineId);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IAuthenticationWebServiceContract", "UberStrike.DataCenter.WebService.CWS.AuthenticationWebServiceContract.svc", nameof (FacebookSingleSignOn), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(MemberAuthenticationResultViewProxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }
  }
}
