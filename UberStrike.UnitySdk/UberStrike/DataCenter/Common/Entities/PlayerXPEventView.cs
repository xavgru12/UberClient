// Decompiled with JetBrains decompiler
// Type: UberStrike.DataCenter.Common.Entities.PlayerXPEventView
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

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
