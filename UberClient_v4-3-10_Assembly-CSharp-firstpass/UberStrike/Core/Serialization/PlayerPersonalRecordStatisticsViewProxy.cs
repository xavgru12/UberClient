// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.PlayerPersonalRecordStatisticsViewProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System.IO;
using UberStrike.DataCenter.Common.Entities;

namespace UberStrike.Core.Serialization
{
  public static class PlayerPersonalRecordStatisticsViewProxy
  {
    public static void Serialize(Stream stream, PlayerPersonalRecordStatisticsView instance)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, instance.MostArmorPickedUp);
        Int32Proxy.Serialize((Stream) bytes, instance.MostCannonSplats);
        Int32Proxy.Serialize((Stream) bytes, instance.MostConsecutiveSnipes);
        Int32Proxy.Serialize((Stream) bytes, instance.MostDamageDealt);
        Int32Proxy.Serialize((Stream) bytes, instance.MostDamageReceived);
        Int32Proxy.Serialize((Stream) bytes, instance.MostHeadshots);
        Int32Proxy.Serialize((Stream) bytes, instance.MostHealthPickedUp);
        Int32Proxy.Serialize((Stream) bytes, instance.MostLauncherSplats);
        Int32Proxy.Serialize((Stream) bytes, instance.MostMachinegunSplats);
        Int32Proxy.Serialize((Stream) bytes, instance.MostMeleeSplats);
        Int32Proxy.Serialize((Stream) bytes, instance.MostNutshots);
        Int32Proxy.Serialize((Stream) bytes, instance.MostShotgunSplats);
        Int32Proxy.Serialize((Stream) bytes, instance.MostSniperSplats);
        Int32Proxy.Serialize((Stream) bytes, instance.MostSplats);
        Int32Proxy.Serialize((Stream) bytes, instance.MostSplattergunSplats);
        Int32Proxy.Serialize((Stream) bytes, instance.MostXPEarned);
        bytes.WriteTo(stream);
      }
    }

    public static PlayerPersonalRecordStatisticsView Deserialize(Stream bytes) => new PlayerPersonalRecordStatisticsView()
    {
      MostArmorPickedUp = Int32Proxy.Deserialize(bytes),
      MostCannonSplats = Int32Proxy.Deserialize(bytes),
      MostConsecutiveSnipes = Int32Proxy.Deserialize(bytes),
      MostDamageDealt = Int32Proxy.Deserialize(bytes),
      MostDamageReceived = Int32Proxy.Deserialize(bytes),
      MostHeadshots = Int32Proxy.Deserialize(bytes),
      MostHealthPickedUp = Int32Proxy.Deserialize(bytes),
      MostLauncherSplats = Int32Proxy.Deserialize(bytes),
      MostMachinegunSplats = Int32Proxy.Deserialize(bytes),
      MostMeleeSplats = Int32Proxy.Deserialize(bytes),
      MostNutshots = Int32Proxy.Deserialize(bytes),
      MostShotgunSplats = Int32Proxy.Deserialize(bytes),
      MostSniperSplats = Int32Proxy.Deserialize(bytes),
      MostSplats = Int32Proxy.Deserialize(bytes),
      MostSplattergunSplats = Int32Proxy.Deserialize(bytes),
      MostXPEarned = Int32Proxy.Deserialize(bytes)
    };
  }
}
