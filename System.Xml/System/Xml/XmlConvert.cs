// Decompiled with JetBrains decompiler
// Type: System.Xml.XmlConvert
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace System.Xml
{
  public class XmlConvert
  {
    private const string encodedColon = "_x003A_";
    private const NumberStyles floatStyle = NumberStyles.Float | NumberStyles.AllowCurrencySymbol;
    private const NumberStyles integerStyle = NumberStyles.Integer;
    private static readonly string[] datetimeFormats = new string[27]
    {
      "yyyy-MM-ddTHH:mm:sszzz",
      "yyyy-MM-ddTHH:mm:ss.FFFFFFFzzz",
      "yyyy-MM-ddTHH:mm:ssZ",
      "yyyy-MM-ddTHH:mm:ss.FFFFFFFZ",
      "yyyy-MM-ddTHH:mm:ss",
      "yyyy-MM-ddTHH:mm:ss.FFFFFFF",
      "HH:mm:ss",
      "HH:mm:ss.FFFFFFF",
      "HH:mm:sszzz",
      "HH:mm:ss.FFFFFFFzzz",
      "HH:mm:ssZ",
      "HH:mm:ss.FFFFFFFZ",
      "yyyy-MM-dd",
      "yyyy-MM-ddzzz",
      "yyyy-MM-ddZ",
      "yyyy-MM",
      "yyyy-MMzzz",
      "yyyy-MMZ",
      "yyyy",
      "yyyyzzz",
      "yyyyZ",
      "--MM-dd",
      "--MM-ddzzz",
      "--MM-ddZ",
      "---dd",
      "---ddzzz",
      "---ddZ"
    };
    private static readonly string[] defaultDateTimeFormats = new string[8]
    {
      "yyyy-MM-ddTHH:mm:ss",
      "yyyy-MM-ddTHH:mm:ss.FFFFFFF",
      "yyyy-MM-dd",
      "HH:mm:ss",
      "yyyy-MM",
      "yyyy",
      "--MM-dd",
      "---dd"
    };
    private static readonly string[] roundtripDateTimeFormats;
    private static readonly string[] localDateTimeFormats;
    private static readonly string[] utcDateTimeFormats;
    private static readonly string[] unspecifiedDateTimeFormats;
    private static DateTimeStyles _defaultStyle = DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite;

    static XmlConvert()
    {
      int length = XmlConvert.defaultDateTimeFormats.Length;
      XmlConvert.roundtripDateTimeFormats = new string[length];
      XmlConvert.localDateTimeFormats = new string[length];
      XmlConvert.utcDateTimeFormats = new string[length * 3];
      XmlConvert.unspecifiedDateTimeFormats = new string[length * 4];
      for (int index = 0; index < length; ++index)
      {
        string defaultDateTimeFormat = XmlConvert.defaultDateTimeFormats[index];
        XmlConvert.localDateTimeFormats[index] = defaultDateTimeFormat + "zzz";
        XmlConvert.roundtripDateTimeFormats[index] = defaultDateTimeFormat + (object) 'K';
        XmlConvert.utcDateTimeFormats[index * 3] = defaultDateTimeFormat;
        XmlConvert.utcDateTimeFormats[index * 3 + 1] = defaultDateTimeFormat + (object) 'Z';
        XmlConvert.utcDateTimeFormats[index * 3 + 2] = defaultDateTimeFormat + "zzz";
        XmlConvert.unspecifiedDateTimeFormats[index * 4] = defaultDateTimeFormat;
        XmlConvert.unspecifiedDateTimeFormats[index * 4 + 1] = XmlConvert.localDateTimeFormats[index];
        XmlConvert.unspecifiedDateTimeFormats[index * 4 + 2] = XmlConvert.roundtripDateTimeFormats[index];
        XmlConvert.unspecifiedDateTimeFormats[index * 4 + 3] = XmlConvert.utcDateTimeFormats[index];
      }
    }

    private static string TryDecoding(string s)
    {
      if (s == null || s.Length < 6)
        return s;
      char ch;
      try
      {
        ch = (char) int.Parse(s.Substring(1, 4), NumberStyles.HexNumber, (IFormatProvider) CultureInfo.InvariantCulture);
      }
      catch
      {
        return s[0].ToString() + XmlConvert.DecodeName(s.Substring(1));
      }
      return s.Length == 6 ? ch.ToString() : ch.ToString() + XmlConvert.DecodeName(s.Substring(6));
    }

    public static string DecodeName(string name)
    {
      if (name == null || name.Length == 0)
        return name;
      int length = name.IndexOf('_');
      if (length == -1 || length + 6 >= name.Length)
        return name;
      return name[length + 1] != 'X' && name[length + 1] != 'x' || name[length + 6] != '_' ? name[0].ToString() + XmlConvert.DecodeName(name.Substring(1)) : name.Substring(0, length) + XmlConvert.TryDecoding(name.Substring(length + 1));
    }

    public static string EncodeLocalName(string name)
    {
      if (name == null)
        return name;
      string str = XmlConvert.EncodeName(name);
      return str.IndexOf(':') == -1 ? str : str.Replace(":", "_x003A_");
    }

    internal static bool IsInvalid(char c, bool firstOnlyLetter)
    {
      if (c == ':')
        return false;
      return firstOnlyLetter ? !XmlChar.IsFirstNameChar((int) c) : !XmlChar.IsNameChar((int) c);
    }

    private static string EncodeName(string name, bool nmtoken)
    {
      if (name == null || name.Length == 0)
        return name;
      StringBuilder stringBuilder = new StringBuilder();
      int length = name.Length;
      for (int index = 0; index < length; ++index)
      {
        char c = name[index];
        if (XmlConvert.IsInvalid(c, index == 0 && !nmtoken))
          stringBuilder.AppendFormat("_x{0:X4}_", (object) (int) c);
        else if (c == '_' && index + 6 < length && name[index + 1] == 'x' && name[index + 6] == '_')
          stringBuilder.Append("_x005F_");
        else
          stringBuilder.Append(c);
      }
      return stringBuilder.ToString();
    }

    public static string EncodeName(string name) => XmlConvert.EncodeName(name, false);

    public static string EncodeNmToken(string name) => !(name == string.Empty) ? XmlConvert.EncodeName(name, true) : throw new XmlException("Invalid NmToken: ''");

    public static bool ToBoolean(string s)
    {
      s = s.Trim(XmlChar.WhitespaceChars);
      string key = s;
      if (key != null)
      {
        // ISSUE: reference to a compiler-generated field
        if (XmlConvert.\u003C\u003Ef__switch\u0024map49 == null)
        {
          // ISSUE: reference to a compiler-generated field
          XmlConvert.\u003C\u003Ef__switch\u0024map49 = new Dictionary<string, int>(4)
          {
            {
              "1",
              0
            },
            {
              "true",
              1
            },
            {
              "0",
              2
            },
            {
              "false",
              3
            }
          };
        }
        int num;
        // ISSUE: reference to a compiler-generated field
        if (XmlConvert.\u003C\u003Ef__switch\u0024map49.TryGetValue(key, out num))
        {
          switch (num)
          {
            case 0:
              return true;
            case 1:
              return true;
            case 2:
              return false;
            case 3:
              return false;
          }
        }
      }
      throw new FormatException(s + " is not a valid boolean value");
    }

    internal static string ToBinHexString(byte[] buffer)
    {
      StringWriter w = new StringWriter();
      XmlConvert.WriteBinHex(buffer, 0, buffer.Length, (TextWriter) w);
      return w.ToString();
    }

    internal static void WriteBinHex(byte[] buffer, int index, int count, TextWriter w)
    {
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer));
      if (index < 0)
        throw new ArgumentOutOfRangeException("index must be non negative integer.");
      if (count < 0)
        throw new ArgumentOutOfRangeException("count must be non negative integer.");
      if (buffer.Length < index + count)
        throw new ArgumentOutOfRangeException("index and count must be smaller than the length of the buffer.");
      int num1 = index + count;
      for (int index1 = index; index1 < num1; ++index1)
      {
        int num2 = (int) buffer[index1];
        int num3 = num2 >> 4;
        int num4 = num2 & 15;
        if (num3 > 9)
          w.Write((char) (num3 + 55));
        else
          w.Write((char) (num3 + 48));
        if (num4 > 9)
          w.Write((char) (num4 + 55));
        else
          w.Write((char) (num4 + 48));
      }
    }

    public static byte ToByte(string s) => byte.Parse(s, NumberStyles.Integer, (IFormatProvider) CultureInfo.InvariantCulture);

    public static char ToChar(string s)
    {
      if (s == null)
        throw new ArgumentNullException(nameof (s));
      return s.Length == 1 ? s[0] : throw new FormatException("String contain more than one char");
    }

    [Obsolete]
    public static DateTime ToDateTime(string s) => XmlConvert.ToDateTime(s, XmlConvert.datetimeFormats);

    public static DateTime ToDateTime(string value, XmlDateTimeSerializationMode mode)
    {
      switch (mode)
      {
        case XmlDateTimeSerializationMode.Local:
          DateTime dateTime1 = XmlConvert.ToDateTime(value, XmlConvert.localDateTimeFormats);
          return dateTime1 == DateTime.MinValue || dateTime1 == DateTime.MaxValue ? dateTime1 : dateTime1.ToLocalTime();
        case XmlDateTimeSerializationMode.Utc:
          DateTime dateTime2 = XmlConvert.ToDateTime(value, XmlConvert.utcDateTimeFormats);
          return dateTime2 == DateTime.MinValue || dateTime2 == DateTime.MaxValue ? dateTime2 : dateTime2.ToUniversalTime();
        case XmlDateTimeSerializationMode.Unspecified:
          return XmlConvert.ToDateTime(value, XmlConvert.unspecifiedDateTimeFormats);
        case XmlDateTimeSerializationMode.RoundtripKind:
          return XmlConvert.ToDateTime(value, XmlConvert.roundtripDateTimeFormats, XmlConvert._defaultStyle | DateTimeStyles.RoundtripKind);
        default:
          return XmlConvert.ToDateTime(value, XmlConvert.defaultDateTimeFormats);
      }
    }

    public static DateTime ToDateTime(string s, string format)
    {
      DateTimeStyles style = DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite;
      return DateTime.ParseExact(s, format, (IFormatProvider) DateTimeFormatInfo.InvariantInfo, style);
    }

    public static DateTime ToDateTime(string s, string[] formats) => XmlConvert.ToDateTime(s, formats, XmlConvert._defaultStyle);

    private static DateTime ToDateTime(string s, string[] formats, DateTimeStyles style) => DateTime.ParseExact(s, formats, (IFormatProvider) DateTimeFormatInfo.InvariantInfo, style);

    public static Decimal ToDecimal(string s) => Decimal.Parse(s, (IFormatProvider) CultureInfo.InvariantCulture);

    public static double ToDouble(string s)
    {
      float num = s != null ? XmlConvert.TryParseStringFloatConstants(s) : throw new ArgumentNullException();
      return (double) num != 0.0 ? (double) num : double.Parse(s, NumberStyles.Float | NumberStyles.AllowCurrencySymbol, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    private static float TryParseStringFloatConstants(string s)
    {
      int num1 = 0;
      while (num1 < s.Length && char.IsWhiteSpace(s[num1]))
        ++num1;
      if (num1 == s.Length)
        throw new FormatException();
      int num2 = s.Length - 1;
      while (char.IsWhiteSpace(s[num2]))
        --num2;
      if (XmlConvert.TryParseStringConstant("NaN", s, num1, num2))
        return float.NaN;
      if (XmlConvert.TryParseStringConstant("INF", s, num1, num2))
        return float.PositiveInfinity;
      if (XmlConvert.TryParseStringConstant("-INF", s, num1, num2))
        return float.NegativeInfinity;
      if (XmlConvert.TryParseStringConstant("Infinity", s, num1, num2))
        return float.PositiveInfinity;
      return XmlConvert.TryParseStringConstant("-Infinity", s, num1, num2) ? float.NegativeInfinity : 0.0f;
    }

    private static bool TryParseStringConstant(string format, string s, int start, int end) => end - start + 1 == format.Length && string.CompareOrdinal(format, 0, s, start, format.Length) == 0;

    public static Guid ToGuid(string s)
    {
      try
      {
        return new Guid(s);
      }
      catch (FormatException ex)
      {
        throw new FormatException(string.Format("Invalid Guid input '{0}'", (object) ex.InnerException));
      }
    }

    public static short ToInt16(string s) => short.Parse(s, NumberStyles.Integer, (IFormatProvider) CultureInfo.InvariantCulture);

    public static int ToInt32(string s) => int.Parse(s, NumberStyles.Integer, (IFormatProvider) CultureInfo.InvariantCulture);

    public static long ToInt64(string s) => long.Parse(s, NumberStyles.Integer, (IFormatProvider) CultureInfo.InvariantCulture);

    [CLSCompliant(false)]
    public static sbyte ToSByte(string s) => sbyte.Parse(s, NumberStyles.Integer, (IFormatProvider) CultureInfo.InvariantCulture);

    public static float ToSingle(string s)
    {
      float num = s != null ? XmlConvert.TryParseStringFloatConstants(s) : throw new ArgumentNullException();
      return (double) num != 0.0 ? num : float.Parse(s, NumberStyles.Float | NumberStyles.AllowCurrencySymbol, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    public static string ToString(Guid value) => value.ToString("D", (IFormatProvider) CultureInfo.InvariantCulture);

    public static string ToString(int value) => value.ToString((IFormatProvider) CultureInfo.InvariantCulture);

    public static string ToString(short value) => value.ToString((IFormatProvider) CultureInfo.InvariantCulture);

    public static string ToString(byte value) => value.ToString((IFormatProvider) CultureInfo.InvariantCulture);

    public static string ToString(long value) => value.ToString((IFormatProvider) CultureInfo.InvariantCulture);

    public static string ToString(char value) => value.ToString((IFormatProvider) CultureInfo.InvariantCulture);

    public static string ToString(bool value) => value ? "true" : "false";

    [CLSCompliant(false)]
    public static string ToString(sbyte value) => value.ToString((IFormatProvider) CultureInfo.InvariantCulture);

    public static string ToString(Decimal value) => value.ToString((IFormatProvider) CultureInfo.InvariantCulture);

    [CLSCompliant(false)]
    public static string ToString(ulong value) => value.ToString((IFormatProvider) CultureInfo.InvariantCulture);

    public static string ToString(TimeSpan value)
    {
      if (value == TimeSpan.Zero)
        return "PT0S";
      StringBuilder stringBuilder = new StringBuilder();
      if (value.Ticks < 0L)
      {
        if (value == TimeSpan.MinValue)
          return "-P10675199DT2H48M5.4775808S";
        stringBuilder.Append('-');
        value = value.Negate();
      }
      stringBuilder.Append('P');
      if (value.Days > 0)
        stringBuilder.Append(value.Days).Append('D');
      long num = value.Ticks % 10000L;
      if (value.Days > 0 || value.Hours > 0 || value.Minutes > 0 || value.Seconds > 0 || value.Milliseconds > 0 || num > 0L)
      {
        stringBuilder.Append('T');
        if (value.Hours > 0)
          stringBuilder.Append(value.Hours).Append('H');
        if (value.Minutes > 0)
          stringBuilder.Append(value.Minutes).Append('M');
        if (value.Seconds > 0 || value.Milliseconds > 0 || num > 0L)
        {
          stringBuilder.Append(value.Seconds);
          bool flag = true;
          if (num > 0L)
            stringBuilder.Append('.').AppendFormat("{0:0000000}", (object) (value.Ticks % 10000000L));
          else if (value.Milliseconds > 0)
            stringBuilder.Append('.').AppendFormat("{0:000}", (object) value.Milliseconds);
          else
            flag = false;
          if (flag)
          {
            while (stringBuilder[stringBuilder.Length - 1] == '0')
              stringBuilder.Remove(stringBuilder.Length - 1, 1);
          }
          stringBuilder.Append('S');
        }
      }
      return stringBuilder.ToString();
    }

    public static string ToString(double value)
    {
      if (double.IsNegativeInfinity(value))
        return "-INF";
      if (double.IsPositiveInfinity(value))
        return "INF";
      return double.IsNaN(value) ? "NaN" : value.ToString("R", (IFormatProvider) CultureInfo.InvariantCulture);
    }

    public static string ToString(float value)
    {
      if (float.IsNegativeInfinity(value))
        return "-INF";
      if (float.IsPositiveInfinity(value))
        return "INF";
      return float.IsNaN(value) ? "NaN" : value.ToString("R", (IFormatProvider) CultureInfo.InvariantCulture);
    }

    [CLSCompliant(false)]
    public static string ToString(uint value) => value.ToString((IFormatProvider) CultureInfo.InvariantCulture);

    [CLSCompliant(false)]
    public static string ToString(ushort value) => value.ToString((IFormatProvider) CultureInfo.InvariantCulture);

    [Obsolete]
    public static string ToString(DateTime value) => value.ToString("yyyy-MM-ddTHH:mm:ss.fffffffzzz", (IFormatProvider) CultureInfo.InvariantCulture);

    public static string ToString(DateTime value, XmlDateTimeSerializationMode mode)
    {
      switch (mode)
      {
        case XmlDateTimeSerializationMode.Local:
          return (!(value == DateTime.MinValue) ? (!(value == DateTime.MaxValue) ? value.ToLocalTime() : value) : DateTime.MinValue).ToString("yyyy-MM-ddTHH:mm:ss.FFFFFFFzzz", (IFormatProvider) CultureInfo.InvariantCulture);
        case XmlDateTimeSerializationMode.Utc:
          return (!(value == DateTime.MinValue) ? (!(value == DateTime.MaxValue) ? value.ToUniversalTime() : value) : DateTime.MinValue).ToString("yyyy-MM-ddTHH:mm:ss.FFFFFFFZ", (IFormatProvider) CultureInfo.InvariantCulture);
        case XmlDateTimeSerializationMode.Unspecified:
          return value.ToString("yyyy-MM-ddTHH:mm:ss.FFFFFFF", (IFormatProvider) CultureInfo.InvariantCulture);
        case XmlDateTimeSerializationMode.RoundtripKind:
          return value.ToString("yyyy-MM-ddTHH:mm:ss.FFFFFFFK", (IFormatProvider) CultureInfo.InvariantCulture);
        default:
          return value.ToString("yyyy-MM-ddTHH:mm:ss.FFFFFFFzzz", (IFormatProvider) CultureInfo.InvariantCulture);
      }
    }

    public static string ToString(DateTime value, string format) => value.ToString(format, (IFormatProvider) CultureInfo.InvariantCulture);

    public static TimeSpan ToTimeSpan(string s)
    {
      s = s.Trim(XmlChar.WhitespaceChars);
      if (s.Length == 0)
        throw new FormatException("Invalid format string for duration schema datatype.");
      int index1 = 0;
      if (s[0] == '-')
        index1 = 1;
      bool flag1 = index1 == 1;
      if (s[index1] != 'P')
        throw new FormatException("Invalid format string for duration schema datatype.");
      int startIndex = index1 + 1;
      int num1 = 0;
      int days = 0;
      bool flag2 = false;
      int hours = 0;
      int minutes = 0;
      int seconds = 0;
      long num2 = 0;
      int num3 = 0;
      bool flag3 = false;
      int index2 = startIndex;
      while (index2 < s.Length)
      {
        if (s[index2] == 'T')
        {
          flag2 = true;
          num1 = 4;
          ++index2;
          startIndex = index2;
        }
        else
        {
          while (index2 < s.Length && s[index2] >= '0' && '9' >= s[index2])
            ++index2;
          if (num1 == 7)
            num3 = index2 - startIndex;
          int num4 = int.Parse(s.Substring(startIndex, index2 - startIndex), (IFormatProvider) CultureInfo.InvariantCulture);
          if (num1 == 7)
          {
            for (; num3 > 7; --num3)
              num4 /= 10;
            for (; num3 < 7; ++num3)
              num4 *= 10;
          }
          switch (s[index2])
          {
            case '.':
              if (num1 > 7)
                flag3 = true;
              seconds = num4;
              num1 = 7;
              break;
            case 'D':
              days += num4;
              if (num1 > 2)
              {
                flag3 = true;
                break;
              }
              num1 = 3;
              break;
            case 'H':
              hours = num4;
              if (!flag2 || num1 > 4)
              {
                flag3 = true;
                break;
              }
              num1 = 5;
              break;
            case 'M':
              if (num1 < 2)
              {
                days += 365 * (num4 / 12) + 30 * (num4 % 12);
                num1 = 2;
                break;
              }
              if (flag2 && num1 < 6)
              {
                minutes = num4;
                num1 = 6;
                break;
              }
              flag3 = true;
              break;
            case 'S':
              if (num1 == 7)
                num2 = (long) num4;
              else
                seconds = num4;
              if (!flag2 || num1 > 7)
              {
                flag3 = true;
                break;
              }
              num1 = 8;
              break;
            case 'Y':
              days += num4 * 365;
              if (num1 > 0)
              {
                flag3 = true;
                break;
              }
              num1 = 1;
              break;
            default:
              flag3 = true;
              break;
          }
          if (!flag3)
          {
            ++index2;
            startIndex = index2;
          }
          else
            break;
        }
      }
      if (flag3)
        throw new FormatException("Invalid format string for duration schema datatype.");
      TimeSpan timeSpan = new TimeSpan(days, hours, minutes, seconds);
      return flag1 ? TimeSpan.FromTicks(-(timeSpan.Ticks + num2)) : TimeSpan.FromTicks(timeSpan.Ticks + num2);
    }

    [CLSCompliant(false)]
    public static ushort ToUInt16(string s) => ushort.Parse(s, NumberStyles.Integer, (IFormatProvider) CultureInfo.InvariantCulture);

    [CLSCompliant(false)]
    public static uint ToUInt32(string s) => uint.Parse(s, NumberStyles.Integer, (IFormatProvider) CultureInfo.InvariantCulture);

    [CLSCompliant(false)]
    public static ulong ToUInt64(string s) => ulong.Parse(s, NumberStyles.Integer, (IFormatProvider) CultureInfo.InvariantCulture);

    public static string VerifyName(string name)
    {
      if (name == null || name.Length == 0)
        throw new ArgumentNullException(nameof (name));
      return XmlChar.IsName(name) ? name : throw new XmlException("'" + name + "' is not a valid XML Name");
    }

    public static string VerifyNCName(string ncname)
    {
      if (ncname == null || ncname.Length == 0)
        throw new ArgumentNullException(nameof (ncname));
      return XmlChar.IsNCName(ncname) ? ncname : throw new XmlException("'" + ncname + "' is not a valid XML NCName");
    }

    public static string VerifyTOKEN(string name)
    {
      switch (name)
      {
        case null:
          throw new ArgumentNullException(nameof (name));
        case "":
          return name;
        default:
          if (XmlChar.IsWhitespace((int) name[0]) || XmlChar.IsWhitespace((int) name[name.Length - 1]))
            throw new XmlException("Whitespace characters (#xA, #xD, #x9, #x20) are not allowed as leading or trailing whitespaces of xs:token.");
          for (int index = 0; index < name.Length; ++index)
          {
            if (XmlChar.IsWhitespace((int) name[index]) && name[index] != ' ')
              throw new XmlException("Either #xA, #xD or #x9 are not allowed inside xs:token.");
          }
          return name;
      }
    }

    public static string VerifyNMTOKEN(string name)
    {
      if (name == null)
        throw new ArgumentNullException(nameof (name));
      return XmlChar.IsNmToken(name) ? name : throw new XmlException("'" + name + "' is not a valid XML NMTOKEN");
    }

    internal static byte[] FromBinHexString(string s)
    {
      char[] charArray = s.ToCharArray();
      byte[] buffer = new byte[charArray.Length / 2 + charArray.Length % 2];
      XmlConvert.FromBinHexString(charArray, 0, charArray.Length, buffer);
      return buffer;
    }

    internal static int FromBinHexString(char[] chars, int offset, int charLength, byte[] buffer)
    {
      int index1 = offset;
      for (int index2 = 0; index2 < charLength - 1; index2 += 2)
      {
        buffer[index1] = chars[index2] <= '9' ? (byte) ((uint) chars[index2] - 48U) : (byte) ((int) chars[index2] - 65 + 10);
        buffer[index1] <<= 4;
        buffer[index1] += chars[index2 + 1] <= '9' ? (byte) ((uint) chars[index2 + 1] - 48U) : (byte) ((int) chars[index2 + 1] - 65 + 10);
        ++index1;
      }
      if (charLength % 2 != 0)
        buffer[index1++] = (byte) ((chars[charLength - 1] <= '9' ? (int) (byte) ((uint) chars[charLength - 1] - 48U) : (int) (byte) ((int) chars[charLength - 1] - 65 + 10)) << 4);
      return index1 - offset;
    }

    public static DateTimeOffset ToDateTimeOffset(string s) => XmlConvert.ToDateTimeOffset(s, XmlConvert.datetimeFormats);

    public static DateTimeOffset ToDateTimeOffset(string s, string format) => DateTimeOffset.ParseExact(s, format, (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);

    public static DateTimeOffset ToDateTimeOffset(string s, string[] formats)
    {
      DateTimeStyles styles = DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite | DateTimeStyles.AssumeUniversal;
      return DateTimeOffset.ParseExact(s, formats, (IFormatProvider) CultureInfo.InvariantCulture, styles);
    }

    public static string ToString(DateTimeOffset value) => XmlConvert.ToString(value, "yyyy-MM-ddTHH:mm:ss.FFFFFFFzzz");

    public static string ToString(DateTimeOffset value, string format) => value.ToString(format, (IFormatProvider) CultureInfo.InvariantCulture);

    internal static Uri ToUri(string s) => new Uri(s, UriKind.RelativeOrAbsolute);
  }
}
