
using System;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class SingleProxy
  {
    public static void Serialize(Stream bytes, float instance)
    {
      byte[] bytes1 = BitConverter.GetBytes(instance);
      bytes.Write(bytes1, 0, bytes1.Length);
    }

    public static float Deserialize(Stream bytes)
    {
      byte[] buffer = new byte[4];
      bytes.Read(buffer, 0, 4);
      return BitConverter.ToSingle(buffer, 0);
    }
  }
}
