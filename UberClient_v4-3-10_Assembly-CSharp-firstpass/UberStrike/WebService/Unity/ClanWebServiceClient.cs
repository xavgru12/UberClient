// Decompiled with JetBrains decompiler
// Type: UberStrike.WebService.Unity.ClanWebServiceClient
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
  public static class ClanWebServiceClient
  {
    public static Coroutine GetOwnClan(
      string authToken,
      int groupId,
      Action<ClanView> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        StringProxy.Serialize((Stream) bytes, authToken);
        Int32Proxy.Serialize((Stream) bytes, groupId);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "ClanWebService", nameof (GetOwnClan), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ClanViewProxy.Deserialize((Stream) new MemoryStream(data)));
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
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "ClanWebService", nameof (UpdateMemberPosition), memoryStream.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(Int32Proxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine InviteMemberToJoinAGroup(
      int clanId,
      string authToken,
      int inviteeCmid,
      string message,
      Action<int> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, clanId);
        StringProxy.Serialize((Stream) bytes, authToken);
        Int32Proxy.Serialize((Stream) bytes, inviteeCmid);
        StringProxy.Serialize((Stream) bytes, message);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "ClanWebService", nameof (InviteMemberToJoinAGroup), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(Int32Proxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine AcceptClanInvitation(
      int clanInvitationId,
      string authToken,
      Action<ClanRequestAcceptView> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, clanInvitationId);
        StringProxy.Serialize((Stream) bytes, authToken);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "ClanWebService", nameof (AcceptClanInvitation), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ClanRequestAcceptViewProxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine DeclineClanInvitation(
      int clanInvitationId,
      string authToken,
      Action<ClanRequestDeclineView> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, clanInvitationId);
        StringProxy.Serialize((Stream) bytes, authToken);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "ClanWebService", nameof (DeclineClanInvitation), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ClanRequestDeclineViewProxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine KickMemberFromClan(
      int groupId,
      string authToken,
      int cmidToKick,
      Action<int> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, groupId);
        StringProxy.Serialize((Stream) bytes, authToken);
        Int32Proxy.Serialize((Stream) bytes, cmidToKick);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "ClanWebService", nameof (KickMemberFromClan), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(Int32Proxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine DisbandGroup(
      int groupId,
      string authToken,
      Action<int> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, groupId);
        StringProxy.Serialize((Stream) bytes, authToken);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "ClanWebService", nameof (DisbandGroup), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(Int32Proxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine LeaveAClan(
      int groupId,
      string authToken,
      Action<int> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, groupId);
        StringProxy.Serialize((Stream) bytes, authToken);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "ClanWebService", nameof (LeaveAClan), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(Int32Proxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine GetMyClanId(
      string authToken,
      Action<int> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        StringProxy.Serialize((Stream) bytes, authToken);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "ClanWebService", nameof (GetMyClanId), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(Int32Proxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine CancelInvitation(
      int groupInvitationId,
      string authToken,
      Action<int> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, groupInvitationId);
        StringProxy.Serialize((Stream) bytes, authToken);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "ClanWebService", nameof (CancelInvitation), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(Int32Proxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine GetAllGroupInvitations(
      string authToken,
      Action<List<GroupInvitationView>> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        StringProxy.Serialize((Stream) bytes, authToken);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "ClanWebService", nameof (GetAllGroupInvitations), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ListProxy<GroupInvitationView>.Deserialize((Stream) new MemoryStream(data), new ListProxy<GroupInvitationView>.Deserializer<GroupInvitationView>(GroupInvitationViewProxy.Deserialize)));
        }), handler));
      }
    }

    public static Coroutine GetPendingGroupInvitations(
      int groupId,
      string authToken,
      Action<List<GroupInvitationView>> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, groupId);
        StringProxy.Serialize((Stream) bytes, authToken);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "ClanWebService", nameof (GetPendingGroupInvitations), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ListProxy<GroupInvitationView>.Deserialize((Stream) new MemoryStream(data), new ListProxy<GroupInvitationView>.Deserializer<GroupInvitationView>(GroupInvitationViewProxy.Deserialize)));
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
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "ClanWebService", nameof (CreateClan), memoryStream.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ClanCreationReturnViewProxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine TransferOwnership(
      int groupId,
      string authToken,
      int newLeaderCmid,
      Action<int> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, groupId);
        StringProxy.Serialize((Stream) bytes, authToken);
        Int32Proxy.Serialize((Stream) bytes, newLeaderCmid);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "ClanWebService", nameof (TransferOwnership), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(Int32Proxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine CanOwnAClan(
      string authToken,
      Action<int> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        StringProxy.Serialize((Stream) bytes, authToken);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "ClanWebService", nameof (CanOwnAClan), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(Int32Proxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine test(Action<int> callback, Action<Exception> handler)
    {
      using (MemoryStream memoryStream = new MemoryStream())
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "ClanWebService", nameof (test), memoryStream.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(Int32Proxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
    }
  }
}
