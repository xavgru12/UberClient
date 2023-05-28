// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.MemberAuthenticationResultViewProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using Cmune.DataCenter.Common.Entities;
using System.IO;
using UberStrike.Core.ViewModel;

namespace UberStrike.Core.Serialization
{
  public static class MemberAuthenticationResultViewProxy
  {
    public static void Serialize(Stream stream, MemberAuthenticationResultView instance)
    {
      int num = 0;
      using (MemoryStream bytes = new MemoryStream())
      {
        if (instance.AuthToken != null)
          StringProxy.Serialize((Stream) bytes, instance.AuthToken);
        else
          num |= 1;
        BooleanProxy.Serialize((Stream) bytes, instance.IsAccountComplete);
        if (instance.LuckyDraw != null)
          LuckyDrawUnityViewProxy.Serialize((Stream) bytes, instance.LuckyDraw);
        else
          num |= 2;
        EnumProxy<MemberAuthenticationResult>.Serialize((Stream) bytes, instance.MemberAuthenticationResult);
        if (instance.MemberView != null)
          MemberViewProxy.Serialize((Stream) bytes, instance.MemberView);
        else
          num |= 4;
        if (instance.PlayerStatisticsView != null)
          PlayerStatisticsViewProxy.Serialize((Stream) bytes, instance.PlayerStatisticsView);
        else
          num |= 8;
        DateTimeProxy.Serialize((Stream) bytes, instance.ServerTime);
        StringProxy.Serialize((Stream) bytes, instance.ServerGameVersion);
        Int32Proxy.Serialize(stream, ~num);
        bytes.WriteTo(stream);
      }
    }

    public static MemberAuthenticationResultView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      MemberAuthenticationResultView authenticationResultView = new MemberAuthenticationResultView();
      if ((num & 1) != 0)
        authenticationResultView.AuthToken = StringProxy.Deserialize(bytes);
      authenticationResultView.IsAccountComplete = BooleanProxy.Deserialize(bytes);
      if ((num & 2) != 0)
        authenticationResultView.LuckyDraw = LuckyDrawUnityViewProxy.Deserialize(bytes);
      authenticationResultView.MemberAuthenticationResult = EnumProxy<MemberAuthenticationResult>.Deserialize(bytes);
      if ((num & 4) != 0)
        authenticationResultView.MemberView = MemberViewProxy.Deserialize(bytes);
      if ((num & 8) != 0)
        authenticationResultView.PlayerStatisticsView = PlayerStatisticsViewProxy.Deserialize(bytes);
      authenticationResultView.ServerTime = DateTimeProxy.Deserialize(bytes);
      authenticationResultView.ServerGameVersion = StringProxy.Deserialize(bytes);
      authenticationResultView.BanDuration = Int32Proxy.Deserialize(bytes);
      return authenticationResultView;
    }
  }
}
