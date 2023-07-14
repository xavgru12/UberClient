// Decompiled with JetBrains decompiler
// Type: Mono.Xml.DTDObjectModel
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System;
using System.Collections;
using System.IO;
using System.Xml;

namespace Mono.Xml
{
  internal class DTDObjectModel
  {
    public const int AllowedExternalEntitiesMax = 256;
    private DTDAutomataFactory factory;
    private DTDElementAutomata rootAutomata;
    private DTDEmptyAutomata emptyAutomata;
    private DTDAnyAutomata anyAutomata;
    private DTDInvalidAutomata invalidAutomata;
    private DTDElementDeclarationCollection elementDecls;
    private DTDAttListDeclarationCollection attListDecls;
    private DTDParameterEntityDeclarationCollection peDecls;
    private DTDEntityDeclarationCollection entityDecls;
    private DTDNotationDeclarationCollection notationDecls;
    private ArrayList validationErrors;
    private XmlResolver resolver;
    private XmlNameTable nameTable;
    private Hashtable externalResources;
    private string baseURI;
    private string name;
    private string publicId;
    private string systemId;
    private string intSubset;
    private bool intSubsetHasPERef;
    private bool isStandalone;
    private int lineNumber;
    private int linePosition;

    public DTDObjectModel(XmlNameTable nameTable)
    {
      this.nameTable = nameTable;
      this.elementDecls = new DTDElementDeclarationCollection(this);
      this.attListDecls = new DTDAttListDeclarationCollection(this);
      this.entityDecls = new DTDEntityDeclarationCollection(this);
      this.peDecls = new DTDParameterEntityDeclarationCollection(this);
      this.notationDecls = new DTDNotationDeclarationCollection(this);
      this.factory = new DTDAutomataFactory(this);
      this.validationErrors = new ArrayList();
      this.externalResources = new Hashtable();
    }

    public string BaseURI
    {
      get => this.baseURI;
      set => this.baseURI = value;
    }

    public bool IsStandalone
    {
      get => this.isStandalone;
      set => this.isStandalone = value;
    }

    public string Name
    {
      get => this.name;
      set => this.name = value;
    }

    public XmlNameTable NameTable => this.nameTable;

    public string PublicId
    {
      get => this.publicId;
      set => this.publicId = value;
    }

    public string SystemId
    {
      get => this.systemId;
      set => this.systemId = value;
    }

    public string InternalSubset
    {
      get => this.intSubset;
      set => this.intSubset = value;
    }

    public bool InternalSubsetHasPEReference
    {
      get => this.intSubsetHasPERef;
      set => this.intSubsetHasPERef = value;
    }

    public int LineNumber
    {
      get => this.lineNumber;
      set => this.lineNumber = value;
    }

    public int LinePosition
    {
      get => this.linePosition;
      set => this.linePosition = value;
    }

    public string ResolveEntity(string name)
    {
      DTDEntityDeclaration entityDecl = this.EntityDecls[name];
      if (entityDecl != null)
        return entityDecl.EntityValue;
      this.AddError(new XmlException(string.Format("Required entity was not found: {0}", (object) name), (Exception) null, this.LineNumber, this.LinePosition));
      return " ";
    }

    internal XmlResolver Resolver => this.resolver;

    public XmlResolver XmlResolver
    {
      set => this.resolver = value;
    }

    internal Hashtable ExternalResources => this.externalResources;

    public DTDAutomataFactory Factory => this.factory;

    public DTDElementDeclaration RootElement => this.ElementDecls[this.Name];

    public DTDElementDeclarationCollection ElementDecls => this.elementDecls;

    public DTDAttListDeclarationCollection AttListDecls => this.attListDecls;

    public DTDEntityDeclarationCollection EntityDecls => this.entityDecls;

    public DTDParameterEntityDeclarationCollection PEDecls => this.peDecls;

    public DTDNotationDeclarationCollection NotationDecls => this.notationDecls;

    public DTDAutomata RootAutomata
    {
      get
      {
        if (this.rootAutomata == null)
          this.rootAutomata = new DTDElementAutomata(this, this.Name);
        return (DTDAutomata) this.rootAutomata;
      }
    }

    public DTDEmptyAutomata Empty
    {
      get
      {
        if (this.emptyAutomata == null)
          this.emptyAutomata = new DTDEmptyAutomata(this);
        return this.emptyAutomata;
      }
    }

    public DTDAnyAutomata Any
    {
      get
      {
        if (this.anyAutomata == null)
          this.anyAutomata = new DTDAnyAutomata(this);
        return this.anyAutomata;
      }
    }

    public DTDInvalidAutomata Invalid
    {
      get
      {
        if (this.invalidAutomata == null)
          this.invalidAutomata = new DTDInvalidAutomata(this);
        return this.invalidAutomata;
      }
    }

    public XmlException[] Errors => this.validationErrors.ToArray(typeof (XmlException)) as XmlException[];

    public void AddError(XmlException ex) => this.validationErrors.Add((object) ex);

    internal string GenerateEntityAttributeText(string entityName) => this.EntityDecls[entityName]?.EntityValue;

    internal Mono.Xml2.XmlTextReader GenerateEntityContentReader(
      string entityName,
      XmlParserContext context)
    {
      DTDEntityDeclaration entityDecl = this.EntityDecls[entityName];
      if (entityDecl == null)
        return (Mono.Xml2.XmlTextReader) null;
      return entityDecl.SystemId != null ? new Mono.Xml2.XmlTextReader(this.resolver.GetEntity(this.resolver.ResolveUri(!(entityDecl.BaseURI == string.Empty) ? new Uri(entityDecl.BaseURI) : (Uri) null, entityDecl.SystemId), (string) null, typeof (Stream)) as Stream, XmlNodeType.Element, context) : new Mono.Xml2.XmlTextReader(entityDecl.EntityValue, XmlNodeType.Element, context);
    }
  }
}
