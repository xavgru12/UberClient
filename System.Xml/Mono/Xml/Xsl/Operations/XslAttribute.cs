// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.Operations.XslAttribute
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
  internal class XslAttribute : XslCompiledElement
  {
    private XslAvt name;
    private XslAvt ns;
    private string calcName;
    private string calcNs;
    private string calcPrefix;
    private Hashtable nsDecls;
    private XslOperation value;

    public XslAttribute(Compiler c)
      : base(c)
    {
    }

    protected override void Compile(Compiler c)
    {
      if (c.Debugger != null)
        c.Debugger.DebugCompile(c.Input);
      XPathNavigator xpathNavigator = c.Input.Clone();
      this.nsDecls = c.GetNamespacesToCopy();
      c.CheckExtraAttributes("attribute", "name", "namespace");
      this.name = c.ParseAvtAttribute("name");
      if (this.name == null)
        throw new XsltCompileException("Attribute \"name\" is required on XSLT attribute element", (Exception) null, c.Input);
      this.ns = c.ParseAvtAttribute("namespace");
      this.calcName = XslAvt.AttemptPreCalc(ref this.name);
      this.calcPrefix = string.Empty;
      if (this.calcName != null)
      {
        int length = this.calcName.IndexOf(':');
        this.calcPrefix = length >= 0 ? this.calcName.Substring(0, length) : string.Empty;
        this.calcName = length >= 0 ? this.calcName.Substring(length + 1, this.calcName.Length - length - 1) : this.calcName;
        try
        {
          XmlConvert.VerifyNCName(this.calcName);
          if (this.calcPrefix != string.Empty)
            XmlConvert.VerifyNCName(this.calcPrefix);
        }
        catch (XmlException ex)
        {
          throw new XsltCompileException("Invalid attribute name", (Exception) ex, c.Input);
        }
      }
      if (this.calcPrefix != string.Empty)
      {
        this.calcPrefix = c.CurrentStylesheet.GetActualPrefix(this.calcPrefix);
        if (this.calcPrefix == null)
          this.calcPrefix = string.Empty;
      }
      if (this.calcPrefix != string.Empty && this.ns == null)
        this.calcNs = xpathNavigator.GetNamespace(this.calcPrefix);
      else if (this.ns != null)
        this.calcNs = XslAvt.AttemptPreCalc(ref this.ns);
      if (!c.Input.MoveToFirstChild())
        return;
      this.value = c.CompileTemplateContent(XPathNodeType.Attribute);
      c.Input.MoveToParent();
    }

    public override void Evaluate(XslTransformProcessor p)
    {
      if (p.Debugger != null)
        p.Debugger.DebugExecute(p, this.DebugInput);
      string str1 = this.calcName == null ? this.name.Evaluate(p) : this.calcName;
      string nsURI = this.calcNs == null ? (this.ns == null ? string.Empty : this.ns.Evaluate(p)) : this.calcNs;
      string str2 = this.calcPrefix == null ? string.Empty : this.calcPrefix;
      if (str1 == "xmlns")
        return;
      int length = str1.IndexOf(':');
      if (length > 0)
      {
        str2 = str1.Substring(0, length);
        str1 = str1.Substring(length + 1);
        if (nsURI == string.Empty && str2 == "xml")
          nsURI = "http://www.w3.org/XML/1998/namespace";
        else if (nsURI == string.Empty)
          nsURI = (string) this.nsDecls[(object) str2] ?? string.Empty;
      }
      if (str2 == "xmlns")
        str2 = string.Empty;
      XmlConvert.VerifyName(str1);
      p.Out.WriteAttributeString(str2, str1, nsURI, this.value != null ? this.value.EvaluateAsString(p) : string.Empty);
    }
  }
}
