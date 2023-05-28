// Decompiled with JetBrains decompiler
// Type: UberStrike.WebService.Unity.UserWebServiceClient
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using Cmune.Core.Models.Views;
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
      string authToken,
      string name,
      string locale,
      string machineId,
      Action<MemberOperationResult> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        StringProxy.Serialize((Stream) bytes, authToken);
        StringProxy.Serialize((Stream) bytes, name);
        StringProxy.Serialize((Stream) bytes, locale);
        StringProxy.Serialize((Stream) bytes, machineId);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IUserWebServiceContract", "UserWebService", nameof (ChangeMemberName), bytes.ToArray(), (Action<byte[]>) (data =>
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
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IUserWebServiceContract", "UserWebService", nameof (IsDuplicateMemberName), bytes.ToArray(), (Action<byte[]>) (data =>
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
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IUserWebServiceContract", "UserWebService", nameof (GenerateNonDuplicatedMemberNames), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ListProxy<string>.Deserialize((Stream) new MemoryStream(data), new ListProxy<string>.Deserializer<string>(StringProxy.Deserialize)));
        }), handler));
      }
    }

    public static Coroutine GetMemberWallet(
      string authToken,
      Action<MemberWalletView> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        StringProxy.Serialize((Stream) bytes, authToken);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IUserWebServiceContract", "UserWebService", nameof (GetMemberWallet), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(MemberWalletViewProxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine GetInventory(
      string authToken,
      Action<List<ItemInventoryView>> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        StringProxy.Serialize((Stream) bytes, authToken);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IUserWebServiceContract", "UserWebService", nameof (GetInventory), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ListProxy<ItemInventoryView>.Deserialize((Stream) new MemoryStream(data), new ListProxy<ItemInventoryView>.Deserializer<ItemInventoryView>(ItemInventoryViewProxy.Deserialize)));
        }), handler));
      }
    }

    public static Coroutine GetCurrencyDeposits(
      string authToken,
      int pageIndex,
      int elementPerPage,
      Action<CurrencyDepositsViewModel> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        StringProxy.Serialize((Stream) bytes, authToken);
        Int32Proxy.Serialize((Stream) bytes, pageIndex);
        Int32Proxy.Serialize((Stream) bytes, elementPerPage);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IUserWebServiceContract", "UserWebService", nameof (GetCurrencyDeposits), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(CurrencyDepositsViewModelProxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine GetItemTransactions(
      string authToken,
      int pageIndex,
      int elementPerPage,
      Action<ItemTransactionsViewModel> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        StringProxy.Serialize((Stream) bytes, authToken);
        Int32Proxy.Serialize((Stream) bytes, pageIndex);
        Int32Proxy.Serialize((Stream) bytes, elementPerPage);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IUserWebServiceContract", "UserWebService", nameof (GetItemTransactions), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ItemTransactionsViewModelProxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine GetPointsDeposits(
      string authToken,
      int pageIndex,
      int elementPerPage,
      Action<PointDepositsViewModel> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        StringProxy.Serialize((Stream) bytes, authToken);
        Int32Proxy.Serialize((Stream) bytes, pageIndex);
        Int32Proxy.Serialize((Stream) bytes, elementPerPage);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IUserWebServiceContract", "UserWebService", nameof (GetPointsDeposits), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(PointDepositsViewModelProxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine GetLoadout(
      string authToken,
      Action<LoadoutView> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        StringProxy.Serialize((Stream) bytes, authToken);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IUserWebServiceContract", "UserWebService", nameof (GetLoadout), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(LoadoutViewProxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine SetLoadout(
      string authToken,
      LoadoutView loadoutView,
      Action<MemberOperationResult> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        StringProxy.Serialize((Stream) bytes, authToken);
        LoadoutViewProxy.Serialize((Stream) bytes, loadoutView);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IUserWebServiceContract", "UserWebService", nameof (SetLoadout), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(EnumProxy<MemberOperationResult>.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine GetMember(
      string authToken,
      Action<UberstrikeUserViewModel> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        StringProxy.Serialize((Stream) bytes, authToken);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IUserWebServiceContract", "UserWebService", nameof (GetMember), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(UberstrikeUserViewModelProxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine GetMemberSessionData(
      string authToken,
      Action<MemberSessionDataView> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        StringProxy.Serialize((Stream) bytes, authToken);
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IUserWebServiceContract", "UserWebService", nameof (GetMemberSessionData), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(MemberSessionDataViewProxy.Deserialize((Stream) new MemoryStream(data)));
        }), handler));
      }
    }

    public static Coroutine GetMemberListSessionData(
      List<string> authTokens,
      Action<List<MemberSessionDataView>> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        ListProxy<string>.Serialize((Stream) bytes, (ICollection<string>) authTokens, new ListProxy<string>.Serializer<string>(StringProxy.Serialize));
        return MonoInstance.Mono.StartCoroutine(SoapClient.MakeRequest("IUserWebServiceContract", "UserWebService", nameof (GetMemberListSessionData), bytes.ToArray(), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ListProxy<MemberSessionDataView>.Deserialize((Stream) new MemoryStream(data), new ListProxy<MemberSessionDataView>.Deserializer<MemberSessionDataView>(MemberSessionDataViewProxy.Deserialize)));
        }), handler));
      }
    }
  }
}
