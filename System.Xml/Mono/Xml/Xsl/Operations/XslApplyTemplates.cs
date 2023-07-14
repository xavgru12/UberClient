// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.Operations.XslApplyTemplates
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
  internal class XslApplyTemplates : XslCompiledElement
  {
    private XPathExpression select;
    private XmlQualifiedName mode;
    private ArrayList withParams;
    private XslSortEvaluator sortEvaluator;

    public XslApplyTemplates(Compiler c)
      : base(c)
    {
    }

    protected override void Compile(Compiler c)
    {
      if (c.Debugger != null)
        c.Debugger.DebugCompile(c.Input);
      c.CheckExtraAttributes("apply-templates", "select", "mode");
      this.select = (XPathExpression) c.CompileExpression(c.GetAttribute("select"));
      this.mode = c.ParseQNameAttribute("mode");
      ArrayList arrayList = (ArrayList) null;
      if (c.Input.MoveToFirstChild())
      {
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
                if (XslApplyTemplates.\u003C\u003Ef__switch\u0024map7 == null)
                {
                  // ISSUE: reference to a compiler-generated field
                  XslApplyTemplates.\u003C\u003Ef__switch\u0024map7 = new Dictionary<string, int>(2)
                  {
                    {
                      "with-param",
                      0
                    },
                    {
                      "sort",
                      1
                    }
                  };
                }
                int num;
                // ISSUE: reference to a compiler-generated field
                if (XslApplyTemplates.\u003C\u003Ef__switch\u0024map7.TryGetValue(localName, out num))
                {
                  if (num != 0)
                  {
                    if (num == 1)
                    {
                      if (arrayList == null)
                        arrayList = new ArrayList();
                      if (this.select == null)
                        this.select = (XPathExpression) c.CompileExpression("*");
                      arrayList.Add((object) new Sort(c));
                      goto case XPathNodeType.SignificantWhitespace;
                    }
                  }
                  else
                  {
                    if (this.withParams == null)
                      this.withParams = new ArrayList();
                    this.withParams.Add((object) new XslVariableInformation(c));
                    goto case XPathNodeType.SignificantWhitespace;
                  }
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
      if (arrayList == null)
        return;
      this.sortEvaluator = new XslSortEvaluator(this.select, (Sort[]) arrayList.ToArray(typeof (Sort)));
    }

    public override void Evaluate(XslTransformProcessor p)
    {
      if (p.Debugger != null)
        p.Debugger.DebugExecute(p, this.DebugInput);
      if (this.select == null)
      {
        p.ApplyTemplates(p.CurrentNode.SelectChildren(XPathNodeType.All), this.mode, this.withParams);
      }
      else
      {
        XPathNodeIterator nodes = this.sortEvaluator == null ? p.Select(this.select) : (XPathNodeIterator) this.sortEvaluator.SortedSelect(p);
        p.ApplyTemplates(nodes, this.mode, this.withParams);
      }
    }
  }
}
