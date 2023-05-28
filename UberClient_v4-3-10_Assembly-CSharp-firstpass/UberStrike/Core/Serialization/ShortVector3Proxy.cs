// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.ShortVector3Proxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.IO;
using UberStrike.Core.Models;
using UnityEngine;

namespace UberStrike.Core.Serialization
{
  public static class ShortVector3Proxy
  {
    public static void Serialize(Stream bytes, ShortVector3 instance)
    {
      bytes.Write(BitConverter.GetBytes((short) Mathf.Clamp(instance.x * 100f, (float) short.MinValue, (float) short.MaxValue)), 0, 2);
      bytes.Write(BitConverter.GetBytes((short) Mathf.Clamp(instance.y * 100f, (float) short.MinValue, (float) short.MaxValue)), 0, 2);
      bytes.Write(BitConverter.GetBytes((short) Mathf.Clamp(instance.z * 100f, (float) short.MinValue, (float) short.MaxValue)), 0, 2);
    }

    public static ShortVector3 Deserialize(Stream bytes)
    {
      byte[] buffer = new byte[6];
      bytes.Read(buffer, 0, 6);
      return (ShortVector3) new Vector3(0.01f * (float) BitConverter.ToInt16(buffer, 0), 0.01f * (float) BitConverter.ToInt16(buffer, 2), 0.01f * (float) BitConverter.ToInt16(buffer, 4));
    }
  }
}
