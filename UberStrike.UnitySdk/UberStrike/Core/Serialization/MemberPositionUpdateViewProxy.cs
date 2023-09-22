
using Cmune.DataCenter.Common.Entities;
using System.IO;

namespace UberStrike.Core.Serialization
{
  public static class MemberPositionUpdateViewProxy
  {
    public static void Serialize(Stream stream, MemberPositionUpdateView instance)
    {
      int num = 0;
      if (instance != null)
      {
        using (MemoryStream bytes = new MemoryStream())
        {
          Int32Proxy.Serialize((Stream) bytes, instance.CmidMakingAction);
          Int32Proxy.Serialize((Stream) bytes, instance.GroupId);
          Int32Proxy.Serialize((Stream) bytes, instance.MemberCmid);
          EnumProxy<GroupPosition>.Serialize((Stream) bytes, instance.Position);
          Int32Proxy.Serialize(stream, ~num);
          bytes.WriteTo(stream);
        }
      }
      else
        Int32Proxy.Serialize(stream, 0);
    }

    public static MemberPositionUpdateView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      MemberPositionUpdateView positionUpdateView = (MemberPositionUpdateView) null;
      if (num != 0)
      {
        positionUpdateView = new MemberPositionUpdateView();
        positionUpdateView.CmidMakingAction = Int32Proxy.Deserialize(bytes);
        positionUpdateView.GroupId = Int32Proxy.Deserialize(bytes);
        positionUpdateView.MemberCmid = Int32Proxy.Deserialize(bytes);
        positionUpdateView.Position = EnumProxy<GroupPosition>.Deserialize(bytes);
      }
      return positionUpdateView;
    }
  }
}
