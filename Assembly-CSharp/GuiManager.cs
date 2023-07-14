// Decompiled with JetBrains decompiler
// Type: GuiManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public static class GuiManager
{
  public static void DrawTooltip()
  {
    if (string.IsNullOrEmpty(GUI.tooltip))
      return;
    Matrix4x4 matrix = GUI.matrix;
    GUI.matrix = Matrix4x4.identity;
    Vector2 vector2 = BlueStonez.tooltip.CalcSize(new GUIContent(GUI.tooltip));
    Rect position = new Rect(Mathf.Clamp(Event.current.mousePosition.x, 14f, (float) Screen.width - (vector2.x + 14f)), Event.current.mousePosition.y + 24f, vector2.x, vector2.y + 16f);
    if ((double) position.yMax > (double) Screen.height)
    {
      position.x += 30f;
      position.y += (float) Screen.height - position.yMax;
    }
    if ((double) position.xMax > (double) Screen.width)
      position.x += (float) Screen.width - position.xMax;
    GUI.Label(position, GUI.tooltip, BlueStonez.tooltip);
    GUI.matrix = matrix;
  }
}
