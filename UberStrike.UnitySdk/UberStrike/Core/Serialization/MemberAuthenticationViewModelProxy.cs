﻿
using Cmune.DataCenter.Common.Entities;
using System.IO;
using UberStrike.Core.ViewModel;

namespace UberStrike.Core.Serialization
{
  public static class MemberAuthenticationViewModelProxy
  {
    public static void Serialize(Stream stream, MemberAuthenticationViewModel instance)
    {
      int num = 0;
      if (instance != null)
      {
        using (MemoryStream bytes = new MemoryStream())
        {
          EnumProxy<MemberAuthenticationResult>.Serialize((Stream) bytes, instance.MemberAuthenticationResult);
          if (instance.MemberView != null)
            MemberViewProxy.Serialize((Stream) bytes, instance.MemberView);
          else
            num |= 1;
          Int32Proxy.Serialize(stream, ~num);
          bytes.WriteTo(stream);
        }
      }
      else
        Int32Proxy.Serialize(stream, 0);
    }

    public static MemberAuthenticationViewModel Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      MemberAuthenticationViewModel authenticationViewModel = (MemberAuthenticationViewModel) null;
      if (num != 0)
      {
        authenticationViewModel = new MemberAuthenticationViewModel();
        authenticationViewModel.MemberAuthenticationResult = EnumProxy<MemberAuthenticationResult>.Deserialize(bytes);
        if ((num & 1) != 0)
          authenticationViewModel.MemberView = MemberViewProxy.Deserialize(bytes);
      }
      return authenticationViewModel;
    }
  }
}
