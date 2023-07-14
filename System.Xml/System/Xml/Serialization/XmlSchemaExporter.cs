// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.XmlSchemaExporter
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.Xml.Schema;

namespace System.Xml.Serialization
{
  public class XmlSchemaExporter
  {
    private XmlSchemas schemas;
    private Hashtable exportedMaps = new Hashtable();
    private Hashtable exportedElements = new Hashtable();
    private bool encodedFormat;
    private XmlDocument xmlDoc;

    public XmlSchemaExporter(XmlSchemas schemas) => this.schemas = schemas;

    internal XmlSchemaExporter(XmlSchemas schemas, bool encodedFormat)
    {
      this.encodedFormat = encodedFormat;
      this.schemas = schemas;
    }

    [MonoTODO]
    public string ExportAnyType(string ns) => throw new NotImplementedException();

    [MonoNotSupported("")]
    public string ExportAnyType(XmlMembersMapping members) => throw new NotImplementedException();

    public void ExportMembersMapping(XmlMembersMapping xmlMembersMapping) => this.ExportMembersMapping(xmlMembersMapping, true);

    public void ExportMembersMapping(XmlMembersMapping xmlMembersMapping, bool exportEnclosingType)
    {
      ClassMap objectMap = (ClassMap) xmlMembersMapping.ObjectMap;
      if (xmlMembersMapping.HasWrapperElement && exportEnclosingType)
      {
        XmlSchema schema = this.GetSchema(xmlMembersMapping.Namespace);
        XmlSchemaComplexType schemaComplexType = new XmlSchemaComplexType();
        XmlSchemaSequence particle;
        XmlSchemaAnyAttribute anyAttribute;
        this.ExportMembersMapSchema(schema, objectMap, (XmlTypeMapping) null, schemaComplexType.Attributes, out particle, out anyAttribute);
        schemaComplexType.Particle = (XmlSchemaParticle) particle;
        schemaComplexType.AnyAttribute = anyAttribute;
        if (this.encodedFormat)
        {
          schemaComplexType.Name = xmlMembersMapping.ElementName;
          schema.Items.Add((XmlSchemaObject) schemaComplexType);
        }
        else
          schema.Items.Add((XmlSchemaObject) new XmlSchemaElement()
          {
            Name = xmlMembersMapping.ElementName,
            SchemaType = (XmlSchemaType) schemaComplexType
          });
      }
      else
      {
        ICollection elementMembers = objectMap.ElementMembers;
        if (elementMembers != null)
        {
          foreach (XmlTypeMapMemberElement mapMemberElement in (IEnumerable) elementMembers)
          {
            if (mapMemberElement is XmlTypeMapMemberAnyElement && mapMemberElement.TypeData.IsListType)
            {
              XmlSchema schema = this.GetSchema(xmlMembersMapping.Namespace);
              XmlSchemaParticle schemaArrayElement = this.GetSchemaArrayElement(schema, mapMemberElement.ElementInfo);
              if (schemaArrayElement is XmlSchemaAny)
              {
                if (this.FindComplexType(schema.Items, "any") == null)
                {
                  XmlSchemaComplexType schemaComplexType = new XmlSchemaComplexType();
                  schemaComplexType.Name = "any";
                  schemaComplexType.IsMixed = true;
                  XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
                  schemaComplexType.Particle = (XmlSchemaParticle) xmlSchemaSequence;
                  xmlSchemaSequence.Items.Add((XmlSchemaObject) schemaArrayElement);
                  schema.Items.Add((XmlSchemaObject) schemaComplexType);
                  continue;
                }
                continue;
              }
            }
            XmlTypeMapElementInfo einfo = (XmlTypeMapElementInfo) mapMemberElement.ElementInfo[0];
            XmlSchema schema1;
            if (this.encodedFormat)
            {
              schema1 = this.GetSchema(xmlMembersMapping.Namespace);
              this.ImportNamespace(schema1, "http://schemas.xmlsoap.org/soap/encoding/");
            }
            else
              schema1 = this.GetSchema(einfo.Namespace);
            XmlSchemaElement element = this.FindElement(schema1.Items, einfo.ElementName);
            XmlSchemaExporter.XmlSchemaObjectContainer container = (XmlSchemaExporter.XmlSchemaObjectContainer) null;
            if (!this.encodedFormat)
              container = new XmlSchemaExporter.XmlSchemaObjectContainer(schema1);
            Type type = mapMemberElement.GetType();
            if (mapMemberElement is XmlTypeMapMemberFlatList)
              throw new InvalidOperationException("Unwrapped arrays not supported as parameters");
            XmlSchemaElement xmlSchemaElement = type != typeof (XmlTypeMapMemberElement) ? (XmlSchemaElement) this.GetSchemaElement(schema1, einfo, false, container) : (XmlSchemaElement) this.GetSchemaElement(schema1, einfo, mapMemberElement.DefaultValue, false, container);
            if (element != null)
            {
              if (element.SchemaTypeName.Equals((object) xmlSchemaElement.SchemaTypeName))
                schema1.Items.Remove((XmlSchemaObject) xmlSchemaElement);
              else
                throw new InvalidOperationException("The XML element named '" + einfo.ElementName + "' " + "from namespace '" + schema1.TargetNamespace + "' references distinct types " + xmlSchemaElement.SchemaTypeName.Name + " and " + element.SchemaTypeName.Name + ". " + "Use XML attributes to specify another XML name or namespace for the element or types.");
            }
          }
        }
      }
      this.CompileSchemas();
    }

    [MonoTODO]
    public XmlQualifiedName ExportTypeMapping(XmlMembersMapping xmlMembersMapping) => throw new NotImplementedException();

    public void ExportTypeMapping(XmlTypeMapping xmlTypeMapping)
    {
      if (!xmlTypeMapping.IncludeInSchema || this.IsElementExported(xmlTypeMapping))
        return;
      if (this.encodedFormat)
      {
        this.ExportClassSchema(xmlTypeMapping);
        this.ImportNamespace(this.GetSchema(xmlTypeMapping.XmlTypeNamespace), "http://schemas.xmlsoap.org/soap/encoding/");
      }
      else
      {
        XmlSchema schema = this.GetSchema(xmlTypeMapping.Namespace);
        XmlTypeMapElementInfo einfo = new XmlTypeMapElementInfo((XmlTypeMapMember) null, xmlTypeMapping.TypeData);
        einfo.Namespace = xmlTypeMapping.Namespace;
        einfo.ElementName = xmlTypeMapping.ElementName;
        if (xmlTypeMapping.TypeData.IsComplexType)
          einfo.MappedType = xmlTypeMapping;
        einfo.IsNullable = xmlTypeMapping.IsNullable;
        this.GetSchemaElement(schema, einfo, false, new XmlSchemaExporter.XmlSchemaObjectContainer(schema));
        this.SetElementExported(xmlTypeMapping);
      }
      this.CompileSchemas();
    }

    private void ExportXmlSerializableSchema(XmlSchema currentSchema, XmlSerializableMapping map)
    {
      if (this.IsMapExported((XmlTypeMapping) map))
        return;
      this.SetMapExported((XmlTypeMapping) map);
      if (map.Schema == null)
        return;
      string targetNamespace = map.Schema.TargetNamespace;
      XmlSchema schema = this.schemas[targetNamespace];
      if (schema == null)
      {
        this.schemas.Add(map.Schema);
        this.ImportNamespace(currentSchema, targetNamespace);
      }
      else if (schema != map.Schema && !XmlSchemaExporter.CanBeDuplicated(schema, map.Schema))
        throw new InvalidOperationException("The namespace '" + targetNamespace + "' defined by the class '" + map.TypeFullName + "' is a duplicate.");
    }

    private static bool CanBeDuplicated(XmlSchema existingSchema, XmlSchema schema) => XmlSchemas.IsDataSet(existingSchema) && XmlSchemas.IsDataSet(schema) && existingSchema.Id == schema.Id;

    private void ExportClassSchema(XmlTypeMapping map)
    {
      if (this.IsMapExported(map))
        return;
      this.SetMapExported(map);
      if (map.TypeData.Type == typeof (object))
      {
        foreach (XmlTypeMapping derivedType in map.DerivedTypes)
        {
          if (derivedType.TypeData.SchemaType == SchemaTypes.Class)
            this.ExportClassSchema(derivedType);
        }
      }
      else
      {
        XmlSchema schema = this.GetSchema(map.XmlTypeNamespace);
        XmlSchemaComplexType schemaComplexType = new XmlSchemaComplexType();
        schemaComplexType.Name = map.XmlType;
        schema.Items.Add((XmlSchemaObject) schemaComplexType);
        ClassMap objectMap = (ClassMap) map.ObjectMap;
        if (objectMap.HasSimpleContent)
        {
          XmlSchemaSimpleContent schemaSimpleContent = new XmlSchemaSimpleContent();
          schemaComplexType.ContentModel = (XmlSchemaContentModel) schemaSimpleContent;
          XmlSchemaSimpleContentExtension contentExtension = new XmlSchemaSimpleContentExtension();
          schemaSimpleContent.Content = (XmlSchemaContent) contentExtension;
          XmlSchemaAnyAttribute anyAttribute;
          this.ExportMembersMapSchema(schema, objectMap, map.BaseMap, contentExtension.Attributes, out XmlSchemaSequence _, out anyAttribute);
          contentExtension.AnyAttribute = anyAttribute;
          if (map.BaseMap == null)
          {
            contentExtension.BaseTypeName = objectMap.SimpleContentBaseType;
          }
          else
          {
            contentExtension.BaseTypeName = new XmlQualifiedName(map.BaseMap.XmlType, map.BaseMap.XmlTypeNamespace);
            this.ImportNamespace(schema, map.BaseMap.XmlTypeNamespace);
            this.ExportClassSchema(map.BaseMap);
          }
        }
        else if (map.BaseMap != null && map.BaseMap.IncludeInSchema)
        {
          XmlSchemaComplexContent schemaComplexContent = new XmlSchemaComplexContent();
          XmlSchemaComplexContentExtension contentExtension = new XmlSchemaComplexContentExtension();
          contentExtension.BaseTypeName = new XmlQualifiedName(map.BaseMap.XmlType, map.BaseMap.XmlTypeNamespace);
          schemaComplexContent.Content = (XmlSchemaContent) contentExtension;
          schemaComplexType.ContentModel = (XmlSchemaContentModel) schemaComplexContent;
          XmlSchemaSequence particle;
          XmlSchemaAnyAttribute anyAttribute;
          this.ExportMembersMapSchema(schema, objectMap, map.BaseMap, contentExtension.Attributes, out particle, out anyAttribute);
          contentExtension.Particle = (XmlSchemaParticle) particle;
          contentExtension.AnyAttribute = anyAttribute;
          schemaComplexType.IsMixed = this.HasMixedContent(map);
          schemaComplexContent.IsMixed = this.BaseHasMixedContent(map);
          this.ImportNamespace(schema, map.BaseMap.XmlTypeNamespace);
          this.ExportClassSchema(map.BaseMap);
        }
        else
        {
          XmlSchemaSequence particle;
          XmlSchemaAnyAttribute anyAttribute;
          this.ExportMembersMapSchema(schema, objectMap, map.BaseMap, schemaComplexType.Attributes, out particle, out anyAttribute);
          schemaComplexType.Particle = (XmlSchemaParticle) particle;
          schemaComplexType.AnyAttribute = anyAttribute;
          schemaComplexType.IsMixed = objectMap.XmlTextCollector != null;
        }
        foreach (XmlTypeMapping derivedType in map.DerivedTypes)
        {
          if (derivedType.TypeData.SchemaType == SchemaTypes.Class)
            this.ExportClassSchema(derivedType);
        }
      }
    }

    private bool BaseHasMixedContent(XmlTypeMapping map)
    {
      ClassMap objectMap = (ClassMap) map.ObjectMap;
      return objectMap.XmlTextCollector != null && map.BaseMap != null && this.DefinedInBaseMap(map.BaseMap, objectMap.XmlTextCollector);
    }

    private bool HasMixedContent(XmlTypeMapping map)
    {
      ClassMap objectMap = (ClassMap) map.ObjectMap;
      if (objectMap.XmlTextCollector == null)
        return false;
      return map.BaseMap == null || !this.DefinedInBaseMap(map.BaseMap, objectMap.XmlTextCollector);
    }

    private void ExportMembersMapSchema(
      XmlSchema schema,
      ClassMap map,
      XmlTypeMapping baseMap,
      XmlSchemaObjectCollection outAttributes,
      out XmlSchemaSequence particle,
      out XmlSchemaAnyAttribute anyAttribute)
    {
      particle = (XmlSchemaSequence) null;
      XmlSchemaSequence group = new XmlSchemaSequence();
      ICollection elementMembers = map.ElementMembers;
      if (elementMembers != null && !map.HasSimpleContent)
      {
        foreach (XmlTypeMapMemberElement member in (IEnumerable) elementMembers)
        {
          if (baseMap == null || !this.DefinedInBaseMap(baseMap, (XmlTypeMapMember) member))
          {
            Type type = member.GetType();
            if (type == typeof (XmlTypeMapMemberFlatList))
            {
              XmlSchemaParticle schemaArrayElement = this.GetSchemaArrayElement(schema, member.ElementInfo);
              if (schemaArrayElement != null)
                group.Items.Add((XmlSchemaObject) schemaArrayElement);
            }
            else if (type == typeof (XmlTypeMapMemberAnyElement))
              group.Items.Add((XmlSchemaObject) this.GetSchemaArrayElement(schema, member.ElementInfo));
            else if (type == typeof (XmlTypeMapMemberElement))
              this.GetSchemaElement(schema, (XmlTypeMapElementInfo) member.ElementInfo[0], member.DefaultValue, true, new XmlSchemaExporter.XmlSchemaObjectContainer((XmlSchemaGroupBase) group));
            else
              this.GetSchemaElement(schema, (XmlTypeMapElementInfo) member.ElementInfo[0], true, new XmlSchemaExporter.XmlSchemaObjectContainer((XmlSchemaGroupBase) group));
          }
        }
      }
      if (group.Items.Count > 0)
        particle = group;
      ICollection attributeMembers = map.AttributeMembers;
      if (attributeMembers != null)
      {
        foreach (XmlTypeMapMemberAttribute mapMemberAttribute in (IEnumerable) attributeMembers)
        {
          if (baseMap == null || !this.DefinedInBaseMap(baseMap, (XmlTypeMapMember) mapMemberAttribute))
            outAttributes.Add((XmlSchemaObject) this.GetSchemaAttribute(schema, mapMemberAttribute, true));
        }
      }
      if ((XmlTypeMapMember) map.DefaultAnyAttributeMember != null)
        anyAttribute = new XmlSchemaAnyAttribute();
      else
        anyAttribute = (XmlSchemaAnyAttribute) null;
    }

    private XmlSchemaElement FindElement(XmlSchemaObjectCollection col, string name)
    {
      foreach (XmlSchemaObject xmlSchemaObject in col)
      {
        if (xmlSchemaObject is XmlSchemaElement element && element.Name == name)
          return element;
      }
      return (XmlSchemaElement) null;
    }

    private XmlSchemaComplexType FindComplexType(XmlSchemaObjectCollection col, string name)
    {
      foreach (XmlSchemaObject xmlSchemaObject in col)
      {
        if (xmlSchemaObject is XmlSchemaComplexType complexType && complexType.Name == name)
          return complexType;
      }
      return (XmlSchemaComplexType) null;
    }

    private XmlSchemaAttribute GetSchemaAttribute(
      XmlSchema currentSchema,
      XmlTypeMapMemberAttribute attinfo,
      bool isTypeMember)
    {
      XmlSchemaAttribute schemaAttribute = new XmlSchemaAttribute();
      if (attinfo.DefaultValue != DBNull.Value)
        schemaAttribute.DefaultValue = this.ExportDefaultValue(attinfo.TypeData, attinfo.MappedType, attinfo.DefaultValue);
      else if (!attinfo.IsOptionalValueType && attinfo.TypeData.IsValueType)
        schemaAttribute.Use = XmlSchemaUse.Required;
      this.ImportNamespace(currentSchema, attinfo.Namespace);
      XmlSchema currentSchema1 = attinfo.Namespace.Length != 0 || attinfo.Form == XmlSchemaForm.Qualified ? this.GetSchema(attinfo.Namespace) : currentSchema;
      if (currentSchema == currentSchema1 || this.encodedFormat)
      {
        schemaAttribute.Name = attinfo.AttributeName;
        if (isTypeMember)
          schemaAttribute.Form = attinfo.Form;
        if (attinfo.TypeData.SchemaType == SchemaTypes.Enum)
        {
          this.ImportNamespace(currentSchema, attinfo.DataTypeNamespace);
          this.ExportEnumSchema(attinfo.MappedType);
          schemaAttribute.SchemaTypeName = new XmlQualifiedName(attinfo.TypeData.XmlType, attinfo.DataTypeNamespace);
        }
        else if (attinfo.TypeData.SchemaType == SchemaTypes.Array && TypeTranslator.IsPrimitive(attinfo.TypeData.ListItemType))
          schemaAttribute.SchemaType = this.GetSchemaSimpleListType(attinfo.TypeData);
        else
          schemaAttribute.SchemaTypeName = new XmlQualifiedName(attinfo.TypeData.XmlType, attinfo.DataTypeNamespace);
      }
      else
      {
        schemaAttribute.RefName = new XmlQualifiedName(attinfo.AttributeName, attinfo.Namespace);
        foreach (XmlSchemaObject xmlSchemaObject in currentSchema1.Items)
        {
          if (xmlSchemaObject is XmlSchemaAttribute && ((XmlSchemaAttribute) xmlSchemaObject).Name == attinfo.AttributeName)
            return schemaAttribute;
        }
        currentSchema1.Items.Add((XmlSchemaObject) this.GetSchemaAttribute(currentSchema1, attinfo, false));
      }
      return schemaAttribute;
    }

    private XmlSchemaParticle GetSchemaElement(
      XmlSchema currentSchema,
      XmlTypeMapElementInfo einfo,
      bool isTypeMember)
    {
      return this.GetSchemaElement(currentSchema, einfo, (object) DBNull.Value, isTypeMember, (XmlSchemaExporter.XmlSchemaObjectContainer) null);
    }

    private XmlSchemaParticle GetSchemaElement(
      XmlSchema currentSchema,
      XmlTypeMapElementInfo einfo,
      bool isTypeMember,
      XmlSchemaExporter.XmlSchemaObjectContainer container)
    {
      return this.GetSchemaElement(currentSchema, einfo, (object) DBNull.Value, isTypeMember, container);
    }

    private XmlSchemaParticle GetSchemaElement(
      XmlSchema currentSchema,
      XmlTypeMapElementInfo einfo,
      object defaultValue,
      bool isTypeMember,
      XmlSchemaExporter.XmlSchemaObjectContainer container)
    {
      if (einfo.IsTextElement)
        return (XmlSchemaParticle) null;
      if (einfo.IsUnnamedAnyElement)
      {
        XmlSchemaAny schemaElement = new XmlSchemaAny();
        schemaElement.MinOccurs = 0M;
        schemaElement.MaxOccurs = 1M;
        container?.Items.Add((XmlSchemaObject) schemaElement);
        return (XmlSchemaParticle) schemaElement;
      }
      XmlSchemaElement elem = new XmlSchemaElement();
      elem.IsNillable = einfo.IsNullable;
      container?.Items.Add((XmlSchemaObject) elem);
      if (isTypeMember)
      {
        elem.MaxOccurs = 1M;
        elem.MinOccurs = (Decimal) (!einfo.IsNullable ? 0 : 1);
        if (defaultValue == DBNull.Value && einfo.TypeData.IsValueType && einfo.Member != null && !einfo.Member.IsOptionalValueType || this.encodedFormat)
          elem.MinOccurs = 1M;
      }
      XmlSchema xmlSchema = (XmlSchema) null;
      if (!this.encodedFormat)
      {
        xmlSchema = this.GetSchema(einfo.Namespace);
        this.ImportNamespace(currentSchema, einfo.Namespace);
      }
      if (currentSchema == xmlSchema || this.encodedFormat || !isTypeMember)
      {
        if (isTypeMember)
          elem.IsNillable = einfo.IsNullable;
        elem.Name = einfo.ElementName;
        if (defaultValue != DBNull.Value)
          elem.DefaultValue = this.ExportDefaultValue(einfo.TypeData, einfo.MappedType, defaultValue);
        if (einfo.Form != XmlSchemaForm.Qualified)
          elem.Form = einfo.Form;
        switch (einfo.TypeData.SchemaType)
        {
          case SchemaTypes.Primitive:
            elem.SchemaTypeName = new XmlQualifiedName(einfo.TypeData.XmlType, einfo.DataTypeNamespace);
            if (!einfo.TypeData.IsXsdType)
            {
              this.ImportNamespace(currentSchema, einfo.MappedType.XmlTypeNamespace);
              this.ExportDerivedSchema(einfo.MappedType);
              break;
            }
            break;
          case SchemaTypes.Enum:
            elem.SchemaTypeName = new XmlQualifiedName(einfo.MappedType.XmlType, einfo.MappedType.XmlTypeNamespace);
            this.ImportNamespace(currentSchema, einfo.MappedType.XmlTypeNamespace);
            this.ExportEnumSchema(einfo.MappedType);
            break;
          case SchemaTypes.Array:
            XmlQualifiedName xmlQualifiedName = this.ExportArraySchema(einfo.MappedType, currentSchema.TargetNamespace);
            elem.SchemaTypeName = xmlQualifiedName;
            this.ImportNamespace(currentSchema, xmlQualifiedName.Namespace);
            break;
          case SchemaTypes.Class:
            if (einfo.MappedType.TypeData.Type != typeof (object))
            {
              elem.SchemaTypeName = new XmlQualifiedName(einfo.MappedType.XmlType, einfo.MappedType.XmlTypeNamespace);
              this.ImportNamespace(currentSchema, einfo.MappedType.XmlTypeNamespace);
            }
            else if (this.encodedFormat)
              elem.SchemaTypeName = new XmlQualifiedName(einfo.MappedType.XmlType, einfo.MappedType.XmlTypeNamespace);
            this.ExportClassSchema(einfo.MappedType);
            break;
          case SchemaTypes.XmlSerializable:
            this.SetSchemaXmlSerializableType(einfo.MappedType as XmlSerializableMapping, elem);
            this.ExportXmlSerializableSchema(currentSchema, einfo.MappedType as XmlSerializableMapping);
            break;
          case SchemaTypes.XmlNode:
            elem.SchemaType = this.GetSchemaXmlNodeType();
            break;
        }
      }
      else
      {
        elem.RefName = new XmlQualifiedName(einfo.ElementName, einfo.Namespace);
        foreach (XmlSchemaObject xmlSchemaObject in xmlSchema.Items)
        {
          if (xmlSchemaObject is XmlSchemaElement && ((XmlSchemaElement) xmlSchemaObject).Name == einfo.ElementName)
            return (XmlSchemaParticle) elem;
        }
        this.GetSchemaElement(xmlSchema, einfo, defaultValue, false, new XmlSchemaExporter.XmlSchemaObjectContainer(xmlSchema));
      }
      return (XmlSchemaParticle) elem;
    }

    private void ImportNamespace(XmlSchema schema, string ns)
    {
      switch (ns)
      {
        case null:
          break;
        case "":
          break;
        default:
          if (ns == schema.TargetNamespace || ns == "http://www.w3.org/2001/XMLSchema")
            break;
          foreach (XmlSchemaObject include in schema.Includes)
          {
            if (include is XmlSchemaImport && ((XmlSchemaImport) include).Namespace == ns)
              return;
          }
          schema.Includes.Add((XmlSchemaObject) new XmlSchemaImport()
          {
            Namespace = ns
          });
          break;
      }
    }

    private bool DefinedInBaseMap(XmlTypeMapping map, XmlTypeMapMember member)
    {
      if (((ClassMap) map.ObjectMap).FindMember(member.Name) != null)
        return true;
      return map.BaseMap != null && this.DefinedInBaseMap(map.BaseMap, member);
    }

    private XmlSchemaType GetSchemaXmlNodeType() => (XmlSchemaType) new XmlSchemaComplexType()
    {
      IsMixed = true,
      Particle = (XmlSchemaParticle) new XmlSchemaSequence()
      {
        Items = {
          (XmlSchemaObject) new XmlSchemaAny()
        }
      }
    };

    private void SetSchemaXmlSerializableType(XmlSerializableMapping map, XmlSchemaElement elem)
    {
      if (map.SchemaType != null && map.Schema != null)
        elem.SchemaType = map.SchemaType;
      else if (map.SchemaType == null && map.SchemaTypeName != (XmlQualifiedName) null)
      {
        elem.SchemaTypeName = map.SchemaTypeName;
        elem.Name = map.SchemaTypeName.Name;
      }
      else
      {
        XmlSchemaComplexType schemaComplexType = new XmlSchemaComplexType();
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        if (map.Schema == null)
        {
          xmlSchemaSequence.Items.Add((XmlSchemaObject) new XmlSchemaElement()
          {
            RefName = new XmlQualifiedName("schema", "http://www.w3.org/2001/XMLSchema")
          });
          xmlSchemaSequence.Items.Add((XmlSchemaObject) new XmlSchemaAny());
        }
        else
          xmlSchemaSequence.Items.Add((XmlSchemaObject) new XmlSchemaAny()
          {
            Namespace = map.Schema.TargetNamespace
          });
        schemaComplexType.Particle = (XmlSchemaParticle) xmlSchemaSequence;
        elem.SchemaType = (XmlSchemaType) schemaComplexType;
      }
    }

    private XmlSchemaSimpleType GetSchemaSimpleListType(TypeData typeData)
    {
      XmlSchemaSimpleType schemaSimpleListType = new XmlSchemaSimpleType();
      XmlSchemaSimpleTypeList schemaSimpleTypeList = new XmlSchemaSimpleTypeList();
      TypeData typeData1 = TypeTranslator.GetTypeData(typeData.ListItemType);
      schemaSimpleTypeList.ItemTypeName = new XmlQualifiedName(typeData1.XmlType, "http://www.w3.org/2001/XMLSchema");
      schemaSimpleListType.Content = (XmlSchemaSimpleTypeContent) schemaSimpleTypeList;
      return schemaSimpleListType;
    }

    private XmlSchemaParticle GetSchemaArrayElement(
      XmlSchema currentSchema,
      XmlTypeMapElementInfoList infos)
    {
      int count = infos.Count;
      if (count > 0 && ((XmlTypeMapElementInfo) infos[0]).IsTextElement)
        --count;
      if (count == 0)
        return (XmlSchemaParticle) null;
      if (count == 1)
      {
        XmlSchemaParticle schemaElement = this.GetSchemaElement(currentSchema, (XmlTypeMapElementInfo) infos[infos.Count - 1], true);
        schemaElement.MinOccursString = "0";
        schemaElement.MaxOccursString = "unbounded";
        return schemaElement;
      }
      XmlSchemaChoice schemaArrayElement = new XmlSchemaChoice();
      schemaArrayElement.MinOccursString = "0";
      schemaArrayElement.MaxOccursString = "unbounded";
      foreach (XmlTypeMapElementInfo info in (ArrayList) infos)
      {
        if (!info.IsTextElement)
          schemaArrayElement.Items.Add((XmlSchemaObject) this.GetSchemaElement(currentSchema, info, true));
      }
      return (XmlSchemaParticle) schemaArrayElement;
    }

    private string ExportDefaultValue(TypeData typeData, XmlTypeMapping map, object defaultValue) => typeData.SchemaType == SchemaTypes.Enum ? ((EnumMap) map.ObjectMap).GetXmlName(map.TypeFullName, defaultValue) : XmlCustomFormatter.ToXmlString(typeData, defaultValue);

    private void ExportDerivedSchema(XmlTypeMapping map)
    {
      if (this.IsMapExported(map))
        return;
      this.SetMapExported(map);
      XmlSchema schema = this.GetSchema(map.XmlTypeNamespace);
      for (int index = 0; index < schema.Items.Count; ++index)
      {
        if (schema.Items[index] is XmlSchemaSimpleType schemaSimpleType && schemaSimpleType.Name == map.ElementName)
          return;
      }
      XmlSchemaSimpleType schemaSimpleType1 = new XmlSchemaSimpleType();
      schemaSimpleType1.Name = map.ElementName;
      schema.Items.Add((XmlSchemaObject) schemaSimpleType1);
      XmlSchemaSimpleTypeRestriction simpleTypeRestriction = new XmlSchemaSimpleTypeRestriction();
      simpleTypeRestriction.BaseTypeName = new XmlQualifiedName(map.TypeData.MappedType.XmlType, "http://www.w3.org/2001/XMLSchema");
      XmlSchemaPatternFacet schemaPatternFacet = map.TypeData.XmlSchemaPatternFacet;
      if (schemaPatternFacet != null)
        simpleTypeRestriction.Facets.Add((XmlSchemaObject) schemaPatternFacet);
      schemaSimpleType1.Content = (XmlSchemaSimpleTypeContent) simpleTypeRestriction;
    }

    private void ExportEnumSchema(XmlTypeMapping map)
    {
      if (this.IsMapExported(map))
        return;
      this.SetMapExported(map);
      XmlSchema schema = this.GetSchema(map.XmlTypeNamespace);
      XmlSchemaSimpleType schemaSimpleType = new XmlSchemaSimpleType();
      schemaSimpleType.Name = map.ElementName;
      schema.Items.Add((XmlSchemaObject) schemaSimpleType);
      XmlSchemaSimpleTypeRestriction simpleTypeRestriction = new XmlSchemaSimpleTypeRestriction();
      simpleTypeRestriction.BaseTypeName = new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema");
      EnumMap objectMap = (EnumMap) map.ObjectMap;
      foreach (EnumMap.EnumMapMember member in objectMap.Members)
      {
        XmlSchemaEnumerationFacet enumerationFacet = new XmlSchemaEnumerationFacet();
        enumerationFacet.Value = member.XmlName;
        simpleTypeRestriction.Facets.Add((XmlSchemaObject) enumerationFacet);
      }
      if (objectMap.IsFlags)
        schemaSimpleType.Content = (XmlSchemaSimpleTypeContent) new XmlSchemaSimpleTypeList()
        {
          ItemType = new XmlSchemaSimpleType()
          {
            Content = (XmlSchemaSimpleTypeContent) simpleTypeRestriction
          }
        };
      else
        schemaSimpleType.Content = (XmlSchemaSimpleTypeContent) simpleTypeRestriction;
    }

    private XmlQualifiedName ExportArraySchema(XmlTypeMapping map, string defaultNamespace)
    {
      ListMap objectMap = (ListMap) map.ObjectMap;
      if (this.encodedFormat)
      {
        string localName;
        string ns;
        objectMap.GetArrayType(-1, out localName, out ns);
        string str = !(ns == "http://www.w3.org/2001/XMLSchema") ? ns : defaultNamespace;
        if (this.IsMapExported(map))
          return new XmlQualifiedName(objectMap.GetSchemaArrayName(), str);
        this.SetMapExported(map);
        XmlSchema schema = this.GetSchema(str);
        XmlSchemaComplexType schemaComplexType = new XmlSchemaComplexType();
        schemaComplexType.Name = objectMap.GetSchemaArrayName();
        schema.Items.Add((XmlSchemaObject) schemaComplexType);
        XmlSchemaComplexContent schemaComplexContent = new XmlSchemaComplexContent();
        schemaComplexContent.IsMixed = false;
        schemaComplexType.ContentModel = (XmlSchemaContentModel) schemaComplexContent;
        XmlSchemaComplexContentRestriction contentRestriction = new XmlSchemaComplexContentRestriction();
        schemaComplexContent.Content = (XmlSchemaContent) contentRestriction;
        contentRestriction.BaseTypeName = new XmlQualifiedName("Array", "http://schemas.xmlsoap.org/soap/encoding/");
        XmlSchemaAttribute xmlSchemaAttribute = new XmlSchemaAttribute();
        contentRestriction.Attributes.Add((XmlSchemaObject) xmlSchemaAttribute);
        xmlSchemaAttribute.RefName = new XmlQualifiedName("arrayType", "http://schemas.xmlsoap.org/soap/encoding/");
        XmlAttribute attribute = this.Document.CreateAttribute("arrayType", "http://schemas.xmlsoap.org/wsdl/");
        attribute.Value = ns + (!(ns != string.Empty) ? string.Empty : ":") + localName;
        xmlSchemaAttribute.UnhandledAttributes = new XmlAttribute[1]
        {
          attribute
        };
        this.ImportNamespace(schema, "http://schemas.xmlsoap.org/wsdl/");
        XmlTypeMapElementInfo typeMapElementInfo = (XmlTypeMapElementInfo) objectMap.ItemInfo[0];
        if (typeMapElementInfo.MappedType != null)
        {
          switch (typeMapElementInfo.TypeData.SchemaType)
          {
            case SchemaTypes.Enum:
              this.ExportEnumSchema(typeMapElementInfo.MappedType);
              break;
            case SchemaTypes.Array:
              this.ExportArraySchema(typeMapElementInfo.MappedType, str);
              break;
            case SchemaTypes.Class:
              this.ExportClassSchema(typeMapElementInfo.MappedType);
              break;
          }
        }
        return new XmlQualifiedName(objectMap.GetSchemaArrayName(), str);
      }
      if (this.IsMapExported(map))
        return new XmlQualifiedName(map.XmlType, map.XmlTypeNamespace);
      this.SetMapExported(map);
      XmlSchema schema1 = this.GetSchema(map.XmlTypeNamespace);
      XmlSchemaComplexType schemaComplexType1 = new XmlSchemaComplexType();
      schemaComplexType1.Name = map.ElementName;
      schema1.Items.Add((XmlSchemaObject) schemaComplexType1);
      XmlSchemaParticle schemaArrayElement = this.GetSchemaArrayElement(schema1, objectMap.ItemInfo);
      if (schemaArrayElement is XmlSchemaChoice)
        schemaComplexType1.Particle = schemaArrayElement;
      else
        schemaComplexType1.Particle = (XmlSchemaParticle) new XmlSchemaSequence()
        {
          Items = {
            (XmlSchemaObject) schemaArrayElement
          }
        };
      return new XmlQualifiedName(map.XmlType, map.XmlTypeNamespace);
    }

    private XmlDocument Document
    {
      get
      {
        if (this.xmlDoc == null)
          this.xmlDoc = new XmlDocument();
        return this.xmlDoc;
      }
    }

    private bool IsMapExported(XmlTypeMapping map) => this.exportedMaps.ContainsKey((object) this.GetMapKey(map));

    private void SetMapExported(XmlTypeMapping map) => this.exportedMaps[(object) this.GetMapKey(map)] = (object) map;

    private bool IsElementExported(XmlTypeMapping map) => this.exportedElements.ContainsKey((object) this.GetMapKey(map)) || map.TypeData.Type == typeof (object);

    private void SetElementExported(XmlTypeMapping map) => this.exportedElements[(object) this.GetMapKey(map)] = (object) map;

    private string GetMapKey(XmlTypeMapping map) => map.TypeData.IsListType ? this.GetArrayKeyName(map.TypeData) + " " + map.XmlType + " " + map.XmlTypeNamespace : map.TypeData.FullTypeName + " " + map.XmlType + " " + map.XmlTypeNamespace;

    private string GetArrayKeyName(TypeData td)
    {
      TypeData listItemTypeData = td.ListItemTypeData;
      return "*arrayof*" + (!listItemTypeData.IsListType ? listItemTypeData.FullTypeName : this.GetArrayKeyName(listItemTypeData));
    }

    private void CompileSchemas()
    {
    }

    private XmlSchema GetSchema(string ns)
    {
      XmlSchema schema = this.schemas[ns];
      if (schema == null)
      {
        schema = new XmlSchema();
        if (ns != null && ns.Length > 0)
          schema.TargetNamespace = ns;
        if (!this.encodedFormat)
          schema.ElementFormDefault = XmlSchemaForm.Qualified;
        this.schemas.Add(schema);
      }
      return schema;
    }

    private class XmlSchemaObjectContainer
    {
      private readonly XmlSchemaObject _xmlSchemaObject;

      public XmlSchemaObjectContainer(XmlSchema schema) => this._xmlSchemaObject = (XmlSchemaObject) schema;

      public XmlSchemaObjectContainer(XmlSchemaGroupBase group) => this._xmlSchemaObject = (XmlSchemaObject) group;

      public XmlSchemaObjectCollection Items => this._xmlSchemaObject is XmlSchema ? ((XmlSchema) this._xmlSchemaObject).Items : ((XmlSchemaGroupBase) this._xmlSchemaObject).Items;
    }
  }
}
