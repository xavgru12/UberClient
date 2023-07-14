// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Schema.XsdName
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System;
using System.Xml;
using System.Xml.Schema;

namespace Mono.Xml.Schema
{
  internal class XsdName : XsdToken
  {
    internal XsdName()
    {
    }

    public override XmlTokenizedType TokenizedType => XmlTokenizedType.CDATA;

    public override XmlTypeCode TypeCode => XmlTypeCode.Name;

    public override Type ValueType => typeof (string);

    public override object ParseValue(
      string s,
      XmlNameTable nameTable,
      IXmlNamespaceResolver nsmgr)
    {
      return XmlChar.IsName(s) ? (object) s : throw new ArgumentException("'" + s + "' is an invalid name.");
    }

    internal override System.ValueType ParseValueType(
      string s,
      XmlNameTable nameTable,
      IXmlNamespaceResolver nsmgr)
    {
      return (System.ValueType) new StringValueType(this.ParseValue(s, nameTable, nsmgr) as string);
    }
  }
}
