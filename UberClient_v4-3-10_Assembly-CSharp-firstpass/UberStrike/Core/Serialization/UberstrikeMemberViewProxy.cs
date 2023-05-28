// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.UberstrikeMemberViewProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System.IO;
using UberStrike.DataCenter.Common.Entities;

namespace UberStrike.Core.Serialization
{
  public static class UberstrikeMemberViewProxy
  {
    public static void Serialize(Stream stream, UberstrikeMemberView instance)
    {
      int num = 0;
      using (MemoryStream memoryStream = new MemoryStream())
      {
        if (instance.PlayerCardView != null)
          PlayerCardViewProxy.Serialize((Stream) memoryStream, instance.PlayerCardView);
        else
          num |= 1;
        if (instance.PlayerStatisticsView != null)
          PlayerStatisticsViewProxy.Serialize((Stream) memoryStream, instance.PlayerStatisticsView);
        else
          num |= 2;
        Int32Proxy.Serialize(stream, ~num);
        memoryStream.WriteTo(stream);
      }
    }

    public static UberstrikeMemberView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      UberstrikeMemberView uberstrikeMemberView = new UberstrikeMemberView();
      if ((num & 1) != 0)
        uberstrikeMemberView.PlayerCardView = PlayerCardViewProxy.Deserialize(bytes);
      if ((num & 2) != 0)
        uberstrikeMemberView.PlayerStatisticsView = PlayerStatisticsViewProxy.Deserialize(bytes);
      return uberstrikeMemberView;
    }
  }
}
