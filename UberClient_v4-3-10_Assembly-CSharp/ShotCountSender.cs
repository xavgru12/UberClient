// Decompiled with JetBrains decompiler
// Type: ShotCountSender
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UberStrike.Core.Types;
using UnityEngine;

public class ShotCountSender
{
  private const float SendDuration = 10f;
  private UberstrikeItemClass[] _weaponClasses = new UberstrikeItemClass[8]
  {
    UberstrikeItemClass.WeaponCannon,
    UberstrikeItemClass.WeaponHandgun,
    UberstrikeItemClass.WeaponLauncher,
    UberstrikeItemClass.WeaponMachinegun,
    UberstrikeItemClass.WeaponMelee,
    UberstrikeItemClass.WeaponShotgun,
    UberstrikeItemClass.WeaponSniperRifle,
    UberstrikeItemClass.WeaponSplattergun
  };
  private FpsGameMode _game;
  private float _nextSendTime;

  public ShotCountSender(FpsGameMode game)
  {
    this._game = game;
    this._nextSendTime = Time.time + 10f;
  }

  public void Send()
  {
    List<int> shotCounts = new List<int>(this._weaponClasses.Length);
    foreach (UberstrikeItemClass weaponClass in this._weaponClasses)
    {
      int shotCount = Singleton<WeaponController>.Instance.ShotCounter.GetShotCount(weaponClass);
      shotCounts.Add(shotCount);
    }
    this._game.SendShotCounts(shotCounts);
  }

  public void UpdateEveryTenSeconds()
  {
    if ((double) Time.time < (double) this._nextSendTime)
      return;
    this._nextSendTime += 10f;
    this.Send();
  }

  public void UpdateEverySecond()
  {
    if ((double) this._nextSendTime - (double) Time.time > 1.0)
      this._nextSendTime = Time.time + 1f;
    if ((double) Time.time < (double) this._nextSendTime)
      return;
    this._nextSendTime = Time.time + 1f;
    this.Send();
  }
}
