
using System;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class BooleanProxy
  {
    public static void Serialize(Stream bytes, bool instance)
    {
      byte[] bytes1 = BitConverter.GetBytes(instance);
      bytes.Write(bytes1, 0, bytes1.Length);
    }

    public static bool Deserialize(Stream bytes)
    {
      byte[] buffer = new byte[1];
      bytes.Read(buffer, 0, 1);
      return BitConverter.ToBoolean(buffer, 0);
    }
  }
}
