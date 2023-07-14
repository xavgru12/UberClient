// Decompiled with JetBrains decompiler
// Type: System.Xml.Schema.XmlSchema
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
  [XmlRoot("schema", Namespace = "http://www.w3.org/2001/XMLSchema")]
  public class XmlSchema : XmlSchemaObject
  {
    public const string Namespace = "http://www.w3.org/2001/XMLSchema";
    public const string InstanceNamespace = "http://www.w3.org/2001/XMLSchema-instance";
    internal const string XdtNamespace = "http://www.w3.org/2003/11/xpath-datatypes";
    private const string xmlname = "schema";
    private XmlSchemaForm attributeFormDefault;
    private XmlSchemaObjectTable attributeGroups;
    private XmlSchemaObjectTable attributes;
    private XmlSchemaDerivationMethod blockDefault;
    private XmlSchemaForm elementFormDefault;
    private XmlSchemaObjectTable elements;
    private XmlSchemaDerivationMethod finalDefault;
    private XmlSchemaObjectTable groups;
    private string id;
    private XmlSchemaObjectCollection includes;
    private XmlSchemaObjectCollection items;
    private XmlSchemaObjectTable notations;
    private XmlSchemaObjectTable schemaTypes;
    private string targetNamespace;
    private XmlAttribute[] unhandledAttributes;
    private string version;
    private XmlSchemaSet schemas;
    private XmlNameTable nameTable;
    internal bool missedSubComponents;
    private XmlSchemaObjectCollection compilationItems;

    public XmlSchema()
    {
      this.attributeFormDefault = XmlSchemaForm.None;
      this.blockDefault = XmlSchemaDerivationMethod.None;
      this.elementFormDefault = XmlSchemaForm.None;
      this.finalDefault = XmlSchemaDerivationMethod.None;
      this.includes = new XmlSchemaObjectCollection();
      this.isCompiled = false;
      this.items = new XmlSchemaObjectCollection();
      this.attributeGroups = new XmlSchemaObjectTable();
      this.attributes = new XmlSchemaObjectTable();
      this.elements = new XmlSchemaObjectTable();
      this.groups = new XmlSchemaObjectTable();
      this.notations = new XmlSchemaObjectTable();
      this.schemaTypes = new XmlSchemaObjectTable();
    }

    [XmlAttribute("attributeFormDefault")]
    [DefaultValue(XmlSchemaForm.None)]
    public XmlSchemaForm AttributeFormDefault
    {
      get => this.attributeFormDefault;
      set => this.attributeFormDefault = value;
    }

    [XmlAttribute("blockDefault")]
    [DefaultValue(XmlSchemaDerivationMethod.None)]
    public XmlSchemaDerivationMethod BlockDefault
    {
      get => this.blockDefault;
      set => this.blockDefault = value;
    }

    [DefaultValue(XmlSchemaDerivationMethod.None)]
    [XmlAttribute("finalDefault")]
    public XmlSchemaDerivationMethod FinalDefault
    {
      get => this.finalDefault;
      set => this.finalDefault = value;
    }

    [DefaultValue(XmlSchemaForm.None)]
    [XmlAttribute("elementFormDefault")]
    public XmlSchemaForm ElementFormDefault
    {
      get => this.elementFormDefault;
      set => this.elementFormDefault = value;
    }

    [XmlAttribute("targetNamespace", DataType = "anyURI")]
    public string TargetNamespace
    {
      get => this.targetNamespace;
      set => this.targetNamespace = value;
    }

    [XmlAttribute("version", DataType = "token")]
    public string Version
    {
      get => this.version;
      set => this.version = value;
    }

    [XmlElement("redefine", typeof (XmlSchemaRedefine))]
    [XmlElement("import", typeof (XmlSchemaImport))]
    [XmlElement("include", typeof (XmlSchemaInclude))]
    public XmlSchemaObjectCollection Includes => this.includes;

    [XmlElement("group", typeof (XmlSchemaGroup))]
    [XmlElement("notation", typeof (XmlSchemaNotation))]
    [XmlElement("annotation", typeof (XmlSchemaAnnotation))]
    [XmlElement("attribute", typeof (XmlSchemaAttribute))]
    [XmlElement("element", typeof (XmlSchemaElement))]
    [XmlElement("simpleType", typeof (XmlSchemaSimpleType))]
    [XmlElement("complexType", typeof (XmlSchemaComplexType))]
    [XmlElement("attributeGroup", typeof (XmlSchemaAttributeGroup))]
    public XmlSchemaObjectCollection Items => this.items;

    [XmlIgnore]
    public bool IsCompiled => this.CompilationId != Guid.Empty;

    [XmlIgnore]
    public XmlSchemaObjectTable Attributes => this.attributes;

    [XmlIgnore]
    public XmlSchemaObjectTable AttributeGroups => this.attributeGroups;

    [XmlIgnore]
    public XmlSchemaObjectTable SchemaTypes => this.schemaTypes;

    [XmlIgnore]
    public XmlSchemaObjectTable Elements => this.elements;

    [XmlAttribute("id", DataType = "ID")]
    public string Id
    {
      get => this.id;
      set => this.id = value;
    }

    [XmlAnyAttribute]
    public XmlAttribute[] UnhandledAttributes
    {
      get
      {
        if (this.unhandledAttributeList != null)
        {
          this.unhandledAttributes = (XmlAttribute[]) this.unhandledAttributeList.ToArray(typeof (XmlAttribute));
          this.unhandledAttributeList = (ArrayList) null;
        }
        return this.unhandledAttributes;
      }
      set
      {
        this.unhandledAttributes = value;
        this.unhandledAttributeList = (ArrayList) null;
      }
    }

    [XmlIgnore]
    public XmlSchemaObjectTable Groups => this.groups;

    [XmlIgnore]
    public XmlSchemaObjectTable Notations => this.notations;

    internal XmlSchemaObjectTable NamedIdentities => this.schemas.NamedIdentities;

    internal XmlSchemaSet Schemas => this.schemas;

    internal Hashtable IDCollection => this.schemas.IDCollection;

    [Obsolete("Use XmlSchemaSet.Compile() instead.")]
    public void Compile(ValidationEventHandler handler) => this.Compile(handler, (XmlResolver) new XmlUrlResolver());

    [Obsolete("Use XmlSchemaSet.Compile() instead.")]
    public void Compile(ValidationEventHandler handler, XmlResolver resolver)
    {
      XmlSchemaSet xmlSchemaSet = new XmlSchemaSet();
      if (handler != null)
        xmlSchemaSet.ValidationEventHandler += handler;
      xmlSchemaSet.XmlResolver = resolver;
      xmlSchemaSet.Add(this);
      xmlSchemaSet.Compile();
    }

    internal void CompileSubset(
      ValidationEventHandler handler,
      XmlSchemaSet col,
      XmlResolver resolver)
    {
      Hashtable handledUris = new Hashtable();
      this.CompileSubset(handler, col, resolver, handledUris);
    }

    internal void CompileSubset(
      ValidationEventHandler handler,
      XmlSchemaSet col,
      XmlResolver resolver,
      Hashtable handledUris)
    {
      if (this.SourceUri != null && this.SourceUri.Length > 0)
      {
        if (handledUris.Contains((object) this.SourceUri))
          return;
        handledUris.Add((object) this.SourceUri, (object) this.SourceUri);
      }
      this.DoCompile(handler, handledUris, col, resolver);
    }

    private void SetParent()
    {
      for (int index = 0; index < this.Items.Count; ++index)
        this.Items[index].SetParent((XmlSchemaObject) this);
      for (int index = 0; index < this.Includes.Count; ++index)
        this.Includes[index].SetParent((XmlSchemaObject) this);
    }

    private void DoCompile(
      ValidationEventHandler handler,
      Hashtable handledUris,
      XmlSchemaSet col,
      XmlResolver resolver)
    {
      this.SetParent();
      this.CompilationId = col.CompilationId;
      this.schemas = col;
      if (!this.schemas.Contains(this))
        this.schemas.Add(this);
      this.attributeGroups.Clear();
      this.attributes.Clear();
      this.elements.Clear();
      this.groups.Clear();
      this.notations.Clear();
      this.schemaTypes.Clear();
      if (this.BlockDefault != XmlSchemaDerivationMethod.All)
      {
        if ((this.BlockDefault & XmlSchemaDerivationMethod.List) != XmlSchemaDerivationMethod.Empty)
          this.error(handler, "list is not allowed in blockDefault attribute");
        if ((this.BlockDefault & XmlSchemaDerivationMethod.Union) != XmlSchemaDerivationMethod.Empty)
          this.error(handler, "union is not allowed in blockDefault attribute");
      }
      if (this.FinalDefault != XmlSchemaDerivationMethod.All && (this.FinalDefault & XmlSchemaDerivationMethod.Substitution) != XmlSchemaDerivationMethod.Empty)
        this.error(handler, "substitution is not allowed in finalDefault attribute");
      XmlSchemaUtil.CompileID(this.Id, (XmlSchemaObject) this, col.IDCollection, handler);
      if (this.TargetNamespace != null)
      {
        if (this.TargetNamespace.Length == 0)
          this.error(handler, "The targetNamespace attribute cannot have have empty string as its value.");
        if (!XmlSchemaUtil.CheckAnyUri(this.TargetNamespace))
          this.error(handler, this.TargetNamespace + " is not a valid value for targetNamespace attribute of schema");
      }
      if (!XmlSchemaUtil.CheckNormalizedString(this.Version))
        this.error(handler, this.Version + "is not a valid value for version attribute of schema");
      this.compilationItems = new XmlSchemaObjectCollection();
      for (int index = 0; index < this.Items.Count; ++index)
        this.compilationItems.Add(this.Items[index]);
      for (int index = 0; index < this.Includes.Count; ++index)
        this.ProcessExternal(handler, handledUris, resolver, this.Includes[index] as XmlSchemaExternal, col);
      for (int index = 0; index < this.compilationItems.Count; ++index)
      {
        XmlSchemaObject compilationItem = this.compilationItems[index];
        switch (compilationItem)
        {
          case XmlSchemaAnnotation _:
            this.errorCount += ((XmlSchemaAnnotation) compilationItem).Compile(handler, this);
            break;
          case XmlSchemaAttribute _:
            XmlSchemaAttribute xmlSchemaAttribute = (XmlSchemaAttribute) compilationItem;
            int num1 = xmlSchemaAttribute.Compile(handler, this);
            this.errorCount += num1;
            if (num1 == 0)
            {
              XmlSchemaUtil.AddToTable(this.Attributes, (XmlSchemaObject) xmlSchemaAttribute, xmlSchemaAttribute.QualifiedName, handler);
              break;
            }
            break;
          case XmlSchemaAttributeGroup _:
            XmlSchemaAttributeGroup schemaAttributeGroup = (XmlSchemaAttributeGroup) compilationItem;
            int num2 = schemaAttributeGroup.Compile(handler, this);
            this.errorCount += num2;
            if (num2 == 0)
            {
              XmlSchemaUtil.AddToTable(this.AttributeGroups, (XmlSchemaObject) schemaAttributeGroup, schemaAttributeGroup.QualifiedName, handler);
              break;
            }
            break;
          case XmlSchemaComplexType _:
            XmlSchemaComplexType schemaComplexType = (XmlSchemaComplexType) compilationItem;
            int num3 = schemaComplexType.Compile(handler, this);
            this.errorCount += num3;
            if (num3 == 0)
            {
              XmlSchemaUtil.AddToTable(this.schemaTypes, (XmlSchemaObject) schemaComplexType, schemaComplexType.QualifiedName, handler);
              break;
            }
            break;
          case XmlSchemaSimpleType _:
            XmlSchemaSimpleType schemaSimpleType = (XmlSchemaSimpleType) compilationItem;
            schemaSimpleType.islocal = false;
            int num4 = schemaSimpleType.Compile(handler, this);
            this.errorCount += num4;
            if (num4 == 0)
            {
              XmlSchemaUtil.AddToTable(this.SchemaTypes, (XmlSchemaObject) schemaSimpleType, schemaSimpleType.QualifiedName, handler);
              break;
            }
            break;
          case XmlSchemaElement _:
            XmlSchemaElement xmlSchemaElement = (XmlSchemaElement) compilationItem;
            xmlSchemaElement.parentIsSchema = true;
            int num5 = xmlSchemaElement.Compile(handler, this);
            this.errorCount += num5;
            if (num5 == 0)
            {
              XmlSchemaUtil.AddToTable(this.Elements, (XmlSchemaObject) xmlSchemaElement, xmlSchemaElement.QualifiedName, handler);
              break;
            }
            break;
          case XmlSchemaGroup _:
            XmlSchemaGroup xmlSchemaGroup = (XmlSchemaGroup) compilationItem;
            int num6 = xmlSchemaGroup.Compile(handler, this);
            this.errorCount += num6;
            if (num6 == 0)
            {
              XmlSchemaUtil.AddToTable(this.Groups, (XmlSchemaObject) xmlSchemaGroup, xmlSchemaGroup.QualifiedName, handler);
              break;
            }
            break;
          case XmlSchemaNotation _:
            XmlSchemaNotation xmlSchemaNotation = (XmlSchemaNotation) compilationItem;
            int num7 = xmlSchemaNotation.Compile(handler, this);
            this.errorCount += num7;
            if (num7 == 0)
            {
              XmlSchemaUtil.AddToTable(this.Notations, (XmlSchemaObject) xmlSchemaNotation, xmlSchemaNotation.QualifiedName, handler);
              break;
            }
            break;
          default:
            ValidationHandler.RaiseValidationEvent(handler, (Exception) null, string.Format("Object of Type {0} is not valid in Item Property of Schema", (object) compilationItem.GetType().Name), (XmlSchemaObject) null, (object) this, (string) null, XmlSeverityType.Error);
            break;
        }
      }
    }

    private string GetResolvedUri(XmlResolver resolver, string relativeUri)
    {
      Uri baseUri = (Uri) null;
      if (this.SourceUri != null && this.SourceUri != string.Empty)
        baseUri = new Uri(this.SourceUri);
      Uri uri = resolver.ResolveUri(baseUri, relativeUri);
      return uri != (Uri) null ? uri.OriginalString : string.Empty;
    }

    private void ProcessExternal(
      ValidationEventHandler handler,
      Hashtable handledUris,
      XmlResolver resolver,
      XmlSchemaExternal ext,
      XmlSchemaSet col)
    {
      if (ext == null)
      {
        this.error(handler, string.Format("Object of Type {0} is not valid in Includes Property of XmlSchema", (object) ext.GetType().Name));
      }
      else
      {
        XmlSchemaImport xmlSchemaImport = ext as XmlSchemaImport;
        if (ext.SchemaLocation == null && xmlSchemaImport == null)
          return;
        XmlSchema s = (XmlSchema) null;
        if (ext.SchemaLocation != null)
        {
          Stream input = (Stream) null;
          string str = (string) null;
          if (resolver != null)
          {
            str = this.GetResolvedUri(resolver, ext.SchemaLocation);
            if (handledUris.Contains((object) str))
              return;
            handledUris.Add((object) str, (object) str);
            try
            {
              input = resolver.GetEntity(new Uri(str), (string) null, typeof (Stream)) as Stream;
            }
            catch (Exception ex)
            {
              this.warn(handler, "Could not resolve schema location URI: " + str);
              input = (Stream) null;
            }
          }
          if (ext is XmlSchemaRedefine xmlSchemaRedefine)
          {
            for (int index = 0; index < xmlSchemaRedefine.Items.Count; ++index)
            {
              XmlSchemaObject xmlSchemaObject = xmlSchemaRedefine.Items[index];
              xmlSchemaObject.isRedefinedComponent = true;
              xmlSchemaObject.isRedefineChild = true;
              switch (xmlSchemaObject)
              {
                case XmlSchemaType _:
                case XmlSchemaGroup _:
                case XmlSchemaAttributeGroup _:
                  this.compilationItems.Add(xmlSchemaObject);
                  break;
                default:
                  this.error(handler, "Redefinition is only allowed to simpleType, complexType, group and attributeGroup.");
                  break;
              }
            }
          }
          if (input == null)
          {
            this.missedSubComponents = true;
            return;
          }
          XmlTextReader rdr = (XmlTextReader) null;
          try
          {
            rdr = new XmlTextReader(str, input, this.nameTable);
            s = XmlSchema.Read((XmlReader) rdr, handler);
          }
          finally
          {
            rdr?.Close();
          }
          s.schemas = this.schemas;
          s.SetParent();
          ext.Schema = s;
        }
        if (xmlSchemaImport != null)
        {
          if (ext.Schema == null && ext.SchemaLocation == null)
          {
            foreach (XmlSchema schema in (IEnumerable) col.Schemas())
            {
              if (schema.TargetNamespace == xmlSchemaImport.Namespace)
              {
                s = schema;
                s.schemas = this.schemas;
                s.SetParent();
                ext.Schema = s;
                break;
              }
            }
            if (s == null)
              return;
          }
          else if (s != null)
          {
            if (this.TargetNamespace == s.TargetNamespace)
            {
              this.error(handler, "Target namespace must be different from that of included schema.");
              return;
            }
            if (s.TargetNamespace != xmlSchemaImport.Namespace)
            {
              this.error(handler, "Attribute namespace and its importing schema's target namespace must be the same.");
              return;
            }
          }
        }
        else if (s != null)
        {
          if (this.TargetNamespace == null && s.TargetNamespace != null)
          {
            this.error(handler, "Target namespace is required to include a schema which has its own target namespace");
            return;
          }
          if (this.TargetNamespace != null && s.TargetNamespace == null)
            s.TargetNamespace = this.TargetNamespace;
        }
        if (s == null)
          return;
        this.AddExternalComponentsTo(s, this.compilationItems, handler, handledUris, resolver, col);
      }
    }

    private void AddExternalComponentsTo(
      XmlSchema s,
      XmlSchemaObjectCollection items,
      ValidationEventHandler handler,
      Hashtable handledUris,
      XmlResolver resolver,
      XmlSchemaSet col)
    {
      foreach (XmlSchemaExternal include in s.Includes)
        this.ProcessExternal(handler, handledUris, resolver, include, col);
      foreach (XmlSchemaObject xmlSchemaObject in s.Items)
        items.Add(xmlSchemaObject);
    }

    internal bool IsNamespaceAbsent(string ns) => !this.schemas.Contains(ns);

    internal XmlSchemaAttribute FindAttribute(XmlQualifiedName name)
    {
      foreach (XmlSchema schema in (IEnumerable) this.schemas.Schemas())
      {
        if (schema.Attributes[name] is XmlSchemaAttribute attribute)
          return attribute;
      }
      return (XmlSchemaAttribute) null;
    }

    internal XmlSchemaAttributeGroup FindAttributeGroup(XmlQualifiedName name)
    {
      foreach (XmlSchema schema in (IEnumerable) this.schemas.Schemas())
      {
        if (schema.AttributeGroups[name] is XmlSchemaAttributeGroup attributeGroup)
          return attributeGroup;
      }
      return (XmlSchemaAttributeGroup) null;
    }

    internal XmlSchemaElement FindElement(XmlQualifiedName name)
    {
      foreach (XmlSchema schema in (IEnumerable) this.schemas.Schemas())
      {
        if (schema.Elements[name] is XmlSchemaElement element)
          return element;
      }
      return (XmlSchemaElement) null;
    }

    internal XmlSchemaType FindSchemaType(XmlQualifiedName name)
    {
      foreach (XmlSchema schema in (IEnumerable) this.schemas.Schemas())
      {
        if (schema.SchemaTypes[name] is XmlSchemaType schemaType)
          return schemaType;
      }
      return (XmlSchemaType) null;
    }

    internal void Validate(ValidationEventHandler handler)
    {
      this.ValidationId = this.CompilationId;
      foreach (XmlSchemaAttribute xmlSchemaAttribute in (IEnumerable) this.Attributes.Values)
        this.errorCount += xmlSchemaAttribute.Validate(handler, this);
      foreach (XmlSchemaAttributeGroup schemaAttributeGroup in (IEnumerable) this.AttributeGroups.Values)
        this.errorCount += schemaAttributeGroup.Validate(handler, this);
      foreach (XmlSchemaType xmlSchemaType in (IEnumerable) this.SchemaTypes.Values)
        this.errorCount += xmlSchemaType.Validate(handler, this);
      foreach (XmlSchemaElement xmlSchemaElement in (IEnumerable) this.Elements.Values)
        this.errorCount += xmlSchemaElement.Validate(handler, this);
      foreach (XmlSchemaGroup xmlSchemaGroup in (IEnumerable) this.Groups.Values)
        this.errorCount += xmlSchemaGroup.Validate(handler, this);
      foreach (XmlSchemaNotation xmlSchemaNotation in (IEnumerable) this.Notations.Values)
        this.errorCount += xmlSchemaNotation.Validate(handler, this);
      if (this.errorCount == 0)
        this.isCompiled = true;
      this.errorCount = 0;
    }

    public static XmlSchema Read(TextReader reader, ValidationEventHandler validationEventHandler) => XmlSchema.Read((XmlReader) new XmlTextReader(reader), validationEventHandler);

    public static XmlSchema Read(Stream stream, ValidationEventHandler validationEventHandler) => XmlSchema.Read((XmlReader) new XmlTextReader(stream), validationEventHandler);

    public static XmlSchema Read(XmlReader rdr, ValidationEventHandler validationEventHandler)
    {
      XmlSchemaReader reader = new XmlSchemaReader(rdr, validationEventHandler);
      if (reader.ReadState == ReadState.Initial)
        reader.ReadNextElement();
      int depth = reader.Depth;
      do
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          if (reader.LocalName == "schema")
          {
            XmlSchema schema = new XmlSchema();
            schema.nameTable = rdr.NameTable;
            schema.LineNumber = reader.LineNumber;
            schema.LinePosition = reader.LinePosition;
            schema.SourceUri = reader.BaseURI;
            XmlSchema.ReadAttributes(schema, reader, validationEventHandler);
            reader.MoveToElement();
            if (!reader.IsEmptyElement)
              XmlSchema.ReadContent(schema, reader, validationEventHandler);
            else
              rdr.Skip();
            return schema;
          }
          XmlSchemaObject.error(validationEventHandler, "The root element must be schema", (Exception) null);
        }
        else
          XmlSchemaObject.error(validationEventHandler, "This should never happen. XmlSchema.Read 1 ", (Exception) null);
      }
      while (reader.Depth > depth && reader.ReadNextElement());
      throw new XmlSchemaException("The top level schema must have namespace http://www.w3.org/2001/XMLSchema", (Exception) null);
    }

    private static void ReadAttributes(
      XmlSchema schema,
      XmlSchemaReader reader,
      ValidationEventHandler h)
    {
      reader.MoveToElement();
      while (reader.MoveToNextAttribute())
      {
        string name = reader.Name;
        if (name != null)
        {
          // ISSUE: reference to a compiler-generated field
          if (XmlSchema.\u003C\u003Ef__switch\u0024map2D == null)
          {
            // ISSUE: reference to a compiler-generated field
            XmlSchema.\u003C\u003Ef__switch\u0024map2D = new Dictionary<string, int>(7)
            {
              {
                "attributeFormDefault",
                0
              },
              {
                "blockDefault",
                1
              },
              {
                "elementFormDefault",
                2
              },
              {
                "finalDefault",
                3
              },
              {
                "id",
                4
              },
              {
                "targetNamespace",
                5
              },
              {
                "version",
                6
              }
            };
          }
          int num;
          Exception innerExcpetion;
          // ISSUE: reference to a compiler-generated field
          if (XmlSchema.\u003C\u003Ef__switch\u0024map2D.TryGetValue(name, out num))
          {
            switch (num)
            {
              case 0:
                schema.attributeFormDefault = XmlSchemaUtil.ReadFormAttribute((XmlReader) reader, out innerExcpetion);
                if (innerExcpetion != null)
                {
                  XmlSchemaObject.error(h, reader.Value + " is not a valid value for attributeFormDefault.", innerExcpetion);
                  continue;
                }
                continue;
              case 1:
                schema.blockDefault = XmlSchemaUtil.ReadDerivationAttribute((XmlReader) reader, out innerExcpetion, "blockDefault", XmlSchemaUtil.ElementBlockAllowed);
                if (innerExcpetion != null)
                {
                  XmlSchemaObject.error(h, innerExcpetion.Message, innerExcpetion);
                  continue;
                }
                continue;
              case 2:
                schema.elementFormDefault = XmlSchemaUtil.ReadFormAttribute((XmlReader) reader, out innerExcpetion);
                if (innerExcpetion != null)
                {
                  XmlSchemaObject.error(h, reader.Value + " is not a valid value for elementFormDefault.", innerExcpetion);
                  continue;
                }
                continue;
              case 3:
                schema.finalDefault = XmlSchemaUtil.ReadDerivationAttribute((XmlReader) reader, out innerExcpetion, "finalDefault", XmlSchemaUtil.FinalAllowed);
                if (innerExcpetion != null)
                {
                  XmlSchemaObject.error(h, innerExcpetion.Message, innerExcpetion);
                  continue;
                }
                continue;
              case 4:
                schema.id = reader.Value;
                continue;
              case 5:
                schema.targetNamespace = reader.Value;
                continue;
              case 6:
                schema.version = reader.Value;
                continue;
            }
          }
        }
        if (reader.NamespaceURI == string.Empty && reader.Name != "xmlns" || reader.NamespaceURI == "http://www.w3.org/2001/XMLSchema")
          XmlSchemaObject.error(h, reader.Name + " attribute is not allowed in schema element", (Exception) null);
        else
          XmlSchemaUtil.ReadUnhandledAttribute((XmlReader) reader, (XmlSchemaObject) schema);
      }
    }

    private static void ReadContent(
      XmlSchema schema,
      XmlSchemaReader reader,
      ValidationEventHandler h)
    {
      reader.MoveToElement();
      if (reader.LocalName != nameof (schema) && reader.NamespaceURI != "http://www.w3.org/2001/XMLSchema" && reader.NodeType != XmlNodeType.Element)
        XmlSchemaObject.error(h, "UNREACHABLE CODE REACHED: Method: Schema.ReadContent, " + reader.LocalName + ", " + reader.NamespaceURI, (Exception) null);
      int num = 1;
      while (reader.ReadNextElement())
      {
        if (reader.NodeType == XmlNodeType.EndElement)
        {
          if (!(reader.LocalName != nameof (schema)))
            break;
          XmlSchemaObject.error(h, "Should not happen :2: XmlSchema.Read, name=" + reader.Name, (Exception) null);
          break;
        }
        if (num <= 1)
        {
          if (reader.LocalName == "include")
          {
            XmlSchemaInclude xmlSchemaInclude = XmlSchemaInclude.Read(reader, h);
            if (xmlSchemaInclude != null)
            {
              schema.includes.Add((XmlSchemaObject) xmlSchemaInclude);
              continue;
            }
            continue;
          }
          if (reader.LocalName == "import")
          {
            XmlSchemaImport xmlSchemaImport = XmlSchemaImport.Read(reader, h);
            if (xmlSchemaImport != null)
            {
              schema.includes.Add((XmlSchemaObject) xmlSchemaImport);
              continue;
            }
            continue;
          }
          if (reader.LocalName == "redefine")
          {
            XmlSchemaRedefine xmlSchemaRedefine = XmlSchemaRedefine.Read(reader, h);
            if (xmlSchemaRedefine != null)
            {
              schema.includes.Add((XmlSchemaObject) xmlSchemaRedefine);
              continue;
            }
            continue;
          }
          if (reader.LocalName == "annotation")
          {
            XmlSchemaAnnotation schemaAnnotation = XmlSchemaAnnotation.Read(reader, h);
            if (schemaAnnotation != null)
            {
              schema.items.Add((XmlSchemaObject) schemaAnnotation);
              continue;
            }
            continue;
          }
        }
        if (num <= 2)
        {
          num = 2;
          if (reader.LocalName == "simpleType")
          {
            XmlSchemaSimpleType schemaSimpleType = XmlSchemaSimpleType.Read(reader, h);
            if (schemaSimpleType != null)
            {
              schema.items.Add((XmlSchemaObject) schemaSimpleType);
              continue;
            }
            continue;
          }
          if (reader.LocalName == "complexType")
          {
            XmlSchemaComplexType schemaComplexType = XmlSchemaComplexType.Read(reader, h);
            if (schemaComplexType != null)
            {
              schema.items.Add((XmlSchemaObject) schemaComplexType);
              continue;
            }
            continue;
          }
          if (reader.LocalName == "group")
          {
            XmlSchemaGroup xmlSchemaGroup = XmlSchemaGroup.Read(reader, h);
            if (xmlSchemaGroup != null)
            {
              schema.items.Add((XmlSchemaObject) xmlSchemaGroup);
              continue;
            }
            continue;
          }
          if (reader.LocalName == "attributeGroup")
          {
            XmlSchemaAttributeGroup schemaAttributeGroup = XmlSchemaAttributeGroup.Read(reader, h);
            if (schemaAttributeGroup != null)
            {
              schema.items.Add((XmlSchemaObject) schemaAttributeGroup);
              continue;
            }
            continue;
          }
          if (reader.LocalName == "element")
          {
            XmlSchemaElement xmlSchemaElement = XmlSchemaElement.Read(reader, h);
            if (xmlSchemaElement != null)
            {
              schema.items.Add((XmlSchemaObject) xmlSchemaElement);
              continue;
            }
            continue;
          }
          if (reader.LocalName == "attribute")
          {
            XmlSchemaAttribute xmlSchemaAttribute = XmlSchemaAttribute.Read(reader, h);
            if (xmlSchemaAttribute != null)
            {
              schema.items.Add((XmlSchemaObject) xmlSchemaAttribute);
              continue;
            }
            continue;
          }
          if (reader.LocalName == "notation")
          {
            XmlSchemaNotation xmlSchemaNotation = XmlSchemaNotation.Read(reader, h);
            if (xmlSchemaNotation != null)
            {
              schema.items.Add((XmlSchemaObject) xmlSchemaNotation);
              continue;
            }
            continue;
          }
          if (reader.LocalName == "annotation")
          {
            XmlSchemaAnnotation schemaAnnotation = XmlSchemaAnnotation.Read(reader, h);
            if (schemaAnnotation != null)
            {
              schema.items.Add((XmlSchemaObject) schemaAnnotation);
              continue;
            }
            continue;
          }
        }
        reader.RaiseInvalidElementError();
      }
    }

    public void Write(Stream stream) => this.Write(stream, (XmlNamespaceManager) null);

    public void Write(TextWriter writer) => this.Write(writer, (XmlNamespaceManager) null);

    public void Write(XmlWriter writer) => this.Write(writer, (XmlNamespaceManager) null);

    public void Write(Stream stream, XmlNamespaceManager namespaceManager) => this.Write((XmlWriter) new XmlTextWriter(stream, (Encoding) null), namespaceManager);

    public void Write(TextWriter writer, XmlNamespaceManager namespaceManager) => this.Write((XmlWriter) new XmlTextWriter(writer)
    {
      Formatting = Formatting.Indented
    }, namespaceManager);

    public void Write(XmlWriter writer, XmlNamespaceManager namespaceManager)
    {
      XmlSerializerNamespaces namespaces1 = new XmlSerializerNamespaces();
      if (namespaceManager != null)
      {
        foreach (string prefix in namespaceManager)
        {
          if (prefix != "xml" && prefix != "xmlns")
            namespaces1.Add(prefix, namespaceManager.LookupNamespace(prefix));
        }
      }
      if (this.Namespaces != null && this.Namespaces.Count > 0)
      {
        XmlQualifiedName[] array = this.Namespaces.ToArray();
        foreach (XmlQualifiedName xmlQualifiedName in array)
          namespaces1.Add(xmlQualifiedName.Name, xmlQualifiedName.Namespace);
        string prefix = string.Empty;
        bool flag = true;
        int num = 1;
        while (flag)
        {
          flag = false;
          foreach (XmlQualifiedName xmlQualifiedName in array)
          {
            if (xmlQualifiedName.Name == prefix)
            {
              prefix = "q" + (object) num;
              flag = true;
              break;
            }
          }
          ++num;
        }
        namespaces1.Add(prefix, "http://www.w3.org/2001/XMLSchema");
      }
      if (namespaces1.Count == 0)
      {
        namespaces1.Add("xs", "http://www.w3.org/2001/XMLSchema");
        if (this.TargetNamespace != null && this.TargetNamespace.Length != 0)
          namespaces1.Add("tns", this.TargetNamespace);
      }
      XmlSchemaSerializer schemaSerializer = new XmlSchemaSerializer();
      XmlSerializerNamespaces namespaces2 = this.Namespaces;
      try
      {
        this.Namespaces = (XmlSerializerNamespaces) null;
        schemaSerializer.Serialize(writer, (object) this, namespaces1);
      }
      finally
      {
        this.Namespaces = namespaces2;
      }
      writer.Flush();
    }
  }
}
