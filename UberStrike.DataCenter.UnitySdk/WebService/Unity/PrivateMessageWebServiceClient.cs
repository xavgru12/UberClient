// Decompiled with JetBrains decompiler
// Type: UberStrike.WebService.Unity.PrivateMessageWebServiceClient
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
  public static class PrivateMessageWebServiceClient
  {
    [Obsolete("Please define a page number!")]
    public static Coroutine GetAllMessageThreadsForUser(
      int cmid,
      Action<List<MessageThreadView>> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, cmid);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IPrivateMessageWebServiceContract", "PrivateMessageWebService", "GetAllMessageThreadsForUser_1", bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ListProxy<MessageThreadView>.Deserialize((Stream) new MemoryStream(data), new ListProxy<MessageThreadView>.Deserializer<MessageThreadView>(MessageThreadViewProxy.Deserialize)));
        }), handler));
      }
    }

    public static Coroutine GetAllMessageThreadsForUser(
      int cmid,
      int pageNumber,
      Action<List<MessageThreadView>> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, cmid);
        Int32Proxy.Serialize((Stream) bytes, pageNumber);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IPrivateMessageWebServiceContract", "PrivateMessageWebService", "GetAllMessageThreadsForUser_2", bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ListProxy<MessageThreadView>.Deserialize((Stream) new MemoryStream(data), new ListProxy<MessageThreadView>.Deserializer<MessageThreadView>(MessageThreadViewProxy.Deserialize)));
        }), handler));
      }
    }

    public static Coroutine GetThreadMessages(
      int threadViewerCmid,
      int otherCmid,
      int pageNumber,
      Action<List<PrivateMessageView>> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, threadViewerCmid);
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
      int senderCmid,
      int receiverCmid,
      string content,
      Action<PrivateMessageView> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, senderCmid);
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

    public static Coroutine GetMessageWithId(
      int messageId,
      int requesterCmid,
      Action<PrivateMessageView> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, messageId);
        Int32Proxy.Serialize((Stream) bytes, requesterCmid);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IPrivateMessageWebServiceContract", "PrivateMessageWebService", nameof (GetMessageWithId), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(PrivateMessageViewProxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine MarkThreadAsRead(
      int threadViewerCmid,
      int otherCmid,
      Action callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, threadViewerCmid);
        Int32Proxy.Serialize((Stream) bytes, otherCmid);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IPrivateMessageWebServiceContract", "PrivateMessageWebService", nameof (MarkThreadAsRead), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback();
        }), handler));
      }
    }

    public static Coroutine DeleteThread(
      int threadViewerCmid,
      int otherCmid,
      Action callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, threadViewerCmid);
        Int32Proxy.Serialize((Stream) bytes, otherCmid);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IPrivateMessageWebServiceContract", "PrivateMessageWebService", nameof (DeleteThread), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback();
        }), handler));
      }
    }
  }
}
