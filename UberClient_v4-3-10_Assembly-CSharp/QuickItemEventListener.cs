// Decompiled with JetBrains decompiler
// Type: QuickItemEventListener
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

internal class QuickItemEventListener : Singleton<QuickItemEventListener>
{
  private QuickItemEventListener()
  {
    CmuneEventHandler.AddListener<QuickItemAmountChangedEvent>((Action<QuickItemAmountChangedEvent>) (ev => Singleton<QuickItemController>.Instance.UpdateQuickSlotAmount()));
    CmuneEventHandler.AddListener<HealthIncreaseEvent>(new Action<HealthIncreaseEvent>(this.OnHealthIncrease));
    CmuneEventHandler.AddListener<ArmorIncreaseEvent>(new Action<ArmorIncreaseEvent>(this.OnArmorIncrease));
    CmuneEventHandler.AddListener<AddAmmoIncreaseEvent>(new Action<AddAmmoIncreaseEvent>(this.OnAmmoIncrease));
    CmuneEventHandler.AddListener<AmmoAddStartEvent>(new Action<AmmoAddStartEvent>(this.OnAddStartAmmo));
    CmuneEventHandler.AddListener<AmmoAddMaxEvent>(new Action<AmmoAddMaxEvent>(this.OnAddMaxAmmo));
  }

  public void Initialize()
  {
  }

  private void OnHealthIncrease(HealthIncreaseEvent ev)
  {
    if (GameState.LocalCharacter == null || GameState.CurrentGame == null)
      return;
    GameState.CurrentGame.IncreaseHealthAndArmor(ev.Health, 0);
    Singleton<HpApHud>.Instance.HP = Mathf.Clamp((int) GameState.LocalCharacter.Health + ev.Health, 0, 200);
  }

  private void OnArmorIncrease(ArmorIncreaseEvent ev)
  {
    GameState.CurrentGame.IncreaseHealthAndArmor(0, ev.Armor);
    Singleton<HpApHud>.Instance.AP = Mathf.Clamp(GameState.LocalCharacter.Armor.ArmorPoints + ev.Armor, 0, 200);
  }

  private void OnAmmoIncrease(AddAmmoIncreaseEvent ev)
  {
    foreach (int t in Enum.GetValues(typeof (AmmoType)))
      AmmoDepot.AddAmmoOfType((AmmoType) t, ev.Amount);
    Singleton<WeaponController>.Instance.UpdateAmmoHUD();
  }

  private void OnAddMaxAmmo(AmmoAddMaxEvent ev)
  {
    foreach (int t in Enum.GetValues(typeof (AmmoType)))
      AmmoDepot.AddMaxAmmoOfType((AmmoType) t, (float) ev.Percent / 100f);
    Singleton<WeaponController>.Instance.UpdateAmmoHUD();
  }

  private void OnAddStartAmmo(AmmoAddStartEvent ev)
  {
    foreach (int t in Enum.GetValues(typeof (AmmoType)))
      AmmoDepot.AddStartAmmoOfType((AmmoType) t, (float) ev.Percent / 100f);
    Singleton<WeaponController>.Instance.UpdateAmmoHUD();
  }
}
