// Decompiled with JetBrains decompiler
// Type: Mono.Xml.XPath.DTMXPathDocument
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.Xml;
using System.Xml.XPath;

namespace Mono.Xml.XPath
{
  internal class DTMXPathDocument : IXPathNavigable
  {
    private XPathNavigator root;

    public DTMXPathDocument(
      XmlNameTable nameTable,
      DTMXPathLinkedNode[] nodes,
      DTMXPathAttributeNode[] attributes,
      DTMXPathNamespaceNode[] namespaces,
      Hashtable idTable)
    {
      this.root = (XPathNavigator) new DTMXPathNavigator(this, nameTable, nodes, attributes, namespaces, idTable);
    }

    public XPathNavigator CreateNavigator() => this.root.Clone();
  }
}
