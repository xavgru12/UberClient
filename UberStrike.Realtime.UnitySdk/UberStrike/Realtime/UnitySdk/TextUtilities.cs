// Decompiled with JetBrains decompiler
// Type: UberStrike.Realtime.UnitySdk.TextUtilities
// Assembly: UberStrike.Realtime.UnitySdk, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AA73603F-9C04-49D4-BBD8-49F06040C777
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\UberStrike.Realtime.UnitySdk.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace UberStrike.Realtime.UnitySdk
{
  public static class TextUtilities
  {
    public static char[] TrimCharacters = new char[3]
    {
      ' ',
      '\n',
      '\t'
    };

    public static string ConvertText(string textToSecure) => TextUtilities.HtmlEncode(textToSecure).Replace("`", "&#96;").Replace("´", "&acute").Replace("'", "&#39").Replace("-", "&#45;").Replace("!", "&#33;").Replace("?", "&#63;");

    public static string HtmlEncode(string text)
    {
      if (text == null)
        return (string) null;
      StringBuilder stringBuilder = new StringBuilder(text.Length);
      int length = text.Length;
      for (int index = 0; index < length; ++index)
      {
        switch (text[index])
        {
          case '"':
            stringBuilder.Append("&quot;");
            break;
          case '&':
            stringBuilder.Append("&amp;");
            break;
          case '<':
            stringBuilder.Append("&lt;");
            break;
          case '>':
            stringBuilder.Append("&gt;");
            break;
          default:
            if (text[index] > '\u009F')
            {
              stringBuilder.Append("&#");
              stringBuilder.Append(((int) text[index]).ToString((IFormatProvider) CultureInfo.InvariantCulture));
              stringBuilder.Append(";");
              break;
            }
            stringBuilder.Append(text[index]);
            break;
        }
      }
      return stringBuilder.ToString();
    }

    public static string ProtectSqlField(string textToSecure) => textToSecure.Replace("'", "''");

    public static string ConvertTextForJavaScript(string textToSecure) => textToSecure.Replace("'", string.Empty).Replace("|", string.Empty);

    public static long InetAToN(string addressIP)
    {
      long num1 = 0;
      if (addressIP.Equals("::1"))
        addressIP = "127.0.0.1";
      if (!TextUtilities.IsNullOrEmpty(addressIP))
      {
        string[] strArray = addressIP.ToString().Split('.');
        if (strArray.Length == 4)
        {
          bool flag = true;
          int result = 0;
          long num2 = 0;
          for (int index = strArray.Length - 1; index >= 0; --index)
          {
            if (int.TryParse(strArray[index], out result) && result >= 0 && result < 256)
              num2 += (long) (result % 256) * (long) Math.Pow(256.0, (double) (3 - index));
            else
              flag = false;
          }
          if (flag)
            num1 = num2;
        }
      }
      return num1;
    }

    public static string InetNToA(long networkAddress)
    {
      string str = string.Empty;
      if (networkAddress <= (long) uint.MaxValue)
      {
        long num1 = networkAddress / 16777216L;
        if (num1 == 0L)
        {
          num1 = (long) byte.MaxValue;
          networkAddress += 16777216L;
        }
        else if (num1 < 0L)
        {
          if (networkAddress % 16777216L == 0L)
          {
            num1 += 256L;
          }
          else
          {
            num1 += (long) byte.MaxValue;
            if (num1 == 128L)
              networkAddress += 2147483648L;
            else
              networkAddress += 16777216L * (256L - num1);
          }
        }
        else
          networkAddress -= 16777216L * num1;
        networkAddress %= 16777216L;
        long num2 = networkAddress / 65536L;
        networkAddress %= 65536L;
        long num3 = networkAddress / 256L;
        networkAddress %= 256L;
        long num4 = networkAddress;
        str = num1.ToString() + "." + num2.ToString() + "." + num3.ToString() + "." + num4.ToString();
      }
      return str;
    }

    public static bool IsNumeric(string numericText)
    {
      bool flag = true;
      if (!TextUtilities.IsNullOrEmpty(numericText))
      {
        if (numericText.StartsWith("-"))
          numericText = numericText.Replace("-", string.Empty);
        foreach (char c in numericText)
        {
          flag = char.IsNumber(c);
          if (!flag)
            return flag;
        }
      }
      else
        flag = false;
      return flag;
    }

    public static string ShortenText(string input, int maxSize, bool addPoints)
    {
      string str = input;
      if (maxSize < input.Length)
      {
        if (addPoints)
          maxSize -= 3;
        if (maxSize > 0)
        {
          str = str.Substring(0, maxSize);
          if (addPoints)
            str += "...";
        }
      }
      return str;
    }

    public static bool IsNullOrEmpty(string value) => TextUtilities.Trim(value).Length == 0;

    public static string Trim(string value) => !string.IsNullOrEmpty(value) ? value.Replace('\n', ' ').Replace('\t', ' ').Trim(TextUtilities.TrimCharacters) : string.Empty;

    public static List<int> IndexOfAll(string haystack, string needle)
    {
      int num1 = 0;
      List<int> intList = new List<int>();
      if (!TextUtilities.IsNullOrEmpty(haystack) && !TextUtilities.IsNullOrEmpty(needle))
      {
        int length = needle.Length;
        int num2;
        do
        {
          num2 = haystack.IndexOf(needle);
          if (num2 > -1)
          {
            haystack = haystack.Substring(num2 + length);
            intList.Add(num2 + num1);
            num1 += num2 + length;
          }
        }
        while (num2 > -1 && !TextUtilities.IsNullOrEmpty(haystack));
      }
      return intList;
    }

    public static string Base64Encode(string data)
    {
      string str = string.Empty;
      if (data != null)
      {
        byte[] numArray = new byte[data.Length];
        str = Convert.ToBase64String(Encoding.UTF8.GetBytes(data));
      }
      return str;
    }

    public static string Base64Decode(string data)
    {
      string str = string.Empty;
      if (data != null)
      {
        Decoder decoder = new UTF8Encoding().GetDecoder();
        byte[] bytes = Convert.FromBase64String(data);
        char[] chars = new char[decoder.GetCharCount(bytes, 0, bytes.Length)];
        decoder.GetChars(bytes, 0, bytes.Length, chars, 0);
        str = new string(chars);
      }
      return str;
    }

    public static byte[] StringToByteArray(string inputString) => new UTF8Encoding().GetBytes(inputString);

    public static string CompleteTrim(string text)
    {
      if (text != null)
      {
        text = text.Trim();
        text = Regex.Replace(text, "\\s+", " ");
      }
      return text;
    }

    public static bool TryParseFacebookId(string handle, out long facebookId)
    {
      bool facebookId1 = false;
      facebookId = 0L;
      if (long.TryParse(handle, out facebookId) && facebookId > 0L)
        facebookId1 = true;
      return facebookId1;
    }

    public static bool TryParseMySpaceId(string handle, out int mySpaceId)
    {
      bool mySpaceId1 = false;
      mySpaceId = 0;
      if (int.TryParse(handle, out mySpaceId) && mySpaceId > 0)
        mySpaceId1 = true;
      return mySpaceId1;
    }

    public static bool TryParseCyworldId(string handle, out int cyworldId)
    {
      bool cyworldId1 = false;
      cyworldId = 0;
      if (int.TryParse(handle, out cyworldId) && cyworldId > 0)
        cyworldId1 = true;
      return cyworldId1;
    }

    public static string Join<T>(string separator, List<T> list)
    {
      string str = string.Empty;
      if (list != null && list.Count > 0)
      {
        string[] strArray = new string[list.Count];
        for (int index = 0; index < list.Count; ++index)
          strArray[index] = list[index].ToString();
        str = string.Join(separator, strArray);
      }
      return str;
    }
  }
}
