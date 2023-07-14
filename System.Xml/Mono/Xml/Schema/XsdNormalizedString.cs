// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Schema.XsdNormalizedString
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System;
using System.Xml;
using System.Xml.Schema;

namespace Mono.Xml.Schema
{
  internal class XsdNormalizedString : XsdString
  {
    internal XsdNormalizedString() => this.WhitespaceValue = XsdWhitespaceFacet.Replace;

    public override XmlTokenizedType TokenizedType => XmlTokenizedType.CDATA;

    public override XmlTypeCode TypeCode => XmlTypeCode.NormalizedString;

    public override Type ValueType => typeof (string);
  }
}
