// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Models.DefaultInventoryItem
// Assembly: UberStrike.Core.Models, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: E29887F9-C6F9-4A17-AD3C-0A827CA1DCD6
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Core.Models.dll

using UberStrike.Core.Types;

namespace UberStrike.Core.Models
{
  public class DefaultInventoryItem
  {
    public int ItemId { get; set; }

    public int Duration { get; set; }

    public bool DisplayToPlayer { get; set; }

    public bool EquipOnAccountCreation { get; set; }

    public LoadoutSlotType LoadoutSlot { get; set; }
  }
}
