// Decompiled with JetBrains decompiler
// Type: System.Xml.Schema.XmlSchemaAll
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
  public class XmlSchemaAll : XmlSchemaGroupBase
  {
    private const string xmlname = "all";
    private XmlSchema schema;
    private XmlSchemaObjectCollection items;
    private bool emptiable;

    public XmlSchemaAll() => this.items = new XmlSchemaObjectCollection();

    [XmlElement("element", typeof (XmlSchemaElement))]
    public override XmlSchemaObjectCollection Items => this.items;

    internal bool Emptiable => this.emptiable;

    internal override void SetParent(XmlSchemaObject parent)
    {
      base.SetParent(parent);
      foreach (XmlSchemaObject xmlSchemaObject in this.Items)
        xmlSchemaObject.SetParent((XmlSchemaObject) this);
    }

    internal override int Compile(ValidationEventHandler h, XmlSchema schema)
    {
      if (this.CompilationId == schema.CompilationId)
        return 0;
      this.schema = schema;
      if (this.MaxOccurs != 1M)
        this.error(h, "maxOccurs must be 1");
      if (this.MinOccurs != 1M && this.MinOccurs != 0M)
        this.error(h, "minOccurs must be 0 or 1");
      XmlSchemaUtil.CompileID(this.Id, (XmlSchemaObject) this, schema.IDCollection, h);
      this.CompileOccurence(h, schema);
      foreach (XmlSchemaObject xmlSchemaObject in this.Items)
      {
        if (xmlSchemaObject is XmlSchemaElement xmlSchemaElement)
        {
          if (xmlSchemaElement.ValidatedMaxOccurs != 1M && xmlSchemaElement.ValidatedMaxOccurs != 0M)
            xmlSchemaElement.error(h, "The {max occurs} of all the elements of 'all' must be 0 or 1. ");
          this.errorCount += xmlSchemaElement.Compile(h, schema);
        }
        else
          this.error(h, "XmlSchemaAll can only contain Items of type Element");
      }
      this.CompilationId = schema.CompilationId;
      return this.errorCount;
    }

    internal override XmlSchemaParticle GetOptimizedParticle(bool isTop)
    {
      if (this.OptimizedParticle != null)
        return this.OptimizedParticle;
      if (this.Items.Count == 0 || this.ValidatedMaxOccurs == 0M)
      {
        this.OptimizedParticle = XmlSchemaParticle.Empty;
        return this.OptimizedParticle;
      }
      if (this.Items.Count == 1 && this.ValidatedMinOccurs == 1M && this.ValidatedMaxOccurs == 1M)
      {
        XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
        this.CopyInfo((XmlSchemaParticle) xmlSchemaSequence);
        XmlSchemaParticle optimizedParticle = ((XmlSchemaParticle) this.Items[0]).GetOptimizedParticle(false);
        if (optimizedParticle == XmlSchemaParticle.Empty)
        {
          this.OptimizedParticle = optimizedParticle;
        }
        else
        {
          xmlSchemaSequence.Items.Add((XmlSchemaObject) optimizedParticle);
          xmlSchemaSequence.CompiledItems.Add((XmlSchemaObject) optimizedParticle);
          xmlSchemaSequence.Compile((ValidationEventHandler) null, this.schema);
          this.OptimizedParticle = (XmlSchemaParticle) xmlSchemaSequence;
        }
        return this.OptimizedParticle;
      }
      XmlSchemaAll gb = new XmlSchemaAll();
      this.CopyInfo((XmlSchemaParticle) gb);
      this.CopyOptimizedItems((XmlSchemaGroupBase) gb);
      this.OptimizedParticle = (XmlSchemaParticle) gb;
      gb.ComputeEmptiable();
      return this.OptimizedParticle;
    }

    internal override int Validate(ValidationEventHandler h, XmlSchema schema)
    {
      if (this.IsValidated(schema.CompilationId))
        return this.errorCount;
      if (!this.parentIsGroupDefinition && this.ValidatedMaxOccurs != 1M)
        this.error(h, "-all- group is limited to be content of a model group, or that of a complex type with maxOccurs to be 1.");
      this.CompiledItems.Clear();
      foreach (XmlSchemaParticle xmlSchemaParticle in this.Items)
      {
        this.errorCount += xmlSchemaParticle.Validate(h, schema);
        if (xmlSchemaParticle.ValidatedMaxOccurs != 0M && xmlSchemaParticle.ValidatedMaxOccurs != 1M)
          this.error(h, "MaxOccurs of a particle inside -all- compositor must be either 0 or 1.");
        this.CompiledItems.Add((XmlSchemaObject) xmlSchemaParticle);
      }
      this.ComputeEmptiable();
      this.ValidationId = schema.ValidationId;
      return this.errorCount;
    }

    private void ComputeEmptiable()
    {
      this.emptiable = true;
      for (int index = 0; index < this.Items.Count; ++index)
      {
        if (((XmlSchemaParticle) this.Items[index]).ValidatedMinOccurs > 0M)
        {
          this.emptiable = false;
          break;
        }
      }
    }

    internal override bool ValidateDerivationByRestriction(
      XmlSchemaParticle baseParticle,
      ValidationEventHandler h,
      XmlSchema schema,
      bool raiseError)
    {
      XmlSchemaAny any = baseParticle as XmlSchemaAny;
      XmlSchemaAll xmlSchemaAll = baseParticle as XmlSchemaAll;
      if (any != null)
        return this.ValidateNSRecurseCheckCardinality(any, h, schema, raiseError);
      if (xmlSchemaAll != null)
        return this.ValidateOccurenceRangeOK((XmlSchemaParticle) xmlSchemaAll, h, schema, raiseError) && this.ValidateRecurse((XmlSchemaGroupBase) xmlSchemaAll, h, schema, raiseError);
      if (raiseError)
        this.error(h, "Invalid -all- particle derivation was found.");
      return false;
    }

    internal override Decimal GetMinEffectiveTotalRange() => this.GetMinEffectiveTotalRangeAllAndSequence();

    internal override void ValidateUniqueParticleAttribution(
      XmlSchemaObjectTable qnames,
      ArrayList nsNames,
      ValidationEventHandler h,
      XmlSchema schema)
    {
      foreach (XmlSchemaElement xmlSchemaElement in this.Items)
        xmlSchemaElement.ValidateUniqueParticleAttribution(qnames, nsNames, h, schema);
    }

    internal override void ValidateUniqueTypeAttribution(
      XmlSchemaObjectTable labels,
      ValidationEventHandler h,
      XmlSchema schema)
    {
      foreach (XmlSchemaElement xmlSchemaElement in this.Items)
        xmlSchemaElement.ValidateUniqueTypeAttribution(labels, h, schema);
    }

    internal static XmlSchemaAll Read(XmlSchemaReader reader, ValidationEventHandler h)
    {
      XmlSchemaAll xso = new XmlSchemaAll();
      reader.MoveToElement();
      if (reader.NamespaceURI != "http://www.w3.org/2001/XMLSchema" || reader.LocalName != "all")
      {
        XmlSchemaObject.error(h, "Should not happen :1: XmlSchemaAll.Read, name=" + reader.Name, (Exception) null);
        reader.SkipToEnd();
        return (XmlSchemaAll) null;
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
        else if (reader.NamespaceURI == string.Empty && reader.Name != "xmlns" || reader.NamespaceURI == "http://www.w3.org/2001/XMLSchema")
          XmlSchemaObject.error(h, reader.Name + " is not a valid attribute for all", (Exception) null);
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
          if (reader.LocalName != "all")
          {
            XmlSchemaObject.error(h, "Should not happen :2: XmlSchemaAll.Read, name=" + reader.Name, (Exception) null);
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
        else if (num <= 2 && reader.LocalName == "element")
        {
          num = 2;
          XmlSchemaElement xmlSchemaElement = XmlSchemaElement.Read(reader, h);
          if (xmlSchemaElement != null)
            xso.items.Add((XmlSchemaObject) xmlSchemaElement);
        }
        else
          reader.RaiseInvalidElementError();
      }
      return xso;
    }
  }
}
