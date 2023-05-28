// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.PlayerWeaponStatisticsViewProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System.IO;
using UberStrike.DataCenter.Common.Entities;

namespace UberStrike.Core.Serialization
{
  public static class PlayerWeaponStatisticsViewProxy
  {
    public static void Serialize(Stream stream, PlayerWeaponStatisticsView instance)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, instance.CannonTotalDamageDone);
        Int32Proxy.Serialize((Stream) bytes, instance.CannonTotalShotsFired);
        Int32Proxy.Serialize((Stream) bytes, instance.CannonTotalShotsHit);
        Int32Proxy.Serialize((Stream) bytes, instance.CannonTotalSplats);
        Int32Proxy.Serialize((Stream) bytes, instance.LauncherTotalDamageDone);
        Int32Proxy.Serialize((Stream) bytes, instance.LauncherTotalShotsFired);
        Int32Proxy.Serialize((Stream) bytes, instance.LauncherTotalShotsHit);
        Int32Proxy.Serialize((Stream) bytes, instance.LauncherTotalSplats);
        Int32Proxy.Serialize((Stream) bytes, instance.MachineGunTotalDamageDone);
        Int32Proxy.Serialize((Stream) bytes, instance.MachineGunTotalShotsFired);
        Int32Proxy.Serialize((Stream) bytes, instance.MachineGunTotalShotsHit);
        Int32Proxy.Serialize((Stream) bytes, instance.MachineGunTotalSplats);
        Int32Proxy.Serialize((Stream) bytes, instance.MeleeTotalDamageDone);
        Int32Proxy.Serialize((Stream) bytes, instance.MeleeTotalShotsFired);
        Int32Proxy.Serialize((Stream) bytes, instance.MeleeTotalShotsHit);
        Int32Proxy.Serialize((Stream) bytes, instance.MeleeTotalSplats);
        Int32Proxy.Serialize((Stream) bytes, instance.ShotgunTotalDamageDone);
        Int32Proxy.Serialize((Stream) bytes, instance.ShotgunTotalShotsFired);
        Int32Proxy.Serialize((Stream) bytes, instance.ShotgunTotalShotsHit);
        Int32Proxy.Serialize((Stream) bytes, instance.ShotgunTotalSplats);
        Int32Proxy.Serialize((Stream) bytes, instance.SniperTotalDamageDone);
        Int32Proxy.Serialize((Stream) bytes, instance.SniperTotalShotsFired);
        Int32Proxy.Serialize((Stream) bytes, instance.SniperTotalShotsHit);
        Int32Proxy.Serialize((Stream) bytes, instance.SniperTotalSplats);
        Int32Proxy.Serialize((Stream) bytes, instance.SplattergunTotalDamageDone);
        Int32Proxy.Serialize((Stream) bytes, instance.SplattergunTotalShotsFired);
        Int32Proxy.Serialize((Stream) bytes, instance.SplattergunTotalShotsHit);
        Int32Proxy.Serialize((Stream) bytes, instance.SplattergunTotalSplats);
        bytes.WriteTo(stream);
      }
    }

    public static PlayerWeaponStatisticsView Deserialize(Stream bytes) => new PlayerWeaponStatisticsView()
    {
      CannonTotalDamageDone = Int32Proxy.Deserialize(bytes),
      CannonTotalShotsFired = Int32Proxy.Deserialize(bytes),
      CannonTotalShotsHit = Int32Proxy.Deserialize(bytes),
      CannonTotalSplats = Int32Proxy.Deserialize(bytes),
      LauncherTotalDamageDone = Int32Proxy.Deserialize(bytes),
      LauncherTotalShotsFired = Int32Proxy.Deserialize(bytes),
      LauncherTotalShotsHit = Int32Proxy.Deserialize(bytes),
      LauncherTotalSplats = Int32Proxy.Deserialize(bytes),
      MachineGunTotalDamageDone = Int32Proxy.Deserialize(bytes),
      MachineGunTotalShotsFired = Int32Proxy.Deserialize(bytes),
      MachineGunTotalShotsHit = Int32Proxy.Deserialize(bytes),
      MachineGunTotalSplats = Int32Proxy.Deserialize(bytes),
      MeleeTotalDamageDone = Int32Proxy.Deserialize(bytes),
      MeleeTotalShotsFired = Int32Proxy.Deserialize(bytes),
      MeleeTotalShotsHit = Int32Proxy.Deserialize(bytes),
      MeleeTotalSplats = Int32Proxy.Deserialize(bytes),
      ShotgunTotalDamageDone = Int32Proxy.Deserialize(bytes),
      ShotgunTotalShotsFired = Int32Proxy.Deserialize(bytes),
      ShotgunTotalShotsHit = Int32Proxy.Deserialize(bytes),
      ShotgunTotalSplats = Int32Proxy.Deserialize(bytes),
      SniperTotalDamageDone = Int32Proxy.Deserialize(bytes),
      SniperTotalShotsFired = Int32Proxy.Deserialize(bytes),
      SniperTotalShotsHit = Int32Proxy.Deserialize(bytes),
      SniperTotalSplats = Int32Proxy.Deserialize(bytes),
      SplattergunTotalDamageDone = Int32Proxy.Deserialize(bytes),
      SplattergunTotalShotsFired = Int32Proxy.Deserialize(bytes),
      SplattergunTotalShotsHit = Int32Proxy.Deserialize(bytes),
      SplattergunTotalSplats = Int32Proxy.Deserialize(bytes)
    };
  }
}
