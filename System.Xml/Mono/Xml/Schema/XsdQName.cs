// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Schema.XsdQName
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System;
using System.Xml;
using System.Xml.Schema;

namespace Mono.Xml.Schema
{
  internal class XsdQName : XsdName
  {
    internal XsdQName()
    {
    }

    public override XmlTokenizedType TokenizedType => XmlTokenizedType.QName;

    public override XmlTypeCode TypeCode => XmlTypeCode.QName;

    public override Type ValueType => typeof (XmlQualifiedName);

    public override object ParseValue(
      string s,
      XmlNameTable nameTable,
      IXmlNamespaceResolver nsmgr)
    {
      if (nameTable == null)
        throw new ArgumentNullException("name table");
      XmlQualifiedName xmlQualifiedName = nsmgr != null ? XmlQualifiedName.Parse(s, nsmgr, true) : throw new ArgumentNullException("namespace manager");
      nameTable.Add(xmlQualifiedName.Name);
      nameTable.Add(xmlQualifiedName.Namespace);
      return (object) xmlQualifiedName;
    }

    internal override System.ValueType ParseValueType(
      string s,
      XmlNameTable nameTable,
      IXmlNamespaceResolver nsmgr)
    {
      return (System.ValueType) new QNameValueType(this.ParseValue(s, nameTable, nsmgr) as XmlQualifiedName);
    }
  }
}
