// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.GroupCreationViewProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using Cmune.DataCenter.Common.Entities;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class GroupCreationViewProxy
  {
    public static void Serialize(Stream stream, GroupCreationView instance)
    {
      int num = 0;
      using (MemoryStream bytes = new MemoryStream())
      {
        if (instance.Address != null)
          StringProxy.Serialize((Stream) bytes, instance.Address);
        else
          num |= 1;
        Int32Proxy.Serialize((Stream) bytes, instance.ApplicationId);
        if (instance.AuthToken != null)
          StringProxy.Serialize((Stream) bytes, instance.AuthToken);
        else
          num |= 2;
        if (instance.Description != null)
          StringProxy.Serialize((Stream) bytes, instance.Description);
        else
          num |= 4;
        BooleanProxy.Serialize((Stream) bytes, instance.HasPicture);
        if (instance.Locale != null)
          StringProxy.Serialize((Stream) bytes, instance.Locale);
        else
          num |= 8;
        if (instance.Motto != null)
          StringProxy.Serialize((Stream) bytes, instance.Motto);
        else
          num |= 16;
        if (instance.Name != null)
          StringProxy.Serialize((Stream) bytes, instance.Name);
        else
          num |= 32;
        if (instance.Tag != null)
          StringProxy.Serialize((Stream) bytes, instance.Tag);
        else
          num |= 64;
        Int32Proxy.Serialize(stream, ~num);
        bytes.WriteTo(stream);
      }
    }

    public static GroupCreationView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      GroupCreationView groupCreationView = new GroupCreationView();
      if ((num & 1) != 0)
        groupCreationView.Address = StringProxy.Deserialize(bytes);
      groupCreationView.ApplicationId = Int32Proxy.Deserialize(bytes);
      if ((num & 2) != 0)
        groupCreationView.AuthToken = StringProxy.Deserialize(bytes);
      if ((num & 4) != 0)
        groupCreationView.Description = StringProxy.Deserialize(bytes);
      groupCreationView.HasPicture = BooleanProxy.Deserialize(bytes);
      if ((num & 8) != 0)
        groupCreationView.Locale = StringProxy.Deserialize(bytes);
      if ((num & 16) != 0)
        groupCreationView.Motto = StringProxy.Deserialize(bytes);
      if ((num & 32) != 0)
        groupCreationView.Name = StringProxy.Deserialize(bytes);
      if ((num & 64) != 0)
        groupCreationView.Tag = StringProxy.Deserialize(bytes);
      return groupCreationView;
    }
  }
}
