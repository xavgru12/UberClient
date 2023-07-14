// Decompiled with JetBrains decompiler
// Type: WaitingTexture
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public static class WaitingTexture
{
  public static void Draw(Vector2 position, int size = 0)
  {
    size = size > 0 ? Mathf.Clamp(size, 1, 32) : 32;
    GUIUtility.RotateAroundPivot((float) WaitingTexture.Angle, position);
    GUI.DrawTexture(new Rect(position.x - (float) size * 0.5f, position.y - (float) size * 0.5f, (float) size, (float) size), (Texture) UberstrikeIcons.Waiting);
    GUI.matrix = Matrix4x4.identity;
  }

  public static int Angle => Mathf.RoundToInt(Time.time * 10f) * 30;
}
