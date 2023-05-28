// Decompiled with JetBrains decompiler
// Type: PlayerXpUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public static class PlayerXpUtil
{
  public static void GetXpRangeForLevel(int level, out int minXp, out int maxXp)
  {
    level = Mathf.Clamp(level, 1, PlayerXpUtil.MaxPlayerLevel);
    minXp = 0;
    maxXp = 0;
    if (level < PlayerXpUtil.MaxPlayerLevel)
    {
      ApplicationDataManager.XpByLevel.TryGetValue(level, out minXp);
      ApplicationDataManager.XpByLevel.TryGetValue(level + 1, out maxXp);
    }
    else
    {
      ApplicationDataManager.XpByLevel.TryGetValue(PlayerXpUtil.MaxPlayerLevel, out minXp);
      maxXp = minXp + 1;
    }
  }

  public static string GetLevelDescription(int level) => level >= PlayerXpUtil.MaxPlayerLevel ? "Uber Space" : "Lvl " + (object) level;

  public static int GetLevelForXp(int xp)
  {
    for (int maxPlayerLevel = PlayerXpUtil.MaxPlayerLevel; maxPlayerLevel > 0; --maxPlayerLevel)
    {
      int num;
      if (ApplicationDataManager.XpByLevel.TryGetValue(maxPlayerLevel, out num) && xp >= num)
        return maxPlayerLevel;
    }
    Debug.LogError((object) "Level calculation based on player XP failed !");
    return 1;
  }

  public static int MaxPlayerLevel { get; set; }
}
