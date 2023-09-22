
using Cmune.DataCenter.Common.Entities;
using Cmune.Util;
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
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IPrivateMessageWebServiceContract", "PrivateMessageWebService", "GetAllMessageThreadsForUser_1", Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ListProxy<MessageThreadView>.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector)), new ListProxy<MessageThreadView>.Deserializer<MessageThreadView>(MessageThreadViewProxy.Deserialize)));
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
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IPrivateMessageWebServiceContract", "PrivateMessageWebService", "GetAllMessageThreadsForUser_2", Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ListProxy<MessageThreadView>.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector)), new ListProxy<MessageThreadView>.Deserializer<MessageThreadView>(MessageThreadViewProxy.Deserialize)));
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
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IPrivateMessageWebServiceContract", "PrivateMessageWebService", nameof (GetThreadMessages), Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ListProxy<PrivateMessageView>.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector)), new ListProxy<PrivateMessageView>.Deserializer<PrivateMessageView>(PrivateMessageViewProxy.Deserialize)));
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
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IPrivateMessageWebServiceContract", "PrivateMessageWebService", nameof (SendMessage), Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(PrivateMessageViewProxy.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector))));
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
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IPrivateMessageWebServiceContract", "PrivateMessageWebService", nameof (GetMessageWithId), Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(PrivateMessageViewProxy.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector))));
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
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IPrivateMessageWebServiceContract", "PrivateMessageWebService", nameof (MarkThreadAsRead), Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
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
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IPrivateMessageWebServiceContract", "PrivateMessageWebService", nameof (DeleteThread), Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback();
        }), handler));
      }
    }
  }
}
