// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.SingleProxy
// Assembly: UberStrike.Core.Serialization, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: 950E20E9-3609-4E9B-B4D8-B32B07AB805E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Core.Serialization.dll

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
