// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.Operations.XslLiteralElement
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.Xml;
using System.Xml.XPath;

namespace Mono.Xml.Xsl.Operations
{
  internal class XslLiteralElement : XslCompiledElement
  {
    private XslOperation children;
    private string localname;
    private string prefix;
    private string nsUri;
    private bool isEmptyElement;
    private ArrayList attrs;
    private XmlQualifiedName[] useAttributeSets;
    private Hashtable nsDecls;

    public XslLiteralElement(Compiler c)
      : base(c)
    {
    }

    protected override void Compile(Compiler c)
    {
      if (c.Debugger != null)
        c.Debugger.DebugCompile(this.DebugInput);
      this.prefix = c.Input.Prefix;
      string actualPrefix = c.CurrentStylesheet.GetActualPrefix(this.prefix);
      if (actualPrefix != this.prefix)
      {
        this.prefix = actualPrefix;
        this.nsUri = c.Input.GetNamespace(actualPrefix);
      }
      else
        this.nsUri = c.Input.NamespaceURI;
      this.localname = c.Input.LocalName;
      this.useAttributeSets = c.ParseQNameListAttribute("use-attribute-sets", "http://www.w3.org/1999/XSL/Transform");
      this.nsDecls = c.GetNamespacesToCopy();
      if (this.nsDecls.Count == 0)
        this.nsDecls = (Hashtable) null;
      this.isEmptyElement = c.Input.IsEmptyElement;
      if (c.Input.MoveToFirstAttribute())
      {
        this.attrs = new ArrayList();
        do
        {
          if (!(c.Input.NamespaceURI == "http://www.w3.org/1999/XSL/Transform"))
            this.attrs.Add((object) new XslLiteralElement.XslLiteralAttribute(c));
        }
        while (c.Input.MoveToNextAttribute());
        c.Input.MoveToParent();
      }
      if (!c.Input.MoveToFirstChild())
        return;
      this.children = c.CompileTemplateContent();
      c.Input.MoveToParent();
    }

    public override void Evaluate(XslTransformProcessor p)
    {
      if (p.Debugger != null)
        p.Debugger.DebugExecute(p, this.DebugInput);
      bool insideCdataElement = p.InsideCDataElement;
      p.PushElementState(this.prefix, this.localname, this.nsUri, true);
      p.Out.WriteStartElement(this.prefix, this.localname, this.nsUri);
      if (this.useAttributeSets != null)
      {
        foreach (XmlQualifiedName useAttributeSet in this.useAttributeSets)
          p.ResolveAttributeSet(useAttributeSet).Evaluate(p);
      }
      if (this.attrs != null)
      {
        int count = this.attrs.Count;
        for (int index = 0; index < count; ++index)
          ((XslLiteralElement.XslLiteralAttribute) this.attrs[index]).Evaluate(p);
      }
      p.OutputLiteralNamespaceUriNodes(this.nsDecls, (ArrayList) null, (string) null);
      if (this.children != null)
        this.children.Evaluate(p);
      if (this.isEmptyElement)
        p.Out.WriteEndElement();
      else
        p.Out.WriteFullEndElement();
      p.PopCDataState(insideCdataElement);
    }

    private class XslLiteralAttribute
    {
      private string localname;
      private string prefix;
      private string nsUri;
      private XslAvt val;

      public XslLiteralAttribute(Compiler c)
      {
        this.prefix = c.Input.Prefix;
        if (this.prefix.Length > 0)
        {
          string actualPrefix = c.CurrentStylesheet.GetActualPrefix(this.prefix);
          if (actualPrefix != this.prefix)
          {
            this.prefix = actualPrefix;
            XPathNavigator xpathNavigator = c.Input.Clone();
            xpathNavigator.MoveToParent();
            this.nsUri = xpathNavigator.GetNamespace(actualPrefix);
          }
          else
            this.nsUri = c.Input.NamespaceURI;
        }
        else
          this.nsUri = string.Empty;
        this.localname = c.Input.LocalName;
        this.val = new XslAvt(c.Input.Value, c);
      }

      public void Evaluate(XslTransformProcessor p) => p.Out.WriteAttributeString(this.prefix, this.localname, this.nsUri, this.val.Evaluate(p));
    }
  }
}
