// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.PlayerXPEventViewProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System.IO;
using UberStrike.DataCenter.Common.Entities;

namespace UberStrike.Core.Serialization
{
  public static class PlayerXPEventViewProxy
  {
    public static void Serialize(Stream stream, PlayerXPEventView instance)
    {
      int num = 0;
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

    public static PlayerXPEventView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      PlayerXPEventView playerXpEventView = new PlayerXPEventView();
      if ((num & 1) != 0)
        playerXpEventView.Name = StringProxy.Deserialize(bytes);
      playerXpEventView.PlayerXPEventId = Int32Proxy.Deserialize(bytes);
      playerXpEventView.XPMultiplier = DecimalProxy.Deserialize(bytes);
      return playerXpEventView;
    }
  }
}
