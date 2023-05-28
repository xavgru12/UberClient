// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.ByteProxy
// Assembly: UberStrike.Core.Serialization, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: 950E20E9-3609-4E9B-B4D8-B32B07AB805E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Core.Serialization.dll

using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class ByteProxy
  {
    public static void Serialize(Stream bytes, byte instance) => bytes.WriteByte(instance);

    public static byte Deserialize(Stream bytes) => (byte) bytes.ReadByte();
  }
}
