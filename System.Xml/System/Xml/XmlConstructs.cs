// Decompiled with JetBrains decompiler
// Type: System.Xml.XmlConstructs
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections.Generic;

namespace System.Xml
{
  internal class XmlConstructs
  {
    internal const int VALID = 1;
    internal const int SPACE = 2;
    internal const int NAME_START = 4;
    internal const int NAME = 8;
    internal const int PUBID = 16;
    internal const int CONTENT = 32;
    internal const int NCNAME_START = 64;
    internal const int NCNAME = 128;
    internal static readonly char[] WhitespaceChars = new char[4]
    {
      ' ',
      '\n',
      '\t',
      '\r'
    };
    internal static readonly byte[] CHARS = new byte[65536];

    static XmlConstructs()
    {
      int[] numArray1 = new int[8]
      {
        9,
        10,
        13,
        13,
        32,
        55295,
        57344,
        65533
      };
      int[] numArray2 = new int[4]{ 32, 9, 13, 10 };
      int[] numArray3 = new int[2]{ 45, 46 };
      int[] numArray4 = new int[2]{ 58, 95 };
      int[] numArray5 = new int[9]
      {
        10,
        13,
        32,
        33,
        35,
        36,
        37,
        61,
        95
      };
      int[] numArray6 = new int[6]
      {
        39,
        59,
        63,
        90,
        97,
        122
      };
      int[] numArray7 = new int[302]
      {
        65,
        90,
        97,
        122,
        192,
        214,
        216,
        246,
        248,
        305,
        308,
        318,
        321,
        328,
        330,
        382,
        384,
        451,
        461,
        496,
        500,
        501,
        506,
        535,
        592,
        680,
        699,
        705,
        904,
        906,
        910,
        929,
        931,
        974,
        976,
        982,
        994,
        1011,
        1025,
        1036,
        1038,
        1103,
        1105,
        1116,
        1118,
        1153,
        1168,
        1220,
        1223,
        1224,
        1227,
        1228,
        1232,
        1259,
        1262,
        1269,
        1272,
        1273,
        1329,
        1366,
        1377,
        1414,
        1488,
        1514,
        1520,
        1522,
        1569,
        1594,
        1601,
        1610,
        1649,
        1719,
        1722,
        1726,
        1728,
        1742,
        1744,
        1747,
        1765,
        1766,
        2309,
        2361,
        2392,
        2401,
        2437,
        2444,
        2447,
        2448,
        2451,
        2472,
        2474,
        2480,
        2486,
        2489,
        2524,
        2525,
        2527,
        2529,
        2544,
        2545,
        2565,
        2570,
        2575,
        2576,
        2579,
        2600,
        2602,
        2608,
        2610,
        2611,
        2613,
        2614,
        2616,
        2617,
        2649,
        2652,
        2674,
        2676,
        2693,
        2699,
        2703,
        2705,
        2707,
        2728,
        2730,
        2736,
        2738,
        2739,
        2741,
        2745,
        2821,
        2828,
        2831,
        2832,
        2835,
        2856,
        2858,
        2864,
        2866,
        2867,
        2870,
        2873,
        2908,
        2909,
        2911,
        2913,
        2949,
        2954,
        2958,
        2960,
        2962,
        2965,
        2969,
        2970,
        2974,
        2975,
        2979,
        2980,
        2984,
        2986,
        2990,
        2997,
        2999,
        3001,
        3077,
        3084,
        3086,
        3088,
        3090,
        3112,
        3114,
        3123,
        3125,
        3129,
        3168,
        3169,
        3205,
        3212,
        3214,
        3216,
        3218,
        3240,
        3242,
        3251,
        3253,
        3257,
        3296,
        3297,
        3333,
        3340,
        3342,
        3344,
        3346,
        3368,
        3370,
        3385,
        3424,
        3425,
        3585,
        3630,
        3634,
        3635,
        3648,
        3653,
        3713,
        3714,
        3719,
        3720,
        3732,
        3735,
        3737,
        3743,
        3745,
        3747,
        3754,
        3755,
        3757,
        3758,
        3762,
        3763,
        3776,
        3780,
        3904,
        3911,
        3913,
        3945,
        4256,
        4293,
        4304,
        4342,
        4354,
        4355,
        4357,
        4359,
        4363,
        4364,
        4366,
        4370,
        4436,
        4437,
        4447,
        4449,
        4461,
        4462,
        4466,
        4467,
        4526,
        4527,
        4535,
        4536,
        4540,
        4546,
        7680,
        7835,
        7840,
        7929,
        7936,
        7957,
        7960,
        7965,
        7968,
        8005,
        8008,
        8013,
        8016,
        8023,
        8031,
        8061,
        8064,
        8116,
        8118,
        8124,
        8130,
        8132,
        8134,
        8140,
        8144,
        8147,
        8150,
        8155,
        8160,
        8172,
        8178,
        8180,
        8182,
        8188,
        8490,
        8491,
        8576,
        8578,
        12353,
        12436,
        12449,
        12538,
        12549,
        12588,
        44032,
        55203,
        12321,
        12329,
        19968,
        40869
      };
      int[] numArray8 = new int[53]
      {
        902,
        908,
        986,
        988,
        990,
        992,
        1369,
        1749,
        2365,
        2482,
        2654,
        2701,
        2749,
        2784,
        2877,
        2972,
        3294,
        3632,
        3716,
        3722,
        3725,
        3749,
        3751,
        3760,
        3773,
        4352,
        4361,
        4412,
        4414,
        4416,
        4428,
        4430,
        4432,
        4441,
        4451,
        4453,
        4455,
        4457,
        4469,
        4510,
        4520,
        4523,
        4538,
        4587,
        4592,
        4601,
        8025,
        8027,
        8029,
        8126,
        8486,
        8494,
        12295
      };
      int[] numArray9 = new int[132]
      {
        768,
        837,
        864,
        865,
        1155,
        1158,
        1425,
        1441,
        1443,
        1465,
        1467,
        1469,
        1473,
        1474,
        1611,
        1618,
        1750,
        1756,
        1757,
        1759,
        1760,
        1764,
        1767,
        1768,
        1770,
        1773,
        2305,
        2307,
        2366,
        2380,
        2385,
        2388,
        2402,
        2403,
        2433,
        2435,
        2496,
        2500,
        2503,
        2504,
        2507,
        2509,
        2530,
        2531,
        2624,
        2626,
        2631,
        2632,
        2635,
        2637,
        2672,
        2673,
        2689,
        2691,
        2750,
        2757,
        2759,
        2761,
        2763,
        2765,
        2817,
        2819,
        2878,
        2883,
        2887,
        2888,
        2891,
        2893,
        2902,
        2903,
        2946,
        2947,
        3006,
        3010,
        3014,
        3016,
        3018,
        3021,
        3073,
        3075,
        3134,
        3140,
        3142,
        3144,
        3146,
        3149,
        3157,
        3158,
        3202,
        3203,
        3262,
        3268,
        3270,
        3272,
        3274,
        3277,
        3285,
        3286,
        3330,
        3331,
        3390,
        3395,
        3398,
        3400,
        3402,
        3405,
        3636,
        3642,
        3655,
        3662,
        3764,
        3769,
        3771,
        3772,
        3784,
        3789,
        3864,
        3865,
        3953,
        3972,
        3974,
        3979,
        3984,
        3989,
        3993,
        4013,
        4017,
        4023,
        8400,
        8412,
        12330,
        12335
      };
      int[] numArray10 = new int[29]
      {
        1471,
        1476,
        1648,
        2364,
        2381,
        2492,
        2494,
        2495,
        2519,
        2562,
        2620,
        2622,
        2623,
        2748,
        2876,
        3031,
        3415,
        3633,
        3761,
        3893,
        3895,
        3897,
        3902,
        3903,
        3991,
        4025,
        8417,
        12441,
        12442
      };
      int[] numArray11 = new int[30]
      {
        48,
        57,
        1632,
        1641,
        1776,
        1785,
        2406,
        2415,
        2534,
        2543,
        2662,
        2671,
        2790,
        2799,
        2918,
        2927,
        3047,
        3055,
        3174,
        3183,
        3302,
        3311,
        3430,
        3439,
        3664,
        3673,
        3792,
        3801,
        3872,
        3881
      };
      int[] numArray12 = new int[6]
      {
        12337,
        12341,
        12445,
        12446,
        12540,
        12542
      };
      int[] numArray13 = new int[8]
      {
        183,
        720,
        721,
        903,
        1600,
        3654,
        3782,
        12293
      };
      int[] numArray14 = new int[5]{ 60, 38, 10, 13, 93 };
      for (int index1 = 0; index1 < numArray1.Length; index1 += 2)
      {
        for (int index2 = numArray1[index1]; index2 <= numArray1[index1 + 1]; ++index2)
          XmlConstructs.CHARS[index2] = (byte) ((int) XmlConstructs.CHARS[index2] | 1 | 32);
      }
      for (int index = 0; index < numArray14.Length; ++index)
        XmlConstructs.CHARS[numArray14[index]] = (byte) ((uint) XmlConstructs.CHARS[numArray14[index]] & 4294967263U);
      for (int index = 0; index < numArray2.Length; ++index)
        XmlConstructs.CHARS[numArray2[index]] = (byte) ((uint) XmlConstructs.CHARS[numArray2[index]] | 2U);
      for (int index = 0; index < numArray4.Length; ++index)
        XmlConstructs.CHARS[numArray4[index]] = (byte) ((int) XmlConstructs.CHARS[numArray4[index]] | 4 | 8 | 64 | 128);
      for (int index3 = 0; index3 < numArray7.Length; index3 += 2)
      {
        for (int index4 = numArray7[index3]; index4 <= numArray7[index3 + 1]; ++index4)
          XmlConstructs.CHARS[index4] = (byte) ((int) XmlConstructs.CHARS[index4] | 4 | 8 | 64 | 128);
      }
      for (int index = 0; index < numArray8.Length; ++index)
        XmlConstructs.CHARS[numArray8[index]] = (byte) ((int) XmlConstructs.CHARS[numArray8[index]] | 4 | 8 | 64 | 128);
      for (int index = 0; index < numArray3.Length; ++index)
        XmlConstructs.CHARS[numArray3[index]] = (byte) ((int) XmlConstructs.CHARS[numArray3[index]] | 8 | 128);
      for (int index5 = 0; index5 < numArray11.Length; index5 += 2)
      {
        for (int index6 = numArray11[index5]; index6 <= numArray11[index5 + 1]; ++index6)
          XmlConstructs.CHARS[index6] = (byte) ((int) XmlConstructs.CHARS[index6] | 8 | 128);
      }
      for (int index7 = 0; index7 < numArray9.Length; index7 += 2)
      {
        for (int index8 = numArray9[index7]; index8 <= numArray9[index7 + 1]; ++index8)
          XmlConstructs.CHARS[index8] = (byte) ((int) XmlConstructs.CHARS[index8] | 8 | 128);
      }
      for (int index = 0; index < numArray10.Length; ++index)
        XmlConstructs.CHARS[numArray10[index]] = (byte) ((int) XmlConstructs.CHARS[numArray10[index]] | 8 | 128);
      for (int index9 = 0; index9 < numArray12.Length; index9 += 2)
      {
        for (int index10 = numArray12[index9]; index10 <= numArray12[index9 + 1]; ++index10)
          XmlConstructs.CHARS[index10] = (byte) ((int) XmlConstructs.CHARS[index10] | 8 | 128);
      }
      for (int index = 0; index < numArray13.Length; ++index)
        XmlConstructs.CHARS[numArray13[index]] = (byte) ((int) XmlConstructs.CHARS[numArray13[index]] | 8 | 128);
      XmlConstructs.CHARS[58] = (byte) ((uint) XmlConstructs.CHARS[58] & 4294967103U);
      for (int index = 0; index < numArray5.Length; ++index)
        XmlConstructs.CHARS[numArray5[index]] = (byte) ((uint) XmlConstructs.CHARS[numArray5[index]] | 16U);
      for (int index11 = 0; index11 < numArray6.Length; index11 += 2)
      {
        for (int index12 = numArray6[index11]; index12 <= numArray6[index11 + 1]; ++index12)
          XmlConstructs.CHARS[index12] = (byte) ((uint) XmlConstructs.CHARS[index12] | 16U);
      }
    }

    public static bool IsValid(char c) => c > char.MinValue && ((int) XmlConstructs.CHARS[(int) c] & 1) != 0;

    public static bool IsValid(int c)
    {
      if (c > (int) ushort.MaxValue)
        return c < 1114112;
      return c > 0 && ((int) XmlConstructs.CHARS[c] & 1) != 0;
    }

    public static bool IsInvalid(char c) => !XmlConstructs.IsValid(c);

    public static bool IsInvalid(int c) => !XmlConstructs.IsValid(c);

    public static bool IsContent(char c) => ((int) XmlConstructs.CHARS[(int) c] & 32) != 0;

    public static bool IsContent(int c) => c > 0 && c < XmlConstructs.CHARS.Length && ((int) XmlConstructs.CHARS[c] & 32) != 0;

    public static bool IsMarkup(char c) => c == '<' || c == '&' || c == '%';

    public static bool IsMarkup(int c)
    {
      if (c <= 0 || c >= XmlConstructs.CHARS.Length)
        return false;
      return c == 60 || c == 38 || c == 37;
    }

    public static bool IsWhitespace(char c) => ((int) XmlConstructs.CHARS[(int) c] & 2) != 0;

    public static bool IsWhitespace(int c) => c > 0 && c < XmlConstructs.CHARS.Length && ((int) XmlConstructs.CHARS[c] & 2) != 0;

    public static bool IsFirstNameChar(char c) => ((int) XmlConstructs.CHARS[(int) c] & 4) != 0;

    public static bool IsFirstNameChar(int c) => c > 0 && c < XmlConstructs.CHARS.Length && ((int) XmlConstructs.CHARS[c] & 4) != 0;

    public static bool IsNameChar(char c) => ((int) XmlConstructs.CHARS[(int) c] & 8) != 0;

    public static bool IsNameChar(int c) => c > 0 && c < XmlConstructs.CHARS.Length && ((int) XmlConstructs.CHARS[c] & 8) != 0;

    public static bool IsNCNameStart(char c) => ((int) XmlConstructs.CHARS[(int) c] & 64) != 0;

    public static bool IsNCNameStart(int c) => c > 0 && c < XmlConstructs.CHARS.Length && ((int) XmlConstructs.CHARS[c] & 64) != 0;

    public static bool IsNCNameChar(char c) => ((int) XmlConstructs.CHARS[(int) c] & 128) != 0;

    public static bool IsNCNameChar(int c) => c > 0 && c < XmlConstructs.CHARS.Length && ((int) XmlConstructs.CHARS[c] & 128) != 0;

    public static bool IsPubidChar(char c) => ((int) XmlConstructs.CHARS[(int) c] & 16) != 0;

    public static bool IsPubidChar(int c) => c > 0 && c < XmlConstructs.CHARS.Length && ((int) XmlConstructs.CHARS[c] & 16) != 0;

    public static bool IsValidName(string name, out Exception err)
    {
      err = (Exception) null;
      if (name.Length == 0)
      {
        err = (Exception) new XmlException("Name can not be an empty string", (Exception) null);
        return false;
      }
      char c1 = name[0];
      if (!XmlConstructs.IsFirstNameChar(c1))
      {
        err = (Exception) new XmlException("The character '" + (object) c1 + "' cannot start a Name", (Exception) null);
        return false;
      }
      for (int index = 1; index < name.Length; ++index)
      {
        char c2 = name[index];
        if (!XmlConstructs.IsNameChar(c2))
        {
          err = (Exception) new XmlException("The character '" + (object) c2 + "' is not allowed in a Name", (Exception) null);
          return false;
        }
      }
      return true;
    }

    public static int IsValidName(string name)
    {
      if (name.Length == 0 || !XmlConstructs.IsFirstNameChar(name[0]))
        return 0;
      for (int index = 1; index < name.Length; ++index)
      {
        if (!XmlConstructs.IsNameChar(name[index]))
          return index;
      }
      return -1;
    }

    public static bool IsValidNCName(string ncName, out Exception err)
    {
      err = (Exception) null;
      if (ncName.Length == 0)
      {
        err = (Exception) new XmlException("NCName can not be an empty string", (Exception) null);
        return false;
      }
      char c1 = ncName[0];
      if (!XmlConstructs.IsNCNameStart(c1))
      {
        err = (Exception) new XmlException("The character '" + (object) c1 + "' cannot start a NCName", (Exception) null);
        return false;
      }
      for (int index = 1; index < ncName.Length; ++index)
      {
        char c2 = ncName[index];
        if (!XmlConstructs.IsNCNameChar(c2))
        {
          err = (Exception) new XmlException("The character '" + (object) c2 + "' is not allowed in a NCName", (Exception) null);
          return false;
        }
      }
      return true;
    }

    public static bool IsValidNmtoken(string nmtoken, out Exception err)
    {
      err = (Exception) null;
      if (nmtoken.Length == 0)
      {
        err = (Exception) new XmlException("NMTOKEN can not be an empty string", (Exception) null);
        return false;
      }
      for (int index = 0; index < nmtoken.Length; ++index)
      {
        char c = nmtoken[index];
        if (!XmlConstructs.IsNameChar(c))
        {
          err = (Exception) new XmlException("The character '" + (object) c + "' is not allowed in a NMTOKEN", (Exception) null);
          return false;
        }
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

    public static bool IsName(string str)
    {
      if (str.Length == 0 || !XmlConstructs.IsFirstNameChar(str[0]))
        return false;
      for (int index = 1; index < str.Length; ++index)
      {
        if (!XmlConstructs.IsNameChar(str[index]))
          return false;
      }
      return true;
    }

    public static bool IsNCName(string str)
    {
      if (str.Length == 0 || !XmlConstructs.IsFirstNameChar(str[0]))
        return false;
      for (int index = 0; index < str.Length; ++index)
      {
        if (!XmlConstructs.IsNCNameChar(str[index]))
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
        if (!XmlConstructs.IsNameChar(str[index]))
          return false;
      }
      return true;
    }

    public static bool IsWhitespace(string str)
    {
      for (int index = 0; index < str.Length; ++index)
      {
        if (!XmlConstructs.IsWhitespace(str[index]))
          return false;
      }
      return true;
    }

    public static int GetPredefinedEntity(string name)
    {
      string key = name;
      if (key != null)
      {
        // ISSUE: reference to a compiler-generated field
        if (XmlConstructs.\u003C\u003Ef__switch\u0024map48 == null)
        {
          // ISSUE: reference to a compiler-generated field
          XmlConstructs.\u003C\u003Ef__switch\u0024map48 = new Dictionary<string, int>(5)
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
        if (XmlConstructs.\u003C\u003Ef__switch\u0024map48.TryGetValue(key, out num))
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
