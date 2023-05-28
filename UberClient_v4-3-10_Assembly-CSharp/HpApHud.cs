// Decompiled with JetBrains decompiler
// Type: HpApHud
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Diagnostics;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class HpApHud : Singleton<HpApHud>
{
  private float _curScaleFactor;
  private MeshGUIText _hpDigitsText;
  private MeshGUIText _apDigitsText;
  private MeshGUIText _hpText;
  private MeshGUIText _apText;
  private Animatable2DGroup _hpApGroup;
  private MeshGUIQuad _glowBlur;
  private Animatable2DGroup _entireGroup;
  private int _curHP;
  private int _curAP;
  private static int WARNING_HEALTH_LOW_VALUE = 25;
  private static float WARNING_HEALTH_LOW_INTERVAL = 0.8f;
  private bool _isLowHealth;
  private float _nextWarningTime;

  private HpApHud()
  {
    this._curAP = 0;
    this._curHP = 0;
    TextAnchor textAnchor = TextAnchor.LowerLeft;
    this._hpDigitsText = new MeshGUIText(this._curHP.ToString(), HudAssets.Instance.InterparkBitmapFont, textAnchor);
    this._hpDigitsText.NamePrefix = nameof (HP);
    this._apDigitsText = new MeshGUIText(this._curAP.ToString(), HudAssets.Instance.InterparkBitmapFont, textAnchor);
    this._apDigitsText.NamePrefix = nameof (AP);
    this._hpText = new MeshGUIText(nameof (HP), HudAssets.Instance.HelveticaBitmapFont, textAnchor);
    this._apText = new MeshGUIText(nameof (AP), HudAssets.Instance.HelveticaBitmapFont, textAnchor);
    this._hpApGroup = new Animatable2DGroup();
    this._hpApGroup.Group.Add((IAnimatable2D) this._hpDigitsText);
    this._hpApGroup.Group.Add((IAnimatable2D) this._apDigitsText);
    this._hpApGroup.Group.Add((IAnimatable2D) this._hpText);
    this._hpApGroup.Group.Add((IAnimatable2D) this._apText);
    this._glowBlur = new MeshGUIQuad((Texture) HudTextures.WhiteBlur128);
    this._glowBlur.Name = "HpApHudGlow";
    this._glowBlur.Depth = 1f;
    this._entireGroup = new Animatable2DGroup();
    this._entireGroup.Group.Add((IAnimatable2D) this._hpApGroup);
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
      if (value)
        this._entireGroup.Show();
      else
        this._entireGroup.Hide();
    }
  }

  public int HP
  {
    get => this._curHP;
    set => this.SetHealthPoint(value);
  }

  public int AP
  {
    get => this._curAP;
    set => this.SetArmorPoint(value);
  }

  public void Update()
  {
    this._entireGroup.Draw(0.0f, 0.0f);
    if (!this._isLowHealth || this._curHP == 0 || (double) this._nextWarningTime <= 0.0 || (double) Time.time <= (double) this._nextWarningTime)
      return;
    MonoRoutine.Start(this.OnHealthLow());
    this._nextWarningTime = -1f;
  }

  public void Draw() => this._entireGroup.Draw(0.0f, 0.0f);

  private void ResetHud()
  {
    this.ResetStyle();
    this.ResetTransform();
  }

  private void ResetStyle()
  {
    Singleton<HudStyleUtility>.Instance.SetDefaultStyle(this._hpDigitsText);
    Singleton<HudStyleUtility>.Instance.SetDefaultStyle(this._apDigitsText);
    Singleton<HudStyleUtility>.Instance.SetDefaultStyle(this._hpText);
    Singleton<HudStyleUtility>.Instance.SetDefaultStyle(this._apText);
  }

  private void OnTeamChange(OnSetPlayerTeamEvent ev)
  {
    if (ev.TeamId == TeamID.RED)
    {
      Singleton<HudStyleUtility>.Instance.SetRedStyle(this._hpDigitsText);
      Singleton<HudStyleUtility>.Instance.SetRedStyle(this._apDigitsText);
      Singleton<HudStyleUtility>.Instance.SetRedStyle(this._hpText);
      Singleton<HudStyleUtility>.Instance.SetRedStyle(this._apText);
      this._glowBlur.Color = HudStyleUtility.GLOW_BLUR_RED_COLOR;
    }
    else
    {
      Singleton<HudStyleUtility>.Instance.SetBlueStyle(this._hpDigitsText);
      Singleton<HudStyleUtility>.Instance.SetBlueStyle(this._apDigitsText);
      Singleton<HudStyleUtility>.Instance.SetBlueStyle(this._hpText);
      Singleton<HudStyleUtility>.Instance.SetBlueStyle(this._apText);
      this._glowBlur.Color = HudStyleUtility.GLOW_BLUR_BLUE_COLOR;
    }
    this._hpText.BitmapMeshText.AlphaMin = 0.4f;
    this._apText.BitmapMeshText.AlphaMin = 0.4f;
  }

  private void OnScreenResolutionChange(ScreenResolutionEvent ev) => this.ResetTransform();

  private void OnGlobalUIRibbonChange(OnGlobalUIRibbonChanged ev) => this.ResetTransform();

  private void ResetTransform()
  {
    this._curScaleFactor = 0.65f;
    this.ResetHpApTransform();
    this.ResetBlurTransform();
    this._entireGroup.Position = new Vector2((float) Screen.width * 0.05f, (float) Screen.height * 0.95f);
    this._entireGroup.UpdateMeshGUIPosition();
  }

  private void ResetHpApTransform()
  {
    float num1 = 0.07f;
    this._hpDigitsText.Text = this._curHP.ToString();
    this._hpDigitsText.Scale = new Vector2(1f * this._curScaleFactor, 1f * this._curScaleFactor);
    this._hpDigitsText.Position = new Vector2(0.0f, num1 * this._hpDigitsText.Size.y);
    float num2 = 3f;
    float x1 = (float) (0.0 + ((double) this._hpDigitsText.Size.x + (double) num2));
    this._hpText.Scale = new Vector2(0.35f * this._curScaleFactor, 0.35f * this._curScaleFactor);
    this._hpText.Position = new Vector2(x1, 0.0f);
    float x2 = x1 + (float) Screen.width * 0.03f;
    this._apDigitsText.Text = this._curAP.ToString();
    this._apDigitsText.Scale = new Vector2(0.7f * this._curScaleFactor, 0.7f * this._curScaleFactor);
    this._apDigitsText.Position = new Vector2(x2, num1 * this._apDigitsText.Size.y);
    float x3 = x2 + (this._apDigitsText.Size.x + num2);
    this._apText.Scale = new Vector2(0.35f * this._curScaleFactor, 0.35f * this._curScaleFactor);
    this._apText.Position = new Vector2(x3, 0.0f);
  }

  private void ResetBlurTransform()
  {
    float num1 = this._hpApGroup.Rect.width * HudStyleUtility.BLUR_WIDTH_SCALE_FACTOR;
    float num2 = this._hpApGroup.Rect.height * HudStyleUtility.BLUR_HEIGHT_SCALE_FACTOR;
    this._glowBlur.Scale = new Vector2(num1 / (float) HudTextures.WhiteBlur128.width, num2 / (float) HudTextures.WhiteBlur128.height);
    this._glowBlur.Position = new Vector2((float) (((double) this._hpApGroup.Rect.width - (double) num1) / 2.0), (float) ((-(double) this._hpApGroup.Rect.height - (double) num2) / 2.0));
  }

  private void SetHealthPoint(int hp)
  {
    bool flag1 = hp > this._curHP;
    bool flag2 = hp < this._curHP;
    this._curHP = hp >= 0 ? hp : 0;
    this.ResetTransform();
    if (flag1)
      this.OnHealthIncrease();
    if (flag2)
      MonoRoutine.Start(this.OnHealthDecrease());
    bool flag3 = this._curHP < HpApHud.WARNING_HEALTH_LOW_VALUE;
    if (flag3 == this._isLowHealth)
      return;
    this._isLowHealth = flag3;
    this._nextWarningTime = Time.time + HpApHud.WARNING_HEALTH_LOW_INTERVAL;
  }

  private void SetArmorPoint(int ap)
  {
    bool flag = ap < this._curAP;
    this._curAP = ap >= 0 ? ap : 0;
    this.ResetTransform();
    if (!flag)
      return;
    MonoRoutine.Start(this.OnArmorDecrease());
  }

  private void OnHealthIncrease()
  {
    float time = 0.1f;
    this._hpDigitsText.Flicker(time, 0.02f);
    this._apDigitsText.Flicker(time, 0.02f);
  }

  [DebuggerHidden]
  private IEnumerator OnHealthDecrease() => (IEnumerator) new HpApHud.\u003COnHealthDecrease\u003Ec__Iterator46()
  {
    \u003C\u003Ef__this = this
  };

  [DebuggerHidden]
  private IEnumerator OnHealthLow() => (IEnumerator) new HpApHud.\u003COnHealthLow\u003Ec__Iterator47()
  {
    \u003C\u003Ef__this = this
  };

  [DebuggerHidden]
  private IEnumerator OnArmorDecrease() => (IEnumerator) new HpApHud.\u003COnArmorDecrease\u003Ec__Iterator48()
  {
    \u003C\u003Ef__this = this
  };
}
