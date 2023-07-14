// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.XslTemplateTable
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System;
using System.Collections;
using System.Xml;
using System.Xml.XPath;

namespace Mono.Xml.Xsl
{
  internal class XslTemplateTable
  {
    private Hashtable templateTables = new Hashtable();
    private Hashtable namedTemplates = new Hashtable();
    private XslStylesheet parent;

    public XslTemplateTable(XslStylesheet parent) => this.parent = parent;

    public Hashtable TemplateTables => this.templateTables;

    public XslModedTemplateTable this[XmlQualifiedName mode] => this.templateTables[(object) mode] as XslModedTemplateTable;

    public void Add(XslTemplate template)
    {
      if (template.Name != XmlQualifiedName.Empty)
      {
        if (this.namedTemplates[(object) template.Name] != null)
          throw new InvalidOperationException("Named template " + (object) template.Name + " is already registered.");
        this.namedTemplates[(object) template.Name] = (object) template;
      }
      if (template.Match == null)
        return;
      XslModedTemplateTable table = this[template.Mode];
      if (table == null)
      {
        table = new XslModedTemplateTable(template.Mode);
        this.Add(table);
      }
      table.Add(template);
    }

    public void Add(XslModedTemplateTable table)
    {
      if (this[table.Mode] != null)
        throw new InvalidOperationException("Mode " + (object) table.Mode + " is already registered.");
      this.templateTables.Add((object) table.Mode, (object) table);
    }

    public XslTemplate FindMatch(
      XPathNavigator node,
      XmlQualifiedName mode,
      XslTransformProcessor p)
    {
      if (this[mode] != null)
      {
        XslTemplate match = this[mode].FindMatch(node, p);
        if (match != null)
          return match;
      }
      for (int index = this.parent.Imports.Count - 1; index >= 0; --index)
      {
        XslTemplate match = ((XslStylesheet) this.parent.Imports[index]).Templates.FindMatch(node, mode, p);
        if (match != null)
          return match;
      }
      return (XslTemplate) null;
    }

    public XslTemplate FindTemplate(XmlQualifiedName name)
    {
      XslTemplate namedTemplate = (XslTemplate) this.namedTemplates[(object) name];
      if (namedTemplate != null)
        return namedTemplate;
      for (int index = this.parent.Imports.Count - 1; index >= 0; --index)
      {
        XslTemplate template = ((XslStylesheet) this.parent.Imports[index]).Templates.FindTemplate(name);
        if (template != null)
          return template;
      }
      return (XslTemplate) null;
    }
  }
}
