// Decompiled with JetBrains decompiler
// Type: ScheduleStrategy
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6C8FEFFB-EA1C-4C92-899E-E8175A35455F
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class ScheduleStrategy
{
  public static void ScheduleInSequence(AnimationSchedulerNew animScheduler)
  {
    if (animScheduler.AnimGroupCount != 0 || animScheduler.StandbyQueueCount == 0)
      return;
    ScheduleStrategy.ScheduleNextAnim(animScheduler);
  }

  public static void PreemptiveSchedule(AnimationSchedulerNew animScheduler)
  {
    if (animScheduler.StandbyQueueCount == 0)
      return;
    animScheduler.ClearAnimGroup();
    ScheduleStrategy.ScheduleNextAnim(animScheduler);
  }

  public static void ScheduleOverlap(AnimationSchedulerNew animScheduler)
  {
    if (animScheduler.StandbyQueueCount == 0)
      return;
    if (animScheduler.AnimGroupCount == 0)
    {
      ScheduleStrategy.ScheduleNextAnim(animScheduler);
    }
    else
    {
      if (animScheduler.AnimGroupCount != 1)
        return;
      IAnim fromAnimGroup = animScheduler.GetFromAnimGroup(0);
      if ((double) Time.time - (double) fromAnimGroup.StartTime <= (double) fromAnimGroup.Duration * 0.5)
        return;
      ScheduleStrategy.ScheduleNextAnim(animScheduler);
    }
  }

  private static void ScheduleNextAnim(AnimationSchedulerNew animScheduler)
  {
    IAnim anim = animScheduler.DequeueAnim();
    animScheduler.AddToAnimGroup(anim);
    anim.Start();
  }
}
