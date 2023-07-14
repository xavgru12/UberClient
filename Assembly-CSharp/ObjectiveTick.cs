// Decompiled with JetBrains decompiler
// Type: ObjectiveTick
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class ObjectiveTick
{
  private bool _completed;
  private Texture _bk;
  private Texture _tip;

  public ObjectiveTick(Texture bk, Texture tip)
  {
    this._bk = bk;
    this._tip = tip;
  }

  public void Draw(Vector2 position, float scale)
  {
    int num = 78;
    Vector2 pivotPoint = new Vector2(position.x, position.y + (float) ((double) num * (double) scale / 2.0));
    GUIUtility.ScaleAroundPivot(new Vector2(scale, scale), pivotPoint);
    GUI.BeginGroup(new Rect(position.x, position.y, (float) num, (float) num));
    GUI.Label(new Rect(0.0f, 0.0f, 78f, 78f), this._bk);
    if (this._completed)
      GUI.DrawTexture(new Rect(4f, 16f, 62f, 49f), this._tip);
    GUI.EndGroup();
    GUI.matrix = Matrix4x4.identity;
  }

  public void Complete() => this._completed = true;
}
