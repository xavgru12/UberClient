// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.XslDecimalFormat
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.Xsl
{
  internal class XslDecimalFormat
  {
    private NumberFormatInfo info = new NumberFormatInfo();
    private char digit = '#';
    private char zeroDigit = '0';
    private char patternSeparator = ';';
    private string baseUri;
    private int lineNumber;
    private int linePosition;
    public static readonly XslDecimalFormat Default = new XslDecimalFormat();

    private XslDecimalFormat()
    {
    }

    public XslDecimalFormat(Compiler c)
    {
      XPathNavigator input = c.Input;
      if (input is IXmlLineInfo xmlLineInfo)
      {
        this.lineNumber = xmlLineInfo.LineNumber;
        this.linePosition = xmlLineInfo.LinePosition;
      }
      this.baseUri = input.BaseURI;
      if (!input.MoveToFirstAttribute())
        return;
      do
      {
        if (!(input.NamespaceURI != string.Empty))
        {
          string localName = input.LocalName;
          if (localName != null)
          {
            // ISSUE: reference to a compiler-generated field
            if (XslDecimalFormat.\u003C\u003Ef__switch\u0024map1B == null)
            {
              // ISSUE: reference to a compiler-generated field
              XslDecimalFormat.\u003C\u003Ef__switch\u0024map1B = new Dictionary<string, int>(11)
              {
                {
                  "name",
                  0
                },
                {
                  "decimal-separator",
                  1
                },
                {
                  "grouping-separator",
                  2
                },
                {
                  "infinity",
                  3
                },
                {
                  "minus-sign",
                  4
                },
                {
                  "NaN",
                  5
                },
                {
                  "percent",
                  6
                },
                {
                  "per-mille",
                  7
                },
                {
                  nameof (digit),
                  8
                },
                {
                  "zero-digit",
                  9
                },
                {
                  "pattern-separator",
                  10
                }
              };
            }
            int num;
            // ISSUE: reference to a compiler-generated field
            if (XslDecimalFormat.\u003C\u003Ef__switch\u0024map1B.TryGetValue(localName, out num))
            {
              switch (num)
              {
                case 1:
                  if (input.Value.Length != 1)
                    throw new XsltCompileException("XSLT decimal-separator value must be exact one character", (Exception) null, input);
                  this.info.NumberDecimalSeparator = input.Value;
                  break;
                case 2:
                  if (input.Value.Length != 1)
                    throw new XsltCompileException("XSLT grouping-separator value must be exact one character", (Exception) null, input);
                  this.info.NumberGroupSeparator = input.Value;
                  break;
                case 3:
                  this.info.PositiveInfinitySymbol = input.Value;
                  break;
                case 4:
                  if (input.Value.Length != 1)
                    throw new XsltCompileException("XSLT minus-sign value must be exact one character", (Exception) null, input);
                  this.info.NegativeSign = input.Value;
                  break;
                case 5:
                  this.info.NaNSymbol = input.Value;
                  break;
                case 6:
                  if (input.Value.Length != 1)
                    throw new XsltCompileException("XSLT percent value must be exact one character", (Exception) null, input);
                  this.info.PercentSymbol = input.Value;
                  break;
                case 7:
                  if (input.Value.Length != 1)
                    throw new XsltCompileException("XSLT per-mille value must be exact one character", (Exception) null, input);
                  this.info.PerMilleSymbol = input.Value;
                  break;
                case 8:
                  if (input.Value.Length != 1)
                    throw new XsltCompileException("XSLT digit value must be exact one character", (Exception) null, input);
                  this.digit = input.Value[0];
                  break;
                case 9:
                  if (input.Value.Length != 1)
                    throw new XsltCompileException("XSLT zero-digit value must be exact one character", (Exception) null, input);
                  this.zeroDigit = input.Value[0];
                  break;
                case 10:
                  if (input.Value.Length != 1)
                    throw new XsltCompileException("XSLT pattern-separator value must be exact one character", (Exception) null, input);
                  this.patternSeparator = input.Value[0];
                  break;
              }
            }
          }
        }
      }
      while (input.MoveToNextAttribute());
      input.MoveToParent();
      this.info.NegativeInfinitySymbol = this.info.NegativeSign + this.info.PositiveInfinitySymbol;
    }

    public char Digit => this.digit;

    public char ZeroDigit => this.zeroDigit;

    public NumberFormatInfo Info => this.info;

    public char PatternSeparator => this.patternSeparator;

    public void CheckSameAs(XslDecimalFormat other)
    {
      if ((int) this.digit != (int) other.digit || (int) this.patternSeparator != (int) other.patternSeparator || (int) this.zeroDigit != (int) other.zeroDigit || this.info.NumberDecimalSeparator != other.info.NumberDecimalSeparator || this.info.NumberGroupSeparator != other.info.NumberGroupSeparator || this.info.PositiveInfinitySymbol != other.info.PositiveInfinitySymbol || this.info.NegativeSign != other.info.NegativeSign || this.info.NaNSymbol != other.info.NaNSymbol || this.info.PercentSymbol != other.info.PercentSymbol || this.info.PerMilleSymbol != other.info.PerMilleSymbol)
        throw new XsltCompileException((Exception) null, other.baseUri, other.lineNumber, other.linePosition);
    }

    public string FormatNumber(double number, string pattern) => this.ParsePatternSet(pattern).FormatNumber(number);

    private DecimalFormatPatternSet ParsePatternSet(string pattern) => new DecimalFormatPatternSet(pattern, this);
  }
}
