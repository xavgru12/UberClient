// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.ItemInventoryViewProxy
// Assembly: UberStrike.Core.Serialization, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: 950E20E9-3609-4E9B-B4D8-B32B07AB805E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Core.Serialization.dll

using Cmune.DataCenter.Common.Entities;
using System;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class ItemInventoryViewProxy
  {
    public static void Serialize(Stream stream, ItemInventoryView instance)
    {
      int num = 0;
      if (instance != null)
      {
        using (MemoryStream bytes = new MemoryStream())
        {
          Int32Proxy.Serialize((Stream) bytes, instance.AmountRemaining);
          Int32Proxy.Serialize((Stream) bytes, instance.Cmid);
          if (instance.ExpirationDate.HasValue)
            DateTimeProxy.Serialize((Stream) bytes, instance.ExpirationDate ?? new DateTime());
          else
            num |= 1;
          Int32Proxy.Serialize((Stream) bytes, instance.ItemId);
          Int32Proxy.Serialize(stream, ~num);
          bytes.WriteTo(stream);
        }
      }
      else
        Int32Proxy.Serialize(stream, 0);
    }

    public static ItemInventoryView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      ItemInventoryView itemInventoryView = (ItemInventoryView) null;
      if (num != 0)
      {
        itemInventoryView = new ItemInventoryView();
        itemInventoryView.AmountRemaining = Int32Proxy.Deserialize(bytes);
        itemInventoryView.Cmid = Int32Proxy.Deserialize(bytes);
        if ((num & 1) != 0)
          itemInventoryView.ExpirationDate = new DateTime?(DateTimeProxy.Deserialize(bytes));
        itemInventoryView.ItemId = Int32Proxy.Deserialize(bytes);
      }
      return itemInventoryView;
    }
  }
}
