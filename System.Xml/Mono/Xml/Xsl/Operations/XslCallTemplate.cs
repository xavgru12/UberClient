// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.Operations.XslCallTemplate
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.Xsl.Operations
{
  internal class XslCallTemplate : XslCompiledElement
  {
    private XmlQualifiedName name;
    private ArrayList withParams;

    public XslCallTemplate(Compiler c)
      : base(c)
    {
    }

    protected override void Compile(Compiler c)
    {
      if (c.Debugger != null)
        c.Debugger.DebugCompile(c.Input);
      c.CheckExtraAttributes("call-template", "name");
      c.AssertAttribute("name");
      this.name = c.ParseQNameAttribute("name");
      if (!c.Input.MoveToFirstChild())
        return;
      do
      {
        switch (c.Input.NodeType)
        {
          case XPathNodeType.Element:
            if (c.Input.NamespaceURI != "http://www.w3.org/1999/XSL/Transform")
              throw new XsltCompileException("Unexpected element", (Exception) null, c.Input);
            string localName = c.Input.LocalName;
            if (localName != null)
            {
              // ISSUE: reference to a compiler-generated field
              if (XslCallTemplate.\u003C\u003Ef__switch\u0024map8 == null)
              {
                // ISSUE: reference to a compiler-generated field
                XslCallTemplate.\u003C\u003Ef__switch\u0024map8 = new Dictionary<string, int>(1)
                {
                  {
                    "with-param",
                    0
                  }
                };
              }
              int num;
              // ISSUE: reference to a compiler-generated field
              if (XslCallTemplate.\u003C\u003Ef__switch\u0024map8.TryGetValue(localName, out num) && num == 0)
              {
                if (this.withParams == null)
                  this.withParams = new ArrayList();
                this.withParams.Add((object) new XslVariableInformation(c));
                goto case XPathNodeType.SignificantWhitespace;
              }
            }
            throw new XsltCompileException("Unexpected element", (Exception) null, c.Input);
          case XPathNodeType.SignificantWhitespace:
          case XPathNodeType.Whitespace:
          case XPathNodeType.ProcessingInstruction:
          case XPathNodeType.Comment:
            continue;
          default:
            throw new XsltCompileException("Unexpected node type " + (object) c.Input.NodeType, (Exception) null, c.Input);
        }
      }
      while (c.Input.MoveToNext());
      c.Input.MoveToParent();
    }

    public override void Evaluate(XslTransformProcessor p)
    {
      if (p.Debugger != null)
        p.Debugger.DebugExecute(p, this.DebugInput);
      p.CallTemplate(this.name, this.withParams);
    }
  }
}
