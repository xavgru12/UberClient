// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.GroupCreationViewProxy
// Assembly: UberStrike.Core.Serialization, Version=1.0.2.98, Culture=neutral, PublicKeyToken=null
// MVID: 950E20E9-3609-4E9B-B4D8-B32B07AB805E
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Core.Serialization.dll

using Cmune.DataCenter.Common.Entities;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class GroupCreationViewProxy
  {
    public static void Serialize(Stream stream, GroupCreationView instance)
    {
      int num = 0;
      if (instance != null)
      {
        using (MemoryStream bytes = new MemoryStream())
        {
          if (instance.Address != null)
            StringProxy.Serialize((Stream) bytes, instance.Address);
          else
            num |= 1;
          Int32Proxy.Serialize((Stream) bytes, instance.ApplicationId);
          Int32Proxy.Serialize((Stream) bytes, instance.Cmid);
          if (instance.Description != null)
            StringProxy.Serialize((Stream) bytes, instance.Description);
          else
            num |= 2;
          BooleanProxy.Serialize((Stream) bytes, instance.HasPicture);
          if (instance.Locale != null)
            StringProxy.Serialize((Stream) bytes, instance.Locale);
          else
            num |= 4;
          if (instance.Motto != null)
            StringProxy.Serialize((Stream) bytes, instance.Motto);
          else
            num |= 8;
          if (instance.Name != null)
            StringProxy.Serialize((Stream) bytes, instance.Name);
          else
            num |= 16;
          if (instance.Tag != null)
            StringProxy.Serialize((Stream) bytes, instance.Tag);
          else
            num |= 32;
          Int32Proxy.Serialize(stream, ~num);
          bytes.WriteTo(stream);
        }
      }
      else
        Int32Proxy.Serialize(stream, 0);
    }

    public static GroupCreationView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      GroupCreationView groupCreationView = (GroupCreationView) null;
      if (num != 0)
      {
        groupCreationView = new GroupCreationView();
        if ((num & 1) != 0)
          groupCreationView.Address = StringProxy.Deserialize(bytes);
        groupCreationView.ApplicationId = Int32Proxy.Deserialize(bytes);
        groupCreationView.Cmid = Int32Proxy.Deserialize(bytes);
        if ((num & 2) != 0)
          groupCreationView.Description = StringProxy.Deserialize(bytes);
        groupCreationView.HasPicture = BooleanProxy.Deserialize(bytes);
        if ((num & 4) != 0)
          groupCreationView.Locale = StringProxy.Deserialize(bytes);
        if ((num & 8) != 0)
          groupCreationView.Motto = StringProxy.Deserialize(bytes);
        if ((num & 16) != 0)
          groupCreationView.Name = StringProxy.Deserialize(bytes);
        if ((num & 32) != 0)
          groupCreationView.Tag = StringProxy.Deserialize(bytes);
      }
      return groupCreationView;
    }
  }
}
