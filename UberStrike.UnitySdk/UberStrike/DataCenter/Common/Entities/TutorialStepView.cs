// Decompiled with JetBrains decompiler
// Type: UberStrike.DataCenter.Common.Entities.TutorialStepView
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

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
