
using Cmune.Util;
using System;
using System.IO;
using UberStrike.Core.Serialization;
using UnityEngine;

namespace UberStrike.WebService.Unity
{
  public static class ModerationWebServiceClient
  {
    public static Coroutine BanPermanently(
      int sourceCmid,
      int targetCmid,
      int applicationId,
      string ip,
      Action<bool> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, sourceCmid);
        Int32Proxy.Serialize((Stream) bytes, targetCmid);
        Int32Proxy.Serialize((Stream) bytes, applicationId);
        StringProxy.Serialize((Stream) bytes, ip);
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IModerationWebServiceContract", "ModerationWebService", nameof (BanPermanently), Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(BooleanProxy.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector))));
        }), handler));
      }
    }
  }
}
