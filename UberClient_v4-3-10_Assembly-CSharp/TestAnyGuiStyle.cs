// Decompiled with JetBrains decompiler
// Type: TestAnyGuiStyle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[ExecuteInEditMode]
public class TestAnyGuiStyle : MonoBehaviour
{
  public GUISkin skin;
  public string style = "label";
  public Vector2 size;
  public string text = string.Empty;

  private void OnGUI()
  {
    GUI.skin = this.skin;
    GUI.Button(new Rect((float) (((double) Screen.width - (double) this.size.x) / 2.0), (float) (((double) Screen.height - (double) this.size.y) / 2.0), this.size.x, this.size.y), this.text, (GUIStyle) this.style);
  }
}
