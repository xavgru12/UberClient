// Decompiled with JetBrains decompiler
// Type: System.Xml.XmlNodeReaderImpl
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using Mono.Xml;
using System.Collections.Generic;
using System.Text;
using System.Xml.Schema;

namespace System.Xml
{
  internal class XmlNodeReaderImpl : XmlReader, IHasXmlParserContext, IXmlNamespaceResolver
  {
    private XmlDocument document;
    private XmlNode startNode;
    private XmlNode current;
    private XmlNode ownerLinkedNode;
    private ReadState state;
    private int depth;
    private bool isEndElement;
    private bool ignoreStartNode;

    internal XmlNodeReaderImpl(XmlNodeReaderImpl entityContainer)
      : this(entityContainer.current)
    {
    }

    public XmlNodeReaderImpl(XmlNode node)
    {
      this.startNode = node;
      this.depth = 0;
      this.document = this.startNode.NodeType != XmlNodeType.Document ? this.startNode.OwnerDocument : this.startNode as XmlDocument;
      XmlNodeType nodeType = node.NodeType;
      switch (nodeType)
      {
        case XmlNodeType.Document:
        case XmlNodeType.DocumentFragment:
          this.ignoreStartNode = true;
          break;
        default:
          if (nodeType != XmlNodeType.EntityReference)
            break;
          goto case XmlNodeType.Document;
      }
    }

    XmlParserContext IHasXmlParserContext.ParserContext => new XmlParserContext(this.document.NameTable, this.current != null ? this.current.ConstructNamespaceManager() : new XmlNamespaceManager(this.document.NameTable), this.document.DocumentType == null ? (DTDObjectModel) null : this.document.DocumentType.DTD, this.current != null ? this.current.BaseURI : this.document.BaseURI, this.XmlLang, this.XmlSpace, Encoding.Unicode);

    public override int AttributeCount
    {
      get
      {
        if (this.state != ReadState.Interactive || this.isEndElement || this.current == null)
          return 0;
        XmlNode ownerLinkedNode = this.ownerLinkedNode;
        return ownerLinkedNode.Attributes != null ? ownerLinkedNode.Attributes.Count : 0;
      }
    }

    public override string BaseURI => this.current == null ? this.startNode.BaseURI : this.current.BaseURI;

    public override bool CanReadBinaryContent => true;

    public override bool CanReadValueChunk => true;

    public override bool CanResolveEntity => false;

    public override int Depth
    {
      get
      {
        if (this.current == null)
          return 0;
        if (this.current == this.ownerLinkedNode)
          return this.depth;
        return this.current.NodeType == XmlNodeType.Attribute ? this.depth + 1 : this.depth + 2;
      }
    }

    public override bool EOF => this.state == ReadState.EndOfFile || this.state == ReadState.Error;

    public override bool HasAttributes
    {
      get
      {
        if (this.isEndElement || this.current == null)
          return false;
        XmlNode ownerLinkedNode = this.ownerLinkedNode;
        return ownerLinkedNode.Attributes != null && ownerLinkedNode.Attributes.Count != 0;
      }
    }

    public override bool HasValue
    {
      get
      {
        if (this.current == null)
          return false;
        XmlNodeType nodeType = this.current.NodeType;
        switch (nodeType)
        {
          case XmlNodeType.EntityReference:
          case XmlNodeType.Document:
          case XmlNodeType.DocumentFragment:
          case XmlNodeType.Notation:
          case XmlNodeType.EndElement:
          case XmlNodeType.EndEntity:
            return false;
          default:
            if (nodeType != XmlNodeType.Element)
              return true;
            goto case XmlNodeType.EntityReference;
        }
      }
    }

    public override bool IsDefault => this.current != null && this.current.NodeType == XmlNodeType.Attribute && !((XmlAttribute) this.current).Specified;

    public override bool IsEmptyElement => this.current != null && this.current.NodeType == XmlNodeType.Element && ((XmlElement) this.current).IsEmpty;

    public override string LocalName
    {
      get
      {
        if (this.current == null)
          return string.Empty;
        XmlNodeType nodeType = this.current.NodeType;
        switch (nodeType)
        {
          case XmlNodeType.Element:
          case XmlNodeType.Attribute:
          case XmlNodeType.EntityReference:
          case XmlNodeType.ProcessingInstruction:
          case XmlNodeType.DocumentType:
            return this.current.LocalName;
          default:
            if (nodeType != XmlNodeType.XmlDeclaration)
              return string.Empty;
            goto case XmlNodeType.Element;
        }
      }
    }

    public override string Name
    {
      get
      {
        if (this.current == null)
          return string.Empty;
        XmlNodeType nodeType = this.current.NodeType;
        switch (nodeType)
        {
          case XmlNodeType.Element:
          case XmlNodeType.Attribute:
          case XmlNodeType.EntityReference:
          case XmlNodeType.ProcessingInstruction:
          case XmlNodeType.DocumentType:
            return this.current.Name;
          default:
            if (nodeType != XmlNodeType.XmlDeclaration)
              return string.Empty;
            goto case XmlNodeType.Element;
        }
      }
    }

    public override string NamespaceURI => this.current == null ? string.Empty : this.current.NamespaceURI;

    public override XmlNameTable NameTable => this.document.NameTable;

    public override XmlNodeType NodeType
    {
      get
      {
        if (this.current == null)
          return XmlNodeType.None;
        return this.isEndElement ? XmlNodeType.EndElement : this.current.NodeType;
      }
    }

    public override string Prefix => this.current == null ? string.Empty : this.current.Prefix;

    public override ReadState ReadState => this.state;

    public override IXmlSchemaInfo SchemaInfo => this.current != null ? this.current.SchemaInfo : (IXmlSchemaInfo) null;

    public override string Value
    {
      get
      {
        if (this.NodeType == XmlNodeType.DocumentType)
          return ((XmlDocumentType) this.current).InternalSubset;
        return this.HasValue ? this.current.Value : string.Empty;
      }
    }

    public override string XmlLang => this.current == null ? this.startNode.XmlLang : this.current.XmlLang;

    public override XmlSpace XmlSpace => this.current == null ? this.startNode.XmlSpace : this.current.XmlSpace;

    public override void Close()
    {
      this.current = (XmlNode) null;
      this.state = ReadState.Closed;
    }

    public override string GetAttribute(int attributeIndex)
    {
      if (this.NodeType == XmlNodeType.XmlDeclaration)
      {
        XmlDeclaration current = this.current as XmlDeclaration;
        switch (attributeIndex)
        {
          case 0:
            return current.Version;
          case 1:
            if (current.Encoding != string.Empty)
              return current.Encoding;
            if (current.Standalone != string.Empty)
              return current.Standalone;
            break;
          case 2:
            if (current.Encoding != string.Empty && current.Standalone != null)
              return current.Standalone;
            break;
        }
        throw new ArgumentOutOfRangeException("Index out of range.");
      }
      if (this.NodeType == XmlNodeType.DocumentType)
      {
        XmlDocumentType current = this.current as XmlDocumentType;
        if (attributeIndex == 0)
        {
          if (current.PublicId != string.Empty)
            return current.PublicId;
          if (current.SystemId != string.Empty)
            return current.SystemId;
        }
        else if (attributeIndex == 1 && current.PublicId == string.Empty && current.SystemId != string.Empty)
          return current.SystemId;
        throw new ArgumentOutOfRangeException("Index out of range.");
      }
      if (this.isEndElement || this.current == null)
        return (string) null;
      if (attributeIndex < 0 || attributeIndex > this.AttributeCount)
        throw new ArgumentOutOfRangeException("Index out of range.");
      return this.ownerLinkedNode.Attributes[attributeIndex].Value;
    }

    public override string GetAttribute(string name)
    {
      if (this.isEndElement || this.current == null)
        return (string) null;
      if (this.NodeType == XmlNodeType.XmlDeclaration)
        return this.GetXmlDeclarationAttribute(name);
      if (this.NodeType == XmlNodeType.DocumentType)
        return this.GetDocumentTypeAttribute(name);
      if (this.ownerLinkedNode.Attributes == null)
        return (string) null;
      return this.ownerLinkedNode.Attributes[name]?.Value;
    }

    public override string GetAttribute(string name, string namespaceURI)
    {
      if (this.isEndElement || this.current == null)
        return (string) null;
      if (this.NodeType == XmlNodeType.XmlDeclaration)
        return this.GetXmlDeclarationAttribute(name);
      if (this.NodeType == XmlNodeType.DocumentType)
        return this.GetDocumentTypeAttribute(name);
      if (this.ownerLinkedNode.Attributes == null)
        return (string) null;
      return this.ownerLinkedNode.Attributes[name, namespaceURI]?.Value;
    }

    private string GetXmlDeclarationAttribute(string name)
    {
      XmlDeclaration current = this.current as XmlDeclaration;
      string key = name;
      if (key != null)
      {
        // ISSUE: reference to a compiler-generated field
        if (XmlNodeReaderImpl.\u003C\u003Ef__switch\u0024map4D == null)
        {
          // ISSUE: reference to a compiler-generated field
          XmlNodeReaderImpl.\u003C\u003Ef__switch\u0024map4D = new Dictionary<string, int>(3)
          {
            {
              "version",
              0
            },
            {
              "encoding",
              1
            },
            {
              "standalone",
              2
            }
          };
        }
        int num;
        // ISSUE: reference to a compiler-generated field
        if (XmlNodeReaderImpl.\u003C\u003Ef__switch\u0024map4D.TryGetValue(key, out num))
        {
          switch (num)
          {
            case 0:
              return current.Version;
            case 1:
              return current.Encoding != string.Empty ? current.Encoding : (string) null;
            case 2:
              return current.Standalone;
          }
        }
      }
      return (string) null;
    }

    private string GetDocumentTypeAttribute(string name)
    {
      XmlDocumentType current = this.current as XmlDocumentType;
      string key = name;
      if (key != null)
      {
        // ISSUE: reference to a compiler-generated field
        if (XmlNodeReaderImpl.\u003C\u003Ef__switch\u0024map4E == null)
        {
          // ISSUE: reference to a compiler-generated field
          XmlNodeReaderImpl.\u003C\u003Ef__switch\u0024map4E = new Dictionary<string, int>(2)
          {
            {
              "PUBLIC",
              0
            },
            {
              "SYSTEM",
              1
            }
          };
        }
        int num;
        // ISSUE: reference to a compiler-generated field
        if (XmlNodeReaderImpl.\u003C\u003Ef__switch\u0024map4E.TryGetValue(key, out num))
        {
          if (num == 0)
            return current.PublicId;
          if (num == 1)
            return current.SystemId;
        }
      }
      return (string) null;
    }

    public IDictionary<string, string> GetNamespacesInScope(XmlNamespaceScope scope)
    {
      IDictionary<string, string> namespacesInScope = (IDictionary<string, string>) new Dictionary<string, string>();
      XmlNode xmlNode = this.current;
      while (xmlNode.NodeType != XmlNodeType.Document)
      {
        for (int i = 0; i < this.current.Attributes.Count; ++i)
        {
          XmlAttribute attribute = this.current.Attributes[i];
          if (attribute.NamespaceURI == "http://www.w3.org/2000/xmlns/")
            namespacesInScope.Add(!(attribute.Prefix == "xmlns") ? string.Empty : attribute.LocalName, attribute.Value);
        }
        if (scope == XmlNamespaceScope.Local)
          return namespacesInScope;
        xmlNode = xmlNode.ParentNode;
        if (xmlNode == null)
          break;
      }
      if (scope == XmlNamespaceScope.All)
        namespacesInScope.Add("xml", "http://www.w3.org/XML/1998/namespace");
      return namespacesInScope;
    }

    private XmlElement GetCurrentElement()
    {
      XmlElement currentElement = (XmlElement) null;
      switch (this.current.NodeType)
      {
        case XmlNodeType.Element:
          currentElement = (XmlElement) this.current;
          break;
        case XmlNodeType.Attribute:
          currentElement = ((XmlAttribute) this.current).OwnerElement;
          break;
        case XmlNodeType.Text:
        case XmlNodeType.CDATA:
        case XmlNodeType.EntityReference:
        case XmlNodeType.ProcessingInstruction:
        case XmlNodeType.Comment:
        case XmlNodeType.Whitespace:
        case XmlNodeType.SignificantWhitespace:
          currentElement = this.current.ParentNode as XmlElement;
          break;
      }
      return currentElement;
    }

    public override string LookupNamespace(string prefix)
    {
      if (this.current == null)
        return (string) null;
      for (XmlElement xmlElement = this.GetCurrentElement(); xmlElement != null; xmlElement = xmlElement.ParentNode as XmlElement)
      {
        for (int i = 0; i < xmlElement.Attributes.Count; ++i)
        {
          XmlAttribute attribute = xmlElement.Attributes[i];
          if (!(attribute.NamespaceURI != "http://www.w3.org/2000/xmlns/"))
          {
            if (prefix == string.Empty)
            {
              if (attribute.Prefix == string.Empty)
                return attribute.Value;
            }
            else if (attribute.LocalName == prefix)
              return attribute.Value;
          }
        }
      }
      string key = prefix;
      if (key != null)
      {
        // ISSUE: reference to a compiler-generated field
        if (XmlNodeReaderImpl.\u003C\u003Ef__switch\u0024map4F == null)
        {
          // ISSUE: reference to a compiler-generated field
          XmlNodeReaderImpl.\u003C\u003Ef__switch\u0024map4F = new Dictionary<string, int>(2)
          {
            {
              "xml",
              0
            },
            {
              "xmlns",
              1
            }
          };
        }
        int num;
        // ISSUE: reference to a compiler-generated field
        if (XmlNodeReaderImpl.\u003C\u003Ef__switch\u0024map4F.TryGetValue(key, out num))
        {
          if (num == 0)
            return "http://www.w3.org/XML/1998/namespace";
          if (num == 1)
            return "http://www.w3.org/2000/xmlns/";
        }
      }
      return (string) null;
    }

    public string LookupPrefix(string ns) => this.LookupPrefix(ns, false);

    public string LookupPrefix(string ns, bool atomizedNames)
    {
      if (this.current == null)
        return (string) null;
      for (XmlElement xmlElement = this.GetCurrentElement(); xmlElement != null; xmlElement = xmlElement.ParentNode as XmlElement)
      {
        for (int i = 0; i < xmlElement.Attributes.Count; ++i)
        {
          XmlAttribute attribute = xmlElement.Attributes[i];
          if (atomizedNames)
          {
            if (object.ReferenceEquals((object) attribute.NamespaceURI, (object) "http://www.w3.org/2000/xmlns/") && object.ReferenceEquals((object) attribute.Value, (object) ns))
              return attribute.Prefix != string.Empty ? attribute.LocalName : string.Empty;
          }
          else if (!(attribute.NamespaceURI != "http://www.w3.org/2000/xmlns/") && attribute.Value == ns)
            return attribute.Prefix != string.Empty ? attribute.LocalName : string.Empty;
        }
      }
      string key = ns;
      if (key != null)
      {
        // ISSUE: reference to a compiler-generated field
        if (XmlNodeReaderImpl.\u003C\u003Ef__switch\u0024map50 == null)
        {
          // ISSUE: reference to a compiler-generated field
          XmlNodeReaderImpl.\u003C\u003Ef__switch\u0024map50 = new Dictionary<string, int>(2)
          {
            {
              "http://www.w3.org/XML/1998/namespace",
              0
            },
            {
              "http://www.w3.org/2000/xmlns/",
              1
            }
          };
        }
        int num;
        // ISSUE: reference to a compiler-generated field
        if (XmlNodeReaderImpl.\u003C\u003Ef__switch\u0024map50.TryGetValue(key, out num))
        {
          if (num == 0)
            return "xml";
          if (num == 1)
            return "xmlns";
        }
      }
      return (string) null;
    }

    public override void MoveToAttribute(int attributeIndex)
    {
      if (this.isEndElement || attributeIndex < 0 || attributeIndex > this.AttributeCount)
        throw new ArgumentOutOfRangeException();
      this.state = ReadState.Interactive;
      this.current = (XmlNode) this.ownerLinkedNode.Attributes[attributeIndex];
    }

    public override bool MoveToAttribute(string name)
    {
      if (this.isEndElement || this.current == null)
        return false;
      XmlNode current = this.current;
      if (this.current.ParentNode.NodeType == XmlNodeType.Attribute)
        this.current = this.current.ParentNode;
      if (this.ownerLinkedNode.Attributes == null)
        return false;
      XmlAttribute attribute = this.ownerLinkedNode.Attributes[name];
      if (attribute == null)
      {
        this.current = current;
        return false;
      }
      this.current = (XmlNode) attribute;
      return true;
    }

    public override bool MoveToAttribute(string name, string namespaceURI)
    {
      if (this.isEndElement || this.current == null || this.ownerLinkedNode.Attributes == null)
        return false;
      XmlAttribute attribute = this.ownerLinkedNode.Attributes[name, namespaceURI];
      if (attribute == null)
        return false;
      this.current = (XmlNode) attribute;
      return true;
    }

    public override bool MoveToElement()
    {
      if (this.current == null)
        return false;
      XmlNode ownerLinkedNode = this.ownerLinkedNode;
      if (this.current == ownerLinkedNode)
        return false;
      this.current = ownerLinkedNode;
      return true;
    }

    public override bool MoveToFirstAttribute()
    {
      if (this.current == null || this.ownerLinkedNode.Attributes == null || this.ownerLinkedNode.Attributes.Count <= 0)
        return false;
      this.current = (XmlNode) this.ownerLinkedNode.Attributes[0];
      return true;
    }

    public override bool MoveToNextAttribute()
    {
      if (this.current == null)
        return false;
      XmlNode xmlNode = this.current;
      if (this.current.NodeType != XmlNodeType.Attribute)
      {
        if (this.current.ParentNode == null || this.current.ParentNode.NodeType != XmlNodeType.Attribute)
          return this.MoveToFirstAttribute();
        xmlNode = this.current.ParentNode;
      }
      XmlAttributeCollection attributes = ((XmlAttribute) xmlNode).OwnerElement.Attributes;
      for (int i1 = 0; i1 < attributes.Count - 1; ++i1)
      {
        if (attributes[i1] == xmlNode)
        {
          int i2 = i1 + 1;
          if (i2 == attributes.Count)
            return false;
          this.current = (XmlNode) attributes[i2];
          return true;
        }
      }
      return false;
    }

    public override bool Read()
    {
      switch (this.state)
      {
        case ReadState.Error:
        case ReadState.EndOfFile:
        case ReadState.Closed:
          return false;
        default:
          if (this.Binary != null)
            this.Binary.Reset();
          bool flag = this.ReadContent();
          this.ownerLinkedNode = this.current;
          return flag;
      }
    }

    private bool ReadContent()
    {
      if (this.ReadState == ReadState.Initial)
      {
        this.current = this.startNode;
        this.state = ReadState.Interactive;
        if (this.ignoreStartNode)
          this.current = this.startNode.FirstChild;
        if (this.current != null)
          return true;
        this.state = ReadState.Error;
        return false;
      }
      this.MoveToElement();
      XmlNode firstChild = this.isEndElement || this.current.NodeType == XmlNodeType.EntityReference ? (XmlNode) null : this.current.FirstChild;
      if (firstChild != null)
      {
        this.isEndElement = false;
        this.current = firstChild;
        ++this.depth;
        return true;
      }
      if (this.current == this.startNode)
      {
        if (this.IsEmptyElement || this.isEndElement)
        {
          this.isEndElement = false;
          this.current = (XmlNode) null;
          this.state = ReadState.EndOfFile;
          return false;
        }
        this.isEndElement = true;
        return true;
      }
      if (!this.isEndElement && !this.IsEmptyElement && this.current.NodeType == XmlNodeType.Element)
      {
        this.isEndElement = true;
        return true;
      }
      XmlNode nextSibling = this.current.NextSibling;
      if (nextSibling != null)
      {
        this.isEndElement = false;
        this.current = nextSibling;
        return true;
      }
      XmlNode parentNode = this.current.ParentNode;
      if (parentNode == null || parentNode == this.startNode && this.ignoreStartNode)
      {
        this.isEndElement = false;
        this.current = (XmlNode) null;
        this.state = ReadState.EndOfFile;
        return false;
      }
      this.current = parentNode;
      --this.depth;
      this.isEndElement = true;
      return true;
    }

    public override bool ReadAttributeValue()
    {
      if (this.current.NodeType == XmlNodeType.Attribute)
      {
        if (this.current.FirstChild == null)
          return false;
        this.current = this.current.FirstChild;
        return true;
      }
      if (this.current.ParentNode.NodeType != XmlNodeType.Attribute || this.current.NextSibling == null)
        return false;
      this.current = this.current.NextSibling;
      return true;
    }

    public override string ReadString() => base.ReadString();

    public override void ResolveEntity() => throw new NotSupportedException("Should not happen.");

    public override void Skip() => base.Skip();
  }
}
