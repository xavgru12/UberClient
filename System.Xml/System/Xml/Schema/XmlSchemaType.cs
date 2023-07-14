// Decompiled with JetBrains decompiler
// Type: System.Xml.Schema.XmlSchemaType
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using Mono.Xml.Schema;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
  public class XmlSchemaType : XmlSchemaAnnotated
  {
    private XmlSchemaDerivationMethod final;
    private bool isMixed;
    private string name;
    private bool recursed;
    internal XmlQualifiedName BaseSchemaTypeName;
    internal XmlSchemaType BaseXmlSchemaTypeInternal;
    internal XmlSchemaDatatype DatatypeInternal;
    internal XmlSchemaDerivationMethod resolvedDerivedBy;
    internal XmlSchemaDerivationMethod finalResolved;
    internal XmlQualifiedName QNameInternal;

    public XmlSchemaType()
    {
      this.final = XmlSchemaDerivationMethod.None;
      this.QNameInternal = XmlQualifiedName.Empty;
    }

    [XmlAttribute("name")]
    public string Name
    {
      get => this.name;
      set => this.name = value;
    }

    [DefaultValue(XmlSchemaDerivationMethod.None)]
    [XmlAttribute("final")]
    public XmlSchemaDerivationMethod Final
    {
      get => this.final;
      set => this.final = value;
    }

    [XmlIgnore]
    public XmlQualifiedName QualifiedName => this.QNameInternal;

    [XmlIgnore]
    public XmlSchemaDerivationMethod FinalResolved => this.finalResolved;

    [Obsolete("This property is going away. Use BaseXmlSchemaType instead")]
    [XmlIgnore]
    public object BaseSchemaType
    {
      get
      {
        if (this.BaseXmlSchemaType != null)
          return (object) this.BaseXmlSchemaType;
        return this == XmlSchemaComplexType.AnyType ? (object) null : (object) this.Datatype;
      }
    }

    [MonoTODO]
    [XmlIgnore]
    public XmlSchemaType BaseXmlSchemaType => this.BaseXmlSchemaTypeInternal;

    [XmlIgnore]
    public XmlSchemaDerivationMethod DerivedBy => this.resolvedDerivedBy;

    [XmlIgnore]
    public XmlSchemaDatatype Datatype => this.DatatypeInternal;

    [XmlIgnore]
    public virtual bool IsMixed
    {
      get => this.isMixed;
      set => this.isMixed = value;
    }

    [XmlIgnore]
    public XmlTypeCode TypeCode
    {
      get
      {
        if (this == XmlSchemaComplexType.AnyType)
          return XmlTypeCode.Item;
        if (this.DatatypeInternal == XmlSchemaSimpleType.AnySimpleType)
          return XmlTypeCode.AnyAtomicType;
        if (this == XmlSchemaSimpleType.XsIDRefs)
          return XmlTypeCode.Idref;
        if (this == XmlSchemaSimpleType.XsEntities)
          return XmlTypeCode.Entity;
        if (this == XmlSchemaSimpleType.XsNMTokens)
          return XmlTypeCode.NmToken;
        return this.DatatypeInternal != null ? this.DatatypeInternal.TypeCode : this.BaseXmlSchemaType.TypeCode;
      }
    }

    internal static XmlSchemaType GetBuiltInType(XmlQualifiedName qualifiedName) => (XmlSchemaType) XmlSchemaType.GetBuiltInSimpleType(qualifiedName) ?? (XmlSchemaType) XmlSchemaType.GetBuiltInComplexType(qualifiedName);

    internal static XmlSchemaType GetBuiltInType(XmlTypeCode typecode) => typecode == XmlTypeCode.Item ? (XmlSchemaType) XmlSchemaComplexType.AnyType : (XmlSchemaType) XmlSchemaType.GetBuiltInSimpleType(typecode);

    public static XmlSchemaComplexType GetBuiltInComplexType(XmlQualifiedName qualifiedName) => qualifiedName.Name == "anyType" && qualifiedName.Namespace == "http://www.w3.org/2001/XMLSchema" ? XmlSchemaComplexType.AnyType : (XmlSchemaComplexType) null;

    public static XmlSchemaComplexType GetBuiltInComplexType(XmlTypeCode type) => type == XmlTypeCode.Item ? XmlSchemaComplexType.AnyType : (XmlSchemaComplexType) null;

    [MonoTODO]
    public static XmlSchemaSimpleType GetBuiltInSimpleType(XmlQualifiedName qualifiedName)
    {
      if (qualifiedName.Namespace == "http://www.w3.org/2003/11/xpath-datatypes")
      {
        string name = qualifiedName.Name;
        if (name != null)
        {
          // ISSUE: reference to a compiler-generated field
          if (XmlSchemaType.\u003C\u003Ef__switch\u0024map2E == null)
          {
            // ISSUE: reference to a compiler-generated field
            XmlSchemaType.\u003C\u003Ef__switch\u0024map2E = new Dictionary<string, int>(4)
            {
              {
                "untypedAtomic",
                0
              },
              {
                "anyAtomicType",
                1
              },
              {
                "yearMonthDuration",
                2
              },
              {
                "dayTimeDuration",
                3
              }
            };
          }
          int num;
          // ISSUE: reference to a compiler-generated field
          if (XmlSchemaType.\u003C\u003Ef__switch\u0024map2E.TryGetValue(name, out num))
          {
            switch (num)
            {
              case 0:
                return XmlSchemaSimpleType.XdtUntypedAtomic;
              case 1:
                return XmlSchemaSimpleType.XdtAnyAtomicType;
              case 2:
                return XmlSchemaSimpleType.XdtYearMonthDuration;
              case 3:
                return XmlSchemaSimpleType.XdtDayTimeDuration;
            }
          }
        }
        return (XmlSchemaSimpleType) null;
      }
      if (qualifiedName.Namespace != "http://www.w3.org/2001/XMLSchema")
        return (XmlSchemaSimpleType) null;
      string name1 = qualifiedName.Name;
      if (name1 != null)
      {
        // ISSUE: reference to a compiler-generated field
        if (XmlSchemaType.\u003C\u003Ef__switch\u0024map2F == null)
        {
          // ISSUE: reference to a compiler-generated field
          XmlSchemaType.\u003C\u003Ef__switch\u0024map2F = new Dictionary<string, int>(45)
          {
            {
              "anySimpleType",
              0
            },
            {
              "string",
              1
            },
            {
              "boolean",
              2
            },
            {
              "decimal",
              3
            },
            {
              "float",
              4
            },
            {
              "double",
              5
            },
            {
              "duration",
              6
            },
            {
              "dateTime",
              7
            },
            {
              "time",
              8
            },
            {
              "date",
              9
            },
            {
              "gYearMonth",
              10
            },
            {
              "gYear",
              11
            },
            {
              "gMonthDay",
              12
            },
            {
              "gDay",
              13
            },
            {
              "gMonth",
              14
            },
            {
              "hexBinary",
              15
            },
            {
              "base64Binary",
              16
            },
            {
              "anyURI",
              17
            },
            {
              "QName",
              18
            },
            {
              "NOTATION",
              19
            },
            {
              "normalizedString",
              20
            },
            {
              "token",
              21
            },
            {
              "language",
              22
            },
            {
              "NMTOKEN",
              23
            },
            {
              "NMTOKENS",
              24
            },
            {
              "Name",
              25
            },
            {
              "NCName",
              26
            },
            {
              "ID",
              27
            },
            {
              "IDREF",
              28
            },
            {
              "IDREFS",
              29
            },
            {
              "ENTITY",
              30
            },
            {
              "ENTITIES",
              31
            },
            {
              "integer",
              32
            },
            {
              "nonPositiveInteger",
              33
            },
            {
              "negativeInteger",
              34
            },
            {
              "long",
              35
            },
            {
              "int",
              36
            },
            {
              "short",
              37
            },
            {
              "byte",
              38
            },
            {
              "nonNegativeInteger",
              39
            },
            {
              "positiveInteger",
              40
            },
            {
              "unsignedLong",
              41
            },
            {
              "unsignedInt",
              42
            },
            {
              "unsignedShort",
              43
            },
            {
              "unsignedByte",
              44
            }
          };
        }
        int num;
        // ISSUE: reference to a compiler-generated field
        if (XmlSchemaType.\u003C\u003Ef__switch\u0024map2F.TryGetValue(name1, out num))
        {
          switch (num)
          {
            case 0:
              return XmlSchemaSimpleType.XsAnySimpleType;
            case 1:
              return XmlSchemaSimpleType.XsString;
            case 2:
              return XmlSchemaSimpleType.XsBoolean;
            case 3:
              return XmlSchemaSimpleType.XsDecimal;
            case 4:
              return XmlSchemaSimpleType.XsFloat;
            case 5:
              return XmlSchemaSimpleType.XsDouble;
            case 6:
              return XmlSchemaSimpleType.XsDuration;
            case 7:
              return XmlSchemaSimpleType.XsDateTime;
            case 8:
              return XmlSchemaSimpleType.XsTime;
            case 9:
              return XmlSchemaSimpleType.XsDate;
            case 10:
              return XmlSchemaSimpleType.XsGYearMonth;
            case 11:
              return XmlSchemaSimpleType.XsGYear;
            case 12:
              return XmlSchemaSimpleType.XsGMonthDay;
            case 13:
              return XmlSchemaSimpleType.XsGDay;
            case 14:
              return XmlSchemaSimpleType.XsGMonth;
            case 15:
              return XmlSchemaSimpleType.XsHexBinary;
            case 16:
              return XmlSchemaSimpleType.XsBase64Binary;
            case 17:
              return XmlSchemaSimpleType.XsAnyUri;
            case 18:
              return XmlSchemaSimpleType.XsQName;
            case 19:
              return XmlSchemaSimpleType.XsNotation;
            case 20:
              return XmlSchemaSimpleType.XsNormalizedString;
            case 21:
              return XmlSchemaSimpleType.XsToken;
            case 22:
              return XmlSchemaSimpleType.XsLanguage;
            case 23:
              return XmlSchemaSimpleType.XsNMToken;
            case 24:
              return XmlSchemaSimpleType.XsNMTokens;
            case 25:
              return XmlSchemaSimpleType.XsName;
            case 26:
              return XmlSchemaSimpleType.XsNCName;
            case 27:
              return XmlSchemaSimpleType.XsID;
            case 28:
              return XmlSchemaSimpleType.XsIDRef;
            case 29:
              return XmlSchemaSimpleType.XsIDRefs;
            case 30:
              return XmlSchemaSimpleType.XsEntity;
            case 31:
              return XmlSchemaSimpleType.XsEntities;
            case 32:
              return XmlSchemaSimpleType.XsInteger;
            case 33:
              return XmlSchemaSimpleType.XsNonPositiveInteger;
            case 34:
              return XmlSchemaSimpleType.XsNegativeInteger;
            case 35:
              return XmlSchemaSimpleType.XsLong;
            case 36:
              return XmlSchemaSimpleType.XsInt;
            case 37:
              return XmlSchemaSimpleType.XsShort;
            case 38:
              return XmlSchemaSimpleType.XsByte;
            case 39:
              return XmlSchemaSimpleType.XsNonNegativeInteger;
            case 40:
              return XmlSchemaSimpleType.XsPositiveInteger;
            case 41:
              return XmlSchemaSimpleType.XsUnsignedLong;
            case 42:
              return XmlSchemaSimpleType.XsUnsignedInt;
            case 43:
              return XmlSchemaSimpleType.XsUnsignedShort;
            case 44:
              return XmlSchemaSimpleType.XsUnsignedByte;
          }
        }
      }
      return (XmlSchemaSimpleType) null;
    }

    internal static XmlSchemaSimpleType GetBuiltInSimpleType(XmlSchemaDatatype type)
    {
      switch (type)
      {
        case XsdEntities _:
          return XmlSchemaSimpleType.XsEntities;
        case XsdNMTokens _:
          return XmlSchemaSimpleType.XsNMTokens;
        case XsdIDRefs _:
          return XmlSchemaSimpleType.XsIDRefs;
        default:
          return XmlSchemaType.GetBuiltInSimpleType(type.TypeCode);
      }
    }

    [MonoTODO]
    public static XmlSchemaSimpleType GetBuiltInSimpleType(XmlTypeCode type)
    {
      switch (type)
      {
        case XmlTypeCode.None:
        case XmlTypeCode.Item:
        case XmlTypeCode.Node:
        case XmlTypeCode.Document:
        case XmlTypeCode.Element:
        case XmlTypeCode.Attribute:
        case XmlTypeCode.Namespace:
        case XmlTypeCode.ProcessingInstruction:
        case XmlTypeCode.Comment:
        case XmlTypeCode.Text:
          return (XmlSchemaSimpleType) null;
        case XmlTypeCode.AnyAtomicType:
          return XmlSchemaSimpleType.XdtAnyAtomicType;
        case XmlTypeCode.UntypedAtomic:
          return XmlSchemaSimpleType.XdtUntypedAtomic;
        case XmlTypeCode.String:
          return XmlSchemaSimpleType.XsString;
        case XmlTypeCode.Boolean:
          return XmlSchemaSimpleType.XsBoolean;
        case XmlTypeCode.Decimal:
          return XmlSchemaSimpleType.XsDecimal;
        case XmlTypeCode.Float:
          return XmlSchemaSimpleType.XsFloat;
        case XmlTypeCode.Double:
          return XmlSchemaSimpleType.XsDouble;
        case XmlTypeCode.Duration:
          return XmlSchemaSimpleType.XsDuration;
        case XmlTypeCode.DateTime:
          return XmlSchemaSimpleType.XsDateTime;
        case XmlTypeCode.Time:
          return XmlSchemaSimpleType.XsTime;
        case XmlTypeCode.Date:
          return XmlSchemaSimpleType.XsDate;
        case XmlTypeCode.GYearMonth:
          return XmlSchemaSimpleType.XsGYearMonth;
        case XmlTypeCode.GYear:
          return XmlSchemaSimpleType.XsGYear;
        case XmlTypeCode.GMonthDay:
          return XmlSchemaSimpleType.XsGMonthDay;
        case XmlTypeCode.GDay:
          return XmlSchemaSimpleType.XsGDay;
        case XmlTypeCode.GMonth:
          return XmlSchemaSimpleType.XsGMonth;
        case XmlTypeCode.HexBinary:
          return XmlSchemaSimpleType.XsHexBinary;
        case XmlTypeCode.Base64Binary:
          return XmlSchemaSimpleType.XsBase64Binary;
        case XmlTypeCode.AnyUri:
          return XmlSchemaSimpleType.XsAnyUri;
        case XmlTypeCode.QName:
          return XmlSchemaSimpleType.XsQName;
        case XmlTypeCode.Notation:
          return XmlSchemaSimpleType.XsNotation;
        case XmlTypeCode.NormalizedString:
          return XmlSchemaSimpleType.XsNormalizedString;
        case XmlTypeCode.Token:
          return XmlSchemaSimpleType.XsToken;
        case XmlTypeCode.Language:
          return XmlSchemaSimpleType.XsLanguage;
        case XmlTypeCode.NmToken:
          return XmlSchemaSimpleType.XsNMToken;
        case XmlTypeCode.Name:
          return XmlSchemaSimpleType.XsName;
        case XmlTypeCode.NCName:
          return XmlSchemaSimpleType.XsNCName;
        case XmlTypeCode.Id:
          return XmlSchemaSimpleType.XsID;
        case XmlTypeCode.Idref:
          return XmlSchemaSimpleType.XsIDRef;
        case XmlTypeCode.Entity:
          return XmlSchemaSimpleType.XsEntity;
        case XmlTypeCode.Integer:
          return XmlSchemaSimpleType.XsInteger;
        case XmlTypeCode.NonPositiveInteger:
          return XmlSchemaSimpleType.XsNonPositiveInteger;
        case XmlTypeCode.NegativeInteger:
          return XmlSchemaSimpleType.XsNegativeInteger;
        case XmlTypeCode.Long:
          return XmlSchemaSimpleType.XsLong;
        case XmlTypeCode.Int:
          return XmlSchemaSimpleType.XsInt;
        case XmlTypeCode.Short:
          return XmlSchemaSimpleType.XsShort;
        case XmlTypeCode.Byte:
          return XmlSchemaSimpleType.XsByte;
        case XmlTypeCode.NonNegativeInteger:
          return XmlSchemaSimpleType.XsNonNegativeInteger;
        case XmlTypeCode.UnsignedLong:
          return XmlSchemaSimpleType.XsUnsignedLong;
        case XmlTypeCode.UnsignedInt:
          return XmlSchemaSimpleType.XsUnsignedInt;
        case XmlTypeCode.UnsignedShort:
          return XmlSchemaSimpleType.XsUnsignedShort;
        case XmlTypeCode.UnsignedByte:
          return XmlSchemaSimpleType.XsUnsignedByte;
        case XmlTypeCode.PositiveInteger:
          return XmlSchemaSimpleType.XsPositiveInteger;
        case XmlTypeCode.YearMonthDuration:
          return XmlSchemaSimpleType.XdtYearMonthDuration;
        case XmlTypeCode.DayTimeDuration:
          return XmlSchemaSimpleType.XdtDayTimeDuration;
        default:
          return (XmlSchemaSimpleType) null;
      }
    }

    public static bool IsDerivedFrom(
      XmlSchemaType derivedType,
      XmlSchemaType baseType,
      XmlSchemaDerivationMethod except)
    {
      if (derivedType.BaseXmlSchemaType == null || (derivedType.DerivedBy & except) != XmlSchemaDerivationMethod.Empty)
        return false;
      return derivedType.BaseXmlSchemaType == baseType || XmlSchemaType.IsDerivedFrom(derivedType.BaseXmlSchemaType, baseType, except);
    }

    internal bool ValidateRecursionCheck()
    {
      if (this.recursed)
        return this != XmlSchemaComplexType.AnyType;
      this.recursed = true;
      XmlSchemaType baseXmlSchemaType = this.BaseXmlSchemaType;
      bool flag = false;
      if (baseXmlSchemaType != null)
        flag = baseXmlSchemaType.ValidateRecursionCheck();
      this.recursed = false;
      return flag;
    }
  }
}
