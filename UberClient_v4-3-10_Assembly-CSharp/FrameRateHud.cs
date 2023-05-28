// Decompiled with JetBrains decompiler
// Type: FrameRateHud
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using Cmune.DataCenter.Common.Entities;
using System;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

internal class FrameRateHud : Singleton<FrameRateHud>
{
  private MeshGUIText _frameRateText;

  private FrameRateHud()
  {
    this._frameRateText = new MeshGUIText(string.Empty, HudAssets.Instance.InterparkBitmapFont, TextAnchor.UpperRight);
    Singleton<HudStyleUtility>.Instance.SetNoShadowStyle(this._frameRateText);
    this.ResetTransform();
    this.Enable = false;
    CmuneEventHandler.AddListener<ScreenResolutionEvent>(new Action<ScreenResolutionEvent>(this.OnScreenResolutionChange));
  }

  public bool Enable
  {
    get => this._frameRateText.IsVisible;
    set
    {
      if (ApplicationDataManager.BuildType == BuildType.Dev)
        value = false;
      if (value)
        this._frameRateText.Show();
      else
        this._frameRateText.Hide();
    }
  }

  public void Draw()
  {
  }

  private void OnScreenResolutionChange(ScreenResolutionEvent ev) => this.ResetTransform();

  private void ResetTransform()
  {
    float num = 0.2f;
    this._frameRateText.Scale = new Vector2(num, num);
    this._frameRateText.Position = new Vector2((float) Screen.width * 0.99f, (float) Screen.height * 0.01f);
  }
}
