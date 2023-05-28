// Decompiled with JetBrains decompiler
// Type: UberStrike.DataCenter.Common.Entities.PlayerXPEventView
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

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
