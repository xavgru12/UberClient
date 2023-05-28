// Decompiled with JetBrains decompiler
// Type: UberStrike.DataCenter.Common.Entities.EsnsBasicStatisticView
// Assembly: UberStrike.Core.Models, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: E29887F9-C6F9-4A17-AD3C-0A827CA1DCD6
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Core.Models.dll

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
