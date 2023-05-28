// Decompiled with JetBrains decompiler
// Type: AnimationSchedulerNew
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

public class AnimationSchedulerNew : IUpdatable
{
  private List<IAnim> _animGroup;
  private Queue<IAnim> _standbyQueue;
  private List<IAnim> _toBeRemoved;

  public AnimationSchedulerNew()
  {
    this._animGroup = new List<IAnim>();
    this._standbyQueue = new Queue<IAnim>();
    this._toBeRemoved = new List<IAnim>();
    this.ScheduleAnimation = new AnimationSchedulerNew.ScheduleAnimationDelegate(ScheduleStrategy.ScheduleInSequence);
  }

  public AnimationSchedulerNew.ScheduleAnimationDelegate ScheduleAnimation { get; set; }

  public void Update()
  {
    if (this.ScheduleAnimation != null)
      this.ScheduleAnimation(this);
    this.UpdateAnimation();
  }

  public void EnqueueAnim(IAnim anim) => this._standbyQueue.Enqueue(anim);

  public IAnim DequeueAnim() => this._standbyQueue.Count != 0 ? this._standbyQueue.Dequeue() : (IAnim) null;

  public int StandbyQueueCount => this._standbyQueue.Count;

  public void AddToAnimGroup(IAnim anim) => this._animGroup.Add(anim);

  public void RemoveFromAnimGroup(IAnim anim) => this._animGroup.Remove(anim);

  public IAnim GetFromAnimGroup(int index) => index >= 0 && index < this._animGroup.Count ? this._animGroup[index] : (IAnim) null;

  public int AnimGroupCount => this._animGroup.Count;

  public void ClearAll()
  {
    this._standbyQueue.Clear();
    this.ClearAnimGroup();
  }

  public void ClearAnimGroup()
  {
    this._toBeRemoved.Clear();
    foreach (IAnim anim in this._animGroup)
      this._toBeRemoved.Add(anim);
    foreach (IAnim anim in this._toBeRemoved)
    {
      anim.Stop();
      this._animGroup.Remove(anim);
    }
  }

  private void UpdateAnimation()
  {
    this._toBeRemoved.Clear();
    foreach (IAnim anim in this._animGroup)
    {
      anim.Update();
      if (!anim.IsAnimating)
        this._toBeRemoved.Add(anim);
    }
    foreach (IAnim anim in this._toBeRemoved)
      this._animGroup.Remove(anim);
  }

  public delegate void ScheduleAnimationDelegate(AnimationSchedulerNew animScheduler);
}
