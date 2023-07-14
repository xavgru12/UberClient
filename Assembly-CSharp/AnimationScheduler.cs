// Decompiled with JetBrains decompiler
// Type: AnimationScheduler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

public class AnimationScheduler
{
  private List<AnimationScheduler.AnimationInfo> _animGroup;
  private Queue<AnimationScheduler.AnimationInfo> _standbyQueue;

  public AnimationScheduler()
  {
    this._animGroup = new List<AnimationScheduler.AnimationInfo>();
    this._standbyQueue = new Queue<AnimationScheduler.AnimationInfo>();
  }

  public void Draw()
  {
    this.ScheduleAnimation();
    foreach (AnimationScheduler.AnimationInfo animationInfo in this._animGroup)
      animationInfo._animatable.Draw();
  }

  public void AddAnimation(
    IAnimatable2D animatable,
    AnimationScheduler.DoAnim anim,
    string[] animArgs,
    AnimationScheduler.OnAnimOver animOverEvent,
    string[] eventArgs)
  {
    AnimationScheduler.AnimationInfo animationInfo = new AnimationScheduler.AnimationInfo(animatable, anim, animArgs, animOverEvent, eventArgs);
    animationInfo._animatable.Hide();
    this._standbyQueue.Enqueue(animationInfo);
  }

  private void ScheduleAnimation()
  {
    if (this._animGroup.Count != 0 || this._standbyQueue.Count == 0)
      return;
    MonoRoutine.Start(this.DoAnimation(this._standbyQueue.Dequeue()));
  }

  [DebuggerHidden]
  private IEnumerator DoAnimation(AnimationScheduler.AnimationInfo animInfo) => (IEnumerator) new AnimationScheduler.\u003CDoAnimation\u003Ec__Iterator45()
  {
    animInfo = animInfo,
    \u003C\u0024\u003EanimInfo = animInfo,
    \u003C\u003Ef__this = this
  };

  private class AnimationInfo
  {
    public IAnimatable2D _animatable;
    public AnimationScheduler.DoAnim _animFunction;
    public string[] _animArgs;
    public AnimationScheduler.OnAnimOver _animOverEvent;
    public string[] _eventArgs;

    public AnimationInfo(
      IAnimatable2D animatable,
      AnimationScheduler.DoAnim anim,
      string[] animArgs,
      AnimationScheduler.OnAnimOver animOverEvent,
      string[] eventArgs)
    {
      this._animatable = animatable;
      this._animFunction = anim;
      this._animArgs = animArgs;
      this._animOverEvent = animOverEvent;
      this._eventArgs = eventArgs;
    }
  }

  public delegate IEnumerator DoAnim(IAnimatable2D animatable, string[] animArgs);

  public delegate IEnumerator OnAnimOver(IAnimatable2D animatable, string[] args);
}
