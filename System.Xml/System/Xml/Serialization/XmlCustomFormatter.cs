// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.XmlCustomFormatter
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace System.Xml.Serialization
{
  internal class XmlCustomFormatter
  {
    internal static string FromByteArrayBase64(byte[] value) => value == null ? string.Empty : Convert.ToBase64String(value);

    internal static string FromByteArrayHex(byte[] value)
    {
      if (value == null)
        return (string) null;
      StringBuilder stringBuilder = new StringBuilder();
      foreach (byte num in value)
        stringBuilder.Append(num.ToString("X2", (IFormatProvider) CultureInfo.InvariantCulture));
      return stringBuilder.ToString();
    }

    internal static string FromChar(char value) => ((int) value).ToString((IFormatProvider) CultureInfo.InvariantCulture);

    internal static string FromDate(DateTime value) => XmlConvert.ToString(value, "yyyy-MM-dd");

    internal static string FromDateTime(DateTime value) => XmlConvert.ToString(value, XmlDateTimeSerializationMode.RoundtripKind);

    internal static string FromTime(DateTime value) => XmlConvert.ToString(value, "HH:mm:ss.fffffffzzz");

    internal static string FromEnum(long value, string[] values, long[] ids) => XmlCustomFormatter.FromEnum(value, values, ids, (string) null);

    internal static string FromEnum(long value, string[] values, long[] ids, string typeName)
    {
      StringBuilder stringBuilder = new StringBuilder();
      int length = ids.Length;
      long num = value;
      int index1 = -1;
      for (int index2 = 0; index2 < length; ++index2)
      {
        if (ids[index2] == 0L)
          index1 = index2;
        else if (num != 0L)
        {
          if ((ids[index2] & value) == ids[index2])
          {
            if (stringBuilder.Length != 0)
              stringBuilder.Append(' ');
            stringBuilder.Append(values[index2]);
            num &= ~ids[index2];
          }
        }
        else
          break;
      }
      if (num != 0L)
      {
        if (typeName != null)
          throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, "'{0}' is not a valid value for {1}.", (object) value, (object) typeName));
        throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, "'{0}' is not a valid value.", (object) value));
      }
      if (stringBuilder.Length == 0 && index1 != -1)
        stringBuilder.Append(values[index1]);
      return stringBuilder.ToString();
    }

    internal static string FromXmlName(string name) => XmlConvert.EncodeName(name);

    internal static string FromXmlNCName(string ncName) => XmlConvert.EncodeLocalName(ncName);

    internal static string FromXmlNmToken(string nmToken) => XmlConvert.EncodeNmToken(nmToken);

    internal static string FromXmlNmTokens(string nmTokens)
    {
      string[] strArray = nmTokens.Split(' ');
      for (int index = 0; index < strArray.Length; ++index)
        strArray[index] = XmlCustomFormatter.FromXmlNmToken(strArray[index]);
      return string.Join(" ", strArray);
    }

    internal static byte[] ToByteArrayBase64(string value) => Convert.FromBase64String(value);

    internal static char ToChar(string value) => (char) XmlConvert.ToUInt16(value);

    internal static DateTime ToDate(string value) => XmlCustomFormatter.ToDateTime(value);

    internal static DateTime ToDateTime(string value) => XmlConvert.ToDateTime(value, XmlDateTimeSerializationMode.RoundtripKind);

    internal static DateTime ToTime(string value) => XmlCustomFormatter.ToDateTime(value);

    internal static long ToEnum(string value, Hashtable values, string typeName, bool validate)
    {
      long num = 0;
      string str = value;
      char[] chArray = new char[1]{ ' ' };
      foreach (string key in str.Split(chArray))
      {
        object obj = values[(object) key];
        if (obj != null)
          num |= (long) obj;
        else if (validate && key.Length != 0)
          throw new InvalidOperationException(string.Format("'{0}' is not a valid member of type {1}.", (object) key, (object) typeName));
      }
      return num;
    }

    internal static string ToXmlName(string value) => XmlConvert.DecodeName(value);

    internal static string ToXmlNCName(string value) => XmlCustomFormatter.ToXmlName(value);

    internal static string ToXmlNmToken(string value) => XmlCustomFormatter.ToXmlName(value);

    internal static string ToXmlNmTokens(string value) => XmlCustomFormatter.ToXmlName(value);

    internal static string ToXmlString(TypeData type, object value)
    {
      if (value == null)
        return (string) null;
      string xmlType = type.XmlType;
      if (xmlType != null)
      {
        // ISSUE: reference to a compiler-generated field
        if (XmlCustomFormatter.\u003C\u003Ef__switch\u0024map3D == null)
        {
          // ISSUE: reference to a compiler-generated field
          XmlCustomFormatter.\u003C\u003Ef__switch\u0024map3D = new Dictionary<string, int>(21)
          {
            {
              "boolean",
              0
            },
            {
              "unsignedByte",
              1
            },
            {
              "char",
              2
            },
            {
              "dateTime",
              3
            },
            {
              "date",
              4
            },
            {
              "time",
              5
            },
            {
              "decimal",
              6
            },
            {
              "double",
              7
            },
            {
              "short",
              8
            },
            {
              "int",
              9
            },
            {
              "long",
              10
            },
            {
              "byte",
              11
            },
            {
              "float",
              12
            },
            {
              "unsignedShort",
              13
            },
            {
              "unsignedInt",
              14
            },
            {
              "unsignedLong",
              15
            },
            {
              "guid",
              16
            },
            {
              "base64",
              17
            },
            {
              "base64Binary",
              17
            },
            {
              "hexBinary",
              18
            },
            {
              "duration",
              19
            }
          };
        }
        int num;
        // ISSUE: reference to a compiler-generated field
        if (XmlCustomFormatter.\u003C\u003Ef__switch\u0024map3D.TryGetValue(xmlType, out num))
        {
          switch (num)
          {
            case 0:
              return XmlConvert.ToString((bool) value);
            case 1:
              return XmlConvert.ToString((byte) value);
            case 2:
              return XmlConvert.ToString((int) (char) value);
            case 3:
              return XmlConvert.ToString((DateTime) value, XmlDateTimeSerializationMode.RoundtripKind);
            case 4:
              return ((DateTime) value).ToString("yyyy-MM-dd", (IFormatProvider) CultureInfo.InvariantCulture);
            case 5:
              return ((DateTime) value).ToString("HH:mm:ss.FFFFFFF", (IFormatProvider) CultureInfo.InvariantCulture);
            case 6:
              return XmlConvert.ToString((Decimal) value);
            case 7:
              return XmlConvert.ToString((double) value);
            case 8:
              return XmlConvert.ToString((short) value);
            case 9:
              return XmlConvert.ToString((int) value);
            case 10:
              return XmlConvert.ToString((long) value);
            case 11:
              return XmlConvert.ToString((sbyte) value);
            case 12:
              return XmlConvert.ToString((float) value);
            case 13:
              return XmlConvert.ToString((ushort) value);
            case 14:
              return XmlConvert.ToString((uint) value);
            case 15:
              return XmlConvert.ToString((ulong) value);
            case 16:
              return XmlConvert.ToString((Guid) value);
            case 17:
              return value == null ? string.Empty : Convert.ToBase64String((byte[]) value);
            case 18:
              return value == null ? string.Empty : XmlConvert.ToBinHexString((byte[]) value);
            case 19:
              return (string) value;
          }
        }
      }
      return value is IFormattable ? ((IFormattable) value).ToString((string) null, (IFormatProvider) CultureInfo.InvariantCulture) : value.ToString();
    }

    internal static object FromXmlString(TypeData type, string value)
    {
      if (value == null)
        return (object) null;
      string xmlType = type.XmlType;
      if (xmlType != null)
      {
        // ISSUE: reference to a compiler-generated field
        if (XmlCustomFormatter.\u003C\u003Ef__switch\u0024map3E == null)
        {
          // ISSUE: reference to a compiler-generated field
          XmlCustomFormatter.\u003C\u003Ef__switch\u0024map3E = new Dictionary<string, int>(21)
          {
            {
              "boolean",
              0
            },
            {
              "unsignedByte",
              1
            },
            {
              "char",
              2
            },
            {
              "dateTime",
              3
            },
            {
              "date",
              4
            },
            {
              "time",
              5
            },
            {
              "decimal",
              6
            },
            {
              "double",
              7
            },
            {
              "short",
              8
            },
            {
              "int",
              9
            },
            {
              "long",
              10
            },
            {
              "byte",
              11
            },
            {
              "float",
              12
            },
            {
              "unsignedShort",
              13
            },
            {
              "unsignedInt",
              14
            },
            {
              "unsignedLong",
              15
            },
            {
              "guid",
              16
            },
            {
              "base64",
              17
            },
            {
              "base64Binary",
              17
            },
            {
              "hexBinary",
              18
            },
            {
              "duration",
              19
            }
          };
        }
        int num;
        // ISSUE: reference to a compiler-generated field
        if (XmlCustomFormatter.\u003C\u003Ef__switch\u0024map3E.TryGetValue(xmlType, out num))
        {
          switch (num)
          {
            case 0:
              return (object) XmlConvert.ToBoolean(value);
            case 1:
              return (object) XmlConvert.ToByte(value);
            case 2:
              return (object) (char) XmlConvert.ToInt32(value);
            case 3:
              return (object) XmlConvert.ToDateTime(value, XmlDateTimeSerializationMode.RoundtripKind);
            case 4:
              return (object) DateTime.ParseExact(value, "yyyy-MM-dd", (IFormatProvider) null);
            case 5:
              return (object) DateTime.ParseExact(value, "HH:mm:ss.FFFFFFF", (IFormatProvider) null);
            case 6:
              return (object) XmlConvert.ToDecimal(value);
            case 7:
              return (object) XmlConvert.ToDouble(value);
            case 8:
              return (object) XmlConvert.ToInt16(value);
            case 9:
              return (object) XmlConvert.ToInt32(value);
            case 10:
              return (object) XmlConvert.ToInt64(value);
            case 11:
              return (object) XmlConvert.ToSByte(value);
            case 12:
              return (object) XmlConvert.ToSingle(value);
            case 13:
              return (object) XmlConvert.ToUInt16(value);
            case 14:
              return (object) XmlConvert.ToUInt32(value);
            case 15:
              return (object) XmlConvert.ToUInt64(value);
            case 16:
              return (object) XmlConvert.ToGuid(value);
            case 17:
              return (object) Convert.FromBase64String(value);
            case 18:
              return (object) XmlConvert.FromBinHexString(value);
            case 19:
              return (object) value;
          }
        }
      }
      return type.Type != null ? Convert.ChangeType((object) value, type.Type) : (object) value;
    }

    internal static string GenerateToXmlString(TypeData type, string value)
    {
      if (!type.NullableOverride)
        return XmlCustomFormatter.GenerateToXmlStringCore(type, value);
      return "(" + value + " != null ? " + XmlCustomFormatter.GenerateToXmlStringCore(type, value) + " : null)";
    }

    private static string GenerateToXmlStringCore(TypeData type, string value)
    {
      if (type.NullableOverride)
        value += ".Value";
      string xmlType = type.XmlType;
      if (xmlType != null)
      {
        // ISSUE: reference to a compiler-generated field
        if (XmlCustomFormatter.\u003C\u003Ef__switch\u0024map3F == null)
        {
          // ISSUE: reference to a compiler-generated field
          XmlCustomFormatter.\u003C\u003Ef__switch\u0024map3F = new Dictionary<string, int>(32)
          {
            {
              "boolean",
              0
            },
            {
              "unsignedByte",
              1
            },
            {
              "char",
              2
            },
            {
              "dateTime",
              3
            },
            {
              "date",
              4
            },
            {
              "time",
              5
            },
            {
              "decimal",
              6
            },
            {
              "double",
              7
            },
            {
              "short",
              8
            },
            {
              "int",
              9
            },
            {
              "long",
              10
            },
            {
              "byte",
              11
            },
            {
              "float",
              12
            },
            {
              "unsignedShort",
              13
            },
            {
              "unsignedInt",
              14
            },
            {
              "unsignedLong",
              15
            },
            {
              "guid",
              16
            },
            {
              "base64",
              17
            },
            {
              "base64Binary",
              17
            },
            {
              "hexBinary",
              18
            },
            {
              "duration",
              19
            },
            {
              "NMTOKEN",
              20
            },
            {
              "Name",
              20
            },
            {
              "NCName",
              20
            },
            {
              "language",
              20
            },
            {
              "ENTITY",
              20
            },
            {
              "ID",
              20
            },
            {
              "IDREF",
              20
            },
            {
              "NOTATION",
              20
            },
            {
              "token",
              20
            },
            {
              "normalizedString",
              20
            },
            {
              "string",
              20
            }
          };
        }
        int num;
        // ISSUE: reference to a compiler-generated field
        if (XmlCustomFormatter.\u003C\u003Ef__switch\u0024map3F.TryGetValue(xmlType, out num))
        {
          switch (num)
          {
            case 0:
              return "(" + value + "?\"true\":\"false\")";
            case 1:
              return value + ".ToString(CultureInfo.InvariantCulture)";
            case 2:
              return "((int)(" + value + ")).ToString(CultureInfo.InvariantCulture)";
            case 3:
              return "XmlConvert.ToString (" + value + ", XmlDateTimeSerializationMode.RoundtripKind)";
            case 4:
              return value + ".ToString(\"yyyy-MM-dd\", CultureInfo.InvariantCulture)";
            case 5:
              return value + ".ToString(\"HH:mm:ss.FFFFFFF\", CultureInfo.InvariantCulture)";
            case 6:
              return "XmlConvert.ToString (" + value + ")";
            case 7:
              return "XmlConvert.ToString (" + value + ")";
            case 8:
              return value + ".ToString(CultureInfo.InvariantCulture)";
            case 9:
              return value + ".ToString(CultureInfo.InvariantCulture)";
            case 10:
              return value + ".ToString(CultureInfo.InvariantCulture)";
            case 11:
              return value + ".ToString(CultureInfo.InvariantCulture)";
            case 12:
              return "XmlConvert.ToString (" + value + ")";
            case 13:
              return value + ".ToString(CultureInfo.InvariantCulture)";
            case 14:
              return value + ".ToString(CultureInfo.InvariantCulture)";
            case 15:
              return value + ".ToString(CultureInfo.InvariantCulture)";
            case 16:
              return "XmlConvert.ToString (" + value + ")";
            case 17:
              return value + " == null ? String.Empty : Convert.ToBase64String (" + value + ")";
            case 18:
              return value + " == null ? String.Empty : ToBinHexString (" + value + ")";
            case 19:
              return value;
            case 20:
              return value;
          }
        }
      }
      return "((" + value + " != null) ? (" + value + ").ToString() : null)";
    }

    internal static string GenerateFromXmlString(TypeData type, string value)
    {
      if (!type.NullableOverride)
        return XmlCustomFormatter.GenerateFromXmlStringCore(type, value);
      return "(" + value + " != null ? (" + type.CSharpName + "?)" + XmlCustomFormatter.GenerateFromXmlStringCore(type, value) + " : null)";
    }

    private static string GenerateFromXmlStringCore(TypeData type, string value)
    {
      string xmlType = type.XmlType;
      if (xmlType != null)
      {
        // ISSUE: reference to a compiler-generated field
        if (XmlCustomFormatter.\u003C\u003Ef__switch\u0024map40 == null)
        {
          // ISSUE: reference to a compiler-generated field
          XmlCustomFormatter.\u003C\u003Ef__switch\u0024map40 = new Dictionary<string, int>(21)
          {
            {
              "boolean",
              0
            },
            {
              "unsignedByte",
              1
            },
            {
              "char",
              2
            },
            {
              "dateTime",
              3
            },
            {
              "date",
              4
            },
            {
              "time",
              5
            },
            {
              "decimal",
              6
            },
            {
              "double",
              7
            },
            {
              "short",
              8
            },
            {
              "int",
              9
            },
            {
              "long",
              10
            },
            {
              "byte",
              11
            },
            {
              "float",
              12
            },
            {
              "unsignedShort",
              13
            },
            {
              "unsignedInt",
              14
            },
            {
              "unsignedLong",
              15
            },
            {
              "guid",
              16
            },
            {
              "base64:",
              17
            },
            {
              "base64Binary",
              17
            },
            {
              "hexBinary",
              18
            },
            {
              "duration",
              19
            }
          };
        }
        int num;
        // ISSUE: reference to a compiler-generated field
        if (XmlCustomFormatter.\u003C\u003Ef__switch\u0024map40.TryGetValue(xmlType, out num))
        {
          switch (num)
          {
            case 0:
              return "XmlConvert.ToBoolean (" + value + ")";
            case 1:
              return "byte.Parse (" + value + ", CultureInfo.InvariantCulture)";
            case 2:
              return "(char)Int32.Parse (" + value + ", CultureInfo.InvariantCulture)";
            case 3:
              return "XmlConvert.ToDateTime (" + value + ", XmlDateTimeSerializationMode.RoundtripKind)";
            case 4:
              return "DateTime.ParseExact (" + value + ", \"yyyy-MM-dd\", CultureInfo.InvariantCulture)";
            case 5:
              return "DateTime.ParseExact (" + value + ", \"HH:mm:ss.FFFFFFF\", CultureInfo.InvariantCulture)";
            case 6:
              return "Decimal.Parse (" + value + ", CultureInfo.InvariantCulture)";
            case 7:
              return "XmlConvert.ToDouble (" + value + ")";
            case 8:
              return "Int16.Parse (" + value + ", CultureInfo.InvariantCulture)";
            case 9:
              return "Int32.Parse (" + value + ", CultureInfo.InvariantCulture)";
            case 10:
              return "Int64.Parse (" + value + ", CultureInfo.InvariantCulture)";
            case 11:
              return "SByte.Parse (" + value + ", CultureInfo.InvariantCulture)";
            case 12:
              return "XmlConvert.ToSingle (" + value + ")";
            case 13:
              return "UInt16.Parse (" + value + ", CultureInfo.InvariantCulture)";
            case 14:
              return "UInt32.Parse (" + value + ", CultureInfo.InvariantCulture)";
            case 15:
              return "UInt64.Parse (" + value + ", CultureInfo.InvariantCulture)";
            case 16:
              return "XmlConvert.ToGuid (" + value + ")";
            case 17:
              return "Convert.FromBase64String (" + value + ")";
            case 18:
              return "FromBinHexString (" + value + ")";
            case 19:
              return value;
          }
        }
      }
      return value;
    }
  }
}
