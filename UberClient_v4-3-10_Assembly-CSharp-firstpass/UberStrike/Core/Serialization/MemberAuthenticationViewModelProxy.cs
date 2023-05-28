// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.MemberAuthenticationViewModelProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

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

    public static MemberAuthenticationViewModel Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      MemberAuthenticationViewModel authenticationViewModel = new MemberAuthenticationViewModel();
      authenticationViewModel.MemberAuthenticationResult = EnumProxy<MemberAuthenticationResult>.Deserialize(bytes);
      if ((num & 1) != 0)
        authenticationViewModel.MemberView = MemberViewProxy.Deserialize(bytes);
      return authenticationViewModel;
    }
  }
}
