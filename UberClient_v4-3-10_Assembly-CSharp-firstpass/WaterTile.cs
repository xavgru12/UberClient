// Decompiled with JetBrains decompiler
// Type: WaterTile
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

[ExecuteInEditMode]
public class WaterTile : MonoBehaviour
{
  public PlanarReflection reflection;
  public WaterBase waterBase;

  public void Start() => this.AcquireComponents();

  private void AcquireComponents()
  {
    if (!(bool) (Object) this.reflection)
      this.reflection = !(bool) (Object) this.transform.parent ? this.transform.GetComponent<PlanarReflection>() : this.transform.parent.GetComponent<PlanarReflection>();
    if ((bool) (Object) this.waterBase)
      return;
    if ((bool) (Object) this.transform.parent)
      this.waterBase = this.transform.parent.GetComponent<WaterBase>();
    else
      this.waterBase = this.transform.GetComponent<WaterBase>();
  }

  public void OnWillRenderObject()
  {
    if ((bool) (Object) this.reflection)
      this.reflection.WaterTileBeingRendered(this.transform, Camera.current);
    if (!(bool) (Object) this.waterBase)
      return;
    this.waterBase.WaterTileBeingRendered(this.transform, Camera.current);
  }
}
