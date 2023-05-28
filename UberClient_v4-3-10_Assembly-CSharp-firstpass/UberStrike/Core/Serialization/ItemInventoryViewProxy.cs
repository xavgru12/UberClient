// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.ItemInventoryViewProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

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
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, instance.AmountRemaining);
        Int32Proxy.Serialize((Stream) bytes, instance.Cmid);
        if (instance.ExpirationDate.HasValue)
        {
          DateTime? expirationDate = instance.ExpirationDate;
          DateTimeProxy.Serialize((Stream) bytes, !expirationDate.HasValue ? new DateTime() : expirationDate.Value);
        }
        else
          num |= 1;
        Int32Proxy.Serialize((Stream) bytes, instance.ItemId);
        Int32Proxy.Serialize(stream, ~num);
        bytes.WriteTo(stream);
      }
    }

    public static ItemInventoryView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      ItemInventoryView itemInventoryView = new ItemInventoryView();
      itemInventoryView.AmountRemaining = Int32Proxy.Deserialize(bytes);
      itemInventoryView.Cmid = Int32Proxy.Deserialize(bytes);
      if ((num & 1) != 0)
        itemInventoryView.ExpirationDate = new DateTime?(DateTimeProxy.Deserialize(bytes));
      itemInventoryView.ItemId = Int32Proxy.Deserialize(bytes);
      return itemInventoryView;
    }
  }
}
