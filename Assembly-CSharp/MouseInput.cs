// Decompiled with JetBrains decompiler
// Type: MouseInput
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class MouseInput : Singleton<MouseInput>
{
  public const float DoubleClickInterval = 0.3f;
  private MouseInput.Click Current;
  private MouseInput.Click Previous;

  private MouseInput()
  {
  }

  public bool DoubleClick(Rect rect) => (double) Time.time - (double) this.Previous.Time < 0.30000001192092896 && GUITools.ToGlobal(rect).Contains(this.Current.Point);

  public void OnGUI()
  {
    if (Event.current.type != UnityEngine.EventType.MouseDown)
      return;
    this.Previous = this.Current;
    this.Current.Time = Time.time;
    this.Current.Point = Event.current.mousePosition;
  }

  private struct Click
  {
    public float Time;
    public Vector2 Point;
  }
}
