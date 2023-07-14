// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.SoapSchemaMember
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.Serialization
{
  public class SoapSchemaMember
  {
    private string memberName;
    private XmlQualifiedName memberType = XmlQualifiedName.Empty;

    public string MemberName
    {
      get => this.memberName == null ? string.Empty : this.memberName;
      set => this.memberName = value;
    }

    public XmlQualifiedName MemberType
    {
      get => this.memberType;
      set => this.memberType = value;
    }
  }
}
