// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.MemberReportViewProxy
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using Cmune.DataCenter.Common.Entities;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class MemberReportViewProxy
  {
    public static void Serialize(Stream stream, MemberReportView instance)
    {
      int num = 0;
      if (instance != null)
      {
        using (MemoryStream bytes = new MemoryStream())
        {
          Int32Proxy.Serialize((Stream) bytes, instance.ApplicationId);
          if (instance.Context != null)
            StringProxy.Serialize((Stream) bytes, instance.Context);
          else
            num |= 1;
          if (instance.IP != null)
            StringProxy.Serialize((Stream) bytes, instance.IP);
          else
            num |= 2;
          if (instance.Reason != null)
            StringProxy.Serialize((Stream) bytes, instance.Reason);
          else
            num |= 4;
          EnumProxy<MemberReportType>.Serialize((Stream) bytes, instance.ReportType);
          Int32Proxy.Serialize((Stream) bytes, instance.SourceCmid);
          Int32Proxy.Serialize((Stream) bytes, instance.TargetCmid);
          Int32Proxy.Serialize(stream, ~num);
          bytes.WriteTo(stream);
        }
      }
      else
        Int32Proxy.Serialize(stream, 0);
    }

    public static MemberReportView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      MemberReportView memberReportView = (MemberReportView) null;
      if (num != 0)
      {
        memberReportView = new MemberReportView();
        memberReportView.ApplicationId = Int32Proxy.Deserialize(bytes);
        if ((num & 1) != 0)
          memberReportView.Context = StringProxy.Deserialize(bytes);
        if ((num & 2) != 0)
          memberReportView.IP = StringProxy.Deserialize(bytes);
        if ((num & 4) != 0)
          memberReportView.Reason = StringProxy.Deserialize(bytes);
        memberReportView.ReportType = EnumProxy<MemberReportType>.Deserialize(bytes);
        memberReportView.SourceCmid = Int32Proxy.Deserialize(bytes);
        memberReportView.TargetCmid = Int32Proxy.Deserialize(bytes);
      }
      return memberReportView;
    }
  }
}
