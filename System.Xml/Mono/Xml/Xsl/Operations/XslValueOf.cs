// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.Operations.XslValueOf
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.Xsl.Operations
{
  internal class XslValueOf : XslCompiledElement
  {
    private XPathExpression select;
    private bool disableOutputEscaping;

    public XslValueOf(Compiler c)
      : base(c)
    {
    }

    protected override void Compile(Compiler c)
    {
      if (c.Debugger != null)
        c.Debugger.DebugCompile(this.DebugInput);
      c.CheckExtraAttributes("value-of", "select", "disable-output-escaping");
      c.AssertAttribute("select");
      this.select = (XPathExpression) c.CompileExpression(c.GetAttribute("select"));
      this.disableOutputEscaping = c.ParseYesNoAttribute("disable-output-escaping", false);
      if (!c.Input.MoveToFirstChild())
        return;
      do
      {
        switch (c.Input.NodeType)
        {
          case XPathNodeType.Element:
            if (!(c.Input.NamespaceURI == "http://www.w3.org/1999/XSL/Transform"))
              break;
            goto case XPathNodeType.Text;
          case XPathNodeType.Text:
          case XPathNodeType.SignificantWhitespace:
            throw new XsltCompileException("XSLT value-of element cannot contain any child.", (Exception) null, c.Input);
        }
      }
      while (c.Input.MoveToNext());
    }

    public override void Evaluate(XslTransformProcessor p)
    {
      if (p.Debugger != null)
        p.Debugger.DebugExecute(p, this.DebugInput);
      if (!this.disableOutputEscaping)
        p.Out.WriteString(p.EvaluateString(this.select));
      else
        p.Out.WriteRaw(p.EvaluateString(this.select));
    }
  }
}
