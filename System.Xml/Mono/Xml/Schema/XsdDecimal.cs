// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Schema.XsdDecimal
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System;
using System.Xml;
using System.Xml.Schema;

namespace Mono.Xml.Schema
{
  internal class XsdDecimal : XsdAnySimpleType
  {
    internal XsdDecimal() => this.WhitespaceValue = XsdWhitespaceFacet.Collapse;

    internal override XmlSchemaFacet.Facet AllowedFacets => XsdAnySimpleType.decimalAllowedFacets;

    public override XmlTokenizedType TokenizedType => XmlTokenizedType.None;

    public override XmlTypeCode TypeCode => XmlTypeCode.Decimal;

    public override Type ValueType => typeof (Decimal);

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
      return (System.ValueType) XmlConvert.ToDecimal(this.Normalize(s));
    }

    internal override XsdOrdering Compare(object x, object y)
    {
      if (!(x is Decimal) || !(y is Decimal d2))
        return XsdOrdering.Indeterminate;
      int num = Decimal.Compare((Decimal) x, d2);
      if (num < 0)
        return XsdOrdering.LessThan;
      return num > 0 ? XsdOrdering.GreaterThan : XsdOrdering.Equal;
    }

    public override bool Bounded => false;

    public override bool Finite => false;

    public override bool Numeric => true;

    public override XsdOrderedFacet Ordered => XsdOrderedFacet.Total;
  }
}
