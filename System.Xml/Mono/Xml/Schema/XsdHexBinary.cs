// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Schema.XsdHexBinary
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System;
using System.Xml;
using System.Xml.Schema;

namespace Mono.Xml.Schema
{
  internal class XsdHexBinary : XsdAnySimpleType
  {
    internal XsdHexBinary() => this.WhitespaceValue = XsdWhitespaceFacet.Collapse;

    internal override XmlSchemaFacet.Facet AllowedFacets => XsdAnySimpleType.stringAllowedFacets;

    public override XmlTypeCode TypeCode => XmlTypeCode.HexBinary;

    public override XmlTokenizedType TokenizedType => XmlTokenizedType.None;

    public override Type ValueType => typeof (byte[]);

    public override object ParseValue(
      string s,
      XmlNameTable nameTable,
      IXmlNamespaceResolver nsmgr)
    {
      return (object) XmlConvert.FromBinHexString(this.Normalize(s));
    }

    internal override int Length(string s) => s.Length / 2 + s.Length % 2;

    internal override System.ValueType ParseValueType(
      string s,
      XmlNameTable nameTable,
      IXmlNamespaceResolver nsmgr)
    {
      return (System.ValueType) new StringValueType(this.ParseValue(s, nameTable, nsmgr) as string);
    }
  }
}
