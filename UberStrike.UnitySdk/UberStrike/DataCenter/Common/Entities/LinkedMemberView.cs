// Decompiled with JetBrains decompiler
// Type: UberStrike.DataCenter.Common.Entities.LinkedMemberView
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

namespace UberStrike.DataCenter.Common.Entities
{
  public class LinkedMemberView
  {
    public int Cmid { get; private set; }

    public string Name { get; private set; }

    public LinkedMemberView(int cmid, string name)
    {
      this.Cmid = cmid;
      this.Name = name;
    }
  }
}
