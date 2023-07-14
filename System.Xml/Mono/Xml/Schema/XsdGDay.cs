// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Schema.XsdGDay
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System;
using System.Xml;
using System.Xml.Schema;

namespace Mono.Xml.Schema
{
  internal class XsdGDay : XsdAnySimpleType
  {
    internal XsdGDay() => this.WhitespaceValue = XsdWhitespaceFacet.Collapse;

    internal override XmlSchemaFacet.Facet AllowedFacets => XsdAnySimpleType.durationAllowedFacets;

    public override XmlTypeCode TypeCode => XmlTypeCode.GDay;

    public override Type ValueType => typeof (DateTime);

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
      return (System.ValueType) DateTime.ParseExact(this.Normalize(s), "---dd", (IFormatProvider) null);
    }

    internal override XsdOrdering Compare(object x, object y)
    {
      if (!(x is DateTime) || !(y is DateTime t2))
        return XsdOrdering.Indeterminate;
      int num = DateTime.Compare((DateTime) x, t2);
      if (num < 0)
        return XsdOrdering.LessThan;
      return num > 0 ? XsdOrdering.GreaterThan : XsdOrdering.Equal;
    }
  }
}
