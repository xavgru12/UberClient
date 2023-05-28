// Decompiled with JetBrains decompiler
// Type: UberStrike.WebService.Unity.ClanWebServiceClient
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
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "UberStrike.DataCenter.WebService.CWS.ClanWebServiceContract.svc", nameof (IsMemberPartOfGroup), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(BooleanProxy.Deserialize((Stream) new MemoryStream(data)));
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
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "UberStrike.DataCenter.WebService.CWS.ClanWebServiceContract.svc", nameof (IsMemberPartOfAnyGroup), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(BooleanProxy.Deserialize((Stream) new MemoryStream(data)));
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
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "UberStrike.DataCenter.WebService.CWS.ClanWebServiceContract.svc", nameof (GetClan), bytes.ToArray(), (Action<byte[]>) (data =>
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
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "UberStrike.DataCenter.WebService.CWS.ClanWebServiceContract.svc", nameof (UpdateMemberPosition), memoryStream.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(Int32Proxy.Deserialize((Stream) new MemoryStream(data)));
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
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "UberStrike.DataCenter.WebService.CWS.ClanWebServiceContract.svc", nameof (InviteMemberToJoinAGroup), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(Int32Proxy.Deserialize((Stream) new MemoryStream(data)));
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
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "UberStrike.DataCenter.WebService.CWS.ClanWebServiceContract.svc", nameof (AcceptClanInvitation), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ClanRequestAcceptViewProxy.Deserialize((Stream) new MemoryStream(data)));
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
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "UberStrike.DataCenter.WebService.CWS.ClanWebServiceContract.svc", nameof (DeclineClanInvitation), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ClanRequestDeclineViewProxy.Deserialize((Stream) new MemoryStream(data)));
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
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "UberStrike.DataCenter.WebService.CWS.ClanWebServiceContract.svc", nameof (KickMemberFromClan), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(Int32Proxy.Deserialize((Stream) new MemoryStream(data)));
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
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "UberStrike.DataCenter.WebService.CWS.ClanWebServiceContract.svc", nameof (DisbandGroup), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(Int32Proxy.Deserialize((Stream) new MemoryStream(data)));
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
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "UberStrike.DataCenter.WebService.CWS.ClanWebServiceContract.svc", nameof (LeaveAClan), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(Int32Proxy.Deserialize((Stream) new MemoryStream(data)));
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
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "UberStrike.DataCenter.WebService.CWS.ClanWebServiceContract.svc", nameof (GetMyClanId), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(Int32Proxy.Deserialize((Stream) new MemoryStream(data)));
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
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "UberStrike.DataCenter.WebService.CWS.ClanWebServiceContract.svc", nameof (CancelInvitation), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(Int32Proxy.Deserialize((Stream) new MemoryStream(data)));
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
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "UberStrike.DataCenter.WebService.CWS.ClanWebServiceContract.svc", nameof (GetAllGroupInvitations), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ListProxy<GroupInvitationView>.Deserialize((Stream) new MemoryStream(data), new ListProxy<GroupInvitationView>.Deserializer<GroupInvitationView>(GroupInvitationViewProxy.Deserialize)));
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
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "UberStrike.DataCenter.WebService.CWS.ClanWebServiceContract.svc", nameof (GetPendingGroupInvitations), bytes.ToArray(), (Action<byte[]>) (data =>
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
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "UberStrike.DataCenter.WebService.CWS.ClanWebServiceContract.svc", nameof (CreateClan), memoryStream.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ClanCreationReturnViewProxy.Deserialize((Stream) new MemoryStream(data)));
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
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "UberStrike.DataCenter.WebService.CWS.ClanWebServiceContract.svc", nameof (TransferOwnership), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(Int32Proxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine CanOwnAClan(int cmid, Action<int> callback, Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, cmid);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "UberStrike.DataCenter.WebService.CWS.ClanWebServiceContract.svc", nameof (CanOwnAClan), bytes.ToArray(), (Action<byte[]>) (data =>
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
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IClanWebServiceContract", "UberStrike.DataCenter.WebService.CWS.ClanWebServiceContract.svc", nameof (test), memoryStream.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(Int32Proxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
    }
  }
}
