// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.WeeklySpecialViewProxy
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using System;
using System.IO;
using UberStrike.DataCenter.Common.Entities;

namespace UberStrike.Core.Serialization
{
  public static class WeeklySpecialViewProxy
  {
    public static void Serialize(Stream stream, WeeklySpecialView instance)
    {
      int num = 0;
      if (instance != null)
      {
        using (MemoryStream bytes = new MemoryStream())
        {
          if (instance.EndDate.HasValue)
            DateTimeProxy.Serialize((Stream) bytes, instance.EndDate ?? new DateTime());
          else
            num |= 1;
          Int32Proxy.Serialize((Stream) bytes, instance.Id);
          if (instance.ImageUrl != null)
            StringProxy.Serialize((Stream) bytes, instance.ImageUrl);
          else
            num |= 2;
          Int32Proxy.Serialize((Stream) bytes, instance.ItemId);
          DateTimeProxy.Serialize((Stream) bytes, instance.StartDate);
          if (instance.Text != null)
            StringProxy.Serialize((Stream) bytes, instance.Text);
          else
            num |= 4;
          if (instance.Title != null)
            StringProxy.Serialize((Stream) bytes, instance.Title);
          else
            num |= 8;
          Int32Proxy.Serialize(stream, ~num);
          bytes.WriteTo(stream);
        }
      }
      else
        Int32Proxy.Serialize(stream, 0);
    }

    public static WeeklySpecialView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      WeeklySpecialView weeklySpecialView = (WeeklySpecialView) null;
      if (num != 0)
      {
        weeklySpecialView = new WeeklySpecialView();
        if ((num & 1) != 0)
          weeklySpecialView.EndDate = new DateTime?(DateTimeProxy.Deserialize(bytes));
        weeklySpecialView.Id = Int32Proxy.Deserialize(bytes);
        if ((num & 2) != 0)
          weeklySpecialView.ImageUrl = StringProxy.Deserialize(bytes);
        weeklySpecialView.ItemId = Int32Proxy.Deserialize(bytes);
        weeklySpecialView.StartDate = DateTimeProxy.Deserialize(bytes);
        if ((num & 4) != 0)
          weeklySpecialView.Text = StringProxy.Deserialize(bytes);
        if ((num & 8) != 0)
          weeklySpecialView.Title = StringProxy.Deserialize(bytes);
      }
      return weeklySpecialView;
    }
  }
}
