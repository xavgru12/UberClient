// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.Operations.XslElement
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System;
using System.Collections;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.Xsl.Operations
{
  internal class XslElement : XslCompiledElement
  {
    private XslAvt name;
    private XslAvt ns;
    private string calcName;
    private string calcNs;
    private string calcPrefix;
    private Hashtable nsDecls;
    private bool isEmptyElement;
    private XslOperation value;
    private XmlQualifiedName[] useAttributeSets;

    public XslElement(Compiler c)
      : base(c)
    {
    }

    protected override void Compile(Compiler c)
    {
      if (c.Debugger != null)
        c.Debugger.DebugCompile(c.Input);
      c.CheckExtraAttributes("element", "name", "namespace", "use-attribute-sets");
      this.name = c.ParseAvtAttribute("name");
      this.ns = c.ParseAvtAttribute("namespace");
      this.nsDecls = c.GetNamespacesToCopy();
      this.calcName = XslAvt.AttemptPreCalc(ref this.name);
      if (this.calcName != null)
      {
        int length = this.calcName.IndexOf(':');
        if (length == 0)
          throw new XsltCompileException("Invalid name attribute", (Exception) null, c.Input);
        this.calcPrefix = length >= 0 ? this.calcName.Substring(0, length) : string.Empty;
        if (length > 0)
          this.calcName = this.calcName.Substring(length + 1);
        try
        {
          XmlConvert.VerifyNCName(this.calcName);
          if (this.calcPrefix != string.Empty)
            XmlConvert.VerifyNCName(this.calcPrefix);
        }
        catch (XmlException ex)
        {
          throw new XsltCompileException("Invalid name attribute", (Exception) ex, c.Input);
        }
        if (this.ns == null)
        {
          this.calcNs = c.Input.GetNamespace(this.calcPrefix);
          if (this.calcPrefix != string.Empty && this.calcNs == string.Empty)
            throw new XsltCompileException("Invalid name attribute", (Exception) null, c.Input);
        }
      }
      else if (this.ns != null)
        this.calcNs = XslAvt.AttemptPreCalc(ref this.ns);
      this.useAttributeSets = c.ParseQNameListAttribute("use-attribute-sets");
      this.isEmptyElement = c.Input.IsEmptyElement;
      if (!c.Input.MoveToFirstChild())
        return;
      this.value = c.CompileTemplateContent(XPathNodeType.Element);
      c.Input.MoveToParent();
    }

    public override void Evaluate(XslTransformProcessor p)
    {
      if (p.Debugger != null)
        p.Debugger.DebugExecute(p, this.DebugInput);
      string name1;
      string str1 = name1 = this.calcName == null ? this.name.Evaluate(p) : this.calcName;
      string str2 = this.calcNs == null ? (this.ns == null ? (string) null : this.ns.Evaluate(p)) : this.calcNs;
      XmlQualifiedName xmlQualifiedName = XslNameUtil.FromString(name1, this.nsDecls);
      string name2 = xmlQualifiedName.Name;
      if (str2 == null)
        str2 = xmlQualifiedName.Namespace;
      int length = name1.IndexOf(':');
      if (length > 0)
        this.calcPrefix = name1.Substring(0, length);
      else if (length == 0)
        XmlConvert.VerifyNCName(string.Empty);
      string str3 = this.calcPrefix == null ? string.Empty : this.calcPrefix;
      if (str3 != string.Empty)
        XmlConvert.VerifyNCName(str3);
      XmlConvert.VerifyNCName(name2);
      bool insideCdataElement = p.InsideCDataElement;
      p.PushElementState(str3, name2, str2, false);
      p.Out.WriteStartElement(str3, name2, str2);
      if (this.useAttributeSets != null)
      {
        foreach (XmlQualifiedName useAttributeSet in this.useAttributeSets)
          p.ResolveAttributeSet(useAttributeSet).Evaluate(p);
      }
      if (this.value != null)
        this.value.Evaluate(p);
      if (this.isEmptyElement && this.useAttributeSets == null)
        p.Out.WriteEndElement();
      else
        p.Out.WriteFullEndElement();
      p.PopCDataState(insideCdataElement);
    }
  }
}
