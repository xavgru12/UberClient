// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.DecimalFormatPattern
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System;
using System.Globalization;
using System.Text;

namespace Mono.Xml.Xsl
{
  internal class DecimalFormatPattern
  {
    public string Prefix = string.Empty;
    public string Suffix = string.Empty;
    public string NumberPart;
    private NumberFormatInfo info;
    private StringBuilder builder = new StringBuilder();

    internal int ParsePattern(int start, string pattern, XslDecimalFormat format)
    {
      if (start == 0)
      {
        this.info = format.Info;
      }
      else
      {
        this.info = format.Info.Clone() as NumberFormatInfo;
        this.info.NegativeSign = string.Empty;
      }
      int pattern1 = start;
      while (pattern1 < pattern.Length && (int) pattern[pattern1] != (int) format.ZeroDigit && (int) pattern[pattern1] != (int) format.Digit && (int) pattern[pattern1] != (int) format.Info.CurrencySymbol[0])
        ++pattern1;
      this.Prefix = pattern.Substring(start, pattern1 - start);
      if (pattern1 == pattern.Length)
        return pattern1;
      int number = this.ParseNumber(pattern1, pattern, format);
      int startIndex = number;
      while (number < pattern.Length && (int) pattern[number] != (int) format.ZeroDigit && (int) pattern[number] != (int) format.Digit && (int) pattern[number] != (int) format.PatternSeparator && (int) pattern[number] != (int) format.Info.CurrencySymbol[0])
        ++number;
      this.Suffix = pattern.Substring(startIndex, number - startIndex);
      return number;
    }

    private int ParseNumber(int start, string pattern, XslDecimalFormat format)
    {
      int index;
      for (index = start; index < pattern.Length; ++index)
      {
        if ((int) pattern[index] == (int) format.Digit)
          this.builder.Append('#');
        else if ((int) pattern[index] == (int) format.Info.NumberGroupSeparator[0])
          this.builder.Append(',');
        else
          break;
      }
      for (; index < pattern.Length; ++index)
      {
        if ((int) pattern[index] == (int) format.ZeroDigit)
          this.builder.Append('0');
        else if ((int) pattern[index] == (int) format.Info.NumberGroupSeparator[0])
          this.builder.Append(',');
        else
          break;
      }
      if (index < pattern.Length)
      {
        if ((int) pattern[index] == (int) format.Info.NumberDecimalSeparator[0])
        {
          this.builder.Append('.');
          ++index;
        }
        while (index < pattern.Length && (int) pattern[index] == (int) format.ZeroDigit)
        {
          ++index;
          this.builder.Append('0');
        }
        while (index < pattern.Length && (int) pattern[index] == (int) format.Digit)
        {
          ++index;
          this.builder.Append('#');
        }
      }
      if (index + 1 < pattern.Length && pattern[index] == 'E' && (int) pattern[index + 1] == (int) format.ZeroDigit)
      {
        index += 2;
        this.builder.Append("E0");
        while (index < pattern.Length && (int) pattern[index] == (int) format.ZeroDigit)
        {
          ++index;
          this.builder.Append('0');
        }
      }
      if (index < pattern.Length)
      {
        if ((int) pattern[index] == (int) this.info.PercentSymbol[0])
          this.builder.Append('%');
        else if ((int) pattern[index] == (int) this.info.PerMilleSymbol[0])
        {
          this.builder.Append('‰');
        }
        else
        {
          if ((int) pattern[index] == (int) this.info.CurrencySymbol[0])
            throw new ArgumentException("Currency symbol is not supported for number format pattern string.");
          --index;
        }
        ++index;
      }
      this.NumberPart = this.builder.ToString();
      return index;
    }

    public string FormatNumber(double number)
    {
      this.builder.Length = 0;
      this.builder.Append(this.Prefix);
      this.builder.Append(number.ToString(this.NumberPart, (IFormatProvider) this.info));
      this.builder.Append(this.Suffix);
      return this.builder.ToString();
    }
  }
}
