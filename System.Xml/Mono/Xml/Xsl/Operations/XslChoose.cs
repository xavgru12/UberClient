// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.Operations.XslChoose
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.Xsl.Operations
{
  internal class XslChoose : XslCompiledElement
  {
    private XslOperation defaultChoice;
    private ArrayList conditions = new ArrayList();

    public XslChoose(Compiler c)
      : base(c)
    {
    }

    protected override void Compile(Compiler c)
    {
      if (c.Debugger != null)
        c.Debugger.DebugCompile(c.Input);
      c.CheckExtraAttributes("choose");
      if (!c.Input.MoveToFirstChild())
        throw new XsltCompileException("Expecting non-empty element", (Exception) null, c.Input);
      do
      {
        if (c.Input.NodeType == XPathNodeType.Element && !(c.Input.NamespaceURI != "http://www.w3.org/1999/XSL/Transform"))
        {
          if (this.defaultChoice != null)
            throw new XsltCompileException("otherwise attribute must be last", (Exception) null, c.Input);
          string localName = c.Input.LocalName;
          if (localName != null)
          {
            // ISSUE: reference to a compiler-generated field
            if (XslChoose.\u003C\u003Ef__switch\u0024map9 == null)
            {
              // ISSUE: reference to a compiler-generated field
              XslChoose.\u003C\u003Ef__switch\u0024map9 = new Dictionary<string, int>(2)
              {
                {
                  "when",
                  0
                },
                {
                  "otherwise",
                  1
                }
              };
            }
            int num;
            // ISSUE: reference to a compiler-generated field
            if (XslChoose.\u003C\u003Ef__switch\u0024map9.TryGetValue(localName, out num))
            {
              switch (num)
              {
                case 0:
                  this.conditions.Add((object) new XslIf(c));
                  goto label_18;
                case 1:
                  c.CheckExtraAttributes("otherwise");
                  if (c.Input.MoveToFirstChild())
                  {
                    this.defaultChoice = c.CompileTemplateContent();
                    c.Input.MoveToParent();
                    goto label_18;
                  }
                  else
                    goto label_18;
              }
            }
          }
          if (c.CurrentStylesheet.Version == "1.0")
            throw new XsltCompileException("XSLT choose element accepts only when and otherwise elements", (Exception) null, c.Input);
        }
label_18:;
      }
      while (c.Input.MoveToNext());
      c.Input.MoveToParent();
      if (this.conditions.Count == 0)
        throw new XsltCompileException("Choose must have 1 or more when elements", (Exception) null, c.Input);
    }

    public override void Evaluate(XslTransformProcessor p)
    {
      if (p.Debugger != null)
        p.Debugger.DebugExecute(p, this.DebugInput);
      int count = this.conditions.Count;
      for (int index = 0; index < count; ++index)
      {
        if (((XslIf) this.conditions[index]).EvaluateIfTrue(p))
          return;
      }
      if (this.defaultChoice == null)
        return;
      this.defaultChoice.Evaluate(p);
    }
  }
}
