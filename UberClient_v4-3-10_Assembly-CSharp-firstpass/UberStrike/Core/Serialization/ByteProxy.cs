// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.ByteProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class ByteProxy
  {
    public static void Serialize(Stream bytes, byte instance) => bytes.WriteByte(instance);

    public static byte Deserialize(Stream bytes) => (byte) bytes.ReadByte();
  }
}
