// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.ItemQuickUseConfigViewProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System.IO;
using UberStrike.Core.Types;
using UberStrike.DataCenter.Common.Entities;

namespace UberStrike.Core.Serialization
{
  public static class ItemQuickUseConfigViewProxy
  {
    public static void Serialize(Stream stream, ItemQuickUseConfigView instance)
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
        bytes.WriteTo(stream);
      }
    }

    public static ItemQuickUseConfigView Deserialize(Stream bytes) => new ItemQuickUseConfigView()
    {
      BehaviourType = EnumProxy<QuickItemLogic>.Deserialize(bytes),
      CoolDownTime = Int32Proxy.Deserialize(bytes),
      ItemId = Int32Proxy.Deserialize(bytes),
      LevelRequired = Int32Proxy.Deserialize(bytes),
      UsesPerGame = Int32Proxy.Deserialize(bytes),
      UsesPerLife = Int32Proxy.Deserialize(bytes),
      UsesPerRound = Int32Proxy.Deserialize(bytes),
      WarmUpTime = Int32Proxy.Deserialize(bytes)
    };
  }
}
