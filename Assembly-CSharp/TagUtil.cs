// Decompiled with JetBrains decompiler
// Type: TagUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public static class TagUtil
{
  public static string GetTag(Collider c)
  {
    string tag = "Cement";
    try
    {
      if ((bool) (UnityEngine.Object) c)
        tag = c.tag;
    }
    catch (Exception ex)
    {
      Debug.LogError((object) ("Failed to get tag from collider: " + ex.Message));
    }
    return tag;
  }
}
