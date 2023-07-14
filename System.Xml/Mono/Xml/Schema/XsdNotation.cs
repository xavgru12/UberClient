// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Schema.XsdNotation
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System;
using System.Xml;
using System.Xml.Schema;

namespace Mono.Xml.Schema
{
  internal class XsdNotation : XsdAnySimpleType
  {
    internal XsdNotation()
    {
    }

    internal override XmlSchemaFacet.Facet AllowedFacets => XsdAnySimpleType.stringAllowedFacets;

    public override XmlTokenizedType TokenizedType => XmlTokenizedType.NOTATION;

    public override XmlTypeCode TypeCode => XmlTypeCode.Notation;

    public override Type ValueType => typeof (string);

    public override object ParseValue(
      string s,
      XmlNameTable nameTable,
      IXmlNamespaceResolver nsmgr)
    {
      return (object) this.Normalize(s);
    }

    public override bool Bounded => false;

    public override bool Finite => false;

    public override bool Numeric => false;

    public override XsdOrderedFacet Ordered => XsdOrderedFacet.False;
  }
}
