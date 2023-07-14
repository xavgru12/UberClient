// Decompiled with JetBrains decompiler
// Type: System.Xml.XmlDocument
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using Mono.Xml;
using Mono.Xml.XPath;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml.Schema;
using System.Xml.XPath;

namespace System.Xml
{
  public class XmlDocument : XmlNode, IHasXmlChildNode
  {
    private static readonly Type[] optimal_create_types = new Type[3]
    {
      typeof (string),
      typeof (string),
      typeof (string)
    };
    private bool optimal_create_element;
    private bool optimal_create_attribute;
    private XmlNameTable nameTable;
    private string baseURI = string.Empty;
    private XmlImplementation implementation;
    private bool preserveWhitespace;
    private XmlResolver resolver;
    private Hashtable idTable = new Hashtable();
    private XmlNameEntryCache nameCache;
    private XmlLinkedNode lastLinkedChild;
    private XmlAttribute nsNodeXml;
    private XmlSchemaSet schemas;
    private IXmlSchemaInfo schemaInfo;
    private bool loadMode;

    public XmlDocument()
      : this((XmlImplementation) null, (XmlNameTable) null)
    {
    }

    protected internal XmlDocument(XmlImplementation imp)
      : this(imp, (XmlNameTable) null)
    {
    }

    public XmlDocument(XmlNameTable nt)
      : this((XmlImplementation) null, nt)
    {
    }

    private XmlDocument(XmlImplementation impl, XmlNameTable nt)
      : base((XmlDocument) null)
    {
      this.implementation = impl != null ? impl : new XmlImplementation();
      this.nameTable = nt == null ? this.implementation.InternalNameTable : nt;
      this.nameCache = new XmlNameEntryCache(this.nameTable);
      this.AddDefaultNameTableKeys();
      this.resolver = (XmlResolver) new XmlUrlResolver();
      Type type = this.GetType();
      this.optimal_create_element = type.GetMethod("CreateElement", XmlDocument.optimal_create_types).DeclaringType == typeof (XmlDocument);
      this.optimal_create_attribute = type.GetMethod("CreateAttribute", XmlDocument.optimal_create_types).DeclaringType == typeof (XmlDocument);
    }

    public event XmlNodeChangedEventHandler NodeChanged;

    public event XmlNodeChangedEventHandler NodeChanging;

    public event XmlNodeChangedEventHandler NodeInserted;

    public event XmlNodeChangedEventHandler NodeInserting;

    public event XmlNodeChangedEventHandler NodeRemoved;

    public event XmlNodeChangedEventHandler NodeRemoving;

    XmlLinkedNode IHasXmlChildNode.LastLinkedChild
    {
      get => this.lastLinkedChild;
      set => this.lastLinkedChild = value;
    }

    internal XmlAttribute NsNodeXml
    {
      get
      {
        if (this.nsNodeXml == null)
        {
          this.nsNodeXml = this.CreateAttribute("xmlns", "xml", "http://www.w3.org/2000/xmlns/");
          this.nsNodeXml.Value = "http://www.w3.org/XML/1998/namespace";
        }
        return this.nsNodeXml;
      }
    }

    public override string BaseURI => this.baseURI;

    public XmlElement DocumentElement
    {
      get
      {
        XmlNode xmlNode = this.FirstChild;
        while (true)
        {
          switch (xmlNode)
          {
            case null:
            case XmlElement _:
              goto label_3;
            default:
              xmlNode = xmlNode.NextSibling;
              continue;
          }
        }
label_3:
        return xmlNode != null ? xmlNode as XmlElement : (XmlElement) null;
      }
    }

    public virtual XmlDocumentType DocumentType
    {
      get
      {
        for (XmlNode documentType = this.FirstChild; documentType != null; documentType = documentType.NextSibling)
        {
          if (documentType.NodeType == XmlNodeType.DocumentType)
            return (XmlDocumentType) documentType;
          if (documentType.NodeType == XmlNodeType.Element)
            return (XmlDocumentType) null;
        }
        return (XmlDocumentType) null;
      }
    }

    public XmlImplementation Implementation => this.implementation;

    public override string InnerXml
    {
      get => base.InnerXml;
      set => this.LoadXml(value);
    }

    public override bool IsReadOnly => false;

    internal bool IsStandalone => this.FirstChild != null && this.FirstChild.NodeType == XmlNodeType.XmlDeclaration && ((XmlDeclaration) this.FirstChild).Standalone == "yes";

    public override string LocalName => "#document";

    public override string Name => "#document";

    internal XmlNameEntryCache NameCache => this.nameCache;

    public XmlNameTable NameTable => this.nameTable;

    public override XmlNodeType NodeType => XmlNodeType.Document;

    internal override XPathNodeType XPathNodeType => XPathNodeType.Root;

    public override XmlDocument OwnerDocument => (XmlDocument) null;

    public bool PreserveWhitespace
    {
      get => this.preserveWhitespace;
      set => this.preserveWhitespace = value;
    }

    internal XmlResolver Resolver => this.resolver;

    internal override string XmlLang => string.Empty;

    public virtual XmlResolver XmlResolver
    {
      set => this.resolver = value;
    }

    internal override XmlSpace XmlSpace => XmlSpace.None;

    internal Encoding TextEncoding => !(this.FirstChild is XmlDeclaration firstChild) || firstChild.Encoding == string.Empty ? (Encoding) null : Encoding.GetEncoding(firstChild.Encoding);

    public override XmlNode ParentNode => (XmlNode) null;

    public XmlSchemaSet Schemas
    {
      get
      {
        if (this.schemas == null)
          this.schemas = new XmlSchemaSet();
        return this.schemas;
      }
      set => this.schemas = value;
    }

    public override IXmlSchemaInfo SchemaInfo
    {
      get => this.schemaInfo;
      internal set => this.schemaInfo = value;
    }

    internal void AddIdenticalAttribute(XmlAttribute attr) => this.idTable[(object) attr.Value] = (object) attr;

    public override XmlNode CloneNode(bool deep)
    {
      XmlDocument xmlDocument = this.implementation == null ? new XmlDocument() : this.implementation.CreateDocument();
      xmlDocument.baseURI = this.baseURI;
      if (deep)
      {
        for (XmlNode node = this.FirstChild; node != null; node = node.NextSibling)
          xmlDocument.AppendChild(xmlDocument.ImportNode(node, deep), false);
      }
      return (XmlNode) xmlDocument;
    }

    public XmlAttribute CreateAttribute(string name)
    {
      string namespaceURI = string.Empty;
      string prefix;
      string localName;
      this.ParseName(name, out prefix, out localName);
      if (prefix == "xmlns" || prefix == string.Empty && localName == "xmlns")
        namespaceURI = "http://www.w3.org/2000/xmlns/";
      else if (prefix == "xml")
        namespaceURI = "http://www.w3.org/XML/1998/namespace";
      return this.CreateAttribute(prefix, localName, namespaceURI);
    }

    public XmlAttribute CreateAttribute(string qualifiedName, string namespaceURI)
    {
      string prefix;
      string localName;
      this.ParseName(qualifiedName, out prefix, out localName);
      return this.CreateAttribute(prefix, localName, namespaceURI);
    }

    public virtual XmlAttribute CreateAttribute(
      string prefix,
      string localName,
      string namespaceURI)
    {
      if (localName == null || localName == string.Empty)
        throw new ArgumentException("The attribute local name cannot be empty.");
      return new XmlAttribute(prefix, localName, namespaceURI, this, false, true);
    }

    internal XmlAttribute CreateAttribute(
      string prefix,
      string localName,
      string namespaceURI,
      bool atomizedNames,
      bool checkNamespace)
    {
      return this.optimal_create_attribute ? new XmlAttribute(prefix, localName, namespaceURI, this, atomizedNames, checkNamespace) : this.CreateAttribute(prefix, localName, namespaceURI);
    }

    public virtual XmlCDataSection CreateCDataSection(string data) => new XmlCDataSection(data, this);

    public virtual XmlComment CreateComment(string data) => new XmlComment(data, this);

    protected internal virtual XmlAttribute CreateDefaultAttribute(
      string prefix,
      string localName,
      string namespaceURI)
    {
      XmlAttribute attribute = this.CreateAttribute(prefix, localName, namespaceURI);
      attribute.isDefault = true;
      return attribute;
    }

    public virtual XmlDocumentFragment CreateDocumentFragment() => new XmlDocumentFragment(this);

    public virtual XmlDocumentType CreateDocumentType(
      string name,
      string publicId,
      string systemId,
      string internalSubset)
    {
      return new XmlDocumentType(name, publicId, systemId, internalSubset, this);
    }

    private XmlDocumentType CreateDocumentType(DTDObjectModel dtd) => new XmlDocumentType(dtd, this);

    public XmlElement CreateElement(string name) => this.CreateElement(name, string.Empty);

    public XmlElement CreateElement(string qualifiedName, string namespaceURI)
    {
      string prefix;
      string localName;
      this.ParseName(qualifiedName, out prefix, out localName);
      return this.CreateElement(prefix, localName, namespaceURI);
    }

    public virtual XmlElement CreateElement(string prefix, string localName, string namespaceURI) => new XmlElement(prefix == null ? string.Empty : prefix, localName, namespaceURI == null ? string.Empty : namespaceURI, this, false);

    internal XmlElement CreateElement(
      string prefix,
      string localName,
      string namespaceURI,
      bool nameAtomized)
    {
      if (localName == null || localName == string.Empty)
        throw new ArgumentException("The local name for elements or attributes cannot be null or an empty string.");
      return this.optimal_create_element ? new XmlElement(prefix == null ? string.Empty : prefix, localName, namespaceURI == null ? string.Empty : namespaceURI, this, nameAtomized) : this.CreateElement(prefix, localName, namespaceURI);
    }

    public virtual XmlEntityReference CreateEntityReference(string name) => new XmlEntityReference(name, this);

    public override XPathNavigator CreateNavigator() => this.CreateNavigator((XmlNode) this);

    protected internal virtual XPathNavigator CreateNavigator(XmlNode node) => new XPathEditableDocument(node).CreateNavigator();

    public virtual XmlNode CreateNode(string nodeTypeString, string name, string namespaceURI) => this.CreateNode(this.GetNodeTypeFromString(nodeTypeString), name, namespaceURI);

    public virtual XmlNode CreateNode(XmlNodeType type, string name, string namespaceURI)
    {
      string prefix = (string) null;
      string localName = name;
      if (type == XmlNodeType.Attribute || type == XmlNodeType.Element || type == XmlNodeType.EntityReference)
        this.ParseName(name, out prefix, out localName);
      return this.CreateNode(type, prefix, localName, namespaceURI);
    }

    public virtual XmlNode CreateNode(
      XmlNodeType type,
      string prefix,
      string name,
      string namespaceURI)
    {
      switch (type)
      {
        case XmlNodeType.Element:
          return (XmlNode) this.CreateElement(prefix, name, namespaceURI);
        case XmlNodeType.Attribute:
          return (XmlNode) this.CreateAttribute(prefix, name, namespaceURI);
        case XmlNodeType.Text:
          return (XmlNode) this.CreateTextNode((string) null);
        case XmlNodeType.CDATA:
          return (XmlNode) this.CreateCDataSection((string) null);
        case XmlNodeType.EntityReference:
          return (XmlNode) this.CreateEntityReference((string) null);
        case XmlNodeType.ProcessingInstruction:
          return (XmlNode) this.CreateProcessingInstruction((string) null, (string) null);
        case XmlNodeType.Comment:
          return (XmlNode) this.CreateComment((string) null);
        case XmlNodeType.Document:
          return (XmlNode) new XmlDocument();
        case XmlNodeType.DocumentType:
          return (XmlNode) this.CreateDocumentType((string) null, (string) null, (string) null, (string) null);
        case XmlNodeType.DocumentFragment:
          return (XmlNode) this.CreateDocumentFragment();
        case XmlNodeType.Whitespace:
          return (XmlNode) this.CreateWhitespace(string.Empty);
        case XmlNodeType.SignificantWhitespace:
          return (XmlNode) this.CreateSignificantWhitespace(string.Empty);
        case XmlNodeType.XmlDeclaration:
          return (XmlNode) this.CreateXmlDeclaration("1.0", (string) null, (string) null);
        default:
          throw new ArgumentException(string.Format("{0}\nParameter name: {1}", (object) "Specified argument was out of the range of valid values", (object) type.ToString()));
      }
    }

    public virtual XmlProcessingInstruction CreateProcessingInstruction(string target, string data) => new XmlProcessingInstruction(target, data, this);

    public virtual XmlSignificantWhitespace CreateSignificantWhitespace(string text) => XmlChar.IsWhitespace(text) ? new XmlSignificantWhitespace(text, this) : throw new ArgumentException("Invalid whitespace characters.");

    public virtual XmlText CreateTextNode(string text) => new XmlText(text, this);

    public virtual XmlWhitespace CreateWhitespace(string text) => XmlChar.IsWhitespace(text) ? new XmlWhitespace(text, this) : throw new ArgumentException("Invalid whitespace characters.");

    public virtual XmlDeclaration CreateXmlDeclaration(
      string version,
      string encoding,
      string standalone)
    {
      if (version != "1.0")
        throw new ArgumentException("version string is not correct.");
      if (standalone != null && standalone != string.Empty && !(standalone == "yes") && !(standalone == "no"))
        throw new ArgumentException("standalone string is not correct.");
      return new XmlDeclaration(version, encoding, standalone, this);
    }

    public virtual XmlElement GetElementById(string elementId) => this.GetIdenticalAttribute(elementId)?.OwnerElement;

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

    private XmlNodeType GetNodeTypeFromString(string nodeTypeString)
    {
      string key = nodeTypeString != null ? nodeTypeString : throw new ArgumentNullException(nameof (nodeTypeString));
      if (key != null)
      {
        // ISSUE: reference to a compiler-generated field
        if (XmlDocument.\u003C\u003Ef__switch\u0024map4B == null)
        {
          // ISSUE: reference to a compiler-generated field
          XmlDocument.\u003C\u003Ef__switch\u0024map4B = new Dictionary<string, int>(12)
          {
            {
              "attribute",
              0
            },
            {
              "cdatasection",
              1
            },
            {
              "comment",
              2
            },
            {
              "document",
              3
            },
            {
              "documentfragment",
              4
            },
            {
              "documenttype",
              5
            },
            {
              "element",
              6
            },
            {
              "entityreference",
              7
            },
            {
              "processinginstruction",
              8
            },
            {
              "significantwhitespace",
              9
            },
            {
              "text",
              10
            },
            {
              "whitespace",
              11
            }
          };
        }
        int num;
        // ISSUE: reference to a compiler-generated field
        if (XmlDocument.\u003C\u003Ef__switch\u0024map4B.TryGetValue(key, out num))
        {
          switch (num)
          {
            case 0:
              return XmlNodeType.Attribute;
            case 1:
              return XmlNodeType.CDATA;
            case 2:
              return XmlNodeType.Comment;
            case 3:
              return XmlNodeType.Document;
            case 4:
              return XmlNodeType.DocumentFragment;
            case 5:
              return XmlNodeType.DocumentType;
            case 6:
              return XmlNodeType.Element;
            case 7:
              return XmlNodeType.EntityReference;
            case 8:
              return XmlNodeType.ProcessingInstruction;
            case 9:
              return XmlNodeType.SignificantWhitespace;
            case 10:
              return XmlNodeType.Text;
            case 11:
              return XmlNodeType.Whitespace;
          }
        }
      }
      throw new ArgumentException(string.Format("The string doesn't represent any node type : {0}.", (object) nodeTypeString));
    }

    internal XmlAttribute GetIdenticalAttribute(string id)
    {
      if (!(this.idTable[(object) id] is XmlAttribute xmlAttribute))
        return (XmlAttribute) null;
      return xmlAttribute.OwnerElement == null || !xmlAttribute.OwnerElement.IsRooted ? (XmlAttribute) null : xmlAttribute;
    }

    public virtual XmlNode ImportNode(XmlNode node, bool deep)
    {
      if (node == null)
        throw new NullReferenceException("Null node cannot be imported.");
      switch (node.NodeType)
      {
        case XmlNodeType.None:
          throw new XmlException("Illegal ImportNode call for NodeType.None");
        case XmlNodeType.Element:
          XmlElement xmlElement = (XmlElement) node;
          XmlElement element = this.CreateElement(xmlElement.Prefix, xmlElement.LocalName, xmlElement.NamespaceURI);
          for (int i = 0; i < xmlElement.Attributes.Count; ++i)
          {
            XmlAttribute attribute = xmlElement.Attributes[i];
            if (attribute.Specified)
              element.SetAttributeNode((XmlAttribute) this.ImportNode((XmlNode) attribute, deep));
          }
          if (deep)
          {
            for (XmlNode node1 = xmlElement.FirstChild; node1 != null; node1 = node1.NextSibling)
              element.AppendChild(this.ImportNode(node1, deep));
          }
          return (XmlNode) element;
        case XmlNodeType.Attribute:
          XmlAttribute xmlAttribute = node as XmlAttribute;
          XmlAttribute attribute1 = this.CreateAttribute(xmlAttribute.Prefix, xmlAttribute.LocalName, xmlAttribute.NamespaceURI);
          for (XmlNode node2 = xmlAttribute.FirstChild; node2 != null; node2 = node2.NextSibling)
            attribute1.AppendChild(this.ImportNode(node2, deep));
          return (XmlNode) attribute1;
        case XmlNodeType.Text:
          return (XmlNode) this.CreateTextNode(node.Value);
        case XmlNodeType.CDATA:
          return (XmlNode) this.CreateCDataSection(node.Value);
        case XmlNodeType.EntityReference:
          return (XmlNode) this.CreateEntityReference(node.Name);
        case XmlNodeType.ProcessingInstruction:
          XmlProcessingInstruction processingInstruction = node as XmlProcessingInstruction;
          return (XmlNode) this.CreateProcessingInstruction(processingInstruction.Target, processingInstruction.Data);
        case XmlNodeType.Comment:
          return (XmlNode) this.CreateComment(node.Value);
        case XmlNodeType.Document:
          throw new XmlException("Document cannot be imported.");
        case XmlNodeType.DocumentType:
          throw new XmlException("DocumentType cannot be imported.");
        case XmlNodeType.DocumentFragment:
          XmlDocumentFragment documentFragment = this.CreateDocumentFragment();
          if (deep)
          {
            for (XmlNode node3 = node.FirstChild; node3 != null; node3 = node3.NextSibling)
              documentFragment.AppendChild(this.ImportNode(node3, deep));
          }
          return (XmlNode) documentFragment;
        case XmlNodeType.Whitespace:
          return (XmlNode) this.CreateWhitespace(node.Value);
        case XmlNodeType.SignificantWhitespace:
          return (XmlNode) this.CreateSignificantWhitespace(node.Value);
        case XmlNodeType.EndElement:
          throw new XmlException("Illegal ImportNode call for NodeType.EndElement");
        case XmlNodeType.EndEntity:
          throw new XmlException("Illegal ImportNode call for NodeType.EndEntity");
        case XmlNodeType.XmlDeclaration:
          XmlDeclaration xmlDeclaration = node as XmlDeclaration;
          return (XmlNode) this.CreateXmlDeclaration(xmlDeclaration.Version, xmlDeclaration.Encoding, xmlDeclaration.Standalone);
        default:
          throw new InvalidOperationException("Cannot import specified node type: " + (object) node.NodeType);
      }
    }

    public virtual void Load(Stream inStream) => this.Load((XmlReader) new XmlValidatingReader((XmlReader) new XmlTextReader(inStream, this.NameTable)
    {
      XmlResolver = this.resolver
    })
    {
      EntityHandling = EntityHandling.ExpandCharEntities,
      ValidationType = ValidationType.None
    });

    public virtual void Load(string filename)
    {
      XmlTextReader reader = (XmlTextReader) null;
      try
      {
        reader = new XmlTextReader(filename, this.NameTable);
        reader.XmlResolver = this.resolver;
        this.Load((XmlReader) new XmlValidatingReader((XmlReader) reader)
        {
          EntityHandling = EntityHandling.ExpandCharEntities,
          ValidationType = ValidationType.None
        });
      }
      finally
      {
        reader?.Close();
      }
    }

    public virtual void Load(TextReader txtReader)
    {
      XmlTextReader reader = new XmlTextReader(txtReader, this.NameTable);
      XmlValidatingReader validatingReader = new XmlValidatingReader((XmlReader) reader);
      validatingReader.EntityHandling = EntityHandling.ExpandCharEntities;
      validatingReader.ValidationType = ValidationType.None;
      reader.XmlResolver = this.resolver;
      this.Load((XmlReader) validatingReader);
    }

    public virtual void Load(XmlReader xmlReader)
    {
      this.RemoveAll();
      this.baseURI = xmlReader.BaseURI;
      try
      {
        this.loadMode = true;
        while (true)
        {
          XmlNode newChild;
          do
          {
            newChild = this.ReadNode(xmlReader);
            if (newChild == null)
              goto label_5;
          }
          while (!this.preserveWhitespace && newChild.NodeType == XmlNodeType.Whitespace);
          this.AppendChild(newChild, false);
        }
label_5:
        if (xmlReader.Settings == null)
          return;
        this.schemas = xmlReader.Settings.Schemas;
      }
      finally
      {
        this.loadMode = false;
      }
    }

    public virtual void LoadXml(string xml)
    {
      XmlTextReader xmlTextReader = new XmlTextReader(xml, XmlNodeType.Document, new XmlParserContext(this.NameTable, new XmlNamespaceManager(this.NameTable), (string) null, XmlSpace.None));
      try
      {
        xmlTextReader.XmlResolver = this.resolver;
        this.Load((XmlReader) xmlTextReader);
      }
      finally
      {
        xmlTextReader.Close();
      }
    }

    internal void onNodeChanged(XmlNode node, XmlNode parent, string oldValue, string newValue)
    {
      if (this.NodeChanged == null)
        return;
      this.NodeChanged((object) node, new XmlNodeChangedEventArgs(node, parent, parent, oldValue, newValue, XmlNodeChangedAction.Change));
    }

    internal void onNodeChanging(XmlNode node, XmlNode parent, string oldValue, string newValue)
    {
      if (node.IsReadOnly)
        throw new ArgumentException("Node is read-only.");
      if (this.NodeChanging == null)
        return;
      this.NodeChanging((object) node, new XmlNodeChangedEventArgs(node, parent, parent, oldValue, newValue, XmlNodeChangedAction.Change));
    }

    internal void onNodeInserted(XmlNode node, XmlNode newParent)
    {
      if (this.NodeInserted == null)
        return;
      this.NodeInserted((object) node, new XmlNodeChangedEventArgs(node, (XmlNode) null, newParent, (string) null, (string) null, XmlNodeChangedAction.Insert));
    }

    internal void onNodeInserting(XmlNode node, XmlNode newParent)
    {
      if (this.NodeInserting == null)
        return;
      this.NodeInserting((object) node, new XmlNodeChangedEventArgs(node, (XmlNode) null, newParent, (string) null, (string) null, XmlNodeChangedAction.Insert));
    }

    internal void onNodeRemoved(XmlNode node, XmlNode oldParent)
    {
      if (this.NodeRemoved == null)
        return;
      this.NodeRemoved((object) node, new XmlNodeChangedEventArgs(node, oldParent, (XmlNode) null, (string) null, (string) null, XmlNodeChangedAction.Remove));
    }

    internal void onNodeRemoving(XmlNode node, XmlNode oldParent)
    {
      if (this.NodeRemoving == null)
        return;
      this.NodeRemoving((object) node, new XmlNodeChangedEventArgs(node, oldParent, (XmlNode) null, (string) null, (string) null, XmlNodeChangedAction.Remove));
    }

    private void ParseName(string name, out string prefix, out string localName)
    {
      int length = name.IndexOf(':');
      if (length != -1)
      {
        prefix = name.Substring(0, length);
        localName = name.Substring(length + 1);
      }
      else
      {
        prefix = string.Empty;
        localName = name;
      }
    }

    private XmlAttribute ReadAttributeNode(XmlReader reader)
    {
      if (reader.NodeType == XmlNodeType.Element)
        reader.MoveToFirstAttribute();
      else if (reader.NodeType != XmlNodeType.Attribute)
        throw new InvalidOperationException(this.MakeReaderErrorMessage("bad position to read attribute.", reader));
      XmlAttribute attribute = this.CreateAttribute(reader.Prefix, reader.LocalName, reader.NamespaceURI);
      if (reader.SchemaInfo != null)
        this.SchemaInfo = (IXmlSchemaInfo) new XmlSchemaInfo(reader.SchemaInfo);
      bool isDefault = reader.IsDefault;
      this.ReadAttributeNodeValue(reader, attribute);
      if (isDefault)
        attribute.SetDefault();
      return attribute;
    }

    internal void ReadAttributeNodeValue(XmlReader reader, XmlAttribute attribute)
    {
      while (reader.ReadAttributeValue())
      {
        if (reader.NodeType == XmlNodeType.EntityReference)
          attribute.AppendChild((XmlNode) this.CreateEntityReference(reader.Name), false);
        else
          attribute.AppendChild((XmlNode) this.CreateTextNode(reader.Value), false);
      }
    }

    public virtual XmlNode ReadNode(XmlReader reader)
    {
      if (this.PreserveWhitespace || !(reader is XmlTextReader xmlTextReader))
        return this.ReadNodeCore(reader);
      if (xmlTextReader.WhitespaceHandling != WhitespaceHandling.All)
        return this.ReadNodeCore(reader);
      try
      {
        xmlTextReader.WhitespaceHandling = WhitespaceHandling.Significant;
        return this.ReadNodeCore(reader);
      }
      finally
      {
        xmlTextReader.WhitespaceHandling = WhitespaceHandling.All;
      }
    }

    private XmlNode ReadNodeCore(XmlReader reader)
    {
      switch (reader.ReadState)
      {
        case ReadState.Initial:
          if (reader.SchemaInfo != null)
            this.SchemaInfo = (IXmlSchemaInfo) new XmlSchemaInfo(reader.SchemaInfo);
          reader.Read();
          goto case ReadState.Interactive;
        case ReadState.Interactive:
          XmlNode xmlNode1;
          switch (reader.NodeType)
          {
            case XmlNodeType.None:
              return (XmlNode) null;
            case XmlNodeType.Element:
              XmlElement element = this.CreateElement(reader.Prefix, reader.LocalName, reader.NamespaceURI, reader.NameTable == this.NameTable);
              if (reader.SchemaInfo != null)
                this.SchemaInfo = (IXmlSchemaInfo) new XmlSchemaInfo(reader.SchemaInfo);
              element.IsEmpty = reader.IsEmptyElement;
              for (int i = 0; i < reader.AttributeCount; ++i)
              {
                reader.MoveToAttribute(i);
                element.SetAttributeNode(this.ReadAttributeNode(reader));
                reader.MoveToElement();
              }
              reader.MoveToElement();
              int depth = reader.Depth;
              if (reader.IsEmptyElement)
              {
                xmlNode1 = (XmlNode) element;
                break;
              }
              reader.Read();
              while (reader.Depth > depth)
              {
                XmlNode newChild = this.ReadNodeCore(reader);
                if (this.preserveWhitespace || newChild.NodeType != XmlNodeType.Whitespace)
                  element.AppendChild(newChild, false);
              }
              xmlNode1 = (XmlNode) element;
              break;
            case XmlNodeType.Attribute:
              string localName = reader.LocalName;
              string namespaceUri = reader.NamespaceURI;
              XmlNode xmlNode2 = (XmlNode) this.ReadAttributeNode(reader);
              reader.MoveToAttribute(localName, namespaceUri);
              return xmlNode2;
            case XmlNodeType.Text:
              xmlNode1 = (XmlNode) this.CreateTextNode(reader.Value);
              break;
            case XmlNodeType.CDATA:
              xmlNode1 = (XmlNode) this.CreateCDataSection(reader.Value);
              break;
            case XmlNodeType.EntityReference:
              if (this.loadMode && this.DocumentType != null && this.DocumentType.Entities.GetNamedItem(reader.Name) == null)
                throw new XmlException("Reference to undeclared entity was found.");
              xmlNode1 = (XmlNode) this.CreateEntityReference(reader.Name);
              if (reader.CanResolveEntity)
              {
                reader.ResolveEntity();
                reader.Read();
                XmlNode newChild;
                while (reader.NodeType != XmlNodeType.EndEntity && (newChild = this.ReadNode(reader)) != null)
                  xmlNode1.InsertBefore(newChild, (XmlNode) null, false, false);
                break;
              }
              break;
            case XmlNodeType.ProcessingInstruction:
              xmlNode1 = (XmlNode) this.CreateProcessingInstruction(reader.Name, reader.Value);
              break;
            case XmlNodeType.Comment:
              xmlNode1 = (XmlNode) this.CreateComment(reader.Value);
              break;
            case XmlNodeType.DocumentType:
              DTDObjectModel dtd = (DTDObjectModel) null;
              if (reader is IHasXmlParserContext xmlParserContext)
                dtd = xmlParserContext.ParserContext.Dtd;
              xmlNode1 = dtd == null ? (XmlNode) this.CreateDocumentType(reader.Name, reader["PUBLIC"], reader["SYSTEM"], reader.Value) : (XmlNode) this.CreateDocumentType(dtd);
              break;
            case XmlNodeType.Whitespace:
              xmlNode1 = (XmlNode) this.CreateWhitespace(reader.Value);
              break;
            case XmlNodeType.SignificantWhitespace:
              xmlNode1 = (XmlNode) this.CreateSignificantWhitespace(reader.Value);
              break;
            case XmlNodeType.XmlDeclaration:
              xmlNode1 = (XmlNode) this.CreateXmlDeclaration("1.0", string.Empty, string.Empty);
              xmlNode1.Value = reader.Value;
              break;
            default:
              throw new NullReferenceException("Unexpected node type " + (object) reader.NodeType + ".");
          }
          reader.Read();
          return xmlNode1;
        default:
          return (XmlNode) null;
      }
    }

    private string MakeReaderErrorMessage(string message, XmlReader reader)
    {
      if (!(reader is IXmlLineInfo xmlLineInfo))
        return message;
      return string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0} Line number = {1}, Inline position = {2}.", (object) message, (object) xmlLineInfo.LineNumber, (object) xmlLineInfo.LinePosition);
    }

    internal void RemoveIdenticalAttribute(string id) => this.idTable.Remove((object) id);

    public virtual void Save(Stream outStream)
    {
      XmlTextWriter w = new XmlTextWriter(outStream, this.TextEncoding);
      if (!this.PreserveWhitespace)
        w.Formatting = Formatting.Indented;
      this.WriteContentTo((XmlWriter) w);
      w.Flush();
    }

    public virtual void Save(string filename)
    {
      XmlTextWriter w = new XmlTextWriter(filename, this.TextEncoding);
      try
      {
        if (!this.PreserveWhitespace)
          w.Formatting = Formatting.Indented;
        this.WriteContentTo((XmlWriter) w);
      }
      finally
      {
        w.Close();
      }
    }

    public virtual void Save(TextWriter writer)
    {
      XmlTextWriter w = new XmlTextWriter(writer);
      if (!this.PreserveWhitespace)
        w.Formatting = Formatting.Indented;
      if (this.FirstChild != null && this.FirstChild.NodeType != XmlNodeType.XmlDeclaration)
        w.WriteStartDocument();
      this.WriteContentTo((XmlWriter) w);
      w.WriteEndDocument();
      w.Flush();
    }

    public virtual void Save(XmlWriter xmlWriter)
    {
      bool flag = this.FirstChild != null && this.FirstChild.NodeType != XmlNodeType.XmlDeclaration;
      if (flag)
        xmlWriter.WriteStartDocument();
      this.WriteContentTo(xmlWriter);
      if (flag)
        xmlWriter.WriteEndDocument();
      xmlWriter.Flush();
    }

    public override void WriteContentTo(XmlWriter w)
    {
      for (XmlNode xmlNode = this.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
        xmlNode.WriteTo(w);
    }

    public override void WriteTo(XmlWriter w) => this.WriteContentTo(w);

    private void AddDefaultNameTableKeys()
    {
      this.nameTable.Add("#text");
      this.nameTable.Add("xml");
      this.nameTable.Add("xmlns");
      this.nameTable.Add("#entity");
      this.nameTable.Add("#document-fragment");
      this.nameTable.Add("#comment");
      this.nameTable.Add("space");
      this.nameTable.Add("id");
      this.nameTable.Add("#whitespace");
      this.nameTable.Add("http://www.w3.org/2000/xmlns/");
      this.nameTable.Add("#cdata-section");
      this.nameTable.Add("lang");
      this.nameTable.Add("#document");
      this.nameTable.Add("#significant-whitespace");
    }

    internal void CheckIdTableUpdate(XmlAttribute attr, string oldValue, string newValue)
    {
      if (this.idTable[(object) oldValue] != attr)
        return;
      this.idTable.Remove((object) oldValue);
      this.idTable[(object) newValue] = (object) attr;
    }

    public void Validate(ValidationEventHandler handler) => this.Validate(handler, (XmlNode) this, XmlSchemaValidationFlags.ProcessIdentityConstraints);

    public void Validate(ValidationEventHandler handler, XmlNode node) => this.Validate(handler, node, XmlSchemaValidationFlags.ProcessIdentityConstraints);

    private void Validate(
      ValidationEventHandler handler,
      XmlNode node,
      XmlSchemaValidationFlags flags)
    {
      XmlReaderSettings settings = new XmlReaderSettings()
      {
        NameTable = this.NameTable,
        Schemas = this.schemas
      };
      settings.Schemas.XmlResolver = this.resolver;
      settings.XmlResolver = this.resolver;
      settings.ValidationFlags = flags;
      settings.ValidationType = ValidationType.Schema;
      XmlReader xmlReader = XmlReader.Create((XmlReader) new XmlNodeReader(node), settings);
      while (!xmlReader.EOF)
        xmlReader.Read();
    }
  }
}
