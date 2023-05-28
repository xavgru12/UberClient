// Decompiled with JetBrains decompiler
// Type: TouchController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class TouchController : Singleton<TouchController>
{
  public float GUIAlpha = 1f;
  private List<TouchBaseControl> _controls;

  private TouchController()
  {
    this._controls = new List<TouchBaseControl>();
    AutoMonoBehaviour<UnityRuntime>.Instance.OnUpdate += new Action(this.OnUpdate);
    AutoMonoBehaviour<UnityRuntime>.Instance.OnGui += new Action(this.OnGui);
  }

  private void OnUpdate()
  {
    foreach (TouchBaseControl control in this._controls)
    {
      if (control.Enabled)
      {
        control.FirstUpdate();
        foreach (Touch touch in Input.touches)
          control.UpdateTouches(touch);
        control.FinalUpdate();
      }
    }
  }

  private void OnGui()
  {
    foreach (TouchBaseControl control in this._controls)
    {
      if (control.Enabled)
        control.Draw();
    }
  }

  public void AddControl(TouchBaseControl control) => this._controls.Add(control);

  public void RemoveControl(TouchBaseControl control) => this._controls.Remove(control);
}
