// Decompiled with JetBrains decompiler
// Type: ScreenshotHud
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

internal class ScreenshotHud : Singleton<ScreenshotHud>
{
  private MeshGUITextFormat _helpText;

  private ScreenshotHud()
  {
    this._helpText = new MeshGUITextFormat("Screenshot mode\nPress [P] to enable HUD", HudAssets.Instance.InterparkBitmapFont, TextAlignment.Right, new MeshGUITextFormat.SetStyle(Singleton<HudStyleUtility>.Instance.SetNoShadowStyle));
    this.ResetTransform();
    this.Enable = false;
    CmuneEventHandler.AddListener<ScreenResolutionEvent>(new Action<ScreenResolutionEvent>(this.OnScreenResolutionChange));
  }

  public bool Enable
  {
    get => this._helpText.IsVisible;
    set
    {
      if (value)
        this._helpText.Show();
      else
        this._helpText.Hide();
    }
  }

  public void Draw()
  {
    if (Event.current.type == UnityEngine.EventType.KeyDown && Event.current.keyCode == KeyCode.P && GameState.CurrentGame.IsMatchRunning && GameState.LocalCharacter.IsAlive)
      Singleton<HudDrawFlagGroup>.Instance.IsScreenshotMode = !Singleton<HudDrawFlagGroup>.Instance.IsScreenshotMode;
    if (this.Enable != Singleton<HudDrawFlagGroup>.Instance.IsScreenshotMode)
      this.Enable = Singleton<HudDrawFlagGroup>.Instance.IsScreenshotMode;
    this._helpText.Draw(0.0f, 0.0f);
  }

  private void OnScreenResolutionChange(ScreenResolutionEvent ev) => this.ResetTransform();

  private void ResetTransform()
  {
    float num = 0.2f;
    this._helpText.Scale = new Vector2(num, num);
    this._helpText.LineGap = this._helpText.Rect.height * 0.1f;
    this._helpText.Position = new Vector2((float) Screen.width * 0.99f, (float) Screen.height * 0.99f - this._helpText.Rect.height);
  }
}
