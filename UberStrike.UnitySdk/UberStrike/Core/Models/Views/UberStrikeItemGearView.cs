// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Models.Views.UberStrikeItemGearView
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using System;
using UberStrike.Core.Types;

namespace UberStrike.Core.Models.Views
{
  [Serializable]
  public class UberStrikeItemGearView : BaseUberStrikeItemView
  {
    public override UberstrikeItemType ItemType => UberstrikeItemType.Gear;

    public int ArmorPoints { get; set; }

    public int ArmorAbsorptionPercent { get; set; }

    public int ArmorWeight { get; set; }
  }
}
