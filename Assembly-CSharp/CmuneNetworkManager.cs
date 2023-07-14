// Decompiled with JetBrains decompiler
// Type: CmuneNetworkManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UberStrike.Realtime.UnitySdk;

public static class CmuneNetworkManager
{
  public static bool UseLocalCommServer = true;
  public static GameServerView CurrentLobbyServer = GameServerView.Empty;
  public static GameServerView CurrentCommServer = GameServerView.Empty;

  static CmuneNetworkManager() => RealtimeSerialization.Converter = (IByteConverter) new UberStrikeByteConverter();
}
