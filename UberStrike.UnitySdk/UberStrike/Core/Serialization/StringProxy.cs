// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.StringProxy
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

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
