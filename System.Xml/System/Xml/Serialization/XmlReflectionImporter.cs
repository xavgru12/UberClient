// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.XmlReflectionImporter
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Xml.Schema;

namespace System.Xml.Serialization
{
  public class XmlReflectionImporter
  {
    private string initialDefaultNamespace;
    private XmlAttributeOverrides attributeOverrides;
    private ArrayList includedTypes;
    private ReflectionHelper helper = new ReflectionHelper();
    private int arrayChoiceCount = 1;
    private ArrayList relatedMaps = new ArrayList();
    private bool allowPrivateTypes;
    private static readonly string errSimple = "Cannot serialize object of type '{0}'. Base type '{1}' has simpleContent and can be only extended by adding XmlAttribute elements. Please consider changing XmlText member of the base class to string array";
    private static readonly string errSimple2 = "Cannot serialize object of type '{0}'. Consider changing type of XmlText member '{1}' from '{2}' to string or string array";

    public XmlReflectionImporter()
      : this((XmlAttributeOverrides) null, (string) null)
    {
    }

    public XmlReflectionImporter(string defaultNamespace)
      : this((XmlAttributeOverrides) null, defaultNamespace)
    {
    }

    public XmlReflectionImporter(XmlAttributeOverrides attributeOverrides)
      : this(attributeOverrides, (string) null)
    {
    }

    public XmlReflectionImporter(XmlAttributeOverrides attributeOverrides, string defaultNamespace)
    {
      this.initialDefaultNamespace = defaultNamespace != null ? defaultNamespace : string.Empty;
      if (attributeOverrides == null)
        this.attributeOverrides = new XmlAttributeOverrides();
      else
        this.attributeOverrides = attributeOverrides;
    }

    internal bool AllowPrivateTypes
    {
      get => this.allowPrivateTypes;
      set => this.allowPrivateTypes = value;
    }

    public XmlMembersMapping ImportMembersMapping(
      string elementName,
      string ns,
      XmlReflectionMember[] members,
      bool hasWrapperElement)
    {
      return this.ImportMembersMapping(elementName, ns, members, hasWrapperElement, true);
    }

    [MonoTODO]
    public XmlMembersMapping ImportMembersMapping(
      string elementName,
      string ns,
      XmlReflectionMember[] members,
      bool hasWrapperElement,
      bool writeAccessors)
    {
      return this.ImportMembersMapping(elementName, ns, members, hasWrapperElement, writeAccessors, true);
    }

    [MonoTODO]
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
      XmlMemberMapping[] mapping = new XmlMemberMapping[members.Length];
      for (int index = 0; index < members.Length; ++index)
      {
        XmlTypeMapMember mapMember = this.CreateMapMember((Type) null, members[index], ns);
        mapping[index] = new XmlMemberMapping(members[index].MemberName, ns, mapMember, false);
      }
      elementName = XmlConvert.EncodeLocalName(elementName);
      XmlMembersMapping xmlMembersMapping = new XmlMembersMapping(elementName, ns, hasWrapperElement, false, mapping);
      xmlMembersMapping.RelatedMaps = this.relatedMaps;
      xmlMembersMapping.Format = SerializationFormat.Literal;
      Type[] array = this.includedTypes == null ? (Type[]) null : (Type[]) this.includedTypes.ToArray(typeof (Type));
      xmlMembersMapping.Source = (SerializationSource) new MembersSerializationSource(elementName, hasWrapperElement, members, false, true, ns, array);
      if (this.allowPrivateTypes)
        xmlMembersMapping.Source.CanBeGenerated = false;
      return xmlMembersMapping;
    }

    public XmlTypeMapping ImportTypeMapping(Type type) => this.ImportTypeMapping(type, (XmlRootAttribute) null, (string) null);

    public XmlTypeMapping ImportTypeMapping(Type type, string defaultNamespace) => this.ImportTypeMapping(type, (XmlRootAttribute) null, defaultNamespace);

    public XmlTypeMapping ImportTypeMapping(Type type, XmlRootAttribute group) => this.ImportTypeMapping(type, group, (string) null);

    public XmlTypeMapping ImportTypeMapping(
      Type type,
      XmlRootAttribute root,
      string defaultNamespace)
    {
      if (type == null)
        throw new ArgumentNullException(nameof (type));
      if (type == typeof (void))
        throw new NotSupportedException("The type " + type.FullName + " may not be serialized.");
      return this.ImportTypeMapping(TypeTranslator.GetTypeData(type), root, defaultNamespace);
    }

    internal XmlTypeMapping ImportTypeMapping(TypeData typeData, string defaultNamespace) => this.ImportTypeMapping(typeData, (XmlRootAttribute) null, defaultNamespace);

    private XmlTypeMapping ImportTypeMapping(
      TypeData typeData,
      XmlRootAttribute root,
      string defaultNamespace)
    {
      if (typeData == null)
        throw new ArgumentNullException(nameof (typeData));
      if (typeData.Type == null)
        throw new ArgumentException("Specified TypeData instance does not have Type set.");
      if (defaultNamespace == null)
        defaultNamespace = this.initialDefaultNamespace;
      if (defaultNamespace == null)
        defaultNamespace = string.Empty;
      try
      {
        XmlTypeMapping xmlTypeMapping;
        switch (typeData.SchemaType)
        {
          case SchemaTypes.Primitive:
            xmlTypeMapping = this.ImportPrimitiveMapping(typeData, root, defaultNamespace);
            break;
          case SchemaTypes.Enum:
            xmlTypeMapping = this.ImportEnumMapping(typeData, root, defaultNamespace);
            break;
          case SchemaTypes.Array:
            xmlTypeMapping = this.ImportListMapping(typeData, root, defaultNamespace, (XmlAttributes) null, 0);
            break;
          case SchemaTypes.Class:
            xmlTypeMapping = this.ImportClassMapping(typeData, root, defaultNamespace);
            break;
          case SchemaTypes.XmlSerializable:
            xmlTypeMapping = this.ImportXmlSerializableMapping(typeData, root, defaultNamespace);
            break;
          case SchemaTypes.XmlNode:
            xmlTypeMapping = this.ImportXmlNodeMapping(typeData, root, defaultNamespace);
            break;
          default:
            throw new NotSupportedException("Type " + typeData.Type.FullName + " not supported for XML stialization");
        }
        xmlTypeMapping.SetKey(typeData.Type.ToString());
        xmlTypeMapping.RelatedMaps = this.relatedMaps;
        xmlTypeMapping.Format = SerializationFormat.Literal;
        Type[] array = this.includedTypes == null ? (Type[]) null : (Type[]) this.includedTypes.ToArray(typeof (Type));
        xmlTypeMapping.Source = (SerializationSource) new XmlTypeSerializationSource(typeData.Type, root, this.attributeOverrides, defaultNamespace, array);
        if (this.allowPrivateTypes)
          xmlTypeMapping.Source.CanBeGenerated = false;
        return xmlTypeMapping;
      }
      catch (InvalidOperationException ex)
      {
        throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "There was an error reflecting type '{0}'.", (object) typeData.Type.FullName), (Exception) ex);
      }
    }

    private XmlTypeMapping CreateTypeMapping(
      TypeData typeData,
      XmlRootAttribute root,
      string defaultXmlType,
      string defaultNamespace)
    {
      string ns = defaultNamespace;
      string xmlTypeNamespace = (string) null;
      bool flag1 = true;
      XmlAttributes xmlAttributes = (XmlAttributes) null;
      bool flag2 = this.CanBeNull(typeData);
      if (defaultXmlType == null)
        defaultXmlType = typeData.XmlType;
      if (!typeData.IsListType)
      {
        if (this.attributeOverrides != null)
          xmlAttributes = this.attributeOverrides[typeData.Type];
        if (xmlAttributes != null && typeData.SchemaType == SchemaTypes.Primitive)
          throw new InvalidOperationException("XmlRoot and XmlType attributes may not be specified for the type " + typeData.FullTypeName);
      }
      if (xmlAttributes == null)
        xmlAttributes = new XmlAttributes((ICustomAttributeProvider) typeData.Type);
      if (xmlAttributes.XmlRoot != null && root == null)
        root = xmlAttributes.XmlRoot;
      if (xmlAttributes.XmlType != null)
      {
        if (xmlAttributes.XmlType.Namespace != null)
          xmlTypeNamespace = xmlAttributes.XmlType.Namespace;
        if (xmlAttributes.XmlType.TypeName != null && xmlAttributes.XmlType.TypeName != string.Empty)
          defaultXmlType = XmlConvert.EncodeLocalName(xmlAttributes.XmlType.TypeName);
        flag1 = xmlAttributes.XmlType.IncludeInSchema;
      }
      string elementName = defaultXmlType;
      if (root != null)
      {
        if (root.ElementName.Length != 0)
          elementName = XmlConvert.EncodeLocalName(root.ElementName);
        if (root.Namespace != null)
          ns = root.Namespace;
        flag2 = root.IsNullable;
      }
      if (ns == null)
        ns = string.Empty;
      if (xmlTypeNamespace == null)
        xmlTypeNamespace = ns;
      XmlTypeMapping typeMapping;
      switch (typeData.SchemaType)
      {
        case SchemaTypes.Primitive:
          typeMapping = typeData.IsXsdType ? new XmlTypeMapping(elementName, ns, typeData, defaultXmlType, xmlTypeNamespace) : new XmlTypeMapping(elementName, ns, typeData, defaultXmlType, "http://microsoft.com/wsdl/types/");
          break;
        case SchemaTypes.XmlSerializable:
          typeMapping = (XmlTypeMapping) new XmlSerializableMapping(elementName, ns, typeData, defaultXmlType, xmlTypeNamespace);
          break;
        default:
          typeMapping = new XmlTypeMapping(elementName, ns, typeData, defaultXmlType, xmlTypeNamespace);
          break;
      }
      typeMapping.IncludeInSchema = flag1;
      typeMapping.IsNullable = flag2;
      this.relatedMaps.Add((object) typeMapping);
      return typeMapping;
    }

    private XmlTypeMapping ImportClassMapping(
      Type type,
      XmlRootAttribute root,
      string defaultNamespace)
    {
      return this.ImportClassMapping(TypeTranslator.GetTypeData(type), root, defaultNamespace);
    }

    private XmlTypeMapping ImportClassMapping(
      TypeData typeData,
      XmlRootAttribute root,
      string defaultNamespace)
    {
      Type type = typeData.Type;
      XmlTypeMapping registeredClrType = this.helper.GetRegisteredClrType(type, this.GetTypeNamespace(typeData, root, defaultNamespace));
      if (registeredClrType != null)
        return registeredClrType;
      if (!this.allowPrivateTypes)
        ReflectionHelper.CheckSerializableType(type, false);
      XmlTypeMapping typeMapping = this.CreateTypeMapping(typeData, root, (string) null, defaultNamespace);
      this.helper.RegisterClrType(typeMapping, type, typeMapping.XmlTypeNamespace);
      this.helper.RegisterSchemaType(typeMapping, typeMapping.XmlType, typeMapping.XmlTypeNamespace);
      ClassMap classMap = new ClassMap();
      typeMapping.ObjectMap = (ObjectMap) classMap;
      foreach (XmlReflectionMember reflectionMember in (IEnumerable) this.GetReflectionMembers(type))
      {
        string xmlTypeNamespace = typeMapping.XmlTypeNamespace;
        if (!reflectionMember.XmlAttributes.XmlIgnore)
        {
          if (reflectionMember.DeclaringType != null)
          {
            if (reflectionMember.DeclaringType != type)
              xmlTypeNamespace = this.ImportClassMapping(reflectionMember.DeclaringType, root, defaultNamespace).XmlTypeNamespace;
          }
          try
          {
            XmlTypeMapMember mapMember = this.CreateMapMember(type, reflectionMember, xmlTypeNamespace);
            mapMember.CheckOptionalValueType(type);
            classMap.AddMember(mapMember);
          }
          catch (Exception ex)
          {
            throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "There was an error reflecting field '{0}'.", (object) reflectionMember.MemberName), ex);
          }
        }
      }
      if (type == typeof (object) && this.includedTypes != null)
      {
        foreach (Type includedType in this.includedTypes)
          typeMapping.DerivedTypes.Add((object) this.ImportTypeMapping(includedType, defaultNamespace));
      }
      if (type.BaseType != null)
      {
        XmlTypeMapping map = this.ImportClassMapping(type.BaseType, root, defaultNamespace);
        ClassMap objectMap = map.ObjectMap as ClassMap;
        if (type.BaseType != typeof (object))
        {
          typeMapping.BaseMap = map;
          if (!objectMap.HasSimpleContent)
            classMap.SetCanBeSimpleType(false);
        }
        this.RegisterDerivedMap(map, typeMapping);
        if (objectMap.HasSimpleContent && classMap.ElementMembers != null && classMap.ElementMembers.Count != 1)
          throw new InvalidOperationException(string.Format(XmlReflectionImporter.errSimple, (object) typeMapping.TypeData.TypeName, (object) typeMapping.BaseMap.TypeData.TypeName));
      }
      this.ImportIncludedTypes(type, defaultNamespace);
      if (classMap.XmlTextCollector != null && !classMap.HasSimpleContent)
      {
        XmlTypeMapMember xmlTextCollector = classMap.XmlTextCollector;
        if (xmlTextCollector.TypeData.Type != typeof (string) && xmlTextCollector.TypeData.Type != typeof (string[]) && xmlTextCollector.TypeData.Type != typeof (object[]) && xmlTextCollector.TypeData.Type != typeof (XmlNode[]))
          throw new InvalidOperationException(string.Format(XmlReflectionImporter.errSimple2, (object) typeMapping.TypeData.TypeName, (object) xmlTextCollector.Name, (object) xmlTextCollector.TypeData.TypeName));
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

    private string GetTypeNamespace(
      TypeData typeData,
      XmlRootAttribute root,
      string defaultNamespace)
    {
      string typeNamespace = (string) null;
      XmlAttributes xmlAttributes = (XmlAttributes) null;
      if (!typeData.IsListType && this.attributeOverrides != null)
        xmlAttributes = this.attributeOverrides[typeData.Type];
      if (xmlAttributes == null)
        xmlAttributes = new XmlAttributes((ICustomAttributeProvider) typeData.Type);
      if (xmlAttributes.XmlType != null && xmlAttributes.XmlType.Namespace != null && xmlAttributes.XmlType.Namespace.Length != 0 && typeData.SchemaType != SchemaTypes.Enum)
        typeNamespace = xmlAttributes.XmlType.Namespace;
      if (typeNamespace != null && typeNamespace.Length != 0)
        return typeNamespace;
      if (xmlAttributes.XmlRoot != null && root == null)
        root = xmlAttributes.XmlRoot;
      if (root != null && root.Namespace != null && root.Namespace.Length != 0)
        return root.Namespace;
      return defaultNamespace == null ? string.Empty : defaultNamespace;
    }

    private XmlTypeMapping ImportListMapping(
      Type type,
      XmlRootAttribute root,
      string defaultNamespace,
      XmlAttributes atts,
      int nestingLevel)
    {
      return this.ImportListMapping(TypeTranslator.GetTypeData(type), root, defaultNamespace, atts, nestingLevel);
    }

    private XmlTypeMapping ImportListMapping(
      TypeData typeData,
      XmlRootAttribute root,
      string defaultNamespace,
      XmlAttributes atts,
      int nestingLevel)
    {
      Type type1 = typeData.Type;
      ListMap listMap = new ListMap();
      if (!this.allowPrivateTypes)
        ReflectionHelper.CheckSerializableType(type1, true);
      if (atts == null)
        atts = new XmlAttributes();
      Type listItemType = typeData.ListItemType;
      bool flag = type1.IsArray && TypeTranslator.GetTypeData(listItemType).SchemaType == SchemaTypes.Array && listItemType.IsArray;
      XmlTypeMapElementInfoList mapElementInfoList = new XmlTypeMapElementInfoList();
      foreach (XmlArrayItemAttribute xmlArrayItem in (CollectionBase) atts.XmlArrayItems)
      {
        if (xmlArrayItem.Namespace != null && xmlArrayItem.Form == XmlSchemaForm.Unqualified)
          throw new InvalidOperationException("XmlArrayItemAttribute.Form must not be Unqualified when it has an explicit Namespace value.");
        if (xmlArrayItem.NestingLevel == nestingLevel)
        {
          Type type2 = xmlArrayItem.Type == null ? listItemType : xmlArrayItem.Type;
          XmlTypeMapElementInfo typeMapElementInfo = new XmlTypeMapElementInfo((XmlTypeMapMember) null, TypeTranslator.GetTypeData(type2, xmlArrayItem.DataType));
          typeMapElementInfo.Namespace = xmlArrayItem.Namespace == null ? defaultNamespace : xmlArrayItem.Namespace;
          if (typeMapElementInfo.Namespace == null)
            typeMapElementInfo.Namespace = string.Empty;
          typeMapElementInfo.Form = xmlArrayItem.Form;
          if (xmlArrayItem.Form == XmlSchemaForm.Unqualified)
            typeMapElementInfo.Namespace = string.Empty;
          typeMapElementInfo.IsNullable = xmlArrayItem.IsNullable && this.CanBeNull(typeMapElementInfo.TypeData);
          typeMapElementInfo.NestingLevel = xmlArrayItem.NestingLevel;
          if (flag)
            typeMapElementInfo.MappedType = this.ImportListMapping(type2, (XmlRootAttribute) null, typeMapElementInfo.Namespace, atts, nestingLevel + 1);
          else if (typeMapElementInfo.TypeData.IsComplexType)
            typeMapElementInfo.MappedType = this.ImportTypeMapping(type2, (XmlRootAttribute) null, typeMapElementInfo.Namespace);
          typeMapElementInfo.ElementName = xmlArrayItem.ElementName.Length == 0 ? (typeMapElementInfo.MappedType == null ? TypeTranslator.GetTypeData(type2).XmlType : typeMapElementInfo.MappedType.ElementName) : XmlConvert.EncodeLocalName(xmlArrayItem.ElementName);
          mapElementInfoList.Add((object) typeMapElementInfo);
        }
      }
      if (mapElementInfoList.Count == 0)
      {
        XmlTypeMapElementInfo typeMapElementInfo = new XmlTypeMapElementInfo((XmlTypeMapMember) null, TypeTranslator.GetTypeData(listItemType));
        if (flag)
          typeMapElementInfo.MappedType = this.ImportListMapping(listItemType, (XmlRootAttribute) null, defaultNamespace, atts, nestingLevel + 1);
        else if (typeMapElementInfo.TypeData.IsComplexType)
          typeMapElementInfo.MappedType = this.ImportTypeMapping(listItemType, (XmlRootAttribute) null, defaultNamespace);
        typeMapElementInfo.ElementName = typeMapElementInfo.MappedType == null ? TypeTranslator.GetTypeData(listItemType).XmlType : typeMapElementInfo.MappedType.XmlType;
        typeMapElementInfo.Namespace = defaultNamespace == null ? string.Empty : defaultNamespace;
        typeMapElementInfo.IsNullable = this.CanBeNull(typeMapElementInfo.TypeData);
        mapElementInfoList.Add((object) typeMapElementInfo);
      }
      listMap.ItemInfo = mapElementInfoList;
      string str1;
      if (mapElementInfoList.Count > 1)
      {
        str1 = "ArrayOfChoice" + (object) this.arrayChoiceCount++;
      }
      else
      {
        XmlTypeMapElementInfo typeMapElementInfo = (XmlTypeMapElementInfo) mapElementInfoList[0];
        str1 = typeMapElementInfo.MappedType == null ? TypeTranslator.GetArrayName(typeMapElementInfo.ElementName) : TypeTranslator.GetArrayName(typeMapElementInfo.MappedType.XmlType);
      }
      int num = 1;
      string str2 = str1;
      do
      {
        XmlTypeMapping registeredSchemaType = this.helper.GetRegisteredSchemaType(str2, defaultNamespace);
        if (registeredSchemaType == null)
        {
          num = -1;
        }
        else
        {
          if (listMap.Equals((object) registeredSchemaType.ObjectMap) && typeData.Type == registeredSchemaType.TypeData.Type)
            return registeredSchemaType;
          str2 = str1 + (object) num++;
        }
      }
      while (num != -1);
      XmlTypeMapping typeMapping = this.CreateTypeMapping(typeData, root, str2, defaultNamespace);
      typeMapping.ObjectMap = (ObjectMap) listMap;
      XmlIncludeAttribute[] customAttributes = (XmlIncludeAttribute[]) type1.GetCustomAttributes(typeof (XmlIncludeAttribute), false);
      XmlTypeMapping xmlTypeMapping = this.ImportTypeMapping(typeof (object));
      for (int index = 0; index < customAttributes.Length; ++index)
      {
        Type type3 = customAttributes[index].Type;
        xmlTypeMapping.DerivedTypes.Add((object) this.ImportTypeMapping(type3, (XmlRootAttribute) null, defaultNamespace));
      }
      this.helper.RegisterSchemaType(typeMapping, str2, defaultNamespace);
      this.ImportTypeMapping(typeof (object)).DerivedTypes.Add((object) typeMapping);
      return typeMapping;
    }

    private XmlTypeMapping ImportXmlNodeMapping(
      TypeData typeData,
      XmlRootAttribute root,
      string defaultNamespace)
    {
      Type type = typeData.Type;
      XmlTypeMapping registeredClrType = this.helper.GetRegisteredClrType(type, this.GetTypeNamespace(typeData, root, defaultNamespace));
      if (registeredClrType != null)
        return registeredClrType;
      XmlTypeMapping typeMapping = this.CreateTypeMapping(typeData, root, (string) null, defaultNamespace);
      this.helper.RegisterClrType(typeMapping, type, typeMapping.XmlTypeNamespace);
      if (type.BaseType != null)
      {
        XmlTypeMapping map = this.ImportTypeMapping(type.BaseType, root, defaultNamespace);
        if (type.BaseType != typeof (object))
          typeMapping.BaseMap = map;
        this.RegisterDerivedMap(map, typeMapping);
      }
      return typeMapping;
    }

    private XmlTypeMapping ImportPrimitiveMapping(
      TypeData typeData,
      XmlRootAttribute root,
      string defaultNamespace)
    {
      Type type = typeData.Type;
      XmlTypeMapping registeredClrType = this.helper.GetRegisteredClrType(type, this.GetTypeNamespace(typeData, root, defaultNamespace));
      if (registeredClrType != null)
        return registeredClrType;
      XmlTypeMapping typeMapping = this.CreateTypeMapping(typeData, root, (string) null, defaultNamespace);
      this.helper.RegisterClrType(typeMapping, type, typeMapping.XmlTypeNamespace);
      return typeMapping;
    }

    private XmlTypeMapping ImportEnumMapping(
      TypeData typeData,
      XmlRootAttribute root,
      string defaultNamespace)
    {
      Type type = typeData.Type;
      XmlTypeMapping registeredClrType = this.helper.GetRegisteredClrType(type, this.GetTypeNamespace(typeData, root, defaultNamespace));
      if (registeredClrType != null)
        return registeredClrType;
      if (!this.allowPrivateTypes)
        ReflectionHelper.CheckSerializableType(type, false);
      XmlTypeMapping typeMapping = this.CreateTypeMapping(typeData, root, (string) null, defaultNamespace);
      typeMapping.IsNullable = false;
      this.helper.RegisterClrType(typeMapping, type, typeMapping.XmlTypeNamespace);
      string[] names = Enum.GetNames(type);
      ArrayList arrayList = new ArrayList();
      foreach (string str in names)
      {
        FieldInfo field = type.GetField(str);
        string xmlName = (string) null;
        if (!field.IsDefined(typeof (XmlIgnoreAttribute), false))
        {
          object[] customAttributes = field.GetCustomAttributes(typeof (XmlEnumAttribute), false);
          if (customAttributes.Length > 0)
            xmlName = ((XmlEnumAttribute) customAttributes[0]).Name;
          if (xmlName == null)
            xmlName = str;
          long int64 = ((IConvertible) field.GetValue((object) null)).ToInt64((IFormatProvider) CultureInfo.InvariantCulture);
          arrayList.Add((object) new EnumMap.EnumMapMember(xmlName, str, int64));
        }
      }
      bool isFlags = type.IsDefined(typeof (FlagsAttribute), false);
      typeMapping.ObjectMap = (ObjectMap) new EnumMap((EnumMap.EnumMapMember[]) arrayList.ToArray(typeof (EnumMap.EnumMapMember)), isFlags);
      this.ImportTypeMapping(typeof (object)).DerivedTypes.Add((object) typeMapping);
      return typeMapping;
    }

    private XmlTypeMapping ImportXmlSerializableMapping(
      TypeData typeData,
      XmlRootAttribute root,
      string defaultNamespace)
    {
      Type type = typeData.Type;
      XmlTypeMapping registeredClrType = this.helper.GetRegisteredClrType(type, this.GetTypeNamespace(typeData, root, defaultNamespace));
      if (registeredClrType != null)
        return registeredClrType;
      if (!this.allowPrivateTypes)
        ReflectionHelper.CheckSerializableType(type, false);
      XmlTypeMapping typeMapping = this.CreateTypeMapping(typeData, root, (string) null, defaultNamespace);
      this.helper.RegisterClrType(typeMapping, type, typeMapping.XmlTypeNamespace);
      return typeMapping;
    }

    private void ImportIncludedTypes(Type type, string defaultNamespace)
    {
      foreach (XmlIncludeAttribute customAttribute in (XmlIncludeAttribute[]) type.GetCustomAttributes(typeof (XmlIncludeAttribute), false))
        this.ImportTypeMapping(customAttribute.Type, (XmlRootAttribute) null, defaultNamespace);
    }

    private ICollection GetReflectionMembers(Type type)
    {
      Type type1 = type;
      ArrayList arrayList1 = new ArrayList();
      arrayList1.Add((object) type1);
      while (type1 != typeof (object))
      {
        type1 = type1.BaseType;
        arrayList1.Insert(0, (object) type1);
      }
      ArrayList arrayList2 = new ArrayList();
      FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
      Type type2 = (Type) null;
      int num1 = 0;
      foreach (FieldInfo fieldInfo in fields)
      {
        if (type2 != fieldInfo.DeclaringType)
        {
          type2 = fieldInfo.DeclaringType;
          num1 = 0;
        }
        arrayList2.Insert(num1++, (object) fieldInfo);
      }
      ArrayList arrayList3 = new ArrayList();
      PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
      Type type3 = (Type) null;
      int num2 = 0;
      foreach (PropertyInfo propertyInfo in properties)
      {
        if (type3 != propertyInfo.DeclaringType)
        {
          type3 = propertyInfo.DeclaringType;
          num2 = 0;
        }
        if (propertyInfo.CanRead && propertyInfo.GetIndexParameters().Length <= 0)
          arrayList3.Insert(num2++, (object) propertyInfo);
      }
      ArrayList reflectionMembers = new ArrayList();
      int index1 = 0;
      int index2 = 0;
      foreach (Type type4 in arrayList1)
      {
        while (index1 < arrayList2.Count)
        {
          FieldInfo provider = (FieldInfo) arrayList2[index1];
          if (provider.DeclaringType == type4)
          {
            ++index1;
            XmlAttributes attributes = this.attributeOverrides[type, provider.Name] ?? new XmlAttributes((ICustomAttributeProvider) provider);
            if (!attributes.XmlIgnore)
              reflectionMembers.Add((object) new XmlReflectionMember(provider.Name, provider.FieldType, attributes)
              {
                DeclaringType = provider.DeclaringType
              });
          }
          else
            break;
        }
        while (index2 < arrayList3.Count)
        {
          PropertyInfo provider = (PropertyInfo) arrayList3[index2];
          if (provider.DeclaringType == type4)
          {
            ++index2;
            XmlAttributes attributes = this.attributeOverrides[type, provider.Name] ?? new XmlAttributes((ICustomAttributeProvider) provider);
            if (!attributes.XmlIgnore && (provider.CanWrite || TypeTranslator.GetTypeData(provider.PropertyType).SchemaType == SchemaTypes.Array && !provider.PropertyType.IsArray))
              reflectionMembers.Add((object) new XmlReflectionMember(provider.Name, provider.PropertyType, attributes)
              {
                DeclaringType = provider.DeclaringType
              });
          }
          else
            break;
        }
      }
      return (ICollection) reflectionMembers;
    }

    private XmlTypeMapMember CreateMapMember(
      Type declaringType,
      XmlReflectionMember rmember,
      string defaultNamespace)
    {
      XmlAttributes xmlAttributes = rmember.XmlAttributes;
      TypeData typeData = TypeTranslator.GetTypeData(rmember.MemberType);
      if (xmlAttributes.XmlArray != null)
      {
        if (xmlAttributes.XmlArray.Namespace != null && xmlAttributes.XmlArray.Form == XmlSchemaForm.Unqualified)
          throw new InvalidOperationException("XmlArrayAttribute.Form must not be Unqualified when it has an explicit Namespace value.");
        if (typeData.SchemaType != SchemaTypes.Array && (typeData.SchemaType != SchemaTypes.Primitive || typeData.Type != typeof (byte[])))
          throw new InvalidOperationException("XmlArrayAttribute can be applied to members of array or collection type.");
      }
      XmlTypeMapMember mapMember;
      if (xmlAttributes.XmlAnyAttribute != null)
      {
        if (!(rmember.MemberType.FullName == "System.Xml.XmlAttribute[]") && !(rmember.MemberType.FullName == "System.Xml.XmlNode[]"))
          throw new InvalidOperationException("XmlAnyAttributeAttribute can only be applied to members of type XmlAttribute[] or XmlNode[]");
        mapMember = (XmlTypeMapMember) new XmlTypeMapMemberAnyAttribute();
      }
      else if (xmlAttributes.XmlAnyElements != null && xmlAttributes.XmlAnyElements.Count > 0)
      {
        if (!(rmember.MemberType.FullName == "System.Xml.XmlElement[]") && !(rmember.MemberType.FullName == "System.Xml.XmlNode[]") && !(rmember.MemberType.FullName == "System.Xml.XmlElement"))
          throw new InvalidOperationException("XmlAnyElementAttribute can only be applied to members of type XmlElement, XmlElement[] or XmlNode[]");
        XmlTypeMapMemberAnyElement member = new XmlTypeMapMemberAnyElement();
        member.ElementInfo = this.ImportAnyElementInfo(defaultNamespace, rmember, (XmlTypeMapMemberElement) member, xmlAttributes);
        mapMember = (XmlTypeMapMember) member;
      }
      else if (xmlAttributes.Xmlns)
        mapMember = (XmlTypeMapMember) new XmlTypeMapMemberNamespaces();
      else if (xmlAttributes.XmlAttribute != null)
      {
        if (xmlAttributes.XmlElements != null && xmlAttributes.XmlElements.Count > 0)
          throw new Exception("XmlAttributeAttribute and XmlElementAttribute cannot be applied to the same member");
        XmlTypeMapMemberAttribute mapMemberAttribute = new XmlTypeMapMemberAttribute()
        {
          AttributeName = xmlAttributes.XmlAttribute.AttributeName.Length != 0 ? xmlAttributes.XmlAttribute.AttributeName : rmember.MemberName
        };
        mapMemberAttribute.AttributeName = XmlConvert.EncodeLocalName(mapMemberAttribute.AttributeName);
        if (typeData.IsComplexType)
          mapMemberAttribute.MappedType = this.ImportTypeMapping(typeData.Type, (XmlRootAttribute) null, defaultNamespace);
        if (xmlAttributes.XmlAttribute.Namespace != null && xmlAttributes.XmlAttribute.Namespace != defaultNamespace)
        {
          if (xmlAttributes.XmlAttribute.Form == XmlSchemaForm.Unqualified)
            throw new InvalidOperationException("The Form property may not be 'Unqualified' when an explicit Namespace property is present");
          mapMemberAttribute.Form = XmlSchemaForm.Qualified;
          mapMemberAttribute.Namespace = xmlAttributes.XmlAttribute.Namespace;
        }
        else
        {
          mapMemberAttribute.Form = xmlAttributes.XmlAttribute.Form;
          mapMemberAttribute.Namespace = xmlAttributes.XmlAttribute.Form != XmlSchemaForm.Qualified ? string.Empty : defaultNamespace;
        }
        typeData = TypeTranslator.GetTypeData(rmember.MemberType, xmlAttributes.XmlAttribute.DataType);
        mapMember = (XmlTypeMapMember) mapMemberAttribute;
      }
      else if (typeData.SchemaType == SchemaTypes.Array)
      {
        if (xmlAttributes.XmlElements.Count > 1 || xmlAttributes.XmlElements.Count == 1 && xmlAttributes.XmlElements[0].Type != typeData.Type || xmlAttributes.XmlText != null)
        {
          if (xmlAttributes.XmlArray != null)
            throw new InvalidOperationException("XmlArrayAttribute cannot be used with members which also attributed with XmlElementAttribute or XmlTextAttribute.");
          XmlTypeMapMemberFlatList member = new XmlTypeMapMemberFlatList()
          {
            ListMap = new ListMap()
          };
          member.ListMap.ItemInfo = this.ImportElementInfo(declaringType, XmlConvert.EncodeLocalName(rmember.MemberName), defaultNamespace, typeData.ListItemType, (XmlTypeMapMemberElement) member, xmlAttributes);
          member.ElementInfo = member.ListMap.ItemInfo;
          member.ListMap.ChoiceMember = member.ChoiceMember;
          mapMember = (XmlTypeMapMember) member;
        }
        else
        {
          XmlTypeMapMemberList member = new XmlTypeMapMemberList();
          member.ElementInfo = new XmlTypeMapElementInfoList();
          XmlTypeMapElementInfo typeMapElementInfo = new XmlTypeMapElementInfo((XmlTypeMapMember) member, typeData)
          {
            ElementName = XmlConvert.EncodeLocalName(xmlAttributes.XmlArray == null || xmlAttributes.XmlArray.ElementName.Length == 0 ? rmember.MemberName : xmlAttributes.XmlArray.ElementName),
            Namespace = xmlAttributes.XmlArray == null || xmlAttributes.XmlArray.Namespace == null ? defaultNamespace : xmlAttributes.XmlArray.Namespace
          };
          typeMapElementInfo.MappedType = this.ImportListMapping(rmember.MemberType, (XmlRootAttribute) null, typeMapElementInfo.Namespace, xmlAttributes, 0);
          typeMapElementInfo.IsNullable = xmlAttributes.XmlArray != null && xmlAttributes.XmlArray.IsNullable;
          typeMapElementInfo.Form = xmlAttributes.XmlArray == null ? XmlSchemaForm.Qualified : xmlAttributes.XmlArray.Form;
          if (xmlAttributes.XmlArray != null && xmlAttributes.XmlArray.Form == XmlSchemaForm.Unqualified)
            typeMapElementInfo.Namespace = string.Empty;
          member.ElementInfo.Add((object) typeMapElementInfo);
          mapMember = (XmlTypeMapMember) member;
        }
      }
      else
      {
        XmlTypeMapMemberElement member = new XmlTypeMapMemberElement();
        member.ElementInfo = this.ImportElementInfo(declaringType, XmlConvert.EncodeLocalName(rmember.MemberName), defaultNamespace, rmember.MemberType, member, xmlAttributes);
        mapMember = (XmlTypeMapMember) member;
      }
      mapMember.DefaultValue = this.GetDefaultValue(typeData, xmlAttributes.XmlDefaultValue);
      mapMember.TypeData = typeData;
      mapMember.Name = rmember.MemberName;
      mapMember.IsReturnValue = rmember.IsReturnValue;
      return mapMember;
    }

    private XmlTypeMapElementInfoList ImportElementInfo(
      Type cls,
      string defaultName,
      string defaultNamespace,
      Type defaultType,
      XmlTypeMapMemberElement member,
      XmlAttributes atts)
    {
      enumMap = (EnumMap) null;
      Type type1 = (Type) null;
      XmlTypeMapElementInfoList list = new XmlTypeMapElementInfoList();
      this.ImportTextElementInfo(list, defaultType, member, atts, defaultNamespace);
      if (atts.XmlChoiceIdentifier != null)
      {
        if (cls == null)
          throw new InvalidOperationException("XmlChoiceIdentifierAttribute not supported in this context.");
        member.ChoiceMember = atts.XmlChoiceIdentifier.MemberName;
        MemberInfo[] member1 = cls.GetMember(member.ChoiceMember, BindingFlags.Instance | BindingFlags.Public);
        if (member1.Length == 0)
          throw new InvalidOperationException("Choice member '" + member.ChoiceMember + "' not found in class '" + (object) cls);
        if (member1[0] is PropertyInfo)
        {
          PropertyInfo propertyInfo = (PropertyInfo) member1[0];
          if (!propertyInfo.CanWrite || !propertyInfo.CanRead)
            throw new InvalidOperationException("Choice property '" + member.ChoiceMember + "' must be read/write.");
          type1 = propertyInfo.PropertyType;
        }
        else
          type1 = ((FieldInfo) member1[0]).FieldType;
        member.ChoiceTypeData = TypeTranslator.GetTypeData(type1);
        if (type1.IsArray)
          type1 = type1.GetElementType();
        if (!(this.ImportTypeMapping(type1).ObjectMap is EnumMap enumMap))
          throw new InvalidOperationException("The member '" + member1[0].Name + "' is not a valid target for XmlChoiceIdentifierAttribute.");
      }
      if (atts.XmlElements.Count == 0 && list.Count == 0)
      {
        XmlTypeMapElementInfo typeMapElementInfo = new XmlTypeMapElementInfo((XmlTypeMapMember) member, TypeTranslator.GetTypeData(defaultType));
        typeMapElementInfo.ElementName = defaultName;
        typeMapElementInfo.Namespace = defaultNamespace;
        if (typeMapElementInfo.TypeData.IsComplexType)
          typeMapElementInfo.MappedType = this.ImportTypeMapping(defaultType, (XmlRootAttribute) null, defaultNamespace);
        list.Add((object) typeMapElementInfo);
      }
      bool flag = atts.XmlElements.Count > 1;
      foreach (XmlElementAttribute xmlElement in (CollectionBase) atts.XmlElements)
      {
        Type type2 = xmlElement.Type == null ? defaultType : xmlElement.Type;
        XmlTypeMapElementInfo typeMapElementInfo = new XmlTypeMapElementInfo((XmlTypeMapMember) member, TypeTranslator.GetTypeData(type2, xmlElement.DataType));
        typeMapElementInfo.Form = xmlElement.Form;
        if (typeMapElementInfo.Form != XmlSchemaForm.Unqualified)
          typeMapElementInfo.Namespace = xmlElement.Namespace == null ? defaultNamespace : xmlElement.Namespace;
        typeMapElementInfo.IsNullable = xmlElement.IsNullable;
        if (typeMapElementInfo.IsNullable && !typeMapElementInfo.TypeData.IsNullable)
          throw new InvalidOperationException("IsNullable may not be 'true' for value type " + typeMapElementInfo.TypeData.FullTypeName + " in member '" + defaultName + "'");
        if (typeMapElementInfo.TypeData.IsComplexType)
        {
          if (xmlElement.DataType.Length != 0)
            throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "'{0}' is an invalid value for '{1}.{2}' of type '{3}'. The property may only be specified for primitive types.", (object) xmlElement.DataType, (object) cls.FullName, (object) defaultName, (object) typeMapElementInfo.TypeData.FullTypeName));
          typeMapElementInfo.MappedType = this.ImportTypeMapping(type2, (XmlRootAttribute) null, typeMapElementInfo.Namespace);
        }
        typeMapElementInfo.ElementName = xmlElement.ElementName.Length == 0 ? (!flag ? defaultName : (typeMapElementInfo.MappedType == null ? TypeTranslator.GetTypeData(type2).XmlType : typeMapElementInfo.MappedType.ElementName)) : XmlConvert.EncodeLocalName(xmlElement.ElementName);
        if (enumMap != null)
          typeMapElementInfo.ChoiceValue = Enum.Parse(type1, enumMap.GetEnumName(type1.FullName, typeMapElementInfo.ElementName) ?? throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Type {0} is missing enumeration value '{1}' for element '{1} from namespace '{2}'.", (object) type1, (object) typeMapElementInfo.ElementName, (object) typeMapElementInfo.Namespace)));
        list.Add((object) typeMapElementInfo);
      }
      return list;
    }

    private XmlTypeMapElementInfoList ImportAnyElementInfo(
      string defaultNamespace,
      XmlReflectionMember rmember,
      XmlTypeMapMemberElement member,
      XmlAttributes atts)
    {
      XmlTypeMapElementInfoList list = new XmlTypeMapElementInfoList();
      this.ImportTextElementInfo(list, rmember.MemberType, member, atts, defaultNamespace);
      foreach (XmlAnyElementAttribute xmlAnyElement in (CollectionBase) atts.XmlAnyElements)
      {
        XmlTypeMapElementInfo typeMapElementInfo = new XmlTypeMapElementInfo((XmlTypeMapMember) member, TypeTranslator.GetTypeData(typeof (XmlElement)));
        if (xmlAnyElement.Name.Length != 0)
        {
          typeMapElementInfo.ElementName = XmlConvert.EncodeLocalName(xmlAnyElement.Name);
          typeMapElementInfo.Namespace = xmlAnyElement.Namespace == null ? string.Empty : xmlAnyElement.Namespace;
        }
        else
        {
          typeMapElementInfo.IsUnnamedAnyElement = true;
          typeMapElementInfo.Namespace = defaultNamespace;
          if (xmlAnyElement.Namespace != null)
            throw new InvalidOperationException("The element " + rmember.MemberName + " has been attributed with an XmlAnyElementAttribute and a namespace '" + xmlAnyElement.Namespace + "', but no name. When a namespace is supplied, a name is also required. Supply a name or remove the namespace.");
        }
        list.Add((object) typeMapElementInfo);
      }
      return list;
    }

    private void ImportTextElementInfo(
      XmlTypeMapElementInfoList list,
      Type defaultType,
      XmlTypeMapMemberElement member,
      XmlAttributes atts,
      string defaultNamespace)
    {
      if (atts.XmlText == null)
        return;
      member.IsXmlTextCollector = true;
      if (atts.XmlText.Type != null)
      {
        TypeData typeData = TypeTranslator.GetTypeData(defaultType);
        if ((typeData.SchemaType == SchemaTypes.Primitive || typeData.SchemaType == SchemaTypes.Enum) && atts.XmlText.Type != defaultType)
          throw new InvalidOperationException("The type for XmlText may not be specified for primitive types.");
        defaultType = atts.XmlText.Type;
      }
      if (defaultType == typeof (XmlNode))
        defaultType = typeof (XmlText);
      XmlTypeMapElementInfo typeMapElementInfo = new XmlTypeMapElementInfo((XmlTypeMapMember) member, TypeTranslator.GetTypeData(defaultType, atts.XmlText.DataType));
      if (typeMapElementInfo.TypeData.SchemaType != SchemaTypes.Primitive && typeMapElementInfo.TypeData.SchemaType != SchemaTypes.Enum && typeMapElementInfo.TypeData.SchemaType != SchemaTypes.XmlNode && (typeMapElementInfo.TypeData.SchemaType != SchemaTypes.Array || typeMapElementInfo.TypeData.ListItemTypeData.SchemaType != SchemaTypes.XmlNode))
        throw new InvalidOperationException("XmlText cannot be used to encode complex types");
      if (typeMapElementInfo.TypeData.IsComplexType)
        typeMapElementInfo.MappedType = this.ImportTypeMapping(defaultType, (XmlRootAttribute) null, defaultNamespace);
      typeMapElementInfo.IsTextElement = true;
      typeMapElementInfo.WrappedElement = false;
      list.Add((object) typeMapElementInfo);
    }

    private bool CanBeNull(TypeData type) => !type.Type.IsValueType || type.IsNullable;

    public void IncludeType(Type type)
    {
      if (type == null)
        throw new ArgumentNullException(nameof (type));
      if (this.includedTypes == null)
        this.includedTypes = new ArrayList();
      if (!this.includedTypes.Contains((object) type))
        this.includedTypes.Add((object) type);
      if (this.relatedMaps.Count <= 0)
        return;
      foreach (XmlTypeMapping xmlTypeMapping in (ArrayList) this.relatedMaps.Clone())
      {
        if (xmlTypeMapping.TypeData.Type == typeof (object))
          xmlTypeMapping.DerivedTypes.Add((object) this.ImportTypeMapping(type));
      }
    }

    public void IncludeTypes(ICustomAttributeProvider provider)
    {
      foreach (XmlIncludeAttribute customAttribute in provider.GetCustomAttributes(typeof (XmlIncludeAttribute), true))
        this.IncludeType(customAttribute.Type);
    }

    private object GetDefaultValue(TypeData typeData, object defaultValue)
    {
      if (defaultValue == DBNull.Value || typeData.SchemaType != SchemaTypes.Enum || !(Enum.Format(typeData.Type, defaultValue, "g") == Enum.Format(typeData.Type, defaultValue, "d")))
        return defaultValue;
      throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Value '{0}' cannot be converted to {1}.", defaultValue, (object) defaultValue.GetType().FullName));
    }
  }
}
