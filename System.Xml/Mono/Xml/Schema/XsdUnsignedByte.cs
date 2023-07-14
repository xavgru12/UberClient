// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Schema.XsdUnsignedByte
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System;
using System.Xml;
using System.Xml.Schema;

namespace Mono.Xml.Schema
{
  internal class XsdUnsignedByte : XsdUnsignedShort
  {
    public override XmlTypeCode TypeCode => XmlTypeCode.UnsignedByte;

    public override Type ValueType => typeof (byte);

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
      return (System.ValueType) XmlConvert.ToByte(this.Normalize(s));
    }

    internal override XsdOrdering Compare(object x, object y)
    {
      if (!(x is byte) || !(y is byte num))
        return XsdOrdering.Indeterminate;
      if ((int) (byte) x == (int) num)
        return XsdOrdering.Equal;
      return (int) (byte) x < (int) (byte) y ? XsdOrdering.LessThan : XsdOrdering.GreaterThan;
    }
  }
}
