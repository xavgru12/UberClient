// Decompiled with JetBrains decompiler
// Type: UberStrike.WebService.Unity.ModerationWebServiceClient
// Assembly: UberStrike.DataCenter.UnitySdk, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: 1925C691-C9DE-44B0-95F4-3171E7957DDD
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.DataCenter.UnitySdk.dll

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
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IModerationWebServiceContract", "UberStrike.DataCenter.WebService.CWS.ModerationWebServiceContract.svc", nameof (BanPermanently), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(BooleanProxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }
  }
}
