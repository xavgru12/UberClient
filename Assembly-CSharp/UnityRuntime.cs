// Decompiled with JetBrains decompiler
// Type: UnityRuntime
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;

public class UnityRuntime : AutoMonoBehaviour<UnityRuntime>
{
  public event Action OnGui;

  public event Action OnUpdate;

  public event Action OnFixedUpdate;

  public event Action<bool> OnAppFocus;

  private void FixedUpdate()
  {
    if (this.OnFixedUpdate == null)
      return;
    this.OnFixedUpdate();
  }

  private void Update()
  {
    if (this.OnUpdate == null)
      return;
    this.OnUpdate();
  }

  private void OnGUI()
  {
    if (this.OnGui == null)
      return;
    this.OnGui();
  }

  private void OnApplicationFocus(bool focus)
  {
    if (this.OnAppFocus == null)
      return;
    this.OnAppFocus(focus);
  }
}
