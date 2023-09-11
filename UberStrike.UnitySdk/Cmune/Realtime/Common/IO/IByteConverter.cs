// Decompiled with JetBrains decompiler
// Type: Cmune.Realtime.Common.IO.IByteConverter
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using System;
using System.Collections.Generic;

namespace Cmune.Realtime.Common.IO
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
