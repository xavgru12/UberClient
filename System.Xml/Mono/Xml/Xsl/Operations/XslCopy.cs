// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.Operations.XslCopy
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
  internal class XslCopy : XslCompiledElement
  {
    private XslOperation children;
    private XmlQualifiedName[] useAttributeSets;
    private Hashtable nsDecls;

    public XslCopy(Compiler c)
      : base(c)
    {
    }

    protected override void Compile(Compiler c)
    {
      if (c.Debugger != null)
        c.Debugger.DebugCompile(c.Input);
      this.nsDecls = c.GetNamespacesToCopy();
      if (this.nsDecls.Count == 0)
        this.nsDecls = (Hashtable) null;
      c.CheckExtraAttributes("copy", "use-attribute-sets");
      this.useAttributeSets = c.ParseQNameListAttribute("use-attribute-sets");
      if (!c.Input.MoveToFirstChild())
        return;
      this.children = c.CompileTemplateContent();
      c.Input.MoveToParent();
    }

    public override void Evaluate(XslTransformProcessor p)
    {
      if (p.Debugger != null)
        p.Debugger.DebugExecute(p, this.DebugInput);
      XPathNavigator currentNode = p.CurrentNode;
      switch (currentNode.NodeType)
      {
        case XPathNodeType.Root:
          if (p.Out.CanProcessAttributes && this.useAttributeSets != null)
          {
            foreach (XmlQualifiedName useAttributeSet in this.useAttributeSets)
              (p.ResolveAttributeSet(useAttributeSet) ?? throw new XsltException("Attribute set was not found", (Exception) null, currentNode)).Evaluate(p);
          }
          if (this.children == null)
            break;
          this.children.Evaluate(p);
          break;
        case XPathNodeType.Element:
          bool insideCdataElement = p.InsideCDataElement;
          string prefix = currentNode.Prefix;
          p.PushElementState(prefix, currentNode.LocalName, currentNode.NamespaceURI, true);
          p.Out.WriteStartElement(prefix, currentNode.LocalName, currentNode.NamespaceURI);
          if (this.useAttributeSets != null)
          {
            foreach (XmlQualifiedName useAttributeSet in this.useAttributeSets)
              p.ResolveAttributeSet(useAttributeSet).Evaluate(p);
          }
          if (currentNode.MoveToFirstNamespace(XPathNamespaceScope.ExcludeXml))
          {
            do
            {
              if (!(currentNode.LocalName == prefix))
                p.Out.WriteNamespaceDecl(currentNode.LocalName, currentNode.Value);
            }
            while (currentNode.MoveToNextNamespace(XPathNamespaceScope.ExcludeXml));
            currentNode.MoveToParent();
          }
          if (this.children != null)
            this.children.Evaluate(p);
          p.Out.WriteFullEndElement();
          p.PopCDataState(insideCdataElement);
          break;
        case XPathNodeType.Attribute:
          p.Out.WriteAttributeString(currentNode.Prefix, currentNode.LocalName, currentNode.NamespaceURI, currentNode.Value);
          break;
        case XPathNodeType.Namespace:
          if (!(p.XPathContext.ElementPrefix != currentNode.Name))
            break;
          p.Out.WriteNamespaceDecl(currentNode.Name, currentNode.Value);
          break;
        case XPathNodeType.Text:
          p.Out.WriteString(currentNode.Value);
          break;
        case XPathNodeType.SignificantWhitespace:
        case XPathNodeType.Whitespace:
          bool insideCdataSection = p.Out.InsideCDataSection;
          p.Out.InsideCDataSection = false;
          p.Out.WriteString(currentNode.Value);
          p.Out.InsideCDataSection = insideCdataSection;
          break;
        case XPathNodeType.ProcessingInstruction:
          p.Out.WriteProcessingInstruction(currentNode.Name, currentNode.Value);
          break;
        case XPathNodeType.Comment:
          p.Out.WriteComment(currentNode.Value);
          break;
      }
    }
  }
}
