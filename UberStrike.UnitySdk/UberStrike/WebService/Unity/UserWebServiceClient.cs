// Decompiled with JetBrains decompiler
// Type: UberStrike.WebService.Unity.UserWebServiceClient
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using Cmune.DataCenter.Common.Entities;
using Cmune.Util;
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
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IUserWebServiceContract", "UserWebService", nameof (ChangeMemberName), Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(EnumProxy<MemberOperationResult>.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector))));
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
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IUserWebServiceContract", "UserWebService", nameof (IsDuplicateMemberName), Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(BooleanProxy.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector))));
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
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IUserWebServiceContract", "UserWebService", nameof (GenerateNonDuplicatedMemberNames), Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ListProxy<string>.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector)), new ListProxy<string>.Deserializer<string>(StringProxy.Deserialize)));
        }), handler));
      }
    }

    public static Coroutine GetPublicProfile(
      int cmid,
      int applicationId,
      Action<PublicProfileView> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, cmid);
        Int32Proxy.Serialize((Stream) bytes, applicationId);
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IUserWebServiceContract", "UserWebService", nameof (GetPublicProfile), Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(PublicProfileViewProxy.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector))));
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
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IUserWebServiceContract", "UserWebService", nameof (GetMemberWallet), Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(MemberWalletViewProxy.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector))));
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
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IUserWebServiceContract", "UserWebService", nameof (GetInventory), Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ListProxy<ItemInventoryView>.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector)), new ListProxy<ItemInventoryView>.Deserializer<ItemInventoryView>(ItemInventoryViewProxy.Deserialize)));
        }), handler));
      }
    }

    public static Coroutine ReportMember(
      MemberReportView memberReport,
      Action<bool> callback,
      Action<Exception> handler)
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        MemberReportViewProxy.Serialize((Stream) memoryStream, memberReport);
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IUserWebServiceContract", "UserWebService", nameof (ReportMember), Cryptography.RijndaelEncrypt(memoryStream.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(BooleanProxy.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector))));
        }), handler));
      }
    }

    public static Coroutine FindMembers(
      string name,
      int maxResults,
      Action<Dictionary<int, string>> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        StringProxy.Serialize((Stream) bytes, name);
        Int32Proxy.Serialize((Stream) bytes, maxResults);
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IUserWebServiceContract", "UserWebService", nameof (FindMembers), Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(DictionaryProxy<int, string>.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector)), new DictionaryProxy<int, string>.Deserializer<int>(Int32Proxy.Deserialize), new DictionaryProxy<int, string>.Deserializer<string>(StringProxy.Deserialize)));
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
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IUserWebServiceContract", "UserWebService", nameof (GetCurrencyDeposits), Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(CurrencyDepositsViewModelProxy.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector))));
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
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IUserWebServiceContract", "UserWebService", nameof (GetItemTransactions), Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ItemTransactionsViewModelProxy.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector))));
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
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IUserWebServiceContract", "UserWebService", nameof (GetPointsDeposits), Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(PointDepositsViewModelProxy.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector))));
        }), handler));
      }
    }

    public static Coroutine GetUserAndTopStats(
      int cmid,
      Action<List<PlayerCardView>> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, cmid);
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IUserWebServiceContract", "UserWebService", nameof (GetUserAndTopStats), Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ListProxy<PlayerCardView>.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector)), new ListProxy<PlayerCardView>.Deserializer<PlayerCardView>(PlayerCardViewProxy.Deserialize)));
        }), handler));
      }
    }

    public static Coroutine GetRealTimeStatistics(
      int cmid,
      Action<PlayerCardView> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, cmid);
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IUserWebServiceContract", "UserWebService", nameof (GetRealTimeStatistics), Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(PlayerCardViewProxy.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector))));
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
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IUserWebServiceContract", "UserWebService", nameof (SetScore), Cryptography.RijndaelEncrypt(memoryStream.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback();
        }), handler));
      }
    }

    public static Coroutine GetMember(
      int cmid,
      Action<UberstrikeUserViewModel> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, cmid);
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IUserWebServiceContract", "UserWebService", nameof (GetMember), Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(UberstrikeUserViewModelProxy.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector))));
        }), handler));
      }
    }

    public static Coroutine GetStatistics(
      int cmid,
      Action<PlayerCardView> callback,
      Action<Exception> handler)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, cmid);
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IUserWebServiceContract", "UserWebService", nameof (GetStatistics), Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(PlayerCardViewProxy.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector))));
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
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IUserWebServiceContract", "UserWebService", nameof (GetLoadout), Cryptography.RijndaelEncrypt(bytes.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(LoadoutViewProxy.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector))));
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
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IUserWebServiceContract", "UserWebService", nameof (SetLoadout), Cryptography.RijndaelEncrypt(memoryStream.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(EnumProxy<MemberOperationResult>.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector))));
        }), handler));
      }
    }

    public static Coroutine GetXPEventsView(
      Action<Dictionary<int, PlayerXPEventView>> callback,
      Action<Exception> handler)
    {
      using (MemoryStream memoryStream = new MemoryStream())
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IUserWebServiceContract", "UserWebService", nameof (GetXPEventsView), Cryptography.RijndaelEncrypt(memoryStream.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(DictionaryProxy<int, PlayerXPEventView>.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector)), new DictionaryProxy<int, PlayerXPEventView>.Deserializer<int>(Int32Proxy.Deserialize), new DictionaryProxy<int, PlayerXPEventView>.Deserializer<PlayerXPEventView>(PlayerXPEventViewProxy.Deserialize)));
        }), handler));
    }

    public static Coroutine GetLevelCapsView(
      Action<List<PlayerLevelCapView>> callback,
      Action<Exception> handler)
    {
      using (MemoryStream memoryStream = new MemoryStream())
        return WebServiceStarter.Mono.StartCoroutine(SoapClient.MakeRequest("IUserWebServiceContract", "UserWebService", nameof (GetLevelCapsView), Cryptography.RijndaelEncrypt(memoryStream.ToArray(), Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector), (Action<byte[]>) (data =>
        {
          if (callback == null)
            return;
          callback(ListProxy<PlayerLevelCapView>.Deserialize((Stream) new MemoryStream(Cryptography.RijndaelDecrypt(data, Configuration.EncryptionPassPhrase, Configuration.EncryptionInitVector)), new ListProxy<PlayerLevelCapView>.Deserializer<PlayerLevelCapView>(PlayerLevelCapViewProxy.Deserialize)));
        }), handler));
    }
  }
}
