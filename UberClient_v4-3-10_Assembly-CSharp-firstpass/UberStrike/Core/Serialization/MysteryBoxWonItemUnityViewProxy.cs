// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.MysteryBoxWonItemUnityViewProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using Cmune.DataCenter.Common.Entities;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class MysteryBoxWonItemUnityViewProxy
  {
    public static void Serialize(Stream stream, MysteryBoxWonItemUnityView instance)
    {
      using (MemoryStream bytes = new MemoryStream())
      {
        Int32Proxy.Serialize((Stream) bytes, instance.CreditWon);
        Int32Proxy.Serialize((Stream) bytes, instance.ItemIdWon);
        Int32Proxy.Serialize((Stream) bytes, instance.PointWon);
        bytes.WriteTo(stream);
      }
    }

    public static MysteryBoxWonItemUnityView Deserialize(Stream bytes) => new MysteryBoxWonItemUnityView()
    {
      CreditWon = Int32Proxy.Deserialize(bytes),
      ItemIdWon = Int32Proxy.Deserialize(bytes),
      PointWon = Int32Proxy.Deserialize(bytes)
    };
  }
}
