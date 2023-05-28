// Decompiled with JetBrains decompiler
// Type: CmuneGUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public static class CmuneGUI
{
  public static int HorizontalScrollbar(string title, int value, int min, int max)
  {
    GUILayout.BeginHorizontal();
    GUILayout.Label(title);
    GUILayout.Space(10f);
    float num = GUILayout.HorizontalScrollbar((float) value, 1f, (float) min, (float) (max + 1));
    GUILayout.Space(10f);
    GUILayout.Label(string.Format("{0} [{1},{2}]", (object) value, (object) min, (object) max));
    GUILayout.EndHorizontal();
    return (int) num;
  }

  public static float HorizontalScrollbar(string title, float value, int min, int max)
  {
    GUILayout.BeginHorizontal();
    GUILayout.Label(title);
    GUILayout.Space(10f);
    float num = GUILayout.HorizontalScrollbar(value, 1f, (float) min, (float) (max + 1));
    GUILayout.Space(10f);
    GUILayout.Label(string.Format("{0} [{1},{2}]", (object) value, (object) min, (object) max));
    GUILayout.EndHorizontal();
    return num;
  }
}
