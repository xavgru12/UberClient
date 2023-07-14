// Decompiled with JetBrains decompiler
// Type: UberStrike.DataCenter.Common.Entities.PlayerLevelCapView
// Assembly: UberStrike.Core.Models, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: E29887F9-C6F9-4A17-AD3C-0A827CA1DCD6
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Core.Models.dll

using System;
using System.Text;

namespace UberStrike.DataCenter.Common.Entities
{
  [Serializable]
  public class PlayerLevelCapView
  {
    public int PlayerLevelCapId { get; set; }

    public int Level { get; set; }

    public int XPRequired { get; set; }

    public PlayerLevelCapView()
    {
    }

    public PlayerLevelCapView(int level, int xpRequired)
    {
      this.Level = level;
      this.XPRequired = xpRequired;
    }

    public PlayerLevelCapView(int playerLevelCapId, int level, int xpRequired)
      : this(level, xpRequired)
    {
      this.PlayerLevelCapId = playerLevelCapId;
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("[PlayerLevelCapView: ");
      stringBuilder.Append("[PlayerLevelCapId: ");
      stringBuilder.Append(this.PlayerLevelCapId);
      stringBuilder.Append("][Level: ");
      stringBuilder.Append(this.Level);
      stringBuilder.Append("][XPRequired: ");
      stringBuilder.Append(this.XPRequired);
      stringBuilder.Append("]]");
      return stringBuilder.ToString();
    }
  }
}
