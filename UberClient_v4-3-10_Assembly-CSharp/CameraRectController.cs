// Decompiled with JetBrains decompiler
// Type: CameraRectController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class CameraRectController : Singleton<CameraRectController>
{
  private FloatAnim _cameraWidthAnim;

  private CameraRectController() => this._cameraWidthAnim = new FloatAnim((FloatAnim.OnValueChange) ((oldValue, newValue) =>
  {
    if (!((Object) GameState.CurrentSpace != (Object) null) || !((Object) GameState.CurrentSpace.Camera != (Object) null))
      return;
    GameState.CurrentSpace.Camera.rect = new Rect(0.0f, 0.0f, newValue, 1f);
    CmuneEventHandler.Route((object) new CameraWidthChangeEvent()
    {
      Width = GameState.CurrentSpace.Camera.rect.width
    });
  }), 1f);

  public float Width
  {
    get => this._cameraWidthAnim.Value;
    set => this._cameraWidthAnim.Value = value;
  }

  public void AnimCameraWidth(float destWidth, float time, EaseType easeType) => this._cameraWidthAnim.AnimTo(destWidth, time, easeType);
}
