// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.Operations.XslNotSupportedOperation
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System;
using System.Collections;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.Xsl.Operations
{
  internal class XslNotSupportedOperation : XslCompiledElement
  {
    private string name;
    private ArrayList fallbacks;

    public XslNotSupportedOperation(Compiler c)
      : base(c)
    {
    }

    protected override void Compile(Compiler c)
    {
      if (c.Debugger != null)
        c.Debugger.DebugCompile(this.DebugInput);
      this.name = c.Input.LocalName;
      if (!c.Input.MoveToFirstChild())
        return;
      do
      {
        if (c.Input.NodeType == XPathNodeType.Element && !(c.Input.LocalName != "fallback") && !(c.Input.NamespaceURI != "http://www.w3.org/1999/XSL/Transform"))
        {
          if (this.fallbacks == null)
            this.fallbacks = new ArrayList();
          this.fallbacks.Add((object) new XslFallback(c));
        }
      }
      while (c.Input.MoveToNext());
      c.Input.MoveToParent();
    }

    public override void Evaluate(XslTransformProcessor p)
    {
      if (p.Debugger != null)
        p.Debugger.DebugExecute(p, this.DebugInput);
      if (this.fallbacks == null)
        throw new XsltException(string.Format("'{0}' element is not supported as a template content in XSLT 1.0.", (object) this.name), (Exception) null);
      foreach (XslFallback fallback in this.fallbacks)
        fallback.Evaluate(p);
    }
  }
}
