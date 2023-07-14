// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.Operations.XslAvt
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.Xsl.Operations
{
  internal class XslAvt
  {
    private string simpleString;
    private ArrayList avtParts;

    public XslAvt(string str, Compiler comp)
    {
      if (str.IndexOf("{") == -1 && str.IndexOf("}") == -1)
      {
        this.simpleString = str;
      }
      else
      {
        this.avtParts = new ArrayList();
        StringBuilder stringBuilder = new StringBuilder();
        StringReader stringReader = new StringReader(str);
        while (stringReader.Peek() != -1)
        {
          char ch1 = (char) stringReader.Read();
          switch (ch1)
          {
            case '{':
              if ((ushort) stringReader.Peek() == (ushort) 123)
              {
                stringBuilder.Append((char) stringReader.Read());
                continue;
              }
              if (stringBuilder.Length != 0)
              {
                this.avtParts.Add((object) new XslAvt.SimpleAvtPart(stringBuilder.ToString()));
                stringBuilder.Length = 0;
              }
              char ch2;
              while ((ch2 = (char) stringReader.Read()) != '}')
              {
                switch (ch2)
                {
                  case '"':
                  case '\'':
                    char ch3 = ch2;
                    stringBuilder.Append(ch2);
                    char ch4;
                    while ((int) (ch4 = (char) stringReader.Read()) != (int) ch3)
                    {
                      stringBuilder.Append(ch4);
                      if (stringReader.Peek() == -1)
                        throw new XsltCompileException("Unexpected end of AVT", (Exception) null, comp.Input);
                    }
                    stringBuilder.Append(ch4);
                    break;
                  default:
                    stringBuilder.Append(ch2);
                    break;
                }
                if (stringReader.Peek() == -1)
                  throw new XsltCompileException("Unexpected end of AVT", (Exception) null, comp.Input);
              }
              this.avtParts.Add((object) new XslAvt.XPathAvtPart((XPathExpression) comp.CompileExpression(stringBuilder.ToString())));
              stringBuilder.Length = 0;
              continue;
            case '}':
              ch1 = (char) stringReader.Read();
              if (ch1 != '}')
                throw new XsltCompileException("Braces must be escaped", (Exception) null, comp.Input);
              break;
          }
          stringBuilder.Append(ch1);
        }
        if (stringBuilder.Length == 0)
          return;
        this.avtParts.Add((object) new XslAvt.SimpleAvtPart(stringBuilder.ToString()));
        stringBuilder.Length = 0;
      }
    }

    public static string AttemptPreCalc(ref XslAvt avt)
    {
      if (avt == null)
        return (string) null;
      if (avt.simpleString == null)
        return (string) null;
      string simpleString = avt.simpleString;
      avt = (XslAvt) null;
      return simpleString;
    }

    public string Evaluate(XslTransformProcessor p)
    {
      if (this.simpleString != null)
        return this.simpleString;
      if (this.avtParts.Count == 1)
        return ((XslAvt.AvtPart) this.avtParts[0]).Evaluate(p);
      StringBuilder avtStringBuilder = p.GetAvtStringBuilder();
      int count = this.avtParts.Count;
      for (int index = 0; index < count; ++index)
        avtStringBuilder.Append(((XslAvt.AvtPart) this.avtParts[index]).Evaluate(p));
      return p.ReleaseAvtStringBuilder();
    }

    private abstract class AvtPart
    {
      public abstract string Evaluate(XslTransformProcessor p);
    }

    private sealed class SimpleAvtPart : XslAvt.AvtPart
    {
      private string val;

      public SimpleAvtPart(string val) => this.val = val;

      public override string Evaluate(XslTransformProcessor p) => this.val;
    }

    private sealed class XPathAvtPart : XslAvt.AvtPart
    {
      private XPathExpression expr;

      public XPathAvtPart(XPathExpression expr) => this.expr = expr;

      public override string Evaluate(XslTransformProcessor p) => p.EvaluateString(this.expr);
    }
  }
}
