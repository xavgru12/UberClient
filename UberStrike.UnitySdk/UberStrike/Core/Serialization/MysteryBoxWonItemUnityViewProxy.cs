﻿
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
