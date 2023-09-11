// Decompiled with JetBrains decompiler
// Type: UberStrike.DataCenter.Common.Entities.EsnsBasicStatisticView
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

namespace UberStrike.DataCenter.Common.Entities
{
  public class EsnsBasicStatisticView
  {
    public string Name { get; protected set; }

    public int SocialRank { get; protected set; }

    public int XP { get; protected set; }

    public int Level { get; protected set; }

    public int Cmid { get; protected set; }

    public EsnsBasicStatisticView(string name, int xp, int level, int cmid)
    {
      this.Name = name;
      this.XP = xp;
      this.Level = level;
      this.Cmid = cmid;
    }

    public EsnsBasicStatisticView()
    {
      this.Name = string.Empty;
      this.XP = 0;
      this.Level = 0;
      this.Cmid = 0;
    }
  }
}
