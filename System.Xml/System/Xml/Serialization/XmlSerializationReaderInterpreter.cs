// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.XmlSerializationReaderInterpreter
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.Reflection;

namespace System.Xml.Serialization
{
  internal class XmlSerializationReaderInterpreter : XmlSerializationReader
  {
    private XmlMapping _typeMap;
    private SerializationFormat _format;
    private static readonly XmlQualifiedName AnyType = new XmlQualifiedName("anyType", "http://www.w3.org/2001/XMLSchema");
    private static readonly object[] empty_array = new object[0];

    public XmlSerializationReaderInterpreter(XmlMapping typeMap)
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
        if (typeMap.TypeData.SchemaType == SchemaTypes.Class || typeMap.TypeData.SchemaType == SchemaTypes.Enum)
        {
          XmlSerializationReaderInterpreter.ReaderCallbackInfo readerCallbackInfo = new XmlSerializationReaderInterpreter.ReaderCallbackInfo(this, typeMap);
          this.AddReadCallback(typeMap.XmlType, typeMap.Namespace, typeMap.TypeData.Type, new XmlSerializationReadCallback(readerCallbackInfo.ReadObject));
        }
      }
    }

    protected override void InitIDs()
    {
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

    public object ReadRoot()
    {
      int content = (int) this.Reader.MoveToContent();
      if (!(this._typeMap is XmlTypeMapping))
        return this.ReadMessage((XmlMembersMapping) this._typeMap);
      return this._format == SerializationFormat.Literal ? this.ReadRoot((XmlTypeMapping) this._typeMap) : this.ReadEncodedObject((XmlTypeMapping) this._typeMap);
    }

    private object ReadEncodedObject(XmlTypeMapping typeMap)
    {
      object obj = (object) null;
      int content = (int) this.Reader.MoveToContent();
      if (this.Reader.NodeType == XmlNodeType.Element)
      {
        if (!(this.Reader.LocalName == typeMap.ElementName) || !(this.Reader.NamespaceURI == typeMap.Namespace))
          throw this.CreateUnknownNodeException();
        obj = this.ReadReferencedElement();
      }
      else
        this.UnknownNode((object) null);
      this.ReadReferencedElements();
      return obj;
    }

    protected virtual object ReadMessage(XmlMembersMapping typeMap)
    {
      object[] ob = new object[typeMap.Count];
      if (typeMap.HasWrapperElement)
      {
        ArrayList allMembers = ((ClassMap) typeMap.ObjectMap).AllMembers;
        for (int index = 0; index < allMembers.Count; ++index)
        {
          XmlTypeMapMember member = (XmlTypeMapMember) allMembers[index];
          if (!member.IsReturnValue && member.TypeData.IsValueType)
            this.SetMemberValueFromAttr(member, (object) ob, this.CreateInstance(member.TypeData.Type), true);
        }
        if (this._format == SerializationFormat.Encoded)
        {
          while (this.Reader.NodeType == XmlNodeType.Element)
          {
            string attribute = this.Reader.GetAttribute("root", "http://schemas.xmlsoap.org/soap/encoding/");
            if (attribute != null && !XmlConvert.ToBoolean(attribute))
            {
              this.ReadReferencedElement();
              int content = (int) this.Reader.MoveToContent();
            }
            else
              break;
          }
        }
        while (this.Reader.NodeType != XmlNodeType.EndElement && this.Reader.ReadState == ReadState.Interactive)
        {
          if (this.Reader.IsStartElement(typeMap.ElementName, typeMap.Namespace) || this._format == SerializationFormat.Encoded)
          {
            this.ReadAttributeMembers((ClassMap) typeMap.ObjectMap, (object) ob, true);
            if (this.Reader.IsEmptyElement)
            {
              this.Reader.Skip();
              int content = (int) this.Reader.MoveToContent();
            }
            else
            {
              this.Reader.ReadStartElement();
              this.ReadMembers((ClassMap) typeMap.ObjectMap, (object) ob, true, false);
              this.ReadEndElement();
              break;
            }
          }
          else
          {
            this.UnknownNode((object) null);
            int content = (int) this.Reader.MoveToContent();
          }
        }
      }
      else
        this.ReadMembers((ClassMap) typeMap.ObjectMap, (object) ob, true, this._format == SerializationFormat.Encoded);
      if (this._format == SerializationFormat.Encoded)
        this.ReadReferencedElements();
      return (object) ob;
    }

    private object ReadRoot(XmlTypeMapping rootMap)
    {
      if (rootMap.TypeData.SchemaType == SchemaTypes.XmlNode)
        return this.ReadXmlNodeElement(rootMap, true);
      if (this.Reader.LocalName != rootMap.ElementName || this.Reader.NamespaceURI != rootMap.Namespace)
        throw this.CreateUnknownNodeException();
      return this.ReadObject(rootMap, rootMap.IsNullable, true);
    }

    protected virtual object ReadObject(XmlTypeMapping typeMap, bool isNullable, bool checkType)
    {
      switch (typeMap.TypeData.SchemaType)
      {
        case SchemaTypes.Primitive:
          return this.ReadPrimitiveElement(typeMap, isNullable);
        case SchemaTypes.Enum:
          return this.ReadEnumElement(typeMap, isNullable);
        case SchemaTypes.Array:
          return this.ReadListElement(typeMap, isNullable, (object) null, true);
        case SchemaTypes.Class:
          return this.ReadClassInstance(typeMap, isNullable, checkType);
        case SchemaTypes.XmlSerializable:
          return this.ReadXmlSerializableElement(typeMap, isNullable);
        case SchemaTypes.XmlNode:
          return this.ReadXmlNodeElement(typeMap, isNullable);
        default:
          throw new Exception("Unsupported map type");
      }
    }

    protected virtual object ReadClassInstance(
      XmlTypeMapping typeMap,
      bool isNullable,
      bool checkType)
    {
      if (isNullable && this.ReadNull())
        return (object) null;
      if (checkType)
      {
        XmlQualifiedName xsiType = this.GetXsiType();
        if (xsiType != (XmlQualifiedName) null)
        {
          XmlTypeMapping realElementMap = typeMap.GetRealElementMap(xsiType.Name, xsiType.Namespace);
          if (realElementMap == null)
          {
            if (typeMap.TypeData.Type == typeof (object))
              return this.ReadTypedPrimitive(xsiType);
            throw this.CreateUnknownTypeException(xsiType);
          }
          if (realElementMap != typeMap)
            return this.ReadObject(realElementMap, false, false);
        }
        else if (typeMap.TypeData.Type == typeof (object))
          return this.ReadTypedPrimitive(XmlSerializationReaderInterpreter.AnyType);
      }
      object instance = Activator.CreateInstance(typeMap.TypeData.Type, true);
      this.Reader.MoveToElement();
      bool isEmptyElement = this.Reader.IsEmptyElement;
      this.ReadClassInstanceMembers(typeMap, instance);
      if (isEmptyElement)
        this.Reader.Skip();
      else
        this.ReadEndElement();
      return instance;
    }

    protected virtual void ReadClassInstanceMembers(XmlTypeMapping typeMap, object ob) => this.ReadMembers((ClassMap) typeMap.ObjectMap, ob, false, false);

    private void ReadAttributeMembers(ClassMap map, object ob, bool isValueList)
    {
      XmlTypeMapMember anyAttributeMember = (XmlTypeMapMember) map.DefaultAnyAttributeMember;
      int length = 0;
      object list = (object) null;
      while (this.Reader.MoveToNextAttribute())
      {
        XmlTypeMapMemberAttribute attribute = map.GetAttribute(this.Reader.LocalName, this.Reader.NamespaceURI);
        if (attribute != null)
          this.SetMemberValue((XmlTypeMapMember) attribute, ob, this.GetValueFromXmlString(this.Reader.Value, attribute.TypeData, attribute.MappedType), isValueList);
        else if (this.IsXmlnsAttribute(this.Reader.Name))
        {
          if (map.NamespaceDeclarations != null)
          {
            if (!(this.GetMemberValue((XmlTypeMapMember) map.NamespaceDeclarations, ob, isValueList) is XmlSerializerNamespaces serializerNamespaces))
            {
              serializerNamespaces = new XmlSerializerNamespaces();
              this.SetMemberValue((XmlTypeMapMember) map.NamespaceDeclarations, ob, (object) serializerNamespaces, isValueList);
            }
            if (this.Reader.Prefix == "xmlns")
              serializerNamespaces.Add(this.Reader.LocalName, this.Reader.Value);
            else
              serializerNamespaces.Add(string.Empty, this.Reader.Value);
          }
        }
        else if (anyAttributeMember != null)
        {
          XmlAttribute attr = (XmlAttribute) this.Document.ReadNode(this.Reader);
          this.ParseWsdlArrayType(attr);
          this.AddListValue(anyAttributeMember.TypeData, ref list, length++, (object) attr, true);
        }
        else
          this.ProcessUnknownAttribute(ob);
      }
      if (anyAttributeMember != null)
      {
        object obj = (object) this.ShrinkArray((Array) list, length, anyAttributeMember.TypeData.Type.GetElementType(), true);
        this.SetMemberValue(anyAttributeMember, ob, obj, isValueList);
      }
      this.Reader.MoveToElement();
    }

    private void ReadMembers(ClassMap map, object ob, bool isValueList, bool readByOrder)
    {
      this.ReadAttributeMembers(map, ob, isValueList);
      if (!isValueList)
      {
        this.Reader.MoveToElement();
        if (this.Reader.IsEmptyElement)
        {
          this.SetListMembersDefaults(map, ob, isValueList);
          return;
        }
        this.Reader.ReadStartElement();
      }
      bool[] flagArray = new bool[map.ElementMembers == null ? 0 : map.ElementMembers.Count];
      bool flag = isValueList && this._format == SerializationFormat.Encoded && map.ReturnMember != null;
      int content1 = (int) this.Reader.MoveToContent();
      int[] numArray = (int[]) null;
      object[] objArray1 = (object[]) null;
      object[] objArray2 = (object[]) null;
      XmlSerializationReader.Fixup fixup = (XmlSerializationReader.Fixup) null;
      int num1 = 0;
      int num2 = !readByOrder ? int.MaxValue : (map.ElementMembers == null ? 0 : map.ElementMembers.Count);
      if (map.FlatLists != null)
      {
        numArray = new int[map.FlatLists.Count];
        objArray1 = new object[map.FlatLists.Count];
        foreach (XmlTypeMapMemberExpandable flatList in map.FlatLists)
        {
          if (this.IsReadOnly((XmlTypeMapMember) flatList, flatList.TypeData, ob, isValueList))
            objArray1[flatList.FlatArrayIndex] = flatList.GetValue(ob);
          else if (flatList.TypeData.Type.IsArray)
          {
            objArray1[flatList.FlatArrayIndex] = this.InitializeList(flatList.TypeData);
          }
          else
          {
            object obj = flatList.GetValue(ob);
            if (obj == null)
            {
              obj = this.InitializeList(flatList.TypeData);
              this.SetMemberValue((XmlTypeMapMember) flatList, ob, obj, isValueList);
            }
            objArray1[flatList.FlatArrayIndex] = obj;
          }
          if (flatList.ChoiceMember != null)
          {
            if (objArray2 == null)
              objArray2 = new object[map.FlatLists.Count];
            objArray2[flatList.FlatArrayIndex] = this.InitializeList(flatList.ChoiceTypeData);
          }
        }
      }
      if (this._format == SerializationFormat.Encoded && map.ElementMembers != null)
      {
        XmlSerializationReaderInterpreter.FixupCallbackInfo fixupCallbackInfo = new XmlSerializationReaderInterpreter.FixupCallbackInfo(this, map, isValueList);
        fixup = new XmlSerializationReader.Fixup(ob, new XmlSerializationFixupCallback(fixupCallbackInfo.FixupMembers), map.ElementMembers.Count);
        this.AddFixup(fixup);
      }
      while (this.Reader.NodeType != XmlNodeType.EndElement && num1 < num2)
      {
        if (this.Reader.NodeType == XmlNodeType.Element)
        {
          XmlTypeMapElementInfo element;
          if (readByOrder)
            element = map.GetElement(num1++);
          else if (flag)
          {
            element = (XmlTypeMapElementInfo) ((XmlTypeMapMemberElement) map.ReturnMember).ElementInfo[0];
            flag = false;
          }
          else
            element = map.GetElement(this.Reader.LocalName, this.Reader.NamespaceURI);
          if (element != null && !flagArray[element.Member.Index])
          {
            if (element.Member.GetType() == typeof (XmlTypeMapMemberList))
            {
              if (this._format == SerializationFormat.Encoded && element.MultiReferenceType)
              {
                object obj = this.ReadReferencingElement(out fixup.Ids[element.Member.Index]);
                if (fixup.Ids[element.Member.Index] == null)
                {
                  if (this.IsReadOnly(element.Member, element.TypeData, ob, isValueList))
                    throw this.CreateReadOnlyCollectionException(element.TypeData.FullTypeName);
                  this.SetMemberValue(element.Member, ob, obj, isValueList);
                }
                else if (!element.MappedType.TypeData.Type.IsArray)
                {
                  object collection;
                  if (this.IsReadOnly(element.Member, element.TypeData, ob, isValueList))
                  {
                    collection = this.GetMemberValue(element.Member, ob, isValueList);
                  }
                  else
                  {
                    collection = this.CreateList(element.MappedType.TypeData.Type);
                    this.SetMemberValue(element.Member, ob, collection, isValueList);
                  }
                  this.AddFixup(new XmlSerializationReader.CollectionFixup(collection, new XmlSerializationCollectionFixupCallback(this.FillList), fixup.Ids[element.Member.Index]));
                  fixup.Ids[element.Member.Index] = (string) null;
                }
              }
              else if (this.IsReadOnly(element.Member, element.TypeData, ob, isValueList))
                this.ReadListElement(element.MappedType, element.IsNullable, this.GetMemberValue(element.Member, ob, isValueList), false);
              else if (element.MappedType.TypeData.Type.IsArray)
              {
                object obj = this.ReadListElement(element.MappedType, element.IsNullable, (object) null, true);
                if (obj != null || element.IsNullable)
                  this.SetMemberValue(element.Member, ob, obj, isValueList);
              }
              else
              {
                object list = this.GetMemberValue(element.Member, ob, isValueList);
                if (list == null)
                {
                  list = this.CreateList(element.MappedType.TypeData.Type);
                  this.SetMemberValue(element.Member, ob, list, isValueList);
                }
                this.ReadListElement(element.MappedType, element.IsNullable, list, true);
              }
              flagArray[element.Member.Index] = true;
            }
            else if (element.Member.GetType() == typeof (XmlTypeMapMemberFlatList))
            {
              XmlTypeMapMemberFlatList member = (XmlTypeMapMemberFlatList) element.Member;
              this.AddListValue(member.TypeData, ref objArray1[member.FlatArrayIndex], numArray[member.FlatArrayIndex]++, this.ReadObjectElement(element), !this.IsReadOnly(element.Member, element.TypeData, ob, isValueList));
              if (member.ChoiceMember != null)
                this.AddListValue(member.ChoiceTypeData, ref objArray2[member.FlatArrayIndex], numArray[member.FlatArrayIndex] - 1, element.ChoiceValue, true);
            }
            else if (element.Member.GetType() == typeof (XmlTypeMapMemberAnyElement))
            {
              XmlTypeMapMemberAnyElement member = (XmlTypeMapMemberAnyElement) element.Member;
              if (member.TypeData.IsListType)
                this.AddListValue(member.TypeData, ref objArray1[member.FlatArrayIndex], numArray[member.FlatArrayIndex]++, this.ReadXmlNode(member.TypeData.ListItemTypeData, false), true);
              else
                this.SetMemberValue((XmlTypeMapMember) member, ob, this.ReadXmlNode(member.TypeData, false), isValueList);
            }
            else
            {
              if (element.Member.GetType() != typeof (XmlTypeMapMemberElement))
                throw new InvalidOperationException("Unknown member type");
              flagArray[element.Member.Index] = true;
              if (this._format == SerializationFormat.Encoded)
              {
                object obj = element.Member.TypeData.SchemaType == SchemaTypes.Primitive ? this.ReadReferencingElement(element.Member.TypeData.XmlType, "http://www.w3.org/2001/XMLSchema", out fixup.Ids[element.Member.Index]) : this.ReadReferencingElement(out fixup.Ids[element.Member.Index]);
                if (element.MultiReferenceType)
                {
                  if (fixup.Ids[element.Member.Index] == null)
                    this.SetMemberValue(element.Member, ob, obj, isValueList);
                }
                else if (obj != null)
                  this.SetMemberValue(element.Member, ob, obj, isValueList);
              }
              else
              {
                this.SetMemberValue(element.Member, ob, this.ReadObjectElement(element), isValueList);
                if (element.ChoiceValue != null)
                  ((XmlTypeMapMemberElement) element.Member).SetChoice(ob, element.ChoiceValue);
              }
            }
          }
          else if (map.DefaultAnyElementMember != null)
          {
            XmlTypeMapMemberAnyElement anyElementMember = map.DefaultAnyElementMember;
            if (anyElementMember.TypeData.IsListType)
              this.AddListValue(anyElementMember.TypeData, ref objArray1[anyElementMember.FlatArrayIndex], numArray[anyElementMember.FlatArrayIndex]++, this.ReadXmlNode(anyElementMember.TypeData.ListItemTypeData, false), true);
            else
              this.SetMemberValue((XmlTypeMapMember) anyElementMember, ob, this.ReadXmlNode(anyElementMember.TypeData, false), isValueList);
          }
          else
            this.ProcessUnknownElement(ob);
        }
        else if ((this.Reader.NodeType == XmlNodeType.Text || this.Reader.NodeType == XmlNodeType.CDATA) && map.XmlTextCollector != null)
        {
          if (map.XmlTextCollector is XmlTypeMapMemberExpandable)
          {
            XmlTypeMapMemberExpandable xmlTextCollector = (XmlTypeMapMemberExpandable) map.XmlTextCollector;
            TypeData type = xmlTextCollector is XmlTypeMapMemberFlatList mapMemberFlatList ? mapMemberFlatList.ListMap.FindTextElement().TypeData : xmlTextCollector.TypeData.ListItemTypeData;
            object obj = type.Type != typeof (string) ? this.ReadXmlNode(type, false) : (object) this.Reader.ReadString();
            this.AddListValue(xmlTextCollector.TypeData, ref objArray1[xmlTextCollector.FlatArrayIndex], numArray[xmlTextCollector.FlatArrayIndex]++, obj, true);
          }
          else
          {
            XmlTypeMapMemberElement xmlTextCollector = (XmlTypeMapMemberElement) map.XmlTextCollector;
            XmlTypeMapElementInfo typeMapElementInfo = (XmlTypeMapElementInfo) xmlTextCollector.ElementInfo[0];
            if (typeMapElementInfo.TypeData.Type == typeof (string))
              this.SetMemberValue((XmlTypeMapMember) xmlTextCollector, ob, (object) this.ReadString((string) this.GetMemberValue((XmlTypeMapMember) xmlTextCollector, ob, isValueList)), isValueList);
            else
              this.SetMemberValue((XmlTypeMapMember) xmlTextCollector, ob, this.GetValueFromXmlString(this.Reader.ReadString(), typeMapElementInfo.TypeData, typeMapElementInfo.MappedType), isValueList);
          }
        }
        else
          this.UnknownNode(ob);
        int content2 = (int) this.Reader.MoveToContent();
      }
      if (objArray1 != null)
      {
        foreach (XmlTypeMapMemberExpandable flatList in map.FlatLists)
        {
          object a = objArray1[flatList.FlatArrayIndex];
          if (flatList.TypeData.Type.IsArray)
            a = (object) this.ShrinkArray((Array) a, numArray[flatList.FlatArrayIndex], flatList.TypeData.Type.GetElementType(), true);
          if (!this.IsReadOnly((XmlTypeMapMember) flatList, flatList.TypeData, ob, isValueList) && flatList.TypeData.Type.IsArray)
            this.SetMemberValue((XmlTypeMapMember) flatList, ob, a, isValueList);
        }
      }
      if (objArray2 != null)
      {
        foreach (XmlTypeMapMemberExpandable flatList in map.FlatLists)
        {
          object a = objArray2[flatList.FlatArrayIndex];
          if (a != null)
          {
            object obj = (object) this.ShrinkArray((Array) a, numArray[flatList.FlatArrayIndex], flatList.ChoiceTypeData.Type.GetElementType(), true);
            XmlTypeMapMember.SetValue(ob, flatList.ChoiceMember, obj);
          }
        }
      }
      this.SetListMembersDefaults(map, ob, isValueList);
    }

    private void SetListMembersDefaults(ClassMap map, object ob, bool isValueList)
    {
      if (map.ListMembers == null)
        return;
      ArrayList listMembers = map.ListMembers;
      for (int index = 0; index < listMembers.Count; ++index)
      {
        XmlTypeMapMember member = (XmlTypeMapMember) listMembers[index];
        if (!this.IsReadOnly(member, member.TypeData, ob, isValueList) && this.GetMemberValue(member, ob, isValueList) == null)
          this.SetMemberValue(member, ob, this.InitializeList(member.TypeData), isValueList);
      }
    }

    internal void FixupMembers(ClassMap map, object obfixup, bool isValueList)
    {
      XmlSerializationReader.Fixup fixup = (XmlSerializationReader.Fixup) obfixup;
      ICollection elementMembers = map.ElementMembers;
      string[] ids = fixup.Ids;
      foreach (XmlTypeMapMember member in (IEnumerable) elementMembers)
      {
        if (ids[member.Index] != null)
          this.SetMemberValue(member, fixup.Source, this.GetTarget(ids[member.Index]), isValueList);
      }
    }

    protected virtual void ProcessUnknownAttribute(object target) => this.UnknownNode(target);

    protected virtual void ProcessUnknownElement(object target) => this.UnknownNode(target);

    private bool IsReadOnly(
      XmlTypeMapMember member,
      TypeData memType,
      object ob,
      bool isValueList)
    {
      return !isValueList && member.IsReadOnly(ob.GetType()) || !memType.HasPublicConstructor;
    }

    private void SetMemberValue(
      XmlTypeMapMember member,
      object ob,
      object value,
      bool isValueList)
    {
      if (isValueList)
      {
        ((object[]) ob)[member.GlobalIndex] = value;
      }
      else
      {
        member.SetValue(ob, value);
        if (!member.IsOptionalValueType)
          return;
        member.SetValueSpecified(ob, true);
      }
    }

    private void SetMemberValueFromAttr(
      XmlTypeMapMember member,
      object ob,
      object value,
      bool isValueList)
    {
      if (member.TypeData.Type.IsEnum)
        value = Enum.ToObject(member.TypeData.Type, value);
      this.SetMemberValue(member, ob, value, isValueList);
    }

    private object GetMemberValue(XmlTypeMapMember member, object ob, bool isValueList) => isValueList ? ((object[]) ob)[member.GlobalIndex] : member.GetValue(ob);

    private object ReadObjectElement(XmlTypeMapElementInfo elem)
    {
      switch (elem.TypeData.SchemaType)
      {
        case SchemaTypes.Primitive:
        case SchemaTypes.Enum:
          return this.ReadPrimitiveValue(elem);
        case SchemaTypes.Array:
          return this.ReadListElement(elem.MappedType, elem.IsNullable, (object) null, true);
        case SchemaTypes.Class:
          return this.ReadObject(elem.MappedType, elem.IsNullable, true);
        case SchemaTypes.XmlSerializable:
          return (object) this.ReadSerializable((IXmlSerializable) Activator.CreateInstance(elem.TypeData.Type, true));
        case SchemaTypes.XmlNode:
          return this.ReadXmlNode(elem.TypeData, true);
        default:
          throw new NotSupportedException("Invalid value type");
      }
    }

    private object ReadPrimitiveValue(XmlTypeMapElementInfo elem) => elem.TypeData.Type == typeof (XmlQualifiedName) ? (elem.IsNullable ? (object) this.ReadNullableQualifiedName() : (object) this.ReadElementQualifiedName()) : (elem.IsNullable ? this.GetValueFromXmlString(this.ReadNullableString(), elem.TypeData, elem.MappedType) : this.GetValueFromXmlString(this.Reader.ReadElementString(), elem.TypeData, elem.MappedType));

    private object GetValueFromXmlString(string value, TypeData typeData, XmlTypeMapping typeMap)
    {
      if (typeData.SchemaType == SchemaTypes.Array)
        return this.ReadListString(typeMap, value);
      if (typeData.SchemaType == SchemaTypes.Enum)
        return this.GetEnumValue(typeMap, value);
      return typeData.Type == typeof (XmlQualifiedName) ? (object) this.ToXmlQualifiedName(value) : XmlCustomFormatter.FromXmlString(typeData, value);
    }

    private object ReadListElement(
      XmlTypeMapping typeMap,
      bool isNullable,
      object list,
      bool canCreateInstance)
    {
      Type type = typeMap.TypeData.Type;
      ListMap objectMap = (ListMap) typeMap.ObjectMap;
      if (type.IsArray && this.ReadNull())
        return (object) null;
      if (list == null)
      {
        if (!canCreateInstance || !typeMap.TypeData.HasPublicConstructor)
          throw this.CreateReadOnlyCollectionException(typeMap.TypeFullName);
        list = this.CreateList(type);
      }
      if (this.Reader.IsEmptyElement)
      {
        this.Reader.Skip();
        if (type.IsArray)
          list = (object) this.ShrinkArray((Array) list, 0, type.GetElementType(), false);
        return list;
      }
      int length = 0;
      this.Reader.ReadStartElement();
      int content1 = (int) this.Reader.MoveToContent();
      while (this.Reader.NodeType != XmlNodeType.EndElement)
      {
        if (this.Reader.NodeType == XmlNodeType.Element)
        {
          XmlTypeMapElementInfo element = objectMap.FindElement(this.Reader.LocalName, this.Reader.NamespaceURI);
          if (element != null)
            this.AddListValue(typeMap.TypeData, ref list, length++, this.ReadObjectElement(element), false);
          else
            this.UnknownNode((object) null);
        }
        else
          this.UnknownNode((object) null);
        int content2 = (int) this.Reader.MoveToContent();
      }
      this.ReadEndElement();
      if (type.IsArray)
        list = (object) this.ShrinkArray((Array) list, length, type.GetElementType(), false);
      return list;
    }

    private object ReadListString(XmlTypeMapping typeMap, string values)
    {
      Type type = typeMap.TypeData.Type;
      ListMap objectMap = (ListMap) typeMap.ObjectMap;
      values = values.Trim();
      if (values == string.Empty)
        return (object) Array.CreateInstance(type.GetElementType(), 0);
      string[] strArray = values.Split(' ');
      Array instance = Array.CreateInstance(type.GetElementType(), strArray.Length);
      XmlTypeMapElementInfo typeMapElementInfo = (XmlTypeMapElementInfo) objectMap.ItemInfo[0];
      for (int index = 0; index < strArray.Length; ++index)
        instance.SetValue(this.GetValueFromXmlString(strArray[index], typeMapElementInfo.TypeData, typeMapElementInfo.MappedType), index);
      return (object) instance;
    }

    private void AddListValue(
      TypeData listType,
      ref object list,
      int index,
      object value,
      bool canCreateInstance)
    {
      Type type = listType.Type;
      if (type.IsArray)
      {
        list = (object) this.EnsureArrayIndex((Array) list, index, type.GetElementType());
        ((Array) list).SetValue(value, index);
      }
      else
      {
        if (list == null)
        {
          if (!canCreateInstance)
            throw this.CreateReadOnlyCollectionException(type.FullName);
          list = Activator.CreateInstance(type, true);
        }
        type.GetMethod("Add", new Type[1]
        {
          listType.ListItemType
        }).Invoke(list, new object[1]{ value });
      }
    }

    private object CreateInstance(Type type) => Activator.CreateInstance(type, XmlSerializationReaderInterpreter.empty_array);

    private object CreateList(Type listType) => listType.IsArray ? (object) this.EnsureArrayIndex((Array) null, 0, listType.GetElementType()) : Activator.CreateInstance(listType, true);

    private object InitializeList(TypeData listType) => listType.Type.IsArray ? (object) null : Activator.CreateInstance(listType.Type, true);

    private void FillList(object list, object items) => this.CopyEnumerableList(items, list);

    private void CopyEnumerableList(object source, object dest)
    {
      if (dest == null)
        throw this.CreateReadOnlyCollectionException(source.GetType().FullName);
      object[] parameters = new object[1];
      MethodInfo method = dest.GetType().GetMethod("Add");
      foreach (object obj in (IEnumerable) source)
      {
        parameters[0] = obj;
        method.Invoke(dest, parameters);
      }
    }

    private object ReadXmlNodeElement(XmlTypeMapping typeMap, bool isNullable) => this.ReadXmlNode(typeMap.TypeData, false);

    private object ReadXmlNode(TypeData type, bool wrapped) => type.Type == typeof (XmlDocument) ? (object) this.ReadXmlDocument(wrapped) : (object) this.ReadXmlNode(wrapped);

    private object ReadPrimitiveElement(XmlTypeMapping typeMap, bool isNullable)
    {
      XmlQualifiedName qname = this.GetXsiType();
      if (qname == (XmlQualifiedName) null)
        qname = new XmlQualifiedName(typeMap.XmlType, typeMap.Namespace);
      return this.ReadTypedPrimitive(qname);
    }

    private object ReadEnumElement(XmlTypeMapping typeMap, bool isNullable)
    {
      this.Reader.ReadStartElement();
      object enumValue = this.GetEnumValue(typeMap, this.Reader.ReadString());
      this.ReadEndElement();
      return enumValue;
    }

    private object GetEnumValue(XmlTypeMapping typeMap, string val)
    {
      if (val == null)
        return (object) null;
      return Enum.Parse(typeMap.TypeData.Type, ((EnumMap) typeMap.ObjectMap).GetEnumName(typeMap.TypeFullName, val) ?? throw this.CreateUnknownConstantException(val, typeMap.TypeData.Type));
    }

    private object ReadXmlSerializableElement(XmlTypeMapping typeMap, bool isNullable)
    {
      int content = (int) this.Reader.MoveToContent();
      if (this.Reader.NodeType == XmlNodeType.Element)
      {
        if (this.Reader.LocalName == typeMap.ElementName && this.Reader.NamespaceURI == typeMap.Namespace)
          return (object) this.ReadSerializable((IXmlSerializable) Activator.CreateInstance(typeMap.TypeData.Type, true));
        throw this.CreateUnknownNodeException();
      }
      this.UnknownNode((object) null);
      return (object) null;
    }

    private class FixupCallbackInfo
    {
      private XmlSerializationReaderInterpreter _sri;
      private ClassMap _map;
      private bool _isValueList;

      public FixupCallbackInfo(
        XmlSerializationReaderInterpreter sri,
        ClassMap map,
        bool isValueList)
      {
        this._sri = sri;
        this._map = map;
        this._isValueList = isValueList;
      }

      public void FixupMembers(object fixup) => this._sri.FixupMembers(this._map, fixup, this._isValueList);
    }

    private class ReaderCallbackInfo
    {
      private XmlSerializationReaderInterpreter _sri;
      private XmlTypeMapping _typeMap;

      public ReaderCallbackInfo(XmlSerializationReaderInterpreter sri, XmlTypeMapping typeMap)
      {
        this._sri = sri;
        this._typeMap = typeMap;
      }

      internal object ReadObject() => this._sri.ReadObject(this._typeMap, true, true);
    }
  }
}
