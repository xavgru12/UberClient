// Decompiled with JetBrains decompiler
// Type: System.Xml.Schema.XmlSchemaSequence
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
  public class XmlSchemaSequence : XmlSchemaGroupBase
  {
    private const string xmlname = "sequence";
    private XmlSchemaObjectCollection items;

    public XmlSchemaSequence() => this.items = new XmlSchemaObjectCollection();

    [XmlElement("sequence", typeof (XmlSchemaSequence))]
    [XmlElement("any", typeof (XmlSchemaAny))]
    [XmlElement("choice", typeof (XmlSchemaChoice))]
    [XmlElement("element", typeof (XmlSchemaElement))]
    [XmlElement("group", typeof (XmlSchemaGroupRef))]
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
            this.error(h, "Invalid schema object was specified in the particles of the sequence model group.");
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
      {
        this.OptimizedParticle = XmlSchemaParticle.Empty;
        return this.OptimizedParticle;
      }
      if (!isTop && this.ValidatedMinOccurs == 1M && this.ValidatedMaxOccurs == 1M && this.Items.Count == 1)
        return ((XmlSchemaParticle) this.Items[0]).GetOptimizedParticle(false);
      XmlSchemaSequence xmlSchemaSequence1 = new XmlSchemaSequence();
      this.CopyInfo((XmlSchemaParticle) xmlSchemaSequence1);
      for (int index1 = 0; index1 < this.Items.Count; ++index1)
      {
        XmlSchemaParticle optimizedParticle = (this.Items[index1] as XmlSchemaParticle).GetOptimizedParticle(false);
        if (optimizedParticle != XmlSchemaParticle.Empty)
        {
          if (optimizedParticle is XmlSchemaSequence && optimizedParticle.ValidatedMinOccurs == 1M && optimizedParticle.ValidatedMaxOccurs == 1M)
          {
            XmlSchemaSequence xmlSchemaSequence2 = optimizedParticle as XmlSchemaSequence;
            for (int index2 = 0; index2 < xmlSchemaSequence2.Items.Count; ++index2)
            {
              xmlSchemaSequence1.Items.Add(xmlSchemaSequence2.Items[index2]);
              xmlSchemaSequence1.CompiledItems.Add(xmlSchemaSequence2.Items[index2]);
            }
          }
          else
          {
            xmlSchemaSequence1.Items.Add((XmlSchemaObject) optimizedParticle);
            xmlSchemaSequence1.CompiledItems.Add((XmlSchemaObject) optimizedParticle);
          }
        }
      }
      if (xmlSchemaSequence1.Items.Count == 0)
        this.OptimizedParticle = XmlSchemaParticle.Empty;
      else
        this.OptimizedParticle = (XmlSchemaParticle) xmlSchemaSequence1;
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
      if (this == baseParticle)
        return true;
      switch (baseParticle)
      {
        case XmlSchemaElement _:
          if (raiseError)
            this.error(h, "Invalid sequence paricle derivation.");
          return false;
        case XmlSchemaSequence xmlSchemaSequence:
          if (!this.ValidateOccurenceRangeOK((XmlSchemaParticle) xmlSchemaSequence, h, schema, raiseError))
            return false;
          return xmlSchemaSequence.ValidatedMinOccurs == 0M && xmlSchemaSequence.ValidatedMaxOccurs == 0M && this.ValidatedMinOccurs == 0M && this.ValidatedMaxOccurs == 0M || this.ValidateRecurse((XmlSchemaGroupBase) xmlSchemaSequence, h, schema, raiseError);
        case XmlSchemaAll xmlSchemaAll:
          XmlSchemaObjectCollection objectCollection = new XmlSchemaObjectCollection();
          for (int index = 0; index < this.Items.Count; ++index)
          {
            if (!(this.Items[index] is XmlSchemaElement xmlSchemaElement))
            {
              if (raiseError)
                this.error(h, "Invalid sequence particle derivation by restriction from all.");
              return false;
            }
            foreach (XmlSchemaElement baseParticle1 in xmlSchemaAll.Items)
            {
              if (baseParticle1.QualifiedName == xmlSchemaElement.QualifiedName)
              {
                if (objectCollection.Contains((XmlSchemaObject) baseParticle1))
                {
                  if (raiseError)
                    this.error(h, "Base element particle is mapped to the derived element particle in a sequence two or more times.");
                  return false;
                }
                objectCollection.Add((XmlSchemaObject) baseParticle1);
                if (!xmlSchemaElement.ValidateDerivationByRestriction((XmlSchemaParticle) baseParticle1, h, schema, raiseError))
                  return false;
              }
            }
          }
          foreach (XmlSchemaElement xmlSchemaElement in xmlSchemaAll.Items)
          {
            if (!objectCollection.Contains((XmlSchemaObject) xmlSchemaElement) && !xmlSchemaElement.ValidateIsEmptiable())
            {
              if (raiseError)
                this.error(h, "In base -all- particle, mapping-skipped base element which is not emptiable was found.");
              return false;
            }
          }
          return true;
        case XmlSchemaAny any:
          return this.ValidateNSRecurseCheckCardinality(any, h, schema, raiseError);
        case XmlSchemaChoice baseGroup:
          return this.ValidateSeqRecurseMapSumCommon((XmlSchemaGroupBase) baseGroup, h, schema, false, true, raiseError);
        default:
          return true;
      }
    }

    internal override Decimal GetMinEffectiveTotalRange() => this.GetMinEffectiveTotalRangeAllAndSequence();

    internal override void ValidateUniqueParticleAttribution(
      XmlSchemaObjectTable qnames,
      ArrayList nsNames,
      ValidationEventHandler h,
      XmlSchema schema)
    {
      this.ValidateUPAOnHeadingOptionalComponents(qnames, nsNames, h, schema);
      this.ValidateUPAOnItems(qnames, nsNames, h, schema);
    }

    private void ValidateUPAOnHeadingOptionalComponents(
      XmlSchemaObjectTable qnames,
      ArrayList nsNames,
      ValidationEventHandler h,
      XmlSchema schema)
    {
      foreach (XmlSchemaParticle xmlSchemaParticle in this.Items)
      {
        xmlSchemaParticle.ValidateUniqueParticleAttribution(qnames, nsNames, h, schema);
        if (xmlSchemaParticle.ValidatedMinOccurs != 0M)
          break;
      }
    }

    private void ValidateUPAOnItems(
      XmlSchemaObjectTable qnames,
      ArrayList nsNames,
      ValidationEventHandler h,
      XmlSchema schema)
    {
      XmlSchemaObjectTable qnames1 = new XmlSchemaObjectTable();
      ArrayList arrayList1 = new ArrayList();
      XmlSchemaObjectTable schemaObjectTable = new XmlSchemaObjectTable();
      ArrayList arrayList2 = new ArrayList();
      for (int index = 0; index < this.Items.Count; ++index)
      {
        XmlSchemaParticle xmlSchemaParticle = this.Items[index] as XmlSchemaParticle;
        xmlSchemaParticle.ValidateUniqueParticleAttribution(qnames1, arrayList1, h, schema);
        if (xmlSchemaParticle.ValidatedMinOccurs == xmlSchemaParticle.ValidatedMaxOccurs)
        {
          qnames1.Clear();
          arrayList1.Clear();
        }
        else
        {
          if (xmlSchemaParticle.ValidatedMinOccurs != 0M)
          {
            foreach (XmlQualifiedName name in (IEnumerable) schemaObjectTable.Names)
              qnames1.Set(name, (XmlSchemaObject) null);
            foreach (object obj in arrayList2)
              arrayList1.Remove(obj);
          }
          foreach (XmlQualifiedName name in (IEnumerable) qnames1.Names)
            schemaObjectTable.Set(name, qnames1[name]);
          arrayList2.Clear();
          arrayList2.AddRange((ICollection) arrayList1);
        }
      }
    }

    internal override void ValidateUniqueTypeAttribution(
      XmlSchemaObjectTable labels,
      ValidationEventHandler h,
      XmlSchema schema)
    {
      foreach (XmlSchemaParticle xmlSchemaParticle in this.Items)
        xmlSchemaParticle.ValidateUniqueTypeAttribution(labels, h, schema);
    }

    internal static XmlSchemaSequence Read(XmlSchemaReader reader, ValidationEventHandler h)
    {
      XmlSchemaSequence xso = new XmlSchemaSequence();
      reader.MoveToElement();
      if (reader.NamespaceURI != "http://www.w3.org/2001/XMLSchema" || reader.LocalName != "sequence")
      {
        XmlSchemaObject.error(h, "Should not happen :1: XmlSchemaSequence.Read, name=" + reader.Name, (Exception) null);
        reader.Skip();
        return (XmlSchemaSequence) null;
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
          XmlSchemaObject.error(h, reader.Name + " is not a valid attribute for sequence", (Exception) null);
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
          if (reader.LocalName != "sequence")
          {
            XmlSchemaObject.error(h, "Should not happen :2: XmlSchemaSequence.Read, name=" + reader.Name, (Exception) null);
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
