// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Schema.XsdBoolean
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System;
using System.Xml;
using System.Xml.Schema;

namespace Mono.Xml.Schema
{
  internal class XsdBoolean : XsdAnySimpleType
  {
    internal XsdBoolean() => this.WhitespaceValue = XsdWhitespaceFacet.Collapse;

    internal override XmlSchemaFacet.Facet AllowedFacets => XsdAnySimpleType.booleanAllowedFacets;

    public override XmlTokenizedType TokenizedType => XmlSchemaUtil.StrictMsCompliant ? XmlTokenizedType.None : XmlTokenizedType.CDATA;

    public override XmlTypeCode TypeCode => XmlTypeCode.Boolean;

    public override Type ValueType => typeof (bool);

    public override object ParseValue(
      string s,
      XmlNameTable nameTable,
      IXmlNamespaceResolver nsmgr)
    {
      return (object) this.ParseValueType(s, nameTable, nsmgr);
    }

    internal override System.ValueType ParseValueType(
      string s,
      XmlNameTable nameTable,
      IXmlNamespaceResolver nsmgr)
    {
      return (System.ValueType) XmlConvert.ToBoolean(this.Normalize(s));
    }

    public override bool Bounded => false;

    public override bool Finite => true;

    public override bool Numeric => false;

    public override XsdOrderedFacet Ordered => XsdOrderedFacet.Total;
  }
}
