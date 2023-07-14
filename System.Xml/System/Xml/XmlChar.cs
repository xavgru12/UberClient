// Decompiled with JetBrains decompiler
// Type: System.Xml.XmlChar
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections.Generic;

namespace System.Xml
{
  internal class XmlChar
  {
    public static readonly char[] WhitespaceChars = new char[4]
    {
      ' ',
      '\n',
      '\t',
      '\r'
    };
    private static readonly byte[] firstNamePages = new byte[256]
    {
      (byte) 2,
      (byte) 3,
      (byte) 4,
      (byte) 5,
      (byte) 6,
      (byte) 7,
      (byte) 8,
      (byte) 0,
      (byte) 0,
      (byte) 9,
      (byte) 10,
      (byte) 11,
      (byte) 12,
      (byte) 13,
      (byte) 14,
      (byte) 15,
      (byte) 16,
      (byte) 17,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 18,
      (byte) 19,
      (byte) 0,
      (byte) 20,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 21,
      (byte) 22,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 23,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 24,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0
    };
    private static readonly byte[] namePages = new byte[256]
    {
      (byte) 25,
      (byte) 3,
      (byte) 26,
      (byte) 27,
      (byte) 28,
      (byte) 29,
      (byte) 30,
      (byte) 0,
      (byte) 0,
      (byte) 31,
      (byte) 32,
      (byte) 33,
      (byte) 34,
      (byte) 35,
      (byte) 36,
      (byte) 37,
      (byte) 16,
      (byte) 17,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 18,
      (byte) 19,
      (byte) 38,
      (byte) 20,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 39,
      (byte) 22,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 23,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 24,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0
    };
    private static readonly uint[] nameBitmap = new uint[320]
    {
      0U,
      0U,
      0U,
      0U,
      0U,
      0U,
      0U,
      0U,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      0U,
      67108864U,
      2281701374U,
      134217726U,
      0U,
      0U,
      4286578687U,
      4286578687U,
      uint.MaxValue,
      2146697215U,
      4294966782U,
      (uint) int.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      4294959119U,
      4231135231U,
      16777215U,
      0U,
      4294901760U,
      uint.MaxValue,
      uint.MaxValue,
      4160750079U,
      3U,
      0U,
      0U,
      0U,
      0U,
      0U,
      4294956864U,
      4294967291U,
      1417641983U,
      1048573U,
      4294959102U,
      uint.MaxValue,
      3758030847U,
      uint.MaxValue,
      4294901763U,
      uint.MaxValue,
      4294908319U,
      54513663U,
      0U,
      4294836224U,
      41943039U,
      4294967294U,
      (uint) sbyte.MaxValue,
      0U,
      4294901760U,
      460799U,
      0U,
      134217726U,
      2046U,
      4294836224U,
      uint.MaxValue,
      2097151999U,
      3112959U,
      96U,
      4294967264U,
      603979775U,
      4278190080U,
      3U,
      4294549472U,
      63307263U,
      2952790016U,
      196611U,
      4294543328U,
      57540095U,
      1577058304U,
      1835008U,
      4294684640U,
      602799615U,
      0U,
      1U,
      4294549472U,
      600702463U,
      2952790016U,
      3U,
      3594373088U,
      62899992U,
      0U,
      0U,
      4294828000U,
      66059775U,
      0U,
      3U,
      4294828000U,
      66059775U,
      1073741824U,
      3U,
      4294828000U,
      67108351U,
      0U,
      3U,
      0U,
      0U,
      0U,
      0U,
      4294967294U,
      884735U,
      63U,
      0U,
      4277151126U,
      537750702U,
      31U,
      0U,
      0U,
      0U,
      4294967039U,
      1023U,
      0U,
      0U,
      0U,
      0U,
      0U,
      0U,
      0U,
      0U,
      0U,
      uint.MaxValue,
      4294901823U,
      8388607U,
      514797U,
      1342177280U,
      2184269825U,
      2908843U,
      1073741824U,
      4118857984U,
      7U,
      33622016U,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      268435455U,
      uint.MaxValue,
      uint.MaxValue,
      67108863U,
      1061158911U,
      uint.MaxValue,
      2868854591U,
      1073741823U,
      uint.MaxValue,
      1608515583U,
      265232348U,
      534519807U,
      0U,
      19520U,
      0U,
      0U,
      7U,
      0U,
      0U,
      0U,
      128U,
      1022U,
      4294967294U,
      uint.MaxValue,
      2097151U,
      4294967294U,
      uint.MaxValue,
      134217727U,
      4294967264U,
      8191U,
      0U,
      0U,
      0U,
      0U,
      0U,
      0U,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      63U,
      0U,
      0U,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      uint.MaxValue,
      15U,
      0U,
      0U,
      0U,
      134176768U,
      2281701374U,
      134217726U,
      0U,
      8388608U,
      4286578687U,
      4286578687U,
      16777215U,
      0U,
      4294901760U,
      uint.MaxValue,
      uint.MaxValue,
      4160750079U,
      196611U,
      0U,
      uint.MaxValue,
      uint.MaxValue,
      63U,
      3U,
      4294956992U,
      4294967291U,
      1417641983U,
      1048573U,
      4294959102U,
      uint.MaxValue,
      3758030847U,
      uint.MaxValue,
      4294901883U,
      uint.MaxValue,
      4294908319U,
      54513663U,
      0U,
      4294836224U,
      41943039U,
      4294967294U,
      4294836351U,
      3154116603U,
      4294901782U,
      460799U,
      0U,
      134217726U,
      524287U,
      4294902783U,
      uint.MaxValue,
      2097151999U,
      4293885951U,
      67059199U,
      4294967278U,
      4093640703U,
      4280172543U,
      65487U,
      4294549486U,
      3552968191U,
      2961193375U,
      262095U,
      4294543332U,
      3547201023U,
      1577073031U,
      2097088U,
      4294684654U,
      4092460543U,
      15295U,
      65473U,
      4294549486U,
      4090363391U,
      2965387663U,
      65475U,
      3594373100U,
      3284125464U,
      8404423U,
      65408U,
      4294828014U,
      3287285247U,
      6307295U,
      65475U,
      4294828012U,
      3287285247U,
      1080049119U,
      65475U,
      4294828012U,
      3288333823U,
      8404431U,
      65475U,
      0U,
      0U,
      0U,
      0U,
      4294967294U,
      134184959U,
      67076095U,
      0U,
      4277151126U,
      1006595246U,
      67059551U,
      0U,
      50331648U,
      3265266687U,
      4294967039U,
      4294837247U,
      4273934303U,
      50216959U,
      0U,
      0U,
      0U,
      0U,
      0U,
      0U,
      0U,
      0U,
      536805376U,
      2U,
      160U,
      4128766U,
      4294967294U,
      uint.MaxValue,
      1713373183U,
      4294967294U,
      uint.MaxValue,
      2013265919U
    };

    public static bool IsWhitespace(int ch) => ch == 32 || ch == 9 || ch == 13 || ch == 10;

    public static bool IsWhitespace(string str)
    {
      for (int index = 0; index < str.Length; ++index)
      {
        if (!XmlChar.IsWhitespace((int) str[index]))
          return false;
      }
      return true;
    }

    public static int IndexOfNonWhitespace(string str)
    {
      for (int index = 0; index < str.Length; ++index)
      {
        if (!XmlChar.IsWhitespace((int) str[index]))
          return index;
      }
      return -1;
    }

    public static bool IsFirstNameChar(int ch)
    {
      if (ch >= 97 && ch <= 122 || ch >= 65 && ch <= 90)
        return true;
      return (uint) ch <= (uint) ushort.MaxValue && ((long) XmlChar.nameBitmap[((int) XmlChar.firstNamePages[ch >> 8] << 3) + ((ch & (int) byte.MaxValue) >> 5)] & (long) (1 << ch)) != 0L;
    }

    public static bool IsValid(int ch) => !XmlChar.IsInvalid(ch);

    public static bool IsInvalid(int ch)
    {
      switch (ch)
      {
        case 9:
        case 10:
        case 13:
          return false;
        default:
          return ch < 32 || ch >= 55296 && (ch < 57344 || ch >= 65534 && (ch < 65536 || ch >= 1114112));
      }
    }

    public static int IndexOfInvalid(string s, bool allowSurrogate)
    {
      for (int index = 0; index < s.Length; ++index)
      {
        if (XmlChar.IsInvalid((int) s[index]))
        {
          if (!allowSurrogate || index + 1 == s.Length || s[index] < '\uD800' || s[index] >= '\uDC00' || s[index + 1] < '\uDC00' || s[index + 1] >= '\uE000')
            return index;
          ++index;
        }
      }
      return -1;
    }

    public static int IndexOfInvalid(char[] s, int start, int length, bool allowSurrogate)
    {
      int num = start + length;
      if (s.Length < num)
        throw new ArgumentOutOfRangeException(nameof (length));
      for (int index = start; index < num; ++index)
      {
        if (XmlChar.IsInvalid((int) s[index]))
        {
          if (!allowSurrogate || index + 1 == num || s[index] < '\uD800' || s[index] >= '\uDC00' || s[index + 1] < '\uDC00' || s[index + 1] >= '\uE000')
            return index;
          ++index;
        }
      }
      return -1;
    }

    public static bool IsNameChar(int ch)
    {
      if (ch >= 97 && ch <= 122 || ch >= 65 && ch <= 90)
        return true;
      return (uint) ch <= (uint) ushort.MaxValue && ((long) XmlChar.nameBitmap[((int) XmlChar.namePages[ch >> 8] << 3) + ((ch & (int) byte.MaxValue) >> 5)] & (long) (1 << ch)) != 0L;
    }

    public static bool IsNCNameChar(int ch)
    {
      bool flag = false;
      if (ch >= 0 && ch <= (int) ushort.MaxValue && ch != 58)
        flag = ((long) XmlChar.nameBitmap[((int) XmlChar.namePages[ch >> 8] << 3) + ((ch & (int) byte.MaxValue) >> 5)] & (long) (1 << ch)) != 0L;
      return flag;
    }

    public static bool IsName(string str)
    {
      if (str.Length == 0 || !XmlChar.IsFirstNameChar((int) str[0]))
        return false;
      for (int index = 1; index < str.Length; ++index)
      {
        if (!XmlChar.IsNameChar((int) str[index]))
          return false;
      }
      return true;
    }

    public static bool IsNCName(string str)
    {
      if (str.Length == 0 || !XmlChar.IsFirstNameChar((int) str[0]))
        return false;
      for (int index = 0; index < str.Length; ++index)
      {
        if (!XmlChar.IsNCNameChar((int) str[index]))
          return false;
      }
      return true;
    }

    public static bool IsNmToken(string str)
    {
      if (str.Length == 0)
        return false;
      for (int index = 0; index < str.Length; ++index)
      {
        if (!XmlChar.IsNameChar((int) str[index]))
          return false;
      }
      return true;
    }

    public static bool IsPubidChar(int ch) => ((!XmlChar.IsWhitespace(ch) ? 0 : (ch != 9 ? 1 : 0)) | (97 > ch ? 0 : (ch <= 122 ? 1 : 0)) | (65 > ch ? 0 : (ch <= 90 ? 1 : 0)) | (48 > ch ? 0 : (ch <= 57 ? 1 : 0)) | ("-'()+,./:=?;!*#@$_%".IndexOf((char) ch) >= 0 ? 1 : 0)) != 0;

    public static bool IsPubid(string str)
    {
      for (int index = 0; index < str.Length; ++index)
      {
        if (!XmlChar.IsPubidChar((int) str[index]))
          return false;
      }
      return true;
    }

    public static bool IsValidIANAEncoding(string ianaEncoding)
    {
      if (ianaEncoding != null)
      {
        int length = ianaEncoding.Length;
        if (length > 0)
        {
          char ch1 = ianaEncoding[0];
          if (ch1 >= 'A' && ch1 <= 'Z' || ch1 >= 'a' && ch1 <= 'z')
          {
            for (int index = 1; index < length; ++index)
            {
              char ch2 = ianaEncoding[index];
              if ((ch2 < 'A' || ch2 > 'Z') && (ch2 < 'a' || ch2 > 'z') && (ch2 < '0' || ch2 > '9') && ch2 != '.' && ch2 != '_' && ch2 != '-')
                return false;
            }
            return true;
          }
        }
      }
      return false;
    }

    public static int GetPredefinedEntity(string name)
    {
      string key = name;
      if (key != null)
      {
        // ISSUE: reference to a compiler-generated field
        if (XmlChar.\u003C\u003Ef__switch\u0024map47 == null)
        {
          // ISSUE: reference to a compiler-generated field
          XmlChar.\u003C\u003Ef__switch\u0024map47 = new Dictionary<string, int>(5)
          {
            {
              "amp",
              0
            },
            {
              "lt",
              1
            },
            {
              "gt",
              2
            },
            {
              "quot",
              3
            },
            {
              "apos",
              4
            }
          };
        }
        int num;
        // ISSUE: reference to a compiler-generated field
        if (XmlChar.\u003C\u003Ef__switch\u0024map47.TryGetValue(key, out num))
        {
          switch (num)
          {
            case 0:
              return 38;
            case 1:
              return 60;
            case 2:
              return 62;
            case 3:
              return 34;
            case 4:
              return 39;
          }
        }
      }
      return -1;
    }
  }
}
