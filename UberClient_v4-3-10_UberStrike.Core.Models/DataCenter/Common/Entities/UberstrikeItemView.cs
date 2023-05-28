// Decompiled with JetBrains decompiler
// Type: UberStrike.DataCenter.Common.Entities.UberstrikeItemView
// Assembly: UberStrike.Core.Models, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: E29887F9-C6F9-4A17-AD3C-0A827CA1DCD6
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Core.Models.dll

using Cmune.DataCenter.Common.Entities;
using System.Text;

namespace UberStrike.DataCenter.Common.Entities
{
  public class UberstrikeItemView : ItemView
  {
    public int LevelRequired { get; set; }

    public UberstrikeItemView()
    {
    }

    public UberstrikeItemView(ItemView item, int levelRequired)
      : base(item)
    {
      this.LevelRequired = levelRequired;
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("[UberstrikeItemView: ");
      stringBuilder.Append(base.ToString());
      stringBuilder.Append("[LevelRequired: ");
      stringBuilder.Append(this.LevelRequired);
      stringBuilder.Append("]]");
      return stringBuilder.ToString();
    }
  }
}
