
using System.IO;
using UberStrike.DataCenter.Common.Entities;

namespace UberStrike.Core.Serialization
{
  public static class DailyPointsViewProxy
  {
    public static void Serialize(Stream stream, DailyPointsView instance)
    {
      int num = 0;
      if (instance != null)
      {
        using (MemoryStream bytes = new MemoryStream())
        {
          Int32Proxy.Serialize((Stream) bytes, instance.Current);
          Int32Proxy.Serialize((Stream) bytes, instance.PointsMax);
          Int32Proxy.Serialize((Stream) bytes, instance.PointsTomorrow);
          Int32Proxy.Serialize(stream, ~num);
          bytes.WriteTo(stream);
        }
      }
      else
        Int32Proxy.Serialize(stream, 0);
    }

    public static DailyPointsView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      DailyPointsView dailyPointsView = (DailyPointsView) null;
      if (num != 0)
      {
        dailyPointsView = new DailyPointsView();
        dailyPointsView.Current = Int32Proxy.Deserialize(bytes);
        dailyPointsView.PointsMax = Int32Proxy.Deserialize(bytes);
        dailyPointsView.PointsTomorrow = Int32Proxy.Deserialize(bytes);
      }
      return dailyPointsView;
    }
  }
}
