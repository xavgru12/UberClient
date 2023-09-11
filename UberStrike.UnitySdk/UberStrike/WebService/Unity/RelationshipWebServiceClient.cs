// Decompiled with JetBrains decompiler
// Type: UberStrike.WebService.Unity.RelationshipWebServiceClient
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using Cmune.DataCenter.Common.Entities;
using Cmune.Util;
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
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IRelationshipWebServiceContract", "RelationshipWebService", nameof (SendContactRequest), Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(Int32Proxy.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector))));
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
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IRelationshipWebServiceContract", "RelationshipWebService", nameof (GetContactRequests), Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ListProxy<ContactRequestView>.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector)), new ListProxy<ContactRequestView>.Deserializer<ContactRequestView>(ContactRequestViewProxy.Deserialize)));
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
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IRelationshipWebServiceContract", "RelationshipWebService", nameof (AcceptContactRequest), Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ContactRequestAcceptViewProxy.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector))));
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
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IRelationshipWebServiceContract", "RelationshipWebService", nameof (DeclineContactRequest), Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ContactRequestDeclineViewProxy.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector))));
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
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IRelationshipWebServiceContract", "RelationshipWebService", nameof (DeleteContact), Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(EnumProxy<MemberOperationResult>.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector))));
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
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IRelationshipWebServiceContract", "RelationshipWebService", nameof (MoveContactToGroup), Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(EnumProxy<MemberOperationResult>.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector))));
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
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IRelationshipWebServiceContract", "RelationshipWebService", nameof (GetContactsByGroups), Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ListProxy<ContactGroupView>.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector)), new ListProxy<ContactGroupView>.Deserializer<ContactGroupView>(ContactGroupViewProxy.Deserialize)));
        }), handler));
      }
    }
  }
}
