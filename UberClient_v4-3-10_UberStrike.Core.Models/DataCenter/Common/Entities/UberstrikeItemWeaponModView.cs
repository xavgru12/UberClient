// Decompiled with JetBrains decompiler
// Type: UberStrike.DataCenter.Common.Entities.UberstrikeItemWeaponModView
// Assembly: UberStrike.Core.Models, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: E29887F9-C6F9-4A17-AD3C-0A827CA1DCD6
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Core.Models.dll

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
