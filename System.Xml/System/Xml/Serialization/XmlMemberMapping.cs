// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.XmlMemberMapping
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Xml.Schema;

namespace System.Xml.Serialization
{
  public class XmlMemberMapping
  {
    private XmlTypeMapMember _mapMember;
    private string _elementName;
    private string _memberName;
    private string _namespace;
    private string _typeNamespace;
    private XmlSchemaForm _form;

    internal XmlMemberMapping(
      string memberName,
      string defaultNamespace,
      XmlTypeMapMember mapMem,
      bool encodedFormat)
    {
      this._mapMember = mapMem;
      this._memberName = memberName;
      switch (mapMem)
      {
        case XmlTypeMapMemberAnyElement _:
          XmlTypeMapMemberAnyElement memberAnyElement = (XmlTypeMapMemberAnyElement) mapMem;
          XmlTypeMapElementInfo typeMapElementInfo1 = (XmlTypeMapElementInfo) memberAnyElement.ElementInfo[memberAnyElement.ElementInfo.Count - 1];
          this._elementName = typeMapElementInfo1.ElementName;
          this._namespace = typeMapElementInfo1.Namespace;
          this._typeNamespace = typeMapElementInfo1.MappedType == null ? string.Empty : typeMapElementInfo1.MappedType.Namespace;
          break;
        case XmlTypeMapMemberElement _:
          XmlTypeMapElementInfo typeMapElementInfo2 = (XmlTypeMapElementInfo) ((XmlTypeMapMemberElement) mapMem).ElementInfo[0];
          this._elementName = typeMapElementInfo2.ElementName;
          if (encodedFormat)
          {
            this._namespace = defaultNamespace;
            this._typeNamespace = typeMapElementInfo2.MappedType == null ? typeMapElementInfo2.DataTypeNamespace : string.Empty;
            break;
          }
          this._namespace = typeMapElementInfo2.Namespace;
          this._typeNamespace = typeMapElementInfo2.MappedType == null ? string.Empty : typeMapElementInfo2.MappedType.Namespace;
          this._form = typeMapElementInfo2.Form;
          break;
        default:
          this._elementName = this._memberName;
          this._namespace = string.Empty;
          break;
      }
      if (this._form != XmlSchemaForm.None)
        return;
      this._form = XmlSchemaForm.Qualified;
    }

    public bool Any => this._mapMember is XmlTypeMapMemberAnyElement;

    public string ElementName => this._elementName;

    public string MemberName => this._memberName;

    public string Namespace => this._namespace;

    public string TypeFullName => this._mapMember.TypeData.FullTypeName;

    public string TypeName => this._mapMember.TypeData.XmlType;

    public string TypeNamespace => this._typeNamespace;

    internal XmlTypeMapMember TypeMapMember => this._mapMember;

    internal XmlSchemaForm Form => this._form;

    public string XsdElementName => this._mapMember.Name;

    public bool CheckSpecified => this._mapMember.IsOptionalValueType;
  }
}
