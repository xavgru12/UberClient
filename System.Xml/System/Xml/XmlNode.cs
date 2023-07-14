// Decompiled with JetBrains decompiler
// Type: System.Xml.XmlNode
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml.Schema;
using System.Xml.XPath;

namespace System.Xml
{
  public abstract class XmlNode : IEnumerable, ICloneable, IXPathNavigable
  {
    private static XmlNode.EmptyNodeList emptyList = new XmlNode.EmptyNodeList();
    private XmlDocument ownerDocument;
    private XmlNode parentNode;
    private XmlNodeListChildren childNodes;

    internal XmlNode(XmlDocument ownerDocument) => this.ownerDocument = ownerDocument;

    object ICloneable.Clone() => (object) this.Clone();

    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

    public virtual XmlAttributeCollection Attributes => (XmlAttributeCollection) null;

    public virtual string BaseURI => this.ParentNode != null ? this.ParentNode.ChildrenBaseURI : string.Empty;

    internal virtual string ChildrenBaseURI => this.BaseURI;

    public virtual XmlNodeList ChildNodes
    {
      get
      {
        if (!(this is IHasXmlChildNode parent))
          return (XmlNodeList) XmlNode.emptyList;
        if (this.childNodes == null)
          this.childNodes = new XmlNodeListChildren(parent);
        return (XmlNodeList) this.childNodes;
      }
    }

    public virtual XmlNode FirstChild
    {
      get
      {
        XmlLinkedNode lastLinkedChild = this is IHasXmlChildNode hasXmlChildNode ? hasXmlChildNode.LastLinkedChild : (XmlLinkedNode) null;
        return lastLinkedChild == null ? (XmlNode) null : (XmlNode) lastLinkedChild.NextLinkedSibling;
      }
    }

    public virtual bool HasChildNodes => this.LastChild != null;

    public virtual string InnerText
    {
      get
      {
        switch (this.NodeType)
        {
          case XmlNodeType.Text:
          case XmlNodeType.CDATA:
          case XmlNodeType.Whitespace:
          case XmlNodeType.SignificantWhitespace:
            return this.Value;
          default:
            if (this.FirstChild == null)
              return string.Empty;
            if (this.FirstChild == this.LastChild)
              return this.FirstChild.NodeType != XmlNodeType.Comment ? this.FirstChild.InnerText : string.Empty;
            StringBuilder builder = (StringBuilder) null;
            this.AppendChildValues(ref builder);
            return builder == null ? string.Empty : builder.ToString();
        }
      }
      set
      {
        if (!(this is XmlDocumentFragment))
          throw new InvalidOperationException("This node is read only. Cannot be modified.");
        this.RemoveAll();
        this.AppendChild((XmlNode) this.OwnerDocument.CreateTextNode(value));
      }
    }

    private void AppendChildValues(ref StringBuilder builder)
    {
      for (XmlNode xmlNode = this.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
      {
        switch (xmlNode.NodeType)
        {
          case XmlNodeType.Text:
          case XmlNodeType.CDATA:
          case XmlNodeType.Whitespace:
          case XmlNodeType.SignificantWhitespace:
            if (builder == null)
              builder = new StringBuilder();
            builder.Append(xmlNode.Value);
            break;
        }
        xmlNode.AppendChildValues(ref builder);
      }
    }

    public virtual string InnerXml
    {
      get
      {
        StringWriter writer = new StringWriter();
        this.WriteContentTo((XmlWriter) new XmlTextWriter((TextWriter) writer));
        return writer.GetStringBuilder().ToString();
      }
      set => throw new InvalidOperationException("This node is readonly or doesn't have any children.");
    }

    public virtual bool IsReadOnly
    {
      get
      {
        XmlNode xmlNode = this;
        do
        {
          switch (xmlNode.NodeType)
          {
            case XmlNodeType.Attribute:
              xmlNode = (XmlNode) ((XmlAttribute) xmlNode).OwnerElement;
              break;
            case XmlNodeType.EntityReference:
            case XmlNodeType.Entity:
              return true;
            default:
              xmlNode = xmlNode.ParentNode;
              break;
          }
        }
        while (xmlNode != null);
        return false;
      }
    }

    public virtual XmlElement this[string name]
    {
      get
      {
        for (int i = 0; i < this.ChildNodes.Count; ++i)
        {
          XmlNode childNode = this.ChildNodes[i];
          if (childNode.NodeType == XmlNodeType.Element && childNode.Name == name)
            return (XmlElement) childNode;
        }
        return (XmlElement) null;
      }
    }

    public virtual XmlElement this[string localname, string ns]
    {
      get
      {
        for (int i = 0; i < this.ChildNodes.Count; ++i)
        {
          XmlNode childNode = this.ChildNodes[i];
          if (childNode.NodeType == XmlNodeType.Element && childNode.LocalName == localname && childNode.NamespaceURI == ns)
            return (XmlElement) childNode;
        }
        return (XmlElement) null;
      }
    }

    public virtual XmlNode LastChild => !(this is IHasXmlChildNode hasXmlChildNode) ? (XmlNode) null : (XmlNode) hasXmlChildNode.LastLinkedChild;

    public abstract string LocalName { get; }

    public abstract string Name { get; }

    public virtual string NamespaceURI => string.Empty;

    public virtual XmlNode NextSibling => (XmlNode) null;

    public abstract XmlNodeType NodeType { get; }

    internal virtual XPathNodeType XPathNodeType => throw new InvalidOperationException("Can not get XPath node type from " + this.GetType().ToString());

    public virtual string OuterXml
    {
      get
      {
        StringWriter writer = new StringWriter();
        this.WriteTo((XmlWriter) new XmlTextWriter((TextWriter) writer));
        return writer.ToString();
      }
    }

    public virtual XmlDocument OwnerDocument => this.ownerDocument;

    public virtual XmlNode ParentNode => this.parentNode;

    public virtual string Prefix
    {
      get => string.Empty;
      set
      {
      }
    }

    public virtual XmlNode PreviousSibling => (XmlNode) null;

    public virtual string Value
    {
      get => (string) null;
      set => throw new InvalidOperationException("This node does not have a value");
    }

    internal virtual string XmlLang
    {
      get
      {
        if (this.Attributes != null)
        {
          for (int i = 0; i < this.Attributes.Count; ++i)
          {
            XmlAttribute attribute = this.Attributes[i];
            if (attribute.Name == "xml:lang")
              return attribute.Value;
          }
        }
        return this.ParentNode != null ? this.ParentNode.XmlLang : this.OwnerDocument.XmlLang;
      }
    }

    internal virtual XmlSpace XmlSpace
    {
      get
      {
        if (this.Attributes != null)
        {
          for (int i = 0; i < this.Attributes.Count; ++i)
          {
            XmlAttribute attribute = this.Attributes[i];
            if (attribute.Name == "xml:space")
            {
              string key = attribute.Value;
              if (key != null)
              {
                if (XmlNode.\u003C\u003Ef__switch\u0024map44 == null)
                  XmlNode.\u003C\u003Ef__switch\u0024map44 = new Dictionary<string, int>(2)
                  {
                    {
                      "preserve",
                      0
                    },
                    {
                      "default",
                      1
                    }
                  };
                int num;
                if (XmlNode.\u003C\u003Ef__switch\u0024map44.TryGetValue(key, out num))
                {
                  if (num == 0)
                    return XmlSpace.Preserve;
                  if (num == 1)
                    return XmlSpace.Default;
                  break;
                }
                break;
              }
              break;
            }
          }
        }
        return this.ParentNode != null ? this.ParentNode.XmlSpace : this.OwnerDocument.XmlSpace;
      }
    }

    public virtual IXmlSchemaInfo SchemaInfo
    {
      get => (IXmlSchemaInfo) null;
      internal set
      {
      }
    }

    public virtual XmlNode AppendChild(XmlNode newChild) => this.InsertBefore(newChild, (XmlNode) null);

    internal XmlNode AppendChild(XmlNode newChild, bool checkNodeType) => this.InsertBefore(newChild, (XmlNode) null, checkNodeType, true);

    public virtual XmlNode Clone() => this.CloneNode(true);

    public abstract XmlNode CloneNode(bool deep);

    public virtual XPathNavigator CreateNavigator() => this.OwnerDocument.CreateNavigator(this);

    public IEnumerator GetEnumerator() => this.ChildNodes.GetEnumerator();

    public virtual string GetNamespaceOfPrefix(string prefix)
    {
      string key = prefix;
      if (key == null)
        throw new ArgumentNullException(nameof (prefix));
      // ISSUE: reference to a compiler-generated field
      if (XmlNode.\u003C\u003Ef__switch\u0024map45 == null)
      {
        // ISSUE: reference to a compiler-generated field
        XmlNode.\u003C\u003Ef__switch\u0024map45 = new Dictionary<string, int>(2)
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
      if (XmlNode.\u003C\u003Ef__switch\u0024map45.TryGetValue(key, out num))
      {
        if (num == 0)
          return "http://www.w3.org/XML/1998/namespace";
        if (num == 1)
          return "http://www.w3.org/2000/xmlns/";
      }
      XmlNode xmlNode;
      switch (this.NodeType)
      {
        case XmlNodeType.Element:
          xmlNode = this;
          break;
        case XmlNodeType.Attribute:
          xmlNode = (XmlNode) ((XmlAttribute) this).OwnerElement;
          if (xmlNode == null)
            return string.Empty;
          break;
        default:
          xmlNode = this.ParentNode;
          break;
      }
      for (; xmlNode != null; xmlNode = xmlNode.ParentNode)
      {
        if (xmlNode.Prefix == prefix)
          return xmlNode.NamespaceURI;
        if (xmlNode.NodeType == XmlNodeType.Element && ((XmlElement) xmlNode).HasAttributes)
        {
          int count = xmlNode.Attributes.Count;
          for (int i = 0; i < count; ++i)
          {
            XmlAttribute attribute = xmlNode.Attributes[i];
            if (prefix == attribute.LocalName && attribute.Prefix == "xmlns" || attribute.Name == "xmlns" && prefix == string.Empty)
              return attribute.Value;
          }
        }
      }
      return string.Empty;
    }

    public virtual string GetPrefixOfNamespace(string namespaceURI)
    {
      string key = namespaceURI;
      if (key != null)
      {
        // ISSUE: reference to a compiler-generated field
        if (XmlNode.\u003C\u003Ef__switch\u0024map46 == null)
        {
          // ISSUE: reference to a compiler-generated field
          XmlNode.\u003C\u003Ef__switch\u0024map46 = new Dictionary<string, int>(2)
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
        if (XmlNode.\u003C\u003Ef__switch\u0024map46.TryGetValue(key, out num))
        {
          if (num == 0)
            return "xml";
          if (num == 1)
            return "xmlns";
        }
      }
      XmlNode xmlNode;
      switch (this.NodeType)
      {
        case XmlNodeType.Element:
          xmlNode = this;
          break;
        case XmlNodeType.Attribute:
          xmlNode = (XmlNode) ((XmlAttribute) this).OwnerElement;
          break;
        default:
          xmlNode = this.ParentNode;
          break;
      }
      for (; xmlNode != null; xmlNode = xmlNode.ParentNode)
      {
        if (xmlNode.NodeType == XmlNodeType.Element && ((XmlElement) xmlNode).HasAttributes)
        {
          for (int i = 0; i < xmlNode.Attributes.Count; ++i)
          {
            XmlAttribute attribute = xmlNode.Attributes[i];
            if (attribute.Prefix == "xmlns" && attribute.Value == namespaceURI)
              return attribute.LocalName;
            if (attribute.Name == "xmlns" && attribute.Value == namespaceURI)
              return string.Empty;
          }
        }
      }
      return string.Empty;
    }

    public virtual XmlNode InsertAfter(XmlNode newChild, XmlNode refChild)
    {
      XmlNode refChild1 = (XmlNode) null;
      if (refChild != null)
        refChild1 = refChild.NextSibling;
      else if (this.FirstChild != null)
        refChild1 = this.FirstChild;
      return this.InsertBefore(newChild, refChild1);
    }

    public virtual XmlNode InsertBefore(XmlNode newChild, XmlNode refChild) => this.InsertBefore(newChild, refChild, true, true);

    internal bool IsAncestor(XmlNode newChild)
    {
      for (XmlNode parentNode = this.ParentNode; parentNode != null; parentNode = parentNode.ParentNode)
      {
        if (parentNode == newChild)
          return true;
      }
      return false;
    }

    internal XmlNode InsertBefore(
      XmlNode newChild,
      XmlNode refChild,
      bool checkNodeType,
      bool raiseEvent)
    {
      if (checkNodeType)
        this.CheckNodeInsertion(newChild, refChild);
      if (newChild == refChild)
        return newChild;
      IHasXmlChildNode hasXmlChildNode = (IHasXmlChildNode) this;
      XmlDocument xmlDocument = this.NodeType != XmlNodeType.Document ? this.OwnerDocument : (XmlDocument) this;
      if (raiseEvent)
        xmlDocument.onNodeInserting(newChild, this);
      if (newChild.ParentNode != null)
        newChild.ParentNode.RemoveChild(newChild, checkNodeType);
      if (newChild.NodeType == XmlNodeType.DocumentFragment)
      {
        while (newChild.FirstChild != null)
          this.InsertBefore(newChild.FirstChild, refChild);
      }
      else
      {
        XmlLinkedNode xmlLinkedNode = (XmlLinkedNode) newChild;
        xmlLinkedNode.parentNode = this;
        if (refChild == null)
        {
          if (hasXmlChildNode.LastLinkedChild != null)
          {
            XmlLinkedNode firstChild = (XmlLinkedNode) this.FirstChild;
            hasXmlChildNode.LastLinkedChild.NextLinkedSibling = xmlLinkedNode;
            hasXmlChildNode.LastLinkedChild = xmlLinkedNode;
            xmlLinkedNode.NextLinkedSibling = firstChild;
          }
          else
          {
            hasXmlChildNode.LastLinkedChild = xmlLinkedNode;
            hasXmlChildNode.LastLinkedChild.NextLinkedSibling = xmlLinkedNode;
          }
        }
        else
        {
          if (!(refChild.PreviousSibling is XmlLinkedNode previousSibling))
            hasXmlChildNode.LastLinkedChild.NextLinkedSibling = xmlLinkedNode;
          else
            previousSibling.NextLinkedSibling = xmlLinkedNode;
          xmlLinkedNode.NextLinkedSibling = refChild as XmlLinkedNode;
        }
        switch (newChild.NodeType)
        {
          case XmlNodeType.EntityReference:
            ((XmlEntityReference) newChild).SetReferencedEntityContent();
            break;
        }
        if (raiseEvent)
          xmlDocument.onNodeInserted(newChild, newChild.ParentNode);
      }
      return newChild;
    }

    private void CheckNodeInsertion(XmlNode newChild, XmlNode refChild)
    {
      XmlDocument xmlDocument = this.NodeType != XmlNodeType.Document ? this.OwnerDocument : (XmlDocument) this;
      if (this.NodeType != XmlNodeType.Element && this.NodeType != XmlNodeType.Attribute && this.NodeType != XmlNodeType.Document && this.NodeType != XmlNodeType.DocumentFragment)
        throw new InvalidOperationException(string.Format("Node cannot be appended to current node {0}.", (object) this.NodeType));
      switch (this.NodeType)
      {
        case XmlNodeType.Element:
          XmlNodeType nodeType = newChild.NodeType;
          switch (nodeType)
          {
            case XmlNodeType.Entity:
            case XmlNodeType.Document:
            case XmlNodeType.DocumentType:
            case XmlNodeType.Notation:
              throw new InvalidOperationException("Cannot insert specified type of node as a child of this node.");
            default:
              if (nodeType == XmlNodeType.Attribute || nodeType == XmlNodeType.XmlDeclaration)
                goto case XmlNodeType.Entity;
              else
                break;
          }
          break;
        case XmlNodeType.Attribute:
          switch (newChild.NodeType)
          {
            case XmlNodeType.Text:
            case XmlNodeType.EntityReference:
              break;
            default:
              throw new InvalidOperationException(string.Format("Cannot insert specified type of node {0} as a child of this node {1}.", (object) newChild.NodeType, (object) this.NodeType));
          }
          break;
      }
      if (this.IsReadOnly)
        throw new InvalidOperationException("The node is readonly.");
      if (newChild.OwnerDocument != xmlDocument)
        throw new ArgumentException("Can't append a node created by another document.");
      if (refChild != null && refChild.ParentNode != this)
        throw new ArgumentException("The reference node is not a child of this node.");
      if (this == xmlDocument && xmlDocument.DocumentElement != null && newChild is XmlElement && newChild != xmlDocument.DocumentElement)
        throw new XmlException("multiple document element not allowed.");
      if (newChild == this || this.IsAncestor(newChild))
        throw new ArgumentException("Cannot insert a node or any ancestor of that node as a child of itself.");
    }

    public virtual void Normalize()
    {
      StringBuilder tmpBuilder = new StringBuilder();
      int count = this.ChildNodes.Count;
      int start = 0;
      for (int i = 0; i < count; ++i)
      {
        XmlNode childNode = this.ChildNodes[i];
        switch (childNode.NodeType)
        {
          case XmlNodeType.Text:
          case XmlNodeType.Whitespace:
          case XmlNodeType.SignificantWhitespace:
            tmpBuilder.Append(childNode.Value);
            break;
          default:
            childNode.Normalize();
            this.NormalizeRange(start, i, tmpBuilder);
            start = i + 1;
            break;
        }
      }
      if (start >= count)
        return;
      this.NormalizeRange(start, count, tmpBuilder);
    }

    private void NormalizeRange(int start, int i, StringBuilder tmpBuilder)
    {
      int num1 = -1;
      for (int i1 = start; i1 < i; ++i1)
      {
        XmlNode childNode = this.ChildNodes[i1];
        if (childNode.NodeType == XmlNodeType.Text)
        {
          num1 = i1;
          break;
        }
        if (childNode.NodeType == XmlNodeType.SignificantWhitespace)
          num1 = i1;
      }
      if (num1 >= 0)
      {
        for (int index = start; index < num1; ++index)
          this.RemoveChild(this.ChildNodes[start]);
        int num2 = i - num1 - 1;
        for (int index = 0; index < num2; ++index)
          this.RemoveChild(this.ChildNodes[start + 1]);
      }
      if (num1 >= 0)
        this.ChildNodes[start].Value = tmpBuilder.ToString();
      tmpBuilder.Length = 0;
    }

    public virtual XmlNode PrependChild(XmlNode newChild) => this.InsertAfter(newChild, (XmlNode) null);

    public virtual void RemoveAll()
    {
      if (this.Attributes != null)
        this.Attributes.RemoveAll();
      XmlNode nextSibling;
      for (XmlNode oldChild = this.FirstChild; oldChild != null; oldChild = nextSibling)
      {
        nextSibling = oldChild.NextSibling;
        this.RemoveChild(oldChild);
      }
    }

    public virtual XmlNode RemoveChild(XmlNode oldChild) => this.RemoveChild(oldChild, true);

    private void CheckNodeRemoval()
    {
      if (this.NodeType != XmlNodeType.Attribute && this.NodeType != XmlNodeType.Element && this.NodeType != XmlNodeType.Document && this.NodeType != XmlNodeType.DocumentFragment)
        throw new ArgumentException(string.Format("This {0} node cannot remove its child.", (object) this.NodeType));
      if (this.IsReadOnly)
        throw new ArgumentException(string.Format("This {0} node is read only.", (object) this.NodeType));
    }

    internal XmlNode RemoveChild(XmlNode oldChild, bool checkNodeType)
    {
      if (oldChild == null)
        throw new NullReferenceException();
      XmlDocument xmlDocument = this.NodeType != XmlNodeType.Document ? this.OwnerDocument : (XmlDocument) this;
      if (oldChild.ParentNode != this)
        throw new ArgumentException("The node to be removed is not a child of this node.");
      if (checkNodeType)
        xmlDocument.onNodeRemoving(oldChild, oldChild.ParentNode);
      if (checkNodeType)
        this.CheckNodeRemoval();
      IHasXmlChildNode hasXmlChildNode = (IHasXmlChildNode) this;
      if (object.ReferenceEquals((object) hasXmlChildNode.LastLinkedChild, (object) hasXmlChildNode.LastLinkedChild.NextLinkedSibling) && object.ReferenceEquals((object) hasXmlChildNode.LastLinkedChild, (object) oldChild))
      {
        hasXmlChildNode.LastLinkedChild = (XmlLinkedNode) null;
      }
      else
      {
        XmlLinkedNode objB = (XmlLinkedNode) oldChild;
        XmlLinkedNode xmlLinkedNode = hasXmlChildNode.LastLinkedChild;
        XmlLinkedNode firstChild = (XmlLinkedNode) this.FirstChild;
        while (!object.ReferenceEquals((object) xmlLinkedNode.NextLinkedSibling, (object) hasXmlChildNode.LastLinkedChild) && !object.ReferenceEquals((object) xmlLinkedNode.NextLinkedSibling, (object) objB))
          xmlLinkedNode = xmlLinkedNode.NextLinkedSibling;
        if (!object.ReferenceEquals((object) xmlLinkedNode.NextLinkedSibling, (object) objB))
          throw new ArgumentException();
        xmlLinkedNode.NextLinkedSibling = objB.NextLinkedSibling;
        if (objB.NextLinkedSibling == firstChild)
          hasXmlChildNode.LastLinkedChild = xmlLinkedNode;
        objB.NextLinkedSibling = (XmlLinkedNode) null;
      }
      if (checkNodeType)
        xmlDocument.onNodeRemoved(oldChild, oldChild.ParentNode);
      oldChild.parentNode = (XmlNode) null;
      return oldChild;
    }

    public virtual XmlNode ReplaceChild(XmlNode newChild, XmlNode oldChild)
    {
      if (oldChild.ParentNode != this)
        throw new ArgumentException("The node to be removed is not a child of this node.");
      if (newChild == this || this.IsAncestor(newChild))
        throw new InvalidOperationException("Cannot insert a node or any ancestor of that node as a child of itself.");
      XmlNode nextSibling = oldChild.NextSibling;
      this.RemoveChild(oldChild);
      this.InsertBefore(newChild, nextSibling);
      return oldChild;
    }

    internal XmlElement AttributeOwnerElement
    {
      get => (XmlElement) this.parentNode;
      set => this.parentNode = (XmlNode) value;
    }

    internal void SearchDescendantElements(string name, bool matchAll, ArrayList list)
    {
      for (XmlNode xmlNode = this.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
      {
        if (xmlNode.NodeType == XmlNodeType.Element)
        {
          if (matchAll || xmlNode.Name == name)
            list.Add((object) xmlNode);
          xmlNode.SearchDescendantElements(name, matchAll, list);
        }
      }
    }

    internal void SearchDescendantElements(
      string name,
      bool matchAllName,
      string ns,
      bool matchAllNS,
      ArrayList list)
    {
      for (XmlNode xmlNode = this.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
      {
        if (xmlNode.NodeType == XmlNodeType.Element)
        {
          if ((matchAllName || xmlNode.LocalName == name) && (matchAllNS || xmlNode.NamespaceURI == ns))
            list.Add((object) xmlNode);
          xmlNode.SearchDescendantElements(name, matchAllName, ns, matchAllNS, list);
        }
      }
    }

    public XmlNodeList SelectNodes(string xpath) => this.SelectNodes(xpath, (XmlNamespaceManager) null);

    public XmlNodeList SelectNodes(string xpath, XmlNamespaceManager nsmgr)
    {
      XPathNavigator navigator = this.CreateNavigator();
      XPathExpression expr = navigator.Compile(xpath);
      if (nsmgr != null)
        expr.SetContext(nsmgr);
      return (XmlNodeList) new XmlIteratorNodeList(navigator.Select(expr));
    }

    public XmlNode SelectSingleNode(string xpath) => this.SelectSingleNode(xpath, (XmlNamespaceManager) null);

    public XmlNode SelectSingleNode(string xpath, XmlNamespaceManager nsmgr)
    {
      XPathNavigator navigator = this.CreateNavigator();
      XPathExpression expr = navigator.Compile(xpath);
      if (nsmgr != null)
        expr.SetContext(nsmgr);
      XPathNodeIterator xpathNodeIterator = navigator.Select(expr);
      return !xpathNodeIterator.MoveNext() ? (XmlNode) null : ((IHasXmlNode) xpathNodeIterator.Current).GetNode();
    }

    public virtual bool Supports(string feature, string version) => string.Compare(feature, "xml", true, CultureInfo.InvariantCulture) == 0 && (string.Compare(version, "1.0", true, CultureInfo.InvariantCulture) == 0 || string.Compare(version, "2.0", true, CultureInfo.InvariantCulture) == 0);

    public abstract void WriteContentTo(XmlWriter w);

    public abstract void WriteTo(XmlWriter w);

    internal XmlNamespaceManager ConstructNamespaceManager()
    {
      XmlNamespaceManager namespaceManager = new XmlNamespaceManager((!(this is XmlDocument) ? this.OwnerDocument : (XmlDocument) this).NameTable);
      XmlElement xmlElement;
      switch (this.NodeType)
      {
        case XmlNodeType.Element:
          xmlElement = this as XmlElement;
          break;
        case XmlNodeType.Attribute:
          xmlElement = ((XmlAttribute) this).OwnerElement;
          break;
        default:
          xmlElement = this.ParentNode as XmlElement;
          break;
      }
      for (; xmlElement != null; xmlElement = xmlElement.ParentNode as XmlElement)
      {
        for (int i = 0; i < xmlElement.Attributes.Count; ++i)
        {
          XmlAttribute attribute = xmlElement.Attributes[i];
          if (attribute.Prefix == "xmlns")
          {
            if (namespaceManager.LookupNamespace(attribute.LocalName) != attribute.Value)
              namespaceManager.AddNamespace(attribute.LocalName, attribute.Value);
          }
          else if (attribute.Name == "xmlns" && namespaceManager.LookupNamespace(string.Empty) != attribute.Value)
            namespaceManager.AddNamespace(string.Empty, attribute.Value);
        }
      }
      return namespaceManager;
    }

    private class EmptyNodeList : XmlNodeList
    {
      private static IEnumerator emptyEnumerator = new object[0].GetEnumerator();

      public override int Count => 0;

      public override IEnumerator GetEnumerator() => XmlNode.EmptyNodeList.emptyEnumerator;

      public override XmlNode Item(int index) => (XmlNode) null;
    }
  }
}
