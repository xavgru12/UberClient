// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.DecimalFormatPatternSet
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System;

namespace Mono.Xml.Xsl
{
  internal class DecimalFormatPatternSet
  {
    private DecimalFormatPattern positivePattern;
    private DecimalFormatPattern negativePattern;

    public DecimalFormatPatternSet(string pattern, XslDecimalFormat decimalFormat) => this.Parse(pattern, decimalFormat);

    private void Parse(string pattern, XslDecimalFormat format)
    {
      if (pattern.Length == 0)
        throw new ArgumentException("Invalid number format pattern string.");
      this.positivePattern = new DecimalFormatPattern();
      this.negativePattern = this.positivePattern;
      int pattern1 = this.positivePattern.ParsePattern(0, pattern, format);
      if (pattern1 >= pattern.Length || (int) pattern[pattern1] != (int) format.PatternSeparator)
        return;
      int start = pattern1 + 1;
      this.negativePattern = new DecimalFormatPattern();
      if (this.negativePattern.ParsePattern(start, pattern, format) < pattern.Length)
        throw new ArgumentException("Number format pattern string ends with extraneous part.");
    }

    public string FormatNumber(double number) => number >= 0.0 ? this.positivePattern.FormatNumber(number) : this.negativePattern.FormatNumber(number);
  }
}
