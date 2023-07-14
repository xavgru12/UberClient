// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.Tokenizer
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using Mono.Xml.Xsl.yyParser;
using System;
using System.Collections;
using System.Globalization;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace Mono.Xml.Xsl
{
  internal class Tokenizer : yyInput
  {
    private const char EOL = '\0';
    private string m_rgchInput;
    private int m_ich;
    private int m_cch;
    private int m_iToken;
    private int m_iTokenPrev = 258;
    private object m_objToken;
    private bool m_fPrevWasOperator;
    private bool m_fThisIsOperator;
    private static readonly Hashtable s_mapTokens = new Hashtable();
    private static readonly object[] s_rgTokenMap = new object[42]
    {
      (object) 274,
      (object) "and",
      (object) 276,
      (object) "or",
      (object) 278,
      (object) "div",
      (object) 280,
      (object) "mod",
      (object) 296,
      (object) "ancestor",
      (object) 298,
      (object) "ancestor-or-self",
      (object) 300,
      (object) "attribute",
      (object) 302,
      (object) "child",
      (object) 304,
      (object) "descendant",
      (object) 306,
      (object) "descendant-or-self",
      (object) 308,
      (object) "following",
      (object) 310,
      (object) "following-sibling",
      (object) 312,
      (object) "namespace",
      (object) 314,
      (object) "parent",
      (object) 316,
      (object) "preceding",
      (object) 318,
      (object) "preceding-sibling",
      (object) 320,
      (object) "self",
      (object) 322,
      (object) "comment",
      (object) 324,
      (object) "text",
      (object) 326,
      (object) "processing-instruction",
      (object) 328,
      (object) "node"
    };

    public Tokenizer(string strInput)
    {
      this.m_rgchInput = strInput;
      this.m_ich = 0;
      this.m_cch = strInput.Length;
      this.SkipWhitespace();
    }

    static Tokenizer()
    {
      for (int index = 0; index < Tokenizer.s_rgTokenMap.Length; index += 2)
        Tokenizer.s_mapTokens.Add(Tokenizer.s_rgTokenMap[index + 1], Tokenizer.s_rgTokenMap[index]);
    }

    private char Peek(int iOffset) => this.m_ich + iOffset >= this.m_cch ? char.MinValue : this.m_rgchInput[this.m_ich + iOffset];

    private char Peek() => this.Peek(0);

    private char GetChar() => this.m_ich >= this.m_cch ? char.MinValue : this.m_rgchInput[this.m_ich++];

    private char PutBack() => this.m_ich != 0 ? this.m_rgchInput[--this.m_ich] : throw new XPathException("XPath parser returned an error status: invalid tokenizer state.");

    private bool SkipWhitespace()
    {
      if (!Tokenizer.IsWhitespace(this.Peek()))
        return false;
      while (Tokenizer.IsWhitespace(this.Peek()))
      {
        int num = (int) this.GetChar();
      }
      return true;
    }

    private int ParseNumber()
    {
      StringBuilder stringBuilder = new StringBuilder();
      while (Tokenizer.IsDigit(this.Peek()))
        stringBuilder.Append(this.GetChar());
      if (this.Peek() == '.')
      {
        stringBuilder.Append(this.GetChar());
        while (Tokenizer.IsDigit(this.Peek()))
          stringBuilder.Append(this.GetChar());
      }
      this.m_objToken = (object) double.Parse(stringBuilder.ToString(), (IFormatProvider) NumberFormatInfo.InvariantInfo);
      return 331;
    }

    private int ParseLiteral()
    {
      StringBuilder stringBuilder = new StringBuilder();
      char ch1 = this.GetChar();
      char ch2;
      while ((int) (ch2 = this.Peek()) != (int) ch1)
      {
        if (ch2 == char.MinValue)
          throw new XPathException("unmatched " + (object) ch1 + " in expression");
        stringBuilder.Append(this.GetChar());
      }
      int num = (int) this.GetChar();
      this.m_objToken = (object) stringBuilder.ToString();
      return 332;
    }

    private string ReadIdentifier()
    {
      StringBuilder stringBuilder = new StringBuilder();
      char c1 = this.Peek();
      if (!char.IsLetter(c1) && c1 != '_')
        return (string) null;
      stringBuilder.Append(this.GetChar());
      char c2;
      while ((c2 = this.Peek()) == '_' || c2 == '-' || c2 == '.' || char.IsLetterOrDigit(c2))
        stringBuilder.Append(this.GetChar());
      this.SkipWhitespace();
      return stringBuilder.ToString();
    }

    private int ParseIdentifier()
    {
      string str = this.ReadIdentifier();
      object mapToken = Tokenizer.s_mapTokens[(object) str];
      int iToken = mapToken == null ? 333 : (int) mapToken;
      this.m_objToken = (object) str;
      char ch1 = this.Peek();
      if (ch1 == ':')
      {
        if (this.Peek(1) == ':')
          return mapToken != null && this.IsAxisName(iToken) ? iToken : throw new XPathException("invalid axis name: '" + str + "'");
        int num1 = (int) this.GetChar();
        this.SkipWhitespace();
        char ch2 = this.Peek();
        if (ch2 == '*')
        {
          int num2 = (int) this.GetChar();
          this.m_objToken = (object) new XmlQualifiedName(string.Empty, str);
          return 333;
        }
        string name = this.ReadIdentifier();
        if (name == null)
          throw new XPathException("invalid QName: " + str + ":" + (object) ch2);
        char ch3 = this.Peek();
        this.m_objToken = (object) new XmlQualifiedName(name, str);
        return ch3 == '(' ? 269 : 333;
      }
      if (!this.IsFirstToken && !this.m_fPrevWasOperator)
        return mapToken != null && this.IsOperatorName(iToken) ? iToken : throw new XPathException("invalid operator name: '" + str + "'");
      if (ch1 == '(')
      {
        if (mapToken == null)
        {
          this.m_objToken = (object) new XmlQualifiedName(str, string.Empty);
          return 269;
        }
        return this.IsNodeType(iToken) ? iToken : throw new XPathException("invalid function name: '" + str + "'");
      }
      this.m_objToken = (object) new XmlQualifiedName(str, string.Empty);
      return 333;
    }

    private static bool IsWhitespace(char ch) => ch == ' ' || ch == '\t' || ch == '\n' || ch == '\r';

    private static bool IsDigit(char ch) => ch >= '0' && ch <= '9';

    private int ParseToken()
    {
      char ch1 = this.Peek();
      char ch2 = ch1;
      switch (ch2)
      {
        case '!':
          int num1 = (int) this.GetChar();
          if (this.Peek() == '=')
          {
            this.m_fThisIsOperator = true;
            int num2 = (int) this.GetChar();
            return 288;
          }
          break;
        case '"':
          return this.ParseLiteral();
        case '$':
          int num3 = (int) this.GetChar();
          this.m_fThisIsOperator = true;
          return 285;
        case '\'':
          return this.ParseLiteral();
        case '(':
          this.m_fThisIsOperator = true;
          int num4 = (int) this.GetChar();
          return 272;
        case ')':
          int num5 = (int) this.GetChar();
          return 273;
        case '*':
          int num6 = (int) this.GetChar();
          if (this.IsFirstToken || this.m_fPrevWasOperator)
            return 284;
          this.m_fThisIsOperator = true;
          return 330;
        case '+':
          this.m_fThisIsOperator = true;
          int num7 = (int) this.GetChar();
          return 282;
        case ',':
          this.m_fThisIsOperator = true;
          int num8 = (int) this.GetChar();
          return 267;
        case '-':
          this.m_fThisIsOperator = true;
          int num9 = (int) this.GetChar();
          return 283;
        case '.':
          int num10 = (int) this.GetChar();
          if (this.Peek() == '.')
          {
            int num11 = (int) this.GetChar();
            return 263;
          }
          if (!Tokenizer.IsDigit(this.Peek()))
            return 262;
          int num12 = (int) this.PutBack();
          return this.ParseNumber();
        case '/':
          this.m_fThisIsOperator = true;
          int num13 = (int) this.GetChar();
          if (this.Peek() != '/')
            return 259;
          int num14 = (int) this.GetChar();
          return 260;
        case ':':
          int num15 = (int) this.GetChar();
          if (this.Peek() != ':')
            return 257;
          this.m_fThisIsOperator = true;
          int num16 = (int) this.GetChar();
          return 265;
        case '<':
          this.m_fThisIsOperator = true;
          int num17 = (int) this.GetChar();
          if (this.Peek() != '=')
            return 294;
          int num18 = (int) this.GetChar();
          return 290;
        case '=':
          this.m_fThisIsOperator = true;
          int num19 = (int) this.GetChar();
          return 287;
        case '>':
          this.m_fThisIsOperator = true;
          int num20 = (int) this.GetChar();
          if (this.Peek() != '=')
            return 295;
          int num21 = (int) this.GetChar();
          return 292;
        case '@':
          this.m_fThisIsOperator = true;
          int num22 = (int) this.GetChar();
          return 268;
        default:
          switch (ch2)
          {
            case '[':
              this.m_fThisIsOperator = true;
              int num23 = (int) this.GetChar();
              return 270;
            case ']':
              int num24 = (int) this.GetChar();
              return 271;
            default:
              if (ch2 == char.MinValue)
                return 258;
              if (ch2 == '|')
              {
                this.m_fThisIsOperator = true;
                int num25 = (int) this.GetChar();
                return 286;
              }
              if (Tokenizer.IsDigit(ch1))
                return this.ParseNumber();
              if (char.IsLetter(ch1) || ch1 == '_')
              {
                int identifier = this.ParseIdentifier();
                if (this.IsOperatorName(identifier))
                  this.m_fThisIsOperator = true;
                return identifier;
              }
              break;
          }
          break;
      }
      throw new XPathException("invalid token: '" + (object) ch1 + "'");
    }

    public bool advance()
    {
      this.m_fThisIsOperator = false;
      this.m_objToken = (object) null;
      this.m_iToken = this.ParseToken();
      this.SkipWhitespace();
      this.m_iTokenPrev = this.m_iToken;
      this.m_fPrevWasOperator = this.m_fThisIsOperator;
      return this.m_iToken != 258;
    }

    public int token() => this.m_iToken;

    public object value() => this.m_objToken;

    private bool IsFirstToken => this.m_iTokenPrev == 258;

    private bool IsNodeType(int iToken)
    {
      switch (iToken)
      {
        case 322:
        case 324:
        case 326:
        case 328:
          return true;
        default:
          return false;
      }
    }

    private bool IsOperatorName(int iToken)
    {
      switch (iToken)
      {
        case 274:
        case 276:
        case 278:
        case 280:
          return true;
        default:
          return false;
      }
    }

    private bool IsAxisName(int iToken)
    {
      switch (iToken)
      {
        case 296:
        case 298:
        case 300:
        case 302:
        case 304:
        case 306:
        case 308:
        case 310:
        case 312:
        case 314:
        case 316:
        case 318:
        case 320:
          return true;
        default:
          return false;
      }
    }
  }
}
