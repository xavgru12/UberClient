// Decompiled with JetBrains decompiler
// Type: System.Xml.Schema.XmlSchemaObject
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
  public abstract class XmlSchemaObject
  {
    private int lineNumber;
    private int linePosition;
    private string sourceUri;
    private XmlSerializerNamespaces namespaces;
    internal ArrayList unhandledAttributeList;
    internal bool isCompiled;
    internal int errorCount;
    internal Guid CompilationId;
    internal Guid ValidationId;
    internal bool isRedefineChild;
    internal bool isRedefinedComponent;
    internal XmlSchemaObject redefinedObject;
    private XmlSchemaObject parent;

    protected XmlSchemaObject()
    {
      this.namespaces = new XmlSerializerNamespaces();
      this.unhandledAttributeList = (ArrayList) null;
      this.CompilationId = Guid.Empty;
    }

    [XmlIgnore]
    public int LineNumber
    {
      get => this.lineNumber;
      set => this.lineNumber = value;
    }

    [XmlIgnore]
    public int LinePosition
    {
      get => this.linePosition;
      set => this.linePosition = value;
    }

    [XmlIgnore]
    public string SourceUri
    {
      get => this.sourceUri;
      set => this.sourceUri = value;
    }

    [XmlIgnore]
    public XmlSchemaObject Parent
    {
      get => this.parent;
      set => this.parent = value;
    }

    internal XmlSchema AncestorSchema
    {
      get
      {
        for (XmlSchemaObject parent = this.Parent; parent != null; parent = parent.Parent)
        {
          if (parent is XmlSchema)
            return (XmlSchema) parent;
        }
        throw new Exception(string.Format("INTERNAL ERROR: Parent object is not set properly : {0} ({1},{2})", (object) this.SourceUri, (object) this.LineNumber, (object) this.LinePosition));
      }
    }

    internal virtual void SetParent(XmlSchemaObject parent) => this.Parent = parent;

    [XmlNamespaceDeclarations]
    public XmlSerializerNamespaces Namespaces
    {
      get => this.namespaces;
      set => this.namespaces = value;
    }

    internal void error(ValidationEventHandler handle, string message)
    {
      ++this.errorCount;
      XmlSchemaObject.error(handle, message, (Exception) null, this, (object) null);
    }

    internal void warn(ValidationEventHandler handle, string message) => XmlSchemaObject.warn(handle, message, (Exception) null, this, (object) null);

    internal static void error(
      ValidationEventHandler handle,
      string message,
      Exception innerException)
    {
      XmlSchemaObject.error(handle, message, innerException, (XmlSchemaObject) null, (object) null);
    }

    internal static void warn(
      ValidationEventHandler handle,
      string message,
      Exception innerException)
    {
      XmlSchemaObject.warn(handle, message, innerException, (XmlSchemaObject) null, (object) null);
    }

    internal static void error(
      ValidationEventHandler handle,
      string message,
      Exception innerException,
      XmlSchemaObject xsobj,
      object sender)
    {
      ValidationHandler.RaiseValidationEvent(handle, innerException, message, xsobj, sender, (string) null, XmlSeverityType.Error);
    }

    internal static void warn(
      ValidationEventHandler handle,
      string message,
      Exception innerException,
      XmlSchemaObject xsobj,
      object sender)
    {
      ValidationHandler.RaiseValidationEvent(handle, innerException, message, xsobj, sender, (string) null, XmlSeverityType.Warning);
    }

    internal virtual int Compile(ValidationEventHandler h, XmlSchema schema) => 0;

    internal virtual int Validate(ValidationEventHandler h, XmlSchema schema) => 0;

    internal bool IsValidated(Guid validationId) => this.ValidationId == validationId;

    internal virtual void CopyInfo(XmlSchemaParticle obj)
    {
      obj.LineNumber = this.LineNumber;
      obj.LinePosition = this.LinePosition;
      obj.SourceUri = this.SourceUri;
      obj.errorCount = this.errorCount;
    }
  }
}
