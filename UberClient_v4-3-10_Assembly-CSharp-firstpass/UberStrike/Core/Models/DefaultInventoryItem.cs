// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Models.DefaultInventoryItem
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

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
