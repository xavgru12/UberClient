// Decompiled with JetBrains decompiler
// Type: Cmune.DataCenter.Common.Entities.BuyItemResult
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

namespace Cmune.DataCenter.Common.Entities
{
  public enum BuyItemResult
  {
    OK = 0,
    DisableInShop = 1,
    DisableForRent = 3,
    DisableForPermanent = 4,
    DurationDisabled = 5,
    PackDisabled = 6,
    IsNotForSale = 7,
    NotEnoughCurrency = 8,
    InvalidMember = 9,
    InvalidExpirationDate = 10, // 0x0000000A
    AlreadyInInventory = 11, // 0x0000000B
    InvalidAmount = 12, // 0x0000000C
    NoStockRemaining = 13, // 0x0000000D
    InvalidData = 14, // 0x0000000E
    TooManyUsage = 15, // 0x0000000F
    InvalidLevel = 100, // 0x00000064
    ItemNotFound = 404, // 0x00000194
  }
}
