
using System;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class EnumProxy<T>
  {
    public static void Serialize(Stream bytes, T instance)
    {
      byte[] bytes1 = BitConverter.GetBytes(Convert.ToInt32((object) instance));
      bytes.Write(bytes1, 0, bytes1.Length);
    }

    public static T Deserialize(Stream bytes)
    {
      byte[] buffer = new byte[4];
      bytes.Read(buffer, 0, 4);
      return (T) Enum.ToObject(typeof (T), BitConverter.ToInt32(buffer, 0));
    }
  }
}
