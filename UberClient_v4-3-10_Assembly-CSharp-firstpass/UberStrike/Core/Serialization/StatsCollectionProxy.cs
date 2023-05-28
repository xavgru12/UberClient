// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.StatsCollectionProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System.IO;
using UberStrike.Core.Models;

namespace UberStrike.Core.Serialization
{
  public static class StatsCollectionProxy
  {
    public static void Serialize(Stream stream, StatsCollection instance)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, instance.ArmorPickedUp);
        Int32Proxy.Serialize((Stream) bytes, instance.CannonDamageDone);
        Int32Proxy.Serialize((Stream) bytes, instance.CannonKills);
        Int32Proxy.Serialize((Stream) bytes, instance.CannonShotsFired);
        Int32Proxy.Serialize((Stream) bytes, instance.CannonShotsHit);
        Int32Proxy.Serialize((Stream) bytes, instance.ConsecutiveSnipes);
        Int32Proxy.Serialize((Stream) bytes, instance.DamageReceived);
        Int32Proxy.Serialize((Stream) bytes, instance.Deaths);
        Int32Proxy.Serialize((Stream) bytes, instance.Headshots);
        Int32Proxy.Serialize((Stream) bytes, instance.HealthPickedUp);
        Int32Proxy.Serialize((Stream) bytes, instance.LauncherDamageDone);
        Int32Proxy.Serialize((Stream) bytes, instance.LauncherKills);
        Int32Proxy.Serialize((Stream) bytes, instance.LauncherShotsFired);
        Int32Proxy.Serialize((Stream) bytes, instance.LauncherShotsHit);
        Int32Proxy.Serialize((Stream) bytes, instance.MachineGunDamageDone);
        Int32Proxy.Serialize((Stream) bytes, instance.MachineGunKills);
        Int32Proxy.Serialize((Stream) bytes, instance.MachineGunShotsFired);
        Int32Proxy.Serialize((Stream) bytes, instance.MachineGunShotsHit);
        Int32Proxy.Serialize((Stream) bytes, instance.MeleeDamageDone);
        Int32Proxy.Serialize((Stream) bytes, instance.MeleeKills);
        Int32Proxy.Serialize((Stream) bytes, instance.MeleeShotsFired);
        Int32Proxy.Serialize((Stream) bytes, instance.MeleeShotsHit);
        Int32Proxy.Serialize((Stream) bytes, instance.Nutshots);
        Int32Proxy.Serialize((Stream) bytes, instance.Points);
        Int32Proxy.Serialize((Stream) bytes, instance.ShotgunDamageDone);
        Int32Proxy.Serialize((Stream) bytes, instance.ShotgunShotsFired);
        Int32Proxy.Serialize((Stream) bytes, instance.ShotgunShotsHit);
        Int32Proxy.Serialize((Stream) bytes, instance.ShotgunSplats);
        Int32Proxy.Serialize((Stream) bytes, instance.SniperDamageDone);
        Int32Proxy.Serialize((Stream) bytes, instance.SniperKills);
        Int32Proxy.Serialize((Stream) bytes, instance.SniperShotsFired);
        Int32Proxy.Serialize((Stream) bytes, instance.SniperShotsHit);
        Int32Proxy.Serialize((Stream) bytes, instance.SplattergunDamageDone);
        Int32Proxy.Serialize((Stream) bytes, instance.SplattergunKills);
        Int32Proxy.Serialize((Stream) bytes, instance.SplattergunShotsFired);
        Int32Proxy.Serialize((Stream) bytes, instance.SplattergunShotsHit);
        Int32Proxy.Serialize((Stream) bytes, instance.Suicides);
        Int32Proxy.Serialize((Stream) bytes, instance.Xp);
        bytes.WriteTo(stream);
      }
    }

    public static StatsCollection Deserialize(Stream bytes) => new StatsCollection()
    {
      ArmorPickedUp = Int32Proxy.Deserialize(bytes),
      CannonDamageDone = Int32Proxy.Deserialize(bytes),
      CannonKills = Int32Proxy.Deserialize(bytes),
      CannonShotsFired = Int32Proxy.Deserialize(bytes),
      CannonShotsHit = Int32Proxy.Deserialize(bytes),
      ConsecutiveSnipes = Int32Proxy.Deserialize(bytes),
      DamageReceived = Int32Proxy.Deserialize(bytes),
      Deaths = Int32Proxy.Deserialize(bytes),
      Headshots = Int32Proxy.Deserialize(bytes),
      HealthPickedUp = Int32Proxy.Deserialize(bytes),
      LauncherDamageDone = Int32Proxy.Deserialize(bytes),
      LauncherKills = Int32Proxy.Deserialize(bytes),
      LauncherShotsFired = Int32Proxy.Deserialize(bytes),
      LauncherShotsHit = Int32Proxy.Deserialize(bytes),
      MachineGunDamageDone = Int32Proxy.Deserialize(bytes),
      MachineGunKills = Int32Proxy.Deserialize(bytes),
      MachineGunShotsFired = Int32Proxy.Deserialize(bytes),
      MachineGunShotsHit = Int32Proxy.Deserialize(bytes),
      MeleeDamageDone = Int32Proxy.Deserialize(bytes),
      MeleeKills = Int32Proxy.Deserialize(bytes),
      MeleeShotsFired = Int32Proxy.Deserialize(bytes),
      MeleeShotsHit = Int32Proxy.Deserialize(bytes),
      Nutshots = Int32Proxy.Deserialize(bytes),
      Points = Int32Proxy.Deserialize(bytes),
      ShotgunDamageDone = Int32Proxy.Deserialize(bytes),
      ShotgunShotsFired = Int32Proxy.Deserialize(bytes),
      ShotgunShotsHit = Int32Proxy.Deserialize(bytes),
      ShotgunSplats = Int32Proxy.Deserialize(bytes),
      SniperDamageDone = Int32Proxy.Deserialize(bytes),
      SniperKills = Int32Proxy.Deserialize(bytes),
      SniperShotsFired = Int32Proxy.Deserialize(bytes),
      SniperShotsHit = Int32Proxy.Deserialize(bytes),
      SplattergunDamageDone = Int32Proxy.Deserialize(bytes),
      SplattergunKills = Int32Proxy.Deserialize(bytes),
      SplattergunShotsFired = Int32Proxy.Deserialize(bytes),
      SplattergunShotsHit = Int32Proxy.Deserialize(bytes),
      Suicides = Int32Proxy.Deserialize(bytes),
      Xp = Int32Proxy.Deserialize(bytes)
    };
  }
}
