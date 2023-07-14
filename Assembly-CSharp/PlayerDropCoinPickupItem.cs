// Decompiled with JetBrains decompiler
// Type: PlayerDropCoinPickupItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class PlayerDropCoinPickupItem : PickupItem
{
  public float Timeout = 10f;
  private float _timeout;

  [DebuggerHidden]
  private IEnumerator Start() => (IEnumerator) new PlayerDropCoinPickupItem.\u003CStart\u003Ec__Iterator39()
  {
    \u003C\u003Ef__this = this
  };

  private void Update()
  {
    if (!(bool) (Object) this._pickupItem)
      return;
    this._pickupItem.Rotate(Vector3.up, 150f * Time.deltaTime, Space.Self);
  }

  protected override bool OnPlayerPickup()
  {
    if (GameState.HasCurrentGame)
    {
      GameState.CurrentGame.PickupPowerup(this.PickupID, PickupItemType.Coin, 1);
      Singleton<PickupNameHud>.Instance.DisplayPickupName("Point", PickUpMessageType.Coin);
      HudController.Instance.XpPtsHud.GainPoints(1);
      this.PlayLocalPickupSound(GameAudio.GetPoints);
      this.StartCoroutine(this.StartHidingPickupForSeconds(0));
    }
    return true;
  }

  protected override void OnRemotePickup() => this.PlayRemotePickupSound(GameAudio.GetPoints, this.transform.position);
}
