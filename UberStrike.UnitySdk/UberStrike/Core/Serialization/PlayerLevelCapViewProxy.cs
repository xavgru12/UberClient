
using System.IO;
using UberStrike.DataCenter.Common.Entities;

namespace UberStrike.Core.Serialization
{
  public static class PlayerLevelCapViewProxy
  {
    public static void Serialize(Stream stream, PlayerLevelCapView instance)
    {
      int num = 0;
      if (instance != null)
      {
        using (MemoryStream bytes = new MemoryStream())
        {
          Int32Proxy.Serialize((Stream) bytes, instance.Level);
          Int32Proxy.Serialize((Stream) bytes, instance.PlayerLevelCapId);
          Int32Proxy.Serialize((Stream) bytes, instance.XPRequired);
          Int32Proxy.Serialize(stream, ~num);
          bytes.WriteTo(stream);
        }
      }
      else
        Int32Proxy.Serialize(stream, 0);
    }

    public static PlayerLevelCapView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      PlayerLevelCapView playerLevelCapView = (PlayerLevelCapView) null;
      if (num != 0)
      {
        playerLevelCapView = new PlayerLevelCapView();
        playerLevelCapView.Level = Int32Proxy.Deserialize(bytes);
        playerLevelCapView.PlayerLevelCapId = Int32Proxy.Deserialize(bytes);
        playerLevelCapView.XPRequired = Int32Proxy.Deserialize(bytes);
      }
      return playerLevelCapView;
    }
  }
}
