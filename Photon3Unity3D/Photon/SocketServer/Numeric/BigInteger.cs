// Decompiled with JetBrains decompiler
// Type: Photon.SocketServer.Numeric.BigInteger
// Assembly: Photon3Unity3D, Version=3.0.1.11, Culture=neutral, PublicKeyToken=null
// MVID: 5A081D50-91FF-4A78-BF8D-1F77FAA7ECB2
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\Photon3Unity3D.dll

using System;

namespace Photon.SocketServer.Numeric
{
  internal class BigInteger
  {
    private const int maxLength = 70;
    public static readonly int[] primesBelow2000 = new int[303]
    {
      2,
      3,
      5,
      7,
      11,
      13,
      17,
      19,
      23,
      29,
      31,
      37,
      41,
      43,
      47,
      53,
      59,
      61,
      67,
      71,
      73,
      79,
      83,
      89,
      97,
      101,
      103,
      107,
      109,
      113,
      (int) sbyte.MaxValue,
      131,
      137,
      139,
      149,
      151,
      157,
      163,
      167,
      173,
      179,
      181,
      191,
      193,
      197,
      199,
      211,
      223,
      227,
      229,
      233,
      239,
      241,
      251,
      257,
      263,
      269,
      271,
      277,
      281,
      283,
      293,
      307,
      311,
      313,
      317,
      331,
      337,
      347,
      349,
      353,
      359,
      367,
      373,
      379,
      383,
      389,
      397,
      401,
      409,
      419,
      421,
      431,
      433,
      439,
      443,
      449,
      457,
      461,
      463,
      467,
      479,
      487,
      491,
      499,
      503,
      509,
      521,
      523,
      541,
      547,
      557,
      563,
      569,
      571,
      577,
      587,
      593,
      599,
      601,
      607,
      613,
      617,
      619,
      631,
      641,
      643,
      647,
      653,
      659,
      661,
      673,
      677,
      683,
      691,
      701,
      709,
      719,
      727,
      733,
      739,
      743,
      751,
      757,
      761,
      769,
      773,
      787,
      797,
      809,
      811,
      821,
      823,
      827,
      829,
      839,
      853,
      857,
      859,
      863,
      877,
      881,
      883,
      887,
      907,
      911,
      919,
      929,
      937,
      941,
      947,
      953,
      967,
      971,
      977,
      983,
      991,
      997,
      1009,
      1013,
      1019,
      1021,
      1031,
      1033,
      1039,
      1049,
      1051,
      1061,
      1063,
      1069,
      1087,
      1091,
      1093,
      1097,
      1103,
      1109,
      1117,
      1123,
      1129,
      1151,
      1153,
      1163,
      1171,
      1181,
      1187,
      1193,
      1201,
      1213,
      1217,
      1223,
      1229,
      1231,
      1237,
      1249,
      1259,
      1277,
      1279,
      1283,
      1289,
      1291,
      1297,
      1301,
      1303,
      1307,
      1319,
      1321,
      1327,
      1361,
      1367,
      1373,
      1381,
      1399,
      1409,
      1423,
      1427,
      1429,
      1433,
      1439,
      1447,
      1451,
      1453,
      1459,
      1471,
      1481,
      1483,
      1487,
      1489,
      1493,
      1499,
      1511,
      1523,
      1531,
      1543,
      1549,
      1553,
      1559,
      1567,
      1571,
      1579,
      1583,
      1597,
      1601,
      1607,
      1609,
      1613,
      1619,
      1621,
      1627,
      1637,
      1657,
      1663,
      1667,
      1669,
      1693,
      1697,
      1699,
      1709,
      1721,
      1723,
      1733,
      1741,
      1747,
      1753,
      1759,
      1777,
      1783,
      1787,
      1789,
      1801,
      1811,
      1823,
      1831,
      1847,
      1861,
      1867,
      1871,
      1873,
      1877,
      1879,
      1889,
      1901,
      1907,
      1913,
      1931,
      1933,
      1949,
      1951,
      1973,
      1979,
      1987,
      1993,
      1997,
      1999
    };
    private uint[] data = (uint[]) null;
    public int dataLength;

    public BigInteger()
    {
      this.data = new uint[70];
      this.dataLength = 1;
    }

    public BigInteger(long value)
    {
      this.data = new uint[70];
      long num = value;
      for (this.dataLength = 0; value != 0L && this.dataLength < 70; ++this.dataLength)
      {
        this.data[this.dataLength] = (uint) ((ulong) value & (ulong) uint.MaxValue);
        value >>= 32;
      }
      if (num > 0L)
      {
        if (value != 0L || ((int) this.data[69] & int.MinValue) != 0)
          throw new ArithmeticException("Positive overflow in constructor.");
      }
      else if (num < 0L && (value != -1L || ((int) this.data[this.dataLength - 1] & int.MinValue) == 0))
        throw new ArithmeticException("Negative underflow in constructor.");
      if (this.dataLength != 0)
        return;
      this.dataLength = 1;
    }

    public BigInteger(ulong value)
    {
      this.data = new uint[70];
      for (this.dataLength = 0; value != 0UL && this.dataLength < 70; ++this.dataLength)
      {
        this.data[this.dataLength] = (uint) (value & (ulong) uint.MaxValue);
        value >>= 32;
      }
      if (value != 0UL || ((int) this.data[69] & int.MinValue) != 0)
        throw new ArithmeticException("Positive overflow in constructor.");
      if (this.dataLength != 0)
        return;
      this.dataLength = 1;
    }

    public BigInteger(BigInteger bi)
    {
      this.data = new uint[70];
      this.dataLength = bi.dataLength;
      for (int index = 0; index < this.dataLength; ++index)
        this.data[index] = bi.data[index];
    }

    public BigInteger(string value, int radix)
    {
      BigInteger bigInteger1 = new BigInteger(1L);
      BigInteger bigInteger2 = new BigInteger();
      value = value.ToUpper().Trim();
      int num1 = 0;
      if (value[0] == '-')
        num1 = 1;
      for (int index = value.Length - 1; index >= num1; --index)
      {
        int num2 = (int) value[index];
        int num3 = num2 < 48 || num2 > 57 ? (num2 < 65 || num2 > 90 ? 9999999 : num2 - 65 + 10) : num2 - 48;
        if (num3 >= radix)
          throw new ArithmeticException("Invalid string in constructor.");
        if (value[0] == '-')
          num3 = -num3;
        bigInteger2 += bigInteger1 * (BigInteger) num3;
        if (index - 1 >= num1)
          bigInteger1 *= (BigInteger) radix;
      }
      if (value[0] == '-')
      {
        if (((int) bigInteger2.data[69] & int.MinValue) == 0)
          throw new ArithmeticException("Negative underflow in constructor.");
      }
      else if (((int) bigInteger2.data[69] & int.MinValue) != 0)
        throw new ArithmeticException("Positive overflow in constructor.");
      this.data = new uint[70];
      for (int index = 0; index < bigInteger2.dataLength; ++index)
        this.data[index] = bigInteger2.data[index];
      this.dataLength = bigInteger2.dataLength;
    }

    public BigInteger(byte[] inData)
    {
      this.dataLength = inData.Length >> 2;
      int num = inData.Length & 3;
      if (num != 0)
        ++this.dataLength;
      if (this.dataLength > 70)
        throw new ArithmeticException("Byte overflow in constructor.");
      this.data = new uint[70];
      int index1 = inData.Length - 1;
      int index2 = 0;
      while (index1 >= 3)
      {
        this.data[index2] = (uint) (((int) inData[index1 - 3] << 24) + ((int) inData[index1 - 2] << 16) + ((int) inData[index1 - 1] << 8)) + (uint) inData[index1];
        index1 -= 4;
        ++index2;
      }
      switch (num)
      {
        case 1:
          this.data[this.dataLength - 1] = (uint) inData[0];
          break;
        case 2:
          this.data[this.dataLength - 1] = ((uint) inData[0] << 8) + (uint) inData[1];
          break;
        case 3:
          this.data[this.dataLength - 1] = (uint) (((int) inData[0] << 16) + ((int) inData[1] << 8)) + (uint) inData[2];
          break;
      }
      while (this.dataLength > 1 && this.data[this.dataLength - 1] == 0U)
        --this.dataLength;
    }

    public BigInteger(byte[] inData, int inLen)
    {
      this.dataLength = inLen >> 2;
      int num = inLen & 3;
      if (num != 0)
        ++this.dataLength;
      if (this.dataLength > 70 || inLen > inData.Length)
        throw new ArithmeticException("Byte overflow in constructor.");
      this.data = new uint[70];
      int index1 = inLen - 1;
      int index2 = 0;
      while (index1 >= 3)
      {
        this.data[index2] = (uint) (((int) inData[index1 - 3] << 24) + ((int) inData[index1 - 2] << 16) + ((int) inData[index1 - 1] << 8)) + (uint) inData[index1];
        index1 -= 4;
        ++index2;
      }
      switch (num)
      {
        case 1:
          this.data[this.dataLength - 1] = (uint) inData[0];
          break;
        case 2:
          this.data[this.dataLength - 1] = ((uint) inData[0] << 8) + (uint) inData[1];
          break;
        case 3:
          this.data[this.dataLength - 1] = (uint) (((int) inData[0] << 16) + ((int) inData[1] << 8)) + (uint) inData[2];
          break;
      }
      if (this.dataLength == 0)
        this.dataLength = 1;
      while (this.dataLength > 1 && this.data[this.dataLength - 1] == 0U)
        --this.dataLength;
    }

    public BigInteger(uint[] inData)
    {
      this.dataLength = inData.Length;
      if (this.dataLength > 70)
        throw new ArithmeticException("Byte overflow in constructor.");
      this.data = new uint[70];
      int index1 = this.dataLength - 1;
      int index2 = 0;
      while (index1 >= 0)
      {
        this.data[index2] = inData[index1];
        --index1;
        ++index2;
      }
      while (this.dataLength > 1 && this.data[this.dataLength - 1] == 0U)
        --this.dataLength;
    }

    public static implicit operator BigInteger(long value) => new BigInteger(value);

    public static implicit operator BigInteger(ulong value) => new BigInteger(value);

    public static implicit operator BigInteger(int value) => new BigInteger((long) value);

    public static implicit operator BigInteger(uint value) => new BigInteger((ulong) value);

    public static BigInteger operator +(BigInteger bi1, BigInteger bi2)
    {
      BigInteger bigInteger = new BigInteger();
      bigInteger.dataLength = bi1.dataLength > bi2.dataLength ? bi1.dataLength : bi2.dataLength;
      long num1 = 0;
      for (int index = 0; index < bigInteger.dataLength; ++index)
      {
        long num2 = (long) bi1.data[index] + (long) bi2.data[index] + num1;
        num1 = num2 >> 32;
        bigInteger.data[index] = (uint) ((ulong) num2 & (ulong) uint.MaxValue);
      }
      if (num1 != 0L && bigInteger.dataLength < 70)
      {
        bigInteger.data[bigInteger.dataLength] = (uint) num1;
        ++bigInteger.dataLength;
      }
      while (bigInteger.dataLength > 1 && bigInteger.data[bigInteger.dataLength - 1] == 0U)
        --bigInteger.dataLength;
      int index1 = 69;
      if (((int) bi1.data[index1] & int.MinValue) == ((int) bi2.data[index1] & int.MinValue) && ((int) bigInteger.data[index1] & int.MinValue) != ((int) bi1.data[index1] & int.MinValue))
        throw new ArithmeticException();
      return bigInteger;
    }

    public static BigInteger operator ++(BigInteger bi1)
    {
      BigInteger bigInteger = new BigInteger(bi1);
      long num1 = 1;
      int index1;
      for (index1 = 0; num1 != 0L && index1 < 70; ++index1)
      {
        long num2 = (long) bigInteger.data[index1] + 1L;
        bigInteger.data[index1] = (uint) ((ulong) num2 & (ulong) uint.MaxValue);
        num1 = num2 >> 32;
      }
      if (index1 > bigInteger.dataLength)
      {
        bigInteger.dataLength = index1;
      }
      else
      {
        while (bigInteger.dataLength > 1 && bigInteger.data[bigInteger.dataLength - 1] == 0U)
          --bigInteger.dataLength;
      }
      int index2 = 69;
      if (((int) bi1.data[index2] & int.MinValue) == 0 && ((int) bigInteger.data[index2] & int.MinValue) != ((int) bi1.data[index2] & int.MinValue))
        throw new ArithmeticException("Overflow in ++.");
      return bigInteger;
    }

    public static BigInteger operator -(BigInteger bi1, BigInteger bi2)
    {
      BigInteger bigInteger = new BigInteger();
      bigInteger.dataLength = bi1.dataLength > bi2.dataLength ? bi1.dataLength : bi2.dataLength;
      long num1 = 0;
      for (int index = 0; index < bigInteger.dataLength; ++index)
      {
        long num2 = (long) bi1.data[index] - (long) bi2.data[index] - num1;
        bigInteger.data[index] = (uint) ((ulong) num2 & (ulong) uint.MaxValue);
        num1 = num2 >= 0L ? 0L : 1L;
      }
      if (num1 != 0L)
      {
        for (int dataLength = bigInteger.dataLength; dataLength < 70; ++dataLength)
          bigInteger.data[dataLength] = uint.MaxValue;
        bigInteger.dataLength = 70;
      }
      while (bigInteger.dataLength > 1 && bigInteger.data[bigInteger.dataLength - 1] == 0U)
        --bigInteger.dataLength;
      int index1 = 69;
      if (((int) bi1.data[index1] & int.MinValue) != ((int) bi2.data[index1] & int.MinValue) && ((int) bigInteger.data[index1] & int.MinValue) != ((int) bi1.data[index1] & int.MinValue))
        throw new ArithmeticException();
      return bigInteger;
    }

    public static BigInteger operator --(BigInteger bi1)
    {
      BigInteger bigInteger = new BigInteger(bi1);
      bool flag = true;
      int index1;
      for (index1 = 0; flag && index1 < 70; ++index1)
      {
        long num = (long) bigInteger.data[index1] - 1L;
        bigInteger.data[index1] = (uint) ((ulong) num & (ulong) uint.MaxValue);
        if (num >= 0L)
          flag = false;
      }
      if (index1 > bigInteger.dataLength)
        bigInteger.dataLength = index1;
      while (bigInteger.dataLength > 1 && bigInteger.data[bigInteger.dataLength - 1] == 0U)
        --bigInteger.dataLength;
      int index2 = 69;
      if (((int) bi1.data[index2] & int.MinValue) != 0 && ((int) bigInteger.data[index2] & int.MinValue) != ((int) bi1.data[index2] & int.MinValue))
        throw new ArithmeticException("Underflow in --.");
      return bigInteger;
    }

    public static BigInteger operator *(BigInteger bi1, BigInteger bi2)
    {
      int index1 = 69;
      bool flag1 = false;
      bool flag2 = false;
      try
      {
        if (((int) bi1.data[index1] & int.MinValue) != 0)
        {
          flag1 = true;
          bi1 = -bi1;
        }
        if (((int) bi2.data[index1] & int.MinValue) != 0)
        {
          flag2 = true;
          bi2 = -bi2;
        }
      }
      catch (Exception ex)
      {
      }
      BigInteger bigInteger = new BigInteger();
      try
      {
        for (int index2 = 0; index2 < bi1.dataLength; ++index2)
        {
          if (bi1.data[index2] != 0U)
          {
            ulong num1 = 0;
            int index3 = 0;
            int index4 = index2;
            while (index3 < bi2.dataLength)
            {
              ulong num2 = (ulong) bi1.data[index2] * (ulong) bi2.data[index3] + (ulong) bigInteger.data[index4] + num1;
              bigInteger.data[index4] = (uint) (num2 & (ulong) uint.MaxValue);
              num1 = num2 >> 32;
              ++index3;
              ++index4;
            }
            if (num1 != 0UL)
              bigInteger.data[index2 + bi2.dataLength] = (uint) num1;
          }
        }
      }
      catch (Exception ex)
      {
        throw new ArithmeticException("Multiplication overflow.");
      }
      bigInteger.dataLength = bi1.dataLength + bi2.dataLength;
      if (bigInteger.dataLength > 70)
        bigInteger.dataLength = 70;
      while (bigInteger.dataLength > 1 && bigInteger.data[bigInteger.dataLength - 1] == 0U)
        --bigInteger.dataLength;
      if (((int) bigInteger.data[index1] & int.MinValue) != 0)
      {
        if (flag1 != flag2 && bigInteger.data[index1] == 2147483648U)
        {
          if (bigInteger.dataLength == 1)
            return bigInteger;
          bool flag3 = true;
          for (int index5 = 0; index5 < bigInteger.dataLength - 1 && flag3; ++index5)
          {
            if (bigInteger.data[index5] != 0U)
              flag3 = false;
          }
          if (flag3)
            return bigInteger;
        }
        throw new ArithmeticException("Multiplication overflow.");
      }
      return flag1 != flag2 ? -bigInteger : bigInteger;
    }

    public static BigInteger operator <<(BigInteger bi1, int shiftVal)
    {
      BigInteger bigInteger = new BigInteger(bi1);
      bigInteger.dataLength = BigInteger.shiftLeft(bigInteger.data, shiftVal);
      return bigInteger;
    }

    private static int shiftLeft(uint[] buffer, int shiftVal)
    {
      int num1 = 32;
      int length = buffer.Length;
      while (length > 1 && buffer[length - 1] == 0U)
        --length;
      for (int index1 = shiftVal; index1 > 0; index1 -= num1)
      {
        if (index1 < num1)
          num1 = index1;
        ulong num2 = 0;
        for (int index2 = 0; index2 < length; ++index2)
        {
          ulong num3 = (ulong) buffer[index2] << num1 | num2;
          buffer[index2] = (uint) (num3 & (ulong) uint.MaxValue);
          num2 = num3 >> 32;
        }
        if (num2 != 0UL && length + 1 <= buffer.Length)
        {
          buffer[length] = (uint) num2;
          ++length;
        }
      }
      return length;
    }

    public static BigInteger operator >>(BigInteger bi1, int shiftVal)
    {
      BigInteger bigInteger = new BigInteger(bi1);
      bigInteger.dataLength = BigInteger.shiftRight(bigInteger.data, shiftVal);
      if (((int) bi1.data[69] & int.MinValue) != 0)
      {
        for (int index = 69; index >= bigInteger.dataLength; --index)
          bigInteger.data[index] = uint.MaxValue;
        uint num = 2147483648;
        for (int index = 0; index < 32 && ((int) bigInteger.data[bigInteger.dataLength - 1] & (int) num) == 0; ++index)
        {
          bigInteger.data[bigInteger.dataLength - 1] |= num;
          num >>= 1;
        }
        bigInteger.dataLength = 70;
      }
      return bigInteger;
    }

    private static int shiftRight(uint[] buffer, int shiftVal)
    {
      int num1 = 32;
      int num2 = 0;
      int length = buffer.Length;
      while (length > 1 && buffer[length - 1] == 0U)
        --length;
      for (int index1 = shiftVal; index1 > 0; index1 -= num1)
      {
        if (index1 < num1)
        {
          num1 = index1;
          num2 = 32 - num1;
        }
        ulong num3 = 0;
        for (int index2 = length - 1; index2 >= 0; --index2)
        {
          ulong num4 = (ulong) buffer[index2] >> num1 | num3;
          num3 = (ulong) buffer[index2] << num2;
          buffer[index2] = (uint) num4;
        }
      }
      while (length > 1 && buffer[length - 1] == 0U)
        --length;
      return length;
    }

    public static BigInteger operator ~(BigInteger bi1)
    {
      BigInteger bigInteger = new BigInteger(bi1);
      for (int index = 0; index < 70; ++index)
        bigInteger.data[index] = ~bi1.data[index];
      bigInteger.dataLength = 70;
      while (bigInteger.dataLength > 1 && bigInteger.data[bigInteger.dataLength - 1] == 0U)
        --bigInteger.dataLength;
      return bigInteger;
    }

    public static BigInteger operator -(BigInteger bi1)
    {
      if (bi1.dataLength == 1 && bi1.data[0] == 0U)
        return new BigInteger();
      BigInteger bigInteger = new BigInteger(bi1);
      for (int index = 0; index < 70; ++index)
        bigInteger.data[index] = ~bi1.data[index];
      long num1 = 1;
      for (int index = 0; num1 != 0L && index < 70; ++index)
      {
        long num2 = (long) bigInteger.data[index] + 1L;
        bigInteger.data[index] = (uint) ((ulong) num2 & (ulong) uint.MaxValue);
        num1 = num2 >> 32;
      }
      if (((int) bi1.data[69] & int.MinValue) == ((int) bigInteger.data[69] & int.MinValue))
        throw new ArithmeticException("Overflow in negation.\n");
      bigInteger.dataLength = 70;
      while (bigInteger.dataLength > 1 && bigInteger.data[bigInteger.dataLength - 1] == 0U)
        --bigInteger.dataLength;
      return bigInteger;
    }

    public static bool operator ==(BigInteger bi1, BigInteger bi2) => bi1.Equals((object) bi2);

    public static bool operator !=(BigInteger bi1, BigInteger bi2) => !bi1.Equals((object) bi2);

    public override bool Equals(object o)
    {
      BigInteger bigInteger = (BigInteger) o;
      if (this.dataLength != bigInteger.dataLength)
        return false;
      for (int index = 0; index < this.dataLength; ++index)
      {
        if ((int) this.data[index] != (int) bigInteger.data[index])
          return false;
      }
      return true;
    }

    public override int GetHashCode() => this.ToString().GetHashCode();

    public static bool operator >(BigInteger bi1, BigInteger bi2)
    {
      int index1 = 69;
      if (((int) bi1.data[index1] & int.MinValue) != 0 && ((int) bi2.data[index1] & int.MinValue) == 0)
        return false;
      if (((int) bi1.data[index1] & int.MinValue) == 0 && ((int) bi2.data[index1] & int.MinValue) != 0)
        return true;
      int index2 = (bi1.dataLength > bi2.dataLength ? bi1.dataLength : bi2.dataLength) - 1;
      while (index2 >= 0 && (int) bi1.data[index2] == (int) bi2.data[index2])
        --index2;
      return index2 >= 0 && bi1.data[index2] > bi2.data[index2];
    }

    public static bool operator <(BigInteger bi1, BigInteger bi2)
    {
      int index1 = 69;
      if (((int) bi1.data[index1] & int.MinValue) != 0 && ((int) bi2.data[index1] & int.MinValue) == 0)
        return true;
      if (((int) bi1.data[index1] & int.MinValue) == 0 && ((int) bi2.data[index1] & int.MinValue) != 0)
        return false;
      int index2 = (bi1.dataLength > bi2.dataLength ? bi1.dataLength : bi2.dataLength) - 1;
      while (index2 >= 0 && (int) bi1.data[index2] == (int) bi2.data[index2])
        --index2;
      return index2 >= 0 && bi1.data[index2] < bi2.data[index2];
    }

    public static bool operator >=(BigInteger bi1, BigInteger bi2) => bi1 == bi2 || bi1 > bi2;

    public static bool operator <=(BigInteger bi1, BigInteger bi2) => bi1 == bi2 || bi1 < bi2;

    private static void multiByteDivide(
      BigInteger bi1,
      BigInteger bi2,
      BigInteger outQuotient,
      BigInteger outRemainder)
    {
      uint[] numArray = new uint[70];
      int length1 = bi1.dataLength + 1;
      uint[] buffer = new uint[length1];
      uint num1 = 2147483648;
      uint num2 = bi2.data[bi2.dataLength - 1];
      int shiftVal = 0;
      int num3 = 0;
      for (; num1 != 0U && ((int) num2 & (int) num1) == 0; num1 >>= 1)
        ++shiftVal;
      for (int index = 0; index < bi1.dataLength; ++index)
        buffer[index] = bi1.data[index];
      BigInteger.shiftLeft(buffer, shiftVal);
      bi2 <<= shiftVal;
      int num4 = length1 - bi2.dataLength;
      int index1 = length1 - 1;
      ulong num5 = (ulong) bi2.data[bi2.dataLength - 1];
      ulong num6 = (ulong) bi2.data[bi2.dataLength - 2];
      int length2 = bi2.dataLength + 1;
      uint[] inData = new uint[length2];
      for (; num4 > 0; --num4)
      {
        ulong num7 = ((ulong) buffer[index1] << 32) + (ulong) buffer[index1 - 1];
        ulong num8 = num7 / num5;
        ulong num9 = num7 % num5;
        bool flag = false;
        while (!flag)
        {
          flag = true;
          if (num8 == 4294967296UL || num8 * num6 > (num9 << 32) + (ulong) buffer[index1 - 2])
          {
            --num8;
            num9 += num5;
            if (num9 < 4294967296UL)
              flag = false;
          }
        }
        for (int index2 = 0; index2 < length2; ++index2)
          inData[index2] = buffer[index1 - index2];
        BigInteger bigInteger1 = new BigInteger(inData);
        BigInteger bigInteger2 = bi2 * (BigInteger) (long) num8;
        while (bigInteger2 > bigInteger1)
        {
          --num8;
          bigInteger2 -= bi2;
        }
        BigInteger bigInteger3 = bigInteger1 - bigInteger2;
        for (int index3 = 0; index3 < length2; ++index3)
          buffer[index1 - index3] = bigInteger3.data[bi2.dataLength - index3];
        numArray[num3++] = (uint) num8;
        --index1;
      }
      outQuotient.dataLength = num3;
      int index4 = 0;
      int index5 = outQuotient.dataLength - 1;
      while (index5 >= 0)
      {
        outQuotient.data[index4] = numArray[index5];
        --index5;
        ++index4;
      }
      for (; index4 < 70; ++index4)
        outQuotient.data[index4] = 0U;
      while (outQuotient.dataLength > 1 && outQuotient.data[outQuotient.dataLength - 1] == 0U)
        --outQuotient.dataLength;
      if (outQuotient.dataLength == 0)
        outQuotient.dataLength = 1;
      outRemainder.dataLength = BigInteger.shiftRight(buffer, shiftVal);
      int index6;
      for (index6 = 0; index6 < outRemainder.dataLength; ++index6)
        outRemainder.data[index6] = buffer[index6];
      for (; index6 < 70; ++index6)
        outRemainder.data[index6] = 0U;
    }

    private static void singleByteDivide(
      BigInteger bi1,
      BigInteger bi2,
      BigInteger outQuotient,
      BigInteger outRemainder)
    {
      uint[] numArray = new uint[70];
      int num1 = 0;
      for (int index = 0; index < 70; ++index)
        outRemainder.data[index] = bi1.data[index];
      outRemainder.dataLength = bi1.dataLength;
      while (outRemainder.dataLength > 1 && outRemainder.data[outRemainder.dataLength - 1] == 0U)
        --outRemainder.dataLength;
      ulong num2 = (ulong) bi2.data[0];
      int index1 = outRemainder.dataLength - 1;
      ulong num3 = (ulong) outRemainder.data[index1];
      if (num3 >= num2)
      {
        ulong num4 = num3 / num2;
        numArray[num1++] = (uint) num4;
        outRemainder.data[index1] = (uint) (num3 % num2);
      }
      ulong num5;
      for (int index2 = index1 - 1; index2 >= 0; outRemainder.data[index2--] = (uint) (num5 % num2))
      {
        num5 = ((ulong) outRemainder.data[index2 + 1] << 32) + (ulong) outRemainder.data[index2];
        ulong num6 = num5 / num2;
        numArray[num1++] = (uint) num6;
        outRemainder.data[index2 + 1] = 0U;
      }
      outQuotient.dataLength = num1;
      int index3 = 0;
      int index4 = outQuotient.dataLength - 1;
      while (index4 >= 0)
      {
        outQuotient.data[index3] = numArray[index4];
        --index4;
        ++index3;
      }
      for (; index3 < 70; ++index3)
        outQuotient.data[index3] = 0U;
      while (outQuotient.dataLength > 1 && outQuotient.data[outQuotient.dataLength - 1] == 0U)
        --outQuotient.dataLength;
      if (outQuotient.dataLength == 0)
        outQuotient.dataLength = 1;
      while (outRemainder.dataLength > 1 && outRemainder.data[outRemainder.dataLength - 1] == 0U)
        --outRemainder.dataLength;
    }

    public static BigInteger operator /(BigInteger bi1, BigInteger bi2)
    {
      BigInteger outQuotient = new BigInteger();
      BigInteger outRemainder = new BigInteger();
      int index = 69;
      bool flag1 = false;
      bool flag2 = false;
      if (((int) bi1.data[index] & int.MinValue) != 0)
      {
        bi1 = -bi1;
        flag2 = true;
      }
      if (((int) bi2.data[index] & int.MinValue) != 0)
      {
        bi2 = -bi2;
        flag1 = true;
      }
      if (bi1 < bi2)
        return outQuotient;
      if (bi2.dataLength == 1)
        BigInteger.singleByteDivide(bi1, bi2, outQuotient, outRemainder);
      else
        BigInteger.multiByteDivide(bi1, bi2, outQuotient, outRemainder);
      return flag2 != flag1 ? -outQuotient : outQuotient;
    }

    public static BigInteger operator %(BigInteger bi1, BigInteger bi2)
    {
      BigInteger outQuotient = new BigInteger();
      BigInteger outRemainder = new BigInteger(bi1);
      int index = 69;
      bool flag = false;
      if (((int) bi1.data[index] & int.MinValue) != 0)
      {
        bi1 = -bi1;
        flag = true;
      }
      if (((int) bi2.data[index] & int.MinValue) != 0)
        bi2 = -bi2;
      if (bi1 < bi2)
        return outRemainder;
      if (bi2.dataLength == 1)
        BigInteger.singleByteDivide(bi1, bi2, outQuotient, outRemainder);
      else
        BigInteger.multiByteDivide(bi1, bi2, outQuotient, outRemainder);
      return flag ? -outRemainder : outRemainder;
    }

    public static BigInteger operator &(BigInteger bi1, BigInteger bi2)
    {
      BigInteger bigInteger = new BigInteger();
      int num1 = bi1.dataLength > bi2.dataLength ? bi1.dataLength : bi2.dataLength;
      for (int index = 0; index < num1; ++index)
      {
        uint num2 = bi1.data[index] & bi2.data[index];
        bigInteger.data[index] = num2;
      }
      bigInteger.dataLength = 70;
      while (bigInteger.dataLength > 1 && bigInteger.data[bigInteger.dataLength - 1] == 0U)
        --bigInteger.dataLength;
      return bigInteger;
    }

    public static BigInteger operator |(BigInteger bi1, BigInteger bi2)
    {
      BigInteger bigInteger = new BigInteger();
      int num1 = bi1.dataLength > bi2.dataLength ? bi1.dataLength : bi2.dataLength;
      for (int index = 0; index < num1; ++index)
      {
        uint num2 = bi1.data[index] | bi2.data[index];
        bigInteger.data[index] = num2;
      }
      bigInteger.dataLength = 70;
      while (bigInteger.dataLength > 1 && bigInteger.data[bigInteger.dataLength - 1] == 0U)
        --bigInteger.dataLength;
      return bigInteger;
    }

    public static BigInteger operator ^(BigInteger bi1, BigInteger bi2)
    {
      BigInteger bigInteger = new BigInteger();
      int num1 = bi1.dataLength > bi2.dataLength ? bi1.dataLength : bi2.dataLength;
      for (int index = 0; index < num1; ++index)
      {
        uint num2 = bi1.data[index] ^ bi2.data[index];
        bigInteger.data[index] = num2;
      }
      bigInteger.dataLength = 70;
      while (bigInteger.dataLength > 1 && bigInteger.data[bigInteger.dataLength - 1] == 0U)
        --bigInteger.dataLength;
      return bigInteger;
    }

    public BigInteger max(BigInteger bi) => this > bi ? new BigInteger(this) : new BigInteger(bi);

    public BigInteger min(BigInteger bi) => this < bi ? new BigInteger(this) : new BigInteger(bi);

    public BigInteger abs() => ((int) this.data[69] & int.MinValue) != 0 ? -this : new BigInteger(this);

    public override string ToString() => this.ToString(10);

    public string ToString(int radix)
    {
      if (radix < 2 || radix > 36)
        throw new ArgumentException("Radix must be >= 2 and <= 36");
      string str1 = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
      string str2 = "";
      BigInteger bi1 = this;
      bool flag = false;
      if (((int) bi1.data[69] & int.MinValue) != 0)
      {
        flag = true;
        try
        {
          bi1 = -bi1;
        }
        catch (Exception ex)
        {
        }
      }
      BigInteger outQuotient = new BigInteger();
      BigInteger outRemainder = new BigInteger();
      BigInteger bi2 = new BigInteger((long) radix);
      if (bi1.dataLength == 1 && bi1.data[0] == 0U)
      {
        str2 = "0";
      }
      else
      {
        for (; bi1.dataLength > 1 || bi1.dataLength == 1 && bi1.data[0] != 0U; bi1 = outQuotient)
        {
          BigInteger.singleByteDivide(bi1, bi2, outQuotient, outRemainder);
          str2 = outRemainder.data[0] >= 10U ? str1[(int) outRemainder.data[0] - 10].ToString() + str2 : outRemainder.data[0].ToString() + str2;
        }
        if (flag)
          str2 = "-" + str2;
      }
      return str2;
    }

    public string ToHexString()
    {
      string hexString = this.data[this.dataLength - 1].ToString("X");
      for (int index = this.dataLength - 2; index >= 0; --index)
        hexString += this.data[index].ToString("X8");
      return hexString;
    }

    public BigInteger ModPow(BigInteger exp, BigInteger n)
    {
      if (((int) exp.data[69] & int.MinValue) != 0)
        throw new ArithmeticException("Positive exponents only.");
      BigInteger bigInteger1 = (BigInteger) 1;
      bool flag = false;
      BigInteger bigInteger2;
      if (((int) this.data[69] & int.MinValue) != 0)
      {
        bigInteger2 = -this % n;
        flag = true;
      }
      else
        bigInteger2 = this % n;
      if (((int) n.data[69] & int.MinValue) != 0)
        n = -n;
      BigInteger bigInteger3 = new BigInteger();
      int index1 = n.dataLength << 1;
      bigInteger3.data[index1] = 1U;
      bigInteger3.dataLength = index1 + 1;
      BigInteger constant = bigInteger3 / n;
      int num1 = exp.bitCount();
      int num2 = 0;
      for (int index2 = 0; index2 < exp.dataLength; ++index2)
      {
        uint num3 = 1;
        for (int index3 = 0; index3 < 32; ++index3)
        {
          if (((int) exp.data[index2] & (int) num3) != 0)
            bigInteger1 = this.BarrettReduction(bigInteger1 * bigInteger2, n, constant);
          num3 <<= 1;
          bigInteger2 = this.BarrettReduction(bigInteger2 * bigInteger2, n, constant);
          if (bigInteger2.dataLength == 1 && bigInteger2.data[0] == 1U)
            return flag && ((int) exp.data[0] & 1) != 0 ? -bigInteger1 : bigInteger1;
          ++num2;
          if (num2 == num1)
            break;
        }
      }
      return flag && ((int) exp.data[0] & 1) != 0 ? -bigInteger1 : bigInteger1;
    }

    private BigInteger BarrettReduction(BigInteger x, BigInteger n, BigInteger constant)
    {
      int dataLength = n.dataLength;
      int index1 = dataLength + 1;
      int num1 = dataLength - 1;
      BigInteger bigInteger1 = new BigInteger();
      int index2 = num1;
      int index3 = 0;
      while (index2 < x.dataLength)
      {
        bigInteger1.data[index3] = x.data[index2];
        ++index2;
        ++index3;
      }
      bigInteger1.dataLength = x.dataLength - num1;
      if (bigInteger1.dataLength <= 0)
        bigInteger1.dataLength = 1;
      BigInteger bigInteger2 = bigInteger1 * constant;
      BigInteger bigInteger3 = new BigInteger();
      int index4 = index1;
      int index5 = 0;
      while (index4 < bigInteger2.dataLength)
      {
        bigInteger3.data[index5] = bigInteger2.data[index4];
        ++index4;
        ++index5;
      }
      bigInteger3.dataLength = bigInteger2.dataLength - index1;
      if (bigInteger3.dataLength <= 0)
        bigInteger3.dataLength = 1;
      BigInteger bigInteger4 = new BigInteger();
      int num2 = x.dataLength > index1 ? index1 : x.dataLength;
      for (int index6 = 0; index6 < num2; ++index6)
        bigInteger4.data[index6] = x.data[index6];
      bigInteger4.dataLength = num2;
      BigInteger bigInteger5 = new BigInteger();
      for (int index7 = 0; index7 < bigInteger3.dataLength; ++index7)
      {
        if (bigInteger3.data[index7] != 0U)
        {
          ulong num3 = 0;
          int index8 = index7;
          for (int index9 = 0; index9 < n.dataLength && index8 < index1; ++index8)
          {
            ulong num4 = (ulong) bigInteger3.data[index7] * (ulong) n.data[index9] + (ulong) bigInteger5.data[index8] + num3;
            bigInteger5.data[index8] = (uint) (num4 & (ulong) uint.MaxValue);
            num3 = num4 >> 32;
            ++index9;
          }
          if (index8 < index1)
            bigInteger5.data[index8] = (uint) num3;
        }
      }
      bigInteger5.dataLength = index1;
      while (bigInteger5.dataLength > 1 && bigInteger5.data[bigInteger5.dataLength - 1] == 0U)
        --bigInteger5.dataLength;
      BigInteger bigInteger6 = bigInteger4 - bigInteger5;
      if (((int) bigInteger6.data[69] & int.MinValue) != 0)
      {
        BigInteger bigInteger7 = new BigInteger();
        bigInteger7.data[index1] = 1U;
        bigInteger7.dataLength = index1 + 1;
        bigInteger6 += bigInteger7;
      }
      while (bigInteger6 >= n)
        bigInteger6 -= n;
      return bigInteger6;
    }

    public BigInteger gcd(BigInteger bi)
    {
      BigInteger bigInteger1 = ((int) this.data[69] & int.MinValue) == 0 ? this : -this;
      BigInteger bigInteger2 = ((int) bi.data[69] & int.MinValue) == 0 ? bi : -bi;
      BigInteger bigInteger3 = bigInteger2;
      while (bigInteger1.dataLength > 1 || bigInteger1.dataLength == 1 && bigInteger1.data[0] != 0U)
      {
        bigInteger3 = bigInteger1;
        bigInteger1 = bigInteger2 % bigInteger1;
        bigInteger2 = bigInteger3;
      }
      return bigInteger3;
    }

    public static BigInteger GenerateRandom(int bits)
    {
      BigInteger random = new BigInteger();
      random.genRandomBits(bits, new Random());
      return random;
    }

    public void genRandomBits(int bits, Random rand)
    {
      int num1 = bits >> 5;
      int num2 = bits & 31;
      if (num2 != 0)
        ++num1;
      if (num1 > 70)
        throw new ArithmeticException("Number of required bits > maxLength.");
      for (int index = 0; index < num1; ++index)
        this.data[index] = (uint) (rand.NextDouble() * 4294967296.0);
      for (int index = num1; index < 70; ++index)
        this.data[index] = 0U;
      if (num2 != 0)
      {
        uint num3 = (uint) (1 << num2 - 1);
        this.data[num1 - 1] |= num3;
        uint num4 = uint.MaxValue >> 32 - num2;
        this.data[num1 - 1] &= num4;
      }
      else
        this.data[num1 - 1] |= 2147483648U;
      this.dataLength = num1;
      if (this.dataLength != 0)
        return;
      this.dataLength = 1;
    }

    public int bitCount()
    {
      while (this.dataLength > 1 && this.data[this.dataLength - 1] == 0U)
        --this.dataLength;
      uint num1 = this.data[this.dataLength - 1];
      uint num2 = 2147483648;
      int num3;
      for (num3 = 32; num3 > 0 && ((int) num1 & (int) num2) == 0; num2 >>= 1)
        --num3;
      return num3 + (this.dataLength - 1 << 5);
    }

    public bool FermatLittleTest(int confidence)
    {
      BigInteger bigInteger1 = ((int) this.data[69] & int.MinValue) == 0 ? this : -this;
      if (bigInteger1.dataLength == 1)
      {
        if (bigInteger1.data[0] == 0U || bigInteger1.data[0] == 1U)
          return false;
        if (bigInteger1.data[0] == 2U || bigInteger1.data[0] == 3U)
          return true;
      }
      if (((int) bigInteger1.data[0] & 1) == 0)
        return false;
      int num = bigInteger1.bitCount();
      BigInteger bigInteger2 = new BigInteger();
      BigInteger exp = bigInteger1 - new BigInteger(1L);
      Random rand = new Random();
      for (int index = 0; index < confidence; ++index)
      {
        bool flag = false;
        while (!flag)
        {
          int bits = 0;
          while (bits < 2)
            bits = (int) (rand.NextDouble() * (double) num);
          bigInteger2.genRandomBits(bits, rand);
          int dataLength = bigInteger2.dataLength;
          if (dataLength > 1 || dataLength == 1 && bigInteger2.data[0] != 1U)
            flag = true;
        }
        BigInteger bigInteger3 = bigInteger2.gcd(bigInteger1);
        if (bigInteger3.dataLength == 1 && bigInteger3.data[0] != 1U)
          return false;
        BigInteger bigInteger4 = bigInteger2.ModPow(exp, bigInteger1);
        int dataLength1 = bigInteger4.dataLength;
        if (dataLength1 > 1 || dataLength1 == 1 && bigInteger4.data[0] != 1U)
          return false;
      }
      return true;
    }

    public bool RabinMillerTest(int confidence)
    {
      BigInteger bigInteger1 = ((int) this.data[69] & int.MinValue) == 0 ? this : -this;
      if (bigInteger1.dataLength == 1)
      {
        if (bigInteger1.data[0] == 0U || bigInteger1.data[0] == 1U)
          return false;
        if (bigInteger1.data[0] == 2U || bigInteger1.data[0] == 3U)
          return true;
      }
      if (((int) bigInteger1.data[0] & 1) == 0)
        return false;
      BigInteger bigInteger2 = bigInteger1 - new BigInteger(1L);
      int num1 = 0;
      for (int index1 = 0; index1 < bigInteger2.dataLength; ++index1)
      {
        uint num2 = 1;
        for (int index2 = 0; index2 < 32; ++index2)
        {
          if (((int) bigInteger2.data[index1] & (int) num2) != 0)
          {
            index1 = bigInteger2.dataLength;
            break;
          }
          num2 <<= 1;
          ++num1;
        }
      }
      BigInteger exp = bigInteger2 >> num1;
      int num3 = bigInteger1.bitCount();
      BigInteger bigInteger3 = new BigInteger();
      Random rand = new Random();
      for (int index3 = 0; index3 < confidence; ++index3)
      {
        bool flag1 = false;
        while (!flag1)
        {
          int bits = 0;
          while (bits < 2)
            bits = (int) (rand.NextDouble() * (double) num3);
          bigInteger3.genRandomBits(bits, rand);
          int dataLength = bigInteger3.dataLength;
          if (dataLength > 1 || dataLength == 1 && bigInteger3.data[0] != 1U)
            flag1 = true;
        }
        BigInteger bigInteger4 = bigInteger3.gcd(bigInteger1);
        if (bigInteger4.dataLength == 1 && bigInteger4.data[0] != 1U)
          return false;
        BigInteger bigInteger5 = bigInteger3.ModPow(exp, bigInteger1);
        bool flag2 = false;
        if (bigInteger5.dataLength == 1 && bigInteger5.data[0] == 1U)
          flag2 = true;
        for (int index4 = 0; !flag2 && index4 < num1; ++index4)
        {
          if (bigInteger5 == bigInteger2)
          {
            flag2 = true;
            break;
          }
          bigInteger5 = bigInteger5 * bigInteger5 % bigInteger1;
        }
        if (!flag2)
          return false;
      }
      return true;
    }

    public bool SolovayStrassenTest(int confidence)
    {
      BigInteger bigInteger1 = ((int) this.data[69] & int.MinValue) == 0 ? this : -this;
      if (bigInteger1.dataLength == 1)
      {
        if (bigInteger1.data[0] == 0U || bigInteger1.data[0] == 1U)
          return false;
        if (bigInteger1.data[0] == 2U || bigInteger1.data[0] == 3U)
          return true;
      }
      if (((int) bigInteger1.data[0] & 1) == 0)
        return false;
      int num = bigInteger1.bitCount();
      BigInteger a = new BigInteger();
      BigInteger bigInteger2 = bigInteger1 - (BigInteger) 1;
      BigInteger exp = bigInteger2 >> 1;
      Random rand = new Random();
      for (int index = 0; index < confidence; ++index)
      {
        bool flag = false;
        while (!flag)
        {
          int bits = 0;
          while (bits < 2)
            bits = (int) (rand.NextDouble() * (double) num);
          a.genRandomBits(bits, rand);
          int dataLength = a.dataLength;
          if (dataLength > 1 || dataLength == 1 && a.data[0] != 1U)
            flag = true;
        }
        BigInteger bigInteger3 = a.gcd(bigInteger1);
        if (bigInteger3.dataLength == 1 && bigInteger3.data[0] != 1U)
          return false;
        BigInteger bigInteger4 = a.ModPow(exp, bigInteger1);
        if (bigInteger4 == bigInteger2)
          bigInteger4 = (BigInteger) -1;
        BigInteger bigInteger5 = (BigInteger) BigInteger.Jacobi(a, bigInteger1);
        if (bigInteger4 != bigInteger5)
          return false;
      }
      return true;
    }

    public bool LucasStrongTest()
    {
      BigInteger thisVal = ((int) this.data[69] & int.MinValue) == 0 ? this : -this;
      if (thisVal.dataLength == 1)
      {
        if (thisVal.data[0] == 0U || thisVal.data[0] == 1U)
          return false;
        if (thisVal.data[0] == 2U || thisVal.data[0] == 3U)
          return true;
      }
      return ((int) thisVal.data[0] & 1) != 0 && this.LucasStrongTestHelper(thisVal);
    }

    private bool LucasStrongTestHelper(BigInteger thisVal)
    {
      long a = 5;
      long num1 = -1;
      long num2 = 0;
      bool flag1 = false;
      while (!flag1)
      {
        int num3;
        switch (BigInteger.Jacobi((BigInteger) a, thisVal))
        {
          case -1:
            flag1 = true;
            goto label_11;
          case 0:
            num3 = !((BigInteger) Math.Abs(a) < thisVal) ? 1 : 0;
            break;
          default:
            num3 = 1;
            break;
        }
        if (num3 == 0)
          return false;
        if (num2 == 20L)
        {
          BigInteger bigInteger = thisVal.sqrt();
          if (bigInteger * bigInteger == thisVal)
            return false;
        }
        a = (Math.Abs(a) + 2L) * num1;
        num1 = -num1;
label_11:
        ++num2;
      }
      long num4 = 1L - a >> 2;
      BigInteger bigInteger1 = thisVal + (BigInteger) 1;
      int num5 = 0;
      for (int index1 = 0; index1 < bigInteger1.dataLength; ++index1)
      {
        uint num6 = 1;
        for (int index2 = 0; index2 < 32; ++index2)
        {
          if (((int) bigInteger1.data[index1] & (int) num6) != 0)
          {
            index1 = bigInteger1.dataLength;
            break;
          }
          num6 <<= 1;
          ++num5;
        }
      }
      BigInteger k = bigInteger1 >> num5;
      BigInteger bigInteger2 = new BigInteger();
      int index3 = thisVal.dataLength << 1;
      bigInteger2.data[index3] = 1U;
      bigInteger2.dataLength = index3 + 1;
      BigInteger constant = bigInteger2 / thisVal;
      BigInteger[] bigIntegerArray1 = BigInteger.LucasSequenceHelper((BigInteger) 1, (BigInteger) num4, k, thisVal, constant, 0);
      bool flag2 = false;
      if (bigIntegerArray1[0].dataLength == 1 && bigIntegerArray1[0].data[0] == 0U || bigIntegerArray1[1].dataLength == 1 && bigIntegerArray1[1].data[0] == 0U)
        flag2 = true;
      for (int index4 = 1; index4 < num5; ++index4)
      {
        if (!flag2)
        {
          bigIntegerArray1[1] = thisVal.BarrettReduction(bigIntegerArray1[1] * bigIntegerArray1[1], thisVal, constant);
          bigIntegerArray1[1] = (bigIntegerArray1[1] - (bigIntegerArray1[2] << 1)) % thisVal;
          if (bigIntegerArray1[1].dataLength == 1 && bigIntegerArray1[1].data[0] == 0U)
            flag2 = true;
        }
        bigIntegerArray1[2] = thisVal.BarrettReduction(bigIntegerArray1[2] * bigIntegerArray1[2], thisVal, constant);
      }
      if (flag2)
      {
        BigInteger bigInteger3 = thisVal.gcd((BigInteger) num4);
        if (bigInteger3.dataLength == 1 && bigInteger3.data[0] == 1U)
        {
          if (((int) bigIntegerArray1[2].data[69] & int.MinValue) != 0)
          {
            BigInteger[] bigIntegerArray2;
            (bigIntegerArray2 = bigIntegerArray1)[2] = bigIntegerArray2[2] + thisVal;
          }
          BigInteger bigInteger4 = (BigInteger) (num4 * (long) BigInteger.Jacobi((BigInteger) num4, thisVal)) % thisVal;
          if (((int) bigInteger4.data[69] & int.MinValue) != 0)
            bigInteger4 += thisVal;
          if (bigIntegerArray1[2] != bigInteger4)
            flag2 = false;
        }
      }
      return flag2;
    }

    public bool isProbablePrime(int confidence)
    {
      BigInteger bigInteger1 = ((int) this.data[69] & int.MinValue) == 0 ? this : -this;
      for (int index = 0; index < BigInteger.primesBelow2000.Length; ++index)
      {
        BigInteger bigInteger2 = (BigInteger) BigInteger.primesBelow2000[index];
        if (!(bigInteger2 >= bigInteger1))
        {
          if ((bigInteger1 % bigInteger2).IntValue() == 0)
            return false;
        }
        else
          break;
      }
      return bigInteger1.RabinMillerTest(confidence);
    }

    public bool isProbablePrime()
    {
      BigInteger bigInteger1 = ((int) this.data[69] & int.MinValue) == 0 ? this : -this;
      if (bigInteger1.dataLength == 1)
      {
        if (bigInteger1.data[0] == 0U || bigInteger1.data[0] == 1U)
          return false;
        if (bigInteger1.data[0] == 2U || bigInteger1.data[0] == 3U)
          return true;
      }
      if (((int) bigInteger1.data[0] & 1) == 0)
        return false;
      for (int index = 0; index < BigInteger.primesBelow2000.Length; ++index)
      {
        BigInteger bigInteger2 = (BigInteger) BigInteger.primesBelow2000[index];
        if (!(bigInteger2 >= bigInteger1))
        {
          if ((bigInteger1 % bigInteger2).IntValue() == 0)
            return false;
        }
        else
          break;
      }
      BigInteger bigInteger3 = bigInteger1 - new BigInteger(1L);
      int num1 = 0;
      for (int index1 = 0; index1 < bigInteger3.dataLength; ++index1)
      {
        uint num2 = 1;
        for (int index2 = 0; index2 < 32; ++index2)
        {
          if (((int) bigInteger3.data[index1] & (int) num2) != 0)
          {
            index1 = bigInteger3.dataLength;
            break;
          }
          num2 <<= 1;
          ++num1;
        }
      }
      BigInteger exp = bigInteger3 >> num1;
      bigInteger1.bitCount();
      BigInteger bigInteger4 = ((BigInteger) 2).ModPow(exp, bigInteger1);
      bool flag = false;
      if (bigInteger4.dataLength == 1 && bigInteger4.data[0] == 1U)
        flag = true;
      for (int index = 0; !flag && index < num1; ++index)
      {
        if (bigInteger4 == bigInteger3)
        {
          flag = true;
          break;
        }
        bigInteger4 = bigInteger4 * bigInteger4 % bigInteger1;
      }
      if (flag)
        flag = this.LucasStrongTestHelper(bigInteger1);
      return flag;
    }

    public int IntValue() => (int) this.data[0];

    public long LongValue()
    {
      long num = (long) this.data[0];
      try
      {
        num |= (long) this.data[1] << 32;
      }
      catch (Exception ex)
      {
        if (((int) this.data[0] & int.MinValue) != 0)
          num = (long) (int) this.data[0];
      }
      return num;
    }

    public static int Jacobi(BigInteger a, BigInteger b)
    {
      if (((int) b.data[0] & 1) == 0)
        throw new ArgumentException("Jacobi defined only for odd integers.");
      if (a >= b)
        a %= b;
      if (a.dataLength == 1 && a.data[0] == 0U)
        return 0;
      if (a.dataLength == 1 && a.data[0] == 1U)
        return 1;
      if (a < (BigInteger) 0)
        return ((int) (b - (BigInteger) 1).data[0] & 2) == 0 ? BigInteger.Jacobi(-a, b) : -BigInteger.Jacobi(-a, b);
      int num1 = 0;
      for (int index1 = 0; index1 < a.dataLength; ++index1)
      {
        uint num2 = 1;
        for (int index2 = 0; index2 < 32; ++index2)
        {
          if (((int) a.data[index1] & (int) num2) != 0)
          {
            index1 = a.dataLength;
            break;
          }
          num2 <<= 1;
          ++num1;
        }
      }
      BigInteger b1 = a >> num1;
      int num3 = 1;
      if ((num1 & 1) != 0 && (((int) b.data[0] & 7) == 3 || ((int) b.data[0] & 7) == 5))
        num3 = -1;
      if (((int) b.data[0] & 3) == 3 && ((int) b1.data[0] & 3) == 3)
        num3 = -num3;
      return b1.dataLength == 1 && b1.data[0] == 1U ? num3 : num3 * BigInteger.Jacobi(b % b1, b1);
    }

    public static BigInteger genPseudoPrime(int bits, int confidence, Random rand)
    {
      BigInteger bigInteger = new BigInteger();
      for (bool flag = false; !flag; flag = bigInteger.isProbablePrime(confidence))
      {
        bigInteger.genRandomBits(bits, rand);
        bigInteger.data[0] |= 1U;
      }
      return bigInteger;
    }

    public BigInteger genCoPrime(int bits, Random rand)
    {
      bool flag = false;
      BigInteger bigInteger1 = new BigInteger();
      while (!flag)
      {
        bigInteger1.genRandomBits(bits, rand);
        BigInteger bigInteger2 = bigInteger1.gcd(this);
        if (bigInteger2.dataLength == 1 && bigInteger2.data[0] == 1U)
          flag = true;
      }
      return bigInteger1;
    }

    public BigInteger modInverse(BigInteger modulus)
    {
      BigInteger[] bigIntegerArray1 = new BigInteger[2]
      {
        (BigInteger) 0,
        (BigInteger) 1
      };
      BigInteger[] bigIntegerArray2 = new BigInteger[2];
      BigInteger[] bigIntegerArray3 = new BigInteger[2]
      {
        (BigInteger) 0,
        (BigInteger) 0
      };
      int num = 0;
      BigInteger bi1 = modulus;
      BigInteger bi2 = this;
      while (bi2.dataLength > 1 || bi2.dataLength == 1 && bi2.data[0] != 0U)
      {
        BigInteger outQuotient = new BigInteger();
        BigInteger outRemainder = new BigInteger();
        if (num > 1)
        {
          BigInteger bigInteger = (bigIntegerArray1[0] - bigIntegerArray1[1] * bigIntegerArray2[0]) % modulus;
          bigIntegerArray1[0] = bigIntegerArray1[1];
          bigIntegerArray1[1] = bigInteger;
        }
        if (bi2.dataLength == 1)
          BigInteger.singleByteDivide(bi1, bi2, outQuotient, outRemainder);
        else
          BigInteger.multiByteDivide(bi1, bi2, outQuotient, outRemainder);
        bigIntegerArray2[0] = bigIntegerArray2[1];
        bigIntegerArray3[0] = bigIntegerArray3[1];
        bigIntegerArray2[1] = outQuotient;
        bigIntegerArray3[1] = outRemainder;
        bi1 = bi2;
        bi2 = outRemainder;
        ++num;
      }
      if (bigIntegerArray3[0].dataLength > 1 || bigIntegerArray3[0].dataLength == 1 && bigIntegerArray3[0].data[0] != 1U)
        throw new ArithmeticException("No inverse!");
      BigInteger bigInteger1 = (bigIntegerArray1[0] - bigIntegerArray1[1] * bigIntegerArray2[0]) % modulus;
      if (((int) bigInteger1.data[69] & int.MinValue) != 0)
        bigInteger1 += modulus;
      return bigInteger1;
    }

    public byte[] GetBytes()
    {
      if (this == (BigInteger) 0)
        return new byte[1];
      int num1 = this.bitCount();
      int length = num1 >> 3;
      if ((num1 & 7) != 0)
        ++length;
      byte[] bytes = new byte[length];
      int num2 = length & 3;
      if (num2 == 0)
        num2 = 4;
      int num3 = 0;
      for (int index1 = this.dataLength - 1; index1 >= 0; --index1)
      {
        uint num4 = this.data[index1];
        for (int index2 = num2 - 1; index2 >= 0; --index2)
        {
          bytes[num3 + index2] = (byte) (num4 & (uint) byte.MaxValue);
          num4 >>= 8;
        }
        num3 += num2;
        num2 = 4;
      }
      return bytes;
    }

    public void setBit(uint bitNum)
    {
      uint index = bitNum >> 5;
      uint num = 1U << (int) (byte) (bitNum & 31U);
      this.data[(IntPtr) index] |= num;
      if ((long) index < (long) this.dataLength)
        return;
      this.dataLength = (int) index + 1;
    }

    public void unsetBit(uint bitNum)
    {
      uint index = bitNum >> 5;
      if ((long) index >= (long) this.dataLength)
        return;
      uint num = uint.MaxValue ^ 1U << (int) (byte) (bitNum & 31U);
      this.data[(IntPtr) index] &= num;
      if (this.dataLength > 1 && this.data[this.dataLength - 1] == 0U)
        --this.dataLength;
    }

    public BigInteger sqrt()
    {
      uint num1 = (uint) this.bitCount();
      uint num2 = ((int) num1 & 1) == 0 ? num1 >> 1 : (num1 >> 1) + 1U;
      uint num3 = num2 >> 5;
      byte num4 = (byte) (num2 & 31U);
      BigInteger bigInteger = new BigInteger();
      uint num5;
      if (num4 == (byte) 0)
      {
        num5 = 2147483648U;
      }
      else
      {
        num5 = 1U << (int) num4;
        ++num3;
      }
      bigInteger.dataLength = (int) num3;
      for (int index = (int) num3 - 1; index >= 0; --index)
      {
        for (; num5 != 0U; num5 >>= 1)
        {
          bigInteger.data[index] ^= num5;
          if (bigInteger * bigInteger > this)
            bigInteger.data[index] ^= num5;
        }
        num5 = 2147483648U;
      }
      return bigInteger;
    }

    public static BigInteger[] LucasSequence(
      BigInteger P,
      BigInteger Q,
      BigInteger k,
      BigInteger n)
    {
      if (k.dataLength == 1 && k.data[0] == 0U)
        return new BigInteger[3]
        {
          (BigInteger) 0,
          (BigInteger) 2 % n,
          (BigInteger) 1 % n
        };
      BigInteger bigInteger = new BigInteger();
      int index1 = n.dataLength << 1;
      bigInteger.data[index1] = 1U;
      bigInteger.dataLength = index1 + 1;
      BigInteger constant = bigInteger / n;
      int s = 0;
      for (int index2 = 0; index2 < k.dataLength; ++index2)
      {
        uint num = 1;
        for (int index3 = 0; index3 < 32; ++index3)
        {
          if (((int) k.data[index2] & (int) num) != 0)
          {
            index2 = k.dataLength;
            break;
          }
          num <<= 1;
          ++s;
        }
      }
      BigInteger k1 = k >> s;
      return BigInteger.LucasSequenceHelper(P, Q, k1, n, constant, s);
    }

    private static BigInteger[] LucasSequenceHelper(
      BigInteger P,
      BigInteger Q,
      BigInteger k,
      BigInteger n,
      BigInteger constant,
      int s)
    {
      BigInteger[] bigIntegerArray = new BigInteger[3];
      if (((int) k.data[0] & 1) == 0)
        throw new ArgumentException("Argument k must be odd.");
      uint num = (uint) (1 << (k.bitCount() & 31) - 1);
      BigInteger bigInteger1 = (BigInteger) 2 % n;
      BigInteger bigInteger2 = (BigInteger) 1 % n;
      BigInteger bigInteger3 = P % n;
      BigInteger bigInteger4 = bigInteger2;
      bool flag = true;
      for (int index = k.dataLength - 1; index >= 0; --index)
      {
        for (; num != 0U && (index != 0 || num != 1U); num >>= 1)
        {
          if (((int) k.data[index] & (int) num) != 0)
          {
            bigInteger4 = bigInteger4 * bigInteger3 % n;
            bigInteger1 = (bigInteger1 * bigInteger3 - P * bigInteger2) % n;
            bigInteger3 = (n.BarrettReduction(bigInteger3 * bigInteger3, n, constant) - (bigInteger2 * Q << 1)) % n;
            if (flag)
              flag = false;
            else
              bigInteger2 = n.BarrettReduction(bigInteger2 * bigInteger2, n, constant);
            bigInteger2 = bigInteger2 * Q % n;
          }
          else
          {
            bigInteger4 = (bigInteger4 * bigInteger1 - bigInteger2) % n;
            bigInteger3 = (bigInteger1 * bigInteger3 - P * bigInteger2) % n;
            bigInteger1 = (n.BarrettReduction(bigInteger1 * bigInteger1, n, constant) - (bigInteger2 << 1)) % n;
            if (flag)
            {
              bigInteger2 = Q % n;
              flag = false;
            }
            else
              bigInteger2 = n.BarrettReduction(bigInteger2 * bigInteger2, n, constant);
          }
        }
        num = 2147483648U;
      }
      BigInteger bigInteger5 = (bigInteger4 * bigInteger1 - bigInteger2) % n;
      BigInteger bigInteger6 = (bigInteger1 * bigInteger3 - P * bigInteger2) % n;
      if (flag)
        flag = false;
      else
        bigInteger2 = n.BarrettReduction(bigInteger2 * bigInteger2, n, constant);
      BigInteger bigInteger7 = bigInteger2 * Q % n;
      for (int index = 0; index < s; ++index)
      {
        bigInteger5 = bigInteger5 * bigInteger6 % n;
        bigInteger6 = (bigInteger6 * bigInteger6 - (bigInteger7 << 1)) % n;
        if (flag)
        {
          bigInteger7 = Q % n;
          flag = false;
        }
        else
          bigInteger7 = n.BarrettReduction(bigInteger7 * bigInteger7, n, constant);
      }
      bigIntegerArray[0] = bigInteger5;
      bigIntegerArray[1] = bigInteger6;
      bigIntegerArray[2] = bigInteger7;
      return bigIntegerArray;
    }

    public static void MulDivTest(int rounds)
    {
      Random random = new Random();
      byte[] inData1 = new byte[64];
      byte[] inData2 = new byte[64];
      for (int index1 = 0; index1 < rounds; ++index1)
      {
        int inLen1 = 0;
        while (inLen1 == 0)
          inLen1 = (int) (random.NextDouble() * 65.0);
        int inLen2 = 0;
        while (inLen2 == 0)
          inLen2 = (int) (random.NextDouble() * 65.0);
        bool flag1 = false;
        while (!flag1)
        {
          for (int index2 = 0; index2 < 64; ++index2)
          {
            inData1[index2] = index2 >= inLen1 ? (byte) 0 : (byte) (random.NextDouble() * 256.0);
            if (inData1[index2] != (byte) 0)
              flag1 = true;
          }
        }
        bool flag2 = false;
        while (!flag2)
        {
          for (int index3 = 0; index3 < 64; ++index3)
          {
            inData2[index3] = index3 >= inLen2 ? (byte) 0 : (byte) (random.NextDouble() * 256.0);
            if (inData2[index3] != (byte) 0)
              flag2 = true;
          }
        }
        while (inData1[0] == (byte) 0)
          inData1[0] = (byte) (random.NextDouble() * 256.0);
        while (inData2[0] == (byte) 0)
          inData2[0] = (byte) (random.NextDouble() * 256.0);
        Console.WriteLine(index1);
        BigInteger bigInteger1 = new BigInteger(inData1, inLen1);
        BigInteger bigInteger2 = new BigInteger(inData2, inLen2);
        BigInteger bigInteger3 = bigInteger1 / bigInteger2;
        BigInteger bigInteger4 = bigInteger1 % bigInteger2;
        BigInteger bigInteger5 = bigInteger3 * bigInteger2 + bigInteger4;
        if (bigInteger5 != bigInteger1)
        {
          Console.WriteLine("Error at " + (object) index1);
          Console.WriteLine(bigInteger1.ToString() + "\n");
          Console.WriteLine(bigInteger2.ToString() + "\n");
          Console.WriteLine(bigInteger3.ToString() + "\n");
          Console.WriteLine(bigInteger4.ToString() + "\n");
          Console.WriteLine(bigInteger5.ToString() + "\n");
          break;
        }
      }
    }

    public static void RSATest(int rounds)
    {
      Random random = new Random(1);
      byte[] inData = new byte[64];
      BigInteger exp1 = new BigInteger("a932b948feed4fb2b692609bd22164fc9edb59fae7880cc1eaff7b3c9626b7e5b241c27a974833b2622ebe09beb451917663d47232488f23a117fc97720f1e7", 16);
      BigInteger exp2 = new BigInteger("4adf2f7a89da93248509347d2ae506d683dd3a16357e859a980c4f77a4e2f7a01fae289f13a851df6e9db5adaa60bfd2b162bbbe31f7c8f828261a6839311929d2cef4f864dde65e556ce43c89bbbf9f1ac5511315847ce9cc8dc92470a747b8792d6a83b0092d2e5ebaf852c85cacf34278efa99160f2f8aa7ee7214de07b7", 16);
      BigInteger n = new BigInteger("e8e77781f36a7b3188d711c2190b560f205a52391b3479cdb99fa010745cbeba5f2adc08e1de6bf38398a0487c4a73610d94ec36f17f3f46ad75e17bc1adfec99839589f45f95ccc94cb2a5c500b477eb3323d8cfab0c8458c96f0147a45d27e45a4d11d54d77684f65d48f15fafcc1ba208e71e921b9bd9017c16a5231af7f", 16);
      Console.WriteLine("e =\n" + exp1.ToString(10));
      Console.WriteLine("\nd =\n" + exp2.ToString(10));
      Console.WriteLine("\nn =\n" + n.ToString(10) + "\n");
      for (int index1 = 0; index1 < rounds; ++index1)
      {
        int inLen = 0;
        while (inLen == 0)
          inLen = (int) (random.NextDouble() * 65.0);
        bool flag = false;
        while (!flag)
        {
          for (int index2 = 0; index2 < 64; ++index2)
          {
            inData[index2] = index2 >= inLen ? (byte) 0 : (byte) (random.NextDouble() * 256.0);
            if (inData[index2] != (byte) 0)
              flag = true;
          }
        }
        while (inData[0] == (byte) 0)
          inData[0] = (byte) (random.NextDouble() * 256.0);
        Console.Write("Round = " + (object) index1);
        BigInteger bigInteger = new BigInteger(inData, inLen);
        if (bigInteger.ModPow(exp1, n).ModPow(exp2, n) != bigInteger)
        {
          Console.WriteLine("\nError at round " + (object) index1);
          Console.WriteLine(bigInteger.ToString() + "\n");
          break;
        }
        Console.WriteLine(" <PASSED>.");
      }
    }

    public static void RSATest2(int rounds)
    {
      Random rand = new Random();
      byte[] inData1 = new byte[64];
      byte[] inData2 = new byte[64]
      {
        (byte) 133,
        (byte) 132,
        (byte) 100,
        (byte) 253,
        (byte) 112,
        (byte) 106,
        (byte) 159,
        (byte) 240,
        (byte) 148,
        (byte) 12,
        (byte) 62,
        (byte) 44,
        (byte) 116,
        (byte) 52,
        (byte) 5,
        (byte) 201,
        (byte) 85,
        (byte) 179,
        (byte) 133,
        (byte) 50,
        (byte) 152,
        (byte) 113,
        (byte) 249,
        (byte) 65,
        (byte) 33,
        (byte) 95,
        (byte) 2,
        (byte) 158,
        (byte) 234,
        (byte) 86,
        (byte) 141,
        (byte) 140,
        (byte) 68,
        (byte) 204,
        (byte) 238,
        (byte) 238,
        (byte) 61,
        (byte) 44,
        (byte) 157,
        (byte) 44,
        (byte) 18,
        (byte) 65,
        (byte) 30,
        (byte) 241,
        (byte) 197,
        (byte) 50,
        (byte) 195,
        (byte) 170,
        (byte) 49,
        (byte) 74,
        (byte) 82,
        (byte) 216,
        (byte) 232,
        (byte) 175,
        (byte) 66,
        (byte) 244,
        (byte) 114,
        (byte) 161,
        (byte) 42,
        (byte) 13,
        (byte) 151,
        (byte) 177,
        (byte) 49,
        (byte) 179
      };
      byte[] inData3 = new byte[64]
      {
        (byte) 153,
        (byte) 152,
        (byte) 202,
        (byte) 184,
        (byte) 94,
        (byte) 215,
        (byte) 229,
        (byte) 220,
        (byte) 40,
        (byte) 92,
        (byte) 111,
        (byte) 14,
        (byte) 21,
        (byte) 9,
        (byte) 89,
        (byte) 110,
        (byte) 132,
        (byte) 243,
        (byte) 129,
        (byte) 205,
        (byte) 222,
        (byte) 66,
        (byte) 220,
        (byte) 147,
        (byte) 194,
        (byte) 122,
        (byte) 98,
        (byte) 172,
        (byte) 108,
        (byte) 175,
        (byte) 222,
        (byte) 116,
        (byte) 227,
        (byte) 203,
        (byte) 96,
        (byte) 32,
        (byte) 56,
        (byte) 156,
        (byte) 33,
        (byte) 195,
        (byte) 220,
        (byte) 200,
        (byte) 162,
        (byte) 77,
        (byte) 198,
        (byte) 42,
        (byte) 53,
        (byte) 127,
        (byte) 243,
        (byte) 169,
        (byte) 232,
        (byte) 29,
        (byte) 123,
        (byte) 44,
        (byte) 120,
        (byte) 250,
        (byte) 184,
        (byte) 2,
        (byte) 85,
        (byte) 128,
        (byte) 155,
        (byte) 194,
        (byte) 165,
        (byte) 203
      };
      BigInteger bigInteger1 = new BigInteger(inData2);
      BigInteger bigInteger2 = new BigInteger(inData3);
      BigInteger modulus = (bigInteger1 - (BigInteger) 1) * (bigInteger2 - (BigInteger) 1);
      BigInteger n = bigInteger1 * bigInteger2;
      for (int index1 = 0; index1 < rounds; ++index1)
      {
        BigInteger exp1 = modulus.genCoPrime(512, rand);
        BigInteger exp2 = exp1.modInverse(modulus);
        Console.WriteLine("\ne =\n" + exp1.ToString(10));
        Console.WriteLine("\nd =\n" + exp2.ToString(10));
        Console.WriteLine("\nn =\n" + n.ToString(10) + "\n");
        int inLen = 0;
        while (inLen == 0)
          inLen = (int) (rand.NextDouble() * 65.0);
        bool flag = false;
        while (!flag)
        {
          for (int index2 = 0; index2 < 64; ++index2)
          {
            inData1[index2] = index2 >= inLen ? (byte) 0 : (byte) (rand.NextDouble() * 256.0);
            if (inData1[index2] != (byte) 0)
              flag = true;
          }
        }
        while (inData1[0] == (byte) 0)
          inData1[0] = (byte) (rand.NextDouble() * 256.0);
        Console.Write("Round = " + (object) index1);
        BigInteger bigInteger3 = new BigInteger(inData1, inLen);
        if (bigInteger3.ModPow(exp1, n).ModPow(exp2, n) != bigInteger3)
        {
          Console.WriteLine("\nError at round " + (object) index1);
          Console.WriteLine(bigInteger3.ToString() + "\n");
          break;
        }
        Console.WriteLine(" <PASSED>.");
      }
    }

    public static void SqrtTest(int rounds)
    {
      Random rand = new Random();
      for (int index = 0; index < rounds; ++index)
      {
        int bits = 0;
        while (bits == 0)
          bits = (int) (rand.NextDouble() * 1024.0);
        Console.Write("Round = " + (object) index);
        BigInteger bigInteger1 = new BigInteger();
        bigInteger1.genRandomBits(bits, rand);
        BigInteger bigInteger2 = bigInteger1.sqrt();
        if ((bigInteger2 + (BigInteger) 1) * (bigInteger2 + (BigInteger) 1) <= bigInteger1)
        {
          Console.WriteLine("\nError at round " + (object) index);
          Console.WriteLine(bigInteger1.ToString() + "\n");
          break;
        }
        Console.WriteLine(" <PASSED>.");
      }
    }

    public static void Main(string[] args)
    {
      byte[] inData = new byte[65]
      {
        (byte) 0,
        (byte) 133,
        (byte) 132,
        (byte) 100,
        (byte) 253,
        (byte) 112,
        (byte) 106,
        (byte) 159,
        (byte) 240,
        (byte) 148,
        (byte) 12,
        (byte) 62,
        (byte) 44,
        (byte) 116,
        (byte) 52,
        (byte) 5,
        (byte) 201,
        (byte) 85,
        (byte) 179,
        (byte) 133,
        (byte) 50,
        (byte) 152,
        (byte) 113,
        (byte) 249,
        (byte) 65,
        (byte) 33,
        (byte) 95,
        (byte) 2,
        (byte) 158,
        (byte) 234,
        (byte) 86,
        (byte) 141,
        (byte) 140,
        (byte) 68,
        (byte) 204,
        (byte) 238,
        (byte) 238,
        (byte) 61,
        (byte) 44,
        (byte) 157,
        (byte) 44,
        (byte) 18,
        (byte) 65,
        (byte) 30,
        (byte) 241,
        (byte) 197,
        (byte) 50,
        (byte) 195,
        (byte) 170,
        (byte) 49,
        (byte) 74,
        (byte) 82,
        (byte) 216,
        (byte) 232,
        (byte) 175,
        (byte) 66,
        (byte) 244,
        (byte) 114,
        (byte) 161,
        (byte) 42,
        (byte) 13,
        (byte) 151,
        (byte) 177,
        (byte) 49,
        (byte) 179
      };
      byte[] numArray = new byte[65]
      {
        (byte) 0,
        (byte) 153,
        (byte) 152,
        (byte) 202,
        (byte) 184,
        (byte) 94,
        (byte) 215,
        (byte) 229,
        (byte) 220,
        (byte) 40,
        (byte) 92,
        (byte) 111,
        (byte) 14,
        (byte) 21,
        (byte) 9,
        (byte) 89,
        (byte) 110,
        (byte) 132,
        (byte) 243,
        (byte) 129,
        (byte) 205,
        (byte) 222,
        (byte) 66,
        (byte) 220,
        (byte) 147,
        (byte) 194,
        (byte) 122,
        (byte) 98,
        (byte) 172,
        (byte) 108,
        (byte) 175,
        (byte) 222,
        (byte) 116,
        (byte) 227,
        (byte) 203,
        (byte) 96,
        (byte) 32,
        (byte) 56,
        (byte) 156,
        (byte) 33,
        (byte) 195,
        (byte) 220,
        (byte) 200,
        (byte) 162,
        (byte) 77,
        (byte) 198,
        (byte) 42,
        (byte) 53,
        (byte) 127,
        (byte) 243,
        (byte) 169,
        (byte) 232,
        (byte) 29,
        (byte) 123,
        (byte) 44,
        (byte) 120,
        (byte) 250,
        (byte) 184,
        (byte) 2,
        (byte) 85,
        (byte) 128,
        (byte) 155,
        (byte) 194,
        (byte) 165,
        (byte) 203
      };
      Console.WriteLine("List of primes < 2000\n---------------------");
      int num1 = 100;
      int num2 = 0;
      for (int index = 0; index < 2000; ++index)
      {
        if (index >= num1)
        {
          Console.WriteLine();
          num1 += 100;
        }
        if (new BigInteger((long) -index).isProbablePrime())
        {
          Console.Write(index.ToString() + ", ");
          ++num2;
        }
      }
      Console.WriteLine("\nCount = " + (object) num2);
      BigInteger bigInteger = new BigInteger(inData);
      Console.WriteLine("\n\nPrimality testing for\n" + bigInteger.ToString() + "\n");
      Console.WriteLine("SolovayStrassenTest(5) = " + (object) bigInteger.SolovayStrassenTest(5));
      Console.WriteLine("RabinMillerTest(5) = " + (object) bigInteger.RabinMillerTest(5));
      Console.WriteLine("FermatLittleTest(5) = " + (object) bigInteger.FermatLittleTest(5));
      Console.WriteLine("isProbablePrime() = " + (object) bigInteger.isProbablePrime());
      Console.Write("\nGenerating 512-bits random pseudoprime. . .");
      Console.WriteLine("\n" + (object) BigInteger.genPseudoPrime(512, 5, new Random()));
    }
  }
}
