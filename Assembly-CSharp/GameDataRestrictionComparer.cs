// Decompiled with JetBrains decompiler
// Type: GameDataRestrictionComparer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UberStrike.Realtime.UnitySdk;

public class GameDataRestrictionComparer : IComparer<GameMetaData>
{
  private int _playerLevel;
  private IComparer<GameMetaData> _baseComparer;

  public GameDataRestrictionComparer(int playerLevel, IComparer<GameMetaData> baseComparer)
  {
    this._playerLevel = playerLevel;
    this._baseComparer = baseComparer;
  }

  public int Compare(GameMetaData x, GameMetaData y)
  {
    if (!x.HasLevelRestriction && !y.HasLevelRestriction)
      return this._baseComparer.Compare(x, y);
    return this._playerLevel < 5 ? this.NoobLevelsUp(x, y) : this.VeteranLevelsUp(x, y);
  }

  private int NoobLevelsUp(GameMetaData x, GameMetaData y) => (x.LevelMin >= 5 || x.LevelMin == 0 ? x.LevelMin : x.LevelMin - 100) - (y.LevelMin >= 5 || y.LevelMin == 0 ? y.LevelMin : y.LevelMin - 100);

  private int VeteranLevelsUp(GameMetaData x, GameMetaData y) => (x.LevelMin >= 5 ? x.LevelMin : x.LevelMin + 100) - (y.LevelMin >= 5 ? y.LevelMin : y.LevelMin + 100);
}
