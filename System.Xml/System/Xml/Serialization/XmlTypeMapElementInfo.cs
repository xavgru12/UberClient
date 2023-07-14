// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.XmlTypeMapElementInfo
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Xml.Schema;

namespace System.Xml.Serialization
{
  internal class XmlTypeMapElementInfo
  {
    private string _elementName;
    private string _namespace = string.Empty;
    private XmlSchemaForm _form;
    private XmlTypeMapMember _member;
    private object _choiceValue;
    private bool _isNullable;
    private int _nestingLevel;
    private XmlTypeMapping _mappedType;
    private TypeData _type;
    private bool _wrappedElement = true;

    public XmlTypeMapElementInfo(XmlTypeMapMember member, TypeData type)
    {
      this._member = member;
      this._type = type;
      if (!type.IsValueType || !type.IsNullable)
        return;
      this._isNullable = true;
    }

    public TypeData TypeData
    {
      get => this._type;
      set => this._type = value;
    }

    public object ChoiceValue
    {
      get => this._choiceValue;
      set => this._choiceValue = value;
    }

    public string ElementName
    {
      get => this._elementName;
      set => this._elementName = value;
    }

    public string Namespace
    {
      get => this._namespace;
      set => this._namespace = value;
    }

    public string DataTypeNamespace => this._mappedType == null ? "http://www.w3.org/2001/XMLSchema" : this._mappedType.XmlTypeNamespace;

    public string DataTypeName => this._mappedType == null ? this.TypeData.XmlType : this._mappedType.XmlType;

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

    public bool IsNullable
    {
      get => this._isNullable;
      set => this._isNullable = value;
    }

    internal bool IsPrimitive => this._mappedType == null;

    public XmlTypeMapMember Member
    {
      get => this._member;
      set => this._member = value;
    }

    public int NestingLevel
    {
      get => this._nestingLevel;
      set => this._nestingLevel = value;
    }

    public bool MultiReferenceType => this._mappedType != null && this._mappedType.MultiReferenceType;

    public bool WrappedElement
    {
      get => this._wrappedElement;
      set => this._wrappedElement = value;
    }

    public bool IsTextElement
    {
      get => this.ElementName == "<text>";
      set
      {
        if (!value)
          throw new Exception("INTERNAL ERROR; someone wrote unexpected code in sys.xml");
        this.ElementName = "<text>";
        this.Namespace = string.Empty;
      }
    }

    public bool IsUnnamedAnyElement
    {
      get => this.ElementName == string.Empty;
      set
      {
        if (!value)
          throw new Exception("INTERNAL ERROR; someone wrote unexpected code in sys.xml");
        this.ElementName = string.Empty;
        this.Namespace = string.Empty;
      }
    }

    public override bool Equals(object other)
    {
      if (other == null)
        return false;
      XmlTypeMapElementInfo typeMapElementInfo = (XmlTypeMapElementInfo) other;
      return !(this._elementName != typeMapElementInfo._elementName) && !(this._type.XmlType != typeMapElementInfo._type.XmlType) && !(this._namespace != typeMapElementInfo._namespace) && this._form == typeMapElementInfo._form && this._type == typeMapElementInfo._type && this._isNullable == typeMapElementInfo._isNullable && (this._choiceValue == null || this._choiceValue.Equals(typeMapElementInfo._choiceValue)) && this._choiceValue == typeMapElementInfo._choiceValue;
    }

    public override int GetHashCode() => base.GetHashCode();
  }
}
