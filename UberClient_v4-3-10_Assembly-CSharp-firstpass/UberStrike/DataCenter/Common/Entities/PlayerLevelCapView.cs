// Decompiled with JetBrains decompiler
// Type: UberStrike.DataCenter.Common.Entities.PlayerLevelCapView
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

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
