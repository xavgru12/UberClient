// Decompiled with JetBrains decompiler
// Type: System.Xml.Schema.XmlSchemaCollection
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;

namespace System.Xml.Schema
{
  [Obsolete("Use XmlSchemaSet.")]
  public sealed class XmlSchemaCollection : IEnumerable, ICollection
  {
    private XmlSchemaSet schemaSet;

    public XmlSchemaCollection()
      : this((XmlNameTable) new System.Xml.NameTable())
    {
    }

    public XmlSchemaCollection(XmlNameTable nameTable)
      : this(new XmlSchemaSet(nameTable))
    {
      this.schemaSet.ValidationEventHandler += new System.Xml.Schema.ValidationEventHandler(this.OnValidationError);
    }

    internal XmlSchemaCollection(XmlSchemaSet schemaSet) => this.schemaSet = schemaSet;

    public event System.Xml.Schema.ValidationEventHandler ValidationEventHandler;

    int ICollection.Count => this.Count;

    void ICollection.CopyTo(Array array, int index)
    {
      lock (this.schemaSet)
        this.schemaSet.CopyTo(array, index);
    }

    bool ICollection.IsSynchronized => true;

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    object ICollection.SyncRoot => (object) this;

    internal XmlSchemaSet SchemaSet => this.schemaSet;

    public int Count => this.schemaSet.Count;

    public XmlNameTable NameTable => this.schemaSet.NameTable;

    public XmlSchema this[string ns]
    {
      get
      {
        ICollection collection = this.schemaSet.Schemas(ns);
        if (collection == null)
          return (XmlSchema) null;
        IEnumerator enumerator = collection.GetEnumerator();
        return enumerator.MoveNext() ? (XmlSchema) enumerator.Current : (XmlSchema) null;
      }
    }

    public XmlSchema Add(string ns, XmlReader reader) => this.Add(ns, reader, (XmlResolver) new XmlUrlResolver());

    public XmlSchema Add(string ns, XmlReader reader, XmlResolver resolver)
    {
      XmlSchema schema = XmlSchema.Read(reader, this.ValidationEventHandler);
      if (schema.TargetNamespace == null)
        schema.TargetNamespace = ns;
      else if (ns != null && schema.TargetNamespace != ns)
        throw new XmlSchemaException("The actual targetNamespace in the schema does not match the parameter.");
      return this.Add(schema);
    }

    public XmlSchema Add(string ns, string uri)
    {
      XmlReader reader = (XmlReader) new XmlTextReader(uri);
      try
      {
        return this.Add(ns, reader);
      }
      finally
      {
        reader.Close();
      }
    }

    public XmlSchema Add(XmlSchema schema) => this.Add(schema, (XmlResolver) new XmlUrlResolver());

    public XmlSchema Add(XmlSchema schema, XmlResolver resolver)
    {
      if (schema == null)
        throw new ArgumentNullException(nameof (schema));
      XmlSchemaSet xmlSchemaSet = new XmlSchemaSet(this.schemaSet.NameTable);
      xmlSchemaSet.Add(this.schemaSet);
      xmlSchemaSet.Add(schema);
      xmlSchemaSet.ValidationEventHandler += this.ValidationEventHandler;
      xmlSchemaSet.XmlResolver = resolver;
      xmlSchemaSet.Compile();
      if (!xmlSchemaSet.IsCompiled)
        return (XmlSchema) null;
      this.schemaSet = xmlSchemaSet;
      return schema;
    }

    public void Add(XmlSchemaCollection schema)
    {
      if (schema == null)
        throw new ArgumentNullException(nameof (schema));
      XmlSchemaSet xmlSchemaSet = new XmlSchemaSet(this.schemaSet.NameTable);
      xmlSchemaSet.Add(this.schemaSet);
      xmlSchemaSet.Add(schema.schemaSet);
      xmlSchemaSet.ValidationEventHandler += this.ValidationEventHandler;
      xmlSchemaSet.XmlResolver = this.schemaSet.XmlResolver;
      xmlSchemaSet.Compile();
      if (!xmlSchemaSet.IsCompiled)
        return;
      this.schemaSet = xmlSchemaSet;
    }

    public bool Contains(string ns)
    {
      lock (this.schemaSet)
        return this.schemaSet.Contains(ns);
    }

    public bool Contains(XmlSchema schema)
    {
      lock (this.schemaSet)
        return this.schemaSet.Contains(schema);
    }

    public void CopyTo(XmlSchema[] array, int index)
    {
      lock (this.schemaSet)
        this.schemaSet.CopyTo(array, index);
    }

    public XmlSchemaCollectionEnumerator GetEnumerator() => new XmlSchemaCollectionEnumerator(this.schemaSet.Schemas());

    private void OnValidationError(object o, ValidationEventArgs e)
    {
      if (this.ValidationEventHandler != null)
        this.ValidationEventHandler(o, e);
      else if (e.Severity == XmlSeverityType.Error)
        throw e.Exception;
    }
  }
}
