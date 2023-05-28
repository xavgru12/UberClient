// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.UberStrikeItemShopClientViewProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using System.IO;
using UberStrike.Core.Models.Views;

namespace UberStrike.Core.Serialization
{
  public static class UberStrikeItemShopClientViewProxy
  {
    public static void Serialize(Stream stream, UberStrikeItemShopClientView instance)
    {
      int num = 0;
      using (MemoryStream bytes = new MemoryStream())
      {
        if (instance.FunctionalItems != null)
          ListProxy<UberStrikeItemFunctionalView>.Serialize((Stream) bytes, (ICollection<UberStrikeItemFunctionalView>) instance.FunctionalItems, new ListProxy<UberStrikeItemFunctionalView>.Serializer<UberStrikeItemFunctionalView>(UberStrikeItemFunctionalViewProxy.Serialize));
        else
          num |= 1;
        if (instance.GearItems != null)
          ListProxy<UberStrikeItemGearView>.Serialize((Stream) bytes, (ICollection<UberStrikeItemGearView>) instance.GearItems, new ListProxy<UberStrikeItemGearView>.Serializer<UberStrikeItemGearView>(UberStrikeItemGearViewProxy.Serialize));
        else
          num |= 2;
        if (instance.QuickItems != null)
          ListProxy<UberStrikeItemQuickView>.Serialize((Stream) bytes, (ICollection<UberStrikeItemQuickView>) instance.QuickItems, new ListProxy<UberStrikeItemQuickView>.Serializer<UberStrikeItemQuickView>(UberStrikeItemQuickViewProxy.Serialize));
        else
          num |= 4;
        if (instance.WeaponItems != null)
          ListProxy<UberStrikeItemWeaponView>.Serialize((Stream) bytes, (ICollection<UberStrikeItemWeaponView>) instance.WeaponItems, new ListProxy<UberStrikeItemWeaponView>.Serializer<UberStrikeItemWeaponView>(UberStrikeItemWeaponViewProxy.Serialize));
        else
          num |= 8;
        Int32Proxy.Serialize(stream, ~num);
        bytes.WriteTo(stream);
      }
    }

    public static UberStrikeItemShopClientView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      UberStrikeItemShopClientView itemShopClientView = new UberStrikeItemShopClientView();
      if ((num & 1) != 0)
        itemShopClientView.FunctionalItems = ListProxy<UberStrikeItemFunctionalView>.Deserialize(bytes, new ListProxy<UberStrikeItemFunctionalView>.Deserializer<UberStrikeItemFunctionalView>(UberStrikeItemFunctionalViewProxy.Deserialize));
      if ((num & 2) != 0)
        itemShopClientView.GearItems = ListProxy<UberStrikeItemGearView>.Deserialize(bytes, new ListProxy<UberStrikeItemGearView>.Deserializer<UberStrikeItemGearView>(UberStrikeItemGearViewProxy.Deserialize));
      if ((num & 4) != 0)
        itemShopClientView.QuickItems = ListProxy<UberStrikeItemQuickView>.Deserialize(bytes, new ListProxy<UberStrikeItemQuickView>.Deserializer<UberStrikeItemQuickView>(UberStrikeItemQuickViewProxy.Deserialize));
      if ((num & 8) != 0)
        itemShopClientView.WeaponItems = ListProxy<UberStrikeItemWeaponView>.Deserialize(bytes, new ListProxy<UberStrikeItemWeaponView>.Deserializer<UberStrikeItemWeaponView>(UberStrikeItemWeaponViewProxy.Deserialize));
      return itemShopClientView;
    }
  }
}
