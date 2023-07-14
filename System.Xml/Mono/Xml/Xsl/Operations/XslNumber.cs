// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.Operations.XslNumber
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using Mono.Xml.XPath;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.Xsl.Operations
{
  internal class XslNumber : XslCompiledElement
  {
    private XslNumberingLevel level;
    private Pattern count;
    private Pattern from;
    private XPathExpression value;
    private XslAvt format;
    private XslAvt lang;
    private XslAvt letterValue;
    private XslAvt groupingSeparator;
    private XslAvt groupingSize;

    public XslNumber(Compiler c)
      : base(c)
    {
    }

    public static double Round(double n)
    {
      double num = Math.Floor(n);
      return n - num >= 0.5 ? num + 1.0 : num;
    }

    protected override void Compile(Compiler c)
    {
      if (c.Debugger != null)
        c.Debugger.DebugCompile(this.DebugInput);
      c.CheckExtraAttributes("number", "level", "count", "from", "value", "format", "lang", "letter-value", "grouping-separator", "grouping-size");
      string attribute = c.GetAttribute("level");
      if (attribute != null)
      {
        // ISSUE: reference to a compiler-generated field
        if (XslNumber.\u003C\u003Ef__switch\u0024mapB == null)
        {
          // ISSUE: reference to a compiler-generated field
          XslNumber.\u003C\u003Ef__switch\u0024mapB = new Dictionary<string, int>(4)
          {
            {
              "single",
              0
            },
            {
              "multiple",
              1
            },
            {
              "any",
              2
            },
            {
              string.Empty,
              3
            }
          };
        }
        int num;
        // ISSUE: reference to a compiler-generated field
        if (XslNumber.\u003C\u003Ef__switch\u0024mapB.TryGetValue(attribute, out num))
        {
          switch (num)
          {
            case 0:
              this.level = XslNumberingLevel.Single;
              goto label_11;
            case 1:
              this.level = XslNumberingLevel.Multiple;
              goto label_11;
            case 2:
              this.level = XslNumberingLevel.Any;
              goto label_11;
          }
        }
      }
      this.level = XslNumberingLevel.Single;
label_11:
      this.count = c.CompilePattern(c.GetAttribute("count"), c.Input);
      this.from = c.CompilePattern(c.GetAttribute("from"), c.Input);
      this.value = (XPathExpression) c.CompileExpression(c.GetAttribute("value"));
      this.format = c.ParseAvtAttribute("format");
      this.lang = c.ParseAvtAttribute("lang");
      this.letterValue = c.ParseAvtAttribute("letter-value");
      this.groupingSeparator = c.ParseAvtAttribute("grouping-separator");
      this.groupingSize = c.ParseAvtAttribute("grouping-size");
    }

    public override void Evaluate(XslTransformProcessor p)
    {
      if (p.Debugger != null)
        p.Debugger.DebugExecute(p, this.DebugInput);
      string format = this.GetFormat(p);
      if (!(format != string.Empty))
        return;
      p.Out.WriteString(format);
    }

    private XslNumber.XslNumberFormatter GetNumberFormatter(XslTransformProcessor p)
    {
      string format = "1";
      string lang = (string) null;
      string letterValue = (string) null;
      char minValue = char.MinValue;
      Decimal groupingSize = 0M;
      if (this.format != null)
        format = this.format.Evaluate(p);
      if (this.lang != null)
        lang = this.lang.Evaluate(p);
      if (this.letterValue != null)
        letterValue = this.letterValue.Evaluate(p);
      if (this.groupingSeparator != null)
        minValue = this.groupingSeparator.Evaluate(p)[0];
      if (this.groupingSize != null)
        groupingSize = Decimal.Parse(this.groupingSize.Evaluate(p), (IFormatProvider) CultureInfo.InvariantCulture);
      if (groupingSize > 2147483647M || groupingSize < 1M)
        groupingSize = 0M;
      return new XslNumber.XslNumberFormatter(format, lang, letterValue, minValue, (int) groupingSize);
    }

    private string GetFormat(XslTransformProcessor p)
    {
      XslNumber.XslNumberFormatter numberFormatter = this.GetNumberFormatter(p);
      if (this.value != null)
      {
        double number = p.EvaluateNumber(this.value);
        return numberFormatter.Format(number);
      }
      switch (this.level)
      {
        case XslNumberingLevel.Single:
          int num1 = this.NumberSingle(p);
          return numberFormatter.Format((double) num1, num1 != 0);
        case XslNumberingLevel.Multiple:
          return numberFormatter.Format(this.NumberMultiple(p));
        case XslNumberingLevel.Any:
          int num2 = this.NumberAny(p);
          return numberFormatter.Format((double) num2, num2 != 0);
        default:
          throw new XsltException("Should not get here", (Exception) null, p.CurrentNode);
      }
    }

    private int[] NumberMultiple(XslTransformProcessor p)
    {
      ArrayList arrayList = new ArrayList();
      XPathNavigator xpathNavigator = p.CurrentNode.Clone();
      bool flag = false;
      while (!this.MatchesFrom(xpathNavigator, p))
      {
        if (this.MatchesCount(xpathNavigator, p))
        {
          int num = 1;
          while (xpathNavigator.MoveToPrevious())
          {
            if (this.MatchesCount(xpathNavigator, p))
              ++num;
          }
          arrayList.Add((object) num);
        }
        if (!xpathNavigator.MoveToParent())
          goto label_10;
      }
      flag = true;
label_10:
      if (!flag)
        return new int[0];
      int[] numArray = new int[arrayList.Count];
      int count = arrayList.Count;
      for (int index = 0; index < arrayList.Count; ++index)
        numArray[--count] = (int) arrayList[index];
      return numArray;
    }

    private int NumberAny(XslTransformProcessor p)
    {
      int num = 0;
      XPathNavigator xpathNavigator = p.CurrentNode.Clone();
      xpathNavigator.MoveToRoot();
      bool flag = this.from == null;
label_1:
      do
      {
        if (this.from != null && this.MatchesFrom(xpathNavigator, p))
        {
          flag = true;
          num = 0;
        }
        else if (flag && this.MatchesCount(xpathNavigator, p))
          ++num;
        if (xpathNavigator.IsSamePosition(p.CurrentNode))
          return num;
      }
      while (xpathNavigator.MoveToFirstChild());
      while (!xpathNavigator.MoveToNext())
      {
        if (!xpathNavigator.MoveToParent())
          return 0;
      }
      goto label_1;
    }

    private int NumberSingle(XslTransformProcessor p)
    {
      XPathNavigator xpathNavigator1 = p.CurrentNode.Clone();
      while (!this.MatchesCount(xpathNavigator1, p))
      {
        if (this.from != null && this.MatchesFrom(xpathNavigator1, p) || !xpathNavigator1.MoveToParent())
          return 0;
      }
      if (this.from != null)
      {
        XPathNavigator xpathNavigator2 = xpathNavigator1.Clone();
        if (this.MatchesFrom(xpathNavigator2, p))
          return 0;
        bool flag = false;
        while (xpathNavigator2.MoveToParent())
        {
          if (this.MatchesFrom(xpathNavigator2, p))
          {
            flag = true;
            break;
          }
        }
        if (!flag)
          return 0;
      }
      int num = 1;
      while (xpathNavigator1.MoveToPrevious())
      {
        if (this.MatchesCount(xpathNavigator1, p))
          ++num;
      }
      return num;
    }

    private bool MatchesCount(XPathNavigator item, XslTransformProcessor p)
    {
      if (this.count != null)
        return p.Matches(this.count, item);
      return item.NodeType == p.CurrentNode.NodeType && item.LocalName == p.CurrentNode.LocalName && item.NamespaceURI == p.CurrentNode.NamespaceURI;
    }

    private bool MatchesFrom(XPathNavigator item, XslTransformProcessor p) => this.from == null ? item.NodeType == XPathNodeType.Root : p.Matches(this.from, item);

    private class XslNumberFormatter
    {
      private string firstSep;
      private string lastSep;
      private ArrayList fmtList = new ArrayList();

      public XslNumberFormatter(
        string format,
        string lang,
        string letterValue,
        char groupingSeparator,
        int groupingSize)
      {
        if (format == null || format == string.Empty)
        {
          this.fmtList.Add((object) XslNumber.XslNumberFormatter.FormatItem.GetItem((string) null, "1", groupingSeparator, groupingSize));
        }
        else
        {
          XslNumber.XslNumberFormatter.NumberFormatterScanner formatterScanner = new XslNumber.XslNumberFormatter.NumberFormatterScanner(format);
          this.firstSep = formatterScanner.Advance(false);
          string str1 = formatterScanner.Advance(true);
          if (str1 == null)
          {
            string firstSep = this.firstSep;
            this.firstSep = (string) null;
            this.fmtList.Add((object) XslNumber.XslNumberFormatter.FormatItem.GetItem(firstSep, "1", groupingSeparator, groupingSize));
          }
          else
          {
            this.fmtList.Add((object) XslNumber.XslNumberFormatter.FormatItem.GetItem(".", str1, groupingSeparator, groupingSize));
            string str2;
            do
            {
              string sep = formatterScanner.Advance(false);
              str2 = formatterScanner.Advance(true);
              if (str2 == null)
              {
                this.lastSep = sep;
                break;
              }
              this.fmtList.Add((object) XslNumber.XslNumberFormatter.FormatItem.GetItem(sep, str2, groupingSeparator, groupingSize));
            }
            while (str2 != null);
          }
        }
      }

      public string Format(double value) => this.Format(value, true);

      public string Format(double value, bool formatContent)
      {
        StringBuilder b = new StringBuilder();
        if (this.firstSep != null)
          b.Append(this.firstSep);
        if (formatContent)
          ((XslNumber.XslNumberFormatter.FormatItem) this.fmtList[0]).Format(b, value);
        if (this.lastSep != null)
          b.Append(this.lastSep);
        return b.ToString();
      }

      public string Format(int[] values)
      {
        StringBuilder b = new StringBuilder();
        if (this.firstSep != null)
          b.Append(this.firstSep);
        int index1 = 0;
        int num1 = this.fmtList.Count - 1;
        if (values.Length > 0)
        {
          if (this.fmtList.Count > 0)
            ((XslNumber.XslNumberFormatter.FormatItem) this.fmtList[index1]).Format(b, (double) values[0]);
          if (index1 < num1)
            ++index1;
        }
        for (int index2 = 1; index2 < values.Length; ++index2)
        {
          XslNumber.XslNumberFormatter.FormatItem fmt = (XslNumber.XslNumberFormatter.FormatItem) this.fmtList[index1];
          b.Append(fmt.sep);
          int num2 = values[index2];
          fmt.Format(b, (double) num2);
          if (index1 < num1)
            ++index1;
        }
        if (this.lastSep != null)
          b.Append(this.lastSep);
        return b.ToString();
      }

      private class NumberFormatterScanner
      {
        private int pos;
        private int len;
        private string fmt;

        public NumberFormatterScanner(string fmt)
        {
          this.fmt = fmt;
          this.len = fmt.Length;
        }

        public string Advance(bool alphaNum)
        {
          int pos = this.pos;
          while (this.pos < this.len && char.IsLetterOrDigit(this.fmt, this.pos) == alphaNum)
            ++this.pos;
          return this.pos == pos ? (string) null : this.fmt.Substring(pos, this.pos - pos);
        }
      }

      private abstract class FormatItem
      {
        public readonly string sep;

        public FormatItem(string sep) => this.sep = sep;

        public abstract void Format(StringBuilder b, double num);

        public static XslNumber.XslNumberFormatter.FormatItem GetItem(
          string sep,
          string item,
          char gpSep,
          int gpSize)
        {
          switch (item[0])
          {
            case '0':
            case '1':
              int num = 1;
              while (num < item.Length && char.IsDigit(item, num))
                ++num;
              return (XslNumber.XslNumberFormatter.FormatItem) new XslNumber.XslNumberFormatter.DigitItem(sep, num, gpSep, gpSize);
            case 'A':
              return (XslNumber.XslNumberFormatter.FormatItem) new XslNumber.XslNumberFormatter.AlphaItem(sep, true);
            case 'I':
              return (XslNumber.XslNumberFormatter.FormatItem) new XslNumber.XslNumberFormatter.RomanItem(sep, true);
            case 'a':
              return (XslNumber.XslNumberFormatter.FormatItem) new XslNumber.XslNumberFormatter.AlphaItem(sep, false);
            case 'i':
              return (XslNumber.XslNumberFormatter.FormatItem) new XslNumber.XslNumberFormatter.RomanItem(sep, false);
            default:
              return (XslNumber.XslNumberFormatter.FormatItem) new XslNumber.XslNumberFormatter.DigitItem(sep, 1, gpSep, gpSize);
          }
        }
      }

      private class AlphaItem : XslNumber.XslNumberFormatter.FormatItem
      {
        private bool uc;
        private static readonly char[] ucl = new char[26]
        {
          'A',
          'B',
          'C',
          'D',
          'E',
          'F',
          'G',
          'H',
          'I',
          'J',
          'K',
          'L',
          'M',
          'N',
          'O',
          'P',
          'Q',
          'R',
          'S',
          'T',
          'U',
          'V',
          'W',
          'X',
          'Y',
          'Z'
        };
        private static readonly char[] lcl = new char[26]
        {
          'a',
          'b',
          'c',
          'd',
          'e',
          'f',
          'g',
          'h',
          'i',
          'j',
          'k',
          'l',
          'm',
          'n',
          'o',
          'p',
          'q',
          'r',
          's',
          't',
          'u',
          'v',
          'w',
          'x',
          'y',
          'z'
        };

        public AlphaItem(string sep, bool uc)
          : base(sep)
        {
          this.uc = uc;
        }

        public override void Format(StringBuilder b, double num) => XslNumber.XslNumberFormatter.AlphaItem.alphaSeq(b, num, !this.uc ? XslNumber.XslNumberFormatter.AlphaItem.lcl : XslNumber.XslNumberFormatter.AlphaItem.ucl);

        private static void alphaSeq(StringBuilder b, double n, char[] alphabet)
        {
          n = XslNumber.Round(n);
          if (n == 0.0)
            return;
          if (n > (double) alphabet.Length)
            XslNumber.XslNumberFormatter.AlphaItem.alphaSeq(b, Math.Floor((n - 1.0) / (double) alphabet.Length), alphabet);
          b.Append(alphabet[((int) n - 1) % alphabet.Length]);
        }
      }

      private class RomanItem : XslNumber.XslNumberFormatter.FormatItem
      {
        private bool uc;
        private static readonly string[] ucrDigits = new string[13]
        {
          "M",
          "CM",
          "D",
          "CD",
          "C",
          "XC",
          "L",
          "XL",
          "X",
          "IX",
          "V",
          "IV",
          "I"
        };
        private static readonly string[] lcrDigits = new string[13]
        {
          "m",
          "cm",
          "d",
          "cd",
          "c",
          "xc",
          "l",
          "xl",
          "x",
          "ix",
          "v",
          "iv",
          "i"
        };
        private static readonly int[] decValues = new int[13]
        {
          1000,
          900,
          500,
          400,
          100,
          90,
          50,
          40,
          10,
          9,
          5,
          4,
          1
        };

        public RomanItem(string sep, bool uc)
          : base(sep)
        {
          this.uc = uc;
        }

        public override void Format(StringBuilder b, double num)
        {
          if (num < 1.0 || num > 4999.0)
          {
            b.Append(num);
          }
          else
          {
            num = XslNumber.Round(num);
            for (int index = 0; index < XslNumber.XslNumberFormatter.RomanItem.decValues.Length; ++index)
            {
              for (; (double) XslNumber.XslNumberFormatter.RomanItem.decValues[index] <= num; num -= (double) XslNumber.XslNumberFormatter.RomanItem.decValues[index])
              {
                if (this.uc)
                  b.Append(XslNumber.XslNumberFormatter.RomanItem.ucrDigits[index]);
                else
                  b.Append(XslNumber.XslNumberFormatter.RomanItem.lcrDigits[index]);
              }
              if (num == 0.0)
                break;
            }
          }
        }
      }

      private class DigitItem : XslNumber.XslNumberFormatter.FormatItem
      {
        private NumberFormatInfo nfi;
        private int decimalSectionLength;
        private StringBuilder numberBuilder;

        public DigitItem(string sep, int len, char gpSep, int gpSize)
          : base(sep)
        {
          this.nfi = new NumberFormatInfo();
          this.nfi.NumberDecimalDigits = 0;
          this.nfi.NumberGroupSizes = new int[1];
          if (gpSep != char.MinValue && gpSize > 0)
          {
            this.nfi.NumberGroupSeparator = gpSep.ToString();
            this.nfi.NumberGroupSizes = new int[1]{ gpSize };
          }
          this.decimalSectionLength = len;
        }

        public override void Format(StringBuilder b, double num)
        {
          string str = num.ToString("N", (IFormatProvider) this.nfi);
          int decimalSectionLength = this.decimalSectionLength;
          if (decimalSectionLength > 1)
          {
            if (this.numberBuilder == null)
              this.numberBuilder = new StringBuilder();
            for (int index = decimalSectionLength; index > str.Length; --index)
              this.numberBuilder.Append('0');
            this.numberBuilder.Append(str.Length > decimalSectionLength ? str.Substring(str.Length - decimalSectionLength, decimalSectionLength) : str);
            str = this.numberBuilder.ToString();
            this.numberBuilder.Length = 0;
          }
          b.Append(str);
        }
      }
    }
  }
}
