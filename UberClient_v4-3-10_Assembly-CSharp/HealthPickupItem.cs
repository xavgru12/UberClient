// Decompiled with JetBrains decompiler
// Type: HealthPickupItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class HealthPickupItem : PickupItem
{
  [SerializeField]
  private HealthPickupItem.Category _healthPoints;

  private AudioClip Get3DAudioClip()
  {
    switch (this._healthPoints)
    {
      case HealthPickupItem.Category.HP_100:
        return GameAudio.MegaHealth2D;
      case HealthPickupItem.Category.HP_50:
        return GameAudio.BigHealth2D;
      case HealthPickupItem.Category.HP_25:
        return GameAudio.MediumHealth2D;
      case HealthPickupItem.Category.HP_5:
        return GameAudio.SmallHealth2D;
      default:
        return GameAudio.SmallHealth2D;
    }
  }

  private AudioClip Get2DAudioClip()
  {
    switch (this._healthPoints)
    {
      case HealthPickupItem.Category.HP_100:
        return GameAudio.MegaHealth;
      case HealthPickupItem.Category.HP_50:
        return GameAudio.BigHealth;
      case HealthPickupItem.Category.HP_25:
        return GameAudio.MediumHealth;
      case HealthPickupItem.Category.HP_5:
        return GameAudio.SmallHealth;
      default:
        return GameAudio.SmallHealth;
    }
  }

  protected override bool OnPlayerPickup()
  {
    int num;
    int max;
    switch (this._healthPoints)
    {
      case HealthPickupItem.Category.HP_100:
        num = 100;
        max = 200;
        break;
      case HealthPickupItem.Category.HP_50:
        num = 50;
        max = 100;
        break;
      case HealthPickupItem.Category.HP_25:
        num = 25;
        max = 100;
        break;
      case HealthPickupItem.Category.HP_5:
        num = 5;
        max = 200;
        break;
      default:
        num = 0;
        max = 100;
        break;
    }
    if (!GameState.HasCurrentPlayer || !GameState.HasCurrentGame || (int) GameState.LocalCharacter.Health >= max)
      return false;
    Singleton<HpApHud>.Instance.HP = Mathf.Clamp((int) GameState.LocalCharacter.Health + num, 0, max);
    GameState.CurrentGame.PickupPowerup(this.PickupID, PickupItemType.Health, num);
    switch (this._healthPoints)
    {
      case HealthPickupItem.Category.HP_100:
        Singleton<PickupNameHud>.Instance.DisplayPickupName("Uber Health", PickUpMessageType.Health100);
        break;
      case HealthPickupItem.Category.HP_50:
        Singleton<PickupNameHud>.Instance.DisplayPickupName("Big Health", PickUpMessageType.Health50);
        break;
      case HealthPickupItem.Category.HP_25:
        Singleton<PickupNameHud>.Instance.DisplayPickupName("Medium Health", PickUpMessageType.Health25);
        break;
      case HealthPickupItem.Category.HP_5:
        Singleton<PickupNameHud>.Instance.DisplayPickupName("Mini Health", PickUpMessageType.Health5);
        break;
    }
    this.PlayLocalPickupSound(this.Get2DAudioClip());
    if (GameState.IsSinglePlayer)
      this.StartCoroutine(this.StartHidingPickupForSeconds(this._respawnTime));
    return true;
  }

  protected override void OnRemotePickup() => this.PlayRemotePickupSound(this.Get3DAudioClip(), this.transform.position);

  protected override bool CanPlayerPickup => GameState.LocalCharacter.Health < (short) 100;

  public enum Category
  {
    HP_100,
    HP_50,
    HP_25,
    HP_5,
  }
}
