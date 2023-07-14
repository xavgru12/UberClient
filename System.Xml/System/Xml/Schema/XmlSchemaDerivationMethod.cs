// Decompiled with JetBrains decompiler
// Type: System.Xml.Schema.XmlSchemaDerivationMethod
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Xml.Serialization;

namespace System.Xml.Schema
{
  [Flags]
  public enum XmlSchemaDerivationMethod
  {
    [XmlEnum("")] Empty = 0,
    [XmlEnum("substitution")] Substitution = 1,
    [XmlEnum("extension")] Extension = 2,
    [XmlEnum("restriction")] Restriction = 4,
    [XmlEnum("list")] List = 8,
    [XmlEnum("union")] Union = 16, // 0x00000010
    [XmlEnum("#all")] All = 255, // 0x000000FF
    [XmlIgnore] None = 256, // 0x00000100
  }
}
