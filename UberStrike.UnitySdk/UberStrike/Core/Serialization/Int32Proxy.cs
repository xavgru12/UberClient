
using System;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class Int32Proxy
  {
    public static void Serialize(Stream bytes, int instance)
    {
      byte[] bytes1 = BitConverter.GetBytes(instance);
      bytes.Write(bytes1, 0, bytes1.Length);
    }

    public static int Deserialize(Stream bytes)
    {
      byte[] buffer = new byte[4];
      bytes.Read(buffer, 0, 4);
      return BitConverter.ToInt32(buffer, 0);
    }
  }
}
