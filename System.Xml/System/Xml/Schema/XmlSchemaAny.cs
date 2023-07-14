// Decompiled with JetBrains decompiler
// Type: System.Xml.Schema.XmlSchemaAny
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using Mono.Xml.Schema;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
  public class XmlSchemaAny : XmlSchemaParticle
  {
    private const string xmlname = "any";
    private static XmlSchemaAny anyTypeContent;
    private string nameSpace;
    private XmlSchemaContentProcessing processing;
    private XsdWildcard wildcard;

    public XmlSchemaAny() => this.wildcard = new XsdWildcard((XmlSchemaObject) this);

    internal static XmlSchemaAny AnyTypeContent
    {
      get
      {
        if (XmlSchemaAny.anyTypeContent == null)
        {
          XmlSchemaAny.anyTypeContent = new XmlSchemaAny();
          XmlSchemaAny.anyTypeContent.MaxOccursString = "unbounded";
          XmlSchemaAny.anyTypeContent.MinOccurs = 0M;
          XmlSchemaAny.anyTypeContent.CompileOccurence((ValidationEventHandler) null, (XmlSchema) null);
          XmlSchemaAny.anyTypeContent.Namespace = "##any";
          XmlSchemaAny.anyTypeContent.wildcard.HasValueAny = true;
          XmlSchemaAny.anyTypeContent.wildcard.ResolvedNamespaces = new StringCollection();
          XsdWildcard wildcard = XmlSchemaAny.anyTypeContent.wildcard;
          XmlSchemaContentProcessing contentProcessing = XmlSchemaContentProcessing.Lax;
          XmlSchemaAny.anyTypeContent.ProcessContents = contentProcessing;
          int num = (int) contentProcessing;
          wildcard.ResolvedProcessing = (XmlSchemaContentProcessing) num;
          XmlSchemaAny.anyTypeContent.wildcard.SkipCompile = true;
        }
        return XmlSchemaAny.anyTypeContent;
      }
    }

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
      XmlSchemaUtil.CompileID(this.Id, (XmlSchemaObject) this, schema.IDCollection, h);
      this.wildcard.TargetNamespace = this.AncestorSchema.TargetNamespace;
      if (this.wildcard.TargetNamespace == null)
        this.wildcard.TargetNamespace = string.Empty;
      this.CompileOccurence(h, schema);
      this.wildcard.Compile(this.Namespace, h, schema);
      this.wildcard.ResolvedProcessing = this.processing != XmlSchemaContentProcessing.None ? this.processing : XmlSchemaContentProcessing.Strict;
      this.CompilationId = schema.CompilationId;
      return this.errorCount;
    }

    internal override XmlSchemaParticle GetOptimizedParticle(bool isTop)
    {
      if (this.OptimizedParticle != null)
        return this.OptimizedParticle;
      XmlSchemaAny xmlSchemaAny = new XmlSchemaAny();
      this.CopyInfo((XmlSchemaParticle) xmlSchemaAny);
      xmlSchemaAny.CompileOccurence((ValidationEventHandler) null, (XmlSchema) null);
      xmlSchemaAny.wildcard = this.wildcard;
      this.OptimizedParticle = (XmlSchemaParticle) xmlSchemaAny;
      xmlSchemaAny.Namespace = this.Namespace;
      xmlSchemaAny.ProcessContents = this.ProcessContents;
      xmlSchemaAny.Annotation = this.Annotation;
      xmlSchemaAny.UnhandledAttributes = this.UnhandledAttributes;
      return this.OptimizedParticle;
    }

    internal override int Validate(ValidationEventHandler h, XmlSchema schema) => this.errorCount;

    internal override bool ParticleEquals(XmlSchemaParticle other)
    {
      if (!(other is XmlSchemaAny xmlSchemaAny) || this.HasValueAny != xmlSchemaAny.HasValueAny || this.HasValueLocal != xmlSchemaAny.HasValueLocal || this.HasValueOther != xmlSchemaAny.HasValueOther || this.HasValueTargetNamespace != xmlSchemaAny.HasValueTargetNamespace || this.ResolvedProcessContents != xmlSchemaAny.ResolvedProcessContents || this.ValidatedMaxOccurs != xmlSchemaAny.ValidatedMaxOccurs || this.ValidatedMinOccurs != xmlSchemaAny.ValidatedMinOccurs || this.ResolvedNamespaces.Count != xmlSchemaAny.ResolvedNamespaces.Count)
        return false;
      for (int index = 0; index < this.ResolvedNamespaces.Count; ++index)
      {
        if (this.ResolvedNamespaces[index] != xmlSchemaAny.ResolvedNamespaces[index])
          return false;
      }
      return true;
    }

    internal bool ExamineAttributeWildcardIntersection(
      XmlSchemaAny other,
      ValidationEventHandler h,
      XmlSchema schema)
    {
      return this.wildcard.ExamineAttributeWildcardIntersection(other, h, schema);
    }

    internal override bool ValidateDerivationByRestriction(
      XmlSchemaParticle baseParticle,
      ValidationEventHandler h,
      XmlSchema schema,
      bool raiseError)
    {
      if (!(baseParticle is XmlSchemaAny xmlSchemaAny))
      {
        if (raiseError)
          this.error(h, "Invalid particle derivation by restriction was found.");
        return false;
      }
      return this.ValidateOccurenceRangeOK(baseParticle, h, schema, raiseError) && this.wildcard.ValidateWildcardSubset(xmlSchemaAny.wildcard, h, schema, raiseError);
    }

    internal override void CheckRecursion(int depth, ValidationEventHandler h, XmlSchema schema)
    {
    }

    internal override void ValidateUniqueParticleAttribution(
      XmlSchemaObjectTable qnames,
      ArrayList nsNames,
      ValidationEventHandler h,
      XmlSchema schema)
    {
      foreach (XmlSchemaAny nsName in nsNames)
      {
        if (!this.ExamineAttributeWildcardIntersection(nsName, h, schema))
          this.error(h, "Ambiguous -any- particle was found.");
      }
      nsNames.Add((object) this);
    }

    internal override void ValidateUniqueTypeAttribution(
      XmlSchemaObjectTable labels,
      ValidationEventHandler h,
      XmlSchema schema)
    {
    }

    internal bool ValidateWildcardAllowsNamespaceName(
      string ns,
      ValidationEventHandler h,
      XmlSchema schema,
      bool raiseError)
    {
      return this.wildcard.ValidateWildcardAllowsNamespaceName(ns, h, schema, raiseError);
    }

    internal static XmlSchemaAny Read(XmlSchemaReader reader, ValidationEventHandler h)
    {
      XmlSchemaAny xso = new XmlSchemaAny();
      reader.MoveToElement();
      if (reader.NamespaceURI != "http://www.w3.org/2001/XMLSchema" || reader.LocalName != "any")
      {
        XmlSchemaObject.error(h, "Should not happen :1: XmlSchemaAny.Read, name=" + reader.Name, (Exception) null);
        reader.SkipToEnd();
        return (XmlSchemaAny) null;
      }
      xso.LineNumber = reader.LineNumber;
      xso.LinePosition = reader.LinePosition;
      xso.SourceUri = reader.BaseURI;
      while (reader.MoveToNextAttribute())
      {
        if (reader.Name == "id")
          xso.Id = reader.Value;
        else if (reader.Name == "maxOccurs")
        {
          try
          {
            xso.MaxOccursString = reader.Value;
          }
          catch (Exception ex)
          {
            XmlSchemaObject.error(h, reader.Value + " is an invalid value for maxOccurs", ex);
          }
        }
        else if (reader.Name == "minOccurs")
        {
          try
          {
            xso.MinOccursString = reader.Value;
          }
          catch (Exception ex)
          {
            XmlSchemaObject.error(h, reader.Value + " is an invalid value for minOccurs", ex);
          }
        }
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
          XmlSchemaObject.error(h, reader.Name + " is not a valid attribute for any", (Exception) null);
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
          if (reader.LocalName != "any")
          {
            XmlSchemaObject.error(h, "Should not happen :2: XmlSchemaAny.Read, name=" + reader.Name, (Exception) null);
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
