// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Schema.XsdLong
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System;
using System.Xml;
using System.Xml.Schema;

namespace Mono.Xml.Schema
{
  internal class XsdLong : XsdInteger
  {
    public override XmlTypeCode TypeCode => XmlTypeCode.Long;

    public override Type ValueType => typeof (long);

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
      return (System.ValueType) XmlConvert.ToInt64(this.Normalize(s));
    }

    internal override XsdOrdering Compare(object x, object y)
    {
      if (!(x is long) || !(y is long num))
        return XsdOrdering.Indeterminate;
      if ((long) x == num)
        return XsdOrdering.Equal;
      return (long) x < (long) y ? XsdOrdering.LessThan : XsdOrdering.GreaterThan;
    }
  }
}
