// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.PlayerPersonalRecordStatisticsViewProxy
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using System.IO;
using UberStrike.DataCenter.Common.Entities;

namespace UberStrike.Core.Serialization
{
  public static class PlayerPersonalRecordStatisticsViewProxy
  {
    public static void Serialize(Stream stream, PlayerPersonalRecordStatisticsView instance)
    {
      int num = 0;
      if (instance != null)
      {
        using (MemoryStream bytes = new MemoryStream())
        {
          Int32Proxy.Serialize((Stream) bytes, instance.MostArmorPickedUp);
          Int32Proxy.Serialize((Stream) bytes, instance.MostCannonSplats);
          Int32Proxy.Serialize((Stream) bytes, instance.MostConsecutiveSnipes);
          Int32Proxy.Serialize((Stream) bytes, instance.MostDamageDealt);
          Int32Proxy.Serialize((Stream) bytes, instance.MostDamageReceived);
          Int32Proxy.Serialize((Stream) bytes, instance.MostHandgunSplats);
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
          Int32Proxy.Serialize(stream, ~num);
          bytes.WriteTo(stream);
        }
      }
      else
        Int32Proxy.Serialize(stream, 0);
    }

    public static PlayerPersonalRecordStatisticsView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      PlayerPersonalRecordStatisticsView recordStatisticsView = (PlayerPersonalRecordStatisticsView) null;
      if (num != 0)
      {
        recordStatisticsView = new PlayerPersonalRecordStatisticsView();
        recordStatisticsView.MostArmorPickedUp = Int32Proxy.Deserialize(bytes);
        recordStatisticsView.MostCannonSplats = Int32Proxy.Deserialize(bytes);
        recordStatisticsView.MostConsecutiveSnipes = Int32Proxy.Deserialize(bytes);
        recordStatisticsView.MostDamageDealt = Int32Proxy.Deserialize(bytes);
        recordStatisticsView.MostDamageReceived = Int32Proxy.Deserialize(bytes);
        recordStatisticsView.MostHandgunSplats = Int32Proxy.Deserialize(bytes);
        recordStatisticsView.MostHeadshots = Int32Proxy.Deserialize(bytes);
        recordStatisticsView.MostHealthPickedUp = Int32Proxy.Deserialize(bytes);
        recordStatisticsView.MostLauncherSplats = Int32Proxy.Deserialize(bytes);
        recordStatisticsView.MostMachinegunSplats = Int32Proxy.Deserialize(bytes);
        recordStatisticsView.MostMeleeSplats = Int32Proxy.Deserialize(bytes);
        recordStatisticsView.MostNutshots = Int32Proxy.Deserialize(bytes);
        recordStatisticsView.MostShotgunSplats = Int32Proxy.Deserialize(bytes);
        recordStatisticsView.MostSniperSplats = Int32Proxy.Deserialize(bytes);
        recordStatisticsView.MostSplats = Int32Proxy.Deserialize(bytes);
        recordStatisticsView.MostSplattergunSplats = Int32Proxy.Deserialize(bytes);
        recordStatisticsView.MostXPEarned = Int32Proxy.Deserialize(bytes);
      }
      return recordStatisticsView;
    }
  }
}
