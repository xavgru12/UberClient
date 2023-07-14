// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Schema.XsdAnySimpleType
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System;
using System.Xml;
using System.Xml.Schema;

namespace Mono.Xml.Schema
{
  internal class XsdAnySimpleType : XmlSchemaDatatype
  {
    private static XsdAnySimpleType instance;
    private static readonly char[] whitespaceArray = new char[1]
    {
      ' '
    };
    internal static readonly XmlSchemaFacet.Facet booleanAllowedFacets = XmlSchemaFacet.Facet.pattern | XmlSchemaFacet.Facet.whiteSpace;
    internal static readonly XmlSchemaFacet.Facet decimalAllowedFacets = XmlSchemaFacet.Facet.pattern | XmlSchemaFacet.Facet.enumeration | XmlSchemaFacet.Facet.whiteSpace | XmlSchemaFacet.Facet.maxInclusive | XmlSchemaFacet.Facet.maxExclusive | XmlSchemaFacet.Facet.minExclusive | XmlSchemaFacet.Facet.minInclusive | XmlSchemaFacet.Facet.totalDigits | XmlSchemaFacet.Facet.fractionDigits;
    internal static readonly XmlSchemaFacet.Facet durationAllowedFacets = XmlSchemaFacet.Facet.pattern | XmlSchemaFacet.Facet.enumeration | XmlSchemaFacet.Facet.whiteSpace | XmlSchemaFacet.Facet.maxInclusive | XmlSchemaFacet.Facet.maxExclusive | XmlSchemaFacet.Facet.minExclusive | XmlSchemaFacet.Facet.minInclusive;
    internal static readonly XmlSchemaFacet.Facet stringAllowedFacets = XmlSchemaFacet.Facet.length | XmlSchemaFacet.Facet.minLength | XmlSchemaFacet.Facet.maxLength | XmlSchemaFacet.Facet.pattern | XmlSchemaFacet.Facet.enumeration | XmlSchemaFacet.Facet.whiteSpace;

    protected XsdAnySimpleType()
    {
    }

    static XsdAnySimpleType() => XsdAnySimpleType.instance = new XsdAnySimpleType();

    public static XsdAnySimpleType Instance => XsdAnySimpleType.instance;

    public override XmlTypeCode TypeCode => XmlTypeCode.AnyAtomicType;

    public virtual bool Bounded => false;

    public virtual bool Finite => false;

    public virtual bool Numeric => false;

    public virtual XsdOrderedFacet Ordered => XsdOrderedFacet.False;

    public override Type ValueType => XmlSchemaUtil.StrictMsCompliant ? typeof (string) : typeof (object);

    public override XmlTokenizedType TokenizedType => XmlTokenizedType.None;

    public override object ParseValue(
      string s,
      XmlNameTable nameTable,
      IXmlNamespaceResolver nsmgr)
    {
      return (object) this.Normalize(s);
    }

    internal override System.ValueType ParseValueType(
      string s,
      XmlNameTable nameTable,
      IXmlNamespaceResolver nsmgr)
    {
      return (System.ValueType) new StringValueType(this.Normalize(s));
    }

    internal string[] ParseListValue(string s, XmlNameTable nameTable) => this.Normalize(s, XsdWhitespaceFacet.Collapse).Split(XsdAnySimpleType.whitespaceArray);

    internal bool AllowsFacet(XmlSchemaFacet xsf) => (this.AllowedFacets & xsf.ThisFacet) != XmlSchemaFacet.Facet.None;

    internal virtual XsdOrdering Compare(object x, object y) => XsdOrdering.Indeterminate;

    internal virtual int Length(string s) => s.Length;

    internal virtual XmlSchemaFacet.Facet AllowedFacets => XmlSchemaFacet.AllFacets;
  }
}
