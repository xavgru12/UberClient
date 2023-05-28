// Decompiled with JetBrains decompiler
// Type: PanelGuiBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public abstract class PanelGuiBase : MonoBehaviour, IPanelGui
{
  public virtual void Show() => this.enabled = true;

  public virtual void Hide() => this.enabled = false;

  public bool IsEnabled => this.enabled;
}
