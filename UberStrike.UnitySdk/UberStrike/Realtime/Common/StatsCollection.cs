// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.Common.StatsCollection
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using Cmune.Realtime.Common;
using Cmune.Realtime.Common.IO;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace UberStrike.Realtime.Common
{
  [Serializable]
  public class StatsCollection : IByteArray
  {
    private List<PropertyInfo> _allValues;

    public StatsCollection(byte[] bytes, ref int idx)
      : this()
    {
      idx = this.FromBytes(bytes, idx);
    }

    public StatsCollection()
    {
      this._allValues = new List<PropertyInfo>();
      foreach (PropertyInfo property in this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
      {
        if ((object) property.PropertyType == (object) typeof (int) && property.CanRead && property.CanWrite)
          this._allValues.Add(property);
      }
    }

    public void Reset()
    {
      foreach (PropertyInfo allValue in this._allValues)
        allValue.SetValue((object) this, (object) 0, (object[]) null);
    }

    public void TakeBestValues(StatsCollection that)
    {
      foreach (PropertyInfo allValue in this._allValues)
      {
        int num1 = (int) allValue.GetValue((object) this, (object[]) null);
        int num2 = (int) allValue.GetValue((object) that, (object[]) null);
        if (num1 < num2)
          allValue.SetValue((object) this, (object) num2, (object[]) null);
      }
    }

    public void AddAllValues(StatsCollection that)
    {
      foreach (PropertyInfo allValue in this._allValues)
      {
        int num1 = (int) allValue.GetValue((object) this, (object[]) null);
        int num2 = (int) allValue.GetValue((object) that, (object[]) null);
        allValue.SetValue((object) this, (object) (num1 + num2), (object[]) null);
      }
    }

    public int GetKills() => this.MeleeKills + this.HandgunKills + this.MachineGunKills + this.ShotgunSplats + this.SniperKills + this.SplattergunKills + this.CannonKills + this.LauncherKills - this.Suicides;

    public int GetShots() => this.MeleeShotsFired + this.HandgunShotsFired + this.MachineGunShotsFired + this.ShotgunShotsFired + this.SniperShotsFired + this.SplattergunShotsFired + this.CannonShotsFired + this.LauncherShotsFired;

    public int GetHits() => this.MeleeShotsHit + this.HandgunShotsHit + this.MachineGunShotsHit + this.ShotgunShotsHit + this.SniperShotsHit + this.SplattergunShotsHit + this.CannonShotsHit + this.LauncherShotsHit;

    public int GetDamageDealt() => this.MeleeDamageDone + this.HandgunDamageDone + this.MachineGunDamageDone + this.ShotgunDamageDone + this.SniperDamageDone + this.SplattergunDamageDone + this.CannonDamageDone + this.LauncherDamageDone;

    public float GetKdr() => (float) Math.Max(this.GetKills(), 0) / Math.Max((float) this.Deaths, 1f);

    public float GetAccuracy() => (float) this.GetHits() / Math.Max((float) this.GetShots(), 1f);

    public int Headshots { get; set; }

    public int Nutshots { get; set; }

    public int ConsecutiveSnipes { get; set; }

    public int Xp { get; set; }

    public int Deaths { get; set; }

    public int DamageReceived { get; set; }

    public int ArmorPickedUp { get; set; }

    public int HealthPickedUp { get; set; }

    public int MeleeKills { get; set; }

    public int MeleeShotsFired { get; set; }

    public int MeleeShotsHit { get; set; }

    public int MeleeDamageDone { get; set; }

    public int HandgunKills { get; set; }

    public int HandgunShotsFired { get; set; }

    public int HandgunShotsHit { get; set; }

    public int HandgunDamageDone { get; set; }

    public int MachineGunKills { get; set; }

    public int MachineGunShotsFired { get; set; }

    public int MachineGunShotsHit { get; set; }

    public int MachineGunDamageDone { get; set; }

    public int ShotgunSplats { get; set; }

    public int ShotgunShotsFired { get; set; }

    public int ShotgunShotsHit { get; set; }

    public int ShotgunDamageDone { get; set; }

    public int SniperKills { get; set; }

    public int SniperShotsFired { get; set; }

    public int SniperShotsHit { get; set; }

    public int SniperDamageDone { get; set; }

    public int SplattergunKills { get; set; }

    public int SplattergunShotsFired { get; set; }

    public int SplattergunShotsHit { get; set; }

    public int SplattergunDamageDone { get; set; }

    public int CannonKills { get; set; }

    public int CannonShotsFired { get; set; }

    public int CannonShotsHit { get; set; }

    public int CannonDamageDone { get; set; }

    public int LauncherKills { get; set; }

    public int LauncherShotsFired { get; set; }

    public int LauncherShotsHit { get; set; }

    public int LauncherDamageDone { get; set; }

    public int Suicides { get; set; }

    public int Points { get; set; }

    public int FromBytes(byte[] bytes, int idx)
    {
      this.Headshots = DefaultByteConverter.ToInt(bytes, ref idx);
      this.Nutshots = DefaultByteConverter.ToInt(bytes, ref idx);
      this.ConsecutiveSnipes = DefaultByteConverter.ToInt(bytes, ref idx);
      this.Xp = DefaultByteConverter.ToInt(bytes, ref idx);
      this.Deaths = DefaultByteConverter.ToInt(bytes, ref idx);
      this.DamageReceived = DefaultByteConverter.ToInt(bytes, ref idx);
      this.ArmorPickedUp = DefaultByteConverter.ToInt(bytes, ref idx);
      this.HealthPickedUp = DefaultByteConverter.ToInt(bytes, ref idx);
      this.MeleeKills = DefaultByteConverter.ToInt(bytes, ref idx);
      this.MeleeShotsFired = DefaultByteConverter.ToInt(bytes, ref idx);
      this.MeleeShotsHit = DefaultByteConverter.ToInt(bytes, ref idx);
      this.MeleeDamageDone = DefaultByteConverter.ToInt(bytes, ref idx);
      this.HandgunKills = DefaultByteConverter.ToInt(bytes, ref idx);
      this.HandgunShotsFired = DefaultByteConverter.ToInt(bytes, ref idx);
      this.HandgunShotsHit = DefaultByteConverter.ToInt(bytes, ref idx);
      this.HandgunDamageDone = DefaultByteConverter.ToInt(bytes, ref idx);
      this.MachineGunKills = DefaultByteConverter.ToInt(bytes, ref idx);
      this.MachineGunShotsFired = DefaultByteConverter.ToInt(bytes, ref idx);
      this.MachineGunShotsHit = DefaultByteConverter.ToInt(bytes, ref idx);
      this.MachineGunDamageDone = DefaultByteConverter.ToInt(bytes, ref idx);
      this.ShotgunSplats = DefaultByteConverter.ToInt(bytes, ref idx);
      this.ShotgunShotsFired = DefaultByteConverter.ToInt(bytes, ref idx);
      this.ShotgunShotsHit = DefaultByteConverter.ToInt(bytes, ref idx);
      this.ShotgunDamageDone = DefaultByteConverter.ToInt(bytes, ref idx);
      this.SniperKills = DefaultByteConverter.ToInt(bytes, ref idx);
      this.SniperShotsFired = DefaultByteConverter.ToInt(bytes, ref idx);
      this.SniperShotsHit = DefaultByteConverter.ToInt(bytes, ref idx);
      this.SniperDamageDone = DefaultByteConverter.ToInt(bytes, ref idx);
      this.SplattergunKills = DefaultByteConverter.ToInt(bytes, ref idx);
      this.SplattergunShotsFired = DefaultByteConverter.ToInt(bytes, ref idx);
      this.SplattergunShotsHit = DefaultByteConverter.ToInt(bytes, ref idx);
      this.SplattergunDamageDone = DefaultByteConverter.ToInt(bytes, ref idx);
      this.CannonKills = DefaultByteConverter.ToInt(bytes, ref idx);
      this.CannonShotsFired = DefaultByteConverter.ToInt(bytes, ref idx);
      this.CannonShotsHit = DefaultByteConverter.ToInt(bytes, ref idx);
      this.CannonDamageDone = DefaultByteConverter.ToInt(bytes, ref idx);
      this.LauncherKills = DefaultByteConverter.ToInt(bytes, ref idx);
      this.LauncherShotsFired = DefaultByteConverter.ToInt(bytes, ref idx);
      this.LauncherShotsHit = DefaultByteConverter.ToInt(bytes, ref idx);
      this.LauncherDamageDone = DefaultByteConverter.ToInt(bytes, ref idx);
      this.Suicides = DefaultByteConverter.ToInt(bytes, ref idx);
      this.Points = DefaultByteConverter.ToInt(bytes, ref idx);
      return idx;
    }

    public byte[] GetBytes()
    {
      List<byte> bytes = new List<byte>(this._allValues.Count * 4);
      DefaultByteConverter.FromInt(this.Headshots, ref bytes);
      DefaultByteConverter.FromInt(this.Nutshots, ref bytes);
      DefaultByteConverter.FromInt(this.ConsecutiveSnipes, ref bytes);
      DefaultByteConverter.FromInt(this.Xp, ref bytes);
      DefaultByteConverter.FromInt(this.Deaths, ref bytes);
      DefaultByteConverter.FromInt(this.DamageReceived, ref bytes);
      DefaultByteConverter.FromInt(this.ArmorPickedUp, ref bytes);
      DefaultByteConverter.FromInt(this.HealthPickedUp, ref bytes);
      DefaultByteConverter.FromInt(this.MeleeKills, ref bytes);
      DefaultByteConverter.FromInt(this.MeleeShotsFired, ref bytes);
      DefaultByteConverter.FromInt(this.MeleeShotsHit, ref bytes);
      DefaultByteConverter.FromInt(this.MeleeDamageDone, ref bytes);
      DefaultByteConverter.FromInt(this.HandgunKills, ref bytes);
      DefaultByteConverter.FromInt(this.HandgunShotsFired, ref bytes);
      DefaultByteConverter.FromInt(this.HandgunShotsHit, ref bytes);
      DefaultByteConverter.FromInt(this.HandgunDamageDone, ref bytes);
      DefaultByteConverter.FromInt(this.MachineGunKills, ref bytes);
      DefaultByteConverter.FromInt(this.MachineGunShotsFired, ref bytes);
      DefaultByteConverter.FromInt(this.MachineGunShotsHit, ref bytes);
      DefaultByteConverter.FromInt(this.MachineGunDamageDone, ref bytes);
      DefaultByteConverter.FromInt(this.ShotgunSplats, ref bytes);
      DefaultByteConverter.FromInt(this.ShotgunShotsFired, ref bytes);
      DefaultByteConverter.FromInt(this.ShotgunShotsHit, ref bytes);
      DefaultByteConverter.FromInt(this.ShotgunDamageDone, ref bytes);
      DefaultByteConverter.FromInt(this.SniperKills, ref bytes);
      DefaultByteConverter.FromInt(this.SniperShotsFired, ref bytes);
      DefaultByteConverter.FromInt(this.SniperShotsHit, ref bytes);
      DefaultByteConverter.FromInt(this.SniperDamageDone, ref bytes);
      DefaultByteConverter.FromInt(this.SplattergunKills, ref bytes);
      DefaultByteConverter.FromInt(this.SplattergunShotsFired, ref bytes);
      DefaultByteConverter.FromInt(this.SplattergunShotsHit, ref bytes);
      DefaultByteConverter.FromInt(this.SplattergunDamageDone, ref bytes);
      DefaultByteConverter.FromInt(this.CannonKills, ref bytes);
      DefaultByteConverter.FromInt(this.CannonShotsFired, ref bytes);
      DefaultByteConverter.FromInt(this.CannonShotsHit, ref bytes);
      DefaultByteConverter.FromInt(this.CannonDamageDone, ref bytes);
      DefaultByteConverter.FromInt(this.LauncherKills, ref bytes);
      DefaultByteConverter.FromInt(this.LauncherShotsFired, ref bytes);
      DefaultByteConverter.FromInt(this.LauncherShotsHit, ref bytes);
      DefaultByteConverter.FromInt(this.LauncherDamageDone, ref bytes);
      DefaultByteConverter.FromInt(this.Suicides, ref bytes);
      DefaultByteConverter.FromInt(this.Points, ref bytes);
      return bytes.ToArray();
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (PropertyInfo allValue in this._allValues)
        stringBuilder.AppendFormat("{0}:{1}\n", (object) allValue.Name, allValue.GetValue((object) this, (object[]) null));
      return stringBuilder.ToString();
    }
  }
}
