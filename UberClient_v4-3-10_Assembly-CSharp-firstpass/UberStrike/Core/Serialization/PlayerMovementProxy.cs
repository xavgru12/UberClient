// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.PlayerMovementProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System.IO;
using UberStrike.Core.Models;

namespace UberStrike.Core.Serialization
{
  public static class PlayerMovementProxy
  {
    public static void Serialize(Stream stream, PlayerMovement instance)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        ByteProxy.Serialize((Stream) bytes, instance.HorizontalRotation);
        ByteProxy.Serialize((Stream) bytes, instance.KeyState);
        ByteProxy.Serialize((Stream) bytes, instance.MovementState);
        ByteProxy.Serialize((Stream) bytes, instance.Number);
        ShortVector3Proxy.Serialize((Stream) bytes, instance.Position);
        ShortVector3Proxy.Serialize((Stream) bytes, instance.Velocity);
        ByteProxy.Serialize((Stream) bytes, instance.VerticalRotation);
        bytes.WriteTo(stream);
      }
    }

    public static PlayerMovement Deserialize(Stream bytes) => new PlayerMovement()
    {
      HorizontalRotation = ByteProxy.Deserialize(bytes),
      KeyState = ByteProxy.Deserialize(bytes),
      MovementState = ByteProxy.Deserialize(bytes),
      Number = ByteProxy.Deserialize(bytes),
      Position = ShortVector3Proxy.Deserialize(bytes),
      Velocity = ShortVector3Proxy.Deserialize(bytes),
      VerticalRotation = ByteProxy.Deserialize(bytes)
    };
  }
}
