// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.UnitySdk.CmunePrint
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9E9FF056-398A-44CC-B2A3-2C99E6116567
// Assembly location: C:\Users\Xaver\Documents\Uber\live_versions\uberkill\client_13-38-94-69\UberStrike_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace UberStrike.Realtime.UnitySdk
{
  public static class CmunePrint
  {
    private static readonly byte _byteBitCountConstant = 7;
    private static readonly byte _byteBitMaskConstant = 128;

    public static string Properties(object instance, bool publicOnly = true)
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (instance == null)
      {
        stringBuilder.Append("[Class=null]");
      }
      else
      {
        stringBuilder.AppendFormat("[Class={0}] ", (object) instance.GetType().Name);
        foreach (PropertyInfo property in instance.GetType().GetProperties(!publicOnly ? BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic : BindingFlags.Instance | BindingFlags.Public))
          stringBuilder.AppendFormat("[{0}={1}],", (object) property.Name, (object) CmunePrint.Object(property.GetValue(instance, (object[]) null)));
      }
      return stringBuilder.ToString();
    }

    public static string Object(object value)
    {
      if (value == null)
        return "null";
      if (value is string)
        return value as string;
      if (value.GetType().IsValueType || !(value is ICollection))
        return value.ToString();
      return CmunePrint.Values(value);
    }

    public static int GetHashCode(object obj)
    {
      if (obj == null)
        return 0;
      if (!(obj is ICollection))
        return obj.GetHashCode();
      int hashCode = 0;
      foreach (object obj1 in (IEnumerable) (obj as ICollection))
        hashCode += obj1.GetHashCode();
      return hashCode;
    }

    public static string Percent(float f) => string.Format("{0:N0}%", (object) Math.Round((double) f * 100.0));

    public static string Order(int time)
    {
      if (time <= 0)
        return time.ToString();
      switch (time)
      {
        case 1:
          return "1st";
        case 2:
          return "2nd";
        case 3:
          return "3rd";
        default:
          return time.ToString() + "th";
      }
    }

    public static string Time(DateTime time) => time.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss.fffffffK");

    public static string Time(TimeSpan s)
    {
      if (s.Days > 0)
        return string.Format("{0:D1}d, {1:D2}:{2:D2}h", (object) s.Days, (object) s.Hours, (object) s.Minutes);
      if (s.Hours > 0)
        return string.Format("{0:D2}:{1:D2}:{2:D2}", (object) s.Hours, (object) s.Minutes, (object) s.Seconds);
      if (s.Minutes > 0)
        return string.Format("{0:D2}:{1:D2}", (object) s.Minutes, (object) s.Seconds);
      return s.Seconds > 10 ? string.Format("{0:D2}", (object) s.Seconds) : string.Format("{0:D1}", (object) s.Seconds);
    }

    public static string Time(int seconds) => CmunePrint.Time(TimeSpan.FromSeconds((double) Math.Max(seconds, 0)));

    public static string Flag(sbyte flag) => CmunePrint.Flag((uint) flag, 7);

    public static string Flag(byte flag) => CmunePrint.Flag((uint) flag, 7);

    public static string Flag(ushort flag) => CmunePrint.Flag((uint) flag, 15);

    public static string Flag(short flag) => CmunePrint.Flag((uint) flag, 15);

    public static string Flag(int flag) => CmunePrint.Flag((uint) flag, 31);

    public static string Flag(uint flag) => CmunePrint.Flag(flag, 31);

    public static string Flag<T>(T flag) where T : IConvertible => typeof (T).IsEnum ? CmunePrint.Flag(Convert.ToUInt32((object) flag), typeof (T)) : CmunePrint.Flag(Convert.ToUInt32((object) flag), 31);

    private static string Flag(uint flag, int bytes)
    {
      int num = 1 << bytes;
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = bytes; index >= 0; --index)
      {
        stringBuilder.Append(((long) flag & (long) num) != 0L ? '1' : '0');
        if (index % 8 == 0)
          stringBuilder.Append(' ');
        flag <<= 1;
      }
      return stringBuilder.ToString();
    }

    private static string Flag(uint flag, System.Type type)
    {
      System.Type underlyingType = Enum.GetUnderlyingType(type);
      try
      {
        int num1 = 31;
        if (underlyingType == typeof (byte) || underlyingType == typeof (sbyte))
          num1 = 7;
        else if (underlyingType == typeof (short) || underlyingType == typeof (ushort))
          num1 = 15;
        int num2 = 1 << num1;
        StringBuilder stringBuilder = new StringBuilder();
        for (int index = num1; index >= 0; --index)
        {
          if (underlyingType == typeof (byte))
          {
            if (((long) flag & (long) num2) != 0L && Enum.IsDefined(type, (object) (byte) (1 << index)))
              stringBuilder.Append(Enum.GetName(type, (object) (1 << index)) + " ");
          }
          else if (underlyingType == typeof (ushort))
          {
            if (((long) flag & (long) num2) != 0L && Enum.IsDefined(type, (object) (ushort) (1 << index)))
              stringBuilder.Append(Enum.GetName(type, (object) (1 << index)) + " ");
          }
          else if (((long) flag & (long) num2) != 0L && Enum.IsDefined(type, (object) (1 << index)))
            stringBuilder.Append(Enum.GetName(type, (object) (1 << index)) + " ");
          flag <<= 1;
        }
        return stringBuilder.ToString();
      }
      catch
      {
        return type.Name + " unsupported: " + underlyingType?.ToString();
      }
    }

    public static string Values(params object[] args)
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (args != null)
      {
        if (args.Length == 0)
        {
          stringBuilder.Append("EMPTY");
        }
        else
        {
          for (int index = 0; index < args.Length; ++index)
          {
            object obj = args[index];
            if (obj != null)
            {
              if (obj is IEnumerable)
              {
                IEnumerable enumerable = obj as IEnumerable;
                stringBuilder.Append("|");
                IEnumerator enumerator = enumerable.GetEnumerator();
                int num;
                for (num = 0; enumerator.MoveNext() && num < 50; ++num)
                {
                  if (enumerator.Current != null)
                    stringBuilder.AppendFormat("{0}|", enumerator.Current);
                  else
                    stringBuilder.Append("null|");
                }
                switch (num)
                {
                  case 0:
                    stringBuilder.Append("empty|");
                    break;
                  case 50:
                    stringBuilder.Append("...");
                    break;
                }
              }
              else
                stringBuilder.AppendFormat("{0}", obj);
            }
            else
              stringBuilder.AppendFormat("null");
            if (index < args.Length - 1)
              stringBuilder.AppendFormat(", ");
          }
        }
      }
      else
        stringBuilder.Append("NULL");
      return stringBuilder.ToString();
    }

    public static string Types(params object[] args)
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (args != null)
      {
        if (args.Length == 0)
        {
          stringBuilder.Append("EMPTY");
        }
        else
        {
          for (int index = 0; index < args.Length; ++index)
          {
            object obj = args[index];
            if (obj != null)
            {
              if (obj is ICollection)
              {
                ICollection collection = obj as ICollection;
                stringBuilder.AppendFormat("{0}({1})", (object) collection.GetType().Name, (object) collection.Count);
              }
              else
                stringBuilder.AppendFormat("{0}", (object) obj.GetType().Name);
            }
            else
              stringBuilder.AppendFormat("null");
            if (index < args.Length - 1)
              stringBuilder.AppendFormat(", ");
          }
        }
      }
      else
        stringBuilder.Append("NULL");
      return stringBuilder.ToString();
    }

    public static string Dictionary(IDictionary t)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (DictionaryEntry dictionaryEntry in t)
        stringBuilder.AppendFormat("{0}: {1}\n", dictionaryEntry.Key, dictionaryEntry.Value);
      return stringBuilder.ToString();
    }

    public static void DebugBitString(byte[] x) => Debug.Log((object) CmunePrint.BitString(x));

    public static void DebugBitString(int x) => Debug.Log((object) CmunePrint.BitString(x));

    public static void DebugBitString(string x) => Debug.Log((object) CmunePrint.BitString(x));

    public static void DebugBitString(byte x) => Debug.Log((object) CmunePrint.BitString(x));

    public static string BitString(byte x)
    {
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index <= (int) CmunePrint._byteBitCountConstant; ++index)
      {
        stringBuilder.Append(((int) x & (int) CmunePrint._byteBitMaskConstant) != 0 ? '1' : '0');
        x <<= 1;
      }
      return stringBuilder.ToString();
    }

    public static string BitString(int x) => CmunePrint.BitString(BitConverter.GetBytes(x));

    public static string BitString(string x) => CmunePrint.BitString(Encoding.Unicode.GetBytes(x));

    public static string BitString(byte[] bytes)
    {
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = bytes.Length - 1; index >= 0; --index)
        stringBuilder.Append(CmunePrint.BitString(bytes[index])).Append(' ');
      return stringBuilder.ToString();
    }
  }
}
