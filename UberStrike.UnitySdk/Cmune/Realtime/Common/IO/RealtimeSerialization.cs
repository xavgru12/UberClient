
using Cmune.Util;
using System;
using System.Collections.Generic;

namespace Cmune.Realtime.Common.IO
{
  public class RealtimeSerialization
  {
    private static IByteConverter _converter;

    public static IByteConverter Converter
    {
      get
      {
        if (RealtimeSerialization._converter == null)
          RealtimeSerialization._converter = (IByteConverter) new DefaultByteConverter();
        return RealtimeSerialization._converter;
      }
      set => RealtimeSerialization._converter = value;
    }

    public static List<byte> ToBytes(params object[] args)
    {
      List<byte> bytes = new List<byte>();
      RealtimeSerialization.ToBytes(ref bytes, args);
      return bytes;
    }

    public static void ToBytes(ref List<byte> bytes, params object[] args)
    {
      foreach (object o in args)
        RealtimeSerialization.Converter.FromObject(o, ref bytes);
    }

    public static void ToBytes(bool encodeType, ref List<byte> bytes, params object[] args)
    {
      foreach (object o in args)
        RealtimeSerialization.Converter.FromObject(o, encodeType, ref bytes);
    }

    public static object[] ToObjects(byte[] bytes)
    {
      int i = 0;
      return RealtimeSerialization.ToObjects(bytes, ref i);
    }

    private static object[] ToObjects(byte[] bytes, ref int i)
    {
      List<object> objectList = new List<object>(5);
      int num = -1;
      while (i < bytes.Length - 1 && num != i)
      {
        num = i;
        object obj = RealtimeSerialization.Converter.ToObject(bytes, ref i);
        if (obj != null)
          objectList.Add(obj);
      }
      if (i == int.MaxValue)
        CmuneDebug.LogError("Error when deserializing Byte[] of Length {0}", (object) bytes.Length);
      else if (i == num)
      {
        CmuneDebug.LogError("Error when deserializing Byte[] of Length {0} because index didn't change at {1}", (object) bytes.Length, (object) num);
        i = int.MaxValue;
      }
      return objectList.ToArray();
    }

    public static bool TryToObjects(byte[] bytes, out object[] parameters)
    {
      int i = 0;
      parameters = RealtimeSerialization.ToObjects(bytes, ref i);
      return i != int.MaxValue;
    }

    public static object ToObject(byte[] bytes, ref int i) => RealtimeSerialization.Converter.ToObject(bytes, ref i);

    public static bool TryDecodeObject(byte[] bytes, ref int i, byte type, out object obj) => RealtimeSerialization.Converter.TryDecodeObject(bytes, type, ref i, out obj);

    public static object ToObject(byte[] bytes)
    {
      int i = 0;
      return RealtimeSerialization.Converter.ToObject(bytes, ref i);
    }

    public static bool IsTypeSupported(Type t) => RealtimeSerialization.Converter.IsTypeSupported(t);

    public static byte GetCmuneDataType(Type t) => RealtimeSerialization.Converter.GetCmuneDataType(t);
  }
}
