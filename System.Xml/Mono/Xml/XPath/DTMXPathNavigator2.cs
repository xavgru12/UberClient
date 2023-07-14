// Decompiled with JetBrains decompiler
// Type: Mono.Xml.XPath.DTMXPathNavigator2
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace Mono.Xml.XPath
{
  internal class DTMXPathNavigator2 : XPathNavigator, IXmlLineInfo
  {
    private DTMXPathDocument2 document;
    private bool currentIsNode;
    private bool currentIsAttr;
    private int currentNode;
    private int currentAttr;
    private int currentNs;

    public DTMXPathNavigator2(DTMXPathDocument2 document)
    {
      this.MoveToRoot();
      this.document = document;
    }

    public DTMXPathNavigator2(DTMXPathNavigator2 org)
    {
      this.document = org.document;
      this.currentIsNode = org.currentIsNode;
      this.currentIsAttr = org.currentIsAttr;
      this.currentNode = org.currentNode;
      this.currentAttr = org.currentAttr;
      this.currentNs = org.currentNs;
    }

    int IXmlLineInfo.LineNumber => this.currentIsAttr ? this.attributes[this.currentAttr].LineNumber : this.nodes[this.currentNode].LineNumber;

    int IXmlLineInfo.LinePosition => this.currentIsAttr ? this.attributes[this.currentAttr].LinePosition : this.nodes[this.currentNode].LinePosition;

    bool IXmlLineInfo.HasLineInfo() => true;

    private XmlNameTable nameTable => this.document.NameTable;

    private DTMXPathLinkedNode2[] nodes => this.document.Nodes;

    private DTMXPathAttributeNode2[] attributes => this.document.Attributes;

    private DTMXPathNamespaceNode2[] namespaces => this.document.Namespaces;

    private string[] atomicStringPool => this.document.AtomicStringPool;

    private string[] nonAtomicStringPool => this.document.NonAtomicStringPool;

    private Hashtable idTable => this.document.IdTable;

    public override string BaseURI => this.atomicStringPool[this.nodes[this.currentNode].BaseURI];

    public override bool HasAttributes => this.currentIsNode && this.nodes[this.currentNode].FirstAttribute != 0;

    public override bool HasChildren => this.currentIsNode && this.nodes[this.currentNode].FirstChild != 0;

    public override bool IsEmptyElement => this.currentIsNode && this.nodes[this.currentNode].IsEmptyElement;

    public override string LocalName
    {
      get
      {
        if (this.currentIsNode)
          return this.atomicStringPool[this.nodes[this.currentNode].LocalName];
        return this.currentIsAttr ? this.atomicStringPool[this.attributes[this.currentAttr].LocalName] : this.atomicStringPool[this.namespaces[this.currentNs].Name];
      }
    }

    public override string Name
    {
      get
      {
        string str1;
        string str2;
        if (this.currentIsNode)
        {
          str1 = this.atomicStringPool[this.nodes[this.currentNode].Prefix];
          str2 = this.atomicStringPool[this.nodes[this.currentNode].LocalName];
        }
        else
        {
          if (!this.currentIsAttr)
            return this.atomicStringPool[this.namespaces[this.currentNs].Name];
          str1 = this.atomicStringPool[this.attributes[this.currentAttr].Prefix];
          str2 = this.atomicStringPool[this.attributes[this.currentAttr].LocalName];
        }
        return str1 != string.Empty ? str1 + (object) ':' + str2 : str2;
      }
    }

    public override string NamespaceURI
    {
      get
      {
        if (this.currentIsNode)
          return this.atomicStringPool[this.nodes[this.currentNode].NamespaceURI];
        return this.currentIsAttr ? this.atomicStringPool[this.attributes[this.currentAttr].NamespaceURI] : string.Empty;
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
          return this.atomicStringPool[this.nodes[this.currentNode].Prefix];
        return this.currentIsAttr ? this.atomicStringPool[this.attributes[this.currentAttr].Prefix] : string.Empty;
      }
    }

    public override string Value
    {
      get
      {
        if (this.currentIsAttr)
          return this.nonAtomicStringPool[this.attributes[this.currentAttr].Value];
        if (!this.currentIsNode)
          return this.atomicStringPool[this.namespaces[this.currentNs].Namespace];
        switch (this.nodes[this.currentNode].NodeType)
        {
          case XPathNodeType.Text:
          case XPathNodeType.SignificantWhitespace:
          case XPathNodeType.Whitespace:
          case XPathNodeType.ProcessingInstruction:
          case XPathNodeType.Comment:
            return this.nonAtomicStringPool[this.nodes[this.currentNode].Value];
          default:
            int firstChild = this.nodes[this.currentNode].FirstChild;
            if (firstChild == 0)
              return string.Empty;
            StringBuilder valueBuilder = (StringBuilder) null;
            this.BuildValue(firstChild, ref valueBuilder);
            return valueBuilder == null ? string.Empty : valueBuilder.ToString();
        }
      }
    }

    private void BuildValue(int iter, ref StringBuilder valueBuilder)
    {
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
      for (; iter < num; ++iter)
      {
        switch (this.nodes[iter].NodeType)
        {
          case XPathNodeType.Text:
          case XPathNodeType.SignificantWhitespace:
          case XPathNodeType.Whitespace:
            if (valueBuilder == null)
              valueBuilder = new StringBuilder();
            valueBuilder.Append(this.nonAtomicStringPool[this.nodes[iter].Value]);
            break;
        }
      }
    }

    public override string XmlLang => this.atomicStringPool[this.nodes[this.currentNode].XmlLang];

    public override XPathNavigator Clone() => (XPathNavigator) new DTMXPathNavigator2(this);

    public override XmlNodeOrder ComparePosition(XPathNavigator nav)
    {
      if (!(nav is DTMXPathNavigator2 dtmxPathNavigator2) || dtmxPathNavigator2.document != this.document)
        return XmlNodeOrder.Unknown;
      if (this.currentNode > dtmxPathNavigator2.currentNode)
        return XmlNodeOrder.After;
      if (this.currentNode < dtmxPathNavigator2.currentNode)
        return XmlNodeOrder.Before;
      if (dtmxPathNavigator2.currentIsAttr)
      {
        if (!this.currentIsAttr)
          return XmlNodeOrder.Before;
        if (this.currentAttr > dtmxPathNavigator2.currentAttr)
          return XmlNodeOrder.After;
        return this.currentAttr < dtmxPathNavigator2.currentAttr ? XmlNodeOrder.Before : XmlNodeOrder.Same;
      }
      if (!dtmxPathNavigator2.currentIsNode)
      {
        if (this.currentIsNode)
          return XmlNodeOrder.Before;
        if (this.currentNs > dtmxPathNavigator2.currentNs)
          return XmlNodeOrder.After;
        return this.currentNs < dtmxPathNavigator2.currentNs ? XmlNodeOrder.Before : XmlNodeOrder.Same;
      }
      return !dtmxPathNavigator2.currentIsNode ? XmlNodeOrder.Before : XmlNodeOrder.Same;
    }

    private int findAttribute(string localName, string namespaceURI)
    {
      if (this.currentIsNode && this.nodes[this.currentNode].NodeType == XPathNodeType.Element)
      {
        for (int attribute = this.nodes[this.currentNode].FirstAttribute; attribute != 0; attribute = this.attributes[attribute].NextAttribute)
        {
          if (this.atomicStringPool[this.attributes[attribute].LocalName] == localName && this.atomicStringPool[this.attributes[attribute].NamespaceURI] == namespaceURI)
            return attribute;
        }
      }
      return 0;
    }

    public override string GetAttribute(string localName, string namespaceURI)
    {
      int attribute = this.findAttribute(localName, namespaceURI);
      return attribute != 0 ? this.nonAtomicStringPool[this.attributes[attribute].Value] : string.Empty;
    }

    public override string GetNamespace(string name)
    {
      if (this.currentIsNode && this.nodes[this.currentNode].NodeType == XPathNodeType.Element)
      {
        for (int index = this.nodes[this.currentNode].FirstNamespace; index != 0; index = this.namespaces[index].NextNamespace)
        {
          if (this.atomicStringPool[this.namespaces[index].Name] == name)
            return this.atomicStringPool[this.namespaces[index].Namespace];
        }
      }
      return string.Empty;
    }

    public override bool IsDescendant(XPathNavigator nav)
    {
      if (!(nav is DTMXPathNavigator2 dtmxPathNavigator2) || dtmxPathNavigator2.document != this.document)
        return false;
      if (dtmxPathNavigator2.currentNode == this.currentNode)
        return !dtmxPathNavigator2.currentIsNode;
      int parent = this.nodes[dtmxPathNavigator2.currentNode].Parent;
      if (parent < this.currentNode)
        return false;
      for (; parent != 0; parent = this.nodes[parent].Parent)
      {
        if (parent == this.currentNode)
          return true;
      }
      return false;
    }

    public override bool IsSamePosition(XPathNavigator other)
    {
      if (!(other is DTMXPathNavigator2 dtmxPathNavigator2) || dtmxPathNavigator2.document != this.document || this.currentNode != dtmxPathNavigator2.currentNode || this.currentIsAttr != dtmxPathNavigator2.currentIsAttr || this.currentIsNode != dtmxPathNavigator2.currentIsNode)
        return false;
      if (this.currentIsAttr)
        return this.currentAttr == dtmxPathNavigator2.currentAttr;
      return this.currentIsNode || this.currentNs == dtmxPathNavigator2.currentNs;
    }

    public override bool MoveTo(XPathNavigator other)
    {
      if (!(other is DTMXPathNavigator2 dtmxPathNavigator2) || dtmxPathNavigator2.document != this.document)
        return false;
      this.currentNode = dtmxPathNavigator2.currentNode;
      this.currentAttr = dtmxPathNavigator2.currentAttr;
      this.currentNs = dtmxPathNavigator2.currentNs;
      this.currentIsNode = dtmxPathNavigator2.currentIsNode;
      this.currentIsAttr = dtmxPathNavigator2.currentIsAttr;
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
      int previousSibling = this.nodes[this.currentNode].PreviousSibling;
      if (previousSibling == 0)
        return false;
      this.currentNode = this.nodes[this.nodes[previousSibling].Parent].FirstChild;
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
      if (cur == 0 || namespaceScope == XPathNamespaceScope.Local && this.namespaces[cur].DeclaredElement != this.currentNode || namespaceScope != XPathNamespaceScope.All && this.namespaces[cur].Namespace == 2 || cur == 0)
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
        if (this.atomicStringPool[this.namespaces[nsNode].Name] == name)
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
