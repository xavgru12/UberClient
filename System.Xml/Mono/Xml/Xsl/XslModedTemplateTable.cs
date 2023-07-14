// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.XslModedTemplateTable
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using Mono.Xml.XPath;
using System;
using System.Collections;
using System.Xml;
using System.Xml.XPath;

namespace Mono.Xml.Xsl
{
  internal class XslModedTemplateTable
  {
    private ArrayList unnamedTemplates = new ArrayList();
    private XmlQualifiedName mode;
    private bool sorted;

    public XslModedTemplateTable(XmlQualifiedName mode) => this.mode = !(mode == (XmlQualifiedName) null) ? mode : throw new InvalidOperationException();

    public XmlQualifiedName Mode => this.mode;

    public void Add(XslTemplate t)
    {
      if (!double.IsNaN(t.Priority))
        this.unnamedTemplates.Add((object) new XslModedTemplateTable.TemplateWithPriority(t, t.Priority));
      else
        this.Add(t, t.Match);
    }

    public void Add(XslTemplate t, Pattern p)
    {
      if (p is UnionPattern)
      {
        this.Add(t, ((UnionPattern) p).p0);
        this.Add(t, ((UnionPattern) p).p1);
      }
      else
        this.unnamedTemplates.Add((object) new XslModedTemplateTable.TemplateWithPriority(t, p));
    }

    public XslTemplate FindMatch(XPathNavigator node, XslTransformProcessor p)
    {
      if (!this.sorted)
      {
        this.unnamedTemplates.Sort();
        this.unnamedTemplates.Reverse();
        this.sorted = true;
      }
      for (int index = 0; index < this.unnamedTemplates.Count; ++index)
      {
        XslModedTemplateTable.TemplateWithPriority unnamedTemplate = (XslModedTemplateTable.TemplateWithPriority) this.unnamedTemplates[index];
        if (unnamedTemplate.Matches(node, p))
          return unnamedTemplate.Template;
      }
      return (XslTemplate) null;
    }

    private class TemplateWithPriority : IComparable
    {
      public readonly double Priority;
      public readonly XslTemplate Template;
      public readonly Pattern Pattern;
      public readonly int TemplateID;

      public TemplateWithPriority(XslTemplate t, Pattern p)
      {
        this.Template = t;
        this.Pattern = p;
        this.Priority = p.DefaultPriority;
        this.TemplateID = t.Id;
      }

      public TemplateWithPriority(XslTemplate t, double p)
      {
        this.Template = t;
        this.Pattern = t.Match;
        this.Priority = p;
        this.TemplateID = t.Id;
      }

      public int CompareTo(object o)
      {
        XslModedTemplateTable.TemplateWithPriority templateWithPriority1 = this;
        XslModedTemplateTable.TemplateWithPriority templateWithPriority2 = (XslModedTemplateTable.TemplateWithPriority) o;
        int num = templateWithPriority1.Priority.CompareTo(templateWithPriority2.Priority);
        return num != 0 ? num : templateWithPriority1.TemplateID.CompareTo(templateWithPriority2.TemplateID);
      }

      public bool Matches(XPathNavigator n, XslTransformProcessor p) => p.Matches(this.Pattern, n);
    }
  }
}
