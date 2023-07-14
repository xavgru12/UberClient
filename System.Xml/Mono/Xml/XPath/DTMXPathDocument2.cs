// Decompiled with JetBrains decompiler
// Type: Mono.Xml.XPath.DTMXPathDocument2
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.Xml;
using System.Xml.XPath;

namespace Mono.Xml.XPath
{
  internal class DTMXPathDocument2 : IXPathNavigable
  {
    private readonly XPathNavigator root;
    internal readonly XmlNameTable NameTable;
    internal readonly DTMXPathLinkedNode2[] Nodes;
    internal readonly DTMXPathAttributeNode2[] Attributes;
    internal readonly DTMXPathNamespaceNode2[] Namespaces;
    internal readonly string[] AtomicStringPool;
    internal readonly string[] NonAtomicStringPool;
    internal readonly Hashtable IdTable;

    public DTMXPathDocument2(
      XmlNameTable nameTable,
      DTMXPathLinkedNode2[] nodes,
      DTMXPathAttributeNode2[] attributes,
      DTMXPathNamespaceNode2[] namespaces,
      string[] atomicStringPool,
      string[] nonAtomicStringPool,
      Hashtable idTable)
    {
      this.Nodes = nodes;
      this.Attributes = attributes;
      this.Namespaces = namespaces;
      this.AtomicStringPool = atomicStringPool;
      this.NonAtomicStringPool = nonAtomicStringPool;
      this.IdTable = idTable;
      this.NameTable = nameTable;
      this.root = (XPathNavigator) new DTMXPathNavigator2(this);
    }

    public XPathNavigator CreateNavigator() => this.root.Clone();
  }
}
