// Decompiled with JetBrains decompiler
// Type: Mono.Xml.XPath.DTMXPathDocumentBuilder
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System;
using System.Collections;
using System.Xml;
using System.Xml.Schema;
using System.Xml.XPath;

namespace Mono.Xml.XPath
{
  internal class DTMXPathDocumentBuilder
  {
    private XmlReader xmlReader;
    private XmlValidatingReader validatingReader;
    private XmlSpace xmlSpace;
    private XmlNameTable nameTable;
    private IXmlLineInfo lineInfo;
    private int nodeCapacity;
    private int attributeCapacity;
    private int nsCapacity;
    private DTMXPathLinkedNode[] nodes;
    private DTMXPathAttributeNode[] attributes;
    private DTMXPathNamespaceNode[] namespaces;
    private Hashtable idTable;
    private int nodeIndex;
    private int attributeIndex;
    private int nsIndex;
    private bool hasAttributes;
    private bool hasLocalNs;
    private int attrIndexAtStart;
    private int nsIndexAtStart;
    private int lastNsInScope;
    private bool skipRead;
    private int[] parentStack = new int[10];
    private int parentStackIndex;

    public DTMXPathDocumentBuilder(string url)
      : this(url, XmlSpace.None, 200)
    {
    }

    public DTMXPathDocumentBuilder(string url, XmlSpace space)
      : this(url, space, 200)
    {
    }

    public DTMXPathDocumentBuilder(string url, XmlSpace space, int defaultCapacity)
    {
      XmlReader reader = (XmlReader) null;
      try
      {
        reader = (XmlReader) new XmlTextReader(url);
        this.Init(reader, space, defaultCapacity);
      }
      finally
      {
        reader?.Close();
      }
    }

    public DTMXPathDocumentBuilder(XmlReader reader)
      : this(reader, XmlSpace.None, 200)
    {
    }

    public DTMXPathDocumentBuilder(XmlReader reader, XmlSpace space)
      : this(reader, space, 200)
    {
    }

    public DTMXPathDocumentBuilder(XmlReader reader, XmlSpace space, int defaultCapacity) => this.Init(reader, space, defaultCapacity);

    private void Init(XmlReader reader, XmlSpace space, int defaultCapacity)
    {
      this.xmlReader = reader;
      this.validatingReader = reader as XmlValidatingReader;
      this.lineInfo = reader as IXmlLineInfo;
      this.xmlSpace = space;
      this.nameTable = reader.NameTable;
      this.nodeCapacity = defaultCapacity;
      this.attributeCapacity = this.nodeCapacity;
      this.nsCapacity = 10;
      this.idTable = new Hashtable();
      this.nodes = new DTMXPathLinkedNode[this.nodeCapacity];
      this.attributes = new DTMXPathAttributeNode[this.attributeCapacity];
      this.namespaces = new DTMXPathNamespaceNode[this.nsCapacity];
      this.Compile();
    }

    public DTMXPathDocument CreateDocument() => new DTMXPathDocument(this.nameTable, this.nodes, this.attributes, this.namespaces, this.idTable);

    public void Compile()
    {
      this.AddNode(0, 0, 0, XPathNodeType.All, string.Empty, false, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 0, 0, 0);
      ++this.nodeIndex;
      this.AddAttribute(0, (string) null, (string) null, (string) null, (string) null, 0, 0);
      this.AddNsNode(0, (string) null, (string) null, 0);
      ++this.nsIndex;
      this.AddNsNode(1, "xml", "http://www.w3.org/XML/1998/namespace", 0);
      this.AddNode(0, 0, 0, XPathNodeType.Root, this.xmlReader.BaseURI, false, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 1, 0, 0);
      this.nodeIndex = 1;
      this.lastNsInScope = 1;
      this.parentStack[0] = this.nodeIndex;
      while (!this.xmlReader.EOF)
        this.Read();
      this.SetNodeArrayLength(this.nodeIndex + 1);
      this.SetAttributeArrayLength(this.attributeIndex + 1);
      this.SetNsArrayLength(this.nsIndex + 1);
      this.xmlReader = (XmlReader) null;
    }

    public void Read()
    {
      if (!this.skipRead && !this.xmlReader.Read())
        return;
      this.skipRead = false;
      int parent = this.parentStack[this.parentStackIndex];
      int previousSibling = this.nodeIndex;
      switch (this.xmlReader.NodeType)
      {
        case XmlNodeType.Element:
        case XmlNodeType.Text:
        case XmlNodeType.CDATA:
        case XmlNodeType.ProcessingInstruction:
        case XmlNodeType.Comment:
        case XmlNodeType.SignificantWhitespace:
          if (parent == this.nodeIndex)
          {
            previousSibling = 0;
          }
          else
          {
            while (this.nodes[previousSibling].Parent != parent)
              previousSibling = this.nodes[previousSibling].Parent;
          }
          ++this.nodeIndex;
          if (previousSibling != 0)
            this.nodes[previousSibling].NextSibling = this.nodeIndex;
          if (this.parentStack[this.parentStackIndex] == this.nodeIndex - 1)
            this.nodes[parent].FirstChild = this.nodeIndex;
          string str = (string) null;
          XPathNodeType nodeType = XPathNodeType.Text;
          switch (this.xmlReader.NodeType)
          {
            case XmlNodeType.Element:
              this.ProcessElement(parent, previousSibling);
              return;
            case XmlNodeType.Attribute:
              return;
            case XmlNodeType.Text:
            case XmlNodeType.CDATA:
              this.AddNode(parent, 0, previousSibling, nodeType, this.xmlReader.BaseURI, this.xmlReader.IsEmptyElement, this.xmlReader.LocalName, this.xmlReader.NamespaceURI, this.xmlReader.Prefix, str, this.xmlReader.XmlLang, this.nsIndex, this.lineInfo == null ? 0 : this.lineInfo.LineNumber, this.lineInfo == null ? 0 : this.lineInfo.LinePosition);
              if (str != null)
                return;
              string empty = string.Empty;
              XPathNodeType xpathNodeType = XPathNodeType.Whitespace;
              bool flag;
              do
              {
                switch (this.xmlReader.NodeType)
                {
                  case XmlNodeType.Text:
                  case XmlNodeType.CDATA:
                    xpathNodeType = XPathNodeType.Text;
                    goto case XmlNodeType.Whitespace;
                  case XmlNodeType.Whitespace:
                    if (this.xmlReader.NodeType != XmlNodeType.Whitespace || this.xmlSpace == XmlSpace.Preserve)
                      empty += this.xmlReader.Value;
                    flag = this.xmlReader.Read();
                    this.skipRead = true;
                    break;
                  case XmlNodeType.SignificantWhitespace:
                    if (xpathNodeType == XPathNodeType.Whitespace)
                    {
                      xpathNodeType = XPathNodeType.SignificantWhitespace;
                      goto case XmlNodeType.Whitespace;
                    }
                    else
                      goto case XmlNodeType.Whitespace;
                  default:
                    flag = false;
                    break;
                }
              }
              while (flag);
              this.nodes[this.nodeIndex].Value = empty;
              this.nodes[this.nodeIndex].NodeType = xpathNodeType;
              return;
            case XmlNodeType.EntityReference:
              return;
            case XmlNodeType.Entity:
              return;
            case XmlNodeType.ProcessingInstruction:
              str = this.xmlReader.Value;
              nodeType = XPathNodeType.ProcessingInstruction;
              goto case XmlNodeType.Text;
            case XmlNodeType.Comment:
              str = this.xmlReader.Value;
              nodeType = XPathNodeType.Comment;
              goto case XmlNodeType.Text;
            case XmlNodeType.Document:
              return;
            case XmlNodeType.DocumentType:
              return;
            case XmlNodeType.DocumentFragment:
              return;
            case XmlNodeType.Notation:
              return;
            case XmlNodeType.Whitespace:
              nodeType = XPathNodeType.Whitespace;
              goto case XmlNodeType.Text;
            case XmlNodeType.SignificantWhitespace:
              nodeType = XPathNodeType.SignificantWhitespace;
              goto case XmlNodeType.Text;
            default:
              return;
          }
        case XmlNodeType.Whitespace:
          if (this.xmlSpace != XmlSpace.Preserve)
            break;
          goto case XmlNodeType.Element;
        case XmlNodeType.EndElement:
          --this.parentStackIndex;
          break;
      }
    }

    private void ProcessElement(int parent, int previousSibling)
    {
      this.WriteStartElement(parent, previousSibling);
      if (this.xmlReader.MoveToFirstAttribute())
      {
        do
        {
          string prefix = this.xmlReader.Prefix;
          string namespaceUri = this.xmlReader.NamespaceURI;
          if (namespaceUri == "http://www.w3.org/2000/xmlns/")
            this.ProcessNamespace(prefix == null || prefix == string.Empty ? string.Empty : this.xmlReader.LocalName, this.xmlReader.Value);
          else
            this.ProcessAttribute(prefix, this.xmlReader.LocalName, namespaceUri, this.xmlReader.Value);
        }
        while (this.xmlReader.MoveToNextAttribute());
        this.xmlReader.MoveToElement();
      }
      this.CloseStartElement();
    }

    private void PrepareStartElement(int previousSibling)
    {
      this.hasAttributes = false;
      this.hasLocalNs = false;
      this.attrIndexAtStart = this.attributeIndex;
      this.nsIndexAtStart = this.nsIndex;
      while (this.namespaces[this.lastNsInScope].DeclaredElement == previousSibling)
        this.lastNsInScope = this.namespaces[this.lastNsInScope].NextNamespace;
    }

    private void WriteStartElement(int parent, int previousSibling)
    {
      this.PrepareStartElement(previousSibling);
      this.AddNode(parent, 0, previousSibling, XPathNodeType.Element, this.xmlReader.BaseURI, this.xmlReader.IsEmptyElement, this.xmlReader.LocalName, this.xmlReader.NamespaceURI, this.xmlReader.Prefix, string.Empty, this.xmlReader.XmlLang, this.lastNsInScope, this.lineInfo == null ? 0 : this.lineInfo.LineNumber, this.lineInfo == null ? 0 : this.lineInfo.LinePosition);
    }

    private void CloseStartElement()
    {
      if (this.attrIndexAtStart != this.attributeIndex)
        this.nodes[this.nodeIndex].FirstAttribute = this.attrIndexAtStart + 1;
      if (this.nsIndexAtStart != this.nsIndex)
      {
        this.nodes[this.nodeIndex].FirstNamespace = this.nsIndex;
        if (!this.xmlReader.IsEmptyElement)
          this.lastNsInScope = this.nsIndex;
      }
      if (this.nodes[this.nodeIndex].IsEmptyElement)
        return;
      ++this.parentStackIndex;
      if (this.parentStack.Length == this.parentStackIndex)
      {
        int[] destinationArray = new int[this.parentStackIndex * 2];
        Array.Copy((Array) this.parentStack, (Array) destinationArray, this.parentStackIndex);
        this.parentStack = destinationArray;
      }
      this.parentStack[this.parentStackIndex] = this.nodeIndex;
    }

    private void ProcessNamespace(string prefix, string ns)
    {
      int nextNs = !this.hasLocalNs ? this.nodes[this.nodeIndex].FirstNamespace : this.nsIndex;
      ++this.nsIndex;
      this.AddNsNode(this.nodeIndex, prefix, ns, nextNs);
      this.hasLocalNs = true;
    }

    private void ProcessAttribute(string prefix, string localName, string ns, string value)
    {
      ++this.attributeIndex;
      this.AddAttribute(this.nodeIndex, localName, ns, prefix == null ? string.Empty : prefix, value, this.lineInfo == null ? 0 : this.lineInfo.LineNumber, this.lineInfo == null ? 0 : this.lineInfo.LinePosition);
      if (this.hasAttributes)
        this.attributes[this.attributeIndex - 1].NextAttribute = this.attributeIndex;
      else
        this.hasAttributes = true;
      if (this.validatingReader == null)
        return;
      if (!(this.validatingReader.SchemaType is XmlSchemaDatatype xmlSchemaDatatype) && this.validatingReader.SchemaType is XmlSchemaType schemaType)
        xmlSchemaDatatype = schemaType.Datatype;
      if (xmlSchemaDatatype == null || xmlSchemaDatatype.TokenizedType != XmlTokenizedType.ID)
        return;
      this.idTable.Add((object) value, (object) this.nodeIndex);
    }

    private void SetNodeArrayLength(int size)
    {
      DTMXPathLinkedNode[] destinationArray = new DTMXPathLinkedNode[size];
      Array.Copy((Array) this.nodes, (Array) destinationArray, Math.Min(size, this.nodes.Length));
      this.nodes = destinationArray;
    }

    private void SetAttributeArrayLength(int size)
    {
      DTMXPathAttributeNode[] destinationArray = new DTMXPathAttributeNode[size];
      Array.Copy((Array) this.attributes, (Array) destinationArray, Math.Min(size, this.attributes.Length));
      this.attributes = destinationArray;
    }

    private void SetNsArrayLength(int size)
    {
      DTMXPathNamespaceNode[] destinationArray = new DTMXPathNamespaceNode[size];
      Array.Copy((Array) this.namespaces, (Array) destinationArray, Math.Min(size, this.namespaces.Length));
      this.namespaces = destinationArray;
    }

    public void AddNode(
      int parent,
      int firstAttribute,
      int previousSibling,
      XPathNodeType nodeType,
      string baseUri,
      bool isEmptyElement,
      string localName,
      string ns,
      string prefix,
      string value,
      string xmlLang,
      int namespaceNode,
      int lineNumber,
      int linePosition)
    {
      if (this.nodes.Length < this.nodeIndex + 1)
      {
        this.nodeCapacity *= 4;
        this.SetNodeArrayLength(this.nodeCapacity);
      }
      this.nodes[this.nodeIndex].FirstChild = 0;
      this.nodes[this.nodeIndex].Parent = parent;
      this.nodes[this.nodeIndex].FirstAttribute = firstAttribute;
      this.nodes[this.nodeIndex].PreviousSibling = previousSibling;
      this.nodes[this.nodeIndex].NextSibling = 0;
      this.nodes[this.nodeIndex].NodeType = nodeType;
      this.nodes[this.nodeIndex].BaseURI = baseUri;
      this.nodes[this.nodeIndex].IsEmptyElement = isEmptyElement;
      this.nodes[this.nodeIndex].LocalName = localName;
      this.nodes[this.nodeIndex].NamespaceURI = ns;
      this.nodes[this.nodeIndex].Prefix = prefix;
      this.nodes[this.nodeIndex].Value = value;
      this.nodes[this.nodeIndex].XmlLang = xmlLang;
      this.nodes[this.nodeIndex].FirstNamespace = namespaceNode;
      this.nodes[this.nodeIndex].LineNumber = lineNumber;
      this.nodes[this.nodeIndex].LinePosition = linePosition;
    }

    public void AddAttribute(
      int ownerElement,
      string localName,
      string ns,
      string prefix,
      string value,
      int lineNumber,
      int linePosition)
    {
      if (this.attributes.Length < this.attributeIndex + 1)
      {
        this.attributeCapacity *= 4;
        this.SetAttributeArrayLength(this.attributeCapacity);
      }
      this.attributes[this.attributeIndex].OwnerElement = ownerElement;
      this.attributes[this.attributeIndex].LocalName = localName;
      this.attributes[this.attributeIndex].NamespaceURI = ns;
      this.attributes[this.attributeIndex].Prefix = prefix;
      this.attributes[this.attributeIndex].Value = value;
      this.attributes[this.attributeIndex].LineNumber = lineNumber;
      this.attributes[this.attributeIndex].LinePosition = linePosition;
    }

    public void AddNsNode(int declaredElement, string name, string ns, int nextNs)
    {
      if (this.namespaces.Length < this.nsIndex + 1)
      {
        this.nsCapacity *= 4;
        this.SetNsArrayLength(this.nsCapacity);
      }
      this.namespaces[this.nsIndex].DeclaredElement = declaredElement;
      this.namespaces[this.nsIndex].Name = name;
      this.namespaces[this.nsIndex].Namespace = ns;
      this.namespaces[this.nsIndex].NextNamespace = nextNs;
    }
  }
}
