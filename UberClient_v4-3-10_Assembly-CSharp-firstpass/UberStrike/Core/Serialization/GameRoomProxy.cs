// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.GameRoomProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System.IO;
using UberStrike.Core.Models;

namespace UberStrike.Core.Serialization
{
  public static class GameRoomProxy
  {
    public static void Serialize(Stream stream, GameRoom instance)
    {
      int num = 0;
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, instance.MapId);
        Int32Proxy.Serialize((Stream) bytes, instance.Number);
        if (instance.Server != null)
          ConnectionAddressProxy.Serialize((Stream) bytes, instance.Server);
        else
          num |= 1;
        Int32Proxy.Serialize(stream, ~num);
        bytes.WriteTo(stream);
      }
    }

    public static GameRoom Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      GameRoom gameRoom = new GameRoom();
      gameRoom.MapId = Int32Proxy.Deserialize(bytes);
      gameRoom.Number = Int32Proxy.Deserialize(bytes);
      if ((num & 1) != 0)
        gameRoom.Server = ConnectionAddressProxy.Deserialize(bytes);
      return gameRoom;
    }
  }
}
