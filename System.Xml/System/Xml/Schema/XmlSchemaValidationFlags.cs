// Decompiled with JetBrains decompiler
// Type: System.Xml.Schema.XmlSchemaValidationFlags
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.Schema
{
  [Flags]
  public enum XmlSchemaValidationFlags
  {
    None = 0,
    ProcessInlineSchema = 1,
    ProcessSchemaLocation = 2,
    ReportValidationWarnings = 4,
    ProcessIdentityConstraints = 8,
    [Obsolete("It is really idiotic idea to include such validation option that breaks W3C XML Schema specification compliance and interoperability.")] AllowXmlAttributes = 16, // 0x00000010
  }
}
