// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Schema.XsdUnsignedLong
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System;
using System.Xml;
using System.Xml.Schema;

namespace Mono.Xml.Schema
{
  internal class XsdUnsignedLong : XsdNonNegativeInteger
  {
    public override XmlTypeCode TypeCode => XmlTypeCode.UnsignedLong;

    public override Type ValueType => typeof (ulong);

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
      return (System.ValueType) XmlConvert.ToUInt64(this.Normalize(s));
    }

    internal override XsdOrdering Compare(object x, object y)
    {
      if (!(x is ulong) || !(y is ulong num))
        return XsdOrdering.Indeterminate;
      if ((long) (ulong) x == (long) num)
        return XsdOrdering.Equal;
      return (ulong) x < (ulong) y ? XsdOrdering.LessThan : XsdOrdering.GreaterThan;
    }
  }
}
