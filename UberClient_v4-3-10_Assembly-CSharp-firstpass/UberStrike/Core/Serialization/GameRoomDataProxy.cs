// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.GameRoomDataProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System.IO;
using UberStrike.Core.Models;
using UberStrike.Core.Types;

namespace UberStrike.Core.Serialization
{
  public static class GameRoomDataProxy
  {
    public static void Serialize(Stream stream, GameRoomData instance)
    {
      int num = 0;
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, instance.ConnectedPlayers);
        Int32Proxy.Serialize((Stream) bytes, instance.GameFlags);
        EnumProxy<GameModeType>.Serialize((Stream) bytes, instance.GameMode);
        EnumProxy<GameBoxType>.Serialize((Stream) bytes, instance.BoxType);
        if (instance.Guid != null)
          StringProxy.Serialize((Stream) bytes, instance.Guid);
        else
          num |= 1;
        BooleanProxy.Serialize((Stream) bytes, instance.IsPasswordProtected);
        BooleanProxy.Serialize((Stream) bytes, instance.IsPermanentGame);
        Int32Proxy.Serialize((Stream) bytes, instance.KillLimit);
        ByteProxy.Serialize((Stream) bytes, instance.LevelMax);
        ByteProxy.Serialize((Stream) bytes, instance.LevelMin);
        Int32Proxy.Serialize((Stream) bytes, instance.MapID);
        if (instance.Name != null)
          StringProxy.Serialize((Stream) bytes, instance.Name);
        else
          num |= 2;
        Int32Proxy.Serialize((Stream) bytes, instance.Number);
        Int32Proxy.Serialize((Stream) bytes, instance.PlayerLimit);
        if (instance.Server != null)
          ConnectionAddressProxy.Serialize((Stream) bytes, instance.Server);
        else
          num |= 4;
        Int32Proxy.Serialize((Stream) bytes, instance.TimeLimit);
        Int32Proxy.Serialize(stream, ~num);
        bytes.WriteTo(stream);
      }
    }

    public static GameRoomData Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      GameRoomData gameRoomData = new GameRoomData();
      gameRoomData.ConnectedPlayers = Int32Proxy.Deserialize(bytes);
      gameRoomData.GameFlags = Int32Proxy.Deserialize(bytes);
      gameRoomData.GameMode = EnumProxy<GameModeType>.Deserialize(bytes);
      gameRoomData.BoxType = EnumProxy<GameBoxType>.Deserialize(bytes);
      if ((num & 1) != 0)
        gameRoomData.Guid = StringProxy.Deserialize(bytes);
      gameRoomData.IsPasswordProtected = BooleanProxy.Deserialize(bytes);
      gameRoomData.IsPermanentGame = BooleanProxy.Deserialize(bytes);
      gameRoomData.KillLimit = Int32Proxy.Deserialize(bytes);
      gameRoomData.LevelMax = ByteProxy.Deserialize(bytes);
      gameRoomData.LevelMin = ByteProxy.Deserialize(bytes);
      gameRoomData.MapID = Int32Proxy.Deserialize(bytes);
      if ((num & 2) != 0)
        gameRoomData.Name = StringProxy.Deserialize(bytes);
      gameRoomData.Number = Int32Proxy.Deserialize(bytes);
      gameRoomData.PlayerLimit = Int32Proxy.Deserialize(bytes);
      if ((num & 4) != 0)
        gameRoomData.Server = ConnectionAddressProxy.Deserialize(bytes);
      gameRoomData.TimeLimit = Int32Proxy.Deserialize(bytes);
      return gameRoomData;
    }
  }
}
