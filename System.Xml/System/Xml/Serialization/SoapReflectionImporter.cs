// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.SoapReflectionImporter
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.Globalization;
using System.Reflection;

namespace System.Xml.Serialization
{
  public class SoapReflectionImporter
  {
    private SoapAttributeOverrides attributeOverrides;
    private string initialDefaultNamespace;
    private ArrayList includedTypes;
    private ArrayList relatedMaps = new ArrayList();
    private ReflectionHelper helper = new ReflectionHelper();

    public SoapReflectionImporter()
      : this((SoapAttributeOverrides) null, (string) null)
    {
    }

    public SoapReflectionImporter(SoapAttributeOverrides attributeOverrides)
      : this(attributeOverrides, (string) null)
    {
    }

    public SoapReflectionImporter(string defaultNamespace)
      : this((SoapAttributeOverrides) null, defaultNamespace)
    {
    }

    public SoapReflectionImporter(
      SoapAttributeOverrides attributeOverrides,
      string defaultNamespace)
    {
      this.initialDefaultNamespace = defaultNamespace != null ? defaultNamespace : string.Empty;
      if (attributeOverrides == null)
        this.attributeOverrides = new SoapAttributeOverrides();
      else
        this.attributeOverrides = attributeOverrides;
    }

    public XmlMembersMapping ImportMembersMapping(
      string elementName,
      string ns,
      XmlReflectionMember[] members)
    {
      return this.ImportMembersMapping(elementName, ns, members, true, true, false);
    }

    public XmlMembersMapping ImportMembersMapping(
      string elementName,
      string ns,
      XmlReflectionMember[] members,
      bool hasWrapperElement,
      bool writeAccessors)
    {
      return this.ImportMembersMapping(elementName, ns, members, hasWrapperElement, writeAccessors, false);
    }

    public XmlMembersMapping ImportMembersMapping(
      string elementName,
      string ns,
      XmlReflectionMember[] members,
      bool hasWrapperElement,
      bool writeAccessors,
      bool validate)
    {
      return this.ImportMembersMapping(elementName, ns, members, hasWrapperElement, writeAccessors, validate, XmlMappingAccess.Read | XmlMappingAccess.Write);
    }

    [MonoTODO]
    public XmlMembersMapping ImportMembersMapping(
      string elementName,
      string ns,
      XmlReflectionMember[] members,
      bool hasWrapperElement,
      bool writeAccessors,
      bool validate,
      XmlMappingAccess access)
    {
      elementName = XmlConvert.EncodeLocalName(elementName);
      XmlMemberMapping[] mapping = new XmlMemberMapping[members.Length];
      for (int index = 0; index < members.Length; ++index)
      {
        XmlTypeMapMember mapMember = this.CreateMapMember(members[index], ns);
        mapping[index] = new XmlMemberMapping(XmlConvert.EncodeLocalName(members[index].MemberName), ns, mapMember, true);
      }
      XmlMembersMapping xmlMembersMapping = new XmlMembersMapping(elementName, ns, hasWrapperElement, writeAccessors, mapping);
      xmlMembersMapping.RelatedMaps = this.relatedMaps;
      xmlMembersMapping.Format = SerializationFormat.Encoded;
      Type[] array = this.includedTypes == null ? (Type[]) null : (Type[]) this.includedTypes.ToArray(typeof (Type));
      xmlMembersMapping.Source = (SerializationSource) new MembersSerializationSource(elementName, hasWrapperElement, members, writeAccessors, false, (string) null, array);
      return xmlMembersMapping;
    }

    public XmlTypeMapping ImportTypeMapping(Type type) => this.ImportTypeMapping(type, (string) null);

    public XmlTypeMapping ImportTypeMapping(Type type, string defaultNamespace)
    {
      if (type == null)
        throw new ArgumentNullException(nameof (type));
      return type != typeof (void) ? this.ImportTypeMapping(TypeTranslator.GetTypeData(type), defaultNamespace) : throw new InvalidOperationException("Type " + type.Name + " may not be serialized.");
    }

    internal XmlTypeMapping ImportTypeMapping(TypeData typeData, string defaultNamespace)
    {
      if (typeData == null)
        throw new ArgumentNullException(nameof (typeData));
      if (typeData.Type == null)
        throw new ArgumentException("Specified TypeData instance does not have Type set.");
      string defaultNamespace1 = this.initialDefaultNamespace;
      if (defaultNamespace == null)
        defaultNamespace = this.initialDefaultNamespace;
      if (defaultNamespace == null)
        defaultNamespace = string.Empty;
      this.initialDefaultNamespace = defaultNamespace;
      XmlTypeMapping xmlTypeMapping;
      switch (typeData.SchemaType)
      {
        case SchemaTypes.Primitive:
          xmlTypeMapping = this.ImportPrimitiveMapping(typeData, defaultNamespace);
          break;
        case SchemaTypes.Enum:
          xmlTypeMapping = this.ImportEnumMapping(typeData, defaultNamespace);
          break;
        case SchemaTypes.Array:
          xmlTypeMapping = this.ImportListMapping(typeData, defaultNamespace);
          break;
        case SchemaTypes.Class:
          xmlTypeMapping = this.ImportClassMapping(typeData, defaultNamespace);
          break;
        case SchemaTypes.XmlNode:
          throw this.CreateTypeException(typeData.Type);
        default:
          throw new NotSupportedException("Type " + typeData.Type.FullName + " not supported for XML serialization");
      }
      xmlTypeMapping.RelatedMaps = this.relatedMaps;
      xmlTypeMapping.Format = SerializationFormat.Encoded;
      Type[] array = this.includedTypes == null ? (Type[]) null : (Type[]) this.includedTypes.ToArray(typeof (Type));
      xmlTypeMapping.Source = (SerializationSource) new SoapTypeSerializationSource(typeData.Type, this.attributeOverrides, defaultNamespace, array);
      this.initialDefaultNamespace = defaultNamespace1;
      return xmlTypeMapping;
    }

    private XmlTypeMapping CreateTypeMapping(
      TypeData typeData,
      string defaultXmlType,
      string defaultNamespace)
    {
      string str = defaultNamespace;
      bool flag = true;
      SoapAttributes soapAttributes = (SoapAttributes) null;
      if (defaultXmlType == null)
        defaultXmlType = typeData.XmlType;
      if (!typeData.IsListType)
      {
        if (this.attributeOverrides != null)
          soapAttributes = this.attributeOverrides[typeData.Type];
        if (soapAttributes != null && typeData.SchemaType == SchemaTypes.Primitive)
          throw new InvalidOperationException("SoapType attribute may not be specified for the type " + typeData.FullTypeName);
      }
      if (soapAttributes == null)
        soapAttributes = new SoapAttributes((ICustomAttributeProvider) typeData.Type);
      if (soapAttributes.SoapType != null)
      {
        if (soapAttributes.SoapType.Namespace != null && soapAttributes.SoapType.Namespace != string.Empty)
          str = soapAttributes.SoapType.Namespace;
        if (soapAttributes.SoapType.TypeName != null && soapAttributes.SoapType.TypeName != string.Empty)
          defaultXmlType = XmlConvert.EncodeLocalName(soapAttributes.SoapType.TypeName);
        flag = soapAttributes.SoapType.IncludeInSchema;
      }
      if (str == null)
        str = string.Empty;
      XmlTypeMapping typeMapping = new XmlTypeMapping(defaultXmlType, str, typeData, defaultXmlType, str);
      typeMapping.IncludeInSchema = flag;
      this.relatedMaps.Add((object) typeMapping);
      return typeMapping;
    }

    private XmlTypeMapping ImportClassMapping(Type type, string defaultNamespace) => this.ImportClassMapping(TypeTranslator.GetTypeData(type), defaultNamespace);

    private XmlTypeMapping ImportClassMapping(TypeData typeData, string defaultNamespace)
    {
      Type type = typeData.Type;
      if (type.IsValueType)
        throw this.CreateStructException(type);
      if (type == typeof (object))
        defaultNamespace = "http://www.w3.org/2001/XMLSchema";
      ReflectionHelper.CheckSerializableType(type, false);
      XmlTypeMapping registeredClrType = this.helper.GetRegisteredClrType(type, this.GetTypeNamespace(typeData, defaultNamespace));
      if (registeredClrType != null)
        return registeredClrType;
      XmlTypeMapping typeMapping = this.CreateTypeMapping(typeData, (string) null, defaultNamespace);
      this.helper.RegisterClrType(typeMapping, type, typeMapping.Namespace);
      typeMapping.MultiReferenceType = true;
      ClassMap classMap = new ClassMap();
      typeMapping.ObjectMap = (ObjectMap) classMap;
      foreach (XmlReflectionMember reflectionMember in (IEnumerable) this.GetReflectionMembers(type))
      {
        if (!reflectionMember.SoapAttributes.SoapIgnore)
          classMap.AddMember(this.CreateMapMember(reflectionMember, defaultNamespace));
      }
      foreach (SoapIncludeAttribute customAttribute in (SoapIncludeAttribute[]) type.GetCustomAttributes(typeof (SoapIncludeAttribute), false))
        this.ImportTypeMapping(customAttribute.Type);
      if (type == typeof (object) && this.includedTypes != null)
      {
        foreach (Type includedType in this.includedTypes)
          typeMapping.DerivedTypes.Add((object) this.ImportTypeMapping(includedType));
      }
      if (type.BaseType != null)
      {
        XmlTypeMapping map = this.ImportClassMapping(type.BaseType, defaultNamespace);
        if (type.BaseType != typeof (object))
          typeMapping.BaseMap = map;
        this.RegisterDerivedMap(map, typeMapping);
      }
      return typeMapping;
    }

    private void RegisterDerivedMap(XmlTypeMapping map, XmlTypeMapping derivedMap)
    {
      map.DerivedTypes.Add((object) derivedMap);
      map.DerivedTypes.AddRange((ICollection) derivedMap.DerivedTypes);
      if (map.BaseMap != null)
      {
        this.RegisterDerivedMap(map.BaseMap, derivedMap);
      }
      else
      {
        XmlTypeMapping xmlTypeMapping = this.ImportTypeMapping(typeof (object));
        if (xmlTypeMapping == map)
          return;
        xmlTypeMapping.DerivedTypes.Add((object) derivedMap);
      }
    }

    private string GetTypeNamespace(TypeData typeData, string defaultNamespace)
    {
      string str = defaultNamespace;
      SoapAttributes soapAttributes = (SoapAttributes) null;
      if (!typeData.IsListType && this.attributeOverrides != null)
        soapAttributes = this.attributeOverrides[typeData.Type];
      if (soapAttributes == null)
        soapAttributes = new SoapAttributes((ICustomAttributeProvider) typeData.Type);
      if (soapAttributes.SoapType != null && soapAttributes.SoapType.Namespace != null && soapAttributes.SoapType.Namespace != string.Empty)
        str = soapAttributes.SoapType.Namespace;
      return str ?? string.Empty;
    }

    private XmlTypeMapping ImportListMapping(TypeData typeData, string defaultNamespace)
    {
      Type type1 = typeData.Type;
      XmlTypeMapping registeredClrType = this.helper.GetRegisteredClrType(type1, "http://schemas.xmlsoap.org/soap/encoding/");
      if (registeredClrType != null)
        return registeredClrType;
      ListMap listMap = new ListMap();
      TypeData listItemTypeData = typeData.ListItemTypeData;
      XmlTypeMapping typeMapping = this.CreateTypeMapping(typeData, "Array", "http://schemas.xmlsoap.org/soap/encoding/");
      this.helper.RegisterClrType(typeMapping, type1, "http://schemas.xmlsoap.org/soap/encoding/");
      typeMapping.MultiReferenceType = true;
      typeMapping.ObjectMap = (ObjectMap) listMap;
      XmlTypeMapElementInfo typeMapElementInfo = new XmlTypeMapElementInfo((XmlTypeMapMember) null, listItemTypeData);
      if (typeMapElementInfo.TypeData.IsComplexType)
      {
        typeMapElementInfo.MappedType = this.ImportTypeMapping(typeData.ListItemType, defaultNamespace);
        typeMapElementInfo.TypeData = typeMapElementInfo.MappedType.TypeData;
      }
      typeMapElementInfo.ElementName = "Item";
      typeMapElementInfo.Namespace = string.Empty;
      typeMapElementInfo.IsNullable = true;
      XmlTypeMapElementInfoList mapElementInfoList = new XmlTypeMapElementInfoList();
      mapElementInfoList.Add((object) typeMapElementInfo);
      listMap.ItemInfo = mapElementInfoList;
      XmlTypeMapping xmlTypeMapping = this.ImportTypeMapping(typeof (object), defaultNamespace);
      xmlTypeMapping.DerivedTypes.Add((object) typeMapping);
      foreach (SoapIncludeAttribute customAttribute in (SoapIncludeAttribute[]) type1.GetCustomAttributes(typeof (SoapIncludeAttribute), false))
      {
        Type type2 = customAttribute.Type;
        xmlTypeMapping.DerivedTypes.Add((object) this.ImportTypeMapping(type2, defaultNamespace));
      }
      return typeMapping;
    }

    private XmlTypeMapping ImportPrimitiveMapping(TypeData typeData, string defaultNamespace)
    {
      if (typeData.SchemaType == SchemaTypes.Primitive)
        defaultNamespace = !typeData.IsXsdType ? "http://microsoft.com/wsdl/types/" : "http://www.w3.org/2001/XMLSchema";
      Type type = typeData.Type;
      XmlTypeMapping registeredClrType = this.helper.GetRegisteredClrType(type, this.GetTypeNamespace(typeData, defaultNamespace));
      if (registeredClrType != null)
        return registeredClrType;
      XmlTypeMapping typeMapping = this.CreateTypeMapping(typeData, (string) null, defaultNamespace);
      this.helper.RegisterClrType(typeMapping, type, typeMapping.Namespace);
      return typeMapping;
    }

    private XmlTypeMapping ImportEnumMapping(TypeData typeData, string defaultNamespace)
    {
      Type type = typeData.Type;
      XmlTypeMapping registeredClrType = this.helper.GetRegisteredClrType(type, this.GetTypeNamespace(typeData, defaultNamespace));
      if (registeredClrType != null)
        return registeredClrType;
      ReflectionHelper.CheckSerializableType(type, false);
      XmlTypeMapping typeMapping = this.CreateTypeMapping(typeData, (string) null, defaultNamespace);
      this.helper.RegisterClrType(typeMapping, type, typeMapping.Namespace);
      typeMapping.MultiReferenceType = true;
      string[] names = Enum.GetNames(type);
      EnumMap.EnumMapMember[] members = new EnumMap.EnumMapMember[names.Length];
      for (int index = 0; index < names.Length; ++index)
      {
        FieldInfo field = type.GetField(names[index]);
        string name = names[index];
        object[] customAttributes = field.GetCustomAttributes(typeof (SoapEnumAttribute), false);
        if (customAttributes.Length > 0)
          name = ((SoapEnumAttribute) customAttributes[0]).Name;
        long int64 = ((IConvertible) field.GetValue((object) null)).ToInt64((IFormatProvider) CultureInfo.InvariantCulture);
        members[index] = new EnumMap.EnumMapMember(XmlConvert.EncodeLocalName(name), names[index], int64);
      }
      bool isFlags = type.IsDefined(typeof (FlagsAttribute), false);
      typeMapping.ObjectMap = (ObjectMap) new EnumMap(members, isFlags);
      this.ImportTypeMapping(typeof (object), defaultNamespace).DerivedTypes.Add((object) typeMapping);
      return typeMapping;
    }

    private ICollection GetReflectionMembers(Type type)
    {
      ArrayList reflectionMembers = new ArrayList();
      foreach (PropertyInfo property in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
      {
        if (property.CanRead && (property.CanWrite || TypeTranslator.GetTypeData(property.PropertyType).SchemaType == SchemaTypes.Array && !property.PropertyType.IsArray))
        {
          SoapAttributes attributes = this.attributeOverrides[type, property.Name] ?? new SoapAttributes((ICustomAttributeProvider) property);
          if (!attributes.SoapIgnore)
          {
            XmlReflectionMember reflectionMember = new XmlReflectionMember(property.Name, property.PropertyType, attributes);
            reflectionMembers.Add((object) reflectionMember);
          }
        }
      }
      foreach (FieldInfo field in type.GetFields(BindingFlags.Instance | BindingFlags.Public))
      {
        SoapAttributes attributes = this.attributeOverrides[type, field.Name] ?? new SoapAttributes((ICustomAttributeProvider) field);
        if (!attributes.SoapIgnore)
        {
          XmlReflectionMember reflectionMember = new XmlReflectionMember(field.Name, field.FieldType, attributes);
          reflectionMembers.Add((object) reflectionMember);
        }
      }
      return (ICollection) reflectionMembers;
    }

    private XmlTypeMapMember CreateMapMember(XmlReflectionMember rmember, string defaultNamespace)
    {
      SoapAttributes soapAttributes = rmember.SoapAttributes;
      TypeData typeData = TypeTranslator.GetTypeData(rmember.MemberType);
      XmlTypeMapMember member;
      if (soapAttributes.SoapAttribute != null)
      {
        if (typeData.SchemaType != SchemaTypes.Enum && typeData.SchemaType != SchemaTypes.Primitive)
          throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Cannot serialize member '{0}' of type {1}. SoapAttribute cannot be used to encode complex types.", (object) rmember.MemberName, (object) typeData.FullTypeName));
        if (soapAttributes.SoapElement != null)
          throw new Exception("SoapAttributeAttribute and SoapElementAttribute cannot be applied to the same member");
        XmlTypeMapMemberAttribute mapMemberAttribute = new XmlTypeMapMemberAttribute();
        mapMemberAttribute.AttributeName = soapAttributes.SoapAttribute.AttributeName.Length != 0 ? XmlConvert.EncodeLocalName(soapAttributes.SoapAttribute.AttributeName) : XmlConvert.EncodeLocalName(rmember.MemberName);
        mapMemberAttribute.Namespace = soapAttributes.SoapAttribute.Namespace == null ? string.Empty : soapAttributes.SoapAttribute.Namespace;
        if (typeData.IsComplexType)
          mapMemberAttribute.MappedType = this.ImportTypeMapping(typeData.Type, defaultNamespace);
        typeData = TypeTranslator.GetTypeData(rmember.MemberType, soapAttributes.SoapAttribute.DataType);
        member = (XmlTypeMapMember) mapMemberAttribute;
        member.DefaultValue = this.GetDefaultValue(typeData, soapAttributes.SoapDefaultValue);
      }
      else
      {
        member = typeData.SchemaType != SchemaTypes.Array ? (XmlTypeMapMember) new XmlTypeMapMemberElement() : (XmlTypeMapMember) new XmlTypeMapMemberList();
        if (soapAttributes.SoapElement != null && soapAttributes.SoapElement.DataType.Length != 0)
          typeData = TypeTranslator.GetTypeData(rmember.MemberType, soapAttributes.SoapElement.DataType);
        XmlTypeMapElementInfoList mapElementInfoList = new XmlTypeMapElementInfoList();
        XmlTypeMapElementInfo typeMapElementInfo = new XmlTypeMapElementInfo(member, typeData);
        typeMapElementInfo.ElementName = XmlConvert.EncodeLocalName(soapAttributes.SoapElement == null || soapAttributes.SoapElement.ElementName.Length == 0 ? rmember.MemberName : soapAttributes.SoapElement.ElementName);
        typeMapElementInfo.Namespace = string.Empty;
        typeMapElementInfo.IsNullable = soapAttributes.SoapElement != null && soapAttributes.SoapElement.IsNullable;
        if (typeData.IsComplexType)
          typeMapElementInfo.MappedType = this.ImportTypeMapping(typeData.Type, defaultNamespace);
        mapElementInfoList.Add((object) typeMapElementInfo);
        ((XmlTypeMapMemberElement) member).ElementInfo = mapElementInfoList;
      }
      member.TypeData = typeData;
      member.Name = rmember.MemberName;
      member.IsReturnValue = rmember.IsReturnValue;
      return member;
    }

    public void IncludeType(Type type)
    {
      if (type == null)
        throw new ArgumentNullException(nameof (type));
      if (this.includedTypes == null)
        this.includedTypes = new ArrayList();
      if (this.includedTypes.Contains((object) type))
        return;
      this.includedTypes.Add((object) type);
    }

    public void IncludeTypes(ICustomAttributeProvider provider)
    {
      foreach (SoapIncludeAttribute customAttribute in provider.GetCustomAttributes(typeof (SoapIncludeAttribute), true))
        this.IncludeType(customAttribute.Type);
    }

    private Exception CreateTypeException(Type type) => (Exception) new NotSupportedException("The type " + type.FullName + " may not be serialized with SOAP-encoded messages. Set the Use for your message to Literal");

    private Exception CreateStructException(Type type) => (Exception) new NotSupportedException("Cannot serialize " + type.FullName + ". Nested structs are not supported with encoded SOAP");

    private object GetDefaultValue(TypeData typeData, object defaultValue)
    {
      if (defaultValue == DBNull.Value || typeData.SchemaType != SchemaTypes.Enum)
        return defaultValue;
      if (typeData.Type != defaultValue.GetType())
        throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Enum {0} cannot be converted to {1}.", (object) defaultValue.GetType().FullName, (object) typeData.FullTypeName));
      if (Enum.Format(typeData.Type, defaultValue, "g") == Enum.Format(typeData.Type, defaultValue, "d"))
        throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Value '{0}' cannot be converted to {1}.", defaultValue, (object) defaultValue.GetType().FullName));
      return defaultValue;
    }
  }
}
