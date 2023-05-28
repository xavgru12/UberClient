// Decompiled with JetBrains decompiler
// Type: MatchStatusHud
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class MatchStatusHud : Singleton<MatchStatusHud>
{
  private float _textScale;
  private MeshGUIText _teamScoreLeft;
  private Sprite2DGUI _boxOverlayRed;
  private Animatable2DGroup _leftScoreGroup;
  private MeshGUIText _teamScoreRight;
  private Sprite2DGUI _boxOverlayBlue;
  private Animatable2DGroup _rightScoreGroup;
  private Animatable2DGroup _scoreGroup;
  private MeshGUIText _remainingKillText;
  private MeshGUIText _clockText;
  private Animatable2DGroup _entireGroup;
  private int _curLeftTeamScore;
  private int _curRightTeamScore;
  private int _remainingSeconds;
  private int _remainingKills;
  private float normalHalfClockHeight;
  private Dictionary<int, AudioClip> _killLeftAudioMap;
  private static int WARNING_TIME_LOW_VALUE = 30;

  private MatchStatusHud()
  {
    this._remainingSeconds = 0;
    this._teamScoreLeft = new MeshGUIText("0", HudAssets.Instance.InterparkBitmapFont, TextAnchor.MiddleCenter);
    this._teamScoreLeft.NamePrefix = "TeamScore";
    this._boxOverlayBlue = new Sprite2DGUI(new GUIContent(), StormFront.BlueBox);
    this._leftScoreGroup = new Animatable2DGroup();
    this._leftScoreGroup.Group.Add((IAnimatable2D) this._teamScoreLeft);
    this._leftScoreGroup.Group.Add((IAnimatable2D) this._boxOverlayBlue);
    this._teamScoreRight = new MeshGUIText("0", HudAssets.Instance.InterparkBitmapFont, TextAnchor.MiddleCenter);
    this._teamScoreRight.NamePrefix = "TeamScore";
    this._boxOverlayRed = new Sprite2DGUI(new GUIContent(), StormFront.RedBox);
    this._rightScoreGroup = new Animatable2DGroup();
    this._rightScoreGroup.Group.Add((IAnimatable2D) this._teamScoreRight);
    this._rightScoreGroup.Group.Add((IAnimatable2D) this._boxOverlayRed);
    this._scoreGroup = new Animatable2DGroup();
    this._scoreGroup.Group.Add((IAnimatable2D) this._leftScoreGroup);
    this._scoreGroup.Group.Add((IAnimatable2D) this._rightScoreGroup);
    this._clockText = new MeshGUIText(this.GetClockString(this._remainingSeconds), HudAssets.Instance.InterparkBitmapFont, TextAnchor.UpperCenter);
    this._clockText.NamePrefix = "Clock";
    this._remainingKillText = new MeshGUIText(this.GetRemainingKillString(this._remainingKills), HudAssets.Instance.InterparkBitmapFont, TextAnchor.UpperCenter);
    this._entireGroup = new Animatable2DGroup();
    this._entireGroup.Group.Add((IAnimatable2D) this._scoreGroup);
    this._entireGroup.Group.Add((IAnimatable2D) this._clockText);
    this._entireGroup.Group.Add((IAnimatable2D) this._remainingKillText);
    this._killLeftAudioMap = new Dictionary<int, AudioClip>(5);
    this.ResetHud();
    this.Enabled = true;
    this.IsClockEnabled = false;
    this.IsScoreEnabled = false;
    this.IsRemainingKillEnabled = false;
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

  public bool IsScoreEnabled
  {
    get => this._scoreGroup.IsVisible;
    set
    {
      if (value)
        this._scoreGroup.Show();
      else
        this._scoreGroup.Hide();
    }
  }

  public bool IsClockEnabled
  {
    get => this._clockText.IsVisible;
    set
    {
      if (value)
        this._clockText.Show();
      else
        this._clockText.Hide();
    }
  }

  public bool IsRemainingKillEnabled
  {
    get => this._remainingKillText.IsVisible;
    set
    {
      if (value)
        this._remainingKillText.Show();
      else
        this._remainingKillText.Hide();
    }
  }

  public int RemainingSeconds
  {
    get => this._remainingSeconds;
    set
    {
      if (this._remainingSeconds == value)
        return;
      this._remainingSeconds = value <= 0 ? 0 : value;
      this._clockText.Text = this.GetClockString(this._remainingSeconds);
      this.OnUpdateRemainingSeconds();
    }
  }

  public int RemainingKills
  {
    get => this._remainingKills;
    set
    {
      if (this._remainingKills == value)
        return;
      this._remainingKills = value <= 0 ? 0 : value;
      this._remainingKillText.Text = this.GetRemainingKillString(this._remainingKills);
    }
  }

  public string RemainingRoundsText
  {
    set => this._remainingKillText.Text = value;
  }

  public int BlueTeamScore
  {
    get => this._curLeftTeamScore;
    set
    {
      this._curLeftTeamScore = value;
      this._teamScoreLeft.Text = this._curLeftTeamScore.ToString();
      this.ResetTeamScoreGroupTransform();
    }
  }

  public int RedTeamScore
  {
    get => this._curRightTeamScore;
    set
    {
      this._curRightTeamScore = value;
      this._teamScoreRight.Text = this._curRightTeamScore.ToString();
      this.ResetTeamScoreGroupTransform();
    }
  }

  public void Draw() => this._entireGroup.Draw(0.0f, 0.0f);

  private void ResetHud()
  {
    this.ResetStyle();
    this.ResetTransform();
  }

  private void ResetStyle()
  {
    Singleton<HudStyleUtility>.Instance.SetDefaultStyle(this._teamScoreLeft);
    Singleton<HudStyleUtility>.Instance.SetDefaultStyle(this._teamScoreRight);
    this._teamScoreRight.BitmapMeshText.ShadowColor = HudStyleUtility.DEFAULT_RED_COLOR;
    Singleton<HudStyleUtility>.Instance.SetDefaultStyle(this._clockText);
    Singleton<HudStyleUtility>.Instance.SetNoShadowStyle(this._remainingKillText);
    this._remainingKillText.BitmapMeshText.ShadowColor = Color.black;
    this._remainingKillText.BitmapMeshText.AlphaMin = 0.1f;
    this._remainingKillText.BitmapMeshText.AlphaMax = 0.6f;
  }

  private void OnTeamChange(OnSetPlayerTeamEvent ev)
  {
    if (ev.TeamId == TeamID.RED)
      Singleton<HudStyleUtility>.Instance.SetRedStyle(this._clockText);
    else
      Singleton<HudStyleUtility>.Instance.SetBlueStyle(this._clockText);
  }

  private void OnGlobalUIRibbonChange(OnGlobalUIRibbonChanged ev) => this.ResetTransform();

  private void OnScreenResolutionChange(ScreenResolutionEvent ev) => this.ResetTransform();

  private void ResetTransform()
  {
    this._textScale = 0.45f;
    this.ResetClockText();
    this.ResetRemainingKillText();
    this.ResetTeamScoreGroupTransform();
    float y = (float) Screen.height * 0.02f;
    if (GlobalUIRibbon.Instance.IsEnabled)
      y += 44f;
    this._entireGroup.Position = new Vector2((float) (Screen.width / 2), y);
  }

  private void ResetClockText()
  {
    this._clockText.Scale = new Vector2(this._textScale, this._textScale);
    this._clockText.Position = Vector2.zero;
    this.normalHalfClockHeight = this._clockText.Rect.height / 2f;
  }

  private void ResetRemainingKillText()
  {
    this._remainingKillText.Scale = new Vector2(this._textScale * 0.4f, this._textScale * 0.4f);
    this._remainingKillText.Position = new Vector2(this._clockText.Position.x, this._clockText.Position.y + this._clockText.Size.y);
  }

  private void ResetTeamScoreGroupTransform()
  {
    MeshGUIText teamScoreLeft = this._teamScoreLeft;
    Vector2 vector2_1 = new Vector2(this._textScale * 1.2f, this._textScale * 1.2f);
    this._teamScoreRight.Scale = vector2_1;
    Vector2 vector2_2 = vector2_1;
    teamScoreLeft.Scale = vector2_2;
    int num1 = this._teamScoreLeft.Text.Length <= this._teamScoreRight.Text.Length ? this._teamScoreRight.Text.Length : this._teamScoreLeft.Text.Length;
    float y = this._teamScoreLeft.Size.y;
    float num2 = y * 1.2f;
    float num3 = num1 > 2 ? (float) ((double) y * (double) num1 * 0.5) : num2;
    float num4 = 1.3f;
    this._boxOverlayBlue.Scale = new Vector2(num3 / this._boxOverlayBlue.GUIBounds.x, y / this._boxOverlayBlue.GUIBounds.y) * num4;
    this._boxOverlayRed.Scale = new Vector2(num3 / this._boxOverlayRed.GUIBounds.x, y / this._boxOverlayRed.GUIBounds.y) * num4;
    this._boxOverlayRed.Position = -this._boxOverlayRed.Size / 2f;
    this._boxOverlayBlue.Position = -this._boxOverlayBlue.Size / 2f;
    this._leftScoreGroup.Position = new Vector2((float) (-(double) num3 / 2.0 - (double) Screen.height * 0.090000003576278687), this.normalHalfClockHeight * 1.5f);
    this._rightScoreGroup.Position = new Vector2((float) ((double) num3 / 2.0 + (double) Screen.height * 0.090000003576278687), this.normalHalfClockHeight * 1.5f);
  }

  [DebuggerHidden]
  private IEnumerator PulseClock() => (IEnumerator) new MatchStatusHud.\u003CPulseClock\u003Ec__Iterator4B()
  {
    \u003C\u003Ef__this = this
  };

  private void OnUpdateRemainingSeconds()
  {
    if (this._remainingSeconds <= MatchStatusHud.WARNING_TIME_LOW_VALUE)
    {
      if (this._remainingSeconds != 0)
        MonoRoutine.Start(this.PulseClock());
      else
        this.StopClockPulsing();
    }
    switch (this._remainingSeconds)
    {
      case 1:
        SfxManager.Play2dAudioClip(GameAudio.MatchEndingCountdown1);
        break;
      case 2:
        SfxManager.Play2dAudioClip(GameAudio.MatchEndingCountdown2);
        break;
      case 3:
        SfxManager.Play2dAudioClip(GameAudio.MatchEndingCountdown3);
        break;
      case 4:
        SfxManager.Play2dAudioClip(GameAudio.MatchEndingCountdown4);
        break;
      case 5:
        SfxManager.Play2dAudioClip(GameAudio.MatchEndingCountdown5);
        break;
    }
  }

  private void StopClockPulsing()
  {
    Singleton<PreemptiveCoroutineManager>.Instance.IncrementId(new PreemptiveCoroutineManager.CoroutineFunction(this.PulseClock));
    this._clockText.StopScaling();
    this.ResetClockText();
  }

  public void PlayKillsLeftAudio(int killsLeft)
  {
    AudioClip soundEffect;
    if (!this._killLeftAudioMap.TryGetValue(killsLeft, out soundEffect))
      return;
    SfxManager.Play2dAudioClip(soundEffect, 2f);
    this._killLeftAudioMap.Remove(killsLeft);
  }

  public void ResetKillsLeftAudio()
  {
    AudioClip[] audioClipArray = new AudioClip[5]
    {
      GameAudio.KillLeft1,
      GameAudio.KillLeft2,
      GameAudio.KillLeft3,
      GameAudio.KillLeft4,
      GameAudio.KillLeft5
    };
    for (int index = 0; index < audioClipArray.Length; ++index)
      this._killLeftAudioMap[index + 1] = audioClipArray[index];
  }

  private string GetClockString(int remainingSeconds)
  {
    int num1 = remainingSeconds / 60;
    int num2 = remainingSeconds % 60;
    return (num1 < 10 ? "0" + (object) num1 : num1.ToString()) + ":" + (num2 < 10 ? "0" + (object) num2 : num2.ToString());
  }

  private string GetRemainingKillString(int remainingKills) => remainingKills > 1 ? remainingKills.ToString() + " Kills Left" : remainingKills.ToString() + " Kill Left";
}
