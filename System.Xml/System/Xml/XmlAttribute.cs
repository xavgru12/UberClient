// Decompiled with JetBrains decompiler
// Type: System.Xml.XmlAttribute
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using Mono.Xml;
using System.Text;
using System.Xml.Schema;
using System.Xml.XPath;

namespace System.Xml
{
  public class XmlAttribute : XmlNode, IHasXmlChildNode
  {
    private XmlNameEntry name;
    internal bool isDefault;
    private XmlLinkedNode lastLinkedChild;
    private IXmlSchemaInfo schemaInfo;

    protected internal XmlAttribute(
      string prefix,
      string localName,
      string namespaceURI,
      XmlDocument doc)
      : this(prefix, localName, namespaceURI, doc, false, true)
    {
    }

    internal XmlAttribute(
      string prefix,
      string localName,
      string namespaceURI,
      XmlDocument doc,
      bool atomizedNames,
      bool checkNamespace)
      : base(doc)
    {
      if (!atomizedNames)
      {
        if (prefix == null)
          prefix = string.Empty;
        if (namespaceURI == null)
          namespaceURI = string.Empty;
      }
      if (checkNamespace && (prefix == "xmlns" || prefix == string.Empty && localName == "xmlns"))
      {
        if (namespaceURI != "http://www.w3.org/2000/xmlns/")
          throw new ArgumentException("Invalid attribute namespace for namespace declaration.");
        if (prefix == "xml" && namespaceURI != "http://www.w3.org/XML/1998/namespace")
          throw new ArgumentException("Invalid attribute namespace for namespace declaration.");
      }
      if (!atomizedNames)
      {
        if (prefix != string.Empty && !XmlChar.IsName(prefix))
          throw new ArgumentException("Invalid attribute prefix.");
        if (!XmlChar.IsName(localName))
          throw new ArgumentException("Invalid attribute local name.");
        prefix = doc.NameTable.Add(prefix);
        localName = doc.NameTable.Add(localName);
        namespaceURI = doc.NameTable.Add(namespaceURI);
      }
      this.name = doc.NameCache.Add(prefix, localName, namespaceURI, true);
    }

    XmlLinkedNode IHasXmlChildNode.LastLinkedChild
    {
      get => this.lastLinkedChild;
      set => this.lastLinkedChild = value;
    }

    public override string BaseURI => this.OwnerElement != null ? this.OwnerElement.BaseURI : string.Empty;

    public override string InnerText
    {
      set => this.Value = value;
    }

    public override string InnerXml
    {
      set
      {
        this.RemoveAll();
        XmlParserContext context = new XmlParserContext(this.OwnerDocument.NameTable, this.ConstructNamespaceManager(), this.OwnerDocument.DocumentType == null ? (DTDObjectModel) null : this.OwnerDocument.DocumentType.DTD, this.BaseURI, this.XmlLang, this.XmlSpace, (Encoding) null);
        XmlTextReader reader = new XmlTextReader(value, XmlNodeType.Attribute, context);
        reader.XmlResolver = this.OwnerDocument.Resolver;
        reader.Read();
        this.OwnerDocument.ReadAttributeNodeValue((XmlReader) reader, this);
      }
    }

    public override string LocalName => this.name.LocalName;

    public override string Name => this.name.GetPrefixedName(this.OwnerDocument.NameCache);

    public override string NamespaceURI => this.name.NS;

    public override XmlNodeType NodeType => XmlNodeType.Attribute;

    internal override XPathNodeType XPathNodeType => XPathNodeType.Attribute;

    public override XmlDocument OwnerDocument => base.OwnerDocument;

    public virtual XmlElement OwnerElement => this.AttributeOwnerElement;

    public override XmlNode ParentNode => (XmlNode) null;

    public override string Prefix
    {
      set
      {
        if (this.IsReadOnly)
          throw new XmlException("This node is readonly.");
        if (this.name.Prefix == "xmlns" && value != "xmlns")
          throw new ArgumentException("Cannot bind to the reserved namespace.");
        value = this.OwnerDocument.NameTable.Add(value);
        this.name = this.OwnerDocument.NameCache.Add(value, this.name.LocalName, this.name.NS, true);
      }
      get => this.name.Prefix;
    }

    public override IXmlSchemaInfo SchemaInfo
    {
      get => this.schemaInfo;
      internal set => this.schemaInfo = value;
    }

    public virtual bool Specified => !this.isDefault;

    public override string Value
    {
      get => this.InnerText;
      set
      {
        if (this.IsReadOnly)
          throw new ArgumentException("Attempt to modify a read-only node.");
        this.OwnerDocument.CheckIdTableUpdate(this, this.InnerText, value);
        XmlNode firstChild = (XmlNode) (this.FirstChild as XmlCharacterData);
        if (firstChild == null)
        {
          this.RemoveAll();
          this.AppendChild((XmlNode) this.OwnerDocument.CreateTextNode(value), false);
        }
        else if (this.FirstChild.NextSibling != null)
        {
          this.RemoveAll();
          this.AppendChild((XmlNode) this.OwnerDocument.CreateTextNode(value), false);
        }
        else
          firstChild.Value = value;
        this.isDefault = false;
      }
    }

    internal override string XmlLang => this.OwnerElement != null ? this.OwnerElement.XmlLang : string.Empty;

    internal override XmlSpace XmlSpace => this.OwnerElement != null ? this.OwnerElement.XmlSpace : XmlSpace.None;

    public override XmlNode AppendChild(XmlNode child) => base.AppendChild(child);

    public override XmlNode InsertBefore(XmlNode newChild, XmlNode refChild) => base.InsertBefore(newChild, refChild);

    public override XmlNode InsertAfter(XmlNode newChild, XmlNode refChild) => base.InsertAfter(newChild, refChild);

    public override XmlNode PrependChild(XmlNode node) => base.PrependChild(node);

    public override XmlNode RemoveChild(XmlNode node) => base.RemoveChild(node);

    public override XmlNode ReplaceChild(XmlNode newChild, XmlNode oldChild) => base.ReplaceChild(newChild, oldChild);

    public override XmlNode CloneNode(bool deep)
    {
      XmlNode attribute = (XmlNode) this.OwnerDocument.CreateAttribute(this.name.Prefix, this.name.LocalName, this.name.NS, true, false);
      if (deep)
      {
        for (XmlNode xmlNode = this.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
          attribute.AppendChild(xmlNode.CloneNode(deep), false);
      }
      return attribute;
    }

    internal void SetDefault() => this.isDefault = true;

    public override void WriteContentTo(XmlWriter w)
    {
      for (XmlNode xmlNode = this.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
        xmlNode.WriteTo(w);
    }

    public override void WriteTo(XmlWriter w)
    {
      if (this.isDefault)
        return;
      w.WriteStartAttribute(this.name.NS.Length <= 0 ? string.Empty : this.name.Prefix, this.name.LocalName, this.name.NS);
      this.WriteContentTo(w);
      w.WriteEndAttribute();
    }

    internal DTDAttributeDefinition GetAttributeDefinition()
    {
      if (this.OwnerElement == null)
        return (DTDAttributeDefinition) null;
      return (this.OwnerDocument.DocumentType == null ? (DTDAttListDeclaration) null : this.OwnerDocument.DocumentType.DTD.AttListDecls[this.OwnerElement.Name])?[this.Name];
    }
  }
}
