// Decompiled with JetBrains decompiler
// Type: System.Xml.Schema.XmlSchemaSimpleType
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using Mono.Xml.Schema;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
  public class XmlSchemaSimpleType : XmlSchemaType
  {
    private const string xmlname = "simpleType";
    private static XmlSchemaSimpleType schemaLocationType;
    private XmlSchemaSimpleTypeContent content;
    internal bool islocal = true;
    private bool recursed;
    private XmlSchemaDerivationMethod variety;
    internal static readonly XmlSchemaSimpleType XsAnySimpleType;
    internal static readonly XmlSchemaSimpleType XsString;
    internal static readonly XmlSchemaSimpleType XsBoolean;
    internal static readonly XmlSchemaSimpleType XsDecimal;
    internal static readonly XmlSchemaSimpleType XsFloat;
    internal static readonly XmlSchemaSimpleType XsDouble;
    internal static readonly XmlSchemaSimpleType XsDuration;
    internal static readonly XmlSchemaSimpleType XsDateTime;
    internal static readonly XmlSchemaSimpleType XsTime;
    internal static readonly XmlSchemaSimpleType XsDate;
    internal static readonly XmlSchemaSimpleType XsGYearMonth;
    internal static readonly XmlSchemaSimpleType XsGYear;
    internal static readonly XmlSchemaSimpleType XsGMonthDay;
    internal static readonly XmlSchemaSimpleType XsGDay;
    internal static readonly XmlSchemaSimpleType XsGMonth;
    internal static readonly XmlSchemaSimpleType XsHexBinary;
    internal static readonly XmlSchemaSimpleType XsBase64Binary;
    internal static readonly XmlSchemaSimpleType XsAnyUri;
    internal static readonly XmlSchemaSimpleType XsQName;
    internal static readonly XmlSchemaSimpleType XsNotation;
    internal static readonly XmlSchemaSimpleType XsNormalizedString;
    internal static readonly XmlSchemaSimpleType XsToken;
    internal static readonly XmlSchemaSimpleType XsLanguage;
    internal static readonly XmlSchemaSimpleType XsNMToken;
    internal static readonly XmlSchemaSimpleType XsNMTokens;
    internal static readonly XmlSchemaSimpleType XsName;
    internal static readonly XmlSchemaSimpleType XsNCName;
    internal static readonly XmlSchemaSimpleType XsID;
    internal static readonly XmlSchemaSimpleType XsIDRef;
    internal static readonly XmlSchemaSimpleType XsIDRefs;
    internal static readonly XmlSchemaSimpleType XsEntity;
    internal static readonly XmlSchemaSimpleType XsEntities;
    internal static readonly XmlSchemaSimpleType XsInteger;
    internal static readonly XmlSchemaSimpleType XsNonPositiveInteger;
    internal static readonly XmlSchemaSimpleType XsNegativeInteger;
    internal static readonly XmlSchemaSimpleType XsLong;
    internal static readonly XmlSchemaSimpleType XsInt;
    internal static readonly XmlSchemaSimpleType XsShort;
    internal static readonly XmlSchemaSimpleType XsByte;
    internal static readonly XmlSchemaSimpleType XsNonNegativeInteger;
    internal static readonly XmlSchemaSimpleType XsUnsignedLong;
    internal static readonly XmlSchemaSimpleType XsUnsignedInt;
    internal static readonly XmlSchemaSimpleType XsUnsignedShort;
    internal static readonly XmlSchemaSimpleType XsUnsignedByte;
    internal static readonly XmlSchemaSimpleType XsPositiveInteger;
    internal static readonly XmlSchemaSimpleType XdtUntypedAtomic;
    internal static readonly XmlSchemaSimpleType XdtAnyAtomicType;
    internal static readonly XmlSchemaSimpleType XdtYearMonthDuration;
    internal static readonly XmlSchemaSimpleType XdtDayTimeDuration;

    static XmlSchemaSimpleType()
    {
      XmlSchemaSimpleType schemaSimpleType = new XmlSchemaSimpleType();
      schemaSimpleType.Content = (XmlSchemaSimpleTypeContent) new XmlSchemaSimpleTypeList()
      {
        ItemTypeName = new XmlQualifiedName("anyURI", "http://www.w3.org/2001/XMLSchema")
      };
      schemaSimpleType.BaseXmlSchemaTypeInternal = (XmlSchemaType) null;
      schemaSimpleType.variety = XmlSchemaDerivationMethod.List;
      XmlSchemaSimpleType.schemaLocationType = schemaSimpleType;
      XmlSchemaSimpleType.XsAnySimpleType = XmlSchemaSimpleType.BuildSchemaType("anySimpleType", (string) null);
      XmlSchemaSimpleType.XsString = XmlSchemaSimpleType.BuildSchemaType("string", "anySimpleType");
      XmlSchemaSimpleType.XsBoolean = XmlSchemaSimpleType.BuildSchemaType("boolean", "anySimpleType");
      XmlSchemaSimpleType.XsDecimal = XmlSchemaSimpleType.BuildSchemaType("decimal", "anySimpleType");
      XmlSchemaSimpleType.XsFloat = XmlSchemaSimpleType.BuildSchemaType("float", "anySimpleType");
      XmlSchemaSimpleType.XsDouble = XmlSchemaSimpleType.BuildSchemaType("double", "anySimpleType");
      XmlSchemaSimpleType.XsDuration = XmlSchemaSimpleType.BuildSchemaType("duration", "anySimpleType");
      XmlSchemaSimpleType.XsDateTime = XmlSchemaSimpleType.BuildSchemaType("dateTime", "anySimpleType");
      XmlSchemaSimpleType.XsTime = XmlSchemaSimpleType.BuildSchemaType("time", "anySimpleType");
      XmlSchemaSimpleType.XsDate = XmlSchemaSimpleType.BuildSchemaType("date", "anySimpleType");
      XmlSchemaSimpleType.XsGYearMonth = XmlSchemaSimpleType.BuildSchemaType("gYearMonth", "anySimpleType");
      XmlSchemaSimpleType.XsGYear = XmlSchemaSimpleType.BuildSchemaType("gYear", "anySimpleType");
      XmlSchemaSimpleType.XsGMonthDay = XmlSchemaSimpleType.BuildSchemaType("gMonthDay", "anySimpleType");
      XmlSchemaSimpleType.XsGDay = XmlSchemaSimpleType.BuildSchemaType("gDay", "anySimpleType");
      XmlSchemaSimpleType.XsGMonth = XmlSchemaSimpleType.BuildSchemaType("gMonth", "anySimpleType");
      XmlSchemaSimpleType.XsHexBinary = XmlSchemaSimpleType.BuildSchemaType("hexBinary", "anySimpleType");
      XmlSchemaSimpleType.XsBase64Binary = XmlSchemaSimpleType.BuildSchemaType("base64Binary", "anySimpleType");
      XmlSchemaSimpleType.XsAnyUri = XmlSchemaSimpleType.BuildSchemaType("anyURI", "anySimpleType");
      XmlSchemaSimpleType.XsQName = XmlSchemaSimpleType.BuildSchemaType("QName", "anySimpleType");
      XmlSchemaSimpleType.XsNotation = XmlSchemaSimpleType.BuildSchemaType("NOTATION", "anySimpleType");
      XmlSchemaSimpleType.XsNormalizedString = XmlSchemaSimpleType.BuildSchemaType("normalizedString", "string");
      XmlSchemaSimpleType.XsToken = XmlSchemaSimpleType.BuildSchemaType("token", "normalizedString");
      XmlSchemaSimpleType.XsLanguage = XmlSchemaSimpleType.BuildSchemaType("language", "token");
      XmlSchemaSimpleType.XsNMToken = XmlSchemaSimpleType.BuildSchemaType("NMTOKEN", "token");
      XmlSchemaSimpleType.XsName = XmlSchemaSimpleType.BuildSchemaType("Name", "token");
      XmlSchemaSimpleType.XsNCName = XmlSchemaSimpleType.BuildSchemaType("NCName", "Name");
      XmlSchemaSimpleType.XsID = XmlSchemaSimpleType.BuildSchemaType("ID", "NCName");
      XmlSchemaSimpleType.XsIDRef = XmlSchemaSimpleType.BuildSchemaType("IDREF", "NCName");
      XmlSchemaSimpleType.XsEntity = XmlSchemaSimpleType.BuildSchemaType("ENTITY", "NCName");
      XmlSchemaSimpleType.XsInteger = XmlSchemaSimpleType.BuildSchemaType("integer", "decimal");
      XmlSchemaSimpleType.XsNonPositiveInteger = XmlSchemaSimpleType.BuildSchemaType("nonPositiveInteger", "integer");
      XmlSchemaSimpleType.XsNegativeInteger = XmlSchemaSimpleType.BuildSchemaType("negativeInteger", "nonPositiveInteger");
      XmlSchemaSimpleType.XsLong = XmlSchemaSimpleType.BuildSchemaType("long", "integer");
      XmlSchemaSimpleType.XsInt = XmlSchemaSimpleType.BuildSchemaType("int", "long");
      XmlSchemaSimpleType.XsShort = XmlSchemaSimpleType.BuildSchemaType("short", "int");
      XmlSchemaSimpleType.XsByte = XmlSchemaSimpleType.BuildSchemaType("byte", "short");
      XmlSchemaSimpleType.XsNonNegativeInteger = XmlSchemaSimpleType.BuildSchemaType("nonNegativeInteger", "integer");
      XmlSchemaSimpleType.XsUnsignedLong = XmlSchemaSimpleType.BuildSchemaType("unsignedLong", "nonNegativeInteger");
      XmlSchemaSimpleType.XsUnsignedInt = XmlSchemaSimpleType.BuildSchemaType("unsignedInt", "unsignedLong");
      XmlSchemaSimpleType.XsUnsignedShort = XmlSchemaSimpleType.BuildSchemaType("unsignedShort", "unsignedInt");
      XmlSchemaSimpleType.XsUnsignedByte = XmlSchemaSimpleType.BuildSchemaType("unsignedByte", "unsignedShort");
      XmlSchemaSimpleType.XsPositiveInteger = XmlSchemaSimpleType.BuildSchemaType("positiveInteger", "nonNegativeInteger");
      XmlSchemaSimpleType.XdtAnyAtomicType = XmlSchemaSimpleType.BuildSchemaType("anyAtomicType", "anySimpleType", true, false);
      XmlSchemaSimpleType.XdtUntypedAtomic = XmlSchemaSimpleType.BuildSchemaType("untypedAtomic", "anyAtomicType", true, true);
      XmlSchemaSimpleType.XdtDayTimeDuration = XmlSchemaSimpleType.BuildSchemaType("dayTimeDuration", "duration", true, false);
      XmlSchemaSimpleType.XdtYearMonthDuration = XmlSchemaSimpleType.BuildSchemaType("yearMonthDuration", "duration", true, false);
      XmlSchemaSimpleType.XsIDRefs = new XmlSchemaSimpleType();
      XmlSchemaSimpleType.XsIDRefs.Content = (XmlSchemaSimpleTypeContent) new XmlSchemaSimpleTypeList()
      {
        ItemType = XmlSchemaSimpleType.XsIDRef
      };
      XmlSchemaSimpleType.XsEntities = new XmlSchemaSimpleType();
      XmlSchemaSimpleType.XsEntities.Content = (XmlSchemaSimpleTypeContent) new XmlSchemaSimpleTypeList()
      {
        ItemType = XmlSchemaSimpleType.XsEntity
      };
      XmlSchemaSimpleType.XsNMTokens = new XmlSchemaSimpleType();
      XmlSchemaSimpleType.XsNMTokens.Content = (XmlSchemaSimpleTypeContent) new XmlSchemaSimpleTypeList()
      {
        ItemType = XmlSchemaSimpleType.XsNMToken
      };
    }

    private static XmlSchemaSimpleType BuildSchemaType(string name, string baseName) => XmlSchemaSimpleType.BuildSchemaType(name, baseName, false, false);

    private static XmlSchemaSimpleType BuildSchemaType(
      string name,
      string baseName,
      bool xdt,
      bool baseXdt)
    {
      string ns1 = !xdt ? "http://www.w3.org/2001/XMLSchema" : "http://www.w3.org/2003/11/xpath-datatypes";
      string ns2 = !baseXdt ? "http://www.w3.org/2001/XMLSchema" : "http://www.w3.org/2003/11/xpath-datatypes";
      XmlSchemaSimpleType schemaSimpleType = new XmlSchemaSimpleType();
      schemaSimpleType.QNameInternal = new XmlQualifiedName(name, ns1);
      if (baseName != null)
        schemaSimpleType.BaseXmlSchemaTypeInternal = (XmlSchemaType) XmlSchemaType.GetBuiltInSimpleType(new XmlQualifiedName(baseName, ns2));
      schemaSimpleType.DatatypeInternal = XmlSchemaDatatype.FromName(schemaSimpleType.QualifiedName);
      return schemaSimpleType;
    }

    internal static XsdAnySimpleType AnySimpleType => XsdAnySimpleType.Instance;

    internal static XmlSchemaSimpleType SchemaLocationType => XmlSchemaSimpleType.schemaLocationType;

    [XmlElement("list", typeof (XmlSchemaSimpleTypeList))]
    [XmlElement("restriction", typeof (XmlSchemaSimpleTypeRestriction))]
    [XmlElement("union", typeof (XmlSchemaSimpleTypeUnion))]
    public XmlSchemaSimpleTypeContent Content
    {
      get => this.content;
      set => this.content = value;
    }

    internal XmlSchemaDerivationMethod Variety => this.variety;

    internal override void SetParent(XmlSchemaObject parent)
    {
      base.SetParent(parent);
      if (this.Content == null)
        return;
      this.Content.SetParent((XmlSchemaObject) this);
    }

    internal override int Compile(ValidationEventHandler h, XmlSchema schema)
    {
      if (this.CompilationId == schema.CompilationId)
        return 0;
      this.errorCount = 0;
      if (this.islocal)
      {
        if (this.Name != null)
          this.error(h, "Name is prohibited in a local simpletype");
        else
          this.QNameInternal = new XmlQualifiedName(this.Name, this.AncestorSchema.TargetNamespace);
        if (this.Final != XmlSchemaDerivationMethod.None)
          this.error(h, "Final is prohibited in a local simpletype");
      }
      else
      {
        if (this.Name == null)
          this.error(h, "Name is required in top level simpletype");
        else if (!XmlSchemaUtil.CheckNCName(this.Name))
          this.error(h, "name attribute of a simpleType must be NCName");
        else
          this.QNameInternal = new XmlQualifiedName(this.Name, this.AncestorSchema.TargetNamespace);
        switch (this.Final)
        {
          case XmlSchemaDerivationMethod.Restriction:
          case XmlSchemaDerivationMethod.List:
          case XmlSchemaDerivationMethod.Union:
            this.finalResolved = this.Final;
            break;
          case XmlSchemaDerivationMethod.All:
            this.finalResolved = XmlSchemaDerivationMethod.All;
            break;
          case XmlSchemaDerivationMethod.None:
            XmlSchemaDerivationMethod derivationMethod = XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction | XmlSchemaDerivationMethod.List | XmlSchemaDerivationMethod.Union;
            switch (schema.FinalDefault)
            {
              case XmlSchemaDerivationMethod.All:
                this.finalResolved = XmlSchemaDerivationMethod.All;
                break;
              case XmlSchemaDerivationMethod.None:
                this.finalResolved = XmlSchemaDerivationMethod.Empty;
                break;
              default:
                this.finalResolved = schema.FinalDefault & derivationMethod;
                break;
            }
            break;
          default:
            this.error(h, "The value of final attribute is not valid for simpleType");
            goto case XmlSchemaDerivationMethod.None;
        }
      }
      XmlSchemaUtil.CompileID(this.Id, (XmlSchemaObject) this, schema.IDCollection, h);
      if (this.Content != null)
        this.Content.OwnerType = this;
      if (this.Content == null)
        this.error(h, "Content is required in a simpletype");
      else if (this.Content is XmlSchemaSimpleTypeRestriction)
      {
        this.resolvedDerivedBy = XmlSchemaDerivationMethod.Restriction;
        this.errorCount += ((XmlSchemaSimpleTypeRestriction) this.Content).Compile(h, schema);
      }
      else if (this.Content is XmlSchemaSimpleTypeList)
      {
        this.resolvedDerivedBy = XmlSchemaDerivationMethod.List;
        this.errorCount += ((XmlSchemaSimpleTypeList) this.Content).Compile(h, schema);
      }
      else if (this.Content is XmlSchemaSimpleTypeUnion)
      {
        this.resolvedDerivedBy = XmlSchemaDerivationMethod.Union;
        this.errorCount += ((XmlSchemaSimpleTypeUnion) this.Content).Compile(h, schema);
      }
      this.CompilationId = schema.CompilationId;
      return this.errorCount;
    }

    internal void CollectBaseType(ValidationEventHandler h, XmlSchema schema)
    {
      if (this.Content is XmlSchemaSimpleTypeRestriction)
      {
        object actualType = ((XmlSchemaSimpleTypeRestriction) this.Content).GetActualType(h, schema, false);
        this.BaseXmlSchemaTypeInternal = (XmlSchemaType) (actualType as XmlSchemaSimpleType);
        if (this.BaseXmlSchemaTypeInternal != null)
          this.DatatypeInternal = this.BaseXmlSchemaTypeInternal.Datatype;
        else
          this.DatatypeInternal = actualType as XmlSchemaDatatype;
      }
      else
        this.DatatypeInternal = (XmlSchemaDatatype) XmlSchemaSimpleType.AnySimpleType;
    }

    internal override int Validate(ValidationEventHandler h, XmlSchema schema)
    {
      if (this.IsValidated(schema.ValidationId))
        return this.errorCount;
      if (this.recursed)
      {
        this.error(h, "Circular type reference was found.");
        return this.errorCount;
      }
      this.recursed = true;
      this.CollectBaseType(h, schema);
      if (this.content != null)
        this.errorCount += this.content.Validate(h, schema);
      if (this.BaseXmlSchemaType is XmlSchemaSimpleType baseXmlSchemaType1)
        this.DatatypeInternal = baseXmlSchemaType1.Datatype;
      if (this.BaseXmlSchemaType is XmlSchemaSimpleType baseXmlSchemaType2 && (baseXmlSchemaType2.FinalResolved & this.resolvedDerivedBy) != XmlSchemaDerivationMethod.Empty)
        this.error(h, "Specified derivation is prohibited by the base simple type.");
      this.variety = this.resolvedDerivedBy != XmlSchemaDerivationMethod.Restriction || baseXmlSchemaType2 == null ? this.resolvedDerivedBy : baseXmlSchemaType2.Variety;
      XmlSchemaSimpleTypeRestriction content1 = this.Content as XmlSchemaSimpleTypeRestriction;
      object baseType = this.BaseXmlSchemaType == null ? (object) this.Datatype : (object) this.BaseXmlSchemaType;
      if (content1 != null)
        this.ValidateDerivationValid(baseType, content1.Facets, h, schema);
      if (this.Content is XmlSchemaSimpleTypeList content2 && content2.ValidatedListItemType is XmlSchemaSimpleType validatedListItemType && validatedListItemType.Content is XmlSchemaSimpleTypeList)
        this.error(h, "List type must not be derived from another list type.");
      this.recursed = false;
      this.ValidationId = schema.ValidationId;
      return this.errorCount;
    }

    internal void ValidateDerivationValid(
      object baseType,
      XmlSchemaObjectCollection facets,
      ValidationEventHandler h,
      XmlSchema schema)
    {
      XmlSchemaSimpleType schemaSimpleType = baseType as XmlSchemaSimpleType;
      switch (this.Variety)
      {
        case XmlSchemaDerivationMethod.Restriction:
          if (schemaSimpleType != null && schemaSimpleType.resolvedDerivedBy != XmlSchemaDerivationMethod.Restriction)
            this.error(h, "Base schema type is not either atomic type or primitive type.");
          if (schemaSimpleType == null || (schemaSimpleType.FinalResolved & XmlSchemaDerivationMethod.Restriction) == XmlSchemaDerivationMethod.Empty)
            break;
          this.error(h, "Derivation by restriction is prohibited by the base simple type.");
          break;
        case XmlSchemaDerivationMethod.List:
          if (facets == null)
            break;
          XmlSchemaObjectEnumerator enumerator1 = facets.GetEnumerator();
          try
          {
            while (enumerator1.MoveNext())
            {
              switch ((XmlSchemaFacet) enumerator1.Current)
              {
                case XmlSchemaLengthFacet _:
                case XmlSchemaMaxLengthFacet _:
                case XmlSchemaMinLengthFacet _:
                case XmlSchemaEnumerationFacet _:
                case XmlSchemaPatternFacet _:
                  continue;
                default:
                  this.error(h, "Not allowed facet was found on this simple type which derives list type.");
                  continue;
              }
            }
            break;
          }
          finally
          {
            if (enumerator1 is IDisposable disposable)
              disposable.Dispose();
          }
        case XmlSchemaDerivationMethod.Union:
          if (facets == null)
            break;
          XmlSchemaObjectEnumerator enumerator2 = facets.GetEnumerator();
          try
          {
            while (enumerator2.MoveNext())
            {
              switch ((XmlSchemaFacet) enumerator2.Current)
              {
                case XmlSchemaEnumerationFacet _:
                case XmlSchemaPatternFacet _:
                  continue;
                default:
                  this.error(h, "Not allowed facet was found on this simple type which derives list type.");
                  continue;
              }
            }
            break;
          }
          finally
          {
            if (enumerator2 is IDisposable disposable)
              disposable.Dispose();
          }
      }
    }

    internal bool ValidateTypeDerivationOK(
      object baseType,
      ValidationEventHandler h,
      XmlSchema schema,
      bool raiseError)
    {
      if (this == baseType || baseType == XmlSchemaSimpleType.AnySimpleType || baseType == XmlSchemaComplexType.AnyType)
        return true;
      if (baseType is XmlSchemaSimpleType schemaSimpleType && (schemaSimpleType.FinalResolved & this.resolvedDerivedBy) != XmlSchemaDerivationMethod.Empty)
      {
        if (raiseError)
          this.error(h, "Specified derivation is prohibited by the base type.");
        return false;
      }
      if (this.BaseXmlSchemaType == baseType || this.Datatype == baseType || this.BaseXmlSchemaType is XmlSchemaSimpleType baseXmlSchemaType && baseXmlSchemaType.ValidateTypeDerivationOK(baseType, h, schema, false))
        return true;
      switch (this.Variety)
      {
        case XmlSchemaDerivationMethod.List:
        case XmlSchemaDerivationMethod.Union:
          if (baseType == XmlSchemaSimpleType.AnySimpleType)
            return true;
          break;
      }
      if (schemaSimpleType != null && schemaSimpleType.Variety == XmlSchemaDerivationMethod.Union)
      {
        foreach (object validatedType in ((XmlSchemaSimpleTypeUnion) schemaSimpleType.Content).ValidatedTypes)
        {
          if (this.ValidateTypeDerivationOK(validatedType, h, schema, false))
            return true;
        }
      }
      if (raiseError)
        this.error(h, "Invalid simple type derivation was found.");
      return false;
    }

    internal string Normalize(string s, XmlNameTable nt, XmlNamespaceManager nsmgr) => this.Content.Normalize(s, nt, nsmgr);

    internal static XmlSchemaSimpleType Read(XmlSchemaReader reader, ValidationEventHandler h)
    {
      XmlSchemaSimpleType xso = new XmlSchemaSimpleType();
      reader.MoveToElement();
      if (reader.NamespaceURI != "http://www.w3.org/2001/XMLSchema" || reader.LocalName != "simpleType")
      {
        XmlSchemaObject.error(h, "Should not happen :1: XmlSchemaGroup.Read, name=" + reader.Name, (Exception) null);
        reader.Skip();
        return (XmlSchemaSimpleType) null;
      }
      xso.LineNumber = reader.LineNumber;
      xso.LinePosition = reader.LinePosition;
      xso.SourceUri = reader.BaseURI;
      while (reader.MoveToNextAttribute())
      {
        if (reader.Name == "final")
        {
          Exception innerExcpetion;
          xso.Final = XmlSchemaUtil.ReadDerivationAttribute((XmlReader) reader, out innerExcpetion, "final", XmlSchemaUtil.FinalAllowed);
          if (innerExcpetion != null)
            XmlSchemaObject.error(h, "some invalid values not a valid value for final", innerExcpetion);
        }
        else if (reader.Name == "id")
          xso.Id = reader.Value;
        else if (reader.Name == "name")
          xso.Name = reader.Value;
        else if (reader.NamespaceURI == string.Empty && reader.Name != "xmlns" || reader.NamespaceURI == "http://www.w3.org/2001/XMLSchema")
          XmlSchemaObject.error(h, reader.Name + " is not a valid attribute for simpleType", (Exception) null);
        else
          XmlSchemaUtil.ReadUnhandledAttribute((XmlReader) reader, (XmlSchemaObject) xso);
      }
      reader.MoveToElement();
      if (reader.IsEmptyElement)
        return xso;
      int num = 1;
      while (reader.ReadNextElement())
      {
        if (reader.NodeType == XmlNodeType.EndElement)
        {
          if (reader.LocalName != "simpleType")
          {
            XmlSchemaObject.error(h, "Should not happen :2: XmlSchemaSimpleType.Read, name=" + reader.Name, (Exception) null);
            break;
          }
          break;
        }
        if (num <= 1 && reader.LocalName == "annotation")
        {
          num = 2;
          XmlSchemaAnnotation schemaAnnotation = XmlSchemaAnnotation.Read(reader, h);
          if (schemaAnnotation != null)
            xso.Annotation = schemaAnnotation;
        }
        else
        {
          if (num <= 2)
          {
            if (reader.LocalName == "restriction")
            {
              num = 3;
              XmlSchemaSimpleTypeRestriction simpleTypeRestriction = XmlSchemaSimpleTypeRestriction.Read(reader, h);
              if (simpleTypeRestriction != null)
              {
                xso.content = (XmlSchemaSimpleTypeContent) simpleTypeRestriction;
                continue;
              }
              continue;
            }
            if (reader.LocalName == "list")
            {
              num = 3;
              XmlSchemaSimpleTypeList schemaSimpleTypeList = XmlSchemaSimpleTypeList.Read(reader, h);
              if (schemaSimpleTypeList != null)
              {
                xso.content = (XmlSchemaSimpleTypeContent) schemaSimpleTypeList;
                continue;
              }
              continue;
            }
            if (reader.LocalName == "union")
            {
              num = 3;
              XmlSchemaSimpleTypeUnion schemaSimpleTypeUnion = XmlSchemaSimpleTypeUnion.Read(reader, h);
              if (schemaSimpleTypeUnion != null)
              {
                xso.content = (XmlSchemaSimpleTypeContent) schemaSimpleTypeUnion;
                continue;
              }
              continue;
            }
          }
          reader.RaiseInvalidElementError();
        }
      }
      return xso;
    }
  }
}
