// Decompiled with JetBrains decompiler
// Type: QuickItemSfxController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UberStrike.Core.Types;
using UnityEngine;

public class QuickItemSfxController : Singleton<QuickItemSfxController>
{
  private Dictionary<QuickItemLogic, QuickItemSfx> _effects;
  private Dictionary<int, QuickItemSfx> _curShownEffects;
  private int _sfxId;

  private QuickItemSfxController()
  {
    this._effects = new Dictionary<QuickItemLogic, QuickItemSfx>();
    this._curShownEffects = new Dictionary<int, QuickItemSfx>();
  }

  private int NextSfxId => ++this._sfxId;

  public void RegisterQuickItemEffect(QuickItemLogic behaviour, QuickItemSfx effect)
  {
    if ((bool) (Object) effect)
      this._effects[behaviour] = effect;
    else
      Debug.LogError((object) ("QuickItemSfx is null: " + (object) behaviour));
  }

  public void ShowThirdPersonEffect(
    CharacterConfig player,
    QuickItemLogic effectType,
    int robotLifeTime,
    int scrapsLifeTime,
    bool isInstant = false)
  {
    robotLifeTime = robotLifeTime <= 0 ? 5000 : robotLifeTime;
    scrapsLifeTime = scrapsLifeTime <= 0 ? 3000 : scrapsLifeTime;
    QuickItemSfx original;
    if (this._effects.TryGetValue(effectType, out original))
    {
      QuickItemSfx quickItemSfx = Object.Instantiate((Object) original) as QuickItemSfx;
      quickItemSfx.ID = QuickItemSfxController.CreateGlobalSfxID(player.State.PlayerNumber, this.NextSfxId);
      if (!(bool) (Object) quickItemSfx)
        return;
      this._curShownEffects.Add(quickItemSfx.ID, quickItemSfx);
      quickItemSfx.transform.parent = player.transform;
      quickItemSfx.transform.localRotation = Quaternion.AngleAxis(-45f, Vector3.up);
      quickItemSfx.transform.localPosition = new Vector3(0.0f, 0.2f, 0.0f);
      quickItemSfx.Play(robotLifeTime, scrapsLifeTime, isInstant);
      LayerUtil.SetLayerRecursively(quickItemSfx.transform, UberstrikeLayer.IgnoreRaycast);
    }
    else
      Debug.LogError((object) ("Failed to get effect: " + (object) effectType));
  }

  public void RemoveEffect(int id)
  {
    if (!this._curShownEffects.TryGetValue(id, out QuickItemSfx _))
      return;
    this._curShownEffects.Remove(id);
  }

  public void DestroytSfxFromPlayer(byte playerNumber)
  {
    foreach (KeyValuePair<int, QuickItemSfx> curShownEffect in this._curShownEffects)
    {
      if ((curShownEffect.Key & (int) byte.MaxValue) == (int) playerNumber)
      {
        curShownEffect.Value.Destroy();
        this._curShownEffects.Remove(curShownEffect.Key);
        break;
      }
    }
  }

  private static int CreateGlobalSfxID(byte playerNumber, int sfxId) => (sfxId << 8) + (int) playerNumber;
}
