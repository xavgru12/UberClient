// Decompiled with JetBrains decompiler
// Type: System.Xml.Schema.XmlSchemaUtil
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using Mono.Xml;
using Mono.Xml.Schema;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
  internal class XmlSchemaUtil
  {
    internal static XmlSchemaDerivationMethod FinalAllowed;
    internal static XmlSchemaDerivationMethod ElementBlockAllowed;
    internal static XmlSchemaDerivationMethod ComplexTypeBlockAllowed;
    internal static readonly bool StrictMsCompliant = Environment.GetEnvironmentVariable("MONO_STRICT_MS_COMPLIANT") == "yes";

    static XmlSchemaUtil()
    {
      XmlSchemaUtil.FinalAllowed = XmlSchemaDerivationMethod.Extension | XmlSchemaDerivationMethod.Restriction;
      XmlSchemaUtil.ComplexTypeBlockAllowed = XmlSchemaUtil.FinalAllowed;
      XmlSchemaUtil.ElementBlockAllowed = XmlSchemaDerivationMethod.Substitution | XmlSchemaUtil.FinalAllowed;
    }

    public static void AddToTable(
      XmlSchemaObjectTable table,
      XmlSchemaObject obj,
      XmlQualifiedName qname,
      ValidationEventHandler h)
    {
      if (table.Contains(qname))
      {
        if (obj.isRedefineChild)
        {
          if (obj.redefinedObject != null)
            obj.error(h, string.Format("Named item {0} was already contained in the schema object table.", (object) qname));
          else
            obj.redefinedObject = table[qname];
          table.Set(qname, obj);
        }
        else if (table[qname].isRedefineChild)
        {
          if (table[qname].redefinedObject != null)
            obj.error(h, string.Format("Named item {0} was already contained in the schema object table.", (object) qname));
          else
            table[qname].redefinedObject = obj;
        }
        else if (XmlSchemaUtil.StrictMsCompliant)
          table.Set(qname, obj);
        else
          obj.error(h, string.Format("Named item {0} was already contained in the schema object table. {1}", (object) qname, (object) "Consider setting MONO_STRICT_MS_COMPLIANT to 'yes' to mimic MS implementation."));
      }
      else
        table.Set(qname, obj);
    }

    public static void CompileID(
      string id,
      XmlSchemaObject xso,
      Hashtable idCollection,
      ValidationEventHandler h)
    {
      if (id == null)
        return;
      if (!XmlSchemaUtil.CheckNCName(id))
        xso.error(h, id + " is not a valid id attribute");
      else if (idCollection.ContainsKey((object) id))
        xso.error(h, "Duplicate id attribute " + id);
      else
        idCollection.Add((object) id, (object) xso);
    }

    public static bool CheckAnyUri(string uri) => !uri.StartsWith("##");

    public static bool CheckNormalizedString(string token) => true;

    public static bool CheckNCName(string name) => XmlChar.IsNCName(name);

    public static bool CheckQName(XmlQualifiedName qname) => true;

    public static XmlParserContext GetParserContext(XmlReader reader) => reader is IHasXmlParserContext xmlParserContext ? xmlParserContext.ParserContext : (XmlParserContext) null;

    public static bool IsBuiltInDatatypeName(XmlQualifiedName qname)
    {
      if (qname.Namespace == "http://www.w3.org/2003/11/xpath-datatypes")
      {
        string name = qname.Name;
        if (name != null)
        {
          // ISSUE: reference to a compiler-generated field
          if (XmlSchemaUtil.\u003C\u003Ef__switch\u0024map36 == null)
          {
            // ISSUE: reference to a compiler-generated field
            XmlSchemaUtil.\u003C\u003Ef__switch\u0024map36 = new Dictionary<string, int>(4)
            {
              {
                "anyAtomicType",
                0
              },
              {
                "untypedAtomic",
                0
              },
              {
                "dayTimeDuration",
                0
              },
              {
                "yearMonthDuration",
                0
              }
            };
          }
          int num;
          // ISSUE: reference to a compiler-generated field
          if (XmlSchemaUtil.\u003C\u003Ef__switch\u0024map36.TryGetValue(name, out num) && num == 0)
            return true;
        }
        return false;
      }
      if (qname.Namespace != "http://www.w3.org/2001/XMLSchema")
        return false;
      string name1 = qname.Name;
      if (name1 != null)
      {
        // ISSUE: reference to a compiler-generated field
        if (XmlSchemaUtil.\u003C\u003Ef__switch\u0024map37 == null)
        {
          // ISSUE: reference to a compiler-generated field
          XmlSchemaUtil.\u003C\u003Ef__switch\u0024map37 = new Dictionary<string, int>(45)
          {
            {
              "anySimpleType",
              0
            },
            {
              "duration",
              0
            },
            {
              "dateTime",
              0
            },
            {
              "time",
              0
            },
            {
              "date",
              0
            },
            {
              "gYearMonth",
              0
            },
            {
              "gYear",
              0
            },
            {
              "gMonthDay",
              0
            },
            {
              "gDay",
              0
            },
            {
              "gMonth",
              0
            },
            {
              "boolean",
              0
            },
            {
              "base64Binary",
              0
            },
            {
              "hexBinary",
              0
            },
            {
              "float",
              0
            },
            {
              "double",
              0
            },
            {
              "anyURI",
              0
            },
            {
              "QName",
              0
            },
            {
              "NOTATION",
              0
            },
            {
              "string",
              0
            },
            {
              "normalizedString",
              0
            },
            {
              "token",
              0
            },
            {
              "language",
              0
            },
            {
              "Name",
              0
            },
            {
              "NCName",
              0
            },
            {
              "ID",
              0
            },
            {
              "IDREF",
              0
            },
            {
              "IDREFS",
              0
            },
            {
              "ENTITY",
              0
            },
            {
              "ENTITIES",
              0
            },
            {
              "NMTOKEN",
              0
            },
            {
              "NMTOKENS",
              0
            },
            {
              "decimal",
              0
            },
            {
              "integer",
              0
            },
            {
              "nonPositiveInteger",
              0
            },
            {
              "negativeInteger",
              0
            },
            {
              "nonNegativeInteger",
              0
            },
            {
              "unsignedLong",
              0
            },
            {
              "unsignedInt",
              0
            },
            {
              "unsignedShort",
              0
            },
            {
              "unsignedByte",
              0
            },
            {
              "positiveInteger",
              0
            },
            {
              "long",
              0
            },
            {
              "int",
              0
            },
            {
              "short",
              0
            },
            {
              "byte",
              0
            }
          };
        }
        int num;
        // ISSUE: reference to a compiler-generated field
        if (XmlSchemaUtil.\u003C\u003Ef__switch\u0024map37.TryGetValue(name1, out num) && num == 0)
          return true;
      }
      return false;
    }

    public static bool AreSchemaDatatypeEqual(
      XmlSchemaSimpleType st1,
      object v1,
      XmlSchemaSimpleType st2,
      object v2)
    {
      if (st1.Datatype is XsdAnySimpleType)
        return XmlSchemaUtil.AreSchemaDatatypeEqual(st1.Datatype as XsdAnySimpleType, v1, st2.Datatype as XsdAnySimpleType, v2);
      string[] strArray1 = v1 as string[];
      string[] strArray2 = v2 as string[];
      if (st1 != st2 || strArray1 == null || strArray2 == null || strArray1.Length != strArray2.Length)
        return false;
      for (int index = 0; index < strArray1.Length; ++index)
      {
        if (strArray1[index] != strArray2[index])
          return false;
      }
      return true;
    }

    public static bool AreSchemaDatatypeEqual(
      XsdAnySimpleType st1,
      object v1,
      XsdAnySimpleType st2,
      object v2)
    {
      if (v1 == null || v2 == null)
        return false;
      if (st1 == null)
        st1 = XmlSchemaSimpleType.AnySimpleType;
      if (st2 == null)
        st2 = XmlSchemaSimpleType.AnySimpleType;
      Type type = st2.GetType();
      switch (st1)
      {
        case XsdFloat _:
          return st2 is XsdFloat && (double) Convert.ToSingle(v1) == (double) Convert.ToSingle(v2);
        case XsdDouble _:
          return st2 is XsdDouble && Convert.ToDouble(v1) == Convert.ToDouble(v2);
        case XsdDecimal _:
          if (!(st2 is XsdDecimal) || Convert.ToDecimal(v1) != Convert.ToDecimal(v2))
            return false;
          switch (st1)
          {
            case XsdNonPositiveInteger _:
              return st2 is XsdNonPositiveInteger || type == typeof (XsdDecimal) || type == typeof (XsdInteger);
            case XsdPositiveInteger _:
              return st2 is XsdPositiveInteger || type == typeof (XsdDecimal) || type == typeof (XsdInteger) || type == typeof (XsdNonNegativeInteger);
            case XsdUnsignedLong _:
              return st2 is XsdUnsignedLong || type == typeof (XsdDecimal) || type == typeof (XsdInteger) || type == typeof (XsdNonNegativeInteger);
            case XsdNonNegativeInteger _:
              return st2 is XsdNonNegativeInteger || type == typeof (XsdDecimal) || type == typeof (XsdInteger);
            case XsdLong _:
              return st2 is XsdLong || type == typeof (XsdDecimal) || type == typeof (XsdInteger);
            default:
              return true;
          }
        default:
          if (!v1.Equals(v2))
            return false;
          if (st1 is XsdString)
          {
            if (!(st2 is XsdString) || st1 is XsdNMToken && (st2 is XsdLanguage || st2 is XsdName) || st2 is XsdNMToken && (st1 is XsdLanguage || st1 is XsdName) || st1 is XsdName && (st2 is XsdLanguage || st2 is XsdNMToken) || st2 is XsdName && (st1 is XsdLanguage || st1 is XsdNMToken) || st1 is XsdID && st2 is XsdIDRef || st1 is XsdIDRef && st2 is XsdID)
              return false;
          }
          else if (st1 != st2)
            return false;
          return true;
      }
    }

    public static bool IsValidQName(string qname)
    {
      string str = qname;
      char[] separator = new char[1]{ ':' };
      foreach (string name in str.Split(separator, 2))
      {
        if (!XmlSchemaUtil.CheckNCName(name))
          return false;
      }
      return true;
    }

    public static string[] SplitList(string list)
    {
      if (list == null || list == string.Empty)
        return new string[0];
      ArrayList arrayList = (ArrayList) null;
      int startIndex = 0;
      bool flag = true;
      for (int index = 0; index < list.Length; ++index)
      {
        char ch = list[index];
        switch (ch)
        {
          case '\t':
          case '\n':
          case '\r':
            if (!flag)
            {
              if (arrayList == null)
                arrayList = new ArrayList();
              arrayList.Add((object) list.Substring(startIndex, index - startIndex));
            }
            flag = true;
            break;
          default:
            if (ch != ' ')
            {
              if (flag)
              {
                flag = false;
                startIndex = index;
                break;
              }
              break;
            }
            goto case '\t';
        }
      }
      if (!flag && startIndex == 0)
        return new string[1]{ list };
      if (!flag && startIndex < list.Length)
        arrayList.Add(startIndex != 0 ? (object) list.Substring(startIndex) : (object) list);
      return arrayList.ToArray(typeof (string)) as string[];
    }

    public static void ReadUnhandledAttribute(XmlReader reader, XmlSchemaObject xso)
    {
      if (reader.Prefix == "xmlns")
        xso.Namespaces.Add(reader.LocalName, reader.Value);
      else if (reader.Name == "xmlns")
      {
        xso.Namespaces.Add(string.Empty, reader.Value);
      }
      else
      {
        if (xso.unhandledAttributeList == null)
          xso.unhandledAttributeList = new ArrayList();
        XmlAttribute attribute = new XmlDocument().CreateAttribute(reader.LocalName, reader.NamespaceURI);
        attribute.Value = reader.Value;
        XmlSchemaUtil.ParseWsdlArrayType(reader, attribute);
        xso.unhandledAttributeList.Add((object) attribute);
      }
    }

    private static void ParseWsdlArrayType(XmlReader reader, XmlAttribute attr)
    {
      if (!(attr.NamespaceURI == "http://schemas.xmlsoap.org/wsdl/") || !(attr.LocalName == "arrayType"))
        return;
      string ns = string.Empty;
      string type;
      string dimensions;
      TypeTranslator.ParseArrayType(attr.Value, out type, out ns, out dimensions);
      if (ns != string.Empty)
        ns = reader.LookupNamespace(ns) + ":";
      attr.Value = ns + type + dimensions;
    }

    public static bool ReadBoolAttribute(XmlReader reader, out Exception innerExcpetion)
    {
      innerExcpetion = (Exception) null;
      try
      {
        return XmlConvert.ToBoolean(reader.Value);
      }
      catch (Exception ex)
      {
        innerExcpetion = ex;
        return false;
      }
    }

    public static Decimal ReadDecimalAttribute(XmlReader reader, out Exception innerExcpetion)
    {
      innerExcpetion = (Exception) null;
      try
      {
        return XmlConvert.ToDecimal(reader.Value);
      }
      catch (Exception ex)
      {
        innerExcpetion = ex;
        return 0M;
      }
    }

    public static XmlSchemaDerivationMethod ReadDerivationAttribute(
      XmlReader reader,
      out Exception innerExcpetion,
      string name,
      XmlSchemaDerivationMethod allowed)
    {
      innerExcpetion = (Exception) null;
      try
      {
        string list = reader.Value;
        string str = string.Empty;
        XmlSchemaDerivationMethod dst = XmlSchemaDerivationMethod.Empty;
        if (list.IndexOf("#all") != -1 && list.Trim() != "#all")
        {
          innerExcpetion = new Exception(list + " is not a valid value for " + name + ". #all if present must be the only value");
          return XmlSchemaDerivationMethod.All;
        }
        foreach (string split in XmlSchemaUtil.SplitList(list))
        {
          string key = split;
          if (key != null)
          {
            // ISSUE: reference to a compiler-generated field
            if (XmlSchemaUtil.\u003C\u003Ef__switch\u0024map38 == null)
            {
              // ISSUE: reference to a compiler-generated field
              XmlSchemaUtil.\u003C\u003Ef__switch\u0024map38 = new Dictionary<string, int>(7)
              {
                {
                  string.Empty,
                  0
                },
                {
                  "#all",
                  1
                },
                {
                  "substitution",
                  2
                },
                {
                  "extension",
                  3
                },
                {
                  "restriction",
                  4
                },
                {
                  "list",
                  5
                },
                {
                  "union",
                  6
                }
              };
            }
            int num;
            // ISSUE: reference to a compiler-generated field
            if (XmlSchemaUtil.\u003C\u003Ef__switch\u0024map38.TryGetValue(key, out num))
            {
              switch (num)
              {
                case 0:
                  dst = XmlSchemaUtil.AddFlag(dst, XmlSchemaDerivationMethod.Empty, allowed);
                  continue;
                case 1:
                  dst = XmlSchemaUtil.AddFlag(dst, XmlSchemaDerivationMethod.All, allowed);
                  continue;
                case 2:
                  dst = XmlSchemaUtil.AddFlag(dst, XmlSchemaDerivationMethod.Substitution, allowed);
                  continue;
                case 3:
                  dst = XmlSchemaUtil.AddFlag(dst, XmlSchemaDerivationMethod.Extension, allowed);
                  continue;
                case 4:
                  dst = XmlSchemaUtil.AddFlag(dst, XmlSchemaDerivationMethod.Restriction, allowed);
                  continue;
                case 5:
                  dst = XmlSchemaUtil.AddFlag(dst, XmlSchemaDerivationMethod.List, allowed);
                  continue;
                case 6:
                  dst = XmlSchemaUtil.AddFlag(dst, XmlSchemaDerivationMethod.Union, allowed);
                  continue;
              }
            }
          }
          str = str + split + " ";
        }
        if (str != string.Empty)
          innerExcpetion = new Exception(str + "is/are not valid values for " + name);
        return dst;
      }
      catch (Exception ex)
      {
        innerExcpetion = ex;
        return XmlSchemaDerivationMethod.None;
      }
    }

    private static XmlSchemaDerivationMethod AddFlag(
      XmlSchemaDerivationMethod dst,
      XmlSchemaDerivationMethod add,
      XmlSchemaDerivationMethod allowed)
    {
      if ((add & allowed) == XmlSchemaDerivationMethod.Empty && allowed != XmlSchemaDerivationMethod.All)
        throw new ArgumentException(add.ToString() + " is not allowed in this attribute.");
      if ((dst & add) != XmlSchemaDerivationMethod.Empty)
        throw new ArgumentException(add.ToString() + " is already specified in this attribute.");
      return dst | add;
    }

    public static XmlSchemaForm ReadFormAttribute(XmlReader reader, out Exception innerExcpetion)
    {
      innerExcpetion = (Exception) null;
      XmlSchemaForm xmlSchemaForm = XmlSchemaForm.None;
      string key = reader.Value;
      if (key != null)
      {
        // ISSUE: reference to a compiler-generated field
        if (XmlSchemaUtil.\u003C\u003Ef__switch\u0024map39 == null)
        {
          // ISSUE: reference to a compiler-generated field
          XmlSchemaUtil.\u003C\u003Ef__switch\u0024map39 = new Dictionary<string, int>(2)
          {
            {
              "qualified",
              0
            },
            {
              "unqualified",
              1
            }
          };
        }
        int num;
        // ISSUE: reference to a compiler-generated field
        if (XmlSchemaUtil.\u003C\u003Ef__switch\u0024map39.TryGetValue(key, out num))
        {
          switch (num)
          {
            case 0:
              xmlSchemaForm = XmlSchemaForm.Qualified;
              goto label_8;
            case 1:
              xmlSchemaForm = XmlSchemaForm.Unqualified;
              goto label_8;
          }
        }
      }
      innerExcpetion = new Exception("only qualified or unqulified is a valid value");
label_8:
      return xmlSchemaForm;
    }

    public static XmlSchemaContentProcessing ReadProcessingAttribute(
      XmlReader reader,
      out Exception innerExcpetion)
    {
      innerExcpetion = (Exception) null;
      XmlSchemaContentProcessing contentProcessing = XmlSchemaContentProcessing.None;
      string key = reader.Value;
      if (key != null)
      {
        // ISSUE: reference to a compiler-generated field
        if (XmlSchemaUtil.\u003C\u003Ef__switch\u0024map3A == null)
        {
          // ISSUE: reference to a compiler-generated field
          XmlSchemaUtil.\u003C\u003Ef__switch\u0024map3A = new Dictionary<string, int>(3)
          {
            {
              "lax",
              0
            },
            {
              "strict",
              1
            },
            {
              "skip",
              2
            }
          };
        }
        int num;
        // ISSUE: reference to a compiler-generated field
        if (XmlSchemaUtil.\u003C\u003Ef__switch\u0024map3A.TryGetValue(key, out num))
        {
          switch (num)
          {
            case 0:
              contentProcessing = XmlSchemaContentProcessing.Lax;
              goto label_9;
            case 1:
              contentProcessing = XmlSchemaContentProcessing.Strict;
              goto label_9;
            case 2:
              contentProcessing = XmlSchemaContentProcessing.Skip;
              goto label_9;
          }
        }
      }
      innerExcpetion = new Exception("only lax , strict or skip are valid values for processContents");
label_9:
      return contentProcessing;
    }

    public static XmlSchemaUse ReadUseAttribute(XmlReader reader, out Exception innerExcpetion)
    {
      innerExcpetion = (Exception) null;
      XmlSchemaUse xmlSchemaUse = XmlSchemaUse.None;
      string key = reader.Value;
      if (key != null)
      {
        // ISSUE: reference to a compiler-generated field
        if (XmlSchemaUtil.\u003C\u003Ef__switch\u0024map3B == null)
        {
          // ISSUE: reference to a compiler-generated field
          XmlSchemaUtil.\u003C\u003Ef__switch\u0024map3B = new Dictionary<string, int>(3)
          {
            {
              "optional",
              0
            },
            {
              "prohibited",
              1
            },
            {
              "required",
              2
            }
          };
        }
        int num;
        // ISSUE: reference to a compiler-generated field
        if (XmlSchemaUtil.\u003C\u003Ef__switch\u0024map3B.TryGetValue(key, out num))
        {
          switch (num)
          {
            case 0:
              xmlSchemaUse = XmlSchemaUse.Optional;
              goto label_9;
            case 1:
              xmlSchemaUse = XmlSchemaUse.Prohibited;
              goto label_9;
            case 2:
              xmlSchemaUse = XmlSchemaUse.Required;
              goto label_9;
          }
        }
      }
      innerExcpetion = new Exception("only optional , prohibited or required are valid values for use");
label_9:
      return xmlSchemaUse;
    }

    public static XmlQualifiedName ReadQNameAttribute(XmlReader reader, out Exception innerEx) => XmlSchemaUtil.ToQName(reader, reader.Value, out innerEx);

    public static XmlQualifiedName ToQName(
      XmlReader reader,
      string qnamestr,
      out Exception innerEx)
    {
      innerEx = (Exception) null;
      if (!XmlSchemaUtil.IsValidQName(qnamestr))
      {
        innerEx = new Exception(qnamestr + " is an invalid QName. Either name or namespace is not a NCName");
        return XmlQualifiedName.Empty;
      }
      string[] strArray = qnamestr.Split(new char[1]{ ':' }, 2);
      string ns;
      string name;
      if (strArray.Length == 2)
      {
        ns = reader.LookupNamespace(strArray[0]);
        if (ns == null)
        {
          innerEx = new Exception("Namespace Prefix '" + strArray[0] + "could not be resolved");
          return XmlQualifiedName.Empty;
        }
        name = strArray[1];
      }
      else
      {
        ns = reader.LookupNamespace(string.Empty);
        name = strArray[0];
      }
      return new XmlQualifiedName(name, ns);
    }

    public static int ValidateAttributesResolved(
      XmlSchemaObjectTable attributesResolved,
      ValidationEventHandler h,
      XmlSchema schema,
      XmlSchemaObjectCollection attributes,
      XmlSchemaAnyAttribute anyAttribute,
      ref XmlSchemaAnyAttribute anyAttributeUse,
      XmlSchemaAttributeGroup redefined,
      bool skipEquivalent)
    {
      int num = 0;
      if (anyAttribute != null && anyAttributeUse == null)
        anyAttributeUse = anyAttribute;
      ArrayList arrayList = new ArrayList();
      foreach (XmlSchemaObject attribute in attributes)
      {
        if (attribute is XmlSchemaAttributeGroupRef attributeGroupRef)
        {
          XmlSchemaAttributeGroup schemaAttributeGroup = redefined == null || !(attributeGroupRef.RefName == redefined.QualifiedName) ? schema.FindAttributeGroup(attributeGroupRef.RefName) : redefined;
          if (schemaAttributeGroup == null)
          {
            if (!schema.missedSubComponents)
              attributeGroupRef.error(h, "Referenced attribute group " + (object) attributeGroupRef.RefName + " was not found in the corresponding schema.");
          }
          else if (schemaAttributeGroup.AttributeGroupRecursionCheck)
          {
            schemaAttributeGroup.error(h, "Attribute group recursion was found: " + (object) attributeGroupRef.RefName);
          }
          else
          {
            try
            {
              schemaAttributeGroup.AttributeGroupRecursionCheck = true;
              num += schemaAttributeGroup.Validate(h, schema);
            }
            finally
            {
              schemaAttributeGroup.AttributeGroupRecursionCheck = false;
            }
            if (schemaAttributeGroup.AnyAttributeUse != null && anyAttribute == null)
              anyAttributeUse = schemaAttributeGroup.AnyAttributeUse;
            foreach (DictionaryEntry attributeUse in schemaAttributeGroup.AttributeUses)
            {
              XmlSchemaAttribute one = (XmlSchemaAttribute) attributeUse.Value;
              if (!XmlSchemaUtil.StrictMsCompliant || one.Use != XmlSchemaUse.Prohibited)
              {
                if (one.RefName != (XmlQualifiedName) null && one.RefName != XmlQualifiedName.Empty && (!skipEquivalent || !XmlSchemaUtil.AreAttributesEqual(one, attributesResolved[one.RefName] as XmlSchemaAttribute)))
                  XmlSchemaUtil.AddToTable(attributesResolved, (XmlSchemaObject) one, one.RefName, h);
                else if (!skipEquivalent || !XmlSchemaUtil.AreAttributesEqual(one, attributesResolved[one.QualifiedName] as XmlSchemaAttribute))
                  XmlSchemaUtil.AddToTable(attributesResolved, (XmlSchemaObject) one, one.QualifiedName, h);
              }
            }
          }
        }
        else if (attribute is XmlSchemaAttribute one1)
        {
          num += one1.Validate(h, schema);
          if (arrayList.Contains((object) one1.QualifiedName))
            one1.error(h, string.Format("Duplicate attributes was found for '{0}'", (object) one1.QualifiedName));
          arrayList.Add((object) one1.QualifiedName);
          if (!XmlSchemaUtil.StrictMsCompliant || one1.Use != XmlSchemaUse.Prohibited)
          {
            if (one1.RefName != (XmlQualifiedName) null && one1.RefName != XmlQualifiedName.Empty && (!skipEquivalent || !XmlSchemaUtil.AreAttributesEqual(one1, attributesResolved[one1.RefName] as XmlSchemaAttribute)))
              XmlSchemaUtil.AddToTable(attributesResolved, (XmlSchemaObject) one1, one1.RefName, h);
            else if (!skipEquivalent || !XmlSchemaUtil.AreAttributesEqual(one1, attributesResolved[one1.QualifiedName] as XmlSchemaAttribute))
              XmlSchemaUtil.AddToTable(attributesResolved, (XmlSchemaObject) one1, one1.QualifiedName, h);
          }
        }
        else if (anyAttribute == null)
        {
          anyAttributeUse = (XmlSchemaAnyAttribute) attribute;
          anyAttribute.Validate(h, schema);
        }
      }
      return num;
    }

    internal static bool AreAttributesEqual(XmlSchemaAttribute one, XmlSchemaAttribute another) => one != null && another != null && one.AttributeType == another.AttributeType && one.Form == another.Form && one.ValidatedUse == another.ValidatedUse && one.ValidatedDefaultValue == another.ValidatedDefaultValue && one.ValidatedFixedValue == another.ValidatedFixedValue;

    public static object ReadTypedValue(
      XmlReader reader,
      object type,
      IXmlNamespaceResolver nsResolver,
      StringBuilder tmpBuilder)
    {
      if (tmpBuilder == null)
        tmpBuilder = new StringBuilder();
      XmlSchemaDatatype xmlSchemaDatatype = type as XmlSchemaDatatype;
      if (type is XmlSchemaSimpleType schemaSimpleType)
        xmlSchemaDatatype = schemaSimpleType.Datatype;
      if (xmlSchemaDatatype == null)
        return (object) null;
      switch (reader.NodeType)
      {
        case XmlNodeType.Element:
          if (reader.IsEmptyElement)
            return (object) null;
          tmpBuilder.Length = 0;
          bool flag = true;
          do
          {
            reader.Read();
            XmlNodeType nodeType = reader.NodeType;
            switch (nodeType)
            {
              case XmlNodeType.Text:
              case XmlNodeType.CDATA:
                tmpBuilder.Append(reader.Value);
                goto case XmlNodeType.Comment;
              case XmlNodeType.Comment:
                continue;
              default:
                if (nodeType != XmlNodeType.SignificantWhitespace)
                {
                  flag = false;
                  goto case XmlNodeType.Comment;
                }
                else
                  goto case XmlNodeType.Text;
            }
          }
          while (flag && !reader.EOF && reader.ReadState == ReadState.Interactive);
          return xmlSchemaDatatype.ParseValue(tmpBuilder.ToString(), reader.NameTable, nsResolver);
        case XmlNodeType.Attribute:
          return xmlSchemaDatatype.ParseValue(reader.Value, reader.NameTable, nsResolver);
        default:
          return (object) null;
      }
    }

    public static XmlSchemaObject FindAttributeDeclaration(
      string ns,
      XmlSchemaSet schemas,
      XmlSchemaComplexType cType,
      XmlQualifiedName qname)
    {
      XmlSchemaObject attributeUse = cType.AttributeUses[qname];
      if (attributeUse != null)
        return attributeUse;
      if (cType.AttributeWildcard == null)
        return (XmlSchemaObject) null;
      if (!XmlSchemaUtil.AttributeWildcardItemValid(cType.AttributeWildcard, qname, ns))
        return (XmlSchemaObject) null;
      if (cType.AttributeWildcard.ResolvedProcessContents == XmlSchemaContentProcessing.Skip)
        return (XmlSchemaObject) cType.AttributeWildcard;
      if (schemas.GlobalAttributes[qname] is XmlSchemaAttribute globalAttribute)
        return (XmlSchemaObject) globalAttribute;
      return cType.AttributeWildcard.ResolvedProcessContents == XmlSchemaContentProcessing.Lax ? (XmlSchemaObject) cType.AttributeWildcard : (XmlSchemaObject) null;
    }

    private static bool AttributeWildcardItemValid(
      XmlSchemaAnyAttribute anyAttr,
      XmlQualifiedName qname,
      string ns)
    {
      if (anyAttr.HasValueAny || anyAttr.HasValueOther && (anyAttr.TargetNamespace == string.Empty || ns != anyAttr.TargetNamespace) || anyAttr.HasValueTargetNamespace && ns == anyAttr.TargetNamespace || anyAttr.HasValueLocal && ns == string.Empty)
        return true;
      for (int index = 0; index < anyAttr.ResolvedNamespaces.Count; ++index)
      {
        if (anyAttr.ResolvedNamespaces[index] == ns)
          return true;
      }
      return false;
    }
  }
}
