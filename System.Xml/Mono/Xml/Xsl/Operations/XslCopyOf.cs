// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.Operations.XslCopyOf
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Xml.XPath;

namespace Mono.Xml.Xsl.Operations
{
  internal class XslCopyOf : XslCompiledElement
  {
    private XPathExpression select;

    public XslCopyOf(Compiler c)
      : base(c)
    {
    }

    protected override void Compile(Compiler c)
    {
      if (c.Debugger != null)
        c.Debugger.DebugCompile(c.Input);
      c.CheckExtraAttributes("copy-of", "select");
      c.AssertAttribute("select");
      this.select = (XPathExpression) c.CompileExpression(c.GetAttribute("select"));
    }

    private void CopyNode(XslTransformProcessor p, XPathNavigator nav)
    {
      Outputter outputter = p.Out;
      switch (nav.NodeType)
      {
        case XPathNodeType.Root:
          XPathNodeIterator xpathNodeIterator = nav.SelectChildren(XPathNodeType.All);
          while (xpathNodeIterator.MoveNext())
            this.CopyNode(p, xpathNodeIterator.Current);
          break;
        case XPathNodeType.Element:
          bool insideCdataElement = p.InsideCDataElement;
          string prefix = nav.Prefix;
          string namespaceUri = nav.NamespaceURI;
          p.PushElementState(prefix, nav.LocalName, namespaceUri, false);
          outputter.WriteStartElement(prefix, nav.LocalName, namespaceUri);
          if (nav.MoveToFirstNamespace(XPathNamespaceScope.ExcludeXml))
          {
            do
            {
              if (!(prefix == nav.Name) && (nav.Name.Length != 0 || namespaceUri.Length != 0))
                outputter.WriteNamespaceDecl(nav.Name, nav.Value);
            }
            while (nav.MoveToNextNamespace(XPathNamespaceScope.ExcludeXml));
            nav.MoveToParent();
          }
          if (nav.MoveToFirstAttribute())
          {
            do
            {
              outputter.WriteAttributeString(nav.Prefix, nav.LocalName, nav.NamespaceURI, nav.Value);
            }
            while (nav.MoveToNextAttribute());
            nav.MoveToParent();
          }
          if (nav.MoveToFirstChild())
          {
            do
            {
              this.CopyNode(p, nav);
            }
            while (nav.MoveToNext());
            nav.MoveToParent();
          }
          if (nav.IsEmptyElement)
            outputter.WriteEndElement();
          else
            outputter.WriteFullEndElement();
          p.PopCDataState(insideCdataElement);
          break;
        case XPathNodeType.Attribute:
          outputter.WriteAttributeString(nav.Prefix, nav.LocalName, nav.NamespaceURI, nav.Value);
          break;
        case XPathNodeType.Namespace:
          if (!(nav.Name != p.XPathContext.ElementPrefix) || p.XPathContext.ElementNamespace.Length <= 0 && nav.Name.Length <= 0)
            break;
          outputter.WriteNamespaceDecl(nav.Name, nav.Value);
          break;
        case XPathNodeType.Text:
          outputter.WriteString(nav.Value);
          break;
        case XPathNodeType.SignificantWhitespace:
        case XPathNodeType.Whitespace:
          bool insideCdataSection = outputter.InsideCDataSection;
          outputter.InsideCDataSection = false;
          outputter.WriteString(nav.Value);
          outputter.InsideCDataSection = insideCdataSection;
          break;
        case XPathNodeType.ProcessingInstruction:
          outputter.WriteProcessingInstruction(nav.Name, nav.Value);
          break;
        case XPathNodeType.Comment:
          outputter.WriteComment(nav.Value);
          break;
      }
    }

    public override void Evaluate(XslTransformProcessor p)
    {
      if (p.Debugger != null)
        p.Debugger.DebugExecute(p, this.DebugInput);
      object obj = p.Evaluate(this.select);
      switch (obj)
      {
        case XPathNodeIterator xpathNodeIterator:
          while (xpathNodeIterator.MoveNext())
            this.CopyNode(p, xpathNodeIterator.Current);
          break;
        case XPathNavigator nav:
          this.CopyNode(p, nav);
          break;
        default:
          p.Out.WriteString(XPathFunctions.ToString(obj));
          break;
      }
    }
  }
}
