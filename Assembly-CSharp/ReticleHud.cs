// Decompiled with JetBrains decompiler
// Type: ReticleHud
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UberStrike.Core.Types;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class ReticleHud : Singleton<ReticleHud>
{
  private ReticleHud.ReticleConfiguration _reticle;
  private ReticleHud.ReticleState _curState;
  private bool _isDisplayingEnemyReticle;
  private float _enemyReticleHideTime;

  private ReticleHud() => this.Enabled = false;

  public bool Enabled { get; set; }

  public void ConfigureReticle(WeaponItemConfiguration weapon)
  {
    Reticle reticle = Singleton<ReticleRepository>.Instance.GetReticle(weapon.ItemClass);
    this._reticle = new ReticleHud.ReticleConfiguration()
    {
      Primary = !weapon.ShowReticleForPrimaryAction ? (Reticle) null : reticle,
      Secondary = weapon.SecondaryActionReticle != ReticuleForSecondaryAction.Default ? (Reticle) null : reticle
    };
  }

  public void Draw()
  {
    if (this._reticle == null || !Singleton<WeaponController>.Instance.HasAnyWeapon)
      return;
    GUI.color = this._curState != ReticleHud.ReticleState.Friend ? (!this._isDisplayingEnemyReticle ? Color.white : Color.red) : Color.green;
    if (Singleton<WeaponController>.Instance.IsSecondaryAction)
    {
      if (this._reticle.Secondary != null)
        this._reticle.Secondary.Draw(new Rect((float) (Screen.width - 64) * 0.5f, (float) (Screen.height - 64) * 0.5f, 64f, 64f));
      else if (!WeaponFeedbackManager.Instance.IsIronSighted)
      {
        float num1 = (float) Mathf.Min(Screen.width, Screen.height);
        float num2 = (float) (((double) Screen.width - (double) num1) * 0.5);
        float num3 = (float) (((double) Screen.height - (double) num1) * 0.5);
        GUI.color = Color.white;
        GUI.DrawTexture(new Rect(num2, num3, num1, num1), (Texture) HudTextures.ReticleSRZoom);
        if (Screen.width > Screen.height)
        {
          GUI.DrawTexture(new Rect(0.0f, 0.0f, num2, (float) Screen.height), (Texture) BlueStonez.box_black.normal.background);
          GUI.DrawTexture(new Rect(num1 + num2, 0.0f, num2, (float) Screen.height), (Texture) BlueStonez.box_black.normal.background);
        }
        else
        {
          GUI.DrawTexture(new Rect(0.0f, 0.0f, (float) Screen.width, num3), (Texture) BlueStonez.box_black.normal.background);
          GUI.DrawTexture(new Rect(0.0f, num3 + num1, (float) Screen.width, num3), (Texture) BlueStonez.box_black.normal.background);
        }
      }
    }
    else if (this._reticle.Primary != null)
      this._reticle.Primary.Draw(new Rect((float) (Screen.width - 64) * 0.5f, (float) (Screen.height - 64) * 0.5f, 64f, 64f));
    GUI.color = Color.white;
  }

  public void Update()
  {
    if (this._isDisplayingEnemyReticle && (double) Time.time > (double) this._enemyReticleHideTime)
      this._isDisplayingEnemyReticle = false;
    Singleton<ReticleRepository>.Instance.UpdateAllReticles();
  }

  public void TriggerReticle(UberstrikeItemClass type)
  {
    Reticle reticle = Singleton<ReticleRepository>.Instance.GetReticle(type);
    if (reticle != null)
      reticle.Trigger();
    else
      Debug.LogError((object) ("The weapon class: " + ((Enum) type).ToString() + " is not configured!"));
  }

  public void FocusCharacter(TeamID teamId)
  {
    if (GameState.CurrentGameMode == GameMode.TeamDeathMatch)
    {
      if (GameState.HasCurrentPlayer && teamId == GameState.LocalCharacter.TeamID)
        this._curState = ReticleHud.ReticleState.Friend;
      else if (this._isDisplayingEnemyReticle)
        this._curState = ReticleHud.ReticleState.Enemy;
      else
        this._curState = ReticleHud.ReticleState.None;
    }
    else if (this._isDisplayingEnemyReticle)
      this._curState = ReticleHud.ReticleState.Enemy;
    else
      this._curState = ReticleHud.ReticleState.None;
  }

  public void UnFocusCharacter() => this._curState = ReticleHud.ReticleState.None;

  public void EnableEnemyReticle()
  {
    this._isDisplayingEnemyReticle = true;
    this._enemyReticleHideTime = Time.time + 1f;
  }

  private enum ReticleState
  {
    None,
    Enemy,
    Friend,
  }

  private class ReticleConfiguration
  {
    public Reticle Primary;
    public Reticle Secondary;
  }
}
