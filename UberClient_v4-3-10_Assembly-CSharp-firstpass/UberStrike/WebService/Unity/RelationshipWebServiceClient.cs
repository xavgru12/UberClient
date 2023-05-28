// Decompiled with JetBrains decompiler
// Type: UberStrike.WebService.Unity.RelationshipWebServiceClient
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
  public static class RelationshipWebServiceClient
  {
    public static Coroutine SendContactRequest(
      string authToken,
      int receiverCmid,
      string message,
      Action callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        StringProxy.Serialize((Stream) bytes, authToken);
        Int32Proxy.Serialize((Stream) bytes, receiverCmid);
        StringProxy.Serialize((Stream) bytes, message);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IRelationshipWebServiceContract", "RelationshipWebService", nameof (SendContactRequest), bytes.ToArray(), (Action<byte[]>) (_param1 =>
        {
          if (callback == null)
            return;
          callback();
        }), handler));
      }
    }

    public static Coroutine GetContactRequests(
      string authToken,
      Action<List<ContactRequestView>> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        StringProxy.Serialize((Stream) bytes, authToken);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IRelationshipWebServiceContract", "RelationshipWebService", nameof (GetContactRequests), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ListProxy<ContactRequestView>.Deserialize((Stream) new MemoryStream(data), new ListProxy<ContactRequestView>.Deserializer<ContactRequestView>(ContactRequestViewProxy.Deserialize)));
        }), handler));
      }
    }

    public static Coroutine AcceptContactRequest(
      string authToken,
      int contactRequestId,
      Action<PublicProfileView> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        StringProxy.Serialize((Stream) bytes, authToken);
        Int32Proxy.Serialize((Stream) bytes, contactRequestId);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IRelationshipWebServiceContract", "RelationshipWebService", nameof (AcceptContactRequest), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(PublicProfileViewProxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine DeclineContactRequest(
      string authToken,
      int contactRequestId,
      Action<bool> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        StringProxy.Serialize((Stream) bytes, authToken);
        Int32Proxy.Serialize((Stream) bytes, contactRequestId);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IRelationshipWebServiceContract", "RelationshipWebService", nameof (DeclineContactRequest), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(BooleanProxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine DeleteContact(
      string authToken,
      int contactCmid,
      Action<MemberOperationResult> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        StringProxy.Serialize((Stream) bytes, authToken);
        Int32Proxy.Serialize((Stream) bytes, contactCmid);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IRelationshipWebServiceContract", "RelationshipWebService", nameof (DeleteContact), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(EnumProxy<MemberOperationResult>.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine GetContactsByGroups(
      string authToken,
      bool populateFacebookIds,
      Action<List<ContactGroupView>> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        StringProxy.Serialize((Stream) bytes, authToken);
        BooleanProxy.Serialize((Stream) bytes, populateFacebookIds);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IRelationshipWebServiceContract", "RelationshipWebService", nameof (GetContactsByGroups), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ListProxy<ContactGroupView>.Deserialize((Stream) new MemoryStream(data), new ListProxy<ContactGroupView>.Deserializer<ContactGroupView>(ContactGroupViewProxy.Deserialize)));
        }), handler));
      }
    }
  }
}
