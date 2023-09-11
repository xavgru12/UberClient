// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.MemberAuthenticationResultViewProxy
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

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
      if (instance != null)
      {
        using (MemoryStream bytes = new MemoryStream())
        {
          BooleanProxy.Serialize((Stream) bytes, instance.IsAccountComplete);
          BooleanProxy.Serialize((Stream) bytes, instance.IsTutorialComplete);
          if (instance.LuckyDraw != null)
            LuckyDrawUnityViewProxy.Serialize((Stream) bytes, instance.LuckyDraw);
          else
            num |= 1;
          EnumProxy<MemberAuthenticationResult>.Serialize((Stream) bytes, instance.MemberAuthenticationResult);
          if (instance.MemberView != null)
            MemberViewProxy.Serialize((Stream) bytes, instance.MemberView);
          else
            num |= 2;
          if (instance.PlayerStatisticsView != null)
            PlayerStatisticsViewProxy.Serialize((Stream) bytes, instance.PlayerStatisticsView);
          else
            num |= 4;
          DateTimeProxy.Serialize((Stream) bytes, instance.ServerTime);
          if (instance.WeeklySpecial != null)
            WeeklySpecialViewProxy.Serialize((Stream) bytes, instance.WeeklySpecial);
          else
            num |= 8;
          Int32Proxy.Serialize(stream, ~num);
          bytes.WriteTo(stream);
        }
      }
      else
        Int32Proxy.Serialize(stream, 0);
    }

    public static MemberAuthenticationResultView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      MemberAuthenticationResultView authenticationResultView = (MemberAuthenticationResultView) null;
      if (num != 0)
      {
        authenticationResultView = new MemberAuthenticationResultView();
        authenticationResultView.IsAccountComplete = BooleanProxy.Deserialize(bytes);
        authenticationResultView.IsTutorialComplete = BooleanProxy.Deserialize(bytes);
        if ((num & 1) != 0)
          authenticationResultView.LuckyDraw = LuckyDrawUnityViewProxy.Deserialize(bytes);
        authenticationResultView.MemberAuthenticationResult = EnumProxy<MemberAuthenticationResult>.Deserialize(bytes);
        if ((num & 2) != 0)
          authenticationResultView.MemberView = MemberViewProxy.Deserialize(bytes);
        if ((num & 4) != 0)
          authenticationResultView.PlayerStatisticsView = PlayerStatisticsViewProxy.Deserialize(bytes);
        authenticationResultView.ServerTime = DateTimeProxy.Deserialize(bytes);
        if ((num & 8) != 0)
          authenticationResultView.WeeklySpecial = WeeklySpecialViewProxy.Deserialize(bytes);
      }
      return authenticationResultView;
    }
  }
}
