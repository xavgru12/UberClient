
using System.IO;
using UberStrike.DataCenter.Common.Entities;

namespace UberStrike.Core.Serialization
{
  public static class PlayerWeaponStatisticsViewProxy
  {
    public static void Serialize(Stream stream, PlayerWeaponStatisticsView instance)
    {
      int num = 0;
      if (instance != null)
      {
        using (MemoryStream bytes = new MemoryStream())
        {
          Int32Proxy.Serialize((Stream) bytes, instance.CannonTotalDamageDone);
          Int32Proxy.Serialize((Stream) bytes, instance.CannonTotalShotsFired);
          Int32Proxy.Serialize((Stream) bytes, instance.CannonTotalShotsHit);
          Int32Proxy.Serialize((Stream) bytes, instance.CannonTotalSplats);
          Int32Proxy.Serialize((Stream) bytes, instance.HandgunTotalDamageDone);
          Int32Proxy.Serialize((Stream) bytes, instance.HandgunTotalShotsFired);
          Int32Proxy.Serialize((Stream) bytes, instance.HandgunTotalShotsHit);
          Int32Proxy.Serialize((Stream) bytes, instance.HandgunTotalSplats);
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
          Int32Proxy.Serialize(stream, ~num);
          bytes.WriteTo(stream);
        }
      }
      else
        Int32Proxy.Serialize(stream, 0);
    }

    public static PlayerWeaponStatisticsView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      PlayerWeaponStatisticsView weaponStatisticsView = (PlayerWeaponStatisticsView) null;
      if (num != 0)
      {
        weaponStatisticsView = new PlayerWeaponStatisticsView();
        weaponStatisticsView.CannonTotalDamageDone = Int32Proxy.Deserialize(bytes);
        weaponStatisticsView.CannonTotalShotsFired = Int32Proxy.Deserialize(bytes);
        weaponStatisticsView.CannonTotalShotsHit = Int32Proxy.Deserialize(bytes);
        weaponStatisticsView.CannonTotalSplats = Int32Proxy.Deserialize(bytes);
        weaponStatisticsView.HandgunTotalDamageDone = Int32Proxy.Deserialize(bytes);
        weaponStatisticsView.HandgunTotalShotsFired = Int32Proxy.Deserialize(bytes);
        weaponStatisticsView.HandgunTotalShotsHit = Int32Proxy.Deserialize(bytes);
        weaponStatisticsView.HandgunTotalSplats = Int32Proxy.Deserialize(bytes);
        weaponStatisticsView.LauncherTotalDamageDone = Int32Proxy.Deserialize(bytes);
        weaponStatisticsView.LauncherTotalShotsFired = Int32Proxy.Deserialize(bytes);
        weaponStatisticsView.LauncherTotalShotsHit = Int32Proxy.Deserialize(bytes);
        weaponStatisticsView.LauncherTotalSplats = Int32Proxy.Deserialize(bytes);
        weaponStatisticsView.MachineGunTotalDamageDone = Int32Proxy.Deserialize(bytes);
        weaponStatisticsView.MachineGunTotalShotsFired = Int32Proxy.Deserialize(bytes);
        weaponStatisticsView.MachineGunTotalShotsHit = Int32Proxy.Deserialize(bytes);
        weaponStatisticsView.MachineGunTotalSplats = Int32Proxy.Deserialize(bytes);
        weaponStatisticsView.MeleeTotalDamageDone = Int32Proxy.Deserialize(bytes);
        weaponStatisticsView.MeleeTotalShotsFired = Int32Proxy.Deserialize(bytes);
        weaponStatisticsView.MeleeTotalShotsHit = Int32Proxy.Deserialize(bytes);
        weaponStatisticsView.MeleeTotalSplats = Int32Proxy.Deserialize(bytes);
        weaponStatisticsView.ShotgunTotalDamageDone = Int32Proxy.Deserialize(bytes);
        weaponStatisticsView.ShotgunTotalShotsFired = Int32Proxy.Deserialize(bytes);
        weaponStatisticsView.ShotgunTotalShotsHit = Int32Proxy.Deserialize(bytes);
        weaponStatisticsView.ShotgunTotalSplats = Int32Proxy.Deserialize(bytes);
        weaponStatisticsView.SniperTotalDamageDone = Int32Proxy.Deserialize(bytes);
        weaponStatisticsView.SniperTotalShotsFired = Int32Proxy.Deserialize(bytes);
        weaponStatisticsView.SniperTotalShotsHit = Int32Proxy.Deserialize(bytes);
        weaponStatisticsView.SniperTotalSplats = Int32Proxy.Deserialize(bytes);
        weaponStatisticsView.SplattergunTotalDamageDone = Int32Proxy.Deserialize(bytes);
        weaponStatisticsView.SplattergunTotalShotsFired = Int32Proxy.Deserialize(bytes);
        weaponStatisticsView.SplattergunTotalShotsHit = Int32Proxy.Deserialize(bytes);
        weaponStatisticsView.SplattergunTotalSplats = Int32Proxy.Deserialize(bytes);
      }
      return weaponStatisticsView;
    }
  }
}
