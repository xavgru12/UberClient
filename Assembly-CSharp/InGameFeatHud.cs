// Decompiled with JetBrains decompiler
// Type: InGameFeatHud
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

internal class InGameFeatHud : Singleton<InGameFeatHud>, IUpdatable
{
  private AnimationSchedulerNew _animScheduler;

  private InGameFeatHud()
  {
    this._animScheduler = new AnimationSchedulerNew();
    this._animScheduler.ScheduleAnimation = new AnimationSchedulerNew.ScheduleAnimationDelegate(ScheduleStrategy.ScheduleOverlap);
  }

  public float TextHeight => (float) Screen.height * 0.08f;

  public Vector2 AnchorPoint => new Vector2((float) (Screen.width / 2), (float) Screen.height * 0.26f);

  public AnimationSchedulerNew AnimationScheduler => this._animScheduler;

  public void Update() => this._animScheduler.Update();
}
