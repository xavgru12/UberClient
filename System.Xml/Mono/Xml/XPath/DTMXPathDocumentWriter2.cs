﻿// Decompiled with JetBrains decompiler
// Type: Mono.Xml.XPath.DTMXPathDocumentWriter2
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System;
using System.Collections;
using System.Xml;
using System.Xml.XPath;

namespace Mono.Xml.XPath
{
  internal class DTMXPathDocumentWriter2 : XmlWriter
  {
    private XmlNameTable nameTable;
    private int nodeCapacity;
    private int attributeCapacity;
    private int nsCapacity;
    private DTMXPathLinkedNode2[] nodes;
    private DTMXPathAttributeNode2[] attributes;
    private DTMXPathNamespaceNode2[] namespaces;
    private string[] atomicStringPool;
    private int atomicIndex;
    private string[] nonAtomicStringPool;
    private int nonAtomicIndex;
    private Hashtable idTable;
    private int nodeIndex;
    private int attributeIndex;
    private int nsIndex;
    private int[] parentStack = new int[10];
    private int parentStackIndex;
    private bool hasAttributes;
    private bool hasLocalNs;
    private int attrIndexAtStart;
    private int nsIndexAtStart;
    private int lastNsInScope;
    private int prevSibling;
    private WriteState state;
    private bool openNamespace;
    private bool isClosed;

    public DTMXPathDocumentWriter2(XmlNameTable nt, int defaultCapacity)
    {
      this.nameTable = nt != null ? nt : (XmlNameTable) new NameTable();
      this.nodeCapacity = defaultCapacity;
      this.attributeCapacity = this.nodeCapacity;
      this.nsCapacity = 10;
      this.idTable = new Hashtable();
      this.nodes = new DTMXPathLinkedNode2[this.nodeCapacity];
      this.attributes = new DTMXPathAttributeNode2[this.attributeCapacity];
      this.namespaces = new DTMXPathNamespaceNode2[this.nsCapacity];
      this.atomicStringPool = new string[20];
      this.nonAtomicStringPool = new string[20];
      this.Init();
    }

    public DTMXPathDocument2 CreateDocument()
    {
      if (!this.isClosed)
        this.Close();
      return new DTMXPathDocument2(this.nameTable, this.nodes, this.attributes, this.namespaces, this.atomicStringPool, this.nonAtomicStringPool, this.idTable);
    }

    public void Init()
    {
      this.atomicStringPool[0] = this.nonAtomicStringPool[0] = string.Empty;
      this.atomicStringPool[1] = this.nonAtomicStringPool[1] = (string) null;
      this.atomicStringPool[2] = this.nonAtomicStringPool[2] = "http://www.w3.org/XML/1998/namespace";
      this.atomicStringPool[3] = this.nonAtomicStringPool[3] = "http://www.w3.org/2000/xmlns/";
      this.atomicIndex = this.nonAtomicIndex = 4;
      this.AddNode(0, 0, 0, XPathNodeType.All, string.Empty, false, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 0, 0, 0);
      ++this.nodeIndex;
      this.AddAttribute(0, string.Empty, string.Empty, string.Empty, string.Empty, 0, 0);
      this.AddNsNode(0, string.Empty, string.Empty, 0);
      ++this.nsIndex;
      this.AddNsNode(1, "xml", "http://www.w3.org/XML/1998/namespace", 0);
      this.AddNode(0, 0, 0, XPathNodeType.Root, string.Empty, false, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 1, 0, 0);
      this.nodeIndex = 1;
      this.lastNsInScope = 1;
      this.parentStack[0] = this.nodeIndex;
      this.state = WriteState.Content;
    }

    private int GetParentIndex() => this.parentStack[this.parentStackIndex];

    private int GetPreviousSiblingIndex()
    {
      int parent = this.parentStack[this.parentStackIndex];
      if (parent == this.nodeIndex)
        return 0;
      int previousSiblingIndex = this.nodeIndex;
      while (this.nodes[previousSiblingIndex].Parent != parent)
        previousSiblingIndex = this.nodes[previousSiblingIndex].Parent;
      return previousSiblingIndex;
    }

    private void UpdateTreeForAddition()
    {
      int parentIndex = this.GetParentIndex();
      this.prevSibling = this.GetPreviousSiblingIndex();
      ++this.nodeIndex;
      if (this.prevSibling != 0)
        this.nodes[this.prevSibling].NextSibling = this.nodeIndex;
      if (parentIndex != this.nodeIndex - 1)
        return;
      this.nodes[parentIndex].FirstChild = this.nodeIndex;
    }

    private void CloseStartElement()
    {
      if (this.attrIndexAtStart != this.attributeIndex)
        this.nodes[this.nodeIndex].FirstAttribute = this.attrIndexAtStart + 1;
      if (this.nsIndexAtStart != this.nsIndex)
      {
        this.nodes[this.nodeIndex].FirstNamespace = this.nsIndex;
        this.lastNsInScope = this.nsIndex;
      }
      ++this.parentStackIndex;
      if (this.parentStack.Length == this.parentStackIndex)
      {
        int[] destinationArray = new int[this.parentStackIndex * 2];
        Array.Copy((Array) this.parentStack, (Array) destinationArray, this.parentStackIndex);
        this.parentStack = destinationArray;
      }
      this.parentStack[this.parentStackIndex] = this.nodeIndex;
      this.state = WriteState.Content;
    }

    private int AtomicIndex(string s)
    {
      if (s == string.Empty)
        return 0;
      if (s == null)
        return 1;
      for (int index = 2; index < this.atomicIndex; ++index)
      {
        if (object.ReferenceEquals((object) s, (object) this.atomicStringPool[index]))
          return index;
      }
      if (this.atomicIndex == this.atomicStringPool.Length)
      {
        string[] destinationArray = new string[this.atomicIndex * 2];
        Array.Copy((Array) this.atomicStringPool, (Array) destinationArray, this.atomicIndex);
        this.atomicStringPool = destinationArray;
      }
      this.atomicStringPool[this.atomicIndex] = s;
      return this.atomicIndex++;
    }

    private int NonAtomicIndex(string s)
    {
      if (s == string.Empty)
        return 0;
      if (s == null)
        return 1;
      int index1 = 2;
      for (int index2 = this.nonAtomicIndex >= 100 ? 100 : this.nonAtomicIndex; index1 < index2; ++index1)
      {
        if (s == this.nonAtomicStringPool[index1])
          return index1;
      }
      if (this.nonAtomicIndex == this.nonAtomicStringPool.Length)
      {
        string[] destinationArray = new string[this.nonAtomicIndex * 2];
        Array.Copy((Array) this.nonAtomicStringPool, (Array) destinationArray, this.nonAtomicIndex);
        this.nonAtomicStringPool = destinationArray;
      }
      this.nonAtomicStringPool[this.nonAtomicIndex] = s;
      return this.nonAtomicIndex++;
    }

    private void SetNodeArrayLength(int size)
    {
      DTMXPathLinkedNode2[] destinationArray = new DTMXPathLinkedNode2[size];
      Array.Copy((Array) this.nodes, (Array) destinationArray, Math.Min(size, this.nodes.Length));
      this.nodes = destinationArray;
    }

    private void SetAttributeArrayLength(int size)
    {
      DTMXPathAttributeNode2[] destinationArray = new DTMXPathAttributeNode2[size];
      Array.Copy((Array) this.attributes, (Array) destinationArray, Math.Min(size, this.attributes.Length));
      this.attributes = destinationArray;
    }

    private void SetNsArrayLength(int size)
    {
      DTMXPathNamespaceNode2[] destinationArray = new DTMXPathNamespaceNode2[size];
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
      this.nodes[this.nodeIndex].BaseURI = this.AtomicIndex(baseUri);
      this.nodes[this.nodeIndex].IsEmptyElement = isEmptyElement;
      this.nodes[this.nodeIndex].LocalName = this.AtomicIndex(localName);
      this.nodes[this.nodeIndex].NamespaceURI = this.AtomicIndex(ns);
      this.nodes[this.nodeIndex].Prefix = this.AtomicIndex(prefix);
      this.nodes[this.nodeIndex].Value = this.NonAtomicIndex(value);
      this.nodes[this.nodeIndex].XmlLang = this.AtomicIndex(xmlLang);
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
      this.attributes[this.attributeIndex].LocalName = this.AtomicIndex(localName);
      this.attributes[this.attributeIndex].NamespaceURI = this.AtomicIndex(ns);
      this.attributes[this.attributeIndex].Prefix = this.AtomicIndex(prefix);
      this.attributes[this.attributeIndex].Value = this.NonAtomicIndex(value);
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
      this.namespaces[this.nsIndex].Name = this.AtomicIndex(name);
      this.namespaces[this.nsIndex].Namespace = this.AtomicIndex(ns);
      this.namespaces[this.nsIndex].NextNamespace = nextNs;
    }

    public override string XmlLang => (string) null;

    public override XmlSpace XmlSpace => XmlSpace.None;

    public override WriteState WriteState => this.state;

    public override void Close()
    {
      this.SetNodeArrayLength(this.nodeIndex + 1);
      this.SetAttributeArrayLength(this.attributeIndex + 1);
      this.SetNsArrayLength(this.nsIndex + 1);
      string[] destinationArray1 = new string[this.atomicIndex];
      Array.Copy((Array) this.atomicStringPool, (Array) destinationArray1, this.atomicIndex);
      this.atomicStringPool = destinationArray1;
      string[] destinationArray2 = new string[this.nonAtomicIndex];
      Array.Copy((Array) this.nonAtomicStringPool, (Array) destinationArray2, this.nonAtomicIndex);
      this.nonAtomicStringPool = destinationArray2;
      this.isClosed = true;
    }

    public override void Flush()
    {
    }

    public override string LookupPrefix(string ns)
    {
      for (int index = this.nsIndex; index != 0; index = this.namespaces[index].NextNamespace)
      {
        if (this.atomicStringPool[this.namespaces[index].Namespace] == ns)
          return this.atomicStringPool[this.namespaces[index].Name];
      }
      return (string) null;
    }

    public override void WriteCData(string data) => this.AddTextNode(data);

    private void AddTextNode(string data)
    {
      switch (this.state)
      {
        case WriteState.Element:
          this.CloseStartElement();
          goto case WriteState.Content;
        case WriteState.Content:
          if (this.nodes[this.nodeIndex].Parent == this.parentStack[this.parentStackIndex])
          {
            switch (this.nodes[this.nodeIndex].NodeType)
            {
              case XPathNodeType.Text:
              case XPathNodeType.SignificantWhitespace:
                string str = this.nonAtomicStringPool[this.nodes[this.nodeIndex].Value] + data;
                this.nodes[this.nodeIndex].Value = this.NonAtomicIndex(str);
                if (this.IsWhitespace(str))
                {
                  this.nodes[this.nodeIndex].NodeType = XPathNodeType.SignificantWhitespace;
                  return;
                }
                this.nodes[this.nodeIndex].NodeType = XPathNodeType.Text;
                return;
            }
          }
          int parentIndex = this.GetParentIndex();
          this.UpdateTreeForAddition();
          this.AddNode(parentIndex, 0, this.prevSibling, XPathNodeType.Text, (string) null, false, string.Empty, string.Empty, string.Empty, data, (string) null, 0, 0, 0);
          break;
        default:
          throw new InvalidOperationException("Invalid document state for CDATA section: " + (object) this.state);
      }
    }

    private void CheckTopLevelNode()
    {
      switch (this.state)
      {
        case WriteState.Start:
          break;
        case WriteState.Prolog:
          break;
        case WriteState.Element:
          this.CloseStartElement();
          break;
        case WriteState.Content:
          break;
        default:
          throw new InvalidOperationException("Invalid document state for CDATA section: " + (object) this.state);
      }
    }

    public override void WriteComment(string data)
    {
      this.CheckTopLevelNode();
      int parentIndex = this.GetParentIndex();
      this.UpdateTreeForAddition();
      this.AddNode(parentIndex, 0, this.prevSibling, XPathNodeType.Comment, (string) null, false, string.Empty, string.Empty, string.Empty, data, (string) null, 0, 0, 0);
    }

    public override void WriteProcessingInstruction(string name, string data)
    {
      this.CheckTopLevelNode();
      int parentIndex = this.GetParentIndex();
      this.UpdateTreeForAddition();
      this.AddNode(parentIndex, 0, this.prevSibling, XPathNodeType.ProcessingInstruction, (string) null, false, name, string.Empty, string.Empty, data, (string) null, 0, 0, 0);
    }

    public override void WriteWhitespace(string data)
    {
      this.CheckTopLevelNode();
      int parentIndex = this.GetParentIndex();
      this.UpdateTreeForAddition();
      this.AddNode(parentIndex, 0, this.prevSibling, XPathNodeType.Whitespace, (string) null, false, string.Empty, string.Empty, string.Empty, data, (string) null, 0, 0, 0);
    }

    public override void WriteStartDocument()
    {
    }

    public override void WriteStartDocument(bool standalone)
    {
    }

    public override void WriteEndDocument()
    {
    }

    public override void WriteStartElement(string prefix, string localName, string ns)
    {
      if (ns == null)
        ns = string.Empty;
      else if (prefix == null && ns.Length > 0)
        prefix = this.LookupPrefix(ns);
      if (prefix == null)
        prefix = string.Empty;
      switch (this.state)
      {
        case WriteState.Start:
        case WriteState.Prolog:
        case WriteState.Content:
          int parentIndex = this.GetParentIndex();
          this.UpdateTreeForAddition();
          this.WriteStartElement(parentIndex, this.prevSibling, prefix, localName, ns);
          this.state = WriteState.Element;
          break;
        case WriteState.Element:
          this.CloseStartElement();
          goto case WriteState.Start;
        default:
          throw new InvalidOperationException("Invalid document state for writing element: " + (object) this.state);
      }
    }

    private void WriteStartElement(
      int parent,
      int previousSibling,
      string prefix,
      string localName,
      string ns)
    {
      this.PrepareStartElement(previousSibling);
      this.AddNode(parent, 0, previousSibling, XPathNodeType.Element, (string) null, false, localName, ns, prefix, string.Empty, (string) null, this.lastNsInScope, 0, 0);
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

    public override void WriteEndElement() => this.WriteEndElement(false);

    public override void WriteFullEndElement() => this.WriteEndElement(true);

    private void WriteEndElement(bool full)
    {
      switch (this.state)
      {
        case WriteState.Element:
          this.CloseStartElement();
          goto case WriteState.Content;
        case WriteState.Content:
          --this.parentStackIndex;
          if (this.nodes[this.nodeIndex].NodeType != XPathNodeType.Element || full)
            break;
          this.nodes[this.nodeIndex].IsEmptyElement = true;
          break;
        default:
          throw new InvalidOperationException("Invalid state for writing EndElement: " + (object) this.state);
      }
    }

    public override void WriteStartAttribute(string prefix, string localName, string ns)
    {
      if (ns == null)
        ns = string.Empty;
      this.state = this.state == WriteState.Element ? WriteState.Attribute : throw new InvalidOperationException("Invalid document state for attribute: " + (object) this.state);
      if (ns == "http://www.w3.org/2000/xmlns/")
        this.ProcessNamespace(prefix == null || prefix == string.Empty ? string.Empty : localName, string.Empty);
      else
        this.ProcessAttribute(prefix, localName, ns, string.Empty);
    }

    private void ProcessNamespace(string prefix, string ns)
    {
      int nextNs = !this.hasLocalNs ? this.nodes[this.nodeIndex].FirstNamespace : this.nsIndex;
      ++this.nsIndex;
      this.AddNsNode(this.nodeIndex, prefix, ns, nextNs);
      this.hasLocalNs = true;
      this.openNamespace = true;
    }

    private void ProcessAttribute(string prefix, string localName, string ns, string value)
    {
      if (prefix == null && ns.Length > 0)
        prefix = this.LookupPrefix(ns);
      ++this.attributeIndex;
      this.AddAttribute(this.nodeIndex, localName, ns, prefix == null ? string.Empty : prefix, value, 0, 0);
      if (this.hasAttributes)
        this.attributes[this.attributeIndex - 1].NextAttribute = this.attributeIndex;
      else
        this.hasAttributes = true;
    }

    public override void WriteEndAttribute()
    {
      if (this.state != WriteState.Attribute)
        throw new InvalidOperationException();
      this.openNamespace = false;
      this.state = WriteState.Element;
    }

    public override void WriteString(string text)
    {
      if (this.WriteState == WriteState.Attribute)
      {
        if (this.openNamespace)
          this.namespaces[this.nsIndex].Namespace = this.AtomicIndex(this.atomicStringPool[this.namespaces[this.nsIndex].Namespace] + text);
        else
          this.attributes[this.attributeIndex].Value = this.NonAtomicIndex(this.nonAtomicStringPool[this.attributes[this.attributeIndex].Value] + text);
      }
      else
        this.AddTextNode(text);
    }

    public override void WriteRaw(string data) => this.WriteString(data);

    public override void WriteRaw(char[] data, int start, int len) => this.WriteString(new string(data, start, len));

    public override void WriteName(string name) => this.WriteString(name);

    public override void WriteNmToken(string name) => this.WriteString(name);

    public override void WriteBase64(byte[] buffer, int index, int count) => throw new NotSupportedException();

    public override void WriteBinHex(byte[] buffer, int index, int count) => throw new NotSupportedException();

    public override void WriteChars(char[] buffer, int index, int count) => throw new NotSupportedException();

    public override void WriteCharEntity(char c) => throw new NotSupportedException();

    public override void WriteDocType(string name, string pub, string sys, string intSubset) => throw new NotSupportedException();

    public override void WriteEntityRef(string name) => throw new NotSupportedException();

    public override void WriteQualifiedName(string localName, string ns) => throw new NotSupportedException();

    public override void WriteSurrogateCharEntity(char high, char low) => throw new NotSupportedException();

    private bool IsWhitespace(string data)
    {
      for (int index = 0; index < data.Length; ++index)
      {
        char ch = data[index];
        switch (ch)
        {
          case '\t':
          case '\n':
          case '\r':
            continue;
          default:
            if (ch != ' ')
              return false;
            continue;
        }
      }
      return true;
    }
  }
}
