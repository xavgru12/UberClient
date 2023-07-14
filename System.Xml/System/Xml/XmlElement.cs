// Decompiled with JetBrains decompiler
// Type: System.Xml.XmlElement
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using Mono.Xml;
using System.Collections;
using System.Text;
using System.Xml.Schema;
using System.Xml.XPath;

namespace System.Xml
{
  public class XmlElement : XmlLinkedNode, IHasXmlChildNode
  {
    private XmlAttributeCollection attributes;
    private XmlNameEntry name;
    private XmlLinkedNode lastLinkedChild;
    private bool isNotEmpty;
    private IXmlSchemaInfo schemaInfo;

    protected internal XmlElement(
      string prefix,
      string localName,
      string namespaceURI,
      XmlDocument doc)
      : this(prefix, localName, namespaceURI, doc, false)
    {
    }

    internal XmlElement(
      string prefix,
      string localName,
      string namespaceURI,
      XmlDocument doc,
      bool atomizedNames)
      : base(doc)
    {
      if (!atomizedNames)
      {
        if (prefix == null)
          prefix = string.Empty;
        if (namespaceURI == null)
          namespaceURI = string.Empty;
        XmlConvert.VerifyName(localName);
        prefix = doc.NameTable.Add(prefix);
        localName = doc.NameTable.Add(localName);
        namespaceURI = doc.NameTable.Add(namespaceURI);
      }
      this.name = doc.NameCache.Add(prefix, localName, namespaceURI, true);
      if (doc.DocumentType == null)
        return;
      DTDAttListDeclaration attListDecl = doc.DocumentType.DTD.AttListDecls[localName];
      if (attListDecl == null)
        return;
      for (int i = 0; i < attListDecl.Definitions.Count; ++i)
      {
        DTDAttributeDefinition attributeDefinition = attListDecl[i];
        if (attributeDefinition.DefaultValue != null)
        {
          this.SetAttribute(attributeDefinition.Name, attributeDefinition.DefaultValue);
          this.Attributes[attributeDefinition.Name].SetDefault();
        }
      }
    }

    XmlLinkedNode IHasXmlChildNode.LastLinkedChild
    {
      get => this.lastLinkedChild;
      set => this.lastLinkedChild = value;
    }

    public override XmlAttributeCollection Attributes
    {
      get
      {
        if (this.attributes == null)
          this.attributes = new XmlAttributeCollection((XmlNode) this);
        return this.attributes;
      }
    }

    public virtual bool HasAttributes => this.attributes != null && this.attributes.Count > 0;

    public override string InnerText
    {
      get => base.InnerText;
      set
      {
        if (this.FirstChild != null && this.FirstChild.NextSibling == null && this.FirstChild.NodeType == XmlNodeType.Text)
        {
          this.FirstChild.Value = value;
        }
        else
        {
          while (this.FirstChild != null)
            this.RemoveChild(this.FirstChild);
          this.AppendChild((XmlNode) this.OwnerDocument.CreateTextNode(value), false);
        }
      }
    }

    public override string InnerXml
    {
      get => base.InnerXml;
      set
      {
        while (this.FirstChild != null)
          this.RemoveChild(this.FirstChild);
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
    }

    public bool IsEmpty
    {
      get => !this.isNotEmpty && this.FirstChild == null;
      set
      {
        this.isNotEmpty = !value;
        if (!value)
          return;
        while (this.FirstChild != null)
          this.RemoveChild(this.FirstChild);
      }
    }

    public override string LocalName => this.name.LocalName;

    public override string Name => this.name.GetPrefixedName(this.OwnerDocument.NameCache);

    public override string NamespaceURI => this.name.NS;

    public override XmlNode NextSibling => this.ParentNode == null || ((IHasXmlChildNode) this.ParentNode).LastLinkedChild == this ? (XmlNode) null : (XmlNode) this.NextLinkedSibling;

    public override XmlNodeType NodeType => XmlNodeType.Element;

    internal override XPathNodeType XPathNodeType => XPathNodeType.Element;

    public override XmlDocument OwnerDocument => base.OwnerDocument;

    public override string Prefix
    {
      get => this.name.Prefix;
      set
      {
        if (this.IsReadOnly)
          throw new ArgumentException("This node is readonly.");
        if (value == null)
          value = string.Empty;
        if (!string.Empty.Equals(value) && !XmlChar.IsNCName(value))
          throw new ArgumentException("Specified name is not a valid NCName: " + value);
        value = this.OwnerDocument.NameTable.Add(value);
        this.name = this.OwnerDocument.NameCache.Add(value, this.name.LocalName, this.name.NS, true);
      }
    }

    public override XmlNode ParentNode => base.ParentNode;

    public override IXmlSchemaInfo SchemaInfo
    {
      get => this.schemaInfo;
      internal set => this.schemaInfo = value;
    }

    public override XmlNode CloneNode(bool deep)
    {
      XmlElement element = this.OwnerDocument.CreateElement(this.name.Prefix, this.name.LocalName, this.name.NS, true);
      for (int i = 0; i < this.Attributes.Count; ++i)
        element.SetAttributeNode((XmlAttribute) this.Attributes[i].CloneNode(true));
      if (deep)
      {
        for (int i = 0; i < this.ChildNodes.Count; ++i)
          element.AppendChild(this.ChildNodes[i].CloneNode(true), false);
      }
      return (XmlNode) element;
    }

    public virtual string GetAttribute(string name)
    {
      XmlNode namedItem = this.Attributes.GetNamedItem(name);
      return namedItem != null ? namedItem.Value : string.Empty;
    }

    public virtual string GetAttribute(string localName, string namespaceURI)
    {
      XmlNode namedItem = this.Attributes.GetNamedItem(localName, namespaceURI);
      return namedItem != null ? namedItem.Value : string.Empty;
    }

    public virtual XmlAttribute GetAttributeNode(string name)
    {
      XmlNode namedItem = this.Attributes.GetNamedItem(name);
      return namedItem != null ? namedItem as XmlAttribute : (XmlAttribute) null;
    }

    public virtual XmlAttribute GetAttributeNode(string localName, string namespaceURI)
    {
      XmlNode namedItem = this.Attributes.GetNamedItem(localName, namespaceURI);
      return namedItem != null ? namedItem as XmlAttribute : (XmlAttribute) null;
    }

    public virtual XmlNodeList GetElementsByTagName(string name)
    {
      ArrayList arrayList = new ArrayList();
      this.SearchDescendantElements(name, name == "*", arrayList);
      return (XmlNodeList) new XmlNodeArrayList(arrayList);
    }

    public virtual XmlNodeList GetElementsByTagName(string localName, string namespaceURI)
    {
      ArrayList arrayList = new ArrayList();
      this.SearchDescendantElements(localName, localName == "*", namespaceURI, namespaceURI == "*", arrayList);
      return (XmlNodeList) new XmlNodeArrayList(arrayList);
    }

    public virtual bool HasAttribute(string name) => this.Attributes.GetNamedItem(name) != null;

    public virtual bool HasAttribute(string localName, string namespaceURI) => this.Attributes.GetNamedItem(localName, namespaceURI) != null;

    public override void RemoveAll() => base.RemoveAll();

    public virtual void RemoveAllAttributes()
    {
      if (this.attributes == null)
        return;
      this.attributes.RemoveAll();
    }

    public virtual void RemoveAttribute(string name)
    {
      if (this.attributes == null || !(this.Attributes.GetNamedItem(name) is XmlAttribute namedItem))
        return;
      this.Attributes.Remove(namedItem);
    }

    public virtual void RemoveAttribute(string localName, string namespaceURI)
    {
      if (this.attributes == null || !(this.attributes.GetNamedItem(localName, namespaceURI) is XmlAttribute namedItem))
        return;
      this.Attributes.Remove(namedItem);
    }

    public virtual XmlNode RemoveAttributeAt(int i) => this.attributes == null || this.attributes.Count <= i ? (XmlNode) null : (XmlNode) this.Attributes.RemoveAt(i);

    public virtual XmlAttribute RemoveAttributeNode(XmlAttribute oldAttr) => this.attributes == null ? (XmlAttribute) null : this.Attributes.Remove(oldAttr);

    public virtual XmlAttribute RemoveAttributeNode(string localName, string namespaceURI) => this.attributes == null ? (XmlAttribute) null : this.Attributes.Remove(this.attributes[localName, namespaceURI]);

    public virtual void SetAttribute(string name, string value)
    {
      XmlAttribute attribute1 = this.Attributes[name];
      if (attribute1 == null)
      {
        XmlAttribute attribute2 = this.OwnerDocument.CreateAttribute(name);
        attribute2.Value = value;
        this.Attributes.SetNamedItem((XmlNode) attribute2);
      }
      else
        attribute1.Value = value;
    }

    public virtual string SetAttribute(string localName, string namespaceURI, string value)
    {
      XmlAttribute attribute = this.Attributes[localName, namespaceURI];
      if (attribute == null)
      {
        attribute = this.OwnerDocument.CreateAttribute(localName, namespaceURI);
        attribute.Value = value;
        this.Attributes.SetNamedItem((XmlNode) attribute);
      }
      else
        attribute.Value = value;
      return attribute.Value;
    }

    public virtual XmlAttribute SetAttributeNode(XmlAttribute newAttr)
    {
      XmlAttribute xmlAttribute = newAttr.OwnerElement == null ? this.Attributes.SetNamedItem((XmlNode) newAttr) as XmlAttribute : throw new InvalidOperationException("Specified attribute is already an attribute of another element.");
      return xmlAttribute == newAttr ? (XmlAttribute) null : xmlAttribute;
    }

    public virtual XmlAttribute SetAttributeNode(string localName, string namespaceURI)
    {
      XmlConvert.VerifyNCName(localName);
      return this.Attributes.Append(this.OwnerDocument.CreateAttribute(string.Empty, localName, namespaceURI, false, true));
    }

    public override void WriteContentTo(XmlWriter w)
    {
      for (XmlNode xmlNode = this.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
        xmlNode.WriteTo(w);
    }

    public override void WriteTo(XmlWriter w)
    {
      w.WriteStartElement(this.name.NS == null || this.name.NS.Length == 0 ? string.Empty : this.name.Prefix, this.name.LocalName, this.name.NS);
      if (this.HasAttributes)
      {
        for (int i = 0; i < this.Attributes.Count; ++i)
          this.Attributes[i].WriteTo(w);
      }
      this.WriteContentTo(w);
      if (this.IsEmpty)
        w.WriteEndElement();
      else
        w.WriteFullEndElement();
    }
  }
}
