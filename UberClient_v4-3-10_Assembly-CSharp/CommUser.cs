// Decompiled with JetBrains decompiler
// Type: CommUser
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using UberStrike.Realtime.UnitySdk;

public class CommUser
{
  private string _name = string.Empty;

  public CommUser(CommActorInfo user) => this.SetActor(user);

  public CommUser(CharacterInfo user)
  {
    this.IsInGame = true;
    this.Cmid = user.Cmid;
    this.Name = user.PlayerName;
    this.ActorId = user.ActorId;
  }

  public CommUser(PublicProfileView profile)
  {
    if (profile == null)
      return;
    this.IsFriend = true;
    this.Cmid = profile.Cmid;
    this.AccessLevel = profile.AccessLevel;
    this.Name = !string.IsNullOrEmpty(profile.GroupTag) ? "[" + profile.GroupTag + "] " + profile.Name : profile.Name;
  }

  public CommUser(ClanMemberView member)
  {
    if (member == null)
      return;
    this.IsClanMember = true;
    this.Cmid = member.Cmid;
    this.AccessLevel = MemberAccessLevel.Default;
    this.Name = !string.IsNullOrEmpty(PlayerDataManager.ClanTag) ? "[" + PlayerDataManager.ClanTag + "] " + member.Name : member.Name;
  }

  public override int GetHashCode() => this.Cmid;

  public void SetActor(CommActorInfo actor)
  {
    if (actor != null)
    {
      this.Cmid = actor.Cmid;
      this.AccessLevel = (MemberAccessLevel) actor.AccessLevel;
      this.ActorId = actor.ActorId;
      this.Name = actor.PlayerName;
      this.IsInGame = actor.IsInGame;
      this.Channel = actor.Channel;
      this.ModerationFlag = (int) actor.ModerationFlag;
      this.ModerationInfo = actor.ModInformation;
      this.CurrentGame = actor.CurrentRoom.Number == 88 ? CmuneRoomID.Empty : actor.CurrentRoom;
    }
    else
    {
      this.ActorId = 0;
      this.IsInGame = false;
      this.CurrentGame = CmuneRoomID.Empty;
    }
  }

  public int Cmid { get; private set; }

  public string Name
  {
    set
    {
      string str = value;
      this.ShortName = str;
      this._name = str;
      int num = this._name.IndexOf("]");
      if (num <= 0 || num + 1 >= this._name.Length)
        return;
      this.ShortName = this._name.Substring(num + 1);
    }
    get => this._name;
  }

  public int ActorId { get; private set; }

  public string ShortName { get; private set; }

  public MemberAccessLevel AccessLevel { get; private set; }

  public PresenceType PresenceIndex
  {
    get
    {
      if (!this.IsOnline)
        return PresenceType.Offline;
      return this.IsInGame ? PresenceType.InGame : PresenceType.Online;
    }
  }

  public int ModerationFlag { get; private set; }

  public string ModerationInfo { get; private set; }

  public ChannelType Channel { get; private set; }

  public CmuneRoomID CurrentGame { get; set; }

  public bool IsFriend { get; set; }

  public bool IsClanMember { get; set; }

  public bool IsInGame { get; set; }

  public bool IsOnline => this.ActorId > 0;
}
