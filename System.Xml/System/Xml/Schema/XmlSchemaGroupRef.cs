// Decompiled with JetBrains decompiler
// Type: System.Xml.Schema.XmlSchemaGroupRef
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
  public class XmlSchemaGroupRef : XmlSchemaParticle
  {
    private const string xmlname = "group";
    private XmlSchema schema;
    private XmlQualifiedName refName;
    private XmlSchemaGroup referencedGroup;
    private bool busy;

    public XmlSchemaGroupRef() => this.refName = XmlQualifiedName.Empty;

    [XmlAttribute("ref")]
    public XmlQualifiedName RefName
    {
      get => this.refName;
      set => this.refName = value;
    }

    [XmlIgnore]
    public XmlSchemaGroupBase Particle => this.TargetGroup != null ? this.TargetGroup.Particle : (XmlSchemaGroupBase) null;

    internal XmlSchemaGroup TargetGroup => this.referencedGroup != null && this.referencedGroup.IsCircularDefinition ? (XmlSchemaGroup) null : this.referencedGroup;

    internal override int Compile(ValidationEventHandler h, XmlSchema schema)
    {
      if (this.CompilationId == schema.CompilationId)
        return 0;
      this.schema = schema;
      XmlSchemaUtil.CompileID(this.Id, (XmlSchemaObject) this, schema.IDCollection, h);
      this.CompileOccurence(h, schema);
      if (this.refName == (XmlQualifiedName) null || this.refName.IsEmpty)
        this.error(h, "ref must be present");
      else if (!XmlSchemaUtil.CheckQName(this.RefName))
        this.error(h, "RefName must be a valid XmlQualifiedName");
      this.CompilationId = schema.CompilationId;
      return this.errorCount;
    }

    internal override int Validate(ValidationEventHandler h, XmlSchema schema)
    {
      if (this.IsValidated(schema.ValidationId))
        return this.errorCount;
      this.referencedGroup = schema.Groups[this.RefName] as XmlSchemaGroup;
      if (this.referencedGroup == null)
      {
        if (!schema.IsNamespaceAbsent(this.RefName.Namespace))
          this.error(h, "Referenced group " + (object) this.RefName + " was not found in the corresponding schema.");
      }
      else if (this.referencedGroup.Particle is XmlSchemaAll && this.ValidatedMaxOccurs != 1M)
        this.error(h, "Group reference to -all- particle must have schema component {maxOccurs}=1.");
      if (this.TargetGroup != null)
        this.TargetGroup.Validate(h, schema);
      this.ValidationId = schema.ValidationId;
      return this.errorCount;
    }

    internal override XmlSchemaParticle GetOptimizedParticle(bool isTop)
    {
      if (this.busy)
        return XmlSchemaParticle.Empty;
      if (this.OptimizedParticle != null)
        return this.OptimizedParticle;
      this.busy = true;
      XmlSchemaGroup xmlSchemaGroup = this.referencedGroup == null ? this.schema.Groups[this.RefName] as XmlSchemaGroup : this.referencedGroup;
      if (xmlSchemaGroup != null && xmlSchemaGroup.Particle != null)
      {
        this.OptimizedParticle = (XmlSchemaParticle) xmlSchemaGroup.Particle;
        this.OptimizedParticle = this.OptimizedParticle.GetOptimizedParticle(isTop);
        if (this.OptimizedParticle != XmlSchemaParticle.Empty && (this.ValidatedMinOccurs != 1M || this.ValidatedMaxOccurs != 1M))
        {
          this.OptimizedParticle = this.OptimizedParticle.GetShallowClone();
          this.OptimizedParticle.OptimizedParticle = (XmlSchemaParticle) null;
          this.OptimizedParticle.MinOccurs = this.MinOccurs;
          this.OptimizedParticle.MaxOccurs = this.MaxOccurs;
          this.OptimizedParticle.CompileOccurence((ValidationEventHandler) null, (XmlSchema) null);
        }
      }
      else
        this.OptimizedParticle = XmlSchemaParticle.Empty;
      this.busy = false;
      return this.OptimizedParticle;
    }

    internal override bool ParticleEquals(XmlSchemaParticle other) => this.GetOptimizedParticle(true).ParticleEquals(other);

    internal override bool ValidateDerivationByRestriction(
      XmlSchemaParticle baseParticle,
      ValidationEventHandler h,
      XmlSchema schema,
      bool raiseError)
    {
      return this.TargetGroup != null && this.TargetGroup.Particle.ValidateDerivationByRestriction(baseParticle, h, schema, raiseError);
    }

    internal override void CheckRecursion(int depth, ValidationEventHandler h, XmlSchema schema)
    {
      if (this.TargetGroup == null)
        return;
      if (this.recursionDepth == -1)
      {
        this.recursionDepth = depth;
        this.TargetGroup.Particle.CheckRecursion(depth, h, schema);
        this.recursionDepth = -2;
      }
      else if (depth == this.recursionDepth)
        throw new XmlSchemaException("Circular group reference was found.", (XmlSchemaObject) this, (Exception) null);
    }

    internal override void ValidateUniqueParticleAttribution(
      XmlSchemaObjectTable qnames,
      ArrayList nsNames,
      ValidationEventHandler h,
      XmlSchema schema)
    {
      if (this.TargetGroup == null)
        return;
      this.TargetGroup.Particle.ValidateUniqueParticleAttribution(qnames, nsNames, h, schema);
    }

    internal override void ValidateUniqueTypeAttribution(
      XmlSchemaObjectTable labels,
      ValidationEventHandler h,
      XmlSchema schema)
    {
      if (this.TargetGroup == null)
        return;
      this.TargetGroup.Particle.ValidateUniqueTypeAttribution(labels, h, schema);
    }

    internal static XmlSchemaGroupRef Read(XmlSchemaReader reader, ValidationEventHandler h)
    {
      XmlSchemaGroupRef xso = new XmlSchemaGroupRef();
      reader.MoveToElement();
      if (reader.NamespaceURI != "http://www.w3.org/2001/XMLSchema" || reader.LocalName != "group")
      {
        XmlSchemaObject.error(h, "Should not happen :1: XmlSchemaGroup.Read, name=" + reader.Name, (Exception) null);
        reader.Skip();
        return (XmlSchemaGroupRef) null;
      }
      xso.LineNumber = reader.LineNumber;
      xso.LinePosition = reader.LinePosition;
      xso.SourceUri = reader.BaseURI;
      while (reader.MoveToNextAttribute())
      {
        if (reader.Name == "id")
          xso.Id = reader.Value;
        else if (reader.Name == "ref")
        {
          Exception innerEx;
          xso.refName = XmlSchemaUtil.ReadQNameAttribute((XmlReader) reader, out innerEx);
          if (innerEx != null)
            XmlSchemaObject.error(h, reader.Value + " is not a valid value for ref attribute", innerEx);
        }
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
        else if (reader.NamespaceURI == string.Empty && reader.Name != "xmlns" || reader.NamespaceURI == "http://www.w3.org/2001/XMLSchema")
          XmlSchemaObject.error(h, reader.Name + " is not a valid attribute for group", (Exception) null);
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
          if (reader.LocalName != "group")
          {
            XmlSchemaObject.error(h, "Should not happen :2: XmlSchemaGroupRef.Read, name=" + reader.Name, (Exception) null);
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
