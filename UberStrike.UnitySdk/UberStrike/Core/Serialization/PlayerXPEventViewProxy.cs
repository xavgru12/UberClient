// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.PlayerXPEventViewProxy
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using System.IO;
using UberStrike.DataCenter.Common.Entities;

namespace UberStrike.Core.Serialization
{
  public static class PlayerXPEventViewProxy
  {
    public static void Serialize(Stream stream, PlayerXPEventView instance)
    {
      int num = 0;
      if (instance != null)
      {
        using (MemoryStream bytes = new MemoryStream())
        {
          if (instance.Name != null)
            StringProxy.Serialize((Stream) bytes, instance.Name);
          else
            num |= 1;
          Int32Proxy.Serialize((Stream) bytes, instance.PlayerXPEventId);
          DecimalProxy.Serialize((Stream) bytes, instance.XPMultiplier);
          Int32Proxy.Serialize(stream, ~num);
          bytes.WriteTo(stream);
        }
      }
      else
        Int32Proxy.Serialize(stream, 0);
    }

    public static PlayerXPEventView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      PlayerXPEventView playerXpEventView = (PlayerXPEventView) null;
      if (num != 0)
      {
        playerXpEventView = new PlayerXPEventView();
        if ((num & 1) != 0)
          playerXpEventView.Name = StringProxy.Deserialize(bytes);
        playerXpEventView.PlayerXPEventId = Int32Proxy.Deserialize(bytes);
        playerXpEventView.XPMultiplier = DecimalProxy.Deserialize(bytes);
      }
      return playerXpEventView;
    }
  }
}
