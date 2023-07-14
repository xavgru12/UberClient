// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.Operations.XslForEach
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.Xml.XPath;

namespace Mono.Xml.Xsl.Operations
{
  internal class XslForEach : XslCompiledElement
  {
    private XPathExpression select;
    private XslOperation children;
    private XslSortEvaluator sortEvaluator;

    public XslForEach(Compiler c)
      : base(c)
    {
    }

    protected override void Compile(Compiler c)
    {
      if (c.Debugger != null)
        c.Debugger.DebugCompile(c.Input);
      c.CheckExtraAttributes("for-each", "select");
      c.AssertAttribute("select");
      this.select = (XPathExpression) c.CompileExpression(c.GetAttribute("select"));
      ArrayList arrayList = (ArrayList) null;
      if (c.Input.MoveToFirstChild())
      {
        bool flag = true;
        while (c.Input.NodeType != XPathNodeType.Text)
        {
          if (c.Input.NodeType == XPathNodeType.Element)
          {
            if (c.Input.NamespaceURI != "http://www.w3.org/1999/XSL/Transform")
            {
              flag = false;
              goto label_16;
            }
            else if (c.Input.LocalName != "sort")
            {
              flag = false;
              goto label_16;
            }
            else
            {
              if (arrayList == null)
                arrayList = new ArrayList();
              arrayList.Add((object) new Sort(c));
            }
          }
          if (!c.Input.MoveToNext())
            goto label_16;
        }
        flag = false;
label_16:
        if (!flag)
          this.children = c.CompileTemplateContent();
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
      XPathNodeIterator xpathNodeIterator = this.sortEvaluator == null ? p.Select(this.select) : (XPathNodeIterator) this.sortEvaluator.SortedSelect(p);
      while (p.NodesetMoveNext(xpathNodeIterator))
      {
        p.PushNodeset(xpathNodeIterator);
        p.PushForEachContext();
        this.children.Evaluate(p);
        p.PopForEachContext();
        p.PopNodeset();
      }
    }
  }
}
