// Decompiled with JetBrains decompiler
// Type: System.Xml.XmlDocumentNavigator
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.Xml.Schema;
using System.Xml.XPath;

namespace System.Xml
{
  internal class XmlDocumentNavigator : XPathNavigator, IHasXmlNode
  {
    private const string Xmlns = "http://www.w3.org/2000/xmlns/";
    private const string XmlnsXML = "http://www.w3.org/XML/1998/namespace";
    private XmlNode node;
    private XmlAttribute nsNode;
    private ArrayList iteratedNsNames;

    internal XmlDocumentNavigator(XmlNode node)
    {
      this.node = node;
      if (node.NodeType != XmlNodeType.Attribute || !(node.NamespaceURI == "http://www.w3.org/2000/xmlns/"))
        return;
      this.nsNode = (XmlAttribute) node;
      node = (XmlNode) this.nsNode.OwnerElement;
    }

    XmlNode IHasXmlNode.GetNode() => this.Node;

    internal XmlDocument Document => this.node.NodeType == XmlNodeType.Document ? this.node as XmlDocument : this.node.OwnerDocument;

    public override string BaseURI => this.node.BaseURI;

    public override bool HasAttributes
    {
      get
      {
        if (this.NsNode != null || !(this.node is XmlElement node) || !node.HasAttributes)
          return false;
        for (int i = 0; i < this.node.Attributes.Count; ++i)
        {
          if (this.node.Attributes[i].NamespaceURI != "http://www.w3.org/2000/xmlns/")
            return true;
        }
        return false;
      }
    }

    public override bool HasChildren
    {
      get
      {
        if (this.NsNode != null)
          return false;
        XPathNodeType nodeType = this.NodeType;
        return (nodeType == XPathNodeType.Root || nodeType == XPathNodeType.Element) && this.GetFirstChild(this.node) != null;
      }
    }

    public override bool IsEmptyElement => this.NsNode == null && this.node.NodeType == XmlNodeType.Element && ((XmlElement) this.node).IsEmpty;

    public XmlAttribute NsNode
    {
      get => this.nsNode;
      set
      {
        if (value == null)
        {
          this.iteratedNsNames = (ArrayList) null;
        }
        else
        {
          if (this.iteratedNsNames == null)
            this.iteratedNsNames = new ArrayList();
          else if (this.iteratedNsNames.IsReadOnly)
            this.iteratedNsNames = new ArrayList((ICollection) this.iteratedNsNames);
          this.iteratedNsNames.Add((object) value.Name);
        }
        this.nsNode = value;
      }
    }

    public override string LocalName
    {
      get
      {
        XmlAttribute nsNode = this.NsNode;
        if (nsNode != null)
        {
          if (nsNode == this.Document.NsNodeXml)
            return "xml";
          return nsNode.Name == "xmlns" ? string.Empty : nsNode.LocalName;
        }
        XPathNodeType nodeType = this.NodeType;
        int num;
        switch (nodeType)
        {
          case XPathNodeType.Element:
          case XPathNodeType.Attribute:
          case XPathNodeType.ProcessingInstruction:
            num = 1;
            break;
          default:
            num = nodeType == XPathNodeType.Namespace ? 1 : 0;
            break;
        }
        return num != 0 ? this.node.LocalName : string.Empty;
      }
    }

    public override string Name
    {
      get
      {
        if (this.NsNode != null)
          return this.LocalName;
        XPathNodeType nodeType = this.NodeType;
        int num;
        switch (nodeType)
        {
          case XPathNodeType.Element:
          case XPathNodeType.Attribute:
          case XPathNodeType.ProcessingInstruction:
            num = 1;
            break;
          default:
            num = nodeType == XPathNodeType.Namespace ? 1 : 0;
            break;
        }
        return num != 0 ? this.node.Name : string.Empty;
      }
    }

    public override string NamespaceURI => this.NsNode != null ? string.Empty : this.node.NamespaceURI;

    public override XmlNameTable NameTable => this.Document.NameTable;

    public override XPathNodeType NodeType
    {
      get
      {
        if (this.NsNode != null)
          return XPathNodeType.Namespace;
        XmlNode n = this.node;
        bool flag = false;
        do
        {
          switch (n.NodeType)
          {
            case XmlNodeType.Text:
            case XmlNodeType.CDATA:
              return XPathNodeType.Text;
            case XmlNodeType.Whitespace:
              n = this.GetNextSibling(n);
              break;
            case XmlNodeType.SignificantWhitespace:
              flag = true;
              n = this.GetNextSibling(n);
              break;
            default:
              n = (XmlNode) null;
              break;
          }
        }
        while (n != null);
        return flag ? XPathNodeType.SignificantWhitespace : this.node.XPathNodeType;
      }
    }

    public override string Prefix => this.NsNode != null ? string.Empty : this.node.Prefix;

    public override IXmlSchemaInfo SchemaInfo => this.NsNode != null ? (IXmlSchemaInfo) null : this.node.SchemaInfo;

    public override object UnderlyingObject => (object) this.node;

    public override string Value
    {
      get
      {
        switch (this.NodeType)
        {
          case XPathNodeType.Root:
          case XPathNodeType.Element:
            return this.node.InnerText;
          case XPathNodeType.Attribute:
          case XPathNodeType.ProcessingInstruction:
          case XPathNodeType.Comment:
            return this.node.Value;
          case XPathNodeType.Namespace:
            return this.NsNode == this.Document.NsNodeXml ? "http://www.w3.org/XML/1998/namespace" : this.NsNode.Value;
          case XPathNodeType.Text:
          case XPathNodeType.SignificantWhitespace:
          case XPathNodeType.Whitespace:
            string str = this.node.Value;
            for (XmlNode nextSibling = this.GetNextSibling(this.node); nextSibling != null; nextSibling = this.GetNextSibling(nextSibling))
            {
              switch (nextSibling.XPathNodeType)
              {
                case XPathNodeType.Text:
                case XPathNodeType.SignificantWhitespace:
                case XPathNodeType.Whitespace:
                  str += nextSibling.Value;
                  continue;
                default:
                  goto label_6;
              }
            }
label_6:
            return str;
          default:
            return string.Empty;
        }
      }
    }

    public override string XmlLang => this.node.XmlLang;

    private bool CheckNsNameAppearance(string name, string ns)
    {
      if (this.iteratedNsNames != null && this.iteratedNsNames.Contains((object) name))
        return true;
      if (!(ns == string.Empty))
        return false;
      if (this.iteratedNsNames == null)
        this.iteratedNsNames = new ArrayList();
      else if (this.iteratedNsNames.IsReadOnly)
        this.iteratedNsNames = new ArrayList((ICollection) this.iteratedNsNames);
      this.iteratedNsNames.Add((object) "xmlns");
      return true;
    }

    public override XPathNavigator Clone() => (XPathNavigator) new XmlDocumentNavigator(this.node)
    {
      nsNode = this.nsNode,
      iteratedNsNames = (this.iteratedNsNames == null || this.iteratedNsNames.IsReadOnly ? this.iteratedNsNames : ArrayList.ReadOnly(this.iteratedNsNames))
    };

    public override string GetAttribute(string localName, string namespaceURI) => this.HasAttributes && this.Node is XmlElement node ? node.GetAttribute(localName, namespaceURI) : string.Empty;

    public override string GetNamespace(string name) => this.Node.GetNamespaceOfPrefix(name);

    public override bool IsDescendant(XPathNavigator other)
    {
      if (this.NsNode != null || !(other is XmlDocumentNavigator documentNavigator))
        return false;
      for (XmlNode xmlNode = documentNavigator.node.NodeType != XmlNodeType.Attribute ? documentNavigator.node.ParentNode : (XmlNode) ((XmlAttribute) documentNavigator.node).OwnerElement; xmlNode != null; xmlNode = xmlNode.ParentNode)
      {
        if (xmlNode == this.node)
          return true;
      }
      return false;
    }

    public override bool IsSamePosition(XPathNavigator other) => other is XmlDocumentNavigator documentNavigator && this.node == documentNavigator.node && this.NsNode == documentNavigator.NsNode;

    public override bool MoveTo(XPathNavigator other)
    {
      if (!(other is XmlDocumentNavigator documentNavigator) || this.Document != documentNavigator.Document)
        return false;
      this.node = documentNavigator.node;
      this.NsNode = documentNavigator.NsNode;
      return true;
    }

    public override bool MoveToAttribute(string localName, string namespaceURI)
    {
      if (this.HasAttributes)
      {
        XmlAttribute attribute = this.node.Attributes[localName, namespaceURI];
        if (attribute != null)
        {
          this.node = (XmlNode) attribute;
          this.NsNode = (XmlAttribute) null;
          return true;
        }
      }
      return false;
    }

    public override bool MoveToFirstAttribute()
    {
      if (this.NodeType == XPathNodeType.Element && (this.node as XmlElement).HasAttributes)
      {
        for (int i = 0; i < this.node.Attributes.Count; ++i)
        {
          XmlAttribute attribute = this.node.Attributes[i];
          if (attribute.NamespaceURI != "http://www.w3.org/2000/xmlns/")
          {
            this.node = (XmlNode) attribute;
            this.NsNode = (XmlAttribute) null;
            return true;
          }
        }
      }
      return false;
    }

    public override bool MoveToFirstChild()
    {
      if (!this.HasChildren)
        return false;
      XmlNode firstChild = this.GetFirstChild(this.node);
      if (firstChild == null)
        return false;
      this.node = firstChild;
      return true;
    }

    public override bool MoveToFirstNamespace(XPathNamespaceScope namespaceScope)
    {
      if (this.NodeType != XPathNodeType.Element)
        return false;
      n = this.node as XmlElement;
      do
      {
        if (n.HasAttributes)
        {
          for (int i = 0; i < n.Attributes.Count; ++i)
          {
            XmlAttribute attribute = n.Attributes[i];
            if (attribute.NamespaceURI == "http://www.w3.org/2000/xmlns/" && !this.CheckNsNameAppearance(attribute.Name, attribute.Value))
            {
              this.NsNode = attribute;
              return true;
            }
          }
        }
        if (namespaceScope == XPathNamespaceScope.Local)
          return false;
      }
      while (this.GetParentNode((XmlNode) n) is XmlElement n);
      if (namespaceScope != XPathNamespaceScope.All || this.CheckNsNameAppearance(this.Document.NsNodeXml.Name, this.Document.NsNodeXml.Value))
        return false;
      this.NsNode = this.Document.NsNodeXml;
      return true;
    }

    public override bool MoveToId(string id)
    {
      XmlElement elementById = this.Document.GetElementById(id);
      if (elementById == null)
        return false;
      this.node = (XmlNode) elementById;
      return true;
    }

    public override bool MoveToNamespace(string name)
    {
      if (name == "xml")
      {
        this.NsNode = this.Document.NsNodeXml;
        return true;
      }
      if (this.NodeType != XPathNodeType.Element)
        return false;
      xmlElement = this.node as XmlElement;
      do
      {
        if (xmlElement.HasAttributes)
        {
          for (int i = 0; i < xmlElement.Attributes.Count; ++i)
          {
            XmlAttribute attribute = xmlElement.Attributes[i];
            if (attribute.NamespaceURI == "http://www.w3.org/2000/xmlns/" && attribute.Name == name)
            {
              this.NsNode = attribute;
              return true;
            }
          }
        }
      }
      while (this.GetParentNode(this.node) is XmlElement xmlElement);
      return false;
    }

    public override bool MoveToNext()
    {
      if (this.NsNode != null)
        return false;
      XmlNode n = this.node;
      if (this.NodeType == XPathNodeType.Text)
      {
label_3:
        n = this.GetNextSibling(n);
        if (n == null)
          return false;
        switch (n.NodeType)
        {
          case XmlNodeType.Text:
          case XmlNodeType.CDATA:
          case XmlNodeType.Whitespace:
          case XmlNodeType.SignificantWhitespace:
            goto label_3;
        }
      }
      else
        n = this.GetNextSibling(n);
      if (n == null)
        return false;
      this.node = n;
      return true;
    }

    public override bool MoveToNextAttribute()
    {
      if (this.node == null || this.NodeType != XPathNodeType.Attribute)
        return false;
      int i1 = 0;
      XmlElement ownerElement = ((XmlAttribute) this.node).OwnerElement;
      if (ownerElement == null)
        return false;
      int count = ownerElement.Attributes.Count;
      while (i1 < count && ownerElement.Attributes[i1] != this.node)
        ++i1;
      if (i1 == count)
        return false;
      for (int i2 = i1 + 1; i2 < count; ++i2)
      {
        if (ownerElement.Attributes[i2].NamespaceURI != "http://www.w3.org/2000/xmlns/")
        {
          this.node = (XmlNode) ownerElement.Attributes[i2];
          this.NsNode = (XmlAttribute) null;
          return true;
        }
      }
      return false;
    }

    public override bool MoveToNextNamespace(XPathNamespaceScope namespaceScope)
    {
      if (this.NsNode == this.Document.NsNodeXml || this.NsNode == null)
        return false;
      int i1 = 0;
      XmlElement ownerElement = this.NsNode.OwnerElement;
      if (ownerElement == null)
        return false;
      int count = ownerElement.Attributes.Count;
      while (i1 < count && ownerElement.Attributes[i1] != this.NsNode)
        ++i1;
      if (i1 == count)
        return false;
      for (int i2 = i1 + 1; i2 < count; ++i2)
      {
        if (ownerElement.Attributes[i2].NamespaceURI == "http://www.w3.org/2000/xmlns/")
        {
          XmlAttribute attribute = ownerElement.Attributes[i2];
          if (!this.CheckNsNameAppearance(attribute.Name, attribute.Value))
          {
            this.NsNode = attribute;
            return true;
          }
        }
      }
      if (namespaceScope == XPathNamespaceScope.Local)
        return false;
      for (XmlElement parentNode = this.GetParentNode((XmlNode) ownerElement) as XmlElement; parentNode != null; parentNode = this.GetParentNode((XmlNode) parentNode) as XmlElement)
      {
        if (parentNode.HasAttributes)
        {
          for (int i3 = 0; i3 < parentNode.Attributes.Count; ++i3)
          {
            XmlAttribute attribute = parentNode.Attributes[i3];
            if (attribute.NamespaceURI == "http://www.w3.org/2000/xmlns/" && !this.CheckNsNameAppearance(attribute.Name, attribute.Value))
            {
              this.NsNode = attribute;
              return true;
            }
          }
        }
      }
      if (namespaceScope != XPathNamespaceScope.All || this.CheckNsNameAppearance(this.Document.NsNodeXml.Name, this.Document.NsNodeXml.Value))
        return false;
      this.NsNode = this.Document.NsNodeXml;
      return true;
    }

    public override bool MoveToParent()
    {
      if (this.NsNode != null)
      {
        this.NsNode = (XmlAttribute) null;
        return true;
      }
      if (this.node.NodeType == XmlNodeType.Attribute)
      {
        XmlElement ownerElement = ((XmlAttribute) this.node).OwnerElement;
        if (ownerElement == null)
          return false;
        this.node = (XmlNode) ownerElement;
        this.NsNode = (XmlAttribute) null;
        return true;
      }
      XmlNode parentNode = this.GetParentNode(this.node);
      if (parentNode == null)
        return false;
      this.node = parentNode;
      this.NsNode = (XmlAttribute) null;
      return true;
    }

    public override bool MoveToPrevious()
    {
      if (this.NsNode != null)
        return false;
      XmlNode previousSibling = this.GetPreviousSibling(this.node);
      if (previousSibling == null)
        return false;
      this.node = previousSibling;
      return true;
    }

    public override void MoveToRoot()
    {
      XmlNode n = !(this.node is XmlAttribute node) ? this.node : (XmlNode) node.OwnerElement;
      if (n == null)
        return;
      for (XmlNode parentNode = this.GetParentNode(n); parentNode != null; parentNode = this.GetParentNode(parentNode))
        n = parentNode;
      this.node = n;
      this.NsNode = (XmlAttribute) null;
    }

    private XmlNode Node => this.NsNode != null ? (XmlNode) this.NsNode : this.node;

    private XmlNode GetFirstChild(XmlNode n)
    {
      if (n.FirstChild == null)
        return (XmlNode) null;
      switch (n.FirstChild.NodeType)
      {
        case XmlNodeType.EntityReference:
          foreach (XmlNode childNode in n.ChildNodes)
          {
            if (childNode.NodeType != XmlNodeType.EntityReference)
              return childNode;
            XmlNode firstChild = this.GetFirstChild(childNode);
            if (firstChild != null)
              return firstChild;
          }
          return (XmlNode) null;
        case XmlNodeType.DocumentType:
        case XmlNodeType.XmlDeclaration:
          return this.GetNextSibling(n.FirstChild);
        default:
          return n.FirstChild;
      }
    }

    private XmlNode GetLastChild(XmlNode n)
    {
      if (n.LastChild == null)
        return (XmlNode) null;
      switch (n.LastChild.NodeType)
      {
        case XmlNodeType.EntityReference:
          for (XmlNode n1 = n.LastChild; n1 != null; n1 = n1.PreviousSibling)
          {
            if (n1.NodeType != XmlNodeType.EntityReference)
              return n1;
            XmlNode lastChild = this.GetLastChild(n1);
            if (lastChild != null)
              return lastChild;
          }
          return (XmlNode) null;
        case XmlNodeType.DocumentType:
        case XmlNodeType.XmlDeclaration:
          return this.GetPreviousSibling(n.LastChild);
        default:
          return n.LastChild;
      }
    }

    private XmlNode GetPreviousSibling(XmlNode n)
    {
      XmlNode previousSibling = n.PreviousSibling;
      if (previousSibling != null)
      {
        switch (previousSibling.NodeType)
        {
          case XmlNodeType.EntityReference:
            return this.GetLastChild(previousSibling) ?? this.GetPreviousSibling(previousSibling);
          case XmlNodeType.DocumentType:
          case XmlNodeType.XmlDeclaration:
            return this.GetPreviousSibling(previousSibling);
          default:
            return previousSibling;
        }
      }
      else
        return n.ParentNode == null || n.ParentNode.NodeType != XmlNodeType.EntityReference ? (XmlNode) null : this.GetPreviousSibling(n.ParentNode);
    }

    private XmlNode GetNextSibling(XmlNode n)
    {
      XmlNode nextSibling = n.NextSibling;
      if (nextSibling != null)
      {
        switch (nextSibling.NodeType)
        {
          case XmlNodeType.EntityReference:
            return this.GetFirstChild(nextSibling) ?? this.GetNextSibling(nextSibling);
          case XmlNodeType.DocumentType:
          case XmlNodeType.XmlDeclaration:
            return this.GetNextSibling(nextSibling);
          default:
            return n.NextSibling;
        }
      }
      else
        return n.ParentNode == null || n.ParentNode.NodeType != XmlNodeType.EntityReference ? (XmlNode) null : this.GetNextSibling(n.ParentNode);
    }

    private XmlNode GetParentNode(XmlNode n)
    {
      if (n.ParentNode == null)
        return (XmlNode) null;
      for (XmlNode parentNode = n.ParentNode; parentNode != null; parentNode = parentNode.ParentNode)
      {
        if (parentNode.NodeType != XmlNodeType.EntityReference)
          return parentNode;
      }
      return (XmlNode) null;
    }

    public override string LookupNamespace(string prefix) => base.LookupNamespace(prefix);

    public override string LookupPrefix(string namespaceUri) => base.LookupPrefix(namespaceUri);

    public override bool MoveToChild(XPathNodeType type) => base.MoveToChild(type);

    public override bool MoveToChild(string localName, string namespaceURI) => base.MoveToChild(localName, namespaceURI);

    public override bool MoveToNext(string localName, string namespaceURI) => base.MoveToNext(localName, namespaceURI);

    public override bool MoveToNext(XPathNodeType type) => base.MoveToNext(type);

    public override bool MoveToFollowing(string localName, string namespaceURI, XPathNavigator end) => base.MoveToFollowing(localName, namespaceURI, end);

    public override bool MoveToFollowing(XPathNodeType type, XPathNavigator end) => base.MoveToFollowing(type, end);
  }
}
