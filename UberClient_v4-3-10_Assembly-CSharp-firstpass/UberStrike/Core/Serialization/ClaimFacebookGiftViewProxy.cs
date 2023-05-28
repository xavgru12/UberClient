// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.ClaimFacebookGiftViewProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using Cmune.DataCenter.Common.Entities;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class ClaimFacebookGiftViewProxy
  {
    public static void Serialize(Stream stream, ClaimFacebookGiftView instance)
    {
      int num = 0;
      using (MemoryStream bytes = new MemoryStream())
      {
        EnumProxy<ClaimFacebookGiftResult>.Serialize((Stream) bytes, instance.ClaimResult);
        if (instance.ItemId.HasValue)
        {
          int? itemId = instance.ItemId;
          Int32Proxy.Serialize((Stream) bytes, itemId.HasValue ? itemId.Value : 0);
        }
        else
          num |= 1;
        Int32Proxy.Serialize(stream, ~num);
        bytes.WriteTo(stream);
      }
    }

    public static ClaimFacebookGiftView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      ClaimFacebookGiftView facebookGiftView = new ClaimFacebookGiftView();
      facebookGiftView.ClaimResult = EnumProxy<ClaimFacebookGiftResult>.Deserialize(bytes);
      if ((num & 1) != 0)
        facebookGiftView.ItemId = new int?(Int32Proxy.Deserialize(bytes));
      return facebookGiftView;
    }
  }
}
