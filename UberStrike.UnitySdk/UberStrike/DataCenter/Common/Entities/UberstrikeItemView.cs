// Decompiled with JetBrains decompiler
// Type: UberStrike.DataCenter.Common.Entities.UberstrikeItemView
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

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
