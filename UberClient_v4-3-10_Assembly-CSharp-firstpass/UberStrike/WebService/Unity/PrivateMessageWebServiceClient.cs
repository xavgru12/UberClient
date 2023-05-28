// Decompiled with JetBrains decompiler
// Type: UberStrike.WebService.Unity.PrivateMessageWebServiceClient
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
  public static class PrivateMessageWebServiceClient
  {
    public static Coroutine GetAllMessageThreadsForUser(
      string authToken,
      int pageNumber,
      Action<List<MessageThreadView>> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        StringProxy.Serialize((Stream) bytes, authToken);
        Int32Proxy.Serialize((Stream) bytes, pageNumber);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IPrivateMessageWebServiceContract", "PrivateMessageWebService", nameof (GetAllMessageThreadsForUser), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ListProxy<MessageThreadView>.Deserialize((Stream) new MemoryStream(data), new ListProxy<MessageThreadView>.Deserializer<MessageThreadView>(MessageThreadViewProxy.Deserialize)));
        }), handler));
      }
    }

    public static Coroutine GetThreadMessages(
      string authToken,
      int otherCmid,
      int pageNumber,
      Action<List<PrivateMessageView>> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        StringProxy.Serialize((Stream) bytes, authToken);
        Int32Proxy.Serialize((Stream) bytes, otherCmid);
        Int32Proxy.Serialize((Stream) bytes, pageNumber);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IPrivateMessageWebServiceContract", "PrivateMessageWebService", nameof (GetThreadMessages), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ListProxy<PrivateMessageView>.Deserialize((Stream) new MemoryStream(data), new ListProxy<PrivateMessageView>.Deserializer<PrivateMessageView>(PrivateMessageViewProxy.Deserialize)));
        }), handler));
      }
    }

    public static Coroutine SendMessage(
      string authToken,
      int receiverCmid,
      string content,
      Action<PrivateMessageView> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        StringProxy.Serialize((Stream) bytes, authToken);
        Int32Proxy.Serialize((Stream) bytes, receiverCmid);
        StringProxy.Serialize((Stream) bytes, content);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IPrivateMessageWebServiceContract", "PrivateMessageWebService", nameof (SendMessage), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(PrivateMessageViewProxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine GetMessageWithIdForCmid(
      string authToken,
      int messageId,
      Action<PrivateMessageView> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        StringProxy.Serialize((Stream) bytes, authToken);
        Int32Proxy.Serialize((Stream) bytes, messageId);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IPrivateMessageWebServiceContract", "PrivateMessageWebService", nameof (GetMessageWithIdForCmid), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(PrivateMessageViewProxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine MarkThreadAsRead(
      string authToken,
      int otherCmid,
      Action callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        StringProxy.Serialize((Stream) bytes, authToken);
        Int32Proxy.Serialize((Stream) bytes, otherCmid);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IPrivateMessageWebServiceContract", "PrivateMessageWebService", nameof (MarkThreadAsRead), bytes.ToArray(), (Action<byte[]>) (_param1 =>
        {
          if (callback == null)
            return;
          callback();
        }), handler));
      }
    }

    public static Coroutine DeleteThread(
      string authToken,
      int otherCmid,
      Action callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        StringProxy.Serialize((Stream) bytes, authToken);
        Int32Proxy.Serialize((Stream) bytes, otherCmid);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IPrivateMessageWebServiceContract", "PrivateMessageWebService", nameof (DeleteThread), bytes.ToArray(), (Action<byte[]>) (_param1 =>
        {
          if (callback == null)
            return;
          callback();
        }), handler));
      }
    }
  }
}
