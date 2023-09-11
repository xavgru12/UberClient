// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.EnumProxy`1
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

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
