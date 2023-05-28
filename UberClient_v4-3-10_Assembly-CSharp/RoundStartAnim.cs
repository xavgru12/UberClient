// Decompiled with JetBrains decompiler
// Type: RoundStartAnim
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class RoundStartAnim : PopupAnim
{
  private bool _countdown5;
  private bool _countdown4;
  private bool _countdown3;
  private bool _countdown2;
  private bool _countdown1;
  private bool _countdown0;

  public RoundStartAnim(
    Animatable2DGroup popupGroup,
    MeshGUIQuad glowBlur,
    MeshGUIText multiKillText,
    Vector2 spawnPosition,
    Vector2 destBlurScale,
    Vector2 destMultiKillScale,
    float displayTime,
    float fadeOutTime,
    AudioClip sound)
    : base(popupGroup, glowBlur, multiKillText, spawnPosition, destBlurScale, destMultiKillScale, displayTime, fadeOutTime, sound)
  {
  }

  protected override void OnStart()
  {
    base.OnStart();
    this._countdown5 = false;
    this._countdown4 = false;
    this._countdown3 = false;
    this._countdown2 = false;
    this._countdown1 = false;
    this._countdown0 = false;
  }

  protected override void OnUpdate()
  {
    base.OnUpdate();
    if (!this.IsAnimating)
      return;
    int num = 6 - Mathf.CeilToInt(Time.time - this.StartTime);
    if (num == 5 && !this._countdown5)
    {
      this.OnRoundStartCountdown("Round Starts In 5", GameAudio.MatchEndingCountdown5);
      this._countdown5 = true;
    }
    else if (num == 4 && !this._countdown4)
    {
      this.OnRoundStartCountdown("Round Starts In 4", GameAudio.MatchEndingCountdown4);
      this._countdown4 = true;
    }
    else if (num == 3 && !this._countdown3)
    {
      this.OnRoundStartCountdown("Round Starts In 3", GameAudio.MatchEndingCountdown3);
      this._countdown3 = true;
    }
    else if (num == 2 && !this._countdown2)
    {
      this.OnRoundStartCountdown("Round Starts In 2", GameAudio.MatchEndingCountdown2);
      this._countdown2 = true;
    }
    else if (num == 1 && !this._countdown1)
    {
      this.OnRoundStartCountdown("Round Starts In 1", GameAudio.MatchEndingCountdown1);
      this._countdown1 = true;
    }
    else
    {
      if (num != 0 || this._countdown0)
        return;
      this.OnRoundStartCountdown("Fight", GameAudio.Fight);
      this._countdown0 = true;
    }
  }

  private void OnRoundStartCountdown(string textStr, AudioClip sound)
  {
    this._popupText.Text = textStr;
    SfxManager.Play2dAudioClip(sound);
  }
}
