// Decompiled with JetBrains decompiler
// Type: System.Xml.XmlReaderSettings
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Xml.Schema;

namespace System.Xml
{
  public sealed class XmlReaderSettings
  {
    private bool checkCharacters;
    private bool closeInput;
    private ConformanceLevel conformance;
    private bool ignoreComments;
    private bool ignoreProcessingInstructions;
    private bool ignoreWhitespace;
    private int lineNumberOffset;
    private int linePositionOffset;
    private bool prohibitDtd;
    private XmlNameTable nameTable;
    private XmlSchemaSet schemas;
    private bool schemasNeedsInitialization;
    private XmlSchemaValidationFlags validationFlags;
    private ValidationType validationType;
    private XmlResolver xmlResolver;

    public XmlReaderSettings() => this.Reset();

    public event System.Xml.Schema.ValidationEventHandler ValidationEventHandler;

    public XmlReaderSettings Clone() => (XmlReaderSettings) this.MemberwiseClone();

    public void Reset()
    {
      this.checkCharacters = true;
      this.closeInput = false;
      this.conformance = ConformanceLevel.Document;
      this.ignoreComments = false;
      this.ignoreProcessingInstructions = false;
      this.ignoreWhitespace = false;
      this.lineNumberOffset = 0;
      this.linePositionOffset = 0;
      this.prohibitDtd = true;
      this.schemas = (XmlSchemaSet) null;
      this.schemasNeedsInitialization = true;
      this.validationFlags = XmlSchemaValidationFlags.ProcessIdentityConstraints | XmlSchemaValidationFlags.AllowXmlAttributes;
      this.validationType = ValidationType.None;
      this.xmlResolver = (XmlResolver) new XmlUrlResolver();
    }

    public bool CheckCharacters
    {
      get => this.checkCharacters;
      set => this.checkCharacters = value;
    }

    public bool CloseInput
    {
      get => this.closeInput;
      set => this.closeInput = value;
    }

    public ConformanceLevel ConformanceLevel
    {
      get => this.conformance;
      set => this.conformance = value;
    }

    public bool IgnoreComments
    {
      get => this.ignoreComments;
      set => this.ignoreComments = value;
    }

    public bool IgnoreProcessingInstructions
    {
      get => this.ignoreProcessingInstructions;
      set => this.ignoreProcessingInstructions = value;
    }

    public bool IgnoreWhitespace
    {
      get => this.ignoreWhitespace;
      set => this.ignoreWhitespace = value;
    }

    public int LineNumberOffset
    {
      get => this.lineNumberOffset;
      set => this.lineNumberOffset = value;
    }

    public int LinePositionOffset
    {
      get => this.linePositionOffset;
      set => this.linePositionOffset = value;
    }

    public bool ProhibitDtd
    {
      get => this.prohibitDtd;
      set => this.prohibitDtd = value;
    }

    public XmlNameTable NameTable
    {
      get => this.nameTable;
      set => this.nameTable = value;
    }

    public XmlSchemaSet Schemas
    {
      get
      {
        if (this.schemasNeedsInitialization)
        {
          this.schemas = new XmlSchemaSet();
          this.schemasNeedsInitialization = false;
        }
        return this.schemas;
      }
      set
      {
        this.schemas = value;
        this.schemasNeedsInitialization = false;
      }
    }

    internal void OnValidationError(object o, ValidationEventArgs e)
    {
      if (this.ValidationEventHandler != null)
        this.ValidationEventHandler(o, e);
      else if (e.Severity == XmlSeverityType.Error)
        throw e.Exception;
    }

    internal void SetSchemas(XmlSchemaSet schemas) => this.schemas = schemas;

    public XmlSchemaValidationFlags ValidationFlags
    {
      get => this.validationFlags;
      set => this.validationFlags = value;
    }

    public ValidationType ValidationType
    {
      get => this.validationType;
      set => this.validationType = value;
    }

    public XmlResolver XmlResolver
    {
      internal get => this.xmlResolver;
      set => this.xmlResolver = value;
    }
  }
}
