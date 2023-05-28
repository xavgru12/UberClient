// Decompiled with JetBrains decompiler
// Type: UberStrike.DataCenter.Common.Entities.EsnsBasicStatisticView
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

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
