// Decompiled with JetBrains decompiler
// Type: UberStrike.WebService.Unity.ModerationWebServiceClient
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.IO;
using UberStrike.Core.Serialization;
using UnityEngine;

namespace UberStrike.WebService.Unity
{
  public static class ModerationWebServiceClient
  {
    public static Coroutine BanPermanently(
      string authToken,
      int targetCmid,
      Action<bool> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        StringProxy.Serialize((Stream) bytes, authToken);
        Int32Proxy.Serialize((Stream) bytes, targetCmid);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IModerationWebServiceContract", "ModerationWebService", nameof (BanPermanently), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(BooleanProxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }
  }
}
