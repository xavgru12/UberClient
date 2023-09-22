
using Cmune.DataCenter.Common.Entities;
using Cmune.Util;
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
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IAuthenticationWebServiceContract", "AuthenticationWebService", nameof (CreateUser), Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(EnumProxy<MemberRegistrationResult>.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector))));
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
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IAuthenticationWebServiceContract", "AuthenticationWebService", nameof (CompleteAccount), Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(AccountCompletionResultViewProxy.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector))));
        }), handler));
      }
    }

    public static Coroutine UncompleteAccount(
      int cmid,
      Action<bool> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, cmid);
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IAuthenticationWebServiceContract", "AuthenticationWebService", nameof (UncompleteAccount), Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(BooleanProxy.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector))));
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
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IAuthenticationWebServiceContract", "AuthenticationWebService", nameof (LoginMemberEmail), Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(MemberAuthenticationResultViewProxy.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector))));
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
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IAuthenticationWebServiceContract", "AuthenticationWebService", nameof (LoginMemberCookie), Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(MemberAuthenticationResultViewProxy.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector))));
        }), handler));
      }
    }
  }
}
