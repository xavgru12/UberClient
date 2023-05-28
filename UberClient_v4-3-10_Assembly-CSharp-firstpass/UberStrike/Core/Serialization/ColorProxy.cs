// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.ColorProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.IO;
using UnityEngine;

namespace UberStrike.Core.Serialization
{
  public static class ColorProxy
  {
    public static void Serialize(Stream bytes, Color instance)
    {
      bytes.Write(BitConverter.GetBytes(instance.r), 0, 4);
      bytes.Write(BitConverter.GetBytes(instance.g), 0, 4);
      bytes.Write(BitConverter.GetBytes(instance.b), 0, 4);
    }

    public static Color Deserialize(Stream bytes)
    {
      byte[] buffer = new byte[12];
      bytes.Read(buffer, 0, 12);
      return new Color(BitConverter.ToSingle(buffer, 0), BitConverter.ToSingle(buffer, 4), BitConverter.ToSingle(buffer, 8));
    }
  }
}
