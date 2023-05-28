// Decompiled with JetBrains decompiler
// Type: UberStrike.WebService.Unity.UserWebServiceClient
// Assembly: UberStrike.DataCenter.UnitySdk, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: 1925C691-C9DE-44B0-95F4-3171E7957DDD
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.DataCenter.UnitySdk.dll

using Cmune.DataCenter.Common.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using UberStrike.Core.Serialization;
using UberStrike.Core.ViewModel;
using UberStrike.DataCenter.Common.Entities;
using UnityEngine;

namespace UberStrike.WebService.Unity
{
  public static class UserWebServiceClient
  {
    public static Coroutine ChangeMemberName(
      int cmid,
      string name,
      string locale,
      string machineId,
      Action<MemberOperationResult> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, cmid);
        StringProxy.Serialize((Stream) bytes, name);
        StringProxy.Serialize((Stream) bytes, locale);
        StringProxy.Serialize((Stream) bytes, machineId);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IUserWebServiceContract", "UberStrike.DataCenter.WebService.CWS.UserWebServiceContract.svc", nameof (ChangeMemberName), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(EnumProxy<MemberOperationResult>.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine IsDuplicateMemberName(
      string username,
      Action<bool> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        StringProxy.Serialize((Stream) bytes, username);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IUserWebServiceContract", "UberStrike.DataCenter.WebService.CWS.UserWebServiceContract.svc", nameof (IsDuplicateMemberName), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(BooleanProxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine GenerateNonDuplicatedMemberNames(
      string username,
      Action<List<string>> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        StringProxy.Serialize((Stream) bytes, username);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IUserWebServiceContract", "UberStrike.DataCenter.WebService.CWS.UserWebServiceContract.svc", nameof (GenerateNonDuplicatedMemberNames), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ListProxy<string>.Deserialize((Stream) new MemoryStream(data), new ListProxy<string>.Deserializer<string>(StringProxy.Deserialize)));
        }), handler));
      }
    }

    public static Coroutine GetMemberWallet(
      int cmid,
      Action<MemberWalletView> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, cmid);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IUserWebServiceContract", "UberStrike.DataCenter.WebService.CWS.UserWebServiceContract.svc", nameof (GetMemberWallet), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(MemberWalletViewProxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine GetInventory(
      int cmid,
      Action<List<ItemInventoryView>> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, cmid);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IUserWebServiceContract", "UberStrike.DataCenter.WebService.CWS.UserWebServiceContract.svc", nameof (GetInventory), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ListProxy<ItemInventoryView>.Deserialize((Stream) new MemoryStream(data), new ListProxy<ItemInventoryView>.Deserializer<ItemInventoryView>(ItemInventoryViewProxy.Deserialize)));
        }), handler));
      }
    }

    public static Coroutine GetCurrencyDeposits(
      int cmid,
      int pageIndex,
      int elementPerPage,
      Action<CurrencyDepositsViewModel> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, cmid);
        Int32Proxy.Serialize((Stream) bytes, pageIndex);
        Int32Proxy.Serialize((Stream) bytes, elementPerPage);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IUserWebServiceContract", "UberStrike.DataCenter.WebService.CWS.UserWebServiceContract.svc", nameof (GetCurrencyDeposits), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(CurrencyDepositsViewModelProxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine GetItemTransactions(
      int cmid,
      int pageIndex,
      int elementPerPage,
      Action<ItemTransactionsViewModel> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, cmid);
        Int32Proxy.Serialize((Stream) bytes, pageIndex);
        Int32Proxy.Serialize((Stream) bytes, elementPerPage);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IUserWebServiceContract", "UberStrike.DataCenter.WebService.CWS.UserWebServiceContract.svc", nameof (GetItemTransactions), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ItemTransactionsViewModelProxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine GetPointsDeposits(
      int cmid,
      int pageIndex,
      int elementPerPage,
      Action<PointDepositsViewModel> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, cmid);
        Int32Proxy.Serialize((Stream) bytes, pageIndex);
        Int32Proxy.Serialize((Stream) bytes, elementPerPage);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IUserWebServiceContract", "UberStrike.DataCenter.WebService.CWS.UserWebServiceContract.svc", nameof (GetPointsDeposits), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(PointDepositsViewModelProxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine GetLoadout(
      int cmid,
      Action<LoadoutView> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, cmid);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IUserWebServiceContract", "UberStrike.DataCenter.WebService.CWS.UserWebServiceContract.svc", nameof (GetLoadout), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(LoadoutViewProxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine SetLoadout(
      LoadoutView loadoutView,
      Action<MemberOperationResult> callback,
      Action<Exception> handler)
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        LoadoutViewProxy.Serialize((Stream) memoryStream, loadoutView);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IUserWebServiceContract", "UberStrike.DataCenter.WebService.CWS.UserWebServiceContract.svc", nameof (SetLoadout), memoryStream.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(EnumProxy<MemberOperationResult>.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine SetScore(
      MatchView scoringView,
      Action callback,
      Action<Exception> handler)
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        MatchViewProxy.Serialize((Stream) memoryStream, scoringView);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IUserWebServiceContract", "UberStrike.DataCenter.WebService.CWS.UserWebServiceContract.svc", nameof (SetScore), memoryStream.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback();
        }), handler));
      }
    }

    public static Coroutine GetXPEventsView(
      Action<Dictionary<int, PlayerXPEventView>> callback,
      Action<Exception> handler)
    {
      using (MemoryStream memoryStream = new MemoryStream())
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IUserWebServiceContract", "UberStrike.DataCenter.WebService.CWS.UserWebServiceContract.svc", nameof (GetXPEventsView), memoryStream.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(DictionaryProxy<int, PlayerXPEventView>.Deserialize((Stream) new MemoryStream(data), new DictionaryProxy<int, PlayerXPEventView>.Deserializer<int>(Int32Proxy.Deserialize), new DictionaryProxy<int, PlayerXPEventView>.Deserializer<PlayerXPEventView>(PlayerXPEventViewProxy.Deserialize)));
        }), handler));
    }

    public static Coroutine GetLevelCapsView(
      Action<List<PlayerLevelCapView>> callback,
      Action<Exception> handler)
    {
      using (MemoryStream memoryStream = new MemoryStream())
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IUserWebServiceContract", "UberStrike.DataCenter.WebService.CWS.UserWebServiceContract.svc", nameof (GetLevelCapsView), memoryStream.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ListProxy<PlayerLevelCapView>.Deserialize((Stream) new MemoryStream(data), new ListProxy<PlayerLevelCapView>.Deserializer<PlayerLevelCapView>(PlayerLevelCapViewProxy.Deserialize)));
        }), handler));
    }

    public static Coroutine GetMember(
      int cmid,
      Action<UberstrikeUserViewModel> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, cmid);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IUserWebServiceContract", "UberStrike.DataCenter.WebService.CWS.UserWebServiceContract.svc", nameof (GetMember), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(UberstrikeUserViewModelProxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }
  }
}
