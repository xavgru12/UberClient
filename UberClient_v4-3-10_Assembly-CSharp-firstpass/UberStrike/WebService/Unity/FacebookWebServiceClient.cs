// Decompiled with JetBrains decompiler
// Type: UberStrike.WebService.Unity.FacebookWebServiceClient
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using Cmune.DataCenter.Common.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using UberStrike.Core.Serialization;
using UnityEngine;

namespace UberStrike.WebService.Unity
{
  public static class FacebookWebServiceClient
  {
    public static Coroutine ClaimFacebookGift(
      string authToken,
      string facebookRequestObjectId,
      Action<ClaimFacebookGiftView> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        StringProxy.Serialize((Stream) bytes, authToken);
        StringProxy.Serialize((Stream) bytes, facebookRequestObjectId);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IFacebookWebServiceContract", "FacebookWebService", nameof (ClaimFacebookGift), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ClaimFacebookGiftViewProxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine AttachFacebookAccountToCmuneAccount(
      string authToken,
      string facebookId,
      Action<MemberOperationResult> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        StringProxy.Serialize((Stream) bytes, authToken);
        StringProxy.Serialize((Stream) bytes, facebookId);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IFacebookWebServiceContract", "FacebookWebService", nameof (AttachFacebookAccountToCmuneAccount), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(EnumProxy<MemberOperationResult>.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine CheckFacebookSession(
      string cmuneAuthToken,
      string facebookIDString,
      Action<bool> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        StringProxy.Serialize((Stream) bytes, cmuneAuthToken);
        StringProxy.Serialize((Stream) bytes, facebookIDString);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IFacebookWebServiceContract", "FacebookWebService", nameof (CheckFacebookSession), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(BooleanProxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine GetFacebookFriendsList(
      List<string> facebookIds,
      Action<List<PublicProfileView>> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        ListProxy<string>.Serialize((Stream) bytes, (ICollection<string>) facebookIds, new ListProxy<string>.Serializer<string>(StringProxy.Serialize));
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IFacebookWebServiceContract", "FacebookWebService", nameof (GetFacebookFriendsList), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ListProxy<PublicProfileView>.Deserialize((Stream) new MemoryStream(data), new ListProxy<PublicProfileView>.Deserializer<PublicProfileView>(PublicProfileViewProxy.Deserialize)));
        }), handler));
      }
    }
  }
}
