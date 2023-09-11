
using System;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class ShortProxy
  {
    public static void Serialize(Stream bytes, short instance)
    {
      byte[] bytes1 = BitConverter.GetBytes(instance);
      bytes.Write(bytes1, 0, bytes1.Length);
    }

    public static short Deserialize(Stream bytes)
    {
      byte[] buffer = new byte[2];
      bytes.Read(buffer, 0, 2);
      return BitConverter.ToInt16(buffer, 0);
    }
  }
}
