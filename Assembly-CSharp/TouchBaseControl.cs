// Decompiled with JetBrains decompiler
// Type: TouchBaseControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public abstract class TouchBaseControl
{
  public TouchBaseControl() => Singleton<TouchController>.Instance.AddControl(this);

  public virtual bool Enabled { get; set; }

  public virtual Rect Boundary { get; set; }

  public virtual void FirstUpdate()
  {
  }

  public virtual void UpdateTouches(Touch touch)
  {
  }

  public virtual void FinalUpdate()
  {
  }

  public virtual void Draw()
  {
  }

  ~TouchBaseControl() => Singleton<TouchController>.Instance.RemoveControl(this);

  public class TouchFinger
  {
    public Vector2 StartPos;
    public Vector2 LastPos;
    public float StartTouchTime;
    public int FingerId;

    public TouchFinger() => this.Reset();

    public void Reset()
    {
      this.StartPos = Vector2.zero;
      this.LastPos = Vector2.zero;
      this.StartTouchTime = 0.0f;
      this.FingerId = -1;
    }
  }
}
