// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Models.Views.UberStrikeItemShopClientView
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

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
  }
}
