
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
