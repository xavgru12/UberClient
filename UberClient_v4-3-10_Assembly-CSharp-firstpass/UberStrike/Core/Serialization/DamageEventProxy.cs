// Decompiled with JetBrains decompiler
// Type: UberStrike.Core.Serialization.DamageEventProxy
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System.IO;
using UberStrike.Core.Models;

namespace UberStrike.Core.Serialization
{
  public static class DamageEventProxy
  {
    public static void Serialize(Stream stream, DamageEvent instance)
    {
      int num = 0;
      using (MemoryStream bytes = new MemoryStream())
      {
        ByteProxy.Serialize((Stream) bytes, instance.BodyPartFlag);
        if (instance.Damage != null)
          DictionaryProxy<byte, byte>.Serialize((Stream) bytes, instance.Damage, new DictionaryProxy<byte, byte>.Serializer<byte>(ByteProxy.Serialize), new DictionaryProxy<byte, byte>.Serializer<byte>(ByteProxy.Serialize));
        else
          num |= 1;
        Int32Proxy.Serialize((Stream) bytes, instance.DamageEffectFlag);
        SingleProxy.Serialize((Stream) bytes, instance.DamgeEffectValue);
        Int32Proxy.Serialize(stream, ~num);
        bytes.WriteTo(stream);
      }
    }

    public static DamageEvent Deserialize(Stream bytes)
    {
      int num = Int32Proxy.Deserialize(bytes);
      DamageEvent damageEvent = new DamageEvent();
      damageEvent.BodyPartFlag = ByteProxy.Deserialize(bytes);
      if ((num & 1) != 0)
        damageEvent.Damage = DictionaryProxy<byte, byte>.Deserialize(bytes, new DictionaryProxy<byte, byte>.Deserializer<byte>(ByteProxy.Deserialize), new DictionaryProxy<byte, byte>.Deserializer<byte>(ByteProxy.Deserialize));
      damageEvent.DamageEffectFlag = Int32Proxy.Deserialize(bytes);
      damageEvent.DamgeEffectValue = SingleProxy.Deserialize(bytes);
      return damageEvent;
    }
  }
}
