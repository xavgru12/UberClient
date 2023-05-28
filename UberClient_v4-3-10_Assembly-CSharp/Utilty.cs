// Decompiled with JetBrains decompiler
// Type: Utilty
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Text;
using UnityEngine;

public static class Utilty
{
  public static string GetPath(Transform transform)
  {
    StringBuilder stringBuilder = new StringBuilder();
    if ((Object) transform != (Object) null)
    {
      stringBuilder.Append("/").Append(transform.name);
      while ((Object) transform.transform.parent != (Object) null)
      {
        transform = transform.parent;
        stringBuilder.Insert(0, "/" + transform.name);
      }
    }
    else
      stringBuilder.Append("null");
    return stringBuilder.ToString();
  }
}
