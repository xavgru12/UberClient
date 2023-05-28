// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.PlayerCardViewProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System.IO;
using UberStrike.DataCenter.Common.Entities;

namespace UberStrike.Core.Serialization
{
  public static class PlayerCardViewProxy
  {
    public static void Serialize(Stream stream, PlayerCardView instance)
    {
      int num = 0;
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

    public static PlayerCardView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      PlayerCardView playerCardView = new PlayerCardView();
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
      return playerCardView;
    }
  }
}
