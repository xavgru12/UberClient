// Decompiled with JetBrains decompiler
// Type: Cmune.Realtime.Common.IO.StringConversion
// Assembly: UberStrike.UnitySdk, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 5841A2D1-61BC-4235-BEF7-EC54B624B7CE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike4-3-9\UberStrike_Data\Managed\UberStrike.UnitySdk.dll

using Cmune.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cmune.Realtime.Common.IO
{
  public class StringConversion
  {
    public static string ToString(byte[] bytes)
    {
      int length = bytes.Length;
      StringBuilder stringBuilder = new StringBuilder();
      int i = 0;
      while (i < length)
        stringBuilder.Append(DefaultByteConverter.ToChar(bytes, ref i));
      return stringBuilder.ToString();
    }

    public static byte[] FromString(string s)
    {
      char[] charArray = s.ToCharArray();
      List<byte> bytes = new List<byte>(charArray.Length * 2);
      foreach (char c in charArray)
        DefaultByteConverter.FromChar(c, ref bytes);
      return bytes.ToArray();
    }

    public static string Base64Decode(string data)
    {
      try
      {
        return Encoding.UTF8.GetString(Convert.FromBase64String(data));
      }
      catch (Exception ex)
      {
        CmuneDebug.LogError("Error in base64Decode decoding:" + data + "\n" + ex.Message, new object[0]);
        throw new Exception("Error in base64Decode: " + ex.Message, ex);
      }
    }

    public static string Base64Encode(string data)
    {
      try
      {
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(data));
      }
      catch (Exception ex)
      {
        CmuneDebug.LogError("Error in base64Encode" + ex.Message, new object[0]);
        throw new Exception("Error in base64Decode: " + ex.Message, ex);
      }
    }

    public static string Replace(string expr, string find, string repl) => !string.Equals(expr, find, StringComparison.CurrentCultureIgnoreCase) ? expr : repl;

    public static string Replace(string expr, string find, string repl, bool bIgnoreCase)
    {
      int length1 = expr.Length;
      int length2 = find.Length;
      if (length1 == 0 || length2 == 0 || length2 > length1)
        return expr;
      if (!bIgnoreCase)
        return expr.Replace(find, repl);
      StringBuilder stringBuilder = new StringBuilder(length1);
      int num = 0;
      while (num + length2 <= length1)
      {
        if (string.Compare(expr, num, find, 0, length2, bIgnoreCase) == 0)
        {
          stringBuilder.Append(repl);
          num += length2;
        }
        else
          stringBuilder.Append(expr, num++, 1);
      }
      stringBuilder.Append(expr, num, length1 - num);
      return stringBuilder.ToString();
    }
  }
}
