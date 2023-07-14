// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.XslAttributeSet
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using Mono.Xml.Xsl.Operations;
using System;
using System.Collections;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.Xsl
{
  internal class XslAttributeSet : XslCompiledElement
  {
    private XmlQualifiedName name;
    private ArrayList usedAttributeSets = new ArrayList();
    private ArrayList attributes = new ArrayList();

    public XslAttributeSet(Compiler c)
      : base(c)
    {
    }

    public XmlQualifiedName Name => this.name;

    protected override void Compile(Compiler c)
    {
      this.name = c.ParseQNameAttribute("name");
      XmlQualifiedName[] qnameListAttribute = c.ParseQNameListAttribute("use-attribute-sets");
      if (qnameListAttribute != null)
      {
        foreach (object obj in qnameListAttribute)
          this.usedAttributeSets.Add(obj);
      }
      if (!c.Input.MoveToFirstChild())
        return;
      do
      {
        if (c.Input.NodeType == XPathNodeType.Element)
        {
          if (c.Input.NamespaceURI != "http://www.w3.org/1999/XSL/Transform" || c.Input.LocalName != "attribute")
            throw new XsltCompileException("Invalid attr set content", (Exception) null, c.Input);
          this.attributes.Add((object) new XslAttribute(c));
        }
      }
      while (c.Input.MoveToNext());
      c.Input.MoveToParent();
    }

    public void Merge(XslAttributeSet s)
    {
      this.attributes.AddRange((ICollection) s.attributes);
      foreach (XmlQualifiedName usedAttributeSet in s.usedAttributeSets)
      {
        if (!this.usedAttributeSets.Contains((object) usedAttributeSet))
          this.usedAttributeSets.Add((object) usedAttributeSet);
      }
    }

    public override void Evaluate(XslTransformProcessor p)
    {
      p.SetBusy((object) this);
      if (this.usedAttributeSets != null)
      {
        for (int index = 0; index < this.usedAttributeSets.Count; ++index)
        {
          XmlQualifiedName usedAttributeSet = (XmlQualifiedName) this.usedAttributeSets[index];
          XslAttributeSet o = p.ResolveAttributeSet(usedAttributeSet);
          if (o == null)
            throw new XsltException("Could not resolve attribute set", (Exception) null, p.CurrentNode);
          if (p.IsBusy((object) o))
            throw new XsltException("circular dependency", (Exception) null, p.CurrentNode);
          o.Evaluate(p);
        }
      }
      for (int index = 0; index < this.attributes.Count; ++index)
        ((XslAttribute) this.attributes[index]).Evaluate(p);
      p.SetFree((object) this);
    }
  }
}
