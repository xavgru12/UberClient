// Decompiled with JetBrains decompiler
// Type: ClanDataManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using System;
using System.Collections.Generic;
using UberStrike.Realtime.UnitySdk;
using UberStrike.WebService.Unity;
using UnityEngine;

public class ClanDataManager : Singleton<ClanDataManager>
{
  private ClanDataManager()
  {
  }

  public bool IsGetMyClanDone { get; set; }

  public bool HaveFriends => Singleton<PlayerDataManager>.Instance.FriendsCount >= 1;

  public bool HaveLevel => PlayerDataManager.PlayerLevel >= 4;

  public bool HaveLicense => Singleton<InventoryManager>.Instance.HasClanLicense();

  public float NextClanRefresh { get; private set; }

  private void HandleWebServiceError() => Debug.LogError((object) "Error getting Clan data for local player.");

  public void CheckCompleteClanData() => ClanWebServiceClient.GetMyClanId(PlayerDataManager.ClanIDSecure, 1, (Action<int>) (ev =>
  {
    PlayerDataManager.ClanID = ev;
    this.RefreshClanData(true);
  }), (Action<Exception>) (ex => DebugConsoleManager.SendExceptionReport(ex.Message, ex.StackTrace)));

  public void RefreshClanData(bool force = false)
  {
    if (!PlayerDataManager.IsPlayerInClan || !force && (double) this.NextClanRefresh >= (double) Time.time)
      return;
    this.NextClanRefresh = Time.time + 30f;
    ClanWebServiceClient.GetClan(PlayerDataManager.ClanIDSecure, (Action<ClanView>) (ev => this.SetClanData(ev)), (Action<Exception>) (ex => DebugConsoleManager.SendExceptionReport(ex.Message, ex.StackTrace)));
  }

  public void SetClanData(ClanView view)
  {
    PlayerDataManager.ClanData = view;
    CommConnectionManager.CommCenter.SendUpdatedActorInfo();
    CommConnectionManager.CommCenter.SendContactList();
    Singleton<ChatManager>.Instance.UpdateClanSection();
  }

  public bool IsProcessingWebservice { get; private set; }

  public void LeaveClan()
  {
    this.IsProcessingWebservice = true;
    ClanWebServiceClient.LeaveAClan(PlayerDataManager.ClanIDSecure, PlayerDataManager.CmidSecure, (Action<int>) (ev =>
    {
      this.IsProcessingWebservice = false;
      if (ev == 0)
      {
        CommConnectionManager.CommCenter.SendUpdateClanMembers((IEnumerable<ClanMemberView>) Singleton<PlayerDataManager>.Instance.ClanMembers);
        this.SetClanData((ClanView) null);
      }
      else
        PopupSystem.ShowMessage("Leave Clan", "There was an error removing you from this clan.\nErrorCode = " + (object) ev, PopupSystem.AlertType.OK);
    }), (Action<Exception>) (ex =>
    {
      this.IsProcessingWebservice = false;
      DebugConsoleManager.SendExceptionReport(ex.Message, ex.StackTrace);
    }));
  }

  public void DisbanClan()
  {
    this.IsProcessingWebservice = true;
    ClanWebServiceClient.DisbandGroup(PlayerDataManager.ClanIDSecure, PlayerDataManager.CmidSecure, (Action<int>) (ev =>
    {
      this.IsProcessingWebservice = false;
      if (ev != 0)
        return;
      CommConnectionManager.CommCenter.SendUpdateClanMembers((IEnumerable<ClanMemberView>) Singleton<PlayerDataManager>.Instance.ClanMembers);
      this.SetClanData((ClanView) null);
    }), (Action<Exception>) (ex =>
    {
      this.IsProcessingWebservice = false;
      DebugConsoleManager.SendExceptionReport(ex.Message, ex.StackTrace, "There was a problem. Please try again later.");
    }));
  }

  public void CreateNewClan(string name, string motto, string tag)
  {
    this.IsProcessingWebservice = true;
    ClanWebServiceClient.CreateClan(new GroupCreationView()
    {
      Name = name,
      Motto = motto,
      ApplicationId = 1,
      Cmid = PlayerDataManager.CmidSecure,
      Tag = tag,
      Locale = ((Enum) ApplicationDataManager.CurrentLocale).ToString()
    }, (Action<ClanCreationReturnView>) (ev =>
    {
      this.IsProcessingWebservice = false;
      if (ev.ResultCode == 0)
      {
        CmuneEventHandler.Route((object) new ClanPageGUI.ClanCreationEvent());
        this.SetClanData(ev.ClanView);
      }
      else
      {
        int resultCode = ev.ResultCode;
        switch (resultCode)
        {
          case 1:
            PopupSystem.ShowMessage("Invalid Clan Name", "The name '" + name + "' is not valid, please modify it.");
            break;
          case 2:
            PopupSystem.ShowMessage("Clan Collision", "You are already member of another clan, please leave first before creating your own.");
            break;
          case 3:
            PopupSystem.ShowMessage("Clan Name", "The name '" + name + "' is already taken, try another one.");
            break;
          case 4:
            PopupSystem.ShowMessage("Invalid Clan Tag", "The tag '" + tag + "' is not valid, please modify it.");
            break;
          case 8:
            PopupSystem.ShowMessage("Invalid Clan Motto", "The motto '" + motto + "' is not valid, please modify it.");
            break;
          case 10:
            PopupSystem.ShowMessage("Clan Tag", "The tag '" + tag + "' is already taken, try another one.");
            break;
          default:
            switch (resultCode - 100)
            {
              case 0:
              case 1:
              case 2:
                PopupSystem.ShowMessage("Sorry", "You don't fulfill the minimal requirements to create your own clan.");
                return;
              default:
                PopupSystem.ShowMessage("Sorry", "There was an error (code " + (object) ev.ResultCode + "), please visit support.uberstrike.com for help.");
                return;
            }
        }
      }
    }), (Action<Exception>) (ex =>
    {
      this.IsProcessingWebservice = false;
      this.SetClanData((ClanView) null);
      DebugConsoleManager.SendExceptionReport(ex.Message, ex.StackTrace, "There was a problem. Please try again later.");
    }));
  }

  public void UpdateMemberTo(int cmid, GroupPosition position)
  {
    this.IsProcessingWebservice = true;
    ClanWebServiceClient.UpdateMemberPosition(new MemberPositionUpdateView(PlayerDataManager.ClanIDSecure, PlayerDataManager.CmidSecure, cmid, position), (Action<int>) (ev =>
    {
      this.IsProcessingWebservice = false;
      ClanMemberView view;
      if (ev != 0 || !PlayerDataManager.TryGetClanMember(cmid, out view))
        return;
      view.Position = position;
    }), (Action<Exception>) (ex =>
    {
      this.IsProcessingWebservice = false;
      DebugConsoleManager.SendExceptionReport(ex.Message, ex.StackTrace, "There was a problem. Please try again later.");
    }));
  }

  public void TransferOwnershipTo(int cmid)
  {
    this.IsProcessingWebservice = true;
    ClanWebServiceClient.TransferOwnership(PlayerDataManager.ClanIDSecure, PlayerDataManager.CmidSecure, cmid, (Action<int>) (ev =>
    {
      this.IsProcessingWebservice = false;
      switch (ev)
      {
        case 0:
          ClanMemberView view;
          if (PlayerDataManager.TryGetClanMember(cmid, out view))
            view.Position = GroupPosition.Leader;
          if (PlayerDataManager.TryGetClanMember(PlayerDataManager.CmidSecure, out view))
            view.Position = GroupPosition.Member;
          Singleton<PlayerDataManager>.Instance.RankInClan = GroupPosition.Member;
          break;
        case 100:
          PopupSystem.ShowMessage("Sorry", "The player you selected can't be a clan leader, because he is not level 4 yet!");
          break;
        case 101:
          PopupSystem.ShowMessage("Sorry", "The player you selected can't be a clan leader, because has no friends!");
          break;
        case 102:
          PopupSystem.ShowMessage("Sorry", "The player you selected can't be a clan leader, because he doesn't own a clan license.");
          break;
        default:
          PopupSystem.ShowMessage("Sorry", "There was an error (code " + (object) ev + "), please visit support.uberstrike.com for help.");
          break;
      }
    }), (Action<Exception>) (ex =>
    {
      this.IsProcessingWebservice = false;
      DebugConsoleManager.SendExceptionReport(ex.Message, ex.StackTrace, "There was a problem. Please try again later.");
    }));
  }

  public void RemoveMemberFromClan(int cmid)
  {
    this.IsProcessingWebservice = true;
    ClanWebServiceClient.KickMemberFromClan(PlayerDataManager.ClanIDSecure, PlayerDataManager.CmidSecure, cmid, (Action<int>) (ev =>
    {
      this.IsProcessingWebservice = false;
      if (ev != 0)
        return;
      Singleton<PlayerDataManager>.Instance.ClanMembers.RemoveAll((Predicate<ClanMemberView>) (m => m.Cmid == cmid));
      CommConnectionManager.CommCenter.SendUpdateClanMembers((IEnumerable<ClanMemberView>) Singleton<PlayerDataManager>.Instance.ClanMembers);
      CommConnectionManager.CommCenter.SendRefreshClanData(cmid);
      Singleton<ChatManager>.Instance.UpdateClanSection();
    }), (Action<Exception>) (ex =>
    {
      this.IsProcessingWebservice = false;
      DebugConsoleManager.SendExceptionReport(ex.Message, ex.StackTrace, "There was a problem. Please try again later.");
    }));
  }
}
