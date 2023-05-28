// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.MysteryBoxWonItemUnityViewProxy
// Assembly: UberStrike.Core.Serialization, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: 950E20E9-3609-4E9B-B4D8-B32B07AB805E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Core.Serialization.dll

using Cmune.DataCenter.Common.Entities;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class MysteryBoxWonItemUnityViewProxy
  {
    public static void Serialize(Stream stream, MysteryBoxWonItemUnityView instance)
    {
      int num = 0;
      if (instance != null)
      {
        using (MemoryStream bytes = new MemoryStream())
        {
          Int32Proxy.Serialize((Stream) bytes, instance.CreditWon);
          Int32Proxy.Serialize((Stream) bytes, instance.ItemIdWon);
          Int32Proxy.Serialize((Stream) bytes, instance.PointWon);
          Int32Proxy.Serialize(stream, ~num);
          bytes.WriteTo(stream);
        }
      }
      else
        Int32Proxy.Serialize(stream, 0);
    }

    public static MysteryBoxWonItemUnityView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      MysteryBoxWonItemUnityView wonItemUnityView = (MysteryBoxWonItemUnityView) null;
      if (num != 0)
      {
        wonItemUnityView = new MysteryBoxWonItemUnityView();
        wonItemUnityView.CreditWon = Int32Proxy.Deserialize(bytes);
        wonItemUnityView.ItemIdWon = Int32Proxy.Deserialize(bytes);
        wonItemUnityView.PointWon = Int32Proxy.Deserialize(bytes);
      }
      return wonItemUnityView;
    }
  }
}
