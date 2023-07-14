// Decompiled with JetBrains decompiler
// Type: AmmoHud
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class AmmoHud : Singleton<AmmoHud>
{
  private float _curScaleFactor;
  private MeshGUIText _ammoDigits;
  private MeshGUIQuad _ammoIcon;
  private MeshGUIQuad _glowBlur;
  private Animatable2DGroup _ammoGroup;
  private Animatable2DGroup _entireGroup;
  private int _curAmmo;

  private AmmoHud()
  {
    if (!HudAssets.Exists)
      return;
    this._curAmmo = 0;
    this._ammoDigits = new MeshGUIText(string.Empty, HudAssets.Instance.InterparkBitmapFont, TextAnchor.LowerRight);
    this._ammoDigits.NamePrefix = "AM";
    this._ammoIcon = new MeshGUIQuad((Texture) HudTextures.AmmoBlue);
    this._glowBlur = new MeshGUIQuad((Texture) HudTextures.WhiteBlur128);
    this._glowBlur.Name = "AmmoHudGlow";
    this._glowBlur.Depth = 1f;
    this._ammoGroup = new Animatable2DGroup();
    this._entireGroup = new Animatable2DGroup();
    this._ammoGroup.Group.Add((IAnimatable2D) this._ammoDigits);
    this._ammoGroup.Group.Add((IAnimatable2D) this._ammoIcon);
    this._entireGroup.Group.Add((IAnimatable2D) this._ammoGroup);
    this._entireGroup.Group.Add((IAnimatable2D) this._glowBlur);
    this.ResetHud();
    this.Enabled = false;
    CmuneEventHandler.AddListener<OnSetPlayerTeamEvent>(new Action<OnSetPlayerTeamEvent>(this.OnTeamChange));
    CmuneEventHandler.AddListener<ScreenResolutionEvent>(new Action<ScreenResolutionEvent>(this.OnScreenResolutionChange));
    CmuneEventHandler.AddListener<OnGlobalUIRibbonChanged>(new Action<OnGlobalUIRibbonChanged>(this.OnGlobalUIRibbonChange));
  }

  public bool Enabled
  {
    get => this._entireGroup.IsVisible;
    set
    {
      if (this._entireGroup.IsVisible == value)
        return;
      if (value)
      {
        this._entireGroup.Show();
        CmuneEventHandler.AddListener<ScreenResolutionEvent>(new Action<ScreenResolutionEvent>(this.OnScreenResolutionChange));
      }
      else
      {
        this._entireGroup.Hide();
        CmuneEventHandler.RemoveListener<ScreenResolutionEvent>(new Action<ScreenResolutionEvent>(this.OnScreenResolutionChange));
        Singleton<TemporaryWeaponHud>.Instance.Enabled = false;
      }
    }
  }

  public int Ammo
  {
    get => this._curAmmo;
    set => this.SetRemainingAmmo(value);
  }

  public void Draw()
  {
    if (!Singleton<WeaponController>.Instance.HasAnyWeapon)
      return;
    this._entireGroup.Draw(0.0f, 0.0f);
  }

  private void ResetHud()
  {
    this.ResetStyle();
    this.ResetTransform();
  }

  private void ResetStyle()
  {
    Singleton<HudStyleUtility>.Instance.SetTeamStyle(this._ammoDigits);
    this._glowBlur.Color = HudStyleUtility.GLOW_BLUR_BLUE_COLOR;
  }

  private void OnTeamChange(OnSetPlayerTeamEvent ev)
  {
    if (ev.TeamId == TeamID.RED)
    {
      Singleton<HudStyleUtility>.Instance.SetRedStyle(this._ammoDigits);
      this._ammoIcon.Texture = (Texture) HudTextures.AmmoRed;
      this._glowBlur.Color = HudStyleUtility.GLOW_BLUR_RED_COLOR;
      this.ResetTransform();
    }
    else
    {
      Singleton<HudStyleUtility>.Instance.SetBlueStyle(this._ammoDigits);
      this._ammoIcon.Texture = (Texture) HudTextures.AmmoBlue;
      this._glowBlur.Color = HudStyleUtility.GLOW_BLUR_BLUE_COLOR;
      this.ResetTransform();
    }
  }

  private void OnScreenResolutionChange(ScreenResolutionEvent ev) => this.ResetTransform();

  private void OnGlobalUIRibbonChange(OnGlobalUIRibbonChanged ev) => this.ResetTransform();

  private void ResetTransform()
  {
    this._curScaleFactor = 0.65f;
    this.ResetAmmoTransform();
    this.ResetBlurTransform();
    this._entireGroup.Position = new Vector2((float) Screen.width * 0.95f, (float) Screen.height * 0.95f);
  }

  private void ResetAmmoTransform()
  {
    float num1 = 0.07f;
    float num2 = (float) Screen.height * 0.03f / (float) this._ammoIcon.Texture.height;
    this._ammoIcon.Scale = new Vector2(num2, num2);
    this._ammoIcon.Position = new Vector2(-this._ammoIcon.Size.x, (float) (-(double) this._ammoIcon.Size.y * 0.800000011920929));
    this._ammoDigits.Text = this._curAmmo.ToString();
    this._ammoDigits.Scale = new Vector2(HudStyleUtility.SMALLER_DIGITS_SCALE * this._curScaleFactor, HudStyleUtility.SMALLER_DIGITS_SCALE * this._curScaleFactor);
    this._ammoDigits.Position = new Vector2(-this._ammoIcon.Size.x - HudStyleUtility.GAP_BETWEEN_TEXT, num1 * this._ammoDigits.Size.y);
  }

  private void ResetBlurTransform()
  {
    float num1 = this._ammoGroup.Rect.width * HudStyleUtility.BLUR_WIDTH_SCALE_FACTOR;
    float num2 = this._ammoGroup.Rect.height * HudStyleUtility.BLUR_HEIGHT_SCALE_FACTOR;
    this._glowBlur.Scale = new Vector2(num1 / (float) HudTextures.WhiteBlur128.width, num2 / (float) HudTextures.WhiteBlur128.height);
    this._glowBlur.Position = new Vector2((float) ((-(double) num1 - (double) this._ammoGroup.Rect.width) / 2.0), (float) ((-(double) num2 - (double) this._ammoGroup.Rect.height) / 2.0));
  }

  private void SetRemainingAmmo(int ammo)
  {
    bool flag = ammo > this._curAmmo;
    this._curAmmo = ammo;
    this.ResetTransform();
    if (!flag)
      return;
    this._ammoDigits.Flicker(0.1f, 0.02f);
  }
}
