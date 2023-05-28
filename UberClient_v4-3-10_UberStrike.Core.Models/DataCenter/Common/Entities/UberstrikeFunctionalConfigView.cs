// Decompiled with JetBrains decompiler
// Type: UberStrike.DataCenter.Common.Entities.UberstrikeFunctionalConfigView
// Assembly: UberStrike.Core.Models, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: E29887F9-C6F9-4A17-AD3C-0A827CA1DCD6
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Core.Models.dll

using System.Text;

namespace UberStrike.DataCenter.Common.Entities
{
  public class UberstrikeFunctionalConfigView
  {
    public int LevelRequired { get; set; }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("[UberstrikeFunctionalConfigView: ");
      stringBuilder.Append("]");
      return stringBuilder.ToString();
    }
  }
}
