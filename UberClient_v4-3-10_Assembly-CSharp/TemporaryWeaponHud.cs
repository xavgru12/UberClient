// Decompiled with JetBrains decompiler
// Type: TemporaryWeaponHud
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

internal class TemporaryWeaponHud : Singleton<TemporaryWeaponHud>
{
  private Animatable2DGroup _entireGroup;
  private MeshGUICircle _circleBackground;
  private MeshGUICircle _circleForeground;
  private MeshGUIText _remainingSecondsText;
  private int _totalSeconds;
  private int _remainingSeconds;

  private TemporaryWeaponHud()
  {
    this._entireGroup = new Animatable2DGroup();
    this._circleBackground = new MeshGUICircle((Texture) HudTextures.QIBlue1);
    this._circleBackground.Name = "Temporary Weapon Background";
    this._circleBackground.Angle = 360f;
    this._circleBackground.Depth = 2f;
    this._circleForeground = new MeshGUICircle((Texture) HudTextures.QIBlue3);
    this._circleForeground.Name = "Temporary Weapon Foreground";
    this._circleForeground.Depth = 1f;
    this._remainingSecondsText = new MeshGUIText(this._remainingSeconds.ToString(), HudAssets.Instance.HelveticaBitmapFont, TextAnchor.MiddleCenter);
    this._entireGroup.Group.Add((IAnimatable2D) this._circleBackground);
    this._entireGroup.Group.Add((IAnimatable2D) this._circleForeground);
    this._entireGroup.Group.Add((IAnimatable2D) this._remainingSecondsText);
    this.ResetHud();
    this.Enabled = false;
    CmuneEventHandler.AddListener<OnSetPlayerTeamEvent>(new Action<OnSetPlayerTeamEvent>(this.OnTeamChange));
    CmuneEventHandler.AddListener<ScreenResolutionEvent>(new Action<ScreenResolutionEvent>(this.OnScreenResolutionChange));
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

  public void StartCounting(int totalSeconds)
  {
    this._totalSeconds = totalSeconds;
    this.RemainingSeconds = totalSeconds;
  }

  public int RemainingSeconds
  {
    get => this._remainingSeconds;
    set
    {
      if (value == this._remainingSeconds)
        return;
      this._remainingSeconds = value <= 0 ? 0 : value;
      this._remainingSecondsText.Text = this._remainingSeconds.ToString();
      this.OnUpdateRemainingSeconds();
    }
  }

  public void Draw() => this._entireGroup.Draw(0.0f, 0.0f);

  private void ResetHud()
  {
    this.ResetStyle();
    this.ResetTransform();
  }

  private void OnScreenResolutionChange(ScreenResolutionEvent ev) => this.ResetTransform();

  private void ResetTransform()
  {
    this._remainingSecondsText.Scale = new Vector2(HudStyleUtility.SMALLER_DIGITS_SCALE * 0.65f, HudStyleUtility.SMALLER_DIGITS_SCALE * 0.65f);
    float num = this._remainingSecondsText.Size.y / this._circleForeground.GetOriginalBounds().x;
    MeshGUICircle circleBackground = this._circleBackground;
    Vector2 vector2_1 = new Vector2(num, num);
    this._circleForeground.Scale = vector2_1;
    Vector2 vector2_2 = vector2_1;
    circleBackground.Scale = vector2_2;
    this._remainingSecondsText.Scale = new Vector2(this._circleBackground.Size.x / 2f / this._remainingSecondsText.TextBounds.y, this._circleBackground.Size.x / 2f / this._remainingSecondsText.TextBounds.y);
    this._entireGroup.Position = new Vector2((float) ((double) Screen.width * 0.88999998569488525 - (double) this._circleBackground.Size.x / 2.0), (float) ((double) Screen.height * 0.949999988079071 - (double) this._circleBackground.Size.y / 2.0));
  }

  private void ResetStyle()
  {
    Singleton<HudStyleUtility>.Instance.SetDefaultStyle(this._remainingSecondsText);
    this._remainingSecondsText.BitmapMeshText.AlphaMin = 0.4f;
  }

  private void OnTeamChange(OnSetPlayerTeamEvent ev)
  {
    if (ev.TeamId == TeamID.RED)
    {
      this._circleBackground.Texture = (Texture) HudTextures.QIRed1;
      this._circleForeground.Texture = (Texture) HudTextures.QIRed3;
      this._remainingSecondsText.ShadowColorAnim.Color = HudStyleUtility.DEFAULT_RED_COLOR;
    }
    else
    {
      this._circleBackground.Texture = (Texture) HudTextures.QIBlue1;
      this._circleForeground.Texture = (Texture) HudTextures.QIBlue3;
      this._remainingSecondsText.ShadowColorAnim.Color = HudStyleUtility.DEFAULT_BLUE_COLOR;
    }
    this._remainingSecondsText.BitmapMeshText.AlphaMin = 0.4f;
  }

  private void OnUpdateRemainingSeconds()
  {
    if (this._totalSeconds == 0)
      return;
    this._circleForeground.Angle = 360f * (float) -this._remainingSeconds / (float) this._totalSeconds;
  }
}
