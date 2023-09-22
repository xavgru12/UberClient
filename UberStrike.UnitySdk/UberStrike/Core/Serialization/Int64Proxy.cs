
using System;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class Int64Proxy
  {
    public static void Serialize(Stream bytes, long instance)
    {
      byte[] bytes1 = BitConverter.GetBytes(instance);
      bytes.Write(bytes1, 0, bytes1.Length);
    }

    public static long Deserialize(Stream bytes)
    {
      byte[] buffer = new byte[8];
      bytes.Read(buffer, 0, 8);
      return BitConverter.ToInt64(buffer, 0);
    }
  }
}
