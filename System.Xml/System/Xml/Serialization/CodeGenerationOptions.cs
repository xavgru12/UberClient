// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.CodeGenerationOptions
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.Serialization
{
  [Flags]
  public enum CodeGenerationOptions
  {
    [XmlIgnore] None = 0,
    [XmlEnum("properties")] GenerateProperties = 1,
    [XmlEnum("newAsync")] GenerateNewAsync = 2,
    [XmlEnum("oldAsync")] GenerateOldAsync = 4,
    [XmlEnum("order")] GenerateOrder = 8,
    [XmlEnum("enableDataBinding")] EnableDataBinding = 16, // 0x00000010
  }
}
