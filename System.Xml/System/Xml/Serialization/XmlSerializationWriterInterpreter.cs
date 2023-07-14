// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.XmlSerializationWriterInterpreter
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.Reflection;
using System.Text;

namespace System.Xml.Serialization
{
  internal class XmlSerializationWriterInterpreter : XmlSerializationWriter
  {
    private const string xmlNamespace = "http://www.w3.org/2000/xmlns/";
    private XmlMapping _typeMap;
    private SerializationFormat _format;

    public XmlSerializationWriterInterpreter(XmlMapping typeMap)
    {
      this._typeMap = typeMap;
      this._format = typeMap.Format;
    }

    protected override void InitCallbacks()
    {
      ArrayList relatedMaps = this._typeMap.RelatedMaps;
      if (relatedMaps == null)
        return;
      foreach (XmlTypeMapping typeMap in relatedMaps)
      {
        XmlSerializationWriterInterpreter.CallbackInfo callbackInfo = new XmlSerializationWriterInterpreter.CallbackInfo(this, typeMap);
        if (typeMap.TypeData.SchemaType == SchemaTypes.Enum)
          this.AddWriteCallback(typeMap.TypeData.Type, typeMap.XmlType, typeMap.Namespace, new XmlSerializationWriteCallback(callbackInfo.WriteEnum));
        else
          this.AddWriteCallback(typeMap.TypeData.Type, typeMap.XmlType, typeMap.Namespace, new XmlSerializationWriteCallback(callbackInfo.WriteObject));
      }
    }

    public void WriteRoot(object ob)
    {
      this.WriteStartDocument();
      if (this._typeMap is XmlTypeMapping)
      {
        XmlTypeMapping typeMap = (XmlTypeMapping) this._typeMap;
        if (typeMap.TypeData.SchemaType == SchemaTypes.Class || typeMap.TypeData.SchemaType == SchemaTypes.Array)
          this.TopLevelElement();
        if (this._format == SerializationFormat.Literal)
          this.WriteObject(typeMap, ob, typeMap.ElementName, typeMap.Namespace, true, false, true);
        else
          this.WritePotentiallyReferencingElement(typeMap.ElementName, typeMap.Namespace, ob, typeMap.TypeData.Type, true, false);
      }
      else
      {
        if (!(ob is object[]))
          throw this.CreateUnknownTypeException(ob);
        this.WriteMessage((XmlMembersMapping) this._typeMap, (object[]) ob);
      }
      this.WriteReferencedElements();
    }

    protected XmlTypeMapping GetTypeMap(Type type)
    {
      ArrayList relatedMaps = this._typeMap.RelatedMaps;
      if (relatedMaps != null)
      {
        foreach (XmlTypeMapping typeMap in relatedMaps)
        {
          if (typeMap.TypeData.Type == type)
            return typeMap;
        }
      }
      throw new InvalidOperationException("Type " + (object) type + " not mapped");
    }

    protected virtual void WriteObject(
      XmlTypeMapping typeMap,
      object ob,
      string element,
      string namesp,
      bool isNullable,
      bool needType,
      bool writeWrappingElem)
    {
      if (ob == null)
      {
        if (!isNullable)
          return;
        if (this._format == SerializationFormat.Literal)
          this.WriteNullTagLiteral(element, namesp);
        else
          this.WriteNullTagEncoded(element, namesp);
      }
      else if (ob is XmlNode)
      {
        if (this._format == SerializationFormat.Literal)
          this.WriteElementLiteral((XmlNode) ob, string.Empty, string.Empty, true, false);
        else
          this.WriteElementEncoded((XmlNode) ob, string.Empty, string.Empty, true, false);
      }
      else if (typeMap.TypeData.SchemaType == SchemaTypes.XmlSerializable)
      {
        this.WriteSerializable((IXmlSerializable) ob, element, namesp, isNullable);
      }
      else
      {
        XmlTypeMapping realTypeMap = typeMap.GetRealTypeMap(ob.GetType());
        if (realTypeMap == null)
        {
          if (ob.GetType().IsArray && typeof (XmlNode).IsAssignableFrom(ob.GetType().GetElementType()))
          {
            this.Writer.WriteStartElement(element, namesp);
            foreach (XmlNode xmlNode in (IEnumerable) ob)
              xmlNode.WriteTo(this.Writer);
            this.Writer.WriteEndElement();
          }
          else
            this.WriteTypedPrimitive(element, namesp, ob, true);
        }
        else
        {
          if (writeWrappingElem)
          {
            if (realTypeMap != typeMap || this._format == SerializationFormat.Encoded)
              needType = true;
            this.WriteStartElement(element, namesp, ob);
          }
          if (needType)
            this.WriteXsiType(realTypeMap.XmlType, realTypeMap.XmlTypeNamespace);
          switch (realTypeMap.TypeData.SchemaType)
          {
            case SchemaTypes.Primitive:
              this.WritePrimitiveElement(realTypeMap, ob, element, namesp);
              break;
            case SchemaTypes.Enum:
              this.WriteEnumElement(realTypeMap, ob, element, namesp);
              break;
            case SchemaTypes.Array:
              this.WriteListElement(realTypeMap, ob, element, namesp);
              break;
            case SchemaTypes.Class:
              this.WriteObjectElement(realTypeMap, ob, element, namesp);
              break;
          }
          if (!writeWrappingElem)
            return;
          this.WriteEndElement(ob);
        }
      }
    }

    protected virtual void WriteMessage(XmlMembersMapping membersMap, object[] parameters)
    {
      if (membersMap.HasWrapperElement)
      {
        this.TopLevelElement();
        this.WriteStartElement(membersMap.ElementName, membersMap.Namespace, this._format == SerializationFormat.Encoded);
        if (this.Writer.LookupPrefix("http://www.w3.org/2001/XMLSchema") == null)
          this.WriteAttribute("xmlns", "xsd", "http://www.w3.org/2001/XMLSchema", "http://www.w3.org/2001/XMLSchema");
        if (this.Writer.LookupPrefix("http://www.w3.org/2001/XMLSchema-instance") == null)
          this.WriteAttribute("xmlns", "xsi", "http://www.w3.org/2001/XMLSchema-instance", "http://www.w3.org/2001/XMLSchema-instance");
      }
      this.WriteMembers((ClassMap) membersMap.ObjectMap, (object) parameters, true);
      if (!membersMap.HasWrapperElement)
        return;
      this.WriteEndElement();
    }

    protected virtual void WriteObjectElement(
      XmlTypeMapping typeMap,
      object ob,
      string element,
      string namesp)
    {
      ClassMap objectMap = (ClassMap) typeMap.ObjectMap;
      if (objectMap.NamespaceDeclarations != null)
        this.WriteNamespaceDeclarations((XmlSerializerNamespaces) objectMap.NamespaceDeclarations.GetValue(ob));
      this.WriteObjectElementAttributes(typeMap, ob);
      this.WriteObjectElementElements(typeMap, ob);
    }

    protected virtual void WriteObjectElementAttributes(XmlTypeMapping typeMap, object ob) => this.WriteAttributeMembers((ClassMap) typeMap.ObjectMap, ob, false);

    protected virtual void WriteObjectElementElements(XmlTypeMapping typeMap, object ob) => this.WriteElementMembers((ClassMap) typeMap.ObjectMap, ob, false);

    private void WriteMembers(ClassMap map, object ob, bool isValueList)
    {
      this.WriteAttributeMembers(map, ob, isValueList);
      this.WriteElementMembers(map, ob, isValueList);
    }

    private void WriteAttributeMembers(ClassMap map, object ob, bool isValueList)
    {
      XmlTypeMapMember anyAttributeMember = (XmlTypeMapMember) map.DefaultAnyAttributeMember;
      if (anyAttributeMember != null && this.MemberHasValue(anyAttributeMember, ob, isValueList))
      {
        ICollection memberValue = (ICollection) this.GetMemberValue(anyAttributeMember, ob, isValueList);
        if (memberValue != null)
        {
          foreach (XmlAttribute node in (IEnumerable) memberValue)
          {
            if (node.NamespaceURI != "http://www.w3.org/2000/xmlns/")
              this.WriteXmlAttribute((XmlNode) node, ob);
          }
        }
      }
      ICollection attributeMembers = map.AttributeMembers;
      if (attributeMembers == null)
        return;
      foreach (XmlTypeMapMemberAttribute member in (IEnumerable) attributeMembers)
      {
        if (this.MemberHasValue((XmlTypeMapMember) member, ob, isValueList))
          this.WriteAttribute(member.AttributeName, member.Namespace, this.GetStringValue(member.MappedType, member.TypeData, this.GetMemberValue((XmlTypeMapMember) member, ob, isValueList)));
      }
    }

    private void WriteElementMembers(ClassMap map, object ob, bool isValueList)
    {
      ICollection elementMembers = map.ElementMembers;
      if (elementMembers == null)
        return;
      foreach (XmlTypeMapMemberElement member in (IEnumerable) elementMembers)
      {
        if (this.MemberHasValue((XmlTypeMapMember) member, ob, isValueList))
        {
          object memberValue = this.GetMemberValue((XmlTypeMapMember) member, ob, isValueList);
          Type type = member.GetType();
          if (type == typeof (XmlTypeMapMemberList))
            this.WriteMemberElement((XmlTypeMapElementInfo) member.ElementInfo[0], memberValue);
          else if (type == typeof (XmlTypeMapMemberFlatList))
          {
            if (memberValue != null)
              this.WriteListContent(ob, member.TypeData, ((XmlTypeMapMemberFlatList) member).ListMap, memberValue, (StringBuilder) null);
          }
          else if (type == typeof (XmlTypeMapMemberAnyElement))
          {
            if (memberValue != null)
              this.WriteAnyElementContent((XmlTypeMapMemberAnyElement) member, memberValue);
          }
          else if (type != typeof (XmlTypeMapMemberAnyAttribute))
          {
            if (type != typeof (XmlTypeMapMemberElement))
              throw new InvalidOperationException("Unknown member type");
            this.WriteMemberElement(member.FindElement(ob, memberValue), memberValue);
          }
        }
      }
    }

    private object GetMemberValue(XmlTypeMapMember member, object ob, bool isValueList) => isValueList ? ((object[]) ob)[member.GlobalIndex] : member.GetValue(ob);

    private bool MemberHasValue(XmlTypeMapMember member, object ob, bool isValueList)
    {
      if (isValueList)
        return member.GlobalIndex < ((object[]) ob).Length;
      if (member.DefaultValue != DBNull.Value)
      {
        object obj = this.GetMemberValue(member, ob, isValueList);
        if (obj == null && member.DefaultValue == null)
          return false;
        if (obj != null && obj.GetType().IsEnum)
        {
          if (obj.Equals(member.DefaultValue))
            return false;
          Type underlyingType = Enum.GetUnderlyingType(obj.GetType());
          obj = Convert.ChangeType(obj, underlyingType);
        }
        if (obj != null && obj.Equals(member.DefaultValue))
          return false;
      }
      else if (member.IsOptionalValueType)
        return member.GetValueSpecified(ob);
      return true;
    }

    private void WriteMemberElement(XmlTypeMapElementInfo elem, object memberValue)
    {
      switch (elem.TypeData.SchemaType)
      {
        case SchemaTypes.Primitive:
        case SchemaTypes.Enum:
          if (this._format == SerializationFormat.Literal)
          {
            this.WritePrimitiveValueLiteral(memberValue, elem.ElementName, elem.Namespace, elem.MappedType, elem.TypeData, elem.WrappedElement, elem.IsNullable);
            break;
          }
          this.WritePrimitiveValueEncoded(memberValue, elem.ElementName, elem.Namespace, new XmlQualifiedName(elem.DataTypeName, elem.DataTypeNamespace), elem.MappedType, elem.TypeData, elem.WrappedElement, elem.IsNullable);
          break;
        case SchemaTypes.Array:
          if (memberValue == null)
          {
            if (!elem.IsNullable)
              break;
            if (this._format == SerializationFormat.Literal)
            {
              this.WriteNullTagLiteral(elem.ElementName, elem.Namespace);
              break;
            }
            this.WriteNullTagEncoded(elem.ElementName, elem.Namespace);
            break;
          }
          if (elem.MappedType.MultiReferenceType)
          {
            this.WriteReferencingElement(elem.ElementName, elem.Namespace, memberValue, elem.IsNullable);
            break;
          }
          this.WriteStartElement(elem.ElementName, elem.Namespace, memberValue);
          this.WriteListContent((object) null, elem.TypeData, (ListMap) elem.MappedType.ObjectMap, memberValue, (StringBuilder) null);
          this.WriteEndElement(memberValue);
          break;
        case SchemaTypes.Class:
          if (elem.MappedType.MultiReferenceType)
          {
            if (elem.MappedType.TypeData.Type == typeof (object))
            {
              this.WritePotentiallyReferencingElement(elem.ElementName, elem.Namespace, memberValue, (Type) null, false, elem.IsNullable);
              break;
            }
            this.WriteReferencingElement(elem.ElementName, elem.Namespace, memberValue, elem.IsNullable);
            break;
          }
          this.WriteObject(elem.MappedType, memberValue, elem.ElementName, elem.Namespace, elem.IsNullable, false, true);
          break;
        case SchemaTypes.XmlSerializable:
          if (!elem.MappedType.TypeData.Type.IsInstanceOfType(memberValue))
            memberValue = this.ImplicitConvert(memberValue, elem.MappedType.TypeData.Type);
          this.WriteSerializable((IXmlSerializable) memberValue, elem.ElementName, elem.Namespace, elem.IsNullable);
          break;
        case SchemaTypes.XmlNode:
          string name = !elem.WrappedElement ? string.Empty : elem.ElementName;
          if (this._format == SerializationFormat.Literal)
          {
            this.WriteElementLiteral((XmlNode) memberValue, name, elem.Namespace, elem.IsNullable, false);
            break;
          }
          this.WriteElementEncoded((XmlNode) memberValue, name, elem.Namespace, elem.IsNullable, false);
          break;
        default:
          throw new NotSupportedException("Invalid value type");
      }
    }

    private object ImplicitConvert(object obj, Type type)
    {
      if (obj == null)
        return (object) null;
      for (Type type1 = type; type1 != typeof (object); type1 = type1.BaseType)
      {
        MethodInfo method = type1.GetMethod("op_Implicit", new Type[1]
        {
          type1
        });
        if (method != null && method.ReturnType.IsAssignableFrom(obj.GetType()))
          return method.Invoke((object) null, new object[1]
          {
            obj
          });
      }
      for (Type type2 = obj.GetType(); type2 != typeof (object); type2 = type2.BaseType)
      {
        MethodInfo method = type2.GetMethod("op_Implicit", new Type[1]
        {
          type2
        });
        if (method != null && method.ReturnType == type)
          return method.Invoke((object) null, new object[1]
          {
            obj
          });
      }
      return obj;
    }

    private void WritePrimitiveValueLiteral(
      object memberValue,
      string name,
      string ns,
      XmlTypeMapping mappedType,
      TypeData typeData,
      bool wrapped,
      bool isNullable)
    {
      if (!wrapped)
        this.WriteValue(this.GetStringValue(mappedType, typeData, memberValue));
      else if (isNullable)
      {
        if (typeData.Type == typeof (XmlQualifiedName))
          this.WriteNullableQualifiedNameLiteral(name, ns, (XmlQualifiedName) memberValue);
        else
          this.WriteNullableStringLiteral(name, ns, this.GetStringValue(mappedType, typeData, memberValue));
      }
      else if (typeData.Type == typeof (XmlQualifiedName))
        this.WriteElementQualifiedName(name, ns, (XmlQualifiedName) memberValue);
      else
        this.WriteElementString(name, ns, this.GetStringValue(mappedType, typeData, memberValue));
    }

    private void WritePrimitiveValueEncoded(
      object memberValue,
      string name,
      string ns,
      XmlQualifiedName xsiType,
      XmlTypeMapping mappedType,
      TypeData typeData,
      bool wrapped,
      bool isNullable)
    {
      if (!wrapped)
        this.WriteValue(this.GetStringValue(mappedType, typeData, memberValue));
      else if (isNullable)
      {
        if (typeData.Type == typeof (XmlQualifiedName))
          this.WriteNullableQualifiedNameEncoded(name, ns, (XmlQualifiedName) memberValue, xsiType);
        else
          this.WriteNullableStringEncoded(name, ns, this.GetStringValue(mappedType, typeData, memberValue), xsiType);
      }
      else if (typeData.Type == typeof (XmlQualifiedName))
        this.WriteElementQualifiedName(name, ns, (XmlQualifiedName) memberValue, xsiType);
      else
        this.WriteElementString(name, ns, this.GetStringValue(mappedType, typeData, memberValue), xsiType);
    }

    protected virtual void WriteListElement(
      XmlTypeMapping typeMap,
      object ob,
      string element,
      string namesp)
    {
      if (this._format == SerializationFormat.Encoded)
      {
        int listCount = this.GetListCount(typeMap.TypeData, ob);
        string localName;
        string ns;
        ((ListMap) typeMap.ObjectMap).GetArrayType(listCount, out localName, out ns);
        this.WriteAttribute("arrayType", "http://schemas.xmlsoap.org/soap/encoding/", !(ns != string.Empty) ? localName : this.FromXmlQualifiedName(new XmlQualifiedName(localName, ns)));
      }
      this.WriteListContent((object) null, typeMap.TypeData, (ListMap) typeMap.ObjectMap, ob, (StringBuilder) null);
    }

    private void WriteListContent(
      object container,
      TypeData listType,
      ListMap map,
      object ob,
      StringBuilder targetString)
    {
      if (listType.Type.IsArray)
      {
        Array array = (Array) ob;
        for (int index = 0; index < array.Length; ++index)
        {
          object obj = array.GetValue(index);
          XmlTypeMapElementInfo element = map.FindElement(container, index, obj);
          if (element != null && targetString == null)
            this.WriteMemberElement(element, obj);
          else if (element != null && targetString != null)
            targetString.Append(this.GetStringValue(element.MappedType, element.TypeData, obj)).Append(" ");
          else if (obj != null)
            throw this.CreateUnknownTypeException(obj);
        }
      }
      else
      {
        switch (ob)
        {
          case ICollection _:
            int num = (int) ob.GetType().GetProperty("Count").GetValue(ob, (object[]) null);
            PropertyInfo indexerProperty = TypeData.GetIndexerProperty(listType.Type);
            object[] index1 = new object[1];
            for (int index2 = 0; index2 < num; ++index2)
            {
              index1[0] = (object) index2;
              object obj = indexerProperty.GetValue(ob, index1);
              XmlTypeMapElementInfo element = map.FindElement(container, index2, obj);
              if (element != null && targetString == null)
                this.WriteMemberElement(element, obj);
              else if (element != null && targetString != null)
                targetString.Append(this.GetStringValue(element.MappedType, element.TypeData, obj)).Append(" ");
              else if (obj != null)
                throw this.CreateUnknownTypeException(obj);
            }
            break;
          case IEnumerable _:
            IEnumerator enumerator = ((IEnumerable) ob).GetEnumerator();
            try
            {
              while (enumerator.MoveNext())
              {
                object current = enumerator.Current;
                XmlTypeMapElementInfo element = map.FindElement(container, -1, current);
                if (element != null && targetString == null)
                  this.WriteMemberElement(element, current);
                else if (element != null && targetString != null)
                  targetString.Append(this.GetStringValue(element.MappedType, element.TypeData, current)).Append(" ");
                else if (current != null)
                  throw this.CreateUnknownTypeException(current);
              }
              break;
            }
            finally
            {
              if (enumerator is IDisposable disposable)
                disposable.Dispose();
            }
          default:
            throw new Exception("Unsupported collection type");
        }
      }
    }

    private int GetListCount(TypeData listType, object ob) => listType.Type.IsArray ? ((Array) ob).Length : (int) listType.Type.GetProperty("Count").GetValue(ob, (object[]) null);

    private void WriteAnyElementContent(XmlTypeMapMemberAnyElement member, object memberValue)
    {
      if (member.TypeData.Type == typeof (XmlElement))
        memberValue = (object) new object[1]{ memberValue };
      foreach (XmlNode node in (Array) memberValue)
      {
        if (node is XmlElement)
        {
          if (!member.IsElementDefined(node.Name, node.NamespaceURI))
            throw this.CreateUnknownAnyElementException(node.Name, node.NamespaceURI);
          if (this._format == SerializationFormat.Literal)
            this.WriteElementLiteral(node, string.Empty, string.Empty, false, true);
          else
            this.WriteElementEncoded(node, string.Empty, string.Empty, false, true);
        }
        else
          node.WriteTo(this.Writer);
      }
    }

    protected virtual void WritePrimitiveElement(
      XmlTypeMapping typeMap,
      object ob,
      string element,
      string namesp)
    {
      this.Writer.WriteString(this.GetStringValue(typeMap, typeMap.TypeData, ob));
    }

    protected virtual void WriteEnumElement(
      XmlTypeMapping typeMap,
      object ob,
      string element,
      string namesp)
    {
      this.Writer.WriteString(this.GetEnumXmlValue(typeMap, ob));
    }

    private string GetStringValue(XmlTypeMapping typeMap, TypeData type, object value)
    {
      if (type.SchemaType == SchemaTypes.Array)
      {
        if (value == null)
          return (string) null;
        StringBuilder targetString = new StringBuilder();
        this.WriteListContent((object) null, typeMap.TypeData, (ListMap) typeMap.ObjectMap, value, targetString);
        return targetString.ToString().Trim();
      }
      if (type.SchemaType == SchemaTypes.Enum)
        return this.GetEnumXmlValue(typeMap, value);
      if (type.Type == typeof (XmlQualifiedName))
        return this.FromXmlQualifiedName((XmlQualifiedName) value);
      return value == null ? (string) null : XmlCustomFormatter.ToXmlString(type, value);
    }

    private string GetEnumXmlValue(XmlTypeMapping typeMap, object ob) => ob == null ? (string) null : ((EnumMap) typeMap.ObjectMap).GetXmlName(typeMap.TypeFullName, ob);

    private class CallbackInfo
    {
      private XmlSerializationWriterInterpreter _swi;
      private XmlTypeMapping _typeMap;

      public CallbackInfo(XmlSerializationWriterInterpreter swi, XmlTypeMapping typeMap)
      {
        this._swi = swi;
        this._typeMap = typeMap;
      }

      internal void WriteObject(object ob) => this._swi.WriteObject(this._typeMap, ob, this._typeMap.ElementName, this._typeMap.Namespace, false, false, false);

      internal void WriteEnum(object ob) => this._swi.WriteObject(this._typeMap, ob, this._typeMap.ElementName, this._typeMap.Namespace, false, true, false);
    }
  }
}
