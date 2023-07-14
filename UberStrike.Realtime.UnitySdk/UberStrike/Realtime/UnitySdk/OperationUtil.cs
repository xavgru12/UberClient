// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.UnitySdk.OperationUtil
// Assembly: UberStrike.Realtime.UnitySdk, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA73603F-9C04-49D4-BBD8-49F06040C777
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Realtime.UnitySdk.dll

using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace UberStrike.Realtime.UnitySdk
{
  public static class OperationUtil
  {
    public static bool HasArg(IDictionary<byte, object> t, byte k) => t.ContainsKey(k);

    public static bool HasGeneralArg(IDictionary t, byte k) => t.Contains((object) k);

    public static T GetGeneralArg<T>(IDictionary t, byte k)
    {
      try
      {
        return (T) t[(object) k];
      }
      catch
      {
        throw CmuneDebug.Exception("Error in OperationUtils.GetGeneralArg(<{2}>)! Key {0}({1}) not found in Table or expecting wrong Type!", (object) k, (object) k, (object) typeof (T));
      }
    }

    public static T GetArg<T>(IDictionary<byte, object> t, byte k)
    {
      try
      {
        return (T) t[k];
      }
      catch
      {
        throw CmuneDebug.Exception("Error in OperationUtils.GetArg(<{2}>)! Key {0}({1}) not found in Table or expecting wrong Type!", (object) k, (object) k, (object) typeof (T));
      }
    }

    public static int GetActor(IDictionary<byte, object> t) => OperationUtil.GetArg<int>(t, (byte) 9);

    public static Hashtable GetData(IDictionary<byte, object> t) => OperationUtil.GetArg<Hashtable>(t, (byte) 42);

    public static byte[] GetBytes(IDictionary<byte, object> t) => OperationUtil.GetArg<byte[]>(t, (byte) 103);

    public static void SetArg<T>(IDictionary<byte, object> t, byte k, object arg)
    {
      try
      {
        t[k] = (object) (T) arg;
      }
      catch
      {
        if (arg != null)
          throw CmuneDebug.Exception("Error in OperationUtils.SetArg()! Key {0}({1}) is expecting value of Type {2} but found {3}", (object) k, (object) k, (object) typeof (T), (object) arg.GetType());
        throw CmuneDebug.Exception("Error in OperationUtils.SetArg()! Key {0}({1}) is expecting value of Type {2} but found NULL", (object) k, (object) k, (object) typeof (T));
      }
    }

    public static void SetMethodId(IDictionary<byte, object> t, object arg) => OperationUtil.SetArg<byte>(t, (byte) 100, arg);

    public static void SetInstanceID(IDictionary<byte, object> t, object arg) => OperationUtil.SetArg<short>(t, (byte) 101, arg);

    public static void SetBytes(IDictionary<byte, object> t, object arg) => OperationUtil.SetArg<byte[]>(t, (byte) 103, arg);

    public static string PrintHashtable(IDictionary t)
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (t != null)
      {
        foreach (DictionaryEntry dictionaryEntry in t)
          stringBuilder.AppendFormat("[{0}]: {1} of Type {2}\n", dictionaryEntry.Key, (object) CmunePrint.Values(dictionaryEntry.Value), (object) CmunePrint.Types(dictionaryEntry.Value));
      }
      else
        stringBuilder.Append("NULL");
      return stringBuilder.ToString();
    }
  }
}
