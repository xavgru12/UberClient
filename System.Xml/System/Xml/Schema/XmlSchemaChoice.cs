// Decompiled with JetBrains decompiler
// Type: System.Xml.Schema.XmlSchemaChoice
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
  public class XmlSchemaChoice : XmlSchemaGroupBase
  {
    private const string xmlname = "choice";
    private XmlSchemaObjectCollection items;
    private Decimal minEffectiveTotalRange = -1M;

    public XmlSchemaChoice() => this.items = new XmlSchemaObjectCollection();

    [XmlElement("sequence", typeof (XmlSchemaSequence))]
    [XmlElement("any", typeof (XmlSchemaAny))]
    [XmlElement("choice", typeof (XmlSchemaChoice))]
    [XmlElement("group", typeof (XmlSchemaGroupRef))]
    [XmlElement("element", typeof (XmlSchemaElement))]
    public override XmlSchemaObjectCollection Items => this.items;

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
      XmlSchemaUtil.CompileID(this.Id, (XmlSchemaObject) this, schema.IDCollection, h);
      this.CompileOccurence(h, schema);
      if (this.Items.Count == 0)
        this.warn(h, "Empty choice is unsatisfiable if minOccurs not equals to 0");
      foreach (XmlSchemaObject xmlSchemaObject in this.Items)
      {
        switch (xmlSchemaObject)
        {
          case XmlSchemaElement _:
          case XmlSchemaGroupRef _:
          case XmlSchemaChoice _:
          case XmlSchemaSequence _:
          case XmlSchemaAny _:
            this.errorCount += xmlSchemaObject.Compile(h, schema);
            continue;
          default:
            this.error(h, "Invalid schema object was specified in the particles of the choice model group.");
            continue;
        }
      }
      this.CompilationId = schema.CompilationId;
      return this.errorCount;
    }

    internal override XmlSchemaParticle GetOptimizedParticle(bool isTop)
    {
      if (this.OptimizedParticle != null)
        return this.OptimizedParticle;
      if (this.Items.Count == 0 || this.ValidatedMaxOccurs == 0M)
        this.OptimizedParticle = XmlSchemaParticle.Empty;
      else if (!isTop && this.Items.Count == 1 && this.ValidatedMinOccurs == 1M && this.ValidatedMaxOccurs == 1M)
      {
        this.OptimizedParticle = ((XmlSchemaParticle) this.Items[0]).GetOptimizedParticle(false);
      }
      else
      {
        XmlSchemaChoice xmlSchemaChoice1 = new XmlSchemaChoice();
        this.CopyInfo((XmlSchemaParticle) xmlSchemaChoice1);
        for (int index1 = 0; index1 < this.Items.Count; ++index1)
        {
          XmlSchemaParticle optimizedParticle = (this.Items[index1] as XmlSchemaParticle).GetOptimizedParticle(false);
          if (optimizedParticle != XmlSchemaParticle.Empty)
          {
            if (optimizedParticle is XmlSchemaChoice && optimizedParticle.ValidatedMinOccurs == 1M && optimizedParticle.ValidatedMaxOccurs == 1M)
            {
              XmlSchemaChoice xmlSchemaChoice2 = optimizedParticle as XmlSchemaChoice;
              for (int index2 = 0; index2 < xmlSchemaChoice2.Items.Count; ++index2)
              {
                xmlSchemaChoice1.Items.Add(xmlSchemaChoice2.Items[index2]);
                xmlSchemaChoice1.CompiledItems.Add(xmlSchemaChoice2.Items[index2]);
              }
            }
            else
            {
              xmlSchemaChoice1.Items.Add((XmlSchemaObject) optimizedParticle);
              xmlSchemaChoice1.CompiledItems.Add((XmlSchemaObject) optimizedParticle);
            }
          }
        }
        if (xmlSchemaChoice1.Items.Count == 0)
          this.OptimizedParticle = XmlSchemaParticle.Empty;
        else
          this.OptimizedParticle = (XmlSchemaParticle) xmlSchemaChoice1;
      }
      return this.OptimizedParticle;
    }

    internal override int Validate(ValidationEventHandler h, XmlSchema schema)
    {
      if (this.IsValidated(schema.CompilationId))
        return this.errorCount;
      this.CompiledItems.Clear();
      foreach (XmlSchemaParticle xmlSchemaParticle in this.Items)
      {
        this.errorCount += xmlSchemaParticle.Validate(h, schema);
        this.CompiledItems.Add((XmlSchemaObject) xmlSchemaParticle);
      }
      this.ValidationId = schema.ValidationId;
      return this.errorCount;
    }

    internal override bool ValidateDerivationByRestriction(
      XmlSchemaParticle baseParticle,
      ValidationEventHandler h,
      XmlSchema schema,
      bool raiseError)
    {
      if (baseParticle is XmlSchemaAny any)
        return this.ValidateNSRecurseCheckCardinality(any, h, schema, raiseError);
      if (baseParticle is XmlSchemaChoice xmlSchemaChoice)
      {
        if (!this.ValidateOccurenceRangeOK((XmlSchemaParticle) xmlSchemaChoice, h, schema, raiseError))
          return false;
        return xmlSchemaChoice.ValidatedMinOccurs == 0M && xmlSchemaChoice.ValidatedMaxOccurs == 0M && this.ValidatedMinOccurs == 0M && this.ValidatedMaxOccurs == 0M || this.ValidateSeqRecurseMapSumCommon((XmlSchemaGroupBase) xmlSchemaChoice, h, schema, true, false, raiseError);
      }
      if (raiseError)
        this.error(h, "Invalid choice derivation by restriction was found.");
      return false;
    }

    internal override Decimal GetMinEffectiveTotalRange()
    {
      if (this.minEffectiveTotalRange >= 0M)
        return this.minEffectiveTotalRange;
      Decimal effectiveTotalRange1 = 0M;
      if (this.Items.Count == 0)
      {
        effectiveTotalRange1 = 0M;
      }
      else
      {
        foreach (XmlSchemaParticle xmlSchemaParticle in this.Items)
        {
          Decimal effectiveTotalRange2 = xmlSchemaParticle.GetMinEffectiveTotalRange();
          if (effectiveTotalRange1 > effectiveTotalRange2)
            effectiveTotalRange1 = effectiveTotalRange2;
        }
      }
      this.minEffectiveTotalRange = effectiveTotalRange1;
      return effectiveTotalRange1;
    }

    internal override void ValidateUniqueParticleAttribution(
      XmlSchemaObjectTable qnames,
      ArrayList nsNames,
      ValidationEventHandler h,
      XmlSchema schema)
    {
      foreach (XmlSchemaParticle xmlSchemaParticle in this.Items)
        xmlSchemaParticle.ValidateUniqueParticleAttribution(qnames, nsNames, h, schema);
    }

    internal override void ValidateUniqueTypeAttribution(
      XmlSchemaObjectTable labels,
      ValidationEventHandler h,
      XmlSchema schema)
    {
      foreach (XmlSchemaParticle xmlSchemaParticle in this.Items)
        xmlSchemaParticle.ValidateUniqueTypeAttribution(labels, h, schema);
    }

    internal static XmlSchemaChoice Read(XmlSchemaReader reader, ValidationEventHandler h)
    {
      XmlSchemaChoice xso = new XmlSchemaChoice();
      reader.MoveToElement();
      if (reader.NamespaceURI != "http://www.w3.org/2001/XMLSchema" || reader.LocalName != "choice")
      {
        XmlSchemaObject.error(h, "Should not happen :1: XmlSchemaChoice.Read, name=" + reader.Name, (Exception) null);
        reader.SkipToEnd();
        return (XmlSchemaChoice) null;
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
          XmlSchemaObject.error(h, reader.Name + " is not a valid attribute for choice", (Exception) null);
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
          if (reader.LocalName != "choice")
          {
            XmlSchemaObject.error(h, "Should not happen :2: XmlSchemaChoice.Read, name=" + reader.Name, (Exception) null);
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
        {
          if (num <= 2)
          {
            if (reader.LocalName == "element")
            {
              num = 2;
              XmlSchemaElement xmlSchemaElement = XmlSchemaElement.Read(reader, h);
              if (xmlSchemaElement != null)
              {
                xso.items.Add((XmlSchemaObject) xmlSchemaElement);
                continue;
              }
              continue;
            }
            if (reader.LocalName == "group")
            {
              num = 2;
              XmlSchemaGroupRef xmlSchemaGroupRef = XmlSchemaGroupRef.Read(reader, h);
              if (xmlSchemaGroupRef != null)
              {
                xso.items.Add((XmlSchemaObject) xmlSchemaGroupRef);
                continue;
              }
              continue;
            }
            if (reader.LocalName == "choice")
            {
              num = 2;
              XmlSchemaChoice xmlSchemaChoice = XmlSchemaChoice.Read(reader, h);
              if (xmlSchemaChoice != null)
              {
                xso.items.Add((XmlSchemaObject) xmlSchemaChoice);
                continue;
              }
              continue;
            }
            if (reader.LocalName == "sequence")
            {
              num = 2;
              XmlSchemaSequence xmlSchemaSequence = XmlSchemaSequence.Read(reader, h);
              if (xmlSchemaSequence != null)
              {
                xso.items.Add((XmlSchemaObject) xmlSchemaSequence);
                continue;
              }
              continue;
            }
            if (reader.LocalName == "any")
            {
              num = 2;
              XmlSchemaAny xmlSchemaAny = XmlSchemaAny.Read(reader, h);
              if (xmlSchemaAny != null)
              {
                xso.items.Add((XmlSchemaObject) xmlSchemaAny);
                continue;
              }
              continue;
            }
          }
          reader.RaiseInvalidElementError();
        }
      }
      return xso;
    }
  }
}
