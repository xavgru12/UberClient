// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.XmlTypeMapping
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;

namespace System.Xml.Serialization
{
  public class XmlTypeMapping : XmlMapping
  {
    private string xmlType;
    private string xmlTypeNamespace;
    private TypeData type;
    private XmlTypeMapping baseMap;
    private bool multiReferenceType;
    private bool isSimpleType;
    private string documentation;
    private bool includeInSchema;
    private bool isNullable = true;
    private ArrayList _derivedTypes = new ArrayList();

    internal XmlTypeMapping(
      string elementName,
      string ns,
      TypeData typeData,
      string xmlType,
      string xmlTypeNamespace)
      : base(elementName, ns)
    {
      this.type = typeData;
      this.xmlType = xmlType;
      this.xmlTypeNamespace = xmlTypeNamespace;
    }

    public string TypeFullName => this.type.FullTypeName;

    public string TypeName => this.type.TypeName;

    public string XsdTypeName => this.XmlType;

    public string XsdTypeNamespace => this.XmlTypeNamespace;

    internal TypeData TypeData => this.type;

    internal string XmlType
    {
      get => this.xmlType;
      set => this.xmlType = value;
    }

    internal string XmlTypeNamespace
    {
      get => this.xmlTypeNamespace;
      set => this.xmlTypeNamespace = value;
    }

    internal ArrayList DerivedTypes
    {
      get => this._derivedTypes;
      set => this._derivedTypes = value;
    }

    internal bool MultiReferenceType
    {
      get => this.multiReferenceType;
      set => this.multiReferenceType = value;
    }

    internal XmlTypeMapping BaseMap
    {
      get => this.baseMap;
      set => this.baseMap = value;
    }

    internal bool IsSimpleType
    {
      get => this.isSimpleType;
      set => this.isSimpleType = value;
    }

    internal string Documentation
    {
      set => this.documentation = value;
      get => this.documentation;
    }

    internal bool IncludeInSchema
    {
      get => this.includeInSchema;
      set => this.includeInSchema = value;
    }

    internal bool IsNullable
    {
      get => this.isNullable;
      set => this.isNullable = value;
    }

    internal XmlTypeMapping GetRealTypeMap(Type objectType)
    {
      if (this.TypeData.SchemaType == SchemaTypes.Enum || this.TypeData.Type == objectType)
        return this;
      for (int index = 0; index < this._derivedTypes.Count; ++index)
      {
        XmlTypeMapping derivedType = (XmlTypeMapping) this._derivedTypes[index];
        if (derivedType.TypeData.Type == objectType)
          return derivedType;
      }
      return (XmlTypeMapping) null;
    }

    internal XmlTypeMapping GetRealElementMap(string name, string ens)
    {
      if (this.xmlType == name && this.xmlTypeNamespace == ens)
        return this;
      foreach (XmlTypeMapping derivedType in this._derivedTypes)
      {
        if (derivedType.xmlType == name && derivedType.xmlTypeNamespace == ens)
          return derivedType;
      }
      return (XmlTypeMapping) null;
    }

    internal void UpdateRoot(XmlQualifiedName qname)
    {
      if (!(qname != (XmlQualifiedName) null))
        return;
      this._elementName = qname.Name;
      this._namespace = qname.Namespace;
    }
  }
}
