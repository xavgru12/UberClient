// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.ItemInventoryViewProxy
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

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
