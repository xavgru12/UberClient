// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.TypeData
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Xml.Schema;

namespace System.Xml.Serialization
{
  internal class TypeData
  {
    private Type type;
    private string elementName;
    private SchemaTypes sType;
    private Type listItemType;
    private string typeName;
    private string fullTypeName;
    private string csharpName;
    private string csharpFullName;
    private TypeData listItemTypeData;
    private TypeData listTypeData;
    private TypeData mappedType;
    private XmlSchemaPatternFacet facet;
    private bool hasPublicConstructor = true;
    private bool nullableOverride;
    private static Hashtable keywordsTable;
    private static string[] keywords = new string[80]
    {
      "abstract",
      "event",
      "new",
      "struct",
      "as",
      "explicit",
      "null",
      "switch",
      "base",
      "extern",
      "this",
      "false",
      "operator",
      "throw",
      "break",
      "finally",
      "out",
      "true",
      "fixed",
      "override",
      "try",
      "case",
      "params",
      "typeof",
      "catch",
      "for",
      "private",
      "foreach",
      "protected",
      "checked",
      "goto",
      "public",
      "unchecked",
      "class",
      "if",
      "readonly",
      "unsafe",
      "const",
      "implicit",
      "ref",
      "continue",
      "in",
      "return",
      "using",
      "virtual",
      "default",
      "interface",
      "sealed",
      "volatile",
      "delegate",
      "internal",
      "do",
      "is",
      "sizeof",
      "while",
      "lock",
      "stackalloc",
      "else",
      "static",
      "enum",
      "namespace",
      "object",
      "bool",
      "byte",
      "float",
      "uint",
      "char",
      "ulong",
      "ushort",
      "decimal",
      "int",
      "sbyte",
      "short",
      "double",
      "long",
      "string",
      "void",
      "partial",
      "yield",
      "where"
    };

    public TypeData(Type type, string elementName, bool isPrimitive)
      : this(type, elementName, isPrimitive, (TypeData) null, (XmlSchemaPatternFacet) null)
    {
    }

    public TypeData(
      Type type,
      string elementName,
      bool isPrimitive,
      TypeData mappedType,
      XmlSchemaPatternFacet facet)
    {
      if (type.IsGenericTypeDefinition)
        throw new InvalidOperationException("Generic type definition cannot be used in serialization. Only specific generic types can be used.");
      this.mappedType = mappedType;
      this.facet = facet;
      this.type = type;
      this.typeName = type.Name;
      this.fullTypeName = type.FullName.Replace('+', '.');
      this.sType = !isPrimitive ? (!type.IsEnum ? (!typeof (IXmlSerializable).IsAssignableFrom(type) ? (!typeof (XmlNode).IsAssignableFrom(type) ? (type.IsArray || typeof (IEnumerable).IsAssignableFrom(type) ? SchemaTypes.Array : SchemaTypes.Class) : SchemaTypes.XmlNode) : SchemaTypes.XmlSerializable) : SchemaTypes.Enum) : SchemaTypes.Primitive;
      this.elementName = !this.IsListType ? elementName : TypeTranslator.GetArrayName(this.ListItemTypeData.XmlType);
      if (this.sType != SchemaTypes.Array && this.sType != SchemaTypes.Class)
        return;
      this.hasPublicConstructor = !type.IsInterface && (type.IsArray || type.GetConstructor(Type.EmptyTypes) != null || type.IsAbstract || type.IsValueType);
    }

    internal TypeData(
      string typeName,
      string fullTypeName,
      string xmlType,
      SchemaTypes schemaType,
      TypeData listItemTypeData)
    {
      this.elementName = xmlType;
      this.typeName = typeName;
      this.fullTypeName = fullTypeName.Replace('+', '.');
      this.listItemTypeData = listItemTypeData;
      this.sType = schemaType;
      this.hasPublicConstructor = true;
    }

    public string TypeName => this.typeName;

    public string XmlType => this.elementName;

    public Type Type => this.type;

    public string FullTypeName => this.fullTypeName;

    public string CSharpName
    {
      get
      {
        if (this.csharpName == null)
          this.csharpName = this.Type != null ? TypeData.ToCSharpName(this.Type, false) : this.TypeName;
        return this.csharpName;
      }
    }

    public string CSharpFullName
    {
      get
      {
        if (this.csharpFullName == null)
          this.csharpFullName = this.Type != null ? TypeData.ToCSharpName(this.Type, true) : this.TypeName;
        return this.csharpFullName;
      }
    }

    public static string ToCSharpName(Type type, bool full)
    {
      if (type.IsArray)
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(TypeData.ToCSharpName(type.GetElementType(), full));
        stringBuilder.Append('[');
        int arrayRank = type.GetArrayRank();
        for (int index = 1; index < arrayRank; ++index)
          stringBuilder.Append(',');
        stringBuilder.Append(']');
        return stringBuilder.ToString();
      }
      if (type.IsGenericType && !type.IsGenericTypeDefinition)
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(TypeData.ToCSharpName(type.GetGenericTypeDefinition(), full));
        stringBuilder.Append('<');
        foreach (Type genericArgument in type.GetGenericArguments())
          stringBuilder.Append(TypeData.ToCSharpName(genericArgument, full)).Append(',');
        --stringBuilder.Length;
        stringBuilder.Append('>');
        return stringBuilder.ToString();
      }
      string str = (!full ? type.Name : type.FullName).Replace('+', '.');
      int length = str.IndexOf('`');
      string name = length <= 0 ? str : str.Substring(0, length);
      return TypeData.IsKeyword(name) ? "@" + name : name;
    }

    private static bool IsKeyword(string name)
    {
      if (TypeData.keywordsTable == null)
      {
        Hashtable hashtable = new Hashtable();
        foreach (string keyword in TypeData.keywords)
          hashtable[(object) keyword] = (object) keyword;
        TypeData.keywordsTable = hashtable;
      }
      return TypeData.keywordsTable.Contains((object) name);
    }

    public SchemaTypes SchemaType => this.sType;

    public bool IsListType => this.SchemaType == SchemaTypes.Array;

    public bool IsComplexType => this.SchemaType == SchemaTypes.Class || this.SchemaType == SchemaTypes.Array || this.SchemaType == SchemaTypes.Enum || this.SchemaType == SchemaTypes.XmlNode || this.SchemaType == SchemaTypes.XmlSerializable || !this.IsXsdType;

    public bool IsValueType
    {
      get
      {
        if (this.type != null)
          return this.type.IsValueType;
        return this.sType == SchemaTypes.Primitive || this.sType == SchemaTypes.Enum;
      }
    }

    public bool NullableOverride => this.nullableOverride;

    public bool IsNullable
    {
      get
      {
        if (this.nullableOverride || !this.IsValueType)
          return true;
        return this.type != null && this.type.IsGenericType && this.type.GetGenericTypeDefinition() == typeof (Nullable<>);
      }
      set => this.nullableOverride = value;
    }

    public TypeData ListItemTypeData
    {
      get
      {
        if (this.listItemTypeData == null && this.type != null)
          this.listItemTypeData = TypeTranslator.GetTypeData(this.ListItemType);
        return this.listItemTypeData;
      }
    }

    public Type ListItemType
    {
      get
      {
        if (this.type == null)
          throw new InvalidOperationException("Property ListItemType is not supported for custom types");
        if (this.listItemType != null)
          return this.listItemType;
        Type type = (Type) null;
        if (this.SchemaType != SchemaTypes.Array)
          throw new InvalidOperationException(this.Type.FullName + " is not a collection");
        if (this.type.IsArray)
          this.listItemType = this.type.GetElementType();
        else if (typeof (ICollection).IsAssignableFrom(this.type) || (type = this.GetGenericListItemType(this.type)) != null)
        {
          if (typeof (IDictionary).IsAssignableFrom(this.type))
            throw new NotSupportedException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "The type {0} is not supported because it implements IDictionary.", (object) this.type.FullName));
          if (type != null)
            this.listItemType = type;
          else
            this.listItemType = (TypeData.GetIndexerProperty(this.type) ?? throw new InvalidOperationException("You must implement a default accessor on " + this.type.FullName + " because it inherits from ICollection")).PropertyType;
          if (this.type.GetMethod("Add", new Type[1]
          {
            this.listItemType
          }) == null)
            throw TypeData.CreateMissingAddMethodException(this.type, "ICollection", this.listItemType);
        }
        else
        {
          PropertyInfo property = (this.type.GetMethod("GetEnumerator", Type.EmptyTypes) ?? this.type.GetMethod("System.Collections.IEnumerable.GetEnumerator", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Type.EmptyTypes, (ParameterModifier[]) null)).ReturnType.GetProperty("Current");
          this.listItemType = property != null ? property.PropertyType : typeof (object);
          if (this.type.GetMethod("Add", new Type[1]
          {
            this.listItemType
          }) == null)
            throw TypeData.CreateMissingAddMethodException(this.type, "IEnumerable", this.listItemType);
        }
        return this.listItemType;
      }
    }

    public TypeData ListTypeData
    {
      get
      {
        if (this.listTypeData != null)
          return this.listTypeData;
        this.listTypeData = new TypeData(this.TypeName + "[]", this.FullTypeName + "[]", TypeTranslator.GetArrayName(this.XmlType), SchemaTypes.Array, this);
        return this.listTypeData;
      }
    }

    public bool IsXsdType => this.mappedType == null;

    public TypeData MappedType => this.mappedType != null ? this.mappedType : this;

    public XmlSchemaPatternFacet XmlSchemaPatternFacet => this.facet;

    public bool HasPublicConstructor => this.hasPublicConstructor;

    public static PropertyInfo GetIndexerProperty(Type collectionType)
    {
      foreach (PropertyInfo property in collectionType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
      {
        ParameterInfo[] indexParameters = property.GetIndexParameters();
        if (indexParameters != null && indexParameters.Length == 1 && indexParameters[0].ParameterType == typeof (int))
          return property;
      }
      return (PropertyInfo) null;
    }

    private static InvalidOperationException CreateMissingAddMethodException(
      Type type,
      string inheritFrom,
      Type argumentType)
    {
      return new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "To be XML serializable, types which inherit from {0} must have an implementation of Add({1}) at all levels of their inheritance hierarchy. {2} does not implement Add({1}).", (object) inheritFrom, (object) argumentType.FullName, (object) type.FullName));
    }

    private Type GetGenericListItemType(Type type)
    {
      if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof (ICollection<>))
        return type.GetGenericArguments()[0];
      foreach (Type type1 in type.GetInterfaces())
      {
        Type genericListItemType;
        if ((genericListItemType = this.GetGenericListItemType(type1)) != null)
          return genericListItemType;
      }
      return (Type) null;
    }
  }
}
