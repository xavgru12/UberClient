// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.UnitySdk.IByteConverter
// Assembly: UberStrike.Realtime.UnitySdk, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA73603F-9C04-49D4-BBD8-49F06040C777
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Realtime.UnitySdk.dll

using System;
using System.Collections.Generic;

namespace UberStrike.Realtime.UnitySdk
{
  public interface IByteConverter
  {
    bool IsTypeSupported(Type t);

    byte GetCmuneDataType(Type t);

    bool TrySerializeObject<T>(T o, bool encodeType, ref List<byte> bytes);

    void FromObject(object o, ref List<byte> bytes);

    bool FromObject(object o, bool encodeType, ref List<byte> bytes);

    bool TryDecodeObject(byte[] bytes, Type type, ref int i, out object obj);

    bool TryDecodeObject(byte[] bytes, byte type, ref int i, out object obj);

    object ToObject(byte[] bytes, ref int i);
  }
}
