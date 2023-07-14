// Decompiled with JetBrains decompiler
// Type: System.Xml.Schema.IXmlSchemaInfo
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.Schema
{
  public interface IXmlSchemaInfo
  {
    bool IsDefault { get; }

    bool IsNil { get; }

    XmlSchemaSimpleType MemberType { get; }

    XmlSchemaAttribute SchemaAttribute { get; }

    XmlSchemaElement SchemaElement { get; }

    XmlSchemaType SchemaType { get; }

    XmlSchemaValidity Validity { get; }
  }
}
