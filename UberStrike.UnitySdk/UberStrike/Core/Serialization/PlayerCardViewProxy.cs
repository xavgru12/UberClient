// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.PlayerCardViewProxy
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using System.IO;
using UberStrike.DataCenter.Common.Entities;

namespace UberStrike.Core.Serialization
{
  public static class PlayerCardViewProxy
  {
    public static void Serialize(Stream stream, PlayerCardView instance)
    {
      int num = 0;
      if (instance != null)
      {
        using (MemoryStream bytes = new MemoryStream())
        {
          Int32Proxy.Serialize((Stream) bytes, instance.Cmid);
          Int64Proxy.Serialize((Stream) bytes, instance.Hits);
          if (instance.Name != null)
            StringProxy.Serialize((Stream) bytes, instance.Name);
          else
            num |= 1;
          if (instance.Precision != null)
            StringProxy.Serialize((Stream) bytes, instance.Precision);
          else
            num |= 2;
          Int32Proxy.Serialize((Stream) bytes, instance.Ranking);
          Int64Proxy.Serialize((Stream) bytes, instance.Shots);
          Int32Proxy.Serialize((Stream) bytes, instance.Splats);
          Int32Proxy.Serialize((Stream) bytes, instance.Splatted);
          if (instance.TagName != null)
            StringProxy.Serialize((Stream) bytes, instance.TagName);
          else
            num |= 4;
          Int32Proxy.Serialize(stream, ~num);
          bytes.WriteTo(stream);
        }
      }
      else
        Int32Proxy.Serialize(stream, 0);
    }

    public static PlayerCardView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      PlayerCardView playerCardView = (PlayerCardView) null;
      if (num != 0)
      {
        playerCardView = new PlayerCardView();
        playerCardView.Cmid = Int32Proxy.Deserialize(bytes);
        playerCardView.Hits = Int64Proxy.Deserialize(bytes);
        if ((num & 1) != 0)
          playerCardView.Name = StringProxy.Deserialize(bytes);
        if ((num & 2) != 0)
          playerCardView.Precision = StringProxy.Deserialize(bytes);
        playerCardView.Ranking = Int32Proxy.Deserialize(bytes);
        playerCardView.Shots = Int64Proxy.Deserialize(bytes);
        playerCardView.Splats = Int32Proxy.Deserialize(bytes);
        playerCardView.Splatted = Int32Proxy.Deserialize(bytes);
        if ((num & 4) != 0)
          playerCardView.TagName = StringProxy.Deserialize(bytes);
      }
      return playerCardView;
    }
  }
}
