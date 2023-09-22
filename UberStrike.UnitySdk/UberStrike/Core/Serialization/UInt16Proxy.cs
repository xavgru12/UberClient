
using System;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class UInt16Proxy
  {
    public static void Serialize(Stream bytes, ushort instance)
    {
      byte[] bytes1 = BitConverter.GetBytes(instance);
      bytes.Write(bytes1, 0, bytes1.Length);
    }

    public static ushort Deserialize(Stream bytes)
    {
      byte[] buffer = new byte[2];
      bytes.Read(buffer, 0, 2);
      return BitConverter.ToUInt16(buffer, 0);
    }
  }
}
