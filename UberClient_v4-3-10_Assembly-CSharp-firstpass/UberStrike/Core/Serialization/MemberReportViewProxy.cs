// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.MemberReportViewProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using Cmune.DataCenter.Common.Entities;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class MemberReportViewProxy
  {
    public static void Serialize(Stream stream, MemberReportView instance)
    {
      int num = 0;
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

    public static MemberReportView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      MemberReportView memberReportView = new MemberReportView();
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
      return memberReportView;
    }
  }
}
