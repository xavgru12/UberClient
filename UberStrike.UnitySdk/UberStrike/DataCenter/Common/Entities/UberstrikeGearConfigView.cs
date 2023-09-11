// Decompiled with JetBrains decompiler
// Type: UberStrike.DataCenter.Common.Entities.UberstrikeGearConfigView
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using System.Text;

namespace UberStrike.DataCenter.Common.Entities
{
  public class UberstrikeGearConfigView
  {
    public int ArmorPoints { get; set; }

    public int ArmorAbsorptionPercent { get; set; }

    public int ArmorWeight { get; set; }

    public int LevelRequired { get; set; }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("[UberstrikeGearConfigView: [ArmorPoints: ");
      stringBuilder.Append(this.ArmorPoints);
      stringBuilder.Append("][ArmorAbsorptionPercent: ");
      stringBuilder.Append(this.ArmorAbsorptionPercent);
      stringBuilder.Append("][ArmorWeight: ");
      stringBuilder.Append(this.ArmorWeight);
      stringBuilder.Append("]]");
      return stringBuilder.ToString();
    }
  }
}
