// Decompiled with JetBrains decompiler
// Type: LayerUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public static class LayerUtil
{
  static LayerUtil() => LayerUtil.ValidateUberstrikeLayers();

  public static void ValidateUberstrikeLayers()
  {
    for (int layer = 0; layer < 32; ++layer)
    {
      if (layer != 2)
      {
        if (!string.IsNullOrEmpty(LayerMask.LayerToName(layer)))
        {
          if (Enum.GetName(typeof (UberstrikeLayer), (object) layer) != LayerMask.LayerToName(layer))
            Debug.LogError((object) ("Editor layer '" + LayerMask.LayerToName(layer) + "' is not defined in the UberstrikeLayer enum!"));
        }
        else if (!string.IsNullOrEmpty(Enum.GetName(typeof (UberstrikeLayer), (object) layer)))
          throw new Exception("UberstrikeLayer mismatch with Editor on layer: " + Enum.GetName(typeof (UberstrikeLayer), (object) layer));
      }
    }
  }

  public static int CreateLayerMask(params UberstrikeLayer[] layers)
  {
    int layerMask = 0;
    foreach (int layer in layers)
      layerMask |= 1 << layer;
    return layerMask;
  }

  public static int AddToLayerMask(int mask, params UberstrikeLayer[] layers)
  {
    foreach (int layer in layers)
      mask |= 1 << layer;
    return mask;
  }

  public static int RemoveFromLayerMask(int mask, params UberstrikeLayer[] layers)
  {
    foreach (int layer in layers)
      mask &= ~(1 << layer);
    return mask;
  }

  public static void SetLayerRecursively(Transform transform, UberstrikeLayer layer)
  {
    foreach (Component componentsInChild in transform.GetComponentsInChildren<Transform>(true))
      componentsInChild.gameObject.layer = (int) layer;
  }

  public static int GetLayer(UberstrikeLayer layer) => (int) layer;

  public static bool IsLayerInMask(int mask, int layer) => (mask & 1 << layer) != 0;

  public static bool IsLayerInMask(int mask, UberstrikeLayer layer) => (mask & 1 << (int) (layer & (UberstrikeLayer.LocallyLit_Refract | UberstrikeLayer.LocallyLit_ReflectRefract))) != 0;

  public static int LayerMaskEverything => -1;

  public static int LayerMaskNothing => 0;
}
