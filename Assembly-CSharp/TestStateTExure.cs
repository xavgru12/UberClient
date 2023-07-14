// Decompiled with JetBrains decompiler
// Type: TestStateTExure
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class TestStateTExure : MonoBehaviour
{
  public Texture[] textures;
  private StateTexture2D texture;
  private int index;

  private void Awake() => this.texture = new StateTexture2D(this.textures);

  private void OnGUI()
  {
    if (GUI.Button(new Rect(100f, 100f, 100f, 20f), "Change"))
      this.texture.ChangeState(++this.index % this.textures.Length);
    GUI.DrawTexture(new Rect(100f, 150f, 100f, 100f), this.texture.Current);
  }
}
