// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.PublicProfileViewProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using Cmune.DataCenter.Common.Entities;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class PublicProfileViewProxy
  {
    public static void Serialize(Stream stream, PublicProfileView instance)
    {
      int num = 0;
      using (MemoryStream bytes = new MemoryStream())
      {
        EnumProxy<MemberAccessLevel>.Serialize((Stream) bytes, instance.AccessLevel);
        Int32Proxy.Serialize((Stream) bytes, instance.Cmid);
        EnumProxy<EmailAddressStatus>.Serialize((Stream) bytes, instance.EmailAddressStatus);
        if (instance.FacebookId != null)
          StringProxy.Serialize((Stream) bytes, instance.FacebookId);
        else
          num |= 1;
        if (instance.GroupTag != null)
          StringProxy.Serialize((Stream) bytes, instance.GroupTag);
        else
          num |= 2;
        BooleanProxy.Serialize((Stream) bytes, instance.IsChatDisabled);
        DateTimeProxy.Serialize((Stream) bytes, instance.LastLoginDate);
        if (instance.Name != null)
          StringProxy.Serialize((Stream) bytes, instance.Name);
        else
          num |= 4;
        Int32Proxy.Serialize(stream, ~num);
        bytes.WriteTo(stream);
      }
    }

    public static PublicProfileView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      PublicProfileView publicProfileView = new PublicProfileView();
      publicProfileView.AccessLevel = EnumProxy<MemberAccessLevel>.Deserialize(bytes);
      publicProfileView.Cmid = Int32Proxy.Deserialize(bytes);
      publicProfileView.EmailAddressStatus = EnumProxy<EmailAddressStatus>.Deserialize(bytes);
      if ((num & 1) != 0)
        publicProfileView.FacebookId = StringProxy.Deserialize(bytes);
      if ((num & 2) != 0)
        publicProfileView.GroupTag = StringProxy.Deserialize(bytes);
      publicProfileView.IsChatDisabled = BooleanProxy.Deserialize(bytes);
      publicProfileView.LastLoginDate = DateTimeProxy.Deserialize(bytes);
      if ((num & 4) != 0)
        publicProfileView.Name = StringProxy.Deserialize(bytes);
      return publicProfileView;
    }
  }
}
