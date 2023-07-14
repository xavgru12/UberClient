// Decompiled with JetBrains decompiler
// Type: System.Xml.Schema.XmlSchemaGroup
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
  public class XmlSchemaGroup : XmlSchemaAnnotated
  {
    private const string xmlname = "group";
    private string name;
    private XmlSchemaGroupBase particle;
    private XmlQualifiedName qualifiedName;
    private bool isCircularDefinition;

    public XmlSchemaGroup() => this.qualifiedName = XmlQualifiedName.Empty;

    [XmlAttribute("name")]
    public string Name
    {
      get => this.name;
      set => this.name = value;
    }

    [XmlElement("all", typeof (XmlSchemaAll))]
    [XmlElement("choice", typeof (XmlSchemaChoice))]
    [XmlElement("sequence", typeof (XmlSchemaSequence))]
    public XmlSchemaGroupBase Particle
    {
      get => this.particle;
      set => this.particle = value;
    }

    [XmlIgnore]
    public XmlQualifiedName QualifiedName => this.qualifiedName;

    internal bool IsCircularDefinition => this.isCircularDefinition;

    internal override void SetParent(XmlSchemaObject parent)
    {
      base.SetParent(parent);
      if (this.Particle == null)
        return;
      this.Particle.SetParent((XmlSchemaObject) this);
    }

    internal override int Compile(ValidationEventHandler h, XmlSchema schema)
    {
      if (this.CompilationId == schema.CompilationId)
        return 0;
      if (this.Name == null)
        this.error(h, "Required attribute name must be present");
      else if (!XmlSchemaUtil.CheckNCName(this.name))
        this.error(h, "attribute name must be NCName");
      else
        this.qualifiedName = new XmlQualifiedName(this.Name, this.AncestorSchema.TargetNamespace);
      if (this.Particle == null)
      {
        this.error(h, "Particle is required");
      }
      else
      {
        if (this.Particle.MaxOccursString != null)
          this.Particle.error(h, "MaxOccurs must not be present when the Particle is a child of Group");
        if (this.Particle.MinOccursString != null)
          this.Particle.error(h, "MinOccurs must not be present when the Particle is a child of Group");
        this.Particle.Compile(h, schema);
      }
      XmlSchemaUtil.CompileID(this.Id, (XmlSchemaObject) this, schema.IDCollection, h);
      this.CompilationId = schema.CompilationId;
      return this.errorCount;
    }

    internal override int Validate(ValidationEventHandler h, XmlSchema schema)
    {
      if (this.IsValidated(schema.ValidationId))
        return this.errorCount;
      if (this.Particle != null)
      {
        this.Particle.parentIsGroupDefinition = true;
        try
        {
          this.Particle.CheckRecursion(0, h, schema);
        }
        catch (XmlSchemaException ex)
        {
          XmlSchemaObject.error(h, ex.Message, (Exception) ex);
          this.isCircularDefinition = true;
          return this.errorCount;
        }
        this.errorCount += this.Particle.Validate(h, schema);
        this.Particle.ValidateUniqueParticleAttribution(new XmlSchemaObjectTable(), new ArrayList(), h, schema);
        this.Particle.ValidateUniqueTypeAttribution(new XmlSchemaObjectTable(), h, schema);
      }
      this.ValidationId = schema.ValidationId;
      return this.errorCount;
    }

    internal static XmlSchemaGroup Read(XmlSchemaReader reader, ValidationEventHandler h)
    {
      XmlSchemaGroup xso = new XmlSchemaGroup();
      reader.MoveToElement();
      if (reader.NamespaceURI != "http://www.w3.org/2001/XMLSchema" || reader.LocalName != "group")
      {
        XmlSchemaObject.error(h, "Should not happen :1: XmlSchemaGroup.Read, name=" + reader.Name, (Exception) null);
        reader.Skip();
        return (XmlSchemaGroup) null;
      }
      xso.LineNumber = reader.LineNumber;
      xso.LinePosition = reader.LinePosition;
      xso.SourceUri = reader.BaseURI;
      while (reader.MoveToNextAttribute())
      {
        if (reader.Name == "id")
          xso.Id = reader.Value;
        else if (reader.Name == "name")
          xso.name = reader.Value;
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
            XmlSchemaObject.error(h, "Should not happen :2: XmlSchemaGroup.Read, name=" + reader.Name, (Exception) null);
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
            if (reader.LocalName == "all")
            {
              num = 3;
              XmlSchemaAll xmlSchemaAll = XmlSchemaAll.Read(reader, h);
              if (xmlSchemaAll != null)
              {
                xso.Particle = (XmlSchemaGroupBase) xmlSchemaAll;
                continue;
              }
              continue;
            }
            if (reader.LocalName == "choice")
            {
              num = 3;
              XmlSchemaChoice xmlSchemaChoice = XmlSchemaChoice.Read(reader, h);
              if (xmlSchemaChoice != null)
              {
                xso.Particle = (XmlSchemaGroupBase) xmlSchemaChoice;
                continue;
              }
              continue;
            }
            if (reader.LocalName == "sequence")
            {
              num = 3;
              XmlSchemaSequence xmlSchemaSequence = XmlSchemaSequence.Read(reader, h);
              if (xmlSchemaSequence != null)
              {
                xso.Particle = (XmlSchemaGroupBase) xmlSchemaSequence;
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
