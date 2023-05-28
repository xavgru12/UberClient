// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.UberstrikeUserViewModelProxy
// Assembly: UberStrike.Core.Serialization, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: 950E20E9-3609-4E9B-B4D8-B32B07AB805E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Core.Serialization.dll

using System.IO;
using UberStrike.Core.ViewModel;

namespace UberStrike.Core.Serialization
{
  public static class UberstrikeUserViewModelProxy
  {
    public static void Serialize(Stream stream, UberstrikeUserViewModel instance)
    {
      int num = 0;
      if (instance != null)
      {
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
      else
        Int32Proxy.Serialize(stream, 0);
    }

    public static UberstrikeUserViewModel Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      UberstrikeUserViewModel uberstrikeUserViewModel = (UberstrikeUserViewModel) null;
      if (num != 0)
      {
        uberstrikeUserViewModel = new UberstrikeUserViewModel();
        if ((num & 1) != 0)
          uberstrikeUserViewModel.CmuneMemberView = MemberViewProxy.Deserialize(bytes);
        if ((num & 2) != 0)
          uberstrikeUserViewModel.UberstrikeMemberView = UberstrikeMemberViewProxy.Deserialize(bytes);
      }
      return uberstrikeUserViewModel;
    }
  }
}
