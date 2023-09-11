// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Models.Views.UberStrikeItemShopClientView
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using System;
using System.Collections.Generic;

namespace UberStrike.Core.Models.Views
{
  [Serializable]
  public class UberStrikeItemShopClientView
  {
    public List<UberStrikeItemFunctionalView> FunctionalItems { get; set; }

    public List<UberStrikeItemGearView> GearItems { get; set; }

    public List<UberStrikeItemQuickView> QuickItems { get; set; }

    public List<UberStrikeItemWeaponView> WeaponItems { get; set; }

    public Dictionary<int, int> ItemsRecommendationPerMap { get; set; }
  }
}
