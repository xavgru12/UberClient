// Decompiled with JetBrains decompiler
// Type: ArmorPickupItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class ArmorPickupItem : PickupItem
{
  [SerializeField]
  private ArmorPickupItem.Category _armorPoints;

  private AudioClip Get2DAudioClip()
  {
    switch (this._armorPoints)
    {
      case ArmorPickupItem.Category.Gold:
        return GameAudio.GoldArmor2D;
      case ArmorPickupItem.Category.Silver:
        return GameAudio.SilverArmor2D;
      case ArmorPickupItem.Category.Bronze:
        return GameAudio.ArmorShard2D;
      default:
        return GameAudio.ArmorShard2D;
    }
  }

  private AudioClip Get3DAudioClip()
  {
    switch (this._armorPoints)
    {
      case ArmorPickupItem.Category.Gold:
        return GameAudio.GoldArmor;
      case ArmorPickupItem.Category.Silver:
        return GameAudio.SilverArmor;
      case ArmorPickupItem.Category.Bronze:
        return GameAudio.ArmorShard;
      default:
        return GameAudio.ArmorShard;
    }
  }

  protected override bool OnPlayerPickup()
  {
    if (!this.CanPlayerPickup)
      return false;
    int num = 0;
    switch (this._armorPoints)
    {
      case ArmorPickupItem.Category.Gold:
        num = 100;
        Singleton<PickupNameHud>.Instance.DisplayPickupName("Uber Armor", PickUpMessageType.Armor100);
        break;
      case ArmorPickupItem.Category.Silver:
        num = 50;
        Singleton<PickupNameHud>.Instance.DisplayPickupName("Big Armor", PickUpMessageType.Armor50);
        break;
      case ArmorPickupItem.Category.Bronze:
        num = 5;
        Singleton<PickupNameHud>.Instance.DisplayPickupName("Mini Armor", PickUpMessageType.Armor5);
        break;
    }
    Singleton<HpApHud>.Instance.AP = Mathf.Clamp(GameState.LocalCharacter.Armor.ArmorPoints + num, 0, 200);
    this.PlayLocalPickupSound(this.Get2DAudioClip());
    if (GameState.HasCurrentGame)
    {
      GameState.CurrentGame.PickupPowerup(this.PickupID, PickupItemType.Armor, num);
      if (GameState.IsSinglePlayer)
        this.StartCoroutine(this.StartHidingPickupForSeconds(this._respawnTime));
    }
    return true;
  }

  protected override void OnRemotePickup() => this.PlayRemotePickupSound(this.Get3DAudioClip(), this.transform.position);

  protected override bool CanPlayerPickup => GameState.HasCurrentPlayer && GameState.LocalCharacter.Armor.ArmorPoints < 200;

  public enum Category
  {
    Gold,
    Silver,
    Bronze,
  }
}
