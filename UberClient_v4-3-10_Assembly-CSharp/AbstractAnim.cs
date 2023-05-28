// Decompiled with JetBrains decompiler
// Type: AbstractAnim
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class AbstractAnim : IUpdatable, IAnim
{
  public bool IsAnimating { get; set; }

  public float Duration { get; set; }

  public float StartTime { get; set; }

  public void Start()
  {
    this.IsAnimating = true;
    this.StartTime = Time.time;
    this.OnStart();
  }

  public void Stop()
  {
    this.OnStop();
    this.IsAnimating = false;
  }

  public void Update()
  {
    if (!this.IsAnimating)
      return;
    if ((double) Time.time > (double) this.StartTime + (double) this.Duration)
      this.Stop();
    else
      this.OnUpdate();
  }

  protected virtual void OnUpdate()
  {
  }

  protected virtual void OnStart()
  {
  }

  protected virtual void OnStop()
  {
  }
}
