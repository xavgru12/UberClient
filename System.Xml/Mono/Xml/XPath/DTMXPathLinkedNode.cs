// Decompiled with JetBrains decompiler
// Type: Mono.Xml.XPath.DTMXPathLinkedNode
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Xml.XPath;

namespace Mono.Xml.XPath
{
  internal struct DTMXPathLinkedNode
  {
    public int FirstChild;
    public int Parent;
    public int PreviousSibling;
    public int NextSibling;
    public int FirstAttribute;
    public int FirstNamespace;
    public XPathNodeType NodeType;
    public string BaseURI;
    public bool IsEmptyElement;
    public string LocalName;
    public string NamespaceURI;
    public string Prefix;
    public string Value;
    public string XmlLang;
    public int LineNumber;
    public int LinePosition;
  }
}
