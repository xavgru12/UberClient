// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.XmlTypeMapMemberAttribute
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Xml.Schema;

namespace System.Xml.Serialization
{
  internal class XmlTypeMapMemberAttribute : XmlTypeMapMember
  {
    private string _attributeName;
    private string _namespace = string.Empty;
    private XmlSchemaForm _form;
    private XmlTypeMapping _mappedType;

    public string AttributeName
    {
      get => this._attributeName;
      set => this._attributeName = value;
    }

    public string Namespace
    {
      get => this._namespace;
      set => this._namespace = value;
    }

    public string DataTypeNamespace => this._mappedType == null ? "http://www.w3.org/2001/XMLSchema" : this._mappedType.Namespace;

    public XmlSchemaForm Form
    {
      get => this._form;
      set => this._form = value;
    }

    public XmlTypeMapping MappedType
    {
      get => this._mappedType;
      set => this._mappedType = value;
    }
  }
}
