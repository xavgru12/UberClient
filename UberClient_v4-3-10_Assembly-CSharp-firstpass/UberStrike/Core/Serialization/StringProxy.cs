// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.StringProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

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
