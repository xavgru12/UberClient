// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Models.Views.UberStrikeItemShopClientView
// Assembly: UberStrike.Core.Models, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: E29887F9-C6F9-4A17-AD3C-0A827CA1DCD6
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Core.Models.dll

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
