
using System.IO;
using UberStrike.Core.Types;
using UberStrike.DataCenter.Common.Entities;

namespace UberStrike.Core.Serialization
{
  public static class ItemQuickUseConfigViewProxy
  {
    public static void Serialize(Stream stream, ItemQuickUseConfigView instance)
    {
      int num = 0;
      if (instance != null)
      {
        using (MemoryStream bytes = new MemoryStream())
        {
          EnumProxy<QuickItemLogic>.Serialize((Stream) bytes, instance.BehaviourType);
          Int32Proxy.Serialize((Stream) bytes, instance.CoolDownTime);
          Int32Proxy.Serialize((Stream) bytes, instance.ItemId);
          Int32Proxy.Serialize((Stream) bytes, instance.LevelRequired);
          Int32Proxy.Serialize((Stream) bytes, instance.UsesPerGame);
          Int32Proxy.Serialize((Stream) bytes, instance.UsesPerLife);
          Int32Proxy.Serialize((Stream) bytes, instance.UsesPerRound);
          Int32Proxy.Serialize((Stream) bytes, instance.WarmUpTime);
          Int32Proxy.Serialize(stream, ~num);
          bytes.WriteTo(stream);
        }
      }
      else
        Int32Proxy.Serialize(stream, 0);
    }

    public static ItemQuickUseConfigView Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      ItemQuickUseConfigView quickUseConfigView = (ItemQuickUseConfigView) null;
      if (num != 0)
      {
        quickUseConfigView = new ItemQuickUseConfigView();
        quickUseConfigView.BehaviourType = EnumProxy<QuickItemLogic>.Deserialize(bytes);
        quickUseConfigView.CoolDownTime = Int32Proxy.Deserialize(bytes);
        quickUseConfigView.ItemId = Int32Proxy.Deserialize(bytes);
        quickUseConfigView.LevelRequired = Int32Proxy.Deserialize(bytes);
        quickUseConfigView.UsesPerGame = Int32Proxy.Deserialize(bytes);
        quickUseConfigView.UsesPerLife = Int32Proxy.Deserialize(bytes);
        quickUseConfigView.UsesPerRound = Int32Proxy.Deserialize(bytes);
        quickUseConfigView.WarmUpTime = Int32Proxy.Deserialize(bytes);
      }
      return quickUseConfigView;
    }
  }
}
