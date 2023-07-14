// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.XmlSchemas
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Schema;

namespace System.Xml.Serialization
{
  public class XmlSchemas : CollectionBase, IEnumerable<XmlSchema>, IEnumerable
  {
    private static string msdataNS = "urn:schemas-microsoft-com:xml-msdata";
    private Hashtable table = new Hashtable();

    IEnumerator<XmlSchema> IEnumerable<XmlSchema>.GetEnumerator() => (IEnumerator<XmlSchema>) new XmlSchemaEnumerator(this);

    public XmlSchema this[int index]
    {
      get => index >= 0 && index <= this.Count ? (XmlSchema) this.List[index] : throw new ArgumentOutOfRangeException();
      set => this.List[index] = (object) value;
    }

    public XmlSchema this[string ns] => (XmlSchema) this.table[ns == null ? (object) string.Empty : (object) ns];

    [MonoTODO]
    public bool IsCompiled => throw new NotImplementedException();

    [MonoTODO]
    public void Compile(ValidationEventHandler handler, bool fullCompile)
    {
      foreach (XmlSchema xmlSchema in (CollectionBase) this)
      {
        if (fullCompile || !xmlSchema.IsCompiled)
          xmlSchema.Compile(handler);
      }
    }

    public int Add(XmlSchema schema)
    {
      this.Insert(this.Count, schema);
      return this.Count - 1;
    }

    public void Add(XmlSchemas schemas)
    {
      foreach (XmlSchema schema in (CollectionBase) schemas)
        this.Add(schema);
    }

    [MonoNotSupported("")]
    public int Add(XmlSchema schema, Uri baseUri) => throw new NotImplementedException();

    [MonoNotSupported("")]
    public void AddReference(XmlSchema schema) => throw new NotImplementedException();

    public bool Contains(XmlSchema schema) => this.List.Contains((object) schema);

    [MonoNotSupported("")]
    public bool Contains(string targetNamespace) => throw new NotImplementedException();

    public void CopyTo(XmlSchema[] array, int index) => this.List.CopyTo((Array) array, index);

    public object Find(XmlQualifiedName name, Type type)
    {
      if (!(this.table[(object) name.Namespace] is XmlSchema schema1))
      {
        foreach (XmlSchema schema in (CollectionBase) this)
        {
          object obj = this.Find(schema, name, type);
          if (obj != null)
            return obj;
        }
        return (object) null;
      }
      object obj1 = this.Find(schema1, name, type);
      if (obj1 == null)
      {
        foreach (XmlSchema schema2 in (CollectionBase) this)
        {
          object obj2 = this.Find(schema2, name, type);
          if (obj2 != null)
            return obj2;
        }
      }
      return obj1;
    }

    private object Find(XmlSchema schema, XmlQualifiedName name, Type type)
    {
      if (!schema.IsCompiled)
        schema.Compile((ValidationEventHandler) null);
      XmlSchemaObjectTable schemaObjectTable = (XmlSchemaObjectTable) null;
      if (type == typeof (XmlSchemaSimpleType) || type == typeof (XmlSchemaComplexType))
        schemaObjectTable = schema.SchemaTypes;
      else if (type == typeof (XmlSchemaAttribute))
        schemaObjectTable = schema.Attributes;
      else if (type == typeof (XmlSchemaAttributeGroup))
        schemaObjectTable = schema.AttributeGroups;
      else if (type == typeof (XmlSchemaElement))
        schemaObjectTable = schema.Elements;
      else if (type == typeof (XmlSchemaGroup))
        schemaObjectTable = schema.Groups;
      else if (type == typeof (XmlSchemaNotation))
        schemaObjectTable = schema.Notations;
      object obj = schemaObjectTable == null ? (object) (XmlSchemaObject) null : (object) schemaObjectTable[name];
      return obj != null && obj.GetType() != type ? (object) null : obj;
    }

    [MonoNotSupported("")]
    public IList GetSchemas(string ns) => throw new NotImplementedException();

    public int IndexOf(XmlSchema schema) => this.List.IndexOf((object) schema);

    public void Insert(int index, XmlSchema schema) => this.List.Insert(index, (object) schema);

    public static bool IsDataSet(XmlSchema schema)
    {
      XmlSchemaElement xmlSchemaElement = schema.Items.Count != 1 ? (XmlSchemaElement) null : schema.Items[0] as XmlSchemaElement;
      if (xmlSchemaElement != null && xmlSchemaElement.UnhandledAttributes != null && xmlSchemaElement.UnhandledAttributes.Length > 0)
      {
        for (int index = 0; index < xmlSchemaElement.UnhandledAttributes.Length; ++index)
        {
          XmlAttribute unhandledAttribute = xmlSchemaElement.UnhandledAttributes[index];
          if (unhandledAttribute.NamespaceURI == XmlSchemas.msdataNS && unhandledAttribute.LocalName == nameof (IsDataSet))
            return unhandledAttribute.Value.ToLower(CultureInfo.InvariantCulture) == "true";
        }
      }
      return false;
    }

    protected override void OnClear() => this.table.Clear();

    protected override void OnInsert(int index, object value) => this.table[(object) (((XmlSchema) value).TargetNamespace ?? string.Empty)] = value;

    protected override void OnRemove(int index, object value) => this.table.Remove(value);

    protected override void OnSet(int index, object oldValue, object newValue) => this.table[(object) (((XmlSchema) oldValue).TargetNamespace ?? string.Empty)] = newValue;

    public void Remove(XmlSchema schema) => this.List.Remove((object) schema);
  }
}
