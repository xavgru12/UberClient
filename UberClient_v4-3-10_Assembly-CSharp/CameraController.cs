// Decompiled with JetBrains decompiler
// Type: CameraController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class CameraController : Singleton<CameraController>
{
  private CameraComponents _currentConfiguration;
  private bool _isOrbitEnabled;

  private CameraController()
  {
  }

  public void SetCameraConfiguration(CameraComponents cameraConfiguration)
  {
    this._currentConfiguration = cameraConfiguration;
    this.UpdateConfiguration();
  }

  internal void RemoveCameraConfiguration(CameraComponents cameraConfiguration)
  {
    if (!((Object) this._currentConfiguration == (Object) cameraConfiguration))
      return;
    this._currentConfiguration = (CameraComponents) null;
  }

  private void UpdateConfiguration()
  {
    if ((Object) this._currentConfiguration == (Object) null)
      return;
    this.EnableMouseOrbit = this._isOrbitEnabled;
  }

  public bool EnableMouseOrbit
  {
    get => this._isOrbitEnabled;
    set
    {
      this._isOrbitEnabled = false;
      if (!(bool) (Object) this._currentConfiguration || !(bool) (Object) this._currentConfiguration.MouseOrbit)
        return;
      this._currentConfiguration.MouseOrbit.enabled = this._isOrbitEnabled;
    }
  }
}
