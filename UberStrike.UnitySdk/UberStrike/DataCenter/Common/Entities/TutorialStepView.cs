
using System;
using UberStrike.Core.Types;

namespace UberStrike.DataCenter.Common.Entities
{
  public class TutorialStepView
  {
    public int Cmid { get; private set; }

    public TutorialStepType StepType { get; private set; }

    public DateTime StepTime { get; private set; }

    public TutorialStepView(int cmid, TutorialStepType stepType, DateTime stepTime)
    {
      this.Cmid = cmid;
      this.StepType = stepType;
      this.StepTime = stepTime;
    }
  }
}
