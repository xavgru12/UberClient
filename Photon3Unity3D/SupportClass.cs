// Decompiled with JetBrains decompiler
// Type: ExitGames.Client.Photon.SupportClass
// Assembly: Photon3Unity3D, Version=3.0.1.11, Culture=neutral, PublicKeyToken=null
// MVID: 5A081D50-91FF-4A78-BF8D-1F77FAA7ECB2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Photon3Unity3D.dll

using System;
using System.Collections;
using System.IO;
using System.Text;

namespace ExitGames.Client.Photon
{
  public class SupportClass
  {
    public static void WriteStackTrace(Exception throwable, TextWriter stream)
    {
      stream.WriteLine(throwable.ToString());
      stream.WriteLine(throwable.StackTrace);
      stream.Flush();
    }

    public static string DictionaryToString(IDictionary dictionary) => SupportClass.DictionaryToString(dictionary, true);

    public static string DictionaryToString(IDictionary dictionary, bool includeTypes)
    {
      if (dictionary == null)
        return "null";
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("{");
      foreach (object key in (IEnumerable) dictionary.Keys)
      {
        if (stringBuilder.Length > 1)
          stringBuilder.Append(", ");
        Type o;
        string str;
        if (dictionary[key] == null)
        {
          o = typeof (object);
          str = "null";
        }
        else
        {
          o = dictionary[key].GetType();
          str = dictionary[key].ToString();
        }
        if (typeof (IDictionary).Equals(o) || typeof (Hashtable).Equals(o))
          str = SupportClass.DictionaryToString((IDictionary) dictionary[key]);
        if (typeof (string[]).Equals(o))
          str = string.Format("{{{0}}}", (object) string.Join(",", (string[]) dictionary[key]));
        if (includeTypes)
          stringBuilder.AppendFormat("({0}){1}=({2}){3}", (object) key.GetType().Name, key, (object) o.Name, (object) str);
        else
          stringBuilder.AppendFormat("{0}={1}", key, (object) str);
      }
      stringBuilder.Append("}");
      return stringBuilder.ToString();
    }

    [Obsolete("Use DictionaryToString() instead.")]
    public static string HashtableToString(Hashtable hash) => SupportClass.DictionaryToString((IDictionary) hash);

    public static void NumberToByteArray(byte[] buffer, int index, short number) => Protocol.Serialize(number, buffer, ref index);

    public static void NumberToByteArray(byte[] buffer, int index, int number) => Protocol.Serialize(number, buffer, ref index);

    public static string ByteArrayToString(byte[] list) => list == null ? string.Empty : BitConverter.ToString(list);

    public class ThreadSafeRandom
    {
      private static Random _r = new Random();

      public static int Next()
      {
        lock (SupportClass.ThreadSafeRandom._r)
          return SupportClass.ThreadSafeRandom._r.Next();
      }
    }
  }
}
