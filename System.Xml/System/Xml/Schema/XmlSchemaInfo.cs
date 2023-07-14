// Decompiled with JetBrains decompiler
// Type: System.Xml.Schema.XmlSchemaInfo
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.Schema
{
  [MonoTODO]
  public class XmlSchemaInfo : IXmlSchemaInfo
  {
    private bool isDefault;
    private bool isNil;
    private XmlSchemaSimpleType memberType;
    private XmlSchemaAttribute attr;
    private XmlSchemaElement elem;
    private XmlSchemaType type;
    private XmlSchemaValidity validity;
    private XmlSchemaContentType contentType;

    public XmlSchemaInfo()
    {
    }

    internal XmlSchemaInfo(IXmlSchemaInfo info)
    {
      this.isDefault = info.IsDefault;
      this.isNil = info.IsNil;
      this.memberType = info.MemberType;
      this.attr = info.SchemaAttribute;
      this.elem = info.SchemaElement;
      this.type = info.SchemaType;
      this.validity = info.Validity;
    }

    [MonoTODO]
    public XmlSchemaContentType ContentType
    {
      get => this.contentType;
      set => this.contentType = value;
    }

    [MonoTODO]
    public bool IsDefault
    {
      get => this.isDefault;
      set => this.isDefault = value;
    }

    [MonoTODO]
    public bool IsNil
    {
      get => this.isNil;
      set => this.isNil = value;
    }

    [MonoTODO]
    public XmlSchemaSimpleType MemberType
    {
      get => this.memberType;
      set => this.memberType = value;
    }

    [MonoTODO]
    public XmlSchemaAttribute SchemaAttribute
    {
      get => this.attr;
      set => this.attr = value;
    }

    [MonoTODO]
    public XmlSchemaElement SchemaElement
    {
      get => this.elem;
      set => this.elem = value;
    }

    [MonoTODO]
    public XmlSchemaType SchemaType
    {
      get => this.type;
      set => this.type = value;
    }

    [MonoTODO]
    public XmlSchemaValidity Validity
    {
      get => this.validity;
      set => this.validity = value;
    }
  }
}
