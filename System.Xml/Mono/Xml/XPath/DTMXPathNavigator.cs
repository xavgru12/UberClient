// Decompiled with JetBrains decompiler
// Type: Mono.Xml.XPath.DTMXPathNavigator
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace Mono.Xml.XPath
{
  internal class DTMXPathNavigator : XPathNavigator, IXmlLineInfo
  {
    private XmlNameTable nameTable;
    private DTMXPathDocument document;
    private DTMXPathLinkedNode[] nodes;
    private DTMXPathAttributeNode[] attributes;
    private DTMXPathNamespaceNode[] namespaces;
    private Hashtable idTable;
    private bool currentIsNode;
    private bool currentIsAttr;
    private int currentNode;
    private int currentAttr;
    private int currentNs;
    private StringBuilder valueBuilder;

    public DTMXPathNavigator(
      DTMXPathDocument document,
      XmlNameTable nameTable,
      DTMXPathLinkedNode[] nodes,
      DTMXPathAttributeNode[] attributes,
      DTMXPathNamespaceNode[] namespaces,
      Hashtable idTable)
    {
      this.nodes = nodes;
      this.attributes = attributes;
      this.namespaces = namespaces;
      this.idTable = idTable;
      this.nameTable = nameTable;
      this.MoveToRoot();
      this.document = document;
    }

    public DTMXPathNavigator(DTMXPathNavigator org)
      : this(org.document, org.nameTable, org.nodes, org.attributes, org.namespaces, org.idTable)
    {
      this.currentIsNode = org.currentIsNode;
      this.currentIsAttr = org.currentIsAttr;
      this.currentNode = org.currentNode;
      this.currentAttr = org.currentAttr;
      this.currentNs = org.currentNs;
    }

    internal DTMXPathNavigator(XmlNameTable nt) => this.nameTable = nt;

    int IXmlLineInfo.LineNumber => this.currentIsAttr ? this.attributes[this.currentAttr].LineNumber : this.nodes[this.currentNode].LineNumber;

    int IXmlLineInfo.LinePosition => this.currentIsAttr ? this.attributes[this.currentAttr].LinePosition : this.nodes[this.currentNode].LinePosition;

    bool IXmlLineInfo.HasLineInfo() => true;

    public override string BaseURI => this.nodes[this.currentNode].BaseURI;

    public override bool HasAttributes => this.currentIsNode && this.nodes[this.currentNode].FirstAttribute != 0;

    public override bool HasChildren => this.currentIsNode && this.nodes[this.currentNode].FirstChild != 0;

    public override bool IsEmptyElement => this.currentIsNode && this.nodes[this.currentNode].IsEmptyElement;

    public override string LocalName
    {
      get
      {
        if (this.currentIsNode)
          return this.nodes[this.currentNode].LocalName;
        return this.currentIsAttr ? this.attributes[this.currentAttr].LocalName : this.namespaces[this.currentNs].Name;
      }
    }

    public override string Name
    {
      get
      {
        string prefix;
        string localName;
        if (this.currentIsNode)
        {
          prefix = this.nodes[this.currentNode].Prefix;
          localName = this.nodes[this.currentNode].LocalName;
        }
        else
        {
          if (!this.currentIsAttr)
            return this.namespaces[this.currentNs].Name;
          prefix = this.attributes[this.currentAttr].Prefix;
          localName = this.attributes[this.currentAttr].LocalName;
        }
        return prefix != string.Empty ? prefix + (object) ':' + localName : localName;
      }
    }

    public override string NamespaceURI
    {
      get
      {
        if (this.currentIsNode)
          return this.nodes[this.currentNode].NamespaceURI;
        return this.currentIsAttr ? this.attributes[this.currentAttr].NamespaceURI : string.Empty;
      }
    }

    public override XmlNameTable NameTable => this.nameTable;

    public override XPathNodeType NodeType
    {
      get
      {
        if (this.currentIsNode)
          return this.nodes[this.currentNode].NodeType;
        return this.currentIsAttr ? XPathNodeType.Attribute : XPathNodeType.Namespace;
      }
    }

    public override string Prefix
    {
      get
      {
        if (this.currentIsNode)
          return this.nodes[this.currentNode].Prefix;
        return this.currentIsAttr ? this.attributes[this.currentAttr].Prefix : string.Empty;
      }
    }

    public override string Value
    {
      get
      {
        if (this.currentIsAttr)
          return this.attributes[this.currentAttr].Value;
        if (!this.currentIsNode)
          return this.namespaces[this.currentNs].Namespace;
        switch (this.nodes[this.currentNode].NodeType)
        {
          case XPathNodeType.Text:
          case XPathNodeType.SignificantWhitespace:
          case XPathNodeType.Whitespace:
          case XPathNodeType.ProcessingInstruction:
          case XPathNodeType.Comment:
            return this.nodes[this.currentNode].Value;
          default:
            int firstChild = this.nodes[this.currentNode].FirstChild;
            if (firstChild == 0)
              return string.Empty;
            if (this.valueBuilder == null)
              this.valueBuilder = new StringBuilder();
            else
              this.valueBuilder.Length = 0;
            int num = this.nodes[this.currentNode].NextSibling;
            if (num == 0)
            {
              int index = this.currentNode;
              do
              {
                index = this.nodes[index].Parent;
                num = this.nodes[index].NextSibling;
              }
              while (num == 0 && index != 0);
              if (num == 0)
                num = this.nodes.Length;
            }
            for (; firstChild < num; ++firstChild)
            {
              switch (this.nodes[firstChild].NodeType)
              {
                case XPathNodeType.Text:
                case XPathNodeType.SignificantWhitespace:
                case XPathNodeType.Whitespace:
                  this.valueBuilder.Append(this.nodes[firstChild].Value);
                  break;
              }
            }
            return this.valueBuilder.ToString();
        }
      }
    }

    public override string XmlLang => this.nodes[this.currentNode].XmlLang;

    public override XPathNavigator Clone() => (XPathNavigator) new DTMXPathNavigator(this);

    public override XmlNodeOrder ComparePosition(XPathNavigator nav)
    {
      if (!(nav is DTMXPathNavigator dtmxPathNavigator) || dtmxPathNavigator.document != this.document)
        return XmlNodeOrder.Unknown;
      if (this.currentNode > dtmxPathNavigator.currentNode)
        return XmlNodeOrder.After;
      if (this.currentNode < dtmxPathNavigator.currentNode)
        return XmlNodeOrder.Before;
      if (dtmxPathNavigator.currentIsAttr)
      {
        if (!this.currentIsAttr)
          return XmlNodeOrder.Before;
        if (this.currentAttr > dtmxPathNavigator.currentAttr)
          return XmlNodeOrder.After;
        return this.currentAttr < dtmxPathNavigator.currentAttr ? XmlNodeOrder.Before : XmlNodeOrder.Same;
      }
      if (!dtmxPathNavigator.currentIsNode)
      {
        if (this.currentIsNode)
          return XmlNodeOrder.Before;
        if (this.currentNs > dtmxPathNavigator.currentNs)
          return XmlNodeOrder.After;
        return this.currentNs < dtmxPathNavigator.currentNs ? XmlNodeOrder.Before : XmlNodeOrder.Same;
      }
      return !dtmxPathNavigator.currentIsNode ? XmlNodeOrder.Before : XmlNodeOrder.Same;
    }

    private int findAttribute(string localName, string namespaceURI)
    {
      if (this.currentIsNode && this.nodes[this.currentNode].NodeType == XPathNodeType.Element)
      {
        for (int attribute = this.nodes[this.currentNode].FirstAttribute; attribute != 0; attribute = this.attributes[attribute].NextAttribute)
        {
          if (this.attributes[attribute].LocalName == localName && this.attributes[attribute].NamespaceURI == namespaceURI)
            return attribute;
        }
      }
      return 0;
    }

    public override string GetAttribute(string localName, string namespaceURI)
    {
      int attribute = this.findAttribute(localName, namespaceURI);
      return attribute != 0 ? this.attributes[attribute].Value : string.Empty;
    }

    public override string GetNamespace(string name)
    {
      if (this.currentIsNode && this.nodes[this.currentNode].NodeType == XPathNodeType.Element)
      {
        for (int index = this.nodes[this.currentNode].FirstNamespace; index != 0; index = this.namespaces[index].NextNamespace)
        {
          if (this.namespaces[index].Name == name)
            return this.namespaces[index].Namespace;
        }
      }
      return string.Empty;
    }

    public override bool IsDescendant(XPathNavigator nav)
    {
      if (!(nav is DTMXPathNavigator dtmxPathNavigator) || dtmxPathNavigator.document != this.document)
        return false;
      if (dtmxPathNavigator.currentNode == this.currentNode)
        return !dtmxPathNavigator.currentIsNode;
      for (int parent = this.nodes[dtmxPathNavigator.currentNode].Parent; parent != 0; parent = this.nodes[parent].Parent)
      {
        if (parent == this.currentNode)
          return true;
      }
      return false;
    }

    public override bool IsSamePosition(XPathNavigator other)
    {
      if (!(other is DTMXPathNavigator dtmxPathNavigator) || dtmxPathNavigator.document != this.document || this.currentNode != dtmxPathNavigator.currentNode || this.currentIsAttr != dtmxPathNavigator.currentIsAttr || this.currentIsNode != dtmxPathNavigator.currentIsNode)
        return false;
      if (this.currentIsAttr)
        return this.currentAttr == dtmxPathNavigator.currentAttr;
      return this.currentIsNode || this.currentNs == dtmxPathNavigator.currentNs;
    }

    public override bool MoveTo(XPathNavigator other)
    {
      if (!(other is DTMXPathNavigator dtmxPathNavigator) || dtmxPathNavigator.document != this.document)
        return false;
      this.currentNode = dtmxPathNavigator.currentNode;
      this.currentAttr = dtmxPathNavigator.currentAttr;
      this.currentNs = dtmxPathNavigator.currentNs;
      this.currentIsNode = dtmxPathNavigator.currentIsNode;
      this.currentIsAttr = dtmxPathNavigator.currentIsAttr;
      return true;
    }

    public override bool MoveToAttribute(string localName, string namespaceURI)
    {
      int attribute = this.findAttribute(localName, namespaceURI);
      if (attribute == 0)
        return false;
      this.currentAttr = attribute;
      this.currentIsAttr = true;
      this.currentIsNode = false;
      return true;
    }

    public override bool MoveToFirst()
    {
      if (this.currentIsAttr)
        return false;
      int index1 = this.nodes[this.currentNode].PreviousSibling;
      if (index1 == 0)
        return false;
      for (int index2 = index1; index2 != 0; index2 = this.nodes[index1].PreviousSibling)
        index1 = index2;
      this.currentNode = index1;
      this.currentIsNode = true;
      return true;
    }

    public override bool MoveToFirstAttribute()
    {
      if (!this.currentIsNode)
        return false;
      int firstAttribute = this.nodes[this.currentNode].FirstAttribute;
      if (firstAttribute == 0)
        return false;
      this.currentAttr = firstAttribute;
      this.currentIsAttr = true;
      this.currentIsNode = false;
      return true;
    }

    public override bool MoveToFirstChild()
    {
      if (!this.currentIsNode)
        return false;
      int firstChild = this.nodes[this.currentNode].FirstChild;
      if (firstChild == 0)
        return false;
      this.currentNode = firstChild;
      return true;
    }

    private bool moveToSpecifiedNamespace(int cur, XPathNamespaceScope namespaceScope)
    {
      if (cur == 0 || namespaceScope == XPathNamespaceScope.Local && this.namespaces[cur].DeclaredElement != this.currentNode || namespaceScope != XPathNamespaceScope.All && this.namespaces[cur].Namespace == "http://www.w3.org/XML/1998/namespace" || cur == 0)
        return false;
      this.moveToNamespace(cur);
      return true;
    }

    public override bool MoveToFirstNamespace(XPathNamespaceScope namespaceScope) => this.currentIsNode && this.moveToSpecifiedNamespace(this.nodes[this.currentNode].FirstNamespace, namespaceScope);

    public override bool MoveToId(string id)
    {
      if (!this.idTable.ContainsKey((object) id))
        return false;
      this.currentNode = (int) this.idTable[(object) id];
      this.currentIsNode = true;
      this.currentIsAttr = false;
      return true;
    }

    private void moveToNamespace(int nsNode)
    {
      this.currentIsNode = this.currentIsAttr = false;
      this.currentNs = nsNode;
    }

    public override bool MoveToNamespace(string name)
    {
      int nsNode = this.nodes[this.currentNode].FirstNamespace;
      if (nsNode == 0)
        return false;
      for (; nsNode != 0; nsNode = this.namespaces[nsNode].NextNamespace)
      {
        if (this.namespaces[nsNode].Name == name)
        {
          this.moveToNamespace(nsNode);
          return true;
        }
      }
      return false;
    }

    public override bool MoveToNext()
    {
      if (this.currentIsAttr)
        return false;
      int nextSibling = this.nodes[this.currentNode].NextSibling;
      if (nextSibling == 0)
        return false;
      this.currentNode = nextSibling;
      this.currentIsNode = true;
      return true;
    }

    public override bool MoveToNextAttribute()
    {
      if (!this.currentIsAttr)
        return false;
      int nextAttribute = this.attributes[this.currentAttr].NextAttribute;
      if (nextAttribute == 0)
        return false;
      this.currentAttr = nextAttribute;
      return true;
    }

    public override bool MoveToNextNamespace(XPathNamespaceScope namespaceScope) => !this.currentIsAttr && !this.currentIsNode && this.moveToSpecifiedNamespace(this.namespaces[this.currentNs].NextNamespace, namespaceScope);

    public override bool MoveToParent()
    {
      if (!this.currentIsNode)
      {
        this.currentIsNode = true;
        this.currentIsAttr = false;
        return true;
      }
      int parent = this.nodes[this.currentNode].Parent;
      if (parent == 0)
        return false;
      this.currentNode = parent;
      return true;
    }

    public override bool MoveToPrevious()
    {
      if (this.currentIsAttr)
        return false;
      int previousSibling = this.nodes[this.currentNode].PreviousSibling;
      if (previousSibling == 0)
        return false;
      this.currentNode = previousSibling;
      this.currentIsNode = true;
      return true;
    }

    public override void MoveToRoot()
    {
      this.currentNode = 1;
      this.currentIsNode = true;
      this.currentIsAttr = false;
    }
  }
}
