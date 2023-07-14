// Decompiled with JetBrains decompiler
// Type: OnPlayerChangeTeamEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UberStrike.Realtime.UnitySdk;

public class OnPlayerChangeTeamEvent
{
  public int PlayerID { get; set; }

  public CharacterInfo PlayerInfo { get; set; }

  public TeamID TargetTeamID { get; set; }
}
