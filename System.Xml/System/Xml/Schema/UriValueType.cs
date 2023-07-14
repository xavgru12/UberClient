// Decompiled with JetBrains decompiler
// Type: System.Xml.Schema.UriValueType
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using Mono.Xml.Schema;

namespace System.Xml.Schema
{
  internal struct UriValueType
  {
    private XmlSchemaUri value;

    public UriValueType(XmlSchemaUri value) => this.value = value;

    public XmlSchemaUri Value => this.value;

    public override bool Equals(object obj) => obj is UriValueType uriValueType && uriValueType == this;

    public override int GetHashCode() => this.value.GetHashCode();

    public override string ToString() => this.value.ToString();

    public static bool operator ==(UriValueType v1, UriValueType v2) => v1.Value == v2.Value;

    public static bool operator !=(UriValueType v1, UriValueType v2) => v1.Value != v2.Value;
  }
}
