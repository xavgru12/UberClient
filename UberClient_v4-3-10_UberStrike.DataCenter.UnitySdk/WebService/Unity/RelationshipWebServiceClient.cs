// Decompiled with JetBrains decompiler
// Type: UberStrike.WebService.Unity.RelationshipWebServiceClient
// Assembly: UberStrike.DataCenter.UnitySdk, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: 1925C691-C9DE-44B0-95F4-3171E7957DDD
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.DataCenter.UnitySdk.dll

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
      int initiatorCmid,
      int receiverCmid,
      string message,
      Action<int> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, initiatorCmid);
        Int32Proxy.Serialize((Stream) bytes, receiverCmid);
        StringProxy.Serialize((Stream) bytes, message);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IRelationshipWebServiceContract", "UberStrike.DataCenter.WebService.CWS.RelationshipWebServiceContract.svc", nameof (SendContactRequest), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(Int32Proxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine GetContactRequests(
      int cmid,
      Action<List<ContactRequestView>> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, cmid);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IRelationshipWebServiceContract", "UberStrike.DataCenter.WebService.CWS.RelationshipWebServiceContract.svc", nameof (GetContactRequests), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ListProxy<ContactRequestView>.Deserialize((Stream) new MemoryStream(data), new ListProxy<ContactRequestView>.Deserializer<ContactRequestView>(ContactRequestViewProxy.Deserialize)));
        }), handler));
      }
    }

    public static Coroutine AcceptContactRequest(
      int contactRequestId,
      int cmid,
      int applicationId,
      Action<ContactRequestAcceptView> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, contactRequestId);
        Int32Proxy.Serialize((Stream) bytes, cmid);
        Int32Proxy.Serialize((Stream) bytes, applicationId);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IRelationshipWebServiceContract", "UberStrike.DataCenter.WebService.CWS.RelationshipWebServiceContract.svc", nameof (AcceptContactRequest), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ContactRequestAcceptViewProxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine DeclineContactRequest(
      int contactRequestId,
      int cmid,
      Action<ContactRequestDeclineView> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, contactRequestId);
        Int32Proxy.Serialize((Stream) bytes, cmid);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IRelationshipWebServiceContract", "UberStrike.DataCenter.WebService.CWS.RelationshipWebServiceContract.svc", nameof (DeclineContactRequest), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ContactRequestDeclineViewProxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine DeleteContact(
      int cmid,
      int contactCmid,
      Action<MemberOperationResult> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, cmid);
        Int32Proxy.Serialize((Stream) bytes, contactCmid);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IRelationshipWebServiceContract", "UberStrike.DataCenter.WebService.CWS.RelationshipWebServiceContract.svc", nameof (DeleteContact), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(EnumProxy<MemberOperationResult>.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine MoveContactToGroup(
      int cmid,
      int contactCmid,
      int previousGroupId,
      int newGroupId,
      Action<MemberOperationResult> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, cmid);
        Int32Proxy.Serialize((Stream) bytes, contactCmid);
        Int32Proxy.Serialize((Stream) bytes, previousGroupId);
        Int32Proxy.Serialize((Stream) bytes, newGroupId);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IRelationshipWebServiceContract", "UberStrike.DataCenter.WebService.CWS.RelationshipWebServiceContract.svc", nameof (MoveContactToGroup), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(EnumProxy<MemberOperationResult>.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine GetContactsByGroups(
      int cmid,
      int applicationId,
      Action<List<ContactGroupView>> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, cmid);
        Int32Proxy.Serialize((Stream) bytes, applicationId);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IRelationshipWebServiceContract", "UberStrike.DataCenter.WebService.CWS.RelationshipWebServiceContract.svc", nameof (GetContactsByGroups), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ListProxy<ContactGroupView>.Deserialize((Stream) new MemoryStream(data), new ListProxy<ContactGroupView>.Deserializer<ContactGroupView>(ContactGroupViewProxy.Deserialize)));
        }), handler));
      }
    }
  }
}
