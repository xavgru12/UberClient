// Decompiled with JetBrains decompiler
// Type: System.Xml.Schema.XmlSchemaAnyAttribute
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using Mono.Xml.Schema;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
  public class XmlSchemaAnyAttribute : XmlSchemaAnnotated
  {
    private const string xmlname = "anyAttribute";
    private string nameSpace;
    private XmlSchemaContentProcessing processing;
    private XsdWildcard wildcard;

    public XmlSchemaAnyAttribute() => this.wildcard = new XsdWildcard((XmlSchemaObject) this);

    [XmlAttribute("namespace")]
    public string Namespace
    {
      get => this.nameSpace;
      set => this.nameSpace = value;
    }

    [XmlAttribute("processContents")]
    [DefaultValue(XmlSchemaContentProcessing.None)]
    public XmlSchemaContentProcessing ProcessContents
    {
      get => this.processing;
      set => this.processing = value;
    }

    internal bool HasValueAny => this.wildcard.HasValueAny;

    internal bool HasValueLocal => this.wildcard.HasValueLocal;

    internal bool HasValueOther => this.wildcard.HasValueOther;

    internal bool HasValueTargetNamespace => this.wildcard.HasValueTargetNamespace;

    internal StringCollection ResolvedNamespaces => this.wildcard.ResolvedNamespaces;

    internal XmlSchemaContentProcessing ResolvedProcessContents => this.wildcard.ResolvedProcessing;

    internal string TargetNamespace => this.wildcard.TargetNamespace;

    internal override int Compile(ValidationEventHandler h, XmlSchema schema)
    {
      if (this.CompilationId == schema.CompilationId)
        return 0;
      this.errorCount = 0;
      this.wildcard.TargetNamespace = this.AncestorSchema.TargetNamespace;
      if (this.wildcard.TargetNamespace == null)
        this.wildcard.TargetNamespace = string.Empty;
      XmlSchemaUtil.CompileID(this.Id, (XmlSchemaObject) this, schema.IDCollection, h);
      this.wildcard.Compile(this.Namespace, h, schema);
      this.wildcard.ResolvedProcessing = this.processing != XmlSchemaContentProcessing.None ? this.processing : XmlSchemaContentProcessing.Strict;
      this.CompilationId = schema.CompilationId;
      return this.errorCount;
    }

    internal override int Validate(ValidationEventHandler h, XmlSchema schema) => this.errorCount;

    internal void ValidateWildcardSubset(
      XmlSchemaAnyAttribute other,
      ValidationEventHandler h,
      XmlSchema schema)
    {
      this.wildcard.ValidateWildcardSubset(other.wildcard, h, schema);
    }

    internal bool ValidateWildcardAllowsNamespaceName(string ns, XmlSchema schema) => this.wildcard.ValidateWildcardAllowsNamespaceName(ns, (ValidationEventHandler) null, schema, false);

    internal static XmlSchemaAnyAttribute Read(XmlSchemaReader reader, ValidationEventHandler h)
    {
      XmlSchemaAnyAttribute xso = new XmlSchemaAnyAttribute();
      reader.MoveToElement();
      if (reader.NamespaceURI != "http://www.w3.org/2001/XMLSchema" || reader.LocalName != "anyAttribute")
      {
        XmlSchemaObject.error(h, "Should not happen :1: XmlSchemaAnyAttribute.Read, name=" + reader.Name, (Exception) null);
        reader.SkipToEnd();
        return (XmlSchemaAnyAttribute) null;
      }
      xso.LineNumber = reader.LineNumber;
      xso.LinePosition = reader.LinePosition;
      xso.SourceUri = reader.BaseURI;
      while (reader.MoveToNextAttribute())
      {
        if (reader.Name == "id")
          xso.Id = reader.Value;
        else if (reader.Name == "namespace")
          xso.nameSpace = reader.Value;
        else if (reader.Name == "processContents")
        {
          Exception innerExcpetion;
          xso.processing = XmlSchemaUtil.ReadProcessingAttribute((XmlReader) reader, out innerExcpetion);
          if (innerExcpetion != null)
            XmlSchemaObject.error(h, reader.Value + " is not a valid value for processContents", innerExcpetion);
        }
        else if (reader.NamespaceURI == string.Empty && reader.Name != "xmlns" || reader.NamespaceURI == "http://www.w3.org/2001/XMLSchema")
          XmlSchemaObject.error(h, reader.Name + " is not a valid attribute for anyAttribute", (Exception) null);
        else
          XmlSchemaUtil.ReadUnhandledAttribute((XmlReader) reader, (XmlSchemaObject) xso);
      }
      reader.MoveToElement();
      if (reader.IsEmptyElement)
        return xso;
      int num = 1;
      while (reader.ReadNextElement())
      {
        if (reader.NodeType == XmlNodeType.EndElement)
        {
          if (reader.LocalName != "anyAttribute")
          {
            XmlSchemaObject.error(h, "Should not happen :2: XmlSchemaAnyAttribute.Read, name=" + reader.Name, (Exception) null);
            break;
          }
          break;
        }
        if (num <= 1 && reader.LocalName == "annotation")
        {
          num = 2;
          XmlSchemaAnnotation schemaAnnotation = XmlSchemaAnnotation.Read(reader, h);
          if (schemaAnnotation != null)
            xso.Annotation = schemaAnnotation;
        }
        else
          reader.RaiseInvalidElementError();
      }
      return xso;
    }
  }
}
