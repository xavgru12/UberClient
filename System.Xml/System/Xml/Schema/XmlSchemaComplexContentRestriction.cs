// Decompiled with JetBrains decompiler
// Type: System.Xml.Schema.XmlSchemaComplexContentRestriction
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Xml.Serialization;

namespace System.Xml.Schema
{
  public class XmlSchemaComplexContentRestriction : XmlSchemaContent
  {
    private const string xmlname = "restriction";
    private XmlSchemaAnyAttribute any;
    private XmlSchemaObjectCollection attributes;
    private XmlQualifiedName baseTypeName;
    private XmlSchemaParticle particle;

    public XmlSchemaComplexContentRestriction()
    {
      this.baseTypeName = XmlQualifiedName.Empty;
      this.attributes = new XmlSchemaObjectCollection();
    }

    [XmlAttribute("base")]
    public XmlQualifiedName BaseTypeName
    {
      get => this.baseTypeName;
      set => this.baseTypeName = value;
    }

    [XmlElement("group", typeof (XmlSchemaGroupRef))]
    [XmlElement("sequence", typeof (XmlSchemaSequence))]
    [XmlElement("choice", typeof (XmlSchemaChoice))]
    [XmlElement("all", typeof (XmlSchemaAll))]
    public XmlSchemaParticle Particle
    {
      get => this.particle;
      set => this.particle = value;
    }

    [XmlElement("attributeGroup", typeof (XmlSchemaAttributeGroupRef))]
    [XmlElement("attribute", typeof (XmlSchemaAttribute))]
    public XmlSchemaObjectCollection Attributes => this.attributes;

    [XmlElement("anyAttribute")]
    public XmlSchemaAnyAttribute AnyAttribute
    {
      get => this.any;
      set => this.any = value;
    }

    internal override bool IsExtension => false;

    internal override void SetParent(XmlSchemaObject parent)
    {
      base.SetParent(parent);
      if (this.Particle != null)
        this.Particle.SetParent((XmlSchemaObject) this);
      if (this.AnyAttribute != null)
        this.AnyAttribute.SetParent((XmlSchemaObject) this);
      foreach (XmlSchemaObject attribute in this.Attributes)
        attribute.SetParent((XmlSchemaObject) this);
    }

    internal override int Compile(ValidationEventHandler h, XmlSchema schema)
    {
      if (this.CompilationId == schema.CompilationId)
        return 0;
      if (this.isRedefinedComponent)
      {
        if (this.Annotation != null)
          this.Annotation.isRedefinedComponent = true;
        if (this.AnyAttribute != null)
          this.AnyAttribute.isRedefinedComponent = true;
        foreach (XmlSchemaObject attribute in this.Attributes)
          attribute.isRedefinedComponent = true;
        if (this.Particle != null)
          this.Particle.isRedefinedComponent = true;
      }
      if (this.BaseTypeName == (XmlQualifiedName) null || this.BaseTypeName.IsEmpty)
        this.error(h, "base must be present, as a QName");
      else if (!XmlSchemaUtil.CheckQName(this.BaseTypeName))
        this.error(h, "BaseTypeName is not a valid XmlQualifiedName");
      if (this.AnyAttribute != null)
        this.errorCount += this.AnyAttribute.Compile(h, schema);
      foreach (XmlSchemaObject attribute in this.Attributes)
      {
        switch (attribute)
        {
          case XmlSchemaAttribute _:
            this.errorCount += ((XmlSchemaAttribute) attribute).Compile(h, schema);
            continue;
          case XmlSchemaAttributeGroupRef _:
            this.errorCount += ((XmlSchemaAttributeGroupRef) attribute).Compile(h, schema);
            continue;
          default:
            this.error(h, attribute.GetType().ToString() + " is not valid in this place::ComplexContentRestriction");
            continue;
        }
      }
      if (this.Particle != null)
      {
        if (this.Particle is XmlSchemaGroupRef)
          this.errorCount += ((XmlSchemaGroupRef) this.Particle).Compile(h, schema);
        else if (this.Particle is XmlSchemaAll)
          this.errorCount += ((XmlSchemaAll) this.Particle).Compile(h, schema);
        else if (this.Particle is XmlSchemaChoice)
          this.errorCount += ((XmlSchemaChoice) this.Particle).Compile(h, schema);
        else if (this.Particle is XmlSchemaSequence)
          this.errorCount += ((XmlSchemaSequence) this.Particle).Compile(h, schema);
        else
          this.error(h, "Particle of a restriction is limited only to group, sequence, choice and all.");
      }
      XmlSchemaUtil.CompileID(this.Id, (XmlSchemaObject) this, schema.IDCollection, h);
      this.CompilationId = schema.CompilationId;
      return this.errorCount;
    }

    internal override XmlQualifiedName GetBaseTypeName() => this.baseTypeName;

    internal override XmlSchemaParticle GetParticle() => this.particle;

    internal override int Validate(ValidationEventHandler h, XmlSchema schema)
    {
      if (this.Particle != null)
        this.Particle.Validate(h, schema);
      return this.errorCount;
    }

    internal static XmlSchemaComplexContentRestriction Read(
      XmlSchemaReader reader,
      ValidationEventHandler h)
    {
      XmlSchemaComplexContentRestriction xso = new XmlSchemaComplexContentRestriction();
      reader.MoveToElement();
      if (reader.NamespaceURI != "http://www.w3.org/2001/XMLSchema" || reader.LocalName != "restriction")
      {
        XmlSchemaObject.error(h, "Should not happen :1: XmlSchemaComplexContentRestriction.Read, name=" + reader.Name, (Exception) null);
        reader.Skip();
        return (XmlSchemaComplexContentRestriction) null;
      }
      xso.LineNumber = reader.LineNumber;
      xso.LinePosition = reader.LinePosition;
      xso.SourceUri = reader.BaseURI;
      while (reader.MoveToNextAttribute())
      {
        if (reader.Name == "base")
        {
          Exception innerEx;
          xso.baseTypeName = XmlSchemaUtil.ReadQNameAttribute((XmlReader) reader, out innerEx);
          if (innerEx != null)
            XmlSchemaObject.error(h, reader.Value + " is not a valid value for base attribute", innerEx);
        }
        else if (reader.Name == "id")
          xso.Id = reader.Value;
        else if (reader.NamespaceURI == string.Empty && reader.Name != "xmlns" || reader.NamespaceURI == "http://www.w3.org/2001/XMLSchema")
          XmlSchemaObject.error(h, reader.Name + " is not a valid attribute for restriction", (Exception) null);
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
          if (reader.LocalName != "restriction")
          {
            XmlSchemaObject.error(h, "Should not happen :2: XmlSchemaComplexContentRestriction.Read, name=" + reader.Name, (Exception) null);
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
            if (reader.LocalName == "group")
            {
              num = 3;
              XmlSchemaGroupRef xmlSchemaGroupRef = XmlSchemaGroupRef.Read(reader, h);
              if (xmlSchemaGroupRef != null)
              {
                xso.particle = (XmlSchemaParticle) xmlSchemaGroupRef;
                continue;
              }
              continue;
            }
            if (reader.LocalName == "all")
            {
              num = 3;
              XmlSchemaAll xmlSchemaAll = XmlSchemaAll.Read(reader, h);
              if (xmlSchemaAll != null)
              {
                xso.particle = (XmlSchemaParticle) xmlSchemaAll;
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
                xso.particle = (XmlSchemaParticle) xmlSchemaChoice;
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
                xso.particle = (XmlSchemaParticle) xmlSchemaSequence;
                continue;
              }
              continue;
            }
          }
          if (num <= 3)
          {
            if (reader.LocalName == "attribute")
            {
              num = 3;
              XmlSchemaAttribute xmlSchemaAttribute = XmlSchemaAttribute.Read(reader, h);
              if (xmlSchemaAttribute != null)
              {
                xso.Attributes.Add((XmlSchemaObject) xmlSchemaAttribute);
                continue;
              }
              continue;
            }
            if (reader.LocalName == "attributeGroup")
            {
              num = 3;
              XmlSchemaAttributeGroupRef attributeGroupRef = XmlSchemaAttributeGroupRef.Read(reader, h);
              if (attributeGroupRef != null)
              {
                xso.attributes.Add((XmlSchemaObject) attributeGroupRef);
                continue;
              }
              continue;
            }
          }
          if (num <= 4 && reader.LocalName == "anyAttribute")
          {
            num = 5;
            XmlSchemaAnyAttribute schemaAnyAttribute = XmlSchemaAnyAttribute.Read(reader, h);
            if (schemaAnyAttribute != null)
              xso.AnyAttribute = schemaAnyAttribute;
          }
          else
            reader.RaiseInvalidElementError();
        }
      }
      return xso;
    }
  }
}
