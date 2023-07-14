// Decompiled with JetBrains decompiler
// Type: System.Xml.Schema.XmlSchemaSet
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;

namespace System.Xml.Schema
{
  public class XmlSchemaSet
  {
    private XmlNameTable nameTable;
    private XmlResolver xmlResolver = (XmlResolver) new XmlUrlResolver();
    private ArrayList schemas;
    private XmlSchemaObjectTable attributes;
    private XmlSchemaObjectTable elements;
    private XmlSchemaObjectTable types;
    private Hashtable idCollection;
    private XmlSchemaObjectTable namedIdentities;
    private XmlSchemaCompilationSettings settings = new XmlSchemaCompilationSettings();
    private bool isCompiled;
    internal Guid CompilationId;

    public XmlSchemaSet()
      : this((XmlNameTable) new System.Xml.NameTable())
    {
    }

    public XmlSchemaSet(XmlNameTable nameTable)
    {
      this.nameTable = nameTable != null ? nameTable : throw new ArgumentNullException(nameof (nameTable));
      this.schemas = new ArrayList();
      this.CompilationId = Guid.NewGuid();
    }

    public event System.Xml.Schema.ValidationEventHandler ValidationEventHandler;

    public int Count => this.schemas.Count;

    public XmlSchemaObjectTable GlobalAttributes
    {
      get
      {
        if (this.attributes == null)
          this.attributes = new XmlSchemaObjectTable();
        return this.attributes;
      }
    }

    public XmlSchemaObjectTable GlobalElements
    {
      get
      {
        if (this.elements == null)
          this.elements = new XmlSchemaObjectTable();
        return this.elements;
      }
    }

    public XmlSchemaObjectTable GlobalTypes
    {
      get
      {
        if (this.types == null)
          this.types = new XmlSchemaObjectTable();
        return this.types;
      }
    }

    public bool IsCompiled => this.isCompiled;

    public XmlNameTable NameTable => this.nameTable;

    public XmlSchemaCompilationSettings CompilationSettings
    {
      get => this.settings;
      set => this.settings = value;
    }

    public XmlResolver XmlResolver
    {
      set => this.xmlResolver = value;
      internal get => this.xmlResolver;
    }

    internal Hashtable IDCollection
    {
      get
      {
        if (this.idCollection == null)
          this.idCollection = new Hashtable();
        return this.idCollection;
      }
    }

    internal XmlSchemaObjectTable NamedIdentities
    {
      get
      {
        if (this.namedIdentities == null)
          this.namedIdentities = new XmlSchemaObjectTable();
        return this.namedIdentities;
      }
    }

    public XmlSchema Add(string targetNamespace, string url)
    {
      XmlTextReader reader = (XmlTextReader) null;
      try
      {
        reader = new XmlTextReader(url, this.nameTable);
        return this.Add(targetNamespace, (XmlReader) reader);
      }
      finally
      {
        reader?.Close();
      }
    }

    public XmlSchema Add(string targetNamespace, XmlReader reader)
    {
      XmlSchema schema = XmlSchema.Read(reader, this.ValidationEventHandler);
      if (schema.TargetNamespace == null)
        schema.TargetNamespace = targetNamespace;
      else if (targetNamespace != null && schema.TargetNamespace != targetNamespace)
        throw new XmlSchemaException("The actual targetNamespace in the schema does not match the parameter.");
      this.Add(schema);
      return schema;
    }

    [MonoTODO]
    public void Add(XmlSchemaSet schemaSet)
    {
      ArrayList arrayList = new ArrayList();
      foreach (XmlSchema schema in schemaSet.schemas)
      {
        if (!this.schemas.Contains((object) schema))
          arrayList.Add((object) schema);
      }
      foreach (XmlSchema schema in arrayList)
        this.Add(schema);
    }

    public XmlSchema Add(XmlSchema schema)
    {
      this.schemas.Add((object) schema);
      this.ResetCompile();
      return schema;
    }

    public void Compile()
    {
      this.ClearGlobalComponents();
      ArrayList arrayList = new ArrayList();
      arrayList.AddRange((ICollection) this.schemas);
      this.IDCollection.Clear();
      this.NamedIdentities.Clear();
      Hashtable handledUris = new Hashtable();
      foreach (XmlSchema xmlSchema in arrayList)
      {
        if (!xmlSchema.IsCompiled)
          xmlSchema.CompileSubset(this.ValidationEventHandler, this, this.xmlResolver, handledUris);
      }
      foreach (XmlSchema xmlSchema in arrayList)
      {
        foreach (XmlSchemaElement xmlSchemaElement in (IEnumerable) xmlSchema.Elements.Values)
          xmlSchemaElement.FillSubstitutionElementInfo();
      }
      foreach (XmlSchema xmlSchema in arrayList)
        xmlSchema.Validate(this.ValidationEventHandler);
      foreach (XmlSchema schema in arrayList)
        this.AddGlobalComponents(schema);
      this.isCompiled = true;
    }

    private void ClearGlobalComponents()
    {
      this.GlobalElements.Clear();
      this.GlobalAttributes.Clear();
      this.GlobalTypes.Clear();
    }

    private void AddGlobalComponents(XmlSchema schema)
    {
      foreach (XmlSchemaElement xmlSchemaElement in (IEnumerable) schema.Elements.Values)
        this.GlobalElements.Add(xmlSchemaElement.QualifiedName, (XmlSchemaObject) xmlSchemaElement);
      foreach (XmlSchemaAttribute xmlSchemaAttribute in (IEnumerable) schema.Attributes.Values)
        this.GlobalAttributes.Add(xmlSchemaAttribute.QualifiedName, (XmlSchemaObject) xmlSchemaAttribute);
      foreach (XmlSchemaType xmlSchemaType in (IEnumerable) schema.SchemaTypes.Values)
        this.GlobalTypes.Add(xmlSchemaType.QualifiedName, (XmlSchemaObject) xmlSchemaType);
    }

    public bool Contains(string targetNamespace)
    {
      targetNamespace = this.GetSafeNs(targetNamespace);
      foreach (XmlSchema schema in this.schemas)
      {
        if (this.GetSafeNs(schema.TargetNamespace) == targetNamespace)
          return true;
      }
      return false;
    }

    public bool Contains(XmlSchema targetNamespace)
    {
      foreach (XmlSchema schema in this.schemas)
      {
        if (schema == targetNamespace)
          return true;
      }
      return false;
    }

    public void CopyTo(XmlSchema[] array, int index) => this.schemas.CopyTo((Array) array, index);

    internal void CopyTo(Array array, int index) => this.schemas.CopyTo(array, index);

    private string GetSafeNs(string ns) => ns == null ? string.Empty : ns;

    [MonoTODO]
    public XmlSchema Remove(XmlSchema schema)
    {
      if (schema == null)
        throw new ArgumentNullException(nameof (schema));
      ArrayList arrayList = new ArrayList();
      arrayList.AddRange((ICollection) this.schemas);
      if (!arrayList.Contains((object) schema))
        return (XmlSchema) null;
      if (!schema.IsCompiled)
        schema.CompileSubset(this.ValidationEventHandler, this, this.xmlResolver);
      this.schemas.Remove((object) schema);
      this.ResetCompile();
      return schema;
    }

    private void ResetCompile()
    {
      this.isCompiled = false;
      this.ClearGlobalComponents();
    }

    public bool RemoveRecursive(XmlSchema schema)
    {
      if (schema == null)
        throw new ArgumentNullException(nameof (schema));
      ArrayList arrayList = new ArrayList();
      arrayList.AddRange((ICollection) this.schemas);
      if (!arrayList.Contains((object) schema))
        return false;
      arrayList.Remove((object) schema);
      this.schemas.Remove((object) schema);
      if (!this.IsCompiled)
        return true;
      this.ClearGlobalComponents();
      foreach (XmlSchema xmlSchema in arrayList)
      {
        if (xmlSchema.IsCompiled)
          this.AddGlobalComponents(schema);
      }
      return true;
    }

    public XmlSchema Reprocess(XmlSchema schema)
    {
      if (schema == null)
        throw new ArgumentNullException(nameof (schema));
      ArrayList arrayList = new ArrayList();
      arrayList.AddRange((ICollection) this.schemas);
      if (!arrayList.Contains((object) schema))
        throw new ArgumentException("Target schema is not contained in the schema set.");
      this.ClearGlobalComponents();
      foreach (XmlSchema xmlSchema in arrayList)
      {
        if (schema == xmlSchema)
          schema.CompileSubset(this.ValidationEventHandler, this, this.xmlResolver);
        if (xmlSchema.IsCompiled)
          this.AddGlobalComponents(schema);
      }
      return schema.IsCompiled ? schema : (XmlSchema) null;
    }

    public ICollection Schemas() => (ICollection) this.schemas;

    public ICollection Schemas(string targetNamespace)
    {
      targetNamespace = this.GetSafeNs(targetNamespace);
      ArrayList arrayList = new ArrayList();
      foreach (XmlSchema schema in this.schemas)
      {
        if (this.GetSafeNs(schema.TargetNamespace) == targetNamespace)
          arrayList.Add((object) schema);
      }
      return (ICollection) arrayList;
    }

    internal bool MissedSubComponents(string targetNamespace)
    {
      foreach (XmlSchema schema in (IEnumerable) this.Schemas(targetNamespace))
      {
        if (schema.missedSubComponents)
          return true;
      }
      return false;
    }
  }
}
