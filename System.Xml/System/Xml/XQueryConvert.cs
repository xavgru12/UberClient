// Decompiled with JetBrains decompiler
// Type: System.Xml.XQueryConvert
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Xml.Schema;

namespace System.Xml
{
  internal class XQueryConvert
  {
    private XQueryConvert()
    {
    }

    public static XmlTypeCode GetFallbackType(XmlTypeCode type)
    {
      switch (type)
      {
        case XmlTypeCode.AnyAtomicType:
          return XmlTypeCode.Item;
        case XmlTypeCode.UntypedAtomic:
          return XmlTypeCode.String;
        case XmlTypeCode.Notation:
          return XmlTypeCode.QName;
        case XmlTypeCode.NormalizedString:
        case XmlTypeCode.Token:
        case XmlTypeCode.Language:
        case XmlTypeCode.NmToken:
        case XmlTypeCode.Name:
        case XmlTypeCode.NCName:
        case XmlTypeCode.Id:
        case XmlTypeCode.Idref:
        case XmlTypeCode.Entity:
          return XmlTypeCode.String;
        case XmlTypeCode.NonPositiveInteger:
          return XmlTypeCode.Decimal;
        case XmlTypeCode.NegativeInteger:
          return XmlTypeCode.NonPositiveInteger;
        case XmlTypeCode.Long:
          return XmlTypeCode.Integer;
        case XmlTypeCode.Short:
          return XmlTypeCode.Int;
        case XmlTypeCode.Byte:
          return XmlTypeCode.Int;
        case XmlTypeCode.NonNegativeInteger:
          return XmlTypeCode.Decimal;
        case XmlTypeCode.UnsignedLong:
          return XmlTypeCode.NonNegativeInteger;
        case XmlTypeCode.UnsignedInt:
          return XmlTypeCode.Integer;
        case XmlTypeCode.UnsignedShort:
          return XmlTypeCode.Int;
        case XmlTypeCode.UnsignedByte:
          return XmlTypeCode.UnsignedShort;
        case XmlTypeCode.PositiveInteger:
          return XmlTypeCode.NonNegativeInteger;
        default:
          return XmlTypeCode.None;
      }
    }

    public static string AnyUriToString(string value) => value;

    public static byte[] Base64BinaryToHexBinary(byte[] value) => XmlConvert.FromBinHexString(Convert.ToBase64String(value));

    public static string Base64BinaryToString(byte[] value) => Convert.ToBase64String(value);

    public static Decimal BooleanToDecimal(bool value) => Convert.ToDecimal(value);

    public static double BooleanToDouble(bool value) => Convert.ToDouble(value);

    public static float BooleanToFloat(bool value) => Convert.ToSingle(value);

    public static int BooleanToInt(bool value) => Convert.ToInt32(value);

    public static long BooleanToInteger(bool value) => Convert.ToInt64(value);

    public static string BooleanToString(bool value) => value ? "true" : "false";

    public static DateTime DateTimeToDate(DateTime value) => value.Date;

    public static DateTime DateTimeToGDay(DateTime value) => new DateTime(0, 0, value.Day);

    public static DateTime DateTimeToGMonth(DateTime value) => new DateTime(0, value.Month, 0);

    public static DateTime DateTimeToGMonthDay(DateTime value) => new DateTime(0, value.Month, value.Day);

    public static DateTime DateTimeToGYear(DateTime value) => new DateTime(value.Year, 0, 0);

    public static DateTime DateTimeToGYearMonth(DateTime value) => new DateTime(value.Year, value.Month, 0);

    public static DateTime DateTimeToTime(DateTime value) => new DateTime(value.TimeOfDay.Ticks);

    public static DateTime DateToDateTime(DateTime value) => value.Date;

    public static DateTime DateToGDay(DateTime value) => new DateTime(0, 0, value.Day);

    public static DateTime DateToGMonth(DateTime value) => new DateTime(0, value.Month, 0);

    public static DateTime DateToGMonthDay(DateTime value) => new DateTime(0, value.Month, value.Day);

    public static DateTime DateToGYear(DateTime value) => new DateTime(value.Year, 0, 0);

    public static DateTime DateToGYearMonth(DateTime value) => new DateTime(value.Year, value.Month, 0);

    public static string DateToString(DateTime value) => XmlConvert.ToString(value);

    public static string DateTimeToString(DateTime value) => XmlConvert.ToString(value);

    public static string DayTimeDurationToDuration(TimeSpan value) => XmlConvert.ToString(value);

    public static string DayTimeDurationToString(TimeSpan value) => XQueryConvert.DayTimeDurationToDuration(value);

    public static bool DecimalToBoolean(Decimal value) => value != 0M;

    public static double DecimalToDouble(Decimal value) => Convert.ToDouble(value);

    public static float DecimalToFloat(Decimal value) => Convert.ToSingle(value);

    public static int DecimalToInt(Decimal value) => Convert.ToInt32(value);

    public static long DecimalToInteger(Decimal value) => Convert.ToInt64(value);

    public static string DecimalToString(Decimal value) => XmlConvert.ToString(value);

    public static bool DoubleToBoolean(double value) => value != 0.0;

    public static Decimal DoubleToDecimal(double value) => (Decimal) value;

    public static float DoubleToFloat(double value) => Convert.ToSingle(value);

    public static int DoubleToInt(double value) => Convert.ToInt32(value);

    public static long DoubleToInteger(double value) => Convert.ToInt64(value);

    public static string DoubleToString(double value) => XmlConvert.ToString(value);

    public static TimeSpan DurationToDayTimeDuration(string value) => XmlConvert.ToTimeSpan(value);

    public static string DurationToString(string value) => XmlConvert.ToString(XmlConvert.ToTimeSpan(value));

    public static TimeSpan DurationToYearMonthDuration(string value) => XmlConvert.ToTimeSpan(value);

    public static bool FloatToBoolean(float value) => (double) value != 0.0;

    public static Decimal FloatToDecimal(float value) => (Decimal) value;

    public static double FloatToDouble(float value) => Convert.ToDouble(value);

    public static int FloatToInt(float value) => Convert.ToInt32(value);

    public static long FloatToInteger(float value) => Convert.ToInt64(value);

    public static string FloatToString(float value) => XmlConvert.ToString(value);

    public static string GDayToString(DateTime value) => XmlConvert.ToString(TimeSpan.FromDays((double) value.Day));

    public static string GMonthDayToString(DateTime value) => XmlConvert.ToString(new TimeSpan(value.Day, value.Hour, value.Minute, value.Second));

    public static string GMonthToString(DateTime value) => XmlConvert.ToString(new TimeSpan(0, value.Month, 0));

    public static string GYearMonthToString(DateTime value) => XmlConvert.ToString(new TimeSpan(value.Year, value.Month, 0));

    public static string GYearToString(DateTime value) => XmlConvert.ToString(new TimeSpan(new DateTime(value.Year, 0, 0).Ticks));

    public static string HexBinaryToString(byte[] data) => XmlConvert.ToBinHexString(data);

    public static byte[] HexBinaryToBase64Binary(byte[] data) => data;

    public static bool IntegerToBoolean(long value) => value != 0L;

    public static Decimal IntegerToDecimal(long value) => (Decimal) value;

    public static double IntegerToDouble(long value) => Convert.ToDouble(value);

    public static float IntegerToFloat(long value) => Convert.ToSingle(value);

    public static int IntegerToInt(long value) => Convert.ToInt32(value);

    public static string IntegerToString(long value) => XmlConvert.ToString(value);

    public static bool IntToBoolean(int value) => value != 0;

    public static Decimal IntToDecimal(int value) => (Decimal) value;

    public static double IntToDouble(int value) => Convert.ToDouble(value);

    public static float IntToFloat(int value) => Convert.ToSingle(value);

    public static long IntToInteger(int value) => (long) value;

    public static string IntToString(int value) => XmlConvert.ToString(value);

    public static string NonNegativeIntegerToString(Decimal value) => XmlConvert.ToString(value);

    public static string NonPositiveIntegerToString(Decimal value) => XmlConvert.ToString(value);

    public static DateTime TimeToDateTime(DateTime value) => value;

    public static string TimeToString(DateTime value) => XmlConvert.ToString(value, "HH:mm:ssZ");

    public static string YearMonthDurationToDuration(TimeSpan value) => XmlConvert.ToString(value);

    public static string YearMonthDurationToString(TimeSpan value) => XQueryConvert.YearMonthDurationToDuration(value);

    public static string StringToAnyUri(string value) => value;

    public static byte[] StringToBase64Binary(string value) => Convert.FromBase64String(value);

    public static bool StringToBoolean(string value) => XmlConvert.ToBoolean(value);

    public static DateTime StringToDate(string value) => XmlConvert.ToDateTime(value);

    public static DateTime StringToDateTime(string value) => XmlConvert.ToDateTime(value);

    public static TimeSpan StringToDayTimeDuration(string value) => XmlConvert.ToTimeSpan(value);

    public static Decimal StringToDecimal(string value) => XmlConvert.ToDecimal(value);

    public static double StringToDouble(string value) => XmlConvert.ToDouble(value);

    public static string StringToDuration(string value) => XmlConvert.ToString(XmlConvert.ToTimeSpan(value));

    public static float StringToFloat(string value) => XmlConvert.ToSingle(value);

    public static DateTime StringToGDay(string value) => XmlConvert.ToDateTime(value);

    public static DateTime StringToGMonth(string value) => XmlConvert.ToDateTime(value);

    public static DateTime StringToGMonthDay(string value) => XmlConvert.ToDateTime(value);

    public static DateTime StringToGYear(string value) => XmlConvert.ToDateTime(value);

    public static DateTime StringToGYearMonth(string value) => XmlConvert.ToDateTime(value);

    public static byte[] StringToHexBinary(string value) => XmlConvert.FromBinHexString(value);

    public static int StringToInt(string value) => XmlConvert.ToInt32(value);

    public static long StringToInteger(string value) => XmlConvert.ToInt64(value);

    public static Decimal StringToNonNegativeInteger(string value) => XmlConvert.ToDecimal(value);

    public static Decimal StringToNonPositiveInteger(string value) => XmlConvert.ToDecimal(value);

    public static DateTime StringToTime(string value) => XmlConvert.ToDateTime(value);

    public static long StringToUnsignedInt(string value) => (long) XmlConvert.ToUInt32(value);

    public static Decimal StringToUnsignedLong(string value) => (Decimal) XmlConvert.ToUInt64(value);

    public static int StringToUnsignedShort(string value) => (int) XmlConvert.ToUInt16(value);

    public static TimeSpan StringToYearMonthDuration(string value) => XmlConvert.ToTimeSpan(value);
  }
}
