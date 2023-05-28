// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.RegisterClientApplicationViewModelProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using Cmune.DataCenter.Common.Entities;
using System.Collections.Generic;
using System.IO;
using UberStrike.Core.ViewModel;

namespace UberStrike.Core.Serialization
{
  public static class RegisterClientApplicationViewModelProxy
  {
    public static void Serialize(Stream stream, RegisterClientApplicationViewModel instance)
    {
      int num = 0;
      using (MemoryStream bytes = new MemoryStream())
      {
        if (instance.ItemsAttributed != null)
          ListProxy<int>.Serialize((Stream) bytes, instance.ItemsAttributed, new ListProxy<int>.Serializer<int>(Int32Proxy.Serialize));
        else
          num |= 1;
        EnumProxy<ApplicationRegistrationResult>.Serialize((Stream) bytes, instance.Result);
        Int32Proxy.Serialize(stream, ~num);
        bytes.WriteTo(stream);
      }
    }

    public static RegisterClientApplicationViewModel Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      RegisterClientApplicationViewModel applicationViewModel = new RegisterClientApplicationViewModel();
      if ((num & 1) != 0)
        applicationViewModel.ItemsAttributed = (ICollection<int>) ListProxy<int>.Deserialize(bytes, new ListProxy<int>.Deserializer<int>(Int32Proxy.Deserialize));
      applicationViewModel.Result = EnumProxy<ApplicationRegistrationResult>.Deserialize(bytes);
      return applicationViewModel;
    }
  }
}
