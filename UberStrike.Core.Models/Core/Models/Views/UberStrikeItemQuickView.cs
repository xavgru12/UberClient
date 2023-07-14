// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Models.Views.UberStrikeItemQuickView
// Assembly: UberStrike.Core.Models, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: E29887F9-C6F9-4A17-AD3C-0A827CA1DCD6
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Core.Models.dll

using System;
using UberStrike.Core.Types;

namespace UberStrike.Core.Models.Views
{
  [Serializable]
  public class UberStrikeItemQuickView : BaseUberStrikeItemView
  {
    public override UberstrikeItemType ItemType => UberstrikeItemType.QuickUse;

    public int UsesPerLife { get; set; }

    public int UsesPerRound { get; set; }

    public int UsesPerGame { get; set; }

    public int CoolDownTime { get; set; }

    public int WarmUpTime { get; set; }

    public int MaxOwnableAmount { get; set; }

    public QuickItemLogic BehaviourType { get; set; }
  }
}
