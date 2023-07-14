// Decompiled with JetBrains decompiler
// Type: System.Xml.XmlDocumentFragment
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using Mono.Xml;
using System.Text;
using System.Xml.XPath;

namespace System.Xml
{
  public class XmlDocumentFragment : XmlNode, IHasXmlChildNode
  {
    private XmlLinkedNode lastLinkedChild;

    protected internal XmlDocumentFragment(XmlDocument doc)
      : base(doc)
    {
    }

    XmlLinkedNode IHasXmlChildNode.LastLinkedChild
    {
      get => this.lastLinkedChild;
      set => this.lastLinkedChild = value;
    }

    public override string InnerXml
    {
      set
      {
        for (int i = 0; i < this.ChildNodes.Count; ++i)
          this.RemoveChild(this.ChildNodes[i]);
        XmlParserContext context = new XmlParserContext(this.OwnerDocument.NameTable, this.ConstructNamespaceManager(), this.OwnerDocument.DocumentType == null ? (DTDObjectModel) null : this.OwnerDocument.DocumentType.DTD, this.BaseURI, this.XmlLang, this.XmlSpace, (Encoding) null);
        XmlTextReader reader = new XmlTextReader(value, XmlNodeType.Element, context);
        reader.XmlResolver = this.OwnerDocument.Resolver;
        while (true)
        {
          XmlNode newChild = this.OwnerDocument.ReadNode((XmlReader) reader);
          if (newChild != null)
            this.AppendChild(newChild);
          else
            break;
        }
      }
      get
      {
        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < this.ChildNodes.Count; ++i)
          stringBuilder.Append(this.ChildNodes[i].OuterXml);
        return stringBuilder.ToString();
      }
    }

    public override string LocalName => "#document-fragment";

    public override string Name => "#document-fragment";

    public override XmlNodeType NodeType => XmlNodeType.DocumentFragment;

    public override XmlDocument OwnerDocument => base.OwnerDocument;

    public override XmlNode ParentNode => (XmlNode) null;

    internal override XPathNodeType XPathNodeType => XPathNodeType.Root;

    public override XmlNode CloneNode(bool deep)
    {
      if (!deep)
        return (XmlNode) new XmlDocumentFragment(this.OwnerDocument);
      XmlNode xmlNode;
      for (xmlNode = this.FirstChild; xmlNode != null && xmlNode.HasChildNodes; xmlNode = xmlNode.NextSibling)
        this.AppendChild(xmlNode.NextSibling.CloneNode(false));
      return xmlNode;
    }

    public override void WriteContentTo(XmlWriter w)
    {
      for (int i = 0; i < this.ChildNodes.Count; ++i)
        this.ChildNodes[i].WriteContentTo(w);
    }

    public override void WriteTo(XmlWriter w)
    {
      for (int i = 0; i < this.ChildNodes.Count; ++i)
        this.ChildNodes[i].WriteTo(w);
    }
  }
}
