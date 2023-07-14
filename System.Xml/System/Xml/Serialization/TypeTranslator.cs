// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.TypeTranslator
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.Globalization;
using System.Xml.Schema;

namespace System.Xml.Serialization
{
  internal class TypeTranslator
  {
    private static Hashtable nameCache = new Hashtable();
    private static Hashtable primitiveTypes;
    private static Hashtable primitiveArrayTypes = Hashtable.Synchronized(new Hashtable());
    private static Hashtable nullableTypes;

    static TypeTranslator()
    {
      TypeTranslator.nameCache = Hashtable.Synchronized(TypeTranslator.nameCache);
      TypeTranslator.nameCache.Add((object) typeof (bool), (object) new TypeData(typeof (bool), "boolean", true));
      TypeTranslator.nameCache.Add((object) typeof (short), (object) new TypeData(typeof (short), "short", true));
      TypeTranslator.nameCache.Add((object) typeof (ushort), (object) new TypeData(typeof (ushort), "unsignedShort", true));
      TypeTranslator.nameCache.Add((object) typeof (int), (object) new TypeData(typeof (int), "int", true));
      TypeTranslator.nameCache.Add((object) typeof (uint), (object) new TypeData(typeof (uint), "unsignedInt", true));
      TypeTranslator.nameCache.Add((object) typeof (long), (object) new TypeData(typeof (long), "long", true));
      TypeTranslator.nameCache.Add((object) typeof (ulong), (object) new TypeData(typeof (ulong), "unsignedLong", true));
      TypeTranslator.nameCache.Add((object) typeof (float), (object) new TypeData(typeof (float), "float", true));
      TypeTranslator.nameCache.Add((object) typeof (double), (object) new TypeData(typeof (double), "double", true));
      TypeTranslator.nameCache.Add((object) typeof (DateTime), (object) new TypeData(typeof (DateTime), "dateTime", true));
      TypeTranslator.nameCache.Add((object) typeof (Decimal), (object) new TypeData(typeof (Decimal), "decimal", true));
      TypeTranslator.nameCache.Add((object) typeof (XmlQualifiedName), (object) new TypeData(typeof (XmlQualifiedName), "QName", true));
      TypeTranslator.nameCache.Add((object) typeof (string), (object) new TypeData(typeof (string), "string", true));
      XmlSchemaPatternFacet facet = new XmlSchemaPatternFacet();
      facet.Value = "[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}";
      TypeTranslator.nameCache.Add((object) typeof (Guid), (object) new TypeData(typeof (Guid), "guid", true, (TypeData) TypeTranslator.nameCache[(object) typeof (string)], facet));
      TypeTranslator.nameCache.Add((object) typeof (byte), (object) new TypeData(typeof (byte), "unsignedByte", true));
      TypeTranslator.nameCache.Add((object) typeof (sbyte), (object) new TypeData(typeof (sbyte), "byte", true));
      TypeTranslator.nameCache.Add((object) typeof (char), (object) new TypeData(typeof (char), "char", true, (TypeData) TypeTranslator.nameCache[(object) typeof (ushort)], (XmlSchemaPatternFacet) null));
      TypeTranslator.nameCache.Add((object) typeof (object), (object) new TypeData(typeof (object), "anyType", false));
      TypeTranslator.nameCache.Add((object) typeof (byte[]), (object) new TypeData(typeof (byte[]), "base64Binary", true));
      TypeTranslator.nameCache.Add((object) typeof (XmlNode), (object) new TypeData(typeof (XmlNode), "XmlNode", false));
      TypeTranslator.nameCache.Add((object) typeof (XmlElement), (object) new TypeData(typeof (XmlElement), "XmlElement", false));
      TypeTranslator.primitiveTypes = new Hashtable();
      foreach (TypeData typeData in (IEnumerable) TypeTranslator.nameCache.Values)
        TypeTranslator.primitiveTypes.Add((object) typeData.XmlType, (object) typeData);
      TypeTranslator.primitiveTypes.Add((object) "date", (object) new TypeData(typeof (DateTime), "date", true));
      TypeTranslator.primitiveTypes.Add((object) "time", (object) new TypeData(typeof (DateTime), "time", true));
      TypeTranslator.primitiveTypes.Add((object) "timePeriod", (object) new TypeData(typeof (DateTime), "timePeriod", true));
      TypeTranslator.primitiveTypes.Add((object) "gDay", (object) new TypeData(typeof (string), "gDay", true));
      TypeTranslator.primitiveTypes.Add((object) "gMonthDay", (object) new TypeData(typeof (string), "gMonthDay", true));
      TypeTranslator.primitiveTypes.Add((object) "gYear", (object) new TypeData(typeof (string), "gYear", true));
      TypeTranslator.primitiveTypes.Add((object) "gYearMonth", (object) new TypeData(typeof (string), "gYearMonth", true));
      TypeTranslator.primitiveTypes.Add((object) "month", (object) new TypeData(typeof (DateTime), "month", true));
      TypeTranslator.primitiveTypes.Add((object) "NMTOKEN", (object) new TypeData(typeof (string), "NMTOKEN", true));
      TypeTranslator.primitiveTypes.Add((object) "NMTOKENS", (object) new TypeData(typeof (string), "NMTOKENS", true));
      TypeTranslator.primitiveTypes.Add((object) "Name", (object) new TypeData(typeof (string), "Name", true));
      TypeTranslator.primitiveTypes.Add((object) "NCName", (object) new TypeData(typeof (string), "NCName", true));
      TypeTranslator.primitiveTypes.Add((object) "language", (object) new TypeData(typeof (string), "language", true));
      TypeTranslator.primitiveTypes.Add((object) "integer", (object) new TypeData(typeof (string), "integer", true));
      TypeTranslator.primitiveTypes.Add((object) "positiveInteger", (object) new TypeData(typeof (string), "positiveInteger", true));
      TypeTranslator.primitiveTypes.Add((object) "nonPositiveInteger", (object) new TypeData(typeof (string), "nonPositiveInteger", true));
      TypeTranslator.primitiveTypes.Add((object) "negativeInteger", (object) new TypeData(typeof (string), "negativeInteger", true));
      TypeTranslator.primitiveTypes.Add((object) "nonNegativeInteger", (object) new TypeData(typeof (string), "nonNegativeInteger", true));
      TypeTranslator.primitiveTypes.Add((object) "ENTITIES", (object) new TypeData(typeof (string), "ENTITIES", true));
      TypeTranslator.primitiveTypes.Add((object) "ENTITY", (object) new TypeData(typeof (string), "ENTITY", true));
      TypeTranslator.primitiveTypes.Add((object) "hexBinary", (object) new TypeData(typeof (byte[]), "hexBinary", true));
      TypeTranslator.primitiveTypes.Add((object) "ID", (object) new TypeData(typeof (string), "ID", true));
      TypeTranslator.primitiveTypes.Add((object) "IDREF", (object) new TypeData(typeof (string), "IDREF", true));
      TypeTranslator.primitiveTypes.Add((object) "IDREFS", (object) new TypeData(typeof (string), "IDREFS", true));
      TypeTranslator.primitiveTypes.Add((object) "NOTATION", (object) new TypeData(typeof (string), "NOTATION", true));
      TypeTranslator.primitiveTypes.Add((object) "token", (object) new TypeData(typeof (string), "token", true));
      TypeTranslator.primitiveTypes.Add((object) "normalizedString", (object) new TypeData(typeof (string), "normalizedString", true));
      TypeTranslator.primitiveTypes.Add((object) "anyURI", (object) new TypeData(typeof (string), "anyURI", true));
      TypeTranslator.primitiveTypes.Add((object) "base64", (object) new TypeData(typeof (byte[]), "base64", true));
      TypeTranslator.primitiveTypes.Add((object) "duration", (object) new TypeData(typeof (string), "duration", true));
      TypeTranslator.nullableTypes = Hashtable.Synchronized(new Hashtable());
      foreach (DictionaryEntry primitiveType in TypeTranslator.primitiveTypes)
      {
        TypeData typeData = (TypeData) primitiveType.Value;
        TypeTranslator.nullableTypes.Add(primitiveType.Key, (object) new TypeData(typeData.Type, typeData.XmlType, true)
        {
          IsNullable = true
        });
      }
    }

    public static TypeData GetTypeData(Type type) => TypeTranslator.GetTypeData(type, (string) null);

    public static TypeData GetTypeData(Type runtimeType, string xmlDataType)
    {
      Type type = runtimeType;
      bool flag = false;
      if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof (Nullable<>))
      {
        flag = true;
        type = type.GetGenericArguments()[0];
        TypeData typeData1 = TypeTranslator.GetTypeData(type);
        if (typeData1 != null)
        {
          TypeData typeData2 = (TypeData) TypeTranslator.nullableTypes[(object) typeData1.XmlType];
          if (typeData2 == null)
          {
            typeData2 = new TypeData(type, typeData1.XmlType, false);
            typeData2.IsNullable = true;
            TypeTranslator.nullableTypes[(object) typeData1.XmlType] = (object) typeData2;
          }
          return typeData2;
        }
      }
      if (xmlDataType != null && xmlDataType.Length != 0)
      {
        TypeData primitiveTypeData = TypeTranslator.GetPrimitiveTypeData(xmlDataType);
        if (!type.IsArray || type == primitiveTypeData.Type)
          return primitiveTypeData;
        TypeData primitiveArrayType = (TypeData) TypeTranslator.primitiveArrayTypes[(object) xmlDataType];
        if (primitiveArrayType != null)
          return primitiveArrayType;
        if (primitiveTypeData.Type == type.GetElementType())
        {
          TypeData typeData = new TypeData(type, TypeTranslator.GetArrayName(primitiveTypeData.XmlType), false);
          TypeTranslator.primitiveArrayTypes[(object) xmlDataType] = (object) typeData;
          return typeData;
        }
        throw new InvalidOperationException("Cannot convert values of type '" + (object) type.GetElementType() + "' to '" + xmlDataType + "'");
      }
      if (TypeTranslator.nameCache[(object) runtimeType] is TypeData typeData3)
        return typeData3;
      string elementName;
      if (type.IsArray)
        elementName = TypeTranslator.GetArrayName(TypeTranslator.GetTypeData(type.GetElementType()).XmlType);
      else if (type.IsGenericType && !type.IsGenericTypeDefinition)
      {
        elementName = XmlConvert.EncodeLocalName(type.Name.Substring(0, type.Name.IndexOf('`'))) + "Of";
        foreach (Type genericArgument in type.GetGenericArguments())
          elementName += genericArgument.IsArray || genericArgument.IsGenericType ? TypeTranslator.GetTypeData(genericArgument).XmlType : CodeIdentifier.MakePascal(XmlConvert.EncodeLocalName(genericArgument.Name));
      }
      else
        elementName = XmlConvert.EncodeLocalName(type.Name);
      TypeData typeData4 = new TypeData(type, elementName, false);
      if (flag)
        typeData4.IsNullable = true;
      TypeTranslator.nameCache[(object) runtimeType] = (object) typeData4;
      return typeData4;
    }

    public static bool IsPrimitive(Type type) => TypeTranslator.GetTypeData(type).SchemaType == SchemaTypes.Primitive;

    public static TypeData GetPrimitiveTypeData(string typeName) => TypeTranslator.GetPrimitiveTypeData(typeName, false);

    public static TypeData GetPrimitiveTypeData(string typeName, bool nullable)
    {
      TypeData primitiveType = (TypeData) TypeTranslator.primitiveTypes[(object) typeName];
      if (primitiveType != null && !primitiveType.Type.IsValueType)
        return primitiveType;
      return (TypeData) (!nullable || TypeTranslator.nullableTypes == null ? TypeTranslator.primitiveTypes : TypeTranslator.nullableTypes)[(object) typeName] ?? throw new NotSupportedException("Data type '" + typeName + "' not supported");
    }

    public static TypeData FindPrimitiveTypeData(string typeName) => (TypeData) TypeTranslator.primitiveTypes[(object) typeName];

    public static TypeData GetDefaultPrimitiveTypeData(TypeData primType)
    {
      if (primType.SchemaType == SchemaTypes.Primitive)
      {
        TypeData typeData = TypeTranslator.GetTypeData(primType.Type, (string) null);
        if (typeData != primType)
          return typeData;
      }
      return primType;
    }

    public static bool IsDefaultPrimitiveTpeData(TypeData primType) => TypeTranslator.GetDefaultPrimitiveTypeData(primType) == primType;

    public static TypeData CreateCustomType(
      string typeName,
      string fullTypeName,
      string xmlType,
      SchemaTypes schemaType,
      TypeData listItemTypeData)
    {
      return new TypeData(typeName, fullTypeName, xmlType, schemaType, listItemTypeData);
    }

    public static string GetArrayName(string elemName) => "ArrayOf" + (object) char.ToUpper(elemName[0], CultureInfo.InvariantCulture) + elemName.Substring(1);

    public static string GetArrayName(string elemName, int dimensions)
    {
      string arrayName = TypeTranslator.GetArrayName(elemName);
      for (; dimensions > 1; --dimensions)
        arrayName = "ArrayOf" + arrayName;
      return arrayName;
    }

    public static void ParseArrayType(
      string arrayType,
      out string type,
      out string ns,
      out string dimensions)
    {
      int length = arrayType.LastIndexOf(":");
      ns = length != -1 ? arrayType.Substring(0, length) : string.Empty;
      int startIndex = arrayType.IndexOf("[", length + 1);
      if (startIndex == -1)
        throw new InvalidOperationException("Cannot parse WSDL array type: " + arrayType);
      type = arrayType.Substring(length + 1, startIndex - length - 1);
      dimensions = arrayType.Substring(startIndex);
    }
  }
}
