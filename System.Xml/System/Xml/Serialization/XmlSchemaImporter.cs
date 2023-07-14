// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.XmlSchemaImporter
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.Xml.Schema;

namespace System.Xml.Serialization
{
  public class XmlSchemaImporter
  {
    private const string XmlNamespace = "http://www.w3.org/XML/1998/namespace";
    private XmlSchemas schemas;
    private CodeIdentifiers typeIdentifiers;
    private CodeIdentifiers elemIdentifiers = new CodeIdentifiers();
    private Hashtable mappedTypes = new Hashtable();
    private Hashtable primitiveDerivedMappedTypes = new Hashtable();
    private Hashtable dataMappedTypes = new Hashtable();
    private Queue pendingMaps = new Queue();
    private Hashtable sharedAnonymousTypes = new Hashtable();
    private bool encodedFormat;
    private XmlReflectionImporter auxXmlRefImporter;
    private SoapReflectionImporter auxSoapRefImporter;
    private bool anyTypeImported;
    private CodeGenerationOptions options;
    private static readonly XmlQualifiedName anyType = new XmlQualifiedName(nameof (anyType), "http://www.w3.org/2001/XMLSchema");
    private static readonly XmlQualifiedName arrayType = new XmlQualifiedName("Array", "http://schemas.xmlsoap.org/soap/encoding/");
    private static readonly XmlQualifiedName arrayTypeRefName = new XmlQualifiedName(nameof (arrayType), "http://schemas.xmlsoap.org/soap/encoding/");
    private XmlSchemaElement anyElement;

    public XmlSchemaImporter(XmlSchemas schemas)
    {
      this.schemas = schemas;
      this.typeIdentifiers = new CodeIdentifiers();
      this.InitializeExtensions();
    }

    public XmlSchemaImporter(XmlSchemas schemas, CodeIdentifiers typeIdentifiers)
      : this(schemas)
    {
      this.typeIdentifiers = typeIdentifiers;
    }

    public XmlSchemaImporter(
      XmlSchemas schemas,
      CodeGenerationOptions options,
      ImportContext context)
    {
      this.schemas = schemas;
      this.options = options;
      if (context != null)
      {
        this.typeIdentifiers = context.TypeIdentifiers;
        this.InitSharedData(context);
      }
      else
        this.typeIdentifiers = new CodeIdentifiers();
      this.InitializeExtensions();
    }

    public XmlSchemaImporter(
      XmlSchemas schemas,
      CodeIdentifiers typeIdentifiers,
      CodeGenerationOptions options)
    {
      this.typeIdentifiers = typeIdentifiers;
      this.schemas = schemas;
      this.options = options;
      this.InitializeExtensions();
    }

    private void InitSharedData(ImportContext context)
    {
      if (!context.ShareTypes)
        return;
      this.mappedTypes = context.MappedTypes;
      this.dataMappedTypes = context.DataMappedTypes;
      this.sharedAnonymousTypes = context.SharedAnonymousTypes;
    }

    internal bool UseEncodedFormat
    {
      get => this.encodedFormat;
      set => this.encodedFormat = value;
    }

    private void InitializeExtensions()
    {
    }

    public XmlMembersMapping ImportAnyType(XmlQualifiedName typeName, string elementName)
    {
      if (typeName == XmlQualifiedName.Empty)
      {
        XmlTypeMapMemberAnyElement memberAnyElement = new XmlTypeMapMemberAnyElement();
        memberAnyElement.Name = typeName.Name;
        memberAnyElement.TypeData = TypeTranslator.GetTypeData(typeof (XmlNode));
        memberAnyElement.ElementInfo.Add((object) this.CreateElementInfo(typeName.Namespace, (XmlTypeMapMember) memberAnyElement, typeName.Name, memberAnyElement.TypeData, true, XmlSchemaForm.None));
        return new XmlMembersMapping(new XmlMemberMapping[1]
        {
          new XmlMemberMapping(typeName.Name, typeName.Namespace, (XmlTypeMapMember) memberAnyElement, this.encodedFormat)
        });
      }
      XmlSchemaComplexType stype = (XmlSchemaComplexType) this.schemas.Find(typeName, typeof (XmlSchemaComplexType));
      if (stype == null)
        throw new InvalidOperationException("Referenced type '" + (object) typeName + "' not found");
      if (!this.CanBeAnyElement(stype))
        throw new InvalidOperationException("The type '" + (object) typeName + "' is not valid for a collection of any elements");
      ClassMap cmap = new ClassMap();
      CodeIdentifiers classIds = new CodeIdentifiers();
      bool isMixed = stype.IsMixed;
      this.ImportSequenceContent(typeName, cmap, ((XmlSchemaSequence) stype.Particle).Items, classIds, false, ref isMixed);
      XmlTypeMapMemberAnyElement allMember = (XmlTypeMapMemberAnyElement) cmap.AllMembers[0];
      allMember.Name = typeName.Name;
      return new XmlMembersMapping(new XmlMemberMapping[1]
      {
        new XmlMemberMapping(typeName.Name, typeName.Namespace, (XmlTypeMapMember) allMember, this.encodedFormat)
      });
    }

    public XmlTypeMapping ImportDerivedTypeMapping(XmlQualifiedName name, Type baseType) => this.ImportDerivedTypeMapping(name, baseType, true);

    public XmlTypeMapping ImportDerivedTypeMapping(
      XmlQualifiedName name,
      Type baseType,
      bool baseTypeCanBeIndirect)
    {
      XmlQualifiedName qname;
      XmlSchemaType stype;
      if (this.encodedFormat)
      {
        qname = name;
        stype = (XmlSchemaType) (this.schemas.Find(name, typeof (XmlSchemaComplexType)) as XmlSchemaComplexType);
        if (stype == null)
          throw new InvalidOperationException("Schema type '" + (object) name + "' not found or not valid");
      }
      else if (!this.LocateElement(name, out qname, out stype))
        return (XmlTypeMapping) null;
      XmlTypeMapping registeredTypeMapping = this.GetRegisteredTypeMapping(qname, baseType);
      if (registeredTypeMapping != null)
      {
        this.SetMapBaseType(registeredTypeMapping, baseType);
        registeredTypeMapping.UpdateRoot(name);
        return registeredTypeMapping;
      }
      XmlTypeMapping typeMapping = this.CreateTypeMapping(qname, SchemaTypes.Class, name);
      if (stype != null)
      {
        typeMapping.Documentation = this.GetDocumentation((XmlSchemaAnnotated) stype);
        this.RegisterMapFixup(typeMapping, qname, (XmlSchemaComplexType) stype);
      }
      else
      {
        ClassMap cmap = new ClassMap();
        CodeIdentifiers classIds = new CodeIdentifiers();
        typeMapping.ObjectMap = (ObjectMap) cmap;
        this.AddTextMember(qname, cmap, classIds);
      }
      this.BuildPendingMaps();
      this.SetMapBaseType(typeMapping, baseType);
      return typeMapping;
    }

    private void SetMapBaseType(XmlTypeMapping map, Type baseType)
    {
      XmlTypeMapping xmlTypeMapping1 = (XmlTypeMapping) null;
      for (; map != null; map = map.BaseMap)
      {
        if (map.TypeData.Type == baseType)
          return;
        xmlTypeMapping1 = map;
      }
      XmlTypeMapping xmlTypeMapping2 = this.ReflectType(baseType);
      xmlTypeMapping1.BaseMap = xmlTypeMapping2;
      xmlTypeMapping2.DerivedTypes.Add((object) xmlTypeMapping1);
      xmlTypeMapping2.DerivedTypes.AddRange((ICollection) xmlTypeMapping1.DerivedTypes);
      ClassMap objectMap1 = (ClassMap) xmlTypeMapping2.ObjectMap;
      ClassMap objectMap2 = (ClassMap) xmlTypeMapping1.ObjectMap;
      foreach (XmlTypeMapMember allMember in objectMap1.AllMembers)
        objectMap2.AddMember(allMember);
      foreach (XmlMapping derivedType in xmlTypeMapping1.DerivedTypes)
      {
        ClassMap objectMap3 = (ClassMap) derivedType.ObjectMap;
        foreach (XmlTypeMapMember allMember in objectMap1.AllMembers)
          objectMap3.AddMember(allMember);
      }
    }

    public XmlMembersMapping ImportMembersMapping(XmlQualifiedName name)
    {
      XmlSchemaElement xmlSchemaElement = (XmlSchemaElement) this.schemas.Find(name, typeof (XmlSchemaElement));
      if (xmlSchemaElement == null)
        throw new InvalidOperationException("Schema element '" + (object) name + "' not found or not valid");
      XmlSchemaComplexType stype;
      if (xmlSchemaElement.SchemaType != null)
      {
        stype = xmlSchemaElement.SchemaType as XmlSchemaComplexType;
      }
      else
      {
        if (xmlSchemaElement.SchemaTypeName.IsEmpty)
          return (XmlMembersMapping) null;
        object obj = this.schemas.Find(xmlSchemaElement.SchemaTypeName, typeof (XmlSchemaComplexType));
        if (obj == null)
        {
          if (this.IsPrimitiveTypeNamespace(xmlSchemaElement.SchemaTypeName.Namespace))
            return (XmlMembersMapping) null;
          throw new InvalidOperationException("Schema type '" + (object) xmlSchemaElement.SchemaTypeName + "' not found");
        }
        stype = obj as XmlSchemaComplexType;
      }
      XmlMemberMapping[] mapping = stype != null ? this.ImportMembersMappingComposite(stype, name) : throw new InvalidOperationException("Schema element '" + (object) name + "' not found or not valid");
      return new XmlMembersMapping(name.Name, name.Namespace, mapping);
    }

    public XmlMembersMapping ImportMembersMapping(XmlQualifiedName[] names)
    {
      XmlMemberMapping[] mapping = new XmlMemberMapping[names.Length];
      for (int index = 0; index < names.Length; ++index)
      {
        XmlSchemaElement elem = (XmlSchemaElement) this.schemas.Find(names[index], typeof (XmlSchemaElement));
        if (elem == null)
          throw new InvalidOperationException("Schema element '" + (object) names[index] + "' not found");
        XmlQualifiedName typeQName = new XmlQualifiedName("Message", names[index].Namespace);
        XmlTypeMapping map;
        TypeData elementTypeData = this.GetElementTypeData(typeQName, elem, names[index], out map);
        mapping[index] = this.ImportMemberMapping(elem.Name, typeQName.Namespace, elem.IsNillable, elementTypeData, map);
      }
      this.BuildPendingMaps();
      return new XmlMembersMapping(mapping);
    }

    [MonoTODO]
    public XmlMembersMapping ImportMembersMapping(
      string name,
      string ns,
      SoapSchemaMember[] members)
    {
      throw new NotImplementedException();
    }

    public XmlTypeMapping ImportSchemaType(XmlQualifiedName typeName) => this.ImportSchemaType(typeName, typeof (object));

    public XmlTypeMapping ImportSchemaType(XmlQualifiedName typeName, Type baseType) => this.ImportSchemaType(typeName, typeof (object), false);

    [MonoTODO("baseType and baseTypeCanBeIndirect are ignored")]
    public XmlTypeMapping ImportSchemaType(
      XmlQualifiedName typeName,
      Type baseType,
      bool baseTypeCanBeIndirect)
    {
      XmlSchemaType stype = (XmlSchemaType) this.schemas.Find(typeName, typeof (XmlSchemaComplexType)) ?? (XmlSchemaType) this.schemas.Find(typeName, typeof (XmlSchemaSimpleType));
      return this.ImportTypeCommon(typeName, typeName, stype, true);
    }

    internal XmlMembersMapping ImportEncodedMembersMapping(
      string name,
      string ns,
      SoapSchemaMember[] members,
      bool hasWrapperElement)
    {
      XmlMemberMapping[] mapping = new XmlMemberMapping[members.Length];
      for (int index = 0; index < members.Length; ++index)
      {
        TypeData typeData = this.GetTypeData(members[index].MemberType, (XmlQualifiedName) null, false);
        XmlTypeMapping typeMapping = this.GetTypeMapping(typeData);
        mapping[index] = this.ImportMemberMapping(members[index].MemberName, members[index].MemberType.Namespace, true, typeData, typeMapping);
      }
      this.BuildPendingMaps();
      return new XmlMembersMapping(name, ns, hasWrapperElement, false, mapping);
    }

    internal XmlMembersMapping ImportEncodedMembersMapping(
      string name,
      string ns,
      SoapSchemaMember member)
    {
      if (!(this.schemas.Find(member.MemberType, typeof (XmlSchemaComplexType)) is XmlSchemaComplexType stype))
        throw new InvalidOperationException("Schema type '" + (object) member.MemberType + "' not found or not valid");
      XmlMemberMapping[] mapping = this.ImportMembersMappingComposite(stype, member.MemberType);
      return new XmlMembersMapping(name, ns, mapping);
    }

    private XmlMemberMapping[] ImportMembersMappingComposite(
      XmlSchemaComplexType stype,
      XmlQualifiedName refer)
    {
      if (stype.Particle == null)
        return new XmlMemberMapping[0];
      ClassMap cmap = new ClassMap();
      if (!(stype.Particle is XmlSchemaSequence particle))
        throw new InvalidOperationException("Schema element '" + (object) refer + "' cannot be imported as XmlMembersMapping");
      CodeIdentifiers classIds = new CodeIdentifiers();
      this.ImportParticleComplexContent(refer, cmap, (XmlSchemaParticle) particle, classIds, false);
      this.ImportAttributes(refer, cmap, stype.Attributes, stype.AnyAttribute, classIds);
      this.BuildPendingMaps();
      int num = 0;
      XmlMemberMapping[] xmlMemberMappingArray = new XmlMemberMapping[cmap.AllMembers.Count];
      foreach (XmlTypeMapMember allMember in cmap.AllMembers)
        xmlMemberMappingArray[num++] = new XmlMemberMapping(allMember.Name, refer.Namespace, allMember, this.encodedFormat);
      return xmlMemberMappingArray;
    }

    private XmlMemberMapping ImportMemberMapping(
      string name,
      string ns,
      bool isNullable,
      TypeData type,
      XmlTypeMapping emap)
    {
      XmlTypeMapMemberElement mapMemberElement = !type.IsListType ? new XmlTypeMapMemberElement() : (XmlTypeMapMemberElement) new XmlTypeMapMemberList();
      mapMemberElement.Name = name;
      mapMemberElement.TypeData = type;
      mapMemberElement.ElementInfo.Add((object) this.CreateElementInfo(ns, (XmlTypeMapMember) mapMemberElement, name, type, isNullable, XmlSchemaForm.None, emap));
      return new XmlMemberMapping(name, ns, (XmlTypeMapMember) mapMemberElement, this.encodedFormat);
    }

    [MonoTODO]
    public XmlMembersMapping ImportMembersMapping(
      XmlQualifiedName[] names,
      Type baseType,
      bool baseTypeCanBeIndirect)
    {
      throw new NotImplementedException();
    }

    public XmlTypeMapping ImportTypeMapping(XmlQualifiedName name)
    {
      XmlSchemaElement elem = (XmlSchemaElement) this.schemas.Find(name, typeof (XmlSchemaElement));
      XmlQualifiedName qname;
      XmlSchemaType stype;
      if (!this.LocateElement(elem, out qname, out stype))
        throw new InvalidOperationException(string.Format("'{0}' is missing.", (object) name));
      return this.ImportTypeCommon(name, qname, stype, elem.IsNillable);
    }

    private XmlTypeMapping ImportTypeCommon(
      XmlQualifiedName name,
      XmlQualifiedName qname,
      XmlSchemaType stype,
      bool isNullable)
    {
      if (stype == null)
      {
        if (!(qname == XmlSchemaImporter.anyType))
          return this.ReflectType(TypeTranslator.GetPrimitiveTypeData(qname.Name), name.Namespace);
        XmlTypeMapping typeMapping = this.GetTypeMapping(TypeTranslator.GetTypeData(typeof (object)));
        this.BuildPendingMaps();
        return typeMapping;
      }
      XmlTypeMapping registeredTypeMapping = this.GetRegisteredTypeMapping(qname);
      if (registeredTypeMapping != null)
        return registeredTypeMapping;
      if (stype is XmlSchemaSimpleType)
        return this.ImportClassSimpleType(stype.QualifiedName, (XmlSchemaSimpleType) stype, name);
      XmlTypeMapping typeMapping1 = this.CreateTypeMapping(qname, SchemaTypes.Class, name);
      typeMapping1.Documentation = this.GetDocumentation((XmlSchemaAnnotated) stype);
      typeMapping1.IsNullable = isNullable;
      this.RegisterMapFixup(typeMapping1, qname, (XmlSchemaComplexType) stype);
      this.BuildPendingMaps();
      return typeMapping1;
    }

    private bool LocateElement(
      XmlQualifiedName name,
      out XmlQualifiedName qname,
      out XmlSchemaType stype)
    {
      return this.LocateElement((XmlSchemaElement) this.schemas.Find(name, typeof (XmlSchemaElement)), out qname, out stype);
    }

    private bool LocateElement(
      XmlSchemaElement elem,
      out XmlQualifiedName qname,
      out XmlSchemaType stype)
    {
      qname = (XmlQualifiedName) null;
      stype = (XmlSchemaType) null;
      if (elem == null)
        return false;
      if (elem.SchemaType != null)
      {
        stype = elem.SchemaType;
        qname = elem.QualifiedName;
      }
      else
      {
        if (elem.ElementType == XmlSchemaComplexType.AnyType)
        {
          qname = XmlSchemaImporter.anyType;
          return true;
        }
        if (elem.SchemaTypeName.IsEmpty)
          return false;
        object obj = this.schemas.Find(elem.SchemaTypeName, typeof (XmlSchemaComplexType)) ?? this.schemas.Find(elem.SchemaTypeName, typeof (XmlSchemaSimpleType));
        if (obj == null)
        {
          if (!this.IsPrimitiveTypeNamespace(elem.SchemaTypeName.Namespace))
            throw new InvalidOperationException("Schema type '" + (object) elem.SchemaTypeName + "' not found");
          qname = elem.SchemaTypeName;
          return true;
        }
        stype = (XmlSchemaType) obj;
        qname = stype.QualifiedName;
        if (stype.BaseSchemaType is XmlSchemaType baseSchemaType && baseSchemaType.QualifiedName == elem.SchemaTypeName)
          throw new InvalidOperationException("Cannot import schema for type '" + elem.SchemaTypeName.Name + "' from namespace '" + elem.SchemaTypeName.Namespace + "'. Redefine not supported");
      }
      return true;
    }

    private XmlTypeMapping ImportType(
      XmlQualifiedName name,
      XmlQualifiedName root,
      bool throwOnError)
    {
      XmlTypeMapping registeredTypeMapping = this.GetRegisteredTypeMapping(name);
      if (registeredTypeMapping != null)
      {
        registeredTypeMapping.UpdateRoot(root);
        return registeredTypeMapping;
      }
      XmlSchemaType stype = (XmlSchemaType) this.schemas.Find(name, typeof (XmlSchemaComplexType)) ?? (XmlSchemaType) this.schemas.Find(name, typeof (XmlSchemaSimpleType));
      if (stype != null)
        return this.ImportType(name, stype, root);
      if (!throwOnError)
        return (XmlTypeMapping) null;
      if (name.Namespace == "http://schemas.xmlsoap.org/soap/encoding/")
        throw new InvalidOperationException("Referenced type '" + (object) name + "' valid only for encoded SOAP.");
      throw new InvalidOperationException("Referenced type '" + (object) name + "' not found.");
    }

    private XmlTypeMapping ImportClass(XmlQualifiedName name)
    {
      XmlTypeMapping xmlTypeMapping = this.ImportType(name, (XmlQualifiedName) null, true);
      if (xmlTypeMapping.TypeData.SchemaType == SchemaTypes.Class)
        return xmlTypeMapping;
      XmlSchemaComplexType stype = this.schemas.Find(name, typeof (XmlSchemaComplexType)) as XmlSchemaComplexType;
      return this.CreateClassMap(name, stype, new XmlQualifiedName(xmlTypeMapping.ElementName, xmlTypeMapping.Namespace));
    }

    private XmlTypeMapping ImportType(
      XmlQualifiedName name,
      XmlSchemaType stype,
      XmlQualifiedName root)
    {
      XmlTypeMapping registeredTypeMapping = this.GetRegisteredTypeMapping(name);
      if (registeredTypeMapping != null)
      {
        XmlSchemaComplexType stype1 = stype as XmlSchemaComplexType;
        if (registeredTypeMapping.TypeData.SchemaType != SchemaTypes.Class || stype1 == null || !this.CanBeArray(name, stype1))
        {
          registeredTypeMapping.UpdateRoot(root);
          return registeredTypeMapping;
        }
      }
      switch (stype)
      {
        case XmlSchemaComplexType _:
          return this.ImportClassComplexType(name, (XmlSchemaComplexType) stype, root);
        case XmlSchemaSimpleType _:
          return this.ImportClassSimpleType(name, (XmlSchemaSimpleType) stype, root);
        default:
          throw new NotSupportedException("Schema type not supported: " + (object) stype.GetType());
      }
    }

    private XmlTypeMapping ImportClassComplexType(
      XmlQualifiedName typeQName,
      XmlSchemaComplexType stype,
      XmlQualifiedName root)
    {
      Type anyElementType = this.GetAnyElementType(stype);
      if (anyElementType != null)
        return this.GetTypeMapping(TypeTranslator.GetTypeData(anyElementType));
      if (this.CanBeArray(typeQName, stype))
      {
        TypeData arrayTypeData;
        ListMap listMap = this.BuildArrayMap(typeQName, stype, out arrayTypeData);
        if (listMap != null)
        {
          XmlTypeMapping arrayTypeMapping = this.CreateArrayTypeMapping(typeQName, arrayTypeData);
          arrayTypeMapping.ObjectMap = (ObjectMap) listMap;
          return arrayTypeMapping;
        }
      }
      else if (this.CanBeIXmlSerializable(stype))
        return this.ImportXmlSerializableMapping(typeQName.Namespace);
      return this.CreateClassMap(typeQName, stype, root);
    }

    private XmlTypeMapping CreateClassMap(
      XmlQualifiedName typeQName,
      XmlSchemaComplexType stype,
      XmlQualifiedName root)
    {
      XmlTypeMapping typeMapping = this.CreateTypeMapping(typeQName, SchemaTypes.Class, root);
      typeMapping.Documentation = this.GetDocumentation((XmlSchemaAnnotated) stype);
      this.RegisterMapFixup(typeMapping, typeQName, stype);
      return typeMapping;
    }

    private void RegisterMapFixup(
      XmlTypeMapping map,
      XmlQualifiedName typeQName,
      XmlSchemaComplexType stype)
    {
      this.pendingMaps.Enqueue((object) new XmlSchemaImporter.MapFixup()
      {
        Map = map,
        SchemaType = stype,
        TypeName = typeQName
      });
    }

    private void BuildPendingMaps()
    {
      while (this.pendingMaps.Count > 0)
      {
        XmlSchemaImporter.MapFixup mapFixup = (XmlSchemaImporter.MapFixup) this.pendingMaps.Dequeue();
        if (mapFixup.Map.ObjectMap == null)
        {
          this.BuildClassMap(mapFixup.Map, mapFixup.TypeName, mapFixup.SchemaType);
          if (mapFixup.Map.ObjectMap == null)
            this.pendingMaps.Enqueue((object) mapFixup);
        }
      }
    }

    private void BuildPendingMap(XmlTypeMapping map)
    {
      if (map.ObjectMap == null)
      {
        foreach (XmlSchemaImporter.MapFixup pendingMap in this.pendingMaps)
        {
          if (pendingMap.Map == map)
          {
            this.BuildClassMap(pendingMap.Map, pendingMap.TypeName, pendingMap.SchemaType);
            return;
          }
        }
        throw new InvalidOperationException("Can't complete map of type " + map.XmlType + " : " + map.Namespace);
      }
    }

    private void BuildClassMap(
      XmlTypeMapping map,
      XmlQualifiedName typeQName,
      XmlSchemaComplexType stype)
    {
      CodeIdentifiers classIds = new CodeIdentifiers();
      classIds.AddReserved(map.TypeData.TypeName);
      ClassMap cmap = new ClassMap();
      map.ObjectMap = (ObjectMap) cmap;
      bool isMixed = stype.IsMixed;
      if (stype.Particle != null)
        this.ImportParticleComplexContent(typeQName, cmap, stype.Particle, classIds, isMixed);
      else if (stype.ContentModel is XmlSchemaSimpleContent)
        this.ImportSimpleContent(typeQName, map, (XmlSchemaSimpleContent) stype.ContentModel, classIds, isMixed);
      else if (stype.ContentModel is XmlSchemaComplexContent)
        this.ImportComplexContent(typeQName, map, (XmlSchemaComplexContent) stype.ContentModel, classIds, isMixed);
      this.ImportAttributes(typeQName, cmap, stype.Attributes, stype.AnyAttribute, classIds);
      this.ImportExtensionTypes(typeQName);
      if (isMixed)
        this.AddTextMember(typeQName, cmap, classIds);
      this.AddObjectDerivedMap(map);
    }

    private void ImportAttributes(
      XmlQualifiedName typeQName,
      ClassMap cmap,
      XmlSchemaObjectCollection atts,
      XmlSchemaAnyAttribute anyat,
      CodeIdentifiers classIds)
    {
      atts = this.CollectAttributeUsesNonOverlap(atts, cmap);
      if (anyat != null)
      {
        XmlTypeMapMemberAnyAttribute member = new XmlTypeMapMemberAnyAttribute();
        member.Name = classIds.AddUnique("AnyAttribute", (object) member);
        member.TypeData = TypeTranslator.GetTypeData(typeof (XmlAttribute[]));
        cmap.AddMember((XmlTypeMapMember) member);
      }
      foreach (XmlSchemaObject att in atts)
      {
        if (att is XmlSchemaAttribute)
        {
          XmlSchemaAttribute xmlSchemaAttribute = (XmlSchemaAttribute) att;
          string ns;
          XmlSchemaAttribute refAttribute = this.GetRefAttribute(typeQName, xmlSchemaAttribute, out ns);
          XmlTypeMapMemberAttribute member = new XmlTypeMapMemberAttribute();
          member.Name = classIds.AddUnique(CodeIdentifier.MakeValid(refAttribute.Name), (object) member);
          member.Documentation = this.GetDocumentation((XmlSchemaAnnotated) xmlSchemaAttribute);
          member.AttributeName = refAttribute.Name;
          member.Namespace = ns;
          member.Form = refAttribute.Form;
          member.TypeData = this.GetAttributeTypeData(typeQName, xmlSchemaAttribute);
          if (refAttribute.DefaultValue != null)
            member.DefaultValue = this.ImportDefaultValue(member.TypeData, refAttribute.DefaultValue);
          else if (member.TypeData.IsValueType)
            member.IsOptionalValueType = refAttribute.ValidatedUse != XmlSchemaUse.Required;
          if (member.TypeData.IsComplexType)
            member.MappedType = this.GetTypeMapping(member.TypeData);
          cmap.AddMember((XmlTypeMapMember) member);
        }
        else if (att is XmlSchemaAttributeGroupRef)
        {
          XmlSchemaAttributeGroup refAttributeGroup = this.FindRefAttributeGroup(((XmlSchemaAttributeGroupRef) att).RefName);
          this.ImportAttributes(typeQName, cmap, refAttributeGroup.Attributes, refAttributeGroup.AnyAttribute, classIds);
        }
      }
    }

    private XmlSchemaObjectCollection CollectAttributeUsesNonOverlap(
      XmlSchemaObjectCollection src,
      ClassMap map)
    {
      XmlSchemaObjectCollection objectCollection = new XmlSchemaObjectCollection();
      foreach (XmlSchemaAttribute xmlSchemaAttribute in src)
      {
        if (map.GetAttribute(xmlSchemaAttribute.QualifiedName.Name, xmlSchemaAttribute.QualifiedName.Namespace) == null)
          objectCollection.Add((XmlSchemaObject) xmlSchemaAttribute);
      }
      return objectCollection;
    }

    private ListMap BuildArrayMap(
      XmlQualifiedName typeQName,
      XmlSchemaComplexType stype,
      out TypeData arrayTypeData)
    {
      if (this.encodedFormat)
      {
        XmlSchemaComplexContentRestriction content = (stype.ContentModel as XmlSchemaComplexContent).Content as XmlSchemaComplexContentRestriction;
        XmlSchemaAttribute arrayAttribute = this.FindArrayAttribute(content.Attributes);
        if (arrayAttribute != null)
        {
          XmlAttribute[] unhandledAttributes = arrayAttribute.UnhandledAttributes;
          if (unhandledAttributes == null || unhandledAttributes.Length == 0)
            throw new InvalidOperationException("arrayType attribute not specified in array declaration: " + (object) typeQName);
          XmlAttribute xmlAttribute1 = (XmlAttribute) null;
          foreach (XmlAttribute xmlAttribute2 in unhandledAttributes)
          {
            if (xmlAttribute2.LocalName == "arrayType" && xmlAttribute2.NamespaceURI == "http://schemas.xmlsoap.org/wsdl/")
            {
              xmlAttribute1 = xmlAttribute2;
              break;
            }
          }
          if (xmlAttribute1 == null)
            throw new InvalidOperationException("arrayType attribute not specified in array declaration: " + (object) typeQName);
          string type;
          string ns;
          string dimensions;
          TypeTranslator.ParseArrayType(xmlAttribute1.Value, out type, out ns, out dimensions);
          return this.BuildEncodedArrayMap(type + dimensions, ns, out arrayTypeData);
        }
        XmlSchemaElement xmlSchemaElement = (XmlSchemaElement) null;
        if (content.Particle is XmlSchemaSequence particle2 && particle2.Items.Count == 1)
          xmlSchemaElement = particle2.Items[0] as XmlSchemaElement;
        else if (content.Particle is XmlSchemaAll particle1 && particle1.Items.Count == 1)
          xmlSchemaElement = particle1.Items[0] as XmlSchemaElement;
        if (xmlSchemaElement == null)
          throw new InvalidOperationException("Unknown array format");
        return this.BuildEncodedArrayMap(xmlSchemaElement.SchemaTypeName.Name + "[]", xmlSchemaElement.SchemaTypeName.Namespace, out arrayTypeData);
      }
      ClassMap cmap = new ClassMap();
      CodeIdentifiers classIds = new CodeIdentifiers();
      this.ImportParticleComplexContent(typeQName, cmap, stype.Particle, classIds, stype.IsMixed);
      XmlTypeMapMemberFlatList allMember = cmap.AllMembers.Count != 1 ? (XmlTypeMapMemberFlatList) null : cmap.AllMembers[0] as XmlTypeMapMemberFlatList;
      if (allMember != null && allMember.ChoiceMember == null)
      {
        arrayTypeData = allMember.TypeData;
        return allMember.ListMap;
      }
      arrayTypeData = (TypeData) null;
      return (ListMap) null;
    }

    private ListMap BuildEncodedArrayMap(string type, string ns, out TypeData arrayTypeData)
    {
      ListMap listMap1 = new ListMap();
      int num = type.LastIndexOf("[");
      if (num == -1)
        throw new InvalidOperationException("Invalid arrayType value: " + type);
      string str = type.IndexOf(",", num) == -1 ? type.Substring(0, num) : throw new InvalidOperationException("Multidimensional arrays are not supported");
      TypeData arrayTypeData1;
      if (str.IndexOf("[") != -1)
      {
        ListMap listMap2 = this.BuildEncodedArrayMap(str, ns, out arrayTypeData1);
        int dimensions = str.Split('[').Length - 1;
        this.CreateArrayTypeMapping(new XmlQualifiedName(TypeTranslator.GetArrayName(type, dimensions), ns), arrayTypeData1).ObjectMap = (ObjectMap) listMap2;
      }
      else
        arrayTypeData1 = this.GetTypeData(new XmlQualifiedName(str, ns), (XmlQualifiedName) null, false);
      arrayTypeData = arrayTypeData1.ListTypeData;
      listMap1.ItemInfo = new XmlTypeMapElementInfoList();
      listMap1.ItemInfo.Add((object) this.CreateElementInfo(string.Empty, (XmlTypeMapMember) null, "Item", arrayTypeData1, true, XmlSchemaForm.None));
      return listMap1;
    }

    private XmlSchemaAttribute FindArrayAttribute(XmlSchemaObjectCollection atts)
    {
      foreach (object att in atts)
      {
        if (att is XmlSchemaAttribute arrayAttribute1 && arrayAttribute1.RefName == XmlSchemaImporter.arrayTypeRefName)
          return arrayAttribute1;
        if (att is XmlSchemaAttributeGroupRef attributeGroupRef)
        {
          XmlSchemaAttribute arrayAttribute2 = this.FindArrayAttribute(this.FindRefAttributeGroup(attributeGroupRef.RefName).Attributes);
          if (arrayAttribute2 != null)
            return arrayAttribute2;
        }
      }
      return (XmlSchemaAttribute) null;
    }

    private void ImportParticleComplexContent(
      XmlQualifiedName typeQName,
      ClassMap cmap,
      XmlSchemaParticle particle,
      CodeIdentifiers classIds,
      bool isMixed)
    {
      this.ImportParticleContent(typeQName, cmap, particle, classIds, false, ref isMixed);
      if (!isMixed)
        return;
      this.AddTextMember(typeQName, cmap, classIds);
    }

    private void AddTextMember(XmlQualifiedName typeQName, ClassMap cmap, CodeIdentifiers classIds)
    {
      if (cmap.XmlTextCollector != null)
        return;
      XmlTypeMapMemberFlatList member = new XmlTypeMapMemberFlatList();
      member.Name = classIds.AddUnique("Text", (object) member);
      member.TypeData = TypeTranslator.GetTypeData(typeof (string[]));
      member.ElementInfo.Add((object) this.CreateTextElementInfo(typeQName.Namespace, (XmlTypeMapMember) member, member.TypeData.ListItemTypeData));
      member.IsXmlTextCollector = true;
      member.ListMap = new ListMap();
      member.ListMap.ItemInfo = member.ElementInfo;
      cmap.AddMember((XmlTypeMapMember) member);
    }

    private void ImportParticleContent(
      XmlQualifiedName typeQName,
      ClassMap cmap,
      XmlSchemaParticle particle,
      CodeIdentifiers classIds,
      bool multiValue,
      ref bool isMixed)
    {
      if (particle == null)
        return;
      if (particle is XmlSchemaGroupRef)
        particle = this.GetRefGroupParticle((XmlSchemaGroupRef) particle);
      if (particle.MaxOccurs > 1M)
        multiValue = true;
      switch (particle)
      {
        case XmlSchemaSequence _:
          this.ImportSequenceContent(typeQName, cmap, ((XmlSchemaSequence) particle).Items, classIds, multiValue, ref isMixed);
          break;
        case XmlSchemaChoice _:
          if (((XmlSchemaChoice) particle).Items.Count == 1)
          {
            this.ImportSequenceContent(typeQName, cmap, ((XmlSchemaChoice) particle).Items, classIds, multiValue, ref isMixed);
            break;
          }
          this.ImportChoiceContent(typeQName, cmap, (XmlSchemaChoice) particle, classIds, multiValue);
          break;
        case XmlSchemaAll _:
          this.ImportSequenceContent(typeQName, cmap, ((XmlSchemaAll) particle).Items, classIds, multiValue, ref isMixed);
          break;
      }
    }

    private void ImportSequenceContent(
      XmlQualifiedName typeQName,
      ClassMap cmap,
      XmlSchemaObjectCollection items,
      CodeIdentifiers classIds,
      bool multiValue,
      ref bool isMixed)
    {
      foreach (XmlSchemaObject particle in items)
      {
        switch (particle)
        {
          case XmlSchemaElement _:
            XmlSchemaElement elem1 = (XmlSchemaElement) particle;
            XmlTypeMapping map;
            TypeData typeData = this.GetElementTypeData(typeQName, elem1, (XmlQualifiedName) null, out map);
            string ns;
            XmlSchemaElement refElement = this.GetRefElement(typeQName, elem1, out ns);
            if (elem1.MaxOccurs == 1M && !multiValue)
            {
              XmlTypeMapMemberElement member;
              if (typeData.SchemaType != SchemaTypes.Array)
              {
                member = new XmlTypeMapMemberElement();
                if (refElement.DefaultValue != null)
                  member.DefaultValue = this.ImportDefaultValue(typeData, refElement.DefaultValue);
              }
              else if (this.GetTypeMapping(typeData).IsSimpleType)
              {
                member = new XmlTypeMapMemberElement();
                typeData = TypeTranslator.GetTypeData(typeof (string));
              }
              else
                member = (XmlTypeMapMemberElement) new XmlTypeMapMemberList();
              if (elem1.MinOccurs == 0M && typeData.IsValueType)
                member.IsOptionalValueType = true;
              member.Name = classIds.AddUnique(CodeIdentifier.MakeValid(refElement.Name), (object) member);
              member.Documentation = this.GetDocumentation((XmlSchemaAnnotated) elem1);
              member.TypeData = typeData;
              member.ElementInfo.Add((object) this.CreateElementInfo(ns, (XmlTypeMapMember) member, refElement.Name, typeData, refElement.IsNillable, refElement.Form, map));
              cmap.AddMember((XmlTypeMapMember) member);
              continue;
            }
            XmlTypeMapMemberFlatList member1 = new XmlTypeMapMemberFlatList();
            member1.ListMap = new ListMap();
            member1.Name = classIds.AddUnique(CodeIdentifier.MakeValid(refElement.Name), (object) member1);
            member1.Documentation = this.GetDocumentation((XmlSchemaAnnotated) elem1);
            member1.TypeData = typeData.ListTypeData;
            member1.ElementInfo.Add((object) this.CreateElementInfo(ns, (XmlTypeMapMember) member1, refElement.Name, typeData, refElement.IsNillable, refElement.Form, map));
            member1.ListMap.ItemInfo = member1.ElementInfo;
            cmap.AddMember((XmlTypeMapMember) member1);
            continue;
          case XmlSchemaAny _:
            XmlSchemaAny elem2 = (XmlSchemaAny) particle;
            XmlTypeMapMemberAnyElement member2 = new XmlTypeMapMemberAnyElement();
            member2.Name = classIds.AddUnique("Any", (object) member2);
            member2.Documentation = this.GetDocumentation((XmlSchemaAnnotated) elem2);
            Type type = elem2.MaxOccurs != 1M || multiValue ? (!isMixed ? typeof (XmlElement[]) : typeof (XmlNode[])) : (!isMixed ? typeof (XmlElement) : typeof (XmlNode));
            member2.TypeData = TypeTranslator.GetTypeData(type);
            member2.ElementInfo.Add((object) new XmlTypeMapElementInfo((XmlTypeMapMember) member2, member2.TypeData)
            {
              IsUnnamedAnyElement = true
            });
            if (isMixed)
            {
              XmlTypeMapElementInfo textElementInfo = this.CreateTextElementInfo(typeQName.Namespace, (XmlTypeMapMember) member2, member2.TypeData);
              member2.ElementInfo.Add((object) textElementInfo);
              member2.IsXmlTextCollector = true;
              isMixed = false;
            }
            cmap.AddMember((XmlTypeMapMember) member2);
            continue;
          case XmlSchemaParticle _:
            this.ImportParticleContent(typeQName, cmap, (XmlSchemaParticle) particle, classIds, multiValue, ref isMixed);
            continue;
          default:
            continue;
        }
      }
    }

    private object ImportDefaultValue(TypeData typeData, string value)
    {
      if (typeData.SchemaType != SchemaTypes.Enum)
        return XmlCustomFormatter.FromXmlString(typeData, value);
      XmlTypeMapping typeMapping = this.GetTypeMapping(typeData);
      return (object) (((EnumMap) typeMapping.ObjectMap).GetEnumName(typeMapping.TypeFullName, value) ?? throw new InvalidOperationException("'" + value + "' is not a valid enumeration value"));
    }

    private void ImportChoiceContent(
      XmlQualifiedName typeQName,
      ClassMap cmap,
      XmlSchemaChoice choice,
      CodeIdentifiers classIds,
      bool multiValue)
    {
      XmlTypeMapElementInfoList choices = new XmlTypeMapElementInfoList();
      multiValue = this.ImportChoices(typeQName, (XmlTypeMapMember) null, choices, choice.Items) || multiValue;
      if (choices.Count == 0)
        return;
      if (choice.MaxOccurs > 1M)
        multiValue = true;
      XmlTypeMapMemberElement member1;
      if (multiValue)
      {
        member1 = (XmlTypeMapMemberElement) new XmlTypeMapMemberFlatList();
        member1.Name = classIds.AddUnique("Items", (object) member1);
        ((XmlTypeMapMemberFlatList) member1).ListMap = new ListMap()
        {
          ItemInfo = choices
        };
      }
      else
      {
        member1 = new XmlTypeMapMemberElement();
        member1.Name = classIds.AddUnique("Item", (object) member1);
      }
      TypeData typeData1 = (TypeData) null;
      bool flag1 = false;
      bool flag2 = true;
      Hashtable hashtable = new Hashtable();
      for (int index = choices.Count - 1; index >= 0; --index)
      {
        XmlTypeMapElementInfo typeMapElementInfo = (XmlTypeMapElementInfo) choices[index];
        if (cmap.GetElement(typeMapElementInfo.ElementName, typeMapElementInfo.Namespace) != null || choices.IndexOfElement(typeMapElementInfo.ElementName, typeMapElementInfo.Namespace) != index)
        {
          choices.RemoveAt(index);
        }
        else
        {
          if (hashtable.ContainsKey((object) typeMapElementInfo.TypeData))
            flag1 = true;
          else
            hashtable.Add((object) typeMapElementInfo.TypeData, (object) typeMapElementInfo);
          TypeData typeData2 = typeMapElementInfo.TypeData;
          if (typeData2.SchemaType == SchemaTypes.Class)
          {
            XmlTypeMapping map = this.GetTypeMapping(typeData2);
            this.BuildPendingMap(map);
            while (map.BaseMap != null)
            {
              map = map.BaseMap;
              this.BuildPendingMap(map);
              typeData2 = map.TypeData;
            }
          }
          if (typeData1 == null)
            typeData1 = typeData2;
          else if (typeData1 != typeData2)
            flag2 = false;
        }
      }
      if (!flag2)
        typeData1 = TypeTranslator.GetTypeData(typeof (object));
      if (flag1)
      {
        XmlTypeMapMemberElement member2 = new XmlTypeMapMemberElement();
        member2.Ignore = true;
        member2.Name = classIds.AddUnique(member1.Name + "ElementName", (object) member2);
        member1.ChoiceMember = member2.Name;
        XmlTypeMapping typeMapping = this.CreateTypeMapping(new XmlQualifiedName(member1.Name + "ChoiceType", typeQName.Namespace), SchemaTypes.Enum, (XmlQualifiedName) null);
        typeMapping.IncludeInSchema = false;
        CodeIdentifiers codeIdentifiers = new CodeIdentifiers();
        EnumMap.EnumMapMember[] members = new EnumMap.EnumMapMember[choices.Count];
        for (int index = 0; index < choices.Count; ++index)
        {
          XmlTypeMapElementInfo typeMapElementInfo = (XmlTypeMapElementInfo) choices[index];
          string xmlName = typeMapElementInfo.Namespace == null || !(typeMapElementInfo.Namespace != string.Empty) || !(typeMapElementInfo.Namespace != typeQName.Namespace) ? typeMapElementInfo.ElementName : typeMapElementInfo.Namespace + ":" + typeMapElementInfo.ElementName;
          string enumName = codeIdentifiers.AddUnique(CodeIdentifier.MakeValid(typeMapElementInfo.ElementName), (object) typeMapElementInfo);
          members[index] = new EnumMap.EnumMapMember(xmlName, enumName);
        }
        typeMapping.ObjectMap = (ObjectMap) new EnumMap(members, false);
        member2.TypeData = !multiValue ? typeMapping.TypeData : typeMapping.TypeData.ListTypeData;
        member2.ElementInfo.Add((object) this.CreateElementInfo(typeQName.Namespace, (XmlTypeMapMember) member2, member2.Name, member2.TypeData, false, XmlSchemaForm.None));
        cmap.AddMember((XmlTypeMapMember) member2);
      }
      if (typeData1 == null)
        return;
      if (multiValue)
        typeData1 = typeData1.ListTypeData;
      member1.ElementInfo = choices;
      member1.Documentation = this.GetDocumentation((XmlSchemaAnnotated) choice);
      member1.TypeData = typeData1;
      cmap.AddMember((XmlTypeMapMember) member1);
    }

    private bool ImportChoices(
      XmlQualifiedName typeQName,
      XmlTypeMapMember member,
      XmlTypeMapElementInfoList choices,
      XmlSchemaObjectCollection items)
    {
      bool flag = false;
      foreach (XmlSchemaObject xmlSchemaObject in items)
      {
        XmlSchemaObject refGroup = xmlSchemaObject;
        if (refGroup is XmlSchemaGroupRef)
          refGroup = (XmlSchemaObject) this.GetRefGroupParticle((XmlSchemaGroupRef) refGroup);
        if (refGroup is XmlSchemaElement)
        {
          XmlSchemaElement elem = (XmlSchemaElement) refGroup;
          XmlTypeMapping map;
          TypeData elementTypeData = this.GetElementTypeData(typeQName, elem, (XmlQualifiedName) null, out map);
          string ns;
          XmlSchemaElement refElement = this.GetRefElement(typeQName, elem, out ns);
          choices.Add((object) this.CreateElementInfo(ns, member, refElement.Name, elementTypeData, refElement.IsNillable, refElement.Form, map));
          if (elem.MaxOccurs > 1M)
            flag = true;
        }
        else if (refGroup is XmlSchemaAny)
          choices.Add((object) new XmlTypeMapElementInfo(member, TypeTranslator.GetTypeData(typeof (XmlElement)))
          {
            IsUnnamedAnyElement = true
          });
        else if (refGroup is XmlSchemaChoice)
          flag = this.ImportChoices(typeQName, member, choices, ((XmlSchemaChoice) refGroup).Items) || flag;
        else if (refGroup is XmlSchemaSequence)
          flag = this.ImportChoices(typeQName, member, choices, ((XmlSchemaSequence) refGroup).Items) || flag;
      }
      return flag;
    }

    private void ImportSimpleContent(
      XmlQualifiedName typeQName,
      XmlTypeMapping map,
      XmlSchemaSimpleContent content,
      CodeIdentifiers classIds,
      bool isMixed)
    {
      XmlSchemaSimpleContentExtension content1 = content.Content as XmlSchemaSimpleContentExtension;
      ClassMap objectMap = (ClassMap) map.ObjectMap;
      XmlQualifiedName contentBaseType = this.GetContentBaseType((XmlSchemaObject) content.Content);
      TypeData typeData = (TypeData) null;
      if (!this.IsPrimitiveTypeNamespace(contentBaseType.Namespace))
      {
        XmlTypeMapping map1 = this.ImportType(contentBaseType, (XmlQualifiedName) null, true);
        this.BuildPendingMap(map1);
        if (map1.IsSimpleType)
        {
          typeData = map1.TypeData;
        }
        else
        {
          foreach (XmlTypeMapMember allMember in ((ClassMap) map1.ObjectMap).AllMembers)
            objectMap.AddMember(allMember);
          map.BaseMap = map1;
          map1.DerivedTypes.Add((object) map);
        }
      }
      else
        typeData = this.FindBuiltInType(contentBaseType);
      if (typeData != null)
      {
        XmlTypeMapMemberElement member = new XmlTypeMapMemberElement();
        member.Name = classIds.AddUnique("Value", (object) member);
        member.TypeData = typeData;
        member.ElementInfo.Add((object) this.CreateTextElementInfo(typeQName.Namespace, (XmlTypeMapMember) member, member.TypeData));
        member.IsXmlTextCollector = true;
        objectMap.AddMember((XmlTypeMapMember) member);
      }
      if (content1 == null)
        return;
      this.ImportAttributes(typeQName, objectMap, content1.Attributes, content1.AnyAttribute, classIds);
    }

    private TypeData FindBuiltInType(XmlQualifiedName qname)
    {
      XmlSchemaComplexType schemaComplexType = (XmlSchemaComplexType) this.schemas.Find(qname, typeof (XmlSchemaComplexType));
      if (schemaComplexType != null)
      {
        if (!(schemaComplexType.ContentModel is XmlSchemaSimpleContent contentModel))
          throw new InvalidOperationException("Invalid schema");
        return this.FindBuiltInType(this.GetContentBaseType((XmlSchemaObject) contentModel.Content));
      }
      XmlSchemaSimpleType st = (XmlSchemaSimpleType) this.schemas.Find(qname, typeof (XmlSchemaSimpleType));
      if (st != null)
        return this.FindBuiltInType(qname, st);
      return this.IsPrimitiveTypeNamespace(qname.Namespace) ? TypeTranslator.GetPrimitiveTypeData(qname.Name) : throw new InvalidOperationException("Definition of type '" + (object) qname + "' not found");
    }

    private TypeData FindBuiltInType(XmlQualifiedName qname, XmlSchemaSimpleType st)
    {
      if (this.CanBeEnum(st) && qname != (XmlQualifiedName) null)
        return this.ImportType(qname, (XmlQualifiedName) null, true).TypeData;
      if (st.Content is XmlSchemaSimpleTypeRestriction)
      {
        XmlSchemaSimpleTypeRestriction content = (XmlSchemaSimpleTypeRestriction) st.Content;
        XmlQualifiedName contentBaseType = this.GetContentBaseType((XmlSchemaObject) content);
        return contentBaseType == XmlQualifiedName.Empty && content.BaseType != null ? this.FindBuiltInType(qname, content.BaseType) : this.FindBuiltInType(contentBaseType);
      }
      if (st.Content is XmlSchemaSimpleTypeList)
        return this.FindBuiltInType(this.GetContentBaseType((XmlSchemaObject) st.Content)).ListTypeData;
      return st.Content is XmlSchemaSimpleTypeUnion ? this.FindBuiltInType(new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema")) : (TypeData) null;
    }

    private XmlQualifiedName GetContentBaseType(XmlSchemaObject ob)
    {
      switch (ob)
      {
        case XmlSchemaSimpleContentExtension _:
          return ((XmlSchemaSimpleContentExtension) ob).BaseTypeName;
        case XmlSchemaSimpleContentRestriction _:
          return ((XmlSchemaSimpleContentRestriction) ob).BaseTypeName;
        case XmlSchemaSimpleTypeRestriction _:
          return ((XmlSchemaSimpleTypeRestriction) ob).BaseTypeName;
        case XmlSchemaSimpleTypeList _:
          return ((XmlSchemaSimpleTypeList) ob).ItemTypeName;
        default:
          return (XmlQualifiedName) null;
      }
    }

    private void ImportComplexContent(
      XmlQualifiedName typeQName,
      XmlTypeMapping map,
      XmlSchemaComplexContent content,
      CodeIdentifiers classIds,
      bool isMixed)
    {
      ClassMap objectMap1 = (ClassMap) map.ObjectMap;
      XmlQualifiedName name = !(content.Content is XmlSchemaComplexContentExtension content1) ? ((XmlSchemaComplexContentRestriction) content.Content).BaseTypeName : content1.BaseTypeName;
      XmlTypeMapping map1 = !(name == typeQName) ? this.ImportClass(name) : throw new InvalidOperationException("Cannot import schema for type '" + typeQName.Name + "' from namespace '" + typeQName.Namespace + "'. Redefine not supported");
      this.BuildPendingMap(map1);
      ClassMap objectMap2 = (ClassMap) map1.ObjectMap;
      foreach (XmlTypeMapMember allMember in objectMap2.AllMembers)
        objectMap1.AddMember(allMember);
      if (objectMap2.XmlTextCollector != null)
        isMixed = false;
      else if (content.IsMixed)
        isMixed = true;
      map.BaseMap = map1;
      map1.DerivedTypes.Add((object) map);
      if (content1 != null)
      {
        this.ImportParticleComplexContent(typeQName, objectMap1, content1.Particle, classIds, isMixed);
        this.ImportAttributes(typeQName, objectMap1, content1.Attributes, content1.AnyAttribute, classIds);
      }
      else
      {
        if (!isMixed)
          return;
        this.ImportParticleComplexContent(typeQName, objectMap1, (XmlSchemaParticle) null, classIds, true);
      }
    }

    private void ImportExtensionTypes(XmlQualifiedName qname)
    {
      foreach (XmlSchema schema in (CollectionBase) this.schemas)
      {
        foreach (XmlSchemaObject xmlSchemaObject in schema.Items)
        {
          if (xmlSchemaObject is XmlSchemaComplexType stype && stype.ContentModel is XmlSchemaComplexContent && (!(stype.ContentModel.Content is XmlSchemaComplexContentExtension content) ? ((XmlSchemaComplexContentRestriction) stype.ContentModel.Content).BaseTypeName : content.BaseTypeName) == qname)
            this.ImportType(new XmlQualifiedName(stype.Name, schema.TargetNamespace), (XmlSchemaType) stype, (XmlQualifiedName) null);
        }
      }
    }

    private XmlTypeMapping ImportClassSimpleType(
      XmlQualifiedName typeQName,
      XmlSchemaSimpleType stype,
      XmlQualifiedName root)
    {
      if (this.CanBeEnum(stype))
      {
        CodeIdentifiers codeIdentifiers = new CodeIdentifiers();
        XmlTypeMapping typeMapping = this.CreateTypeMapping(typeQName, SchemaTypes.Enum, root);
        typeMapping.Documentation = this.GetDocumentation((XmlSchemaAnnotated) stype);
        bool isFlags = false;
        if (stype.Content is XmlSchemaSimpleTypeList)
        {
          stype = ((XmlSchemaSimpleTypeList) stype.Content).ItemType;
          isFlags = true;
        }
        XmlSchemaSimpleTypeRestriction content = (XmlSchemaSimpleTypeRestriction) stype.Content;
        codeIdentifiers.AddReserved(typeMapping.TypeData.TypeName);
        EnumMap.EnumMapMember[] members = new EnumMap.EnumMapMember[content.Facets.Count];
        for (int index = 0; index < content.Facets.Count; ++index)
        {
          XmlSchemaEnumerationFacet facet = (XmlSchemaEnumerationFacet) content.Facets[index];
          string enumName = codeIdentifiers.AddUnique(CodeIdentifier.MakeValid(facet.Value), (object) facet);
          members[index] = new EnumMap.EnumMapMember(facet.Value, enumName);
          members[index].Documentation = this.GetDocumentation((XmlSchemaAnnotated) facet);
        }
        typeMapping.ObjectMap = (ObjectMap) new EnumMap(members, isFlags);
        typeMapping.IsSimpleType = true;
        return typeMapping;
      }
      if (stype.Content is XmlSchemaSimpleTypeList)
      {
        TypeData builtInType = this.FindBuiltInType(((XmlSchemaSimpleTypeList) stype.Content).ItemTypeName, stype);
        ListMap listMap = new ListMap();
        listMap.ItemInfo = new XmlTypeMapElementInfoList();
        listMap.ItemInfo.Add((object) this.CreateElementInfo(typeQName.Namespace, (XmlTypeMapMember) null, "Item", builtInType.ListItemTypeData, false, XmlSchemaForm.None));
        XmlTypeMapping arrayTypeMapping = this.CreateArrayTypeMapping(typeQName, builtInType);
        arrayTypeMapping.ObjectMap = (ObjectMap) listMap;
        arrayTypeMapping.IsSimpleType = true;
        return arrayTypeMapping;
      }
      XmlTypeMapping typeMapping1 = this.GetTypeMapping(this.FindBuiltInType(typeQName, stype));
      typeMapping1.IsSimpleType = true;
      return typeMapping1;
    }

    private bool CanBeEnum(XmlSchemaSimpleType stype)
    {
      if (stype.Content is XmlSchemaSimpleTypeRestriction)
      {
        XmlSchemaSimpleTypeRestriction content = (XmlSchemaSimpleTypeRestriction) stype.Content;
        if (content.Facets.Count == 0)
          return false;
        foreach (object facet in content.Facets)
        {
          if (!(facet is XmlSchemaEnumerationFacet))
            return false;
        }
        return true;
      }
      if (!(stype.Content is XmlSchemaSimpleTypeList))
        return false;
      XmlSchemaSimpleTypeList content1 = (XmlSchemaSimpleTypeList) stype.Content;
      return content1.ItemType != null && this.CanBeEnum(content1.ItemType);
    }

    private bool CanBeArray(XmlQualifiedName typeQName, XmlSchemaComplexType stype) => this.encodedFormat ? stype.ContentModel is XmlSchemaComplexContent contentModel && contentModel.Content is XmlSchemaComplexContentRestriction content && content.BaseTypeName == XmlSchemaImporter.arrayType : stype.Attributes.Count <= 0 && stype.AnyAttribute == null && !stype.IsMixed && this.CanBeArray(typeQName, stype.Particle, false);

    private bool CanBeArray(
      XmlQualifiedName typeQName,
      XmlSchemaParticle particle,
      bool multiValue)
    {
      if (particle == null)
        return false;
      multiValue = multiValue || particle.MaxOccurs > 1M;
      switch (particle)
      {
        case XmlSchemaGroupRef _:
          return this.CanBeArray(typeQName, this.GetRefGroupParticle((XmlSchemaGroupRef) particle), multiValue);
        case XmlSchemaElement _:
          XmlSchemaElement elem = (XmlSchemaElement) particle;
          if (!elem.RefName.IsEmpty)
            return this.CanBeArray(typeQName, (XmlSchemaParticle) this.FindRefElement(elem), multiValue);
          return multiValue && !typeQName.Equals((object) ((XmlSchemaElement) particle).SchemaTypeName);
        case XmlSchemaAny _:
          return multiValue;
        case XmlSchemaSequence _:
          XmlSchemaSequence xmlSchemaSequence = particle as XmlSchemaSequence;
          return xmlSchemaSequence.Items.Count == 1 && this.CanBeArray(typeQName, (XmlSchemaParticle) xmlSchemaSequence.Items[0], multiValue);
        case XmlSchemaChoice _:
          ArrayList types = new ArrayList();
          return this.CheckChoiceType(typeQName, particle, types, ref multiValue) && multiValue;
        default:
          return false;
      }
    }

    private bool CheckChoiceType(
      XmlQualifiedName typeQName,
      XmlSchemaParticle particle,
      ArrayList types,
      ref bool multiValue)
    {
      XmlQualifiedName other = (XmlQualifiedName) null;
      multiValue = multiValue || particle.MaxOccurs > 1M;
      switch (particle)
      {
        case XmlSchemaGroupRef _:
          return this.CheckChoiceType(typeQName, this.GetRefGroupParticle((XmlSchemaGroupRef) particle), types, ref multiValue);
        case XmlSchemaElement _:
          XmlSchemaElement elem = (XmlSchemaElement) particle;
          XmlSchemaElement refElement = this.GetRefElement(typeQName, elem, out string _);
          if (refElement.SchemaType != null)
            return true;
          other = refElement.SchemaTypeName;
          break;
        case XmlSchemaAny _:
          other = XmlSchemaImporter.anyType;
          break;
        case XmlSchemaSequence _:
          foreach (XmlSchemaParticle particle1 in (particle as XmlSchemaSequence).Items)
          {
            if (!this.CheckChoiceType(typeQName, particle1, types, ref multiValue))
              return false;
          }
          return true;
        case XmlSchemaChoice _:
          foreach (XmlSchemaParticle particle2 in ((XmlSchemaChoice) particle).Items)
          {
            if (!this.CheckChoiceType(typeQName, particle2, types, ref multiValue))
              return false;
          }
          return true;
      }
      if (typeQName.Equals((object) other))
        return false;
      string str = !this.IsPrimitiveTypeNamespace(other.Namespace) ? other.Name + ":" + other.Namespace : TypeTranslator.GetPrimitiveTypeData(other.Name).FullTypeName + ":" + other.Namespace;
      if (types.Contains((object) str))
        return false;
      types.Add((object) str);
      return true;
    }

    private bool CanBeAnyElement(XmlSchemaComplexType stype) => stype.Particle is XmlSchemaSequence particle && particle.Items.Count == 1 && particle.Items[0] is XmlSchemaAny;

    private Type GetAnyElementType(XmlSchemaComplexType stype)
    {
      if (!(stype.Particle is XmlSchemaSequence particle) || particle.Items.Count != 1 || !(particle.Items[0] is XmlSchemaAny))
        return (Type) null;
      if (this.encodedFormat)
        return typeof (object);
      return (particle.Items[0] as XmlSchemaAny).MaxOccurs == 1M ? (stype.IsMixed ? typeof (XmlNode) : typeof (XmlElement)) : (stype.IsMixed ? typeof (XmlNode[]) : typeof (XmlElement[]));
    }

    private bool CanBeIXmlSerializable(XmlSchemaComplexType stype) => stype.Particle is XmlSchemaSequence particle && particle.Items.Count == 2 && particle.Items[0] is XmlSchemaElement xmlSchemaElement && !(xmlSchemaElement.RefName != new XmlQualifiedName("schema", "http://www.w3.org/2001/XMLSchema")) && particle.Items[1] is XmlSchemaAny;

    private XmlTypeMapping ImportXmlSerializableMapping(string ns)
    {
      XmlQualifiedName xmlQualifiedName = new XmlQualifiedName("System.Data.DataSet", ns);
      XmlTypeMapping registeredTypeMapping = this.GetRegisteredTypeMapping(xmlQualifiedName);
      if (registeredTypeMapping != null)
        return registeredTypeMapping;
      TypeData typeData = new TypeData("System.Data.DataSet", "System.Data.DataSet", "System.Data.DataSet", SchemaTypes.XmlSerializable, (TypeData) null);
      XmlTypeMapping map = new XmlTypeMapping("System.Data.DataSet", string.Empty, typeData, "System.Data.DataSet", ns);
      map.IncludeInSchema = true;
      this.RegisterTypeMapping(xmlQualifiedName, typeData, map);
      return map;
    }

    private XmlTypeMapElementInfo CreateElementInfo(
      string ns,
      XmlTypeMapMember member,
      string name,
      TypeData typeData,
      bool isNillable,
      XmlSchemaForm form)
    {
      return typeData.IsComplexType ? this.CreateElementInfo(ns, member, name, typeData, isNillable, form, this.GetTypeMapping(typeData)) : this.CreateElementInfo(ns, member, name, typeData, isNillable, form, (XmlTypeMapping) null);
    }

    private XmlTypeMapElementInfo CreateElementInfo(
      string ns,
      XmlTypeMapMember member,
      string name,
      TypeData typeData,
      bool isNillable,
      XmlSchemaForm form,
      XmlTypeMapping emap)
    {
      XmlTypeMapElementInfo elementInfo = new XmlTypeMapElementInfo(member, typeData);
      elementInfo.ElementName = name;
      elementInfo.Namespace = ns;
      elementInfo.IsNullable = isNillable;
      elementInfo.Form = form;
      if (typeData.IsComplexType)
        elementInfo.MappedType = emap;
      return elementInfo;
    }

    private XmlTypeMapElementInfo CreateTextElementInfo(
      string ns,
      XmlTypeMapMember member,
      TypeData typeData)
    {
      XmlTypeMapElementInfo textElementInfo = new XmlTypeMapElementInfo(member, typeData);
      textElementInfo.IsTextElement = true;
      textElementInfo.WrappedElement = false;
      if (typeData.IsComplexType)
        textElementInfo.MappedType = this.GetTypeMapping(typeData);
      return textElementInfo;
    }

    private XmlTypeMapping CreateTypeMapping(
      XmlQualifiedName typeQName,
      SchemaTypes schemaType,
      XmlQualifiedName root)
    {
      string str = this.typeIdentifiers.AddUnique(CodeIdentifier.MakeValid(typeQName.Name), (object) null);
      TypeData typeData = new TypeData(str, str, str, schemaType, (TypeData) null);
      string name;
      string empty;
      if (root != (XmlQualifiedName) null)
      {
        name = root.Name;
        empty = root.Namespace;
      }
      else
      {
        name = typeQName.Name;
        empty = string.Empty;
      }
      XmlTypeMapping map = new XmlTypeMapping(name, empty, typeData, typeQName.Name, typeQName.Namespace);
      map.IncludeInSchema = true;
      this.RegisterTypeMapping(typeQName, typeData, map);
      return map;
    }

    private XmlTypeMapping CreateArrayTypeMapping(
      XmlQualifiedName typeQName,
      TypeData arrayTypeData)
    {
      XmlTypeMapping map = !this.encodedFormat ? new XmlTypeMapping(arrayTypeData.XmlType, typeQName.Namespace, arrayTypeData, arrayTypeData.XmlType, typeQName.Namespace) : new XmlTypeMapping("Array", "http://schemas.xmlsoap.org/soap/encoding/", arrayTypeData, "Array", "http://schemas.xmlsoap.org/soap/encoding/");
      map.IncludeInSchema = true;
      this.RegisterTypeMapping(typeQName, arrayTypeData, map);
      return map;
    }

    private XmlSchemaElement GetRefElement(
      XmlQualifiedName typeQName,
      XmlSchemaElement elem,
      out string ns)
    {
      if (!elem.RefName.IsEmpty)
      {
        ns = elem.RefName.Namespace;
        return this.FindRefElement(elem);
      }
      ns = typeQName.Namespace;
      return elem;
    }

    private XmlSchemaAttribute GetRefAttribute(
      XmlQualifiedName typeQName,
      XmlSchemaAttribute attr,
      out string ns)
    {
      if (!attr.RefName.IsEmpty)
      {
        ns = attr.RefName.Namespace;
        return this.FindRefAttribute(attr.RefName) ?? throw new InvalidOperationException("The attribute " + (object) attr.RefName + " is missing");
      }
      ns = !attr.ParentIsSchema ? string.Empty : typeQName.Namespace;
      return attr;
    }

    private TypeData GetElementTypeData(
      XmlQualifiedName typeQName,
      XmlSchemaElement elem,
      XmlQualifiedName root,
      out XmlTypeMapping map)
    {
      bool sharedAnnType = false;
      map = (XmlTypeMapping) null;
      if (!elem.RefName.IsEmpty)
      {
        XmlSchemaElement refElement = this.FindRefElement(elem);
        if (refElement == null)
          throw new InvalidOperationException("Global element not found: " + (object) elem.RefName);
        root = elem.RefName;
        elem = refElement;
        sharedAnnType = true;
      }
      TypeData typeData;
      if (!elem.SchemaTypeName.IsEmpty)
      {
        typeData = this.GetTypeData(elem.SchemaTypeName, root, elem.IsNillable);
        map = this.GetRegisteredTypeMapping(typeData);
      }
      else
        typeData = elem.SchemaType != null ? this.GetTypeData(elem.SchemaType, typeQName, elem.Name, sharedAnnType, root) : TypeTranslator.GetTypeData(typeof (object));
      if (map == null && typeData.IsComplexType)
        map = this.GetTypeMapping(typeData);
      return typeData;
    }

    private TypeData GetAttributeTypeData(XmlQualifiedName typeQName, XmlSchemaAttribute attr)
    {
      bool sharedAnnType = false;
      if (!attr.RefName.IsEmpty)
      {
        attr = this.FindRefAttribute(attr.RefName) ?? throw new InvalidOperationException("Global attribute not found: " + (object) attr.RefName);
        sharedAnnType = true;
      }
      if (!attr.SchemaTypeName.IsEmpty)
        return this.GetTypeData(attr.SchemaTypeName, (XmlQualifiedName) null, false);
      return attr.SchemaType == null ? TypeTranslator.GetTypeData(typeof (string)) : this.GetTypeData((XmlSchemaType) attr.SchemaType, typeQName, attr.Name, sharedAnnType, (XmlQualifiedName) null);
    }

    private TypeData GetTypeData(
      XmlQualifiedName typeQName,
      XmlQualifiedName root,
      bool isNullable)
    {
      if (this.IsPrimitiveTypeNamespace(typeQName.Namespace))
      {
        XmlTypeMapping xmlTypeMapping = this.ImportType(typeQName, root, false);
        return xmlTypeMapping != null ? xmlTypeMapping.TypeData : TypeTranslator.GetPrimitiveTypeData(typeQName.Name, isNullable);
      }
      return this.encodedFormat && typeQName.Namespace == string.Empty ? TypeTranslator.GetPrimitiveTypeData(typeQName.Name) : this.ImportType(typeQName, root, true).TypeData;
    }

    private TypeData GetTypeData(
      XmlSchemaType stype,
      XmlQualifiedName typeQNname,
      string propertyName,
      bool sharedAnnType,
      XmlQualifiedName root)
    {
      string identifier;
      if (sharedAnnType)
      {
        if (this.sharedAnonymousTypes[(object) stype] is TypeData sharedAnonymousType)
          return sharedAnonymousType;
        identifier = propertyName;
      }
      else
        identifier = typeQNname.Name + this.typeIdentifiers.MakeRightCase(propertyName);
      XmlTypeMapping xmlTypeMapping = this.ImportType(new XmlQualifiedName(this.elemIdentifiers.AddUnique(identifier, (object) stype), typeQNname.Namespace), stype, root);
      if (sharedAnnType)
        this.sharedAnonymousTypes[(object) stype] = (object) xmlTypeMapping.TypeData;
      return xmlTypeMapping.TypeData;
    }

    private XmlTypeMapping GetTypeMapping(TypeData typeData)
    {
      if (typeData.Type == typeof (object) && !this.anyTypeImported)
        this.ImportAllObjectTypes();
      XmlTypeMapping registeredTypeMapping = this.GetRegisteredTypeMapping(typeData);
      if (registeredTypeMapping != null)
        return registeredTypeMapping;
      if (typeData.IsListType)
      {
        XmlTypeMapping typeMapping = this.GetTypeMapping(typeData.ListItemTypeData);
        XmlTypeMapping map = new XmlTypeMapping(typeData.XmlType, typeMapping.Namespace, typeData, typeData.XmlType, typeMapping.Namespace);
        map.IncludeInSchema = true;
        ListMap listMap = new ListMap();
        listMap.ItemInfo = new XmlTypeMapElementInfoList();
        listMap.ItemInfo.Add((object) this.CreateElementInfo(typeMapping.Namespace, (XmlTypeMapMember) null, typeData.ListItemTypeData.XmlType, typeData.ListItemTypeData, false, XmlSchemaForm.None));
        map.ObjectMap = (ObjectMap) listMap;
        this.RegisterTypeMapping(new XmlQualifiedName(map.ElementName, map.Namespace), typeData, map);
        return map;
      }
      if (typeData.SchemaType == SchemaTypes.Primitive || typeData.Type == typeof (object) || typeof (XmlNode).IsAssignableFrom(typeData.Type))
        return this.CreateSystemMap(typeData);
      throw new InvalidOperationException("Map for type " + typeData.TypeName + " not found");
    }

    private void AddObjectDerivedMap(XmlTypeMapping map)
    {
      TypeData typeData = TypeTranslator.GetTypeData(typeof (object));
      (this.GetRegisteredTypeMapping(typeData) ?? this.CreateSystemMap(typeData)).DerivedTypes.Add((object) map);
    }

    private XmlTypeMapping CreateSystemMap(TypeData typeData)
    {
      XmlTypeMapping systemMap = new XmlTypeMapping(typeData.XmlType, "http://www.w3.org/2001/XMLSchema", typeData, typeData.XmlType, "http://www.w3.org/2001/XMLSchema");
      systemMap.IncludeInSchema = false;
      systemMap.ObjectMap = (ObjectMap) new ClassMap();
      this.dataMappedTypes[(object) typeData] = (object) systemMap;
      return systemMap;
    }

    private void ImportAllObjectTypes()
    {
      this.anyTypeImported = true;
      foreach (XmlSchema schema in (CollectionBase) this.schemas)
      {
        foreach (XmlSchemaObject xmlSchemaObject in schema.Items)
        {
          if (xmlSchemaObject is XmlSchemaComplexType stype)
            this.ImportType(new XmlQualifiedName(stype.Name, schema.TargetNamespace), (XmlSchemaType) stype, (XmlQualifiedName) null);
        }
      }
    }

    private XmlTypeMapping GetRegisteredTypeMapping(XmlQualifiedName typeQName, Type baseType) => this.IsPrimitiveTypeNamespace(typeQName.Namespace) ? (XmlTypeMapping) this.primitiveDerivedMappedTypes[(object) typeQName] : (XmlTypeMapping) this.mappedTypes[(object) typeQName];

    private XmlTypeMapping GetRegisteredTypeMapping(XmlQualifiedName typeQName) => (XmlTypeMapping) this.mappedTypes[(object) typeQName];

    private XmlTypeMapping GetRegisteredTypeMapping(TypeData typeData) => (XmlTypeMapping) this.dataMappedTypes[(object) typeData];

    private void RegisterTypeMapping(XmlQualifiedName qname, TypeData typeData, XmlTypeMapping map)
    {
      this.dataMappedTypes[(object) typeData] = (object) map;
      if (this.IsPrimitiveTypeNamespace(qname.Namespace) && !map.IsSimpleType)
        this.primitiveDerivedMappedTypes[(object) qname] = (object) map;
      else
        this.mappedTypes[(object) qname] = (object) map;
    }

    private XmlSchemaParticle GetRefGroupParticle(XmlSchemaGroupRef refGroup) => (XmlSchemaParticle) ((XmlSchemaGroup) this.schemas.Find(refGroup.RefName, typeof (XmlSchemaGroup))).Particle;

    private XmlSchemaElement FindRefElement(XmlSchemaElement elem)
    {
      XmlSchemaElement refElement = (XmlSchemaElement) this.schemas.Find(elem.RefName, typeof (XmlSchemaElement));
      if (refElement != null)
        return refElement;
      if (!this.IsPrimitiveTypeNamespace(elem.RefName.Namespace))
        return (XmlSchemaElement) null;
      if (this.anyElement != null)
        return this.anyElement;
      this.anyElement = new XmlSchemaElement();
      this.anyElement.Name = "any";
      this.anyElement.SchemaTypeName = XmlSchemaImporter.anyType;
      return this.anyElement;
    }

    private XmlSchemaAttribute FindRefAttribute(XmlQualifiedName refName)
    {
      if (!(refName.Namespace == "http://www.w3.org/XML/1998/namespace"))
        return (XmlSchemaAttribute) this.schemas.Find(refName, typeof (XmlSchemaAttribute));
      return new XmlSchemaAttribute()
      {
        Name = refName.Name,
        SchemaTypeName = new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema")
      };
    }

    private XmlSchemaAttributeGroup FindRefAttributeGroup(XmlQualifiedName refName)
    {
      XmlSchemaAttributeGroup refAttributeGroup = (XmlSchemaAttributeGroup) this.schemas.Find(refName, typeof (XmlSchemaAttributeGroup));
      foreach (XmlSchemaObject attribute in refAttributeGroup.Attributes)
      {
        if (attribute is XmlSchemaAttributeGroupRef && ((XmlSchemaAttributeGroupRef) attribute).RefName == refName)
          throw new InvalidOperationException("Cannot import attribute group '" + refName.Name + "' from namespace '" + refName.Namespace + "'. Redefine not supported");
      }
      return refAttributeGroup;
    }

    private XmlTypeMapping ReflectType(Type type) => this.ReflectType(TypeTranslator.GetTypeData(type), (string) null);

    private XmlTypeMapping ReflectType(TypeData typeData, string ns)
    {
      if (!this.encodedFormat)
      {
        if (this.auxXmlRefImporter == null)
          this.auxXmlRefImporter = new XmlReflectionImporter();
        return this.auxXmlRefImporter.ImportTypeMapping(typeData, ns);
      }
      if (this.auxSoapRefImporter == null)
        this.auxSoapRefImporter = new SoapReflectionImporter();
      return this.auxSoapRefImporter.ImportTypeMapping(typeData, ns);
    }

    private string GetDocumentation(XmlSchemaAnnotated elem)
    {
      string empty = string.Empty;
      XmlSchemaAnnotation annotation = elem.Annotation;
      if (annotation == null || annotation.Items == null)
        return (string) null;
      foreach (object obj in annotation.Items)
      {
        if (obj is XmlSchemaDocumentation schemaDocumentation && schemaDocumentation.Markup != null && schemaDocumentation.Markup.Length > 0)
        {
          if (empty != string.Empty)
            empty += "\n";
          foreach (XmlNode xmlNode in schemaDocumentation.Markup)
            empty += xmlNode.Value;
        }
      }
      return empty;
    }

    private bool IsPrimitiveTypeNamespace(string ns)
    {
      if (ns == "http://www.w3.org/2001/XMLSchema")
        return true;
      return this.encodedFormat && ns == "http://schemas.xmlsoap.org/soap/encoding/";
    }

    private class MapFixup
    {
      public XmlTypeMapping Map;
      public XmlSchemaComplexType SchemaType;
      public XmlQualifiedName TypeName;
    }
  }
}
