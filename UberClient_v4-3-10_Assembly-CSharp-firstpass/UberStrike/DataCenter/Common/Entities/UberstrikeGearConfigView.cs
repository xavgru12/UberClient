// Decompiled with JetBrains decompiler
// Type: UberStrike.DataCenter.Common.Entities.UberstrikeGearConfigView
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Text;

namespace UberStrike.DataCenter.Common.Entities
{
  public class UberstrikeGearConfigView
  {
    public int ArmorPoints { get; set; }

    public int ArmorWeight { get; set; }

    public int LevelRequired { get; set; }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("[UberstrikeGearConfigView: [ArmorPoints: ");
      stringBuilder.Append(this.ArmorPoints);
      stringBuilder.Append("][ArmorWeight: ");
      stringBuilder.Append(this.ArmorWeight);
      stringBuilder.Append("]]");
      return stringBuilder.ToString();
    }
  }
}
