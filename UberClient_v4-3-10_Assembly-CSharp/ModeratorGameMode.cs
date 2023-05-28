// Decompiled with JetBrains decompiler
// Type: ModeratorGameMode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UberStrike.Realtime.UnitySdk;

[NetworkClass(107)]
public class ModeratorGameMode : ClientNetworkClass
{
  private static ModeratorGameMode _moderator;

  private ModeratorGameMode(GameMetaData data)
    : base(GameConnectionManager.Rmi)
  {
  }

  public static void ModerateGameMode(FpsGameMode mode)
  {
    if (ModeratorGameMode._moderator != null)
      ModeratorGameMode._moderator.Dispose();
    ModeratorGameMode._moderator = new ModeratorGameMode(mode.GameData);
    mode.InitializeMode(isSpectator: true);
    Singleton<PlayerSpectatorControl>.Instance.IsEnabled = true;
  }

  protected override void Dispose(bool dispose)
  {
    Singleton<PlayerSpectatorControl>.Instance.IsEnabled = false;
    base.Dispose(dispose);
    ModeratorGameMode._moderator = (ModeratorGameMode) null;
  }
}
