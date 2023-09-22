
using System.IO;
using System.Text;

namespace UberStrike.Core.Serialization
{
  public static class StringProxy
  {
    public static void Serialize(Stream bytes, string instance)
    {
      if (string.IsNullOrEmpty(instance))
      {
        UShortProxy.Serialize(bytes, (ushort) 0);
      }
      else
      {
        UShortProxy.Serialize(bytes, (ushort) instance.Length);
        byte[] bytes1 = Encoding.Unicode.GetBytes(instance);
        bytes.Write(bytes1, 0, bytes1.Length);
      }
    }

    public static string Deserialize(Stream bytes)
    {
      ushort num = UShortProxy.Deserialize(bytes);
      if (num <= (ushort) 0)
        return string.Empty;
      byte[] numArray = new byte[(int) num * 2];
      bytes.Read(numArray, 0, numArray.Length);
      return Encoding.Unicode.GetString(numArray, 0, numArray.Length);
    }
  }
}
