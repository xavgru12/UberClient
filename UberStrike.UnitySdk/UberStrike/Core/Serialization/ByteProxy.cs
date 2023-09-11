// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.ByteProxy
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class ByteProxy
  {
    public static void Serialize(Stream bytes, byte instance) => bytes.WriteByte(instance);

    public static byte Deserialize(Stream bytes) => (byte) bytes.ReadByte();
  }
}
