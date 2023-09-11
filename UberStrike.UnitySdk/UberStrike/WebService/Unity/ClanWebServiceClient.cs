// Decompiled with JetBrains decompiler
// Type: UberStrike.WebService.Unity.ClanWebServiceClient
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
  public static class ClanWebServiceClient
  {
    public static Coroutine IsMemberPartOfGroup(
      int cmuneId,
      int groupId,
      Action<bool> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, cmuneId);
        Int32Proxy.Serialize((Stream) bytes, groupId);
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "ClanWebService", nameof (IsMemberPartOfGroup), Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(BooleanProxy.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector))));
        }), handler));
      }
    }

    public static Coroutine IsMemberPartOfAnyGroup(
      int cmuneId,
      int applicationId,
      Action<bool> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, cmuneId);
        Int32Proxy.Serialize((Stream) bytes, applicationId);
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "ClanWebService", nameof (IsMemberPartOfAnyGroup), Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(BooleanProxy.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector))));
        }), handler));
      }
    }

    public static Coroutine GetClan(
      int groupId,
      Action<ClanView> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, groupId);
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "ClanWebService", nameof (GetClan), Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ClanViewProxy.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector))));
        }), handler));
      }
    }

    public static Coroutine UpdateMemberPosition(
      MemberPositionUpdateView updateMemberPositionData,
      Action<int> callback,
      Action<Exception> handler)
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        MemberPositionUpdateViewProxy.Serialize((Stream) memoryStream, updateMemberPositionData);
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "ClanWebService", nameof (UpdateMemberPosition), Cryptography.RijndaelEncrypt(memoryStream.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(Int32Proxy.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector))));
        }), handler));
      }
    }

    public static Coroutine InviteMemberToJoinAGroup(
      int clanId,
      int inviterCmid,
      int inviteeCmid,
      string message,
      Action<int> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, clanId);
        Int32Proxy.Serialize((Stream) bytes, inviterCmid);
        Int32Proxy.Serialize((Stream) bytes, inviteeCmid);
        StringProxy.Serialize((Stream) bytes, message);
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "ClanWebService", nameof (InviteMemberToJoinAGroup), Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(Int32Proxy.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector))));
        }), handler));
      }
    }

    public static Coroutine AcceptClanInvitation(
      int clanInvitationId,
      int cmid,
      Action<ClanRequestAcceptView> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, clanInvitationId);
        Int32Proxy.Serialize((Stream) bytes, cmid);
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "ClanWebService", nameof (AcceptClanInvitation), Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ClanRequestAcceptViewProxy.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector))));
        }), handler));
      }
    }

    public static Coroutine DeclineClanInvitation(
      int clanInvitationId,
      int cmid,
      Action<ClanRequestDeclineView> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, clanInvitationId);
        Int32Proxy.Serialize((Stream) bytes, cmid);
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "ClanWebService", nameof (DeclineClanInvitation), Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ClanRequestDeclineViewProxy.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector))));
        }), handler));
      }
    }

    public static Coroutine KickMemberFromClan(
      int groupId,
      int cmidTakingAction,
      int cmidToKick,
      Action<int> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, groupId);
        Int32Proxy.Serialize((Stream) bytes, cmidTakingAction);
        Int32Proxy.Serialize((Stream) bytes, cmidToKick);
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "ClanWebService", nameof (KickMemberFromClan), Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(Int32Proxy.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector))));
        }), handler));
      }
    }

    public static Coroutine DisbandGroup(
      int groupId,
      int cmidTakingAction,
      Action<int> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, groupId);
        Int32Proxy.Serialize((Stream) bytes, cmidTakingAction);
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "ClanWebService", nameof (DisbandGroup), Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(Int32Proxy.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector))));
        }), handler));
      }
    }

    public static Coroutine LeaveAClan(
      int groupId,
      int cmid,
      Action<int> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, groupId);
        Int32Proxy.Serialize((Stream) bytes, cmid);
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "ClanWebService", nameof (LeaveAClan), Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(Int32Proxy.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector))));
        }), handler));
      }
    }

    public static Coroutine GetMyClanId(
      int cmid,
      int applicationId,
      Action<int> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, cmid);
        Int32Proxy.Serialize((Stream) bytes, applicationId);
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "ClanWebService", nameof (GetMyClanId), Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(Int32Proxy.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector))));
        }), handler));
      }
    }

    public static Coroutine CancelInvitation(
      int groupInvitationId,
      int cmid,
      Action<int> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, groupInvitationId);
        Int32Proxy.Serialize((Stream) bytes, cmid);
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "ClanWebService", nameof (CancelInvitation), Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(Int32Proxy.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector))));
        }), handler));
      }
    }

    public static Coroutine CancelRequest(
      int groupRequestId,
      Action<int> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, groupRequestId);
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "ClanWebService", nameof (CancelRequest), Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(Int32Proxy.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector))));
        }), handler));
      }
    }

    public static Coroutine GetAllGroupInvitations(
      int inviteeCmid,
      int applicationId,
      Action<List<GroupInvitationView>> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, inviteeCmid);
        Int32Proxy.Serialize((Stream) bytes, applicationId);
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "ClanWebService", nameof (GetAllGroupInvitations), Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ListProxy<GroupInvitationView>.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector)), new ListProxy<GroupInvitationView>.Deserializer<GroupInvitationView>(GroupInvitationViewProxy.Deserialize)));
        }), handler));
      }
    }

    public static Coroutine GetPendingGroupInvitations(
      int groupId,
      Action<List<GroupInvitationView>> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, groupId);
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "ClanWebService", nameof (GetPendingGroupInvitations), Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ListProxy<GroupInvitationView>.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector)), new ListProxy<GroupInvitationView>.Deserializer<GroupInvitationView>(GroupInvitationViewProxy.Deserialize)));
        }), handler));
      }
    }

    public static Coroutine CreateClan(
      GroupCreationView createClanData,
      Action<ClanCreationReturnView> callback,
      Action<Exception> handler)
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        GroupCreationViewProxy.Serialize((Stream) memoryStream, createClanData);
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "ClanWebService", nameof (CreateClan), Cryptography.RijndaelEncrypt(memoryStream.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ClanCreationReturnViewProxy.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector))));
        }), handler));
      }
    }

    public static Coroutine TransferOwnership(
      int groupId,
      int previousLeaderCmid,
      int newLeaderCmid,
      Action<int> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, groupId);
        Int32Proxy.Serialize((Stream) bytes, previousLeaderCmid);
        Int32Proxy.Serialize((Stream) bytes, newLeaderCmid);
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "ClanWebService", nameof (TransferOwnership), Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(Int32Proxy.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector))));
        }), handler));
      }
    }

    public static Coroutine CanOwnAClan(int cmid, Action<int> callback, Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, cmid);
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "ClanWebService", nameof (CanOwnAClan), Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(Int32Proxy.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector))));
        }), handler));
      }
    }

    public static Coroutine test(Action<int> callback, Action<Exception> handler)
    {
      using (MemoryStream memoryStream = new MemoryStream())
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "ClanWebService", nameof (test), Cryptography.RijndaelEncrypt(memoryStream.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(Int32Proxy.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector))));
        }), handler));
    }
  }
}
