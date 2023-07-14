// Decompiled with JetBrains decompiler
// Type: System.Xml.Schema.XmlSchemaDatatype
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using Mono.Xml.Schema;
using System.Collections.Generic;
using System.Text;

namespace System.Xml.Schema
{
  public abstract class XmlSchemaDatatype
  {
    internal XsdWhitespaceFacet WhitespaceValue;
    private static char[] wsChars = new char[4]
    {
      ' ',
      '\t',
      '\n',
      '\r'
    };
    private StringBuilder sb = new StringBuilder();
    private static readonly XsdAnySimpleType datatypeAnySimpleType = XsdAnySimpleType.Instance;
    private static readonly XsdString datatypeString = new XsdString();
    private static readonly XsdNormalizedString datatypeNormalizedString = new XsdNormalizedString();
    private static readonly XsdToken datatypeToken = new XsdToken();
    private static readonly XsdLanguage datatypeLanguage = new XsdLanguage();
    private static readonly XsdNMToken datatypeNMToken = new XsdNMToken();
    private static readonly XsdNMTokens datatypeNMTokens = new XsdNMTokens();
    private static readonly XsdName datatypeName = new XsdName();
    private static readonly XsdNCName datatypeNCName = new XsdNCName();
    private static readonly XsdID datatypeID = new XsdID();
    private static readonly XsdIDRef datatypeIDRef = new XsdIDRef();
    private static readonly XsdIDRefs datatypeIDRefs = new XsdIDRefs();
    private static readonly XsdEntity datatypeEntity = new XsdEntity();
    private static readonly XsdEntities datatypeEntities = new XsdEntities();
    private static readonly XsdNotation datatypeNotation = new XsdNotation();
    private static readonly XsdDecimal datatypeDecimal = new XsdDecimal();
    private static readonly XsdInteger datatypeInteger = new XsdInteger();
    private static readonly XsdLong datatypeLong = new XsdLong();
    private static readonly XsdInt datatypeInt = new XsdInt();
    private static readonly XsdShort datatypeShort = new XsdShort();
    private static readonly XsdByte datatypeByte = new XsdByte();
    private static readonly XsdNonNegativeInteger datatypeNonNegativeInteger = new XsdNonNegativeInteger();
    private static readonly XsdPositiveInteger datatypePositiveInteger = new XsdPositiveInteger();
    private static readonly XsdUnsignedLong datatypeUnsignedLong = new XsdUnsignedLong();
    private static readonly XsdUnsignedInt datatypeUnsignedInt = new XsdUnsignedInt();
    private static readonly XsdUnsignedShort datatypeUnsignedShort = new XsdUnsignedShort();
    private static readonly XsdUnsignedByte datatypeUnsignedByte = new XsdUnsignedByte();
    private static readonly XsdNonPositiveInteger datatypeNonPositiveInteger = new XsdNonPositiveInteger();
    private static readonly XsdNegativeInteger datatypeNegativeInteger = new XsdNegativeInteger();
    private static readonly XsdFloat datatypeFloat = new XsdFloat();
    private static readonly XsdDouble datatypeDouble = new XsdDouble();
    private static readonly XsdBase64Binary datatypeBase64Binary = new XsdBase64Binary();
    private static readonly XsdBoolean datatypeBoolean = new XsdBoolean();
    private static readonly XsdAnyURI datatypeAnyURI = new XsdAnyURI();
    private static readonly XsdDuration datatypeDuration = new XsdDuration();
    private static readonly XsdDateTime datatypeDateTime = new XsdDateTime();
    private static readonly XsdDate datatypeDate = new XsdDate();
    private static readonly XsdTime datatypeTime = new XsdTime();
    private static readonly XsdHexBinary datatypeHexBinary = new XsdHexBinary();
    private static readonly XsdQName datatypeQName = new XsdQName();
    private static readonly XsdGYearMonth datatypeGYearMonth = new XsdGYearMonth();
    private static readonly XsdGMonthDay datatypeGMonthDay = new XsdGMonthDay();
    private static readonly XsdGYear datatypeGYear = new XsdGYear();
    private static readonly XsdGMonth datatypeGMonth = new XsdGMonth();
    private static readonly XsdGDay datatypeGDay = new XsdGDay();
    private static readonly XdtAnyAtomicType datatypeAnyAtomicType = new XdtAnyAtomicType();
    private static readonly XdtUntypedAtomic datatypeUntypedAtomic = new XdtUntypedAtomic();
    private static readonly XdtDayTimeDuration datatypeDayTimeDuration = new XdtDayTimeDuration();
    private static readonly XdtYearMonthDuration datatypeYearMonthDuration = new XdtYearMonthDuration();

    internal virtual XsdWhitespaceFacet Whitespace => this.WhitespaceValue;

    public virtual XmlTypeCode TypeCode => XmlTypeCode.None;

    public virtual XmlSchemaDatatypeVariety Variety => XmlSchemaDatatypeVariety.Atomic;

    public abstract XmlTokenizedType TokenizedType { get; }

    public abstract Type ValueType { get; }

    [MonoTODO]
    public virtual object ChangeType(object value, Type targetType) => this.ChangeType(value, targetType, (IXmlNamespaceResolver) null);

    [MonoTODO]
    public virtual object ChangeType(
      object value,
      Type targetType,
      IXmlNamespaceResolver nsResolver)
    {
      throw new NotImplementedException();
    }

    public virtual bool IsDerivedFrom(XmlSchemaDatatype datatype) => this == datatype;

    public abstract object ParseValue(
      string s,
      XmlNameTable nameTable,
      IXmlNamespaceResolver nsmgr);

    internal virtual System.ValueType ParseValueType(
      string s,
      XmlNameTable nameTable,
      IXmlNamespaceResolver nsmgr)
    {
      return (System.ValueType) null;
    }

    internal string Normalize(string s) => this.Normalize(s, this.Whitespace);

    internal string Normalize(string s, XsdWhitespaceFacet whitespaceFacet)
    {
      if (s.IndexOfAny(XmlSchemaDatatype.wsChars) < 0)
        return s;
      switch (whitespaceFacet)
      {
        case XsdWhitespaceFacet.Replace:
          this.sb.Length = 0;
          this.sb.Append(s);
          for (int index = 0; index < this.sb.Length; ++index)
          {
            switch (this.sb[index])
            {
              case '\t':
              case '\n':
              case '\r':
                this.sb[index] = ' ';
                break;
            }
          }
          string str1 = this.sb.ToString();
          this.sb.Length = 0;
          return str1;
        case XsdWhitespaceFacet.Collapse:
          foreach (string str2 in s.Trim().Split(XmlSchemaDatatype.wsChars))
          {
            if (str2 != string.Empty)
            {
              this.sb.Append(str2);
              this.sb.Append(" ");
            }
          }
          string str3 = this.sb.ToString();
          this.sb.Length = 0;
          return str3.Trim();
        default:
          return s;
      }
    }

    internal static XmlSchemaDatatype FromName(XmlQualifiedName qname) => XmlSchemaDatatype.FromName(qname.Name, qname.Namespace);

    internal static XmlSchemaDatatype FromName(string localName, string ns)
    {
      string key1 = ns;
      if (key1 != null)
      {
        // ISSUE: reference to a compiler-generated field
        if (XmlSchemaDatatype.\u003C\u003Ef__switch\u0024map2B == null)
        {
          // ISSUE: reference to a compiler-generated field
          XmlSchemaDatatype.\u003C\u003Ef__switch\u0024map2B = new Dictionary<string, int>(2)
          {
            {
              "http://www.w3.org/2001/XMLSchema",
              0
            },
            {
              "http://www.w3.org/2003/11/xpath-datatypes",
              1
            }
          };
        }
        int num1;
        // ISSUE: reference to a compiler-generated field
        if (XmlSchemaDatatype.\u003C\u003Ef__switch\u0024map2B.TryGetValue(key1, out num1))
        {
          switch (num1)
          {
            case 0:
              string key2 = localName;
              if (key2 != null)
              {
                // ISSUE: reference to a compiler-generated field
                if (XmlSchemaDatatype.\u003C\u003Ef__switch\u0024map2C == null)
                {
                  // ISSUE: reference to a compiler-generated field
                  XmlSchemaDatatype.\u003C\u003Ef__switch\u0024map2C = new Dictionary<string, int>(45)
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
                      "normalizedString",
                      2
                    },
                    {
                      "token",
                      3
                    },
                    {
                      "language",
                      4
                    },
                    {
                      "NMTOKEN",
                      5
                    },
                    {
                      "NMTOKENS",
                      6
                    },
                    {
                      "Name",
                      7
                    },
                    {
                      "NCName",
                      8
                    },
                    {
                      "ID",
                      9
                    },
                    {
                      "IDREF",
                      10
                    },
                    {
                      "IDREFS",
                      11
                    },
                    {
                      "ENTITY",
                      12
                    },
                    {
                      "ENTITIES",
                      13
                    },
                    {
                      "NOTATION",
                      14
                    },
                    {
                      "decimal",
                      15
                    },
                    {
                      "integer",
                      16
                    },
                    {
                      "long",
                      17
                    },
                    {
                      "int",
                      18
                    },
                    {
                      "short",
                      19
                    },
                    {
                      "byte",
                      20
                    },
                    {
                      "nonPositiveInteger",
                      21
                    },
                    {
                      "negativeInteger",
                      22
                    },
                    {
                      "nonNegativeInteger",
                      23
                    },
                    {
                      "unsignedLong",
                      24
                    },
                    {
                      "unsignedInt",
                      25
                    },
                    {
                      "unsignedShort",
                      26
                    },
                    {
                      "unsignedByte",
                      27
                    },
                    {
                      "positiveInteger",
                      28
                    },
                    {
                      "float",
                      29
                    },
                    {
                      "double",
                      30
                    },
                    {
                      "base64Binary",
                      31
                    },
                    {
                      "boolean",
                      32
                    },
                    {
                      "anyURI",
                      33
                    },
                    {
                      "duration",
                      34
                    },
                    {
                      "dateTime",
                      35
                    },
                    {
                      "date",
                      36
                    },
                    {
                      "time",
                      37
                    },
                    {
                      "hexBinary",
                      38
                    },
                    {
                      "QName",
                      39
                    },
                    {
                      "gYearMonth",
                      40
                    },
                    {
                      "gMonthDay",
                      41
                    },
                    {
                      "gYear",
                      42
                    },
                    {
                      "gMonth",
                      43
                    },
                    {
                      "gDay",
                      44
                    }
                  };
                }
                // ISSUE: reference to a compiler-generated field
                if (XmlSchemaDatatype.\u003C\u003Ef__switch\u0024map2C.TryGetValue(key2, out num1))
                {
                  switch (num1)
                  {
                    case 0:
                      return (XmlSchemaDatatype) XmlSchemaDatatype.datatypeAnySimpleType;
                    case 1:
                      return (XmlSchemaDatatype) XmlSchemaDatatype.datatypeString;
                    case 2:
                      return (XmlSchemaDatatype) XmlSchemaDatatype.datatypeNormalizedString;
                    case 3:
                      return (XmlSchemaDatatype) XmlSchemaDatatype.datatypeToken;
                    case 4:
                      return (XmlSchemaDatatype) XmlSchemaDatatype.datatypeLanguage;
                    case 5:
                      return (XmlSchemaDatatype) XmlSchemaDatatype.datatypeNMToken;
                    case 6:
                      return (XmlSchemaDatatype) XmlSchemaDatatype.datatypeNMTokens;
                    case 7:
                      return (XmlSchemaDatatype) XmlSchemaDatatype.datatypeName;
                    case 8:
                      return (XmlSchemaDatatype) XmlSchemaDatatype.datatypeNCName;
                    case 9:
                      return (XmlSchemaDatatype) XmlSchemaDatatype.datatypeID;
                    case 10:
                      return (XmlSchemaDatatype) XmlSchemaDatatype.datatypeIDRef;
                    case 11:
                      return (XmlSchemaDatatype) XmlSchemaDatatype.datatypeIDRefs;
                    case 12:
                      return (XmlSchemaDatatype) XmlSchemaDatatype.datatypeEntity;
                    case 13:
                      return (XmlSchemaDatatype) XmlSchemaDatatype.datatypeEntities;
                    case 14:
                      return (XmlSchemaDatatype) XmlSchemaDatatype.datatypeNotation;
                    case 15:
                      return (XmlSchemaDatatype) XmlSchemaDatatype.datatypeDecimal;
                    case 16:
                      return (XmlSchemaDatatype) XmlSchemaDatatype.datatypeInteger;
                    case 17:
                      return (XmlSchemaDatatype) XmlSchemaDatatype.datatypeLong;
                    case 18:
                      return (XmlSchemaDatatype) XmlSchemaDatatype.datatypeInt;
                    case 19:
                      return (XmlSchemaDatatype) XmlSchemaDatatype.datatypeShort;
                    case 20:
                      return (XmlSchemaDatatype) XmlSchemaDatatype.datatypeByte;
                    case 21:
                      return (XmlSchemaDatatype) XmlSchemaDatatype.datatypeNonPositiveInteger;
                    case 22:
                      return (XmlSchemaDatatype) XmlSchemaDatatype.datatypeNegativeInteger;
                    case 23:
                      return (XmlSchemaDatatype) XmlSchemaDatatype.datatypeNonNegativeInteger;
                    case 24:
                      return (XmlSchemaDatatype) XmlSchemaDatatype.datatypeUnsignedLong;
                    case 25:
                      return (XmlSchemaDatatype) XmlSchemaDatatype.datatypeUnsignedInt;
                    case 26:
                      return (XmlSchemaDatatype) XmlSchemaDatatype.datatypeUnsignedShort;
                    case 27:
                      return (XmlSchemaDatatype) XmlSchemaDatatype.datatypeUnsignedByte;
                    case 28:
                      return (XmlSchemaDatatype) XmlSchemaDatatype.datatypePositiveInteger;
                    case 29:
                      return (XmlSchemaDatatype) XmlSchemaDatatype.datatypeFloat;
                    case 30:
                      return (XmlSchemaDatatype) XmlSchemaDatatype.datatypeDouble;
                    case 31:
                      return (XmlSchemaDatatype) XmlSchemaDatatype.datatypeBase64Binary;
                    case 32:
                      return (XmlSchemaDatatype) XmlSchemaDatatype.datatypeBoolean;
                    case 33:
                      return (XmlSchemaDatatype) XmlSchemaDatatype.datatypeAnyURI;
                    case 34:
                      return (XmlSchemaDatatype) XmlSchemaDatatype.datatypeDuration;
                    case 35:
                      return (XmlSchemaDatatype) XmlSchemaDatatype.datatypeDateTime;
                    case 36:
                      return (XmlSchemaDatatype) XmlSchemaDatatype.datatypeDate;
                    case 37:
                      return (XmlSchemaDatatype) XmlSchemaDatatype.datatypeTime;
                    case 38:
                      return (XmlSchemaDatatype) XmlSchemaDatatype.datatypeHexBinary;
                    case 39:
                      return (XmlSchemaDatatype) XmlSchemaDatatype.datatypeQName;
                    case 40:
                      return (XmlSchemaDatatype) XmlSchemaDatatype.datatypeGYearMonth;
                    case 41:
                      return (XmlSchemaDatatype) XmlSchemaDatatype.datatypeGMonthDay;
                    case 42:
                      return (XmlSchemaDatatype) XmlSchemaDatatype.datatypeGYear;
                    case 43:
                      return (XmlSchemaDatatype) XmlSchemaDatatype.datatypeGMonth;
                    case 44:
                      return (XmlSchemaDatatype) XmlSchemaDatatype.datatypeGDay;
                  }
                }
              }
              return (XmlSchemaDatatype) null;
            case 1:
              string key3 = localName;
              if (key3 != null)
              {
                // ISSUE: reference to a compiler-generated field
                if (XmlSchemaDatatype.\u003C\u003Ef__switch\u0024map2A == null)
                {
                  // ISSUE: reference to a compiler-generated field
                  XmlSchemaDatatype.\u003C\u003Ef__switch\u0024map2A = new Dictionary<string, int>(4)
                  {
                    {
                      "anyAtomicType",
                      0
                    },
                    {
                      "untypedAtomic",
                      1
                    },
                    {
                      "dayTimeDuration",
                      2
                    },
                    {
                      "yearMonthDuration",
                      3
                    }
                  };
                }
                int num2;
                // ISSUE: reference to a compiler-generated field
                if (XmlSchemaDatatype.\u003C\u003Ef__switch\u0024map2A.TryGetValue(key3, out num2))
                {
                  switch (num2)
                  {
                    case 0:
                      return (XmlSchemaDatatype) XmlSchemaDatatype.datatypeAnyAtomicType;
                    case 1:
                      return (XmlSchemaDatatype) XmlSchemaDatatype.datatypeUntypedAtomic;
                    case 2:
                      return (XmlSchemaDatatype) XmlSchemaDatatype.datatypeDayTimeDuration;
                    case 3:
                      return (XmlSchemaDatatype) XmlSchemaDatatype.datatypeYearMonthDuration;
                  }
                }
              }
              return (XmlSchemaDatatype) null;
          }
        }
      }
      return (XmlSchemaDatatype) null;
    }
  }
}
