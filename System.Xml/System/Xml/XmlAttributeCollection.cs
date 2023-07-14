// Decompiled with JetBrains decompiler
// Type: System.Xml.XmlAttributeCollection
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using Mono.Xml;
using System.Collections;
using System.Runtime.CompilerServices;

namespace System.Xml
{
  public sealed class XmlAttributeCollection : XmlNamedNodeMap, IEnumerable, ICollection
  {
    private XmlElement ownerElement;
    private XmlDocument ownerDocument;

    internal XmlAttributeCollection(XmlNode parent)
      : base(parent)
    {
      this.ownerElement = parent as XmlElement;
      this.ownerDocument = parent.OwnerDocument;
      if (this.ownerElement == null)
        throw new XmlException("invalid construction for XmlAttributeCollection.");
    }

    bool ICollection.IsSynchronized => false;

    object ICollection.SyncRoot => (object) this;

    void ICollection.CopyTo(Array array, int index) => array.CopyTo(this.Nodes.ToArray(typeof (XmlAttribute)), index);

    private bool IsReadOnly => this.ownerElement.IsReadOnly;

    [IndexerName("ItemOf")]
    public XmlAttribute this[string name] => (XmlAttribute) this.GetNamedItem(name);

    [IndexerName("ItemOf")]
    public XmlAttribute this[int i] => (XmlAttribute) this.Nodes[i];

    [IndexerName("ItemOf")]
    public XmlAttribute this[string localName, string namespaceURI] => (XmlAttribute) this.GetNamedItem(localName, namespaceURI);

    public XmlAttribute Append(XmlAttribute node)
    {
      this.SetNamedItem((XmlNode) node);
      return node;
    }

    public void CopyTo(XmlAttribute[] array, int index)
    {
      for (int index1 = 0; index1 < this.Count; ++index1)
        array[index + index1] = this.Nodes[index1] as XmlAttribute;
    }

    public XmlAttribute InsertAfter(XmlAttribute newNode, XmlAttribute refNode)
    {
      if (refNode == null)
        return this.Count == 0 ? this.InsertBefore(newNode, (XmlAttribute) null) : this.InsertBefore(newNode, this[0]);
      for (int index = 0; index < this.Count; ++index)
      {
        if (refNode == this.Nodes[index])
          return this.InsertBefore(newNode, this.Count != index + 1 ? this[index + 1] : (XmlAttribute) null);
      }
      throw new ArgumentException("refNode not found in this collection.");
    }

    public XmlAttribute InsertBefore(XmlAttribute newNode, XmlAttribute refNode)
    {
      if (newNode.OwnerDocument != this.ownerDocument)
        throw new ArgumentException("different document created this newNode.");
      this.ownerDocument.onNodeInserting((XmlNode) newNode, (XmlNode) null);
      int pos = this.Count;
      if (refNode != null)
      {
        for (int index = 0; index < this.Count; ++index)
        {
          if (this.Nodes[index] as XmlNode == refNode)
          {
            pos = index;
            break;
          }
        }
        if (pos == this.Count)
          throw new ArgumentException("refNode not found in this collection.");
      }
      this.SetNamedItem((XmlNode) newNode, pos, false);
      this.ownerDocument.onNodeInserted((XmlNode) newNode, (XmlNode) null);
      return newNode;
    }

    public XmlAttribute Prepend(XmlAttribute node) => this.InsertAfter(node, (XmlAttribute) null);

    public XmlAttribute Remove(XmlAttribute node)
    {
      if (this.IsReadOnly)
        throw new ArgumentException("This attribute collection is read-only.");
      if (node == null)
        throw new ArgumentException("Specified node is null.");
      if (node.OwnerDocument != this.ownerDocument)
        throw new ArgumentException("Specified node is in a different document.");
      if (node.OwnerElement != this.ownerElement)
        throw new ArgumentException("The specified attribute is not contained in the element.");
      XmlAttribute existing = (XmlAttribute) null;
      for (int index = 0; index < this.Count; ++index)
      {
        XmlAttribute node1 = (XmlAttribute) this.Nodes[index];
        if (node1 == node)
        {
          existing = node1;
          break;
        }
      }
      if (existing != null)
      {
        this.ownerDocument.onNodeRemoving((XmlNode) node, (XmlNode) this.ownerElement);
        this.RemoveNamedItem(existing.LocalName, existing.NamespaceURI);
        this.RemoveIdenticalAttribute((XmlNode) existing);
        this.ownerDocument.onNodeRemoved((XmlNode) node, (XmlNode) this.ownerElement);
      }
      DTDAttributeDefinition attributeDefinition = existing.GetAttributeDefinition();
      if (attributeDefinition != null && attributeDefinition.DefaultValue != null)
      {
        XmlAttribute attribute = this.ownerDocument.CreateAttribute(existing.Prefix, existing.LocalName, existing.NamespaceURI, true, false);
        attribute.Value = attributeDefinition.DefaultValue;
        attribute.SetDefault();
        this.SetNamedItem((XmlNode) attribute);
      }
      existing.AttributeOwnerElement = (XmlElement) null;
      return existing;
    }

    public void RemoveAll()
    {
      int i = 0;
      while (i < this.Count)
      {
        XmlAttribute node = this[i];
        if (!node.Specified)
          ++i;
        this.Remove(node);
      }
    }

    public XmlAttribute RemoveAt(int i) => this.Count <= i ? (XmlAttribute) null : this.Remove((XmlAttribute) this.Nodes[i]);

    public override XmlNode SetNamedItem(XmlNode node)
    {
      if (this.IsReadOnly)
        throw new ArgumentException("this AttributeCollection is read only.");
      XmlAttribute xmlAttribute = node as XmlAttribute;
      if (xmlAttribute.OwnerElement == this.ownerElement)
        return node;
      if (xmlAttribute.OwnerElement != null)
        throw new ArgumentException("This attribute is already set to another element.");
      this.ownerElement.OwnerDocument.onNodeInserting(node, (XmlNode) this.ownerElement);
      xmlAttribute.AttributeOwnerElement = this.ownerElement;
      XmlNode xmlNode = this.SetNamedItem(node, -1, false);
      this.AdjustIdenticalAttributes(node as XmlAttribute, xmlNode != node ? xmlNode : (XmlNode) null);
      this.ownerElement.OwnerDocument.onNodeInserted(node, (XmlNode) this.ownerElement);
      return (XmlNode) (xmlNode as XmlAttribute);
    }

    internal void AddIdenticalAttribute() => this.SetIdenticalAttribute(false);

    internal void RemoveIdenticalAttribute() => this.SetIdenticalAttribute(true);

    private void SetIdenticalAttribute(bool remove)
    {
      if (this.ownerElement == null)
        return;
      XmlDocumentType documentType = this.ownerDocument.DocumentType;
      if (documentType == null || documentType.DTD == null)
        return;
      DTDElementDeclaration elementDecl = documentType.DTD.ElementDecls[this.ownerElement.Name];
      for (int index = 0; index < this.Count; ++index)
      {
        XmlAttribute node = (XmlAttribute) this.Nodes[index];
        DTDAttributeDefinition attribute = elementDecl?.Attributes[node.Name];
        if (attribute != null && attribute.Datatype.TokenizedType == XmlTokenizedType.ID)
        {
          if (remove)
          {
            if (this.ownerDocument.GetIdenticalAttribute(node.Value) != null)
            {
              this.ownerDocument.RemoveIdenticalAttribute(node.Value);
              break;
            }
          }
          else
          {
            if (this.ownerDocument.GetIdenticalAttribute(node.Value) != null)
              throw new XmlException(string.Format("ID value {0} already exists in this document.", (object) node.Value));
            this.ownerDocument.AddIdenticalAttribute(node);
            break;
          }
        }
      }
    }

    private void AdjustIdenticalAttributes(XmlAttribute node, XmlNode existing)
    {
      if (this.ownerElement == null)
        return;
      if (existing != null)
        this.RemoveIdenticalAttribute(existing);
      XmlDocumentType documentType = node.OwnerDocument.DocumentType;
      if (documentType == null || documentType.DTD == null)
        return;
      DTDAttributeDefinition attributeDefinition = documentType.DTD.AttListDecls[this.ownerElement.Name]?.Get(node.Name);
      if (attributeDefinition == null || attributeDefinition.Datatype.TokenizedType != XmlTokenizedType.ID)
        return;
      this.ownerDocument.AddIdenticalAttribute(node);
    }

    private XmlNode RemoveIdenticalAttribute(XmlNode existing)
    {
      if (this.ownerElement == null || existing == null || this.ownerDocument.GetIdenticalAttribute(existing.Value) == null)
        return existing;
      this.ownerDocument.RemoveIdenticalAttribute(existing.Value);
      return existing;
    }
  }
}
