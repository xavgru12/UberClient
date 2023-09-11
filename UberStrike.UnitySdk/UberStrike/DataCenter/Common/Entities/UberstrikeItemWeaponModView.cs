// Decompiled with JetBrains decompiler
// Type: UberStrike.DataCenter.Common.Entities.UberstrikeItemWeaponModView
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using Cmune.DataCenter.Common.Entities;
using System.Text;

namespace UberStrike.DataCenter.Common.Entities
{
  public class UberstrikeItemWeaponModView : UberstrikeItemView
  {
    public UberstrikeWeaponModConfigView Config { get; set; }

    public UberstrikeItemWeaponModView()
    {
    }

    public UberstrikeItemWeaponModView(
      ItemView item,
      int level,
      UberstrikeWeaponModConfigView config)
      : base(item, level)
    {
      this.Config = config;
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("[UberstrikeWeaponModView: ");
      stringBuilder.Append(base.ToString());
      stringBuilder.Append((object) this.Config);
      stringBuilder.Append("]]");
      return stringBuilder.ToString();
    }
  }
}
