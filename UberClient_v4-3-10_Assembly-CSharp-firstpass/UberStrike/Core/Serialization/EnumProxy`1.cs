// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.EnumProxy`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

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
