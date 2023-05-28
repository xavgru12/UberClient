// Decompiled with JetBrains decompiler
// Type: TeamChangeWarningHud
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class TeamChangeWarningHud : Singleton<TeamChangeWarningHud>
{
  private float _curScaleFactor;
  private MeshGUIText _warningMsg;
  private bool _isTextDisplaying;
  private float _textDisplayTime;
  private float _animFadeOutTime;
  private float _nextHideTime;
  private bool _isFadingOutText;

  private TeamChangeWarningHud()
  {
    this._warningMsg = new MeshGUIText(string.Empty, HudAssets.Instance.InterparkBitmapFont, TextAnchor.MiddleCenter);
    this._animFadeOutTime = 1f;
    this._textDisplayTime = 2f;
    this.ResetTransform();
    this.Enabled = true;
  }

  public bool Enabled
  {
    get => this._warningMsg.IsVisible;
    set
    {
      if (value)
        this._warningMsg.Show();
      else
        this._warningMsg.Hide();
    }
  }

  public bool IsDisplaying
  {
    get => this._isTextDisplaying;
    private set
    {
      this._isTextDisplaying = value;
      if (value)
      {
        this._warningMsg.StopFading();
        this._isFadingOutText = false;
        this._nextHideTime = Time.time + this._textDisplayTime;
      }
      else
      {
        this._warningMsg.Alpha = 0.0f;
        this._isFadingOutText = false;
      }
    }
  }

  public void Draw()
  {
    if (!this.Enabled)
      return;
    this.DrawWarningMsg();
  }

  public void ClearMsg()
  {
    this._warningMsg.Text = string.Empty;
    this.IsDisplaying = false;
  }

  public void DisplayWarningMsg(string warningMsg, Color msgColor)
  {
    this.IsDisplaying = true;
    this._warningMsg.Text = warningMsg;
    Singleton<HudStyleUtility>.Instance.SetNoShadowStyle(this._warningMsg);
    this._warningMsg.Color = msgColor;
    this._warningMsg.ShadowColorAnim.Alpha = 0.0f;
  }

  private void ResetTransform()
  {
    this._curScaleFactor = 0.45f;
    this._warningMsg.Scale = new Vector2(this._curScaleFactor, this._curScaleFactor);
    this._warningMsg.Position = new Vector2((float) (Screen.width / 2), (float) Screen.height * 0.3f);
  }

  private void DrawWarningMsg()
  {
    if (this.IsDisplaying && (double) Time.time > (double) this._nextHideTime)
    {
      if (!this._isFadingOutText)
      {
        this._warningMsg.FadeAlphaTo(0.0f, this._animFadeOutTime, EaseType.Out);
        this._isFadingOutText = true;
      }
      else if ((double) Time.time > (double) this._nextHideTime + (double) this._animFadeOutTime)
        this.IsDisplaying = false;
    }
    this._warningMsg.Draw(0.0f, 0.0f);
    this._warningMsg.ShadowColorAnim.Alpha = 0.0f;
  }
}
