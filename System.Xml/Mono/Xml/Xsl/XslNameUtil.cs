// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.XslNameUtil
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System;
using System.Collections;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.Xsl
{
  internal class XslNameUtil
  {
    public static XmlQualifiedName[] FromListString(string names, XPathNavigator current)
    {
      string[] strArray = names.Split(XmlChar.WhitespaceChars);
      int length = 0;
      for (int index = 0; index < strArray.Length; ++index)
      {
        if (strArray[index] != string.Empty)
          ++length;
      }
      XmlQualifiedName[] xmlQualifiedNameArray = new XmlQualifiedName[length];
      int num = 0;
      for (int index = 0; index < strArray.Length; ++index)
      {
        if (strArray[index] != string.Empty)
          xmlQualifiedNameArray[num++] = XslNameUtil.FromString(strArray[index], current, true);
      }
      return xmlQualifiedNameArray;
    }

    public static XmlQualifiedName FromString(string name, XPathNavigator current) => XslNameUtil.FromString(name, current, false);

    public static XmlQualifiedName FromString(
      string name,
      XPathNavigator current,
      bool useDefaultXmlns)
    {
      if (current.NodeType == XPathNodeType.Attribute)
        (current = current.Clone()).MoveToParent();
      int length = name.IndexOf(':');
      if (length > 0)
        return new XmlQualifiedName(name.Substring(length + 1), current.GetNamespace(name.Substring(0, length)));
      if (length < 0)
        return new XmlQualifiedName(name, !useDefaultXmlns ? string.Empty : current.GetNamespace(string.Empty));
      throw new ArgumentException("Invalid name: " + name);
    }

    public static XmlQualifiedName FromString(string name, Hashtable nsDecls)
    {
      int length = name.IndexOf(':');
      if (length > 0)
        return new XmlQualifiedName(name.Substring(length + 1), nsDecls[(object) name.Substring(0, length)] as string);
      if (length < 0)
        return new XmlQualifiedName(name, !nsDecls.ContainsKey((object) string.Empty) ? string.Empty : (string) nsDecls[(object) string.Empty]);
      throw new ArgumentException("Invalid name: " + name);
    }

    public static XmlQualifiedName FromString(string name, IStaticXsltContext ctx)
    {
      int length = name.IndexOf(':');
      if (length > 0)
        return new XmlQualifiedName(name.Substring(length + 1), ctx.LookupNamespace(name.Substring(0, length)));
      if (length < 0)
        return new XmlQualifiedName(name, string.Empty);
      throw new ArgumentException("Invalid name: " + name);
    }

    public static XmlQualifiedName FromString(string name, XmlNamespaceManager ctx)
    {
      int length = name.IndexOf(':');
      if (length > 0)
        return new XmlQualifiedName(name.Substring(length + 1), ctx.LookupNamespace(name.Substring(0, length), false));
      if (length < 0)
        return new XmlQualifiedName(name, string.Empty);
      throw new ArgumentException("Invalid name: " + name);
    }

    public static string LocalNameOf(string name)
    {
      int num = name.IndexOf(':');
      if (num > 0)
        return name.Substring(num + 1);
      if (num < 0)
        return name;
      throw new ArgumentException("Invalid name: " + name);
    }
  }
}
