
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
      if (instance != null)
      {
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
      else
        Int32Proxy.Serialize(stream, 0);
    }

    public static RegisterClientApplicationViewModel Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      RegisterClientApplicationViewModel applicationViewModel = (RegisterClientApplicationViewModel) null;
      if (num != 0)
      {
        applicationViewModel = new RegisterClientApplicationViewModel();
        if ((num & 1) != 0)
          applicationViewModel.ItemsAttributed = (ICollection<int>) ListProxy<int>.Deserialize(bytes, new ListProxy<int>.Deserializer<int>(Int32Proxy.Deserialize));
        applicationViewModel.Result = EnumProxy<ApplicationRegistrationResult>.Deserialize(bytes);
      }
      return applicationViewModel;
    }
  }
}
