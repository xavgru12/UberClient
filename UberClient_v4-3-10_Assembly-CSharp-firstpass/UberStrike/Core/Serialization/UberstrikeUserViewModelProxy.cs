// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.UberstrikeUserViewModelProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System.IO;
using UberStrike.Core.ViewModel;

namespace UberStrike.Core.Serialization
{
  public static class UberstrikeUserViewModelProxy
  {
    public static void Serialize(Stream stream, UberstrikeUserViewModel instance)
    {
      int num = 0;
      using (MemoryStream memoryStream = new MemoryStream())
      {
        if (instance.CmuneMemberView != null)
          MemberViewProxy.Serialize((Stream) memoryStream, instance.CmuneMemberView);
        else
          num |= 1;
        if (instance.UberstrikeMemberView != null)
          UberstrikeMemberViewProxy.Serialize((Stream) memoryStream, instance.UberstrikeMemberView);
        else
          num |= 2;
        Int32Proxy.Serialize(stream, ~num);
        memoryStream.WriteTo(stream);
      }
    }

    public static UberstrikeUserViewModel Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      UberstrikeUserViewModel uberstrikeUserViewModel = new UberstrikeUserViewModel();
      if ((num & 1) != 0)
        uberstrikeUserViewModel.CmuneMemberView = MemberViewProxy.Deserialize(bytes);
      if ((num & 2) != 0)
        uberstrikeUserViewModel.UberstrikeMemberView = UberstrikeMemberViewProxy.Deserialize(bytes);
      return uberstrikeUserViewModel;
    }
  }
}
