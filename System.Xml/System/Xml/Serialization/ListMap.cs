// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.ListMap
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;

namespace System.Xml.Serialization
{
  internal class ListMap : ObjectMap
  {
    private XmlTypeMapElementInfoList _itemInfo;
    private bool _gotNestedMapping;
    private XmlTypeMapping _nestedArrayMapping;
    private string _choiceMember;

    public bool IsMultiArray => this.NestedArrayMapping != null;

    public string ChoiceMember
    {
      get => this._choiceMember;
      set => this._choiceMember = value;
    }

    public XmlTypeMapping NestedArrayMapping
    {
      get
      {
        if (this._gotNestedMapping)
          return this._nestedArrayMapping;
        this._gotNestedMapping = true;
        this._nestedArrayMapping = ((XmlTypeMapElementInfo) this._itemInfo[0]).MappedType;
        if (this._nestedArrayMapping == null)
          return (XmlTypeMapping) null;
        if (this._nestedArrayMapping.TypeData.SchemaType != SchemaTypes.Array)
        {
          this._nestedArrayMapping = (XmlTypeMapping) null;
          return (XmlTypeMapping) null;
        }
        foreach (XmlTypeMapElementInfo typeMapElementInfo in (ArrayList) this._itemInfo)
        {
          if (typeMapElementInfo.MappedType != this._nestedArrayMapping)
          {
            this._nestedArrayMapping = (XmlTypeMapping) null;
            return (XmlTypeMapping) null;
          }
        }
        return this._nestedArrayMapping;
      }
    }

    public XmlTypeMapElementInfoList ItemInfo
    {
      get => this._itemInfo;
      set => this._itemInfo = value;
    }

    public XmlTypeMapElementInfo FindElement(object ob, int index, object memberValue)
    {
      if (this._itemInfo.Count == 1)
        return (XmlTypeMapElementInfo) this._itemInfo[0];
      if (this._choiceMember != null && index != -1)
      {
        Array array = (Array) XmlTypeMapMember.GetValue(ob, this._choiceMember);
        if (array == null || index >= array.Length)
          throw new InvalidOperationException("Invalid or missing choice enum value in member '" + this._choiceMember + "'.");
        object obj = array.GetValue(index);
        foreach (XmlTypeMapElementInfo element in (ArrayList) this._itemInfo)
        {
          if (element.ChoiceValue != null && element.ChoiceValue.Equals(obj))
            return element;
        }
      }
      else
      {
        if (memberValue == null)
          return (XmlTypeMapElementInfo) null;
        Type type = memberValue.GetType();
        foreach (XmlTypeMapElementInfo element in (ArrayList) this._itemInfo)
        {
          if (element.TypeData.Type == type)
            return element;
        }
      }
      return (XmlTypeMapElementInfo) null;
    }

    public XmlTypeMapElementInfo FindElement(string elementName, string ns)
    {
      foreach (XmlTypeMapElementInfo element in (ArrayList) this._itemInfo)
      {
        if (element.ElementName == elementName && element.Namespace == ns)
          return element;
      }
      return (XmlTypeMapElementInfo) null;
    }

    public XmlTypeMapElementInfo FindTextElement()
    {
      foreach (XmlTypeMapElementInfo textElement in (ArrayList) this._itemInfo)
      {
        if (textElement.IsTextElement)
          return textElement;
      }
      return (XmlTypeMapElementInfo) null;
    }

    public string GetSchemaArrayName()
    {
      XmlTypeMapElementInfo typeMapElementInfo = (XmlTypeMapElementInfo) this._itemInfo[0];
      return typeMapElementInfo.MappedType != null ? TypeTranslator.GetArrayName(typeMapElementInfo.MappedType.XmlType) : TypeTranslator.GetArrayName(typeMapElementInfo.TypeData.XmlType);
    }

    public void GetArrayType(int itemCount, out string localName, out string ns)
    {
      string str = itemCount == -1 ? "[]" : "[" + (object) itemCount + "]";
      XmlTypeMapElementInfo typeMapElementInfo = (XmlTypeMapElementInfo) this._itemInfo[0];
      if (typeMapElementInfo.TypeData.SchemaType == SchemaTypes.Array)
      {
        string localName1;
        ((ListMap) typeMapElementInfo.MappedType.ObjectMap).GetArrayType(-1, out localName1, out ns);
        localName = localName1 + str;
      }
      else if (typeMapElementInfo.MappedType != null)
      {
        localName = typeMapElementInfo.MappedType.XmlType + str;
        ns = typeMapElementInfo.MappedType.Namespace;
      }
      else
      {
        localName = typeMapElementInfo.TypeData.XmlType + str;
        ns = typeMapElementInfo.DataTypeNamespace;
      }
    }

    public override bool Equals(object other)
    {
      if (!(other is ListMap listMap) || this._itemInfo.Count != listMap._itemInfo.Count)
        return false;
      for (int index = 0; index < this._itemInfo.Count; ++index)
      {
        if (!this._itemInfo[index].Equals(listMap._itemInfo[index]))
          return false;
      }
      return true;
    }

    public override int GetHashCode() => base.GetHashCode();
  }
}
