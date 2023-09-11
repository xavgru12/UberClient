// Decompiled with JetBrains decompiler
// Type: UberStrike.DataCenter.Common.Entities.ItemQuickUseConfigView
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using System;
using UberStrike.Core.Types;

namespace UberStrike.DataCenter.Common.Entities
{
  [Serializable]
  public class ItemQuickUseConfigView
  {
    public int ItemId { get; set; }

    public int LevelRequired { get; set; }

    public int UsesPerLife { get; set; }

    public int UsesPerRound { get; set; }

    public int UsesPerGame { get; set; }

    public int CoolDownTime { get; set; }

    public int WarmUpTime { get; set; }

    public QuickItemLogic BehaviourType { get; set; }
  }
}
