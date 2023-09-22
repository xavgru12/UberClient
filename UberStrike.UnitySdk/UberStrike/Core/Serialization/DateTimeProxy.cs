
using System;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class DateTimeProxy
  {
    public static void Serialize(Stream bytes, DateTime instance)
    {
      byte[] bytes1 = BitConverter.GetBytes(instance.Ticks);
      bytes.Write(bytes1, 0, bytes1.Length);
    }

    public static DateTime Deserialize(Stream bytes)
    {
      byte[] buffer = new byte[8];
      bytes.Read(buffer, 0, 8);
      return new DateTime(BitConverter.ToInt64(buffer, 0));
    }
  }
}
