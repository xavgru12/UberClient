// Decompiled with JetBrains decompiler
// Type: System.Xml.Schema.StringValueType
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.Schema
{
  internal struct StringValueType
  {
    private string value;

    public StringValueType(string value) => this.value = value;

    public string Value => this.value;

    public override bool Equals(object obj) => obj is StringValueType stringValueType && stringValueType == this;

    public override int GetHashCode() => this.value.GetHashCode();

    public static bool operator ==(StringValueType v1, StringValueType v2) => v1.Value == v2.Value;

    public static bool operator !=(StringValueType v1, StringValueType v2) => v1.Value != v2.Value;
  }
}
