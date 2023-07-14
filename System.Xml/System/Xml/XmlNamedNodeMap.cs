// Decompiled with JetBrains decompiler
// Type: System.Xml.XmlNamedNodeMap
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using Mono.Xml;
using System.Collections;

namespace System.Xml
{
  public class XmlNamedNodeMap : IEnumerable
  {
    private static readonly IEnumerator emptyEnumerator = new XmlNode[0].GetEnumerator();
    private XmlNode parent;
    private ArrayList nodeList;
    private bool readOnly;

    internal XmlNamedNodeMap(XmlNode parent) => this.parent = parent;

    private ArrayList NodeList
    {
      get
      {
        if (this.nodeList == null)
          this.nodeList = new ArrayList();
        return this.nodeList;
      }
    }

    public virtual int Count => this.nodeList == null ? 0 : this.nodeList.Count;

    public virtual IEnumerator GetEnumerator() => this.nodeList == null ? XmlNamedNodeMap.emptyEnumerator : this.nodeList.GetEnumerator();

    public virtual XmlNode GetNamedItem(string name)
    {
      if (this.nodeList == null)
        return (XmlNode) null;
      for (int index = 0; index < this.nodeList.Count; ++index)
      {
        XmlNode node = (XmlNode) this.nodeList[index];
        if (node.Name == name)
          return node;
      }
      return (XmlNode) null;
    }

    public virtual XmlNode GetNamedItem(string localName, string namespaceURI)
    {
      if (this.nodeList == null)
        return (XmlNode) null;
      for (int index = 0; index < this.nodeList.Count; ++index)
      {
        XmlNode node = (XmlNode) this.nodeList[index];
        if (node.LocalName == localName && node.NamespaceURI == namespaceURI)
          return node;
      }
      return (XmlNode) null;
    }

    public virtual XmlNode Item(int index) => this.nodeList == null || index < 0 || index >= this.nodeList.Count ? (XmlNode) null : (XmlNode) this.nodeList[index];

    public virtual XmlNode RemoveNamedItem(string name)
    {
      if (this.nodeList == null)
        return (XmlNode) null;
      for (int index = 0; index < this.nodeList.Count; ++index)
      {
        XmlNode node = (XmlNode) this.nodeList[index];
        if (node.Name == name)
        {
          if (node.IsReadOnly)
            throw new InvalidOperationException("Cannot remove. This node is read only: " + name);
          this.nodeList.Remove((object) node);
          if (node is XmlAttribute xmlAttribute)
          {
            DTDAttributeDefinition attributeDefinition = xmlAttribute.GetAttributeDefinition();
            if (attributeDefinition != null && attributeDefinition.DefaultValue != null)
            {
              XmlAttribute attribute = xmlAttribute.OwnerDocument.CreateAttribute(xmlAttribute.Prefix, xmlAttribute.LocalName, xmlAttribute.NamespaceURI, true, false);
              attribute.Value = attributeDefinition.DefaultValue;
              attribute.SetDefault();
              xmlAttribute.OwnerElement.SetAttributeNode(attribute);
            }
          }
          return node;
        }
      }
      return (XmlNode) null;
    }

    public virtual XmlNode RemoveNamedItem(string localName, string namespaceURI)
    {
      if (this.nodeList == null)
        return (XmlNode) null;
      for (int index = 0; index < this.nodeList.Count; ++index)
      {
        XmlNode node = (XmlNode) this.nodeList[index];
        if (node.LocalName == localName && node.NamespaceURI == namespaceURI)
        {
          this.nodeList.Remove((object) node);
          return node;
        }
      }
      return (XmlNode) null;
    }

    public virtual XmlNode SetNamedItem(XmlNode node) => this.SetNamedItem(node, -1, true);

    internal XmlNode SetNamedItem(XmlNode node, bool raiseEvent) => this.SetNamedItem(node, -1, raiseEvent);

    internal XmlNode SetNamedItem(XmlNode node, int pos, bool raiseEvent)
    {
      if (this.readOnly || node.OwnerDocument != this.parent.OwnerDocument)
        throw new ArgumentException("Cannot add to NodeMap.");
      if (raiseEvent)
        this.parent.OwnerDocument.onNodeInserting(node, this.parent);
      try
      {
        for (int index = 0; index < this.NodeList.Count; ++index)
        {
          XmlNode node1 = (XmlNode) this.nodeList[index];
          if (node1.LocalName == node.LocalName && node1.NamespaceURI == node.NamespaceURI)
          {
            this.nodeList.Remove((object) node1);
            if (pos < 0)
              this.nodeList.Add((object) node);
            else
              this.nodeList.Insert(pos, (object) node);
            return node1;
          }
        }
        if (pos < 0)
          this.nodeList.Add((object) node);
        else
          this.nodeList.Insert(pos, (object) node);
        return node;
      }
      finally
      {
        if (raiseEvent)
          this.parent.OwnerDocument.onNodeInserted(node, this.parent);
      }
    }

    internal ArrayList Nodes => this.NodeList;
  }
}
