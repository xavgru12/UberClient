// Decompiled with JetBrains decompiler
// Type: UberStrike.DataCenter.Common.Entities.PlayerXPEventView
// Assembly: UberStrike.Core.Models, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: E29887F9-C6F9-4A17-AD3C-0A827CA1DCD6
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Core.Models.dll

using System;
using System.Text;

namespace UberStrike.DataCenter.Common.Entities
{
  [Serializable]
  public class PlayerXPEventView
  {
    public int PlayerXPEventId { get; set; }

    public string Name { get; set; }

    public Decimal XPMultiplier { get; set; }

    public PlayerXPEventView()
    {
    }

    public PlayerXPEventView(string name, Decimal xpMultiplier)
    {
      this.Name = name;
      this.XPMultiplier = xpMultiplier;
    }

    public PlayerXPEventView(int playerXPEventId, string name, Decimal xpMultiplier)
      : this(name, xpMultiplier)
    {
      this.PlayerXPEventId = playerXPEventId;
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("[PlayerXPEventView: ");
      stringBuilder.Append("[PlayerXPEventId: ");
      stringBuilder.Append(this.PlayerXPEventId);
      stringBuilder.Append("][Name: ");
      stringBuilder.Append(this.Name);
      stringBuilder.Append("][XPMultiplier: ");
      stringBuilder.Append(this.XPMultiplier);
      stringBuilder.Append("]]");
      return stringBuilder.ToString();
    }
  }
}
