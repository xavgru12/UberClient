﻿// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Schema.XsdIDRefs
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System;
using System.Xml;
using System.Xml.Schema;

namespace Mono.Xml.Schema
{
  internal class XsdIDRefs : XsdName
  {
    internal XsdIDRefs()
    {
    }

    public override XmlTokenizedType TokenizedType => XmlTokenizedType.IDREFS;

    [MonoTODO]
    public override XmlTypeCode TypeCode => XmlTypeCode.Item;

    public override Type ValueType => typeof (string[]);

    public override object ParseValue(string value, XmlNameTable nt, IXmlNamespaceResolver nsmgr) => (object) this.GetValidatedArray(value, nt);

    internal override System.ValueType ParseValueType(
      string s,
      XmlNameTable nameTable,
      IXmlNamespaceResolver nsmgr)
    {
      return (System.ValueType) new StringArrayValueType(this.GetValidatedArray(s, nameTable));
    }

    private string[] GetValidatedArray(string value, XmlNameTable nt)
    {
      string[] listValue = this.ParseListValue(value, nt);
      for (int index = 0; index < listValue.Length; ++index)
        XmlConvert.VerifyNCName(listValue[index]);
      return listValue;
    }
  }
}
