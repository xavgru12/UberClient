// Decompiled with JetBrains decompiler
// Type: System.Xml.Schema.XmlSchemaComplexContent
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Xml.Serialization;

namespace System.Xml.Schema
{
  public class XmlSchemaComplexContent : XmlSchemaContentModel
  {
    private const string xmlname = "complexContent";
    private XmlSchemaContent content;
    private bool isMixed;

    [XmlAttribute("mixed")]
    public bool IsMixed
    {
      get => this.isMixed;
      set => this.isMixed = value;
    }

    [XmlElement("restriction", typeof (XmlSchemaComplexContentRestriction))]
    [XmlElement("extension", typeof (XmlSchemaComplexContentExtension))]
    public override XmlSchemaContent Content
    {
      get => this.content;
      set => this.content = value;
    }

    internal override void SetParent(XmlSchemaObject parent)
    {
      base.SetParent(parent);
      if (this.Content == null)
        return;
      this.Content.SetParent((XmlSchemaObject) this);
    }

    internal override int Compile(ValidationEventHandler h, XmlSchema schema)
    {
      if (this.CompilationId == schema.CompilationId)
        return 0;
      if (this.isRedefinedComponent)
      {
        if (this.Annotation != null)
          this.Annotation.isRedefinedComponent = true;
        if (this.Content != null)
          this.Content.isRedefinedComponent = true;
      }
      if (this.Content == null)
        this.error(h, "Content must be present in a complexContent");
      else if (this.Content is XmlSchemaComplexContentRestriction)
        this.errorCount += ((XmlSchemaComplexContentRestriction) this.Content).Compile(h, schema);
      else if (this.Content is XmlSchemaComplexContentExtension)
        this.errorCount += ((XmlSchemaComplexContentExtension) this.Content).Compile(h, schema);
      else
        this.error(h, "complexContent can't have any value other than restriction or extention");
      XmlSchemaUtil.CompileID(this.Id, (XmlSchemaObject) this, schema.IDCollection, h);
      this.CompilationId = schema.CompilationId;
      return this.errorCount;
    }

    internal override int Validate(ValidationEventHandler h, XmlSchema schema)
    {
      if (this.IsValidated(schema.ValidationId))
        return this.errorCount;
      this.errorCount += this.Content.Validate(h, schema);
      this.ValidationId = schema.ValidationId;
      return this.errorCount;
    }

    internal static XmlSchemaComplexContent Read(XmlSchemaReader reader, ValidationEventHandler h)
    {
      XmlSchemaComplexContent xso = new XmlSchemaComplexContent();
      reader.MoveToElement();
      if (reader.NamespaceURI != "http://www.w3.org/2001/XMLSchema" || reader.LocalName != "complexContent")
      {
        XmlSchemaObject.error(h, "Should not happen :1: XmlSchemaComplexContent.Read, name=" + reader.Name, (Exception) null);
        reader.Skip();
        return (XmlSchemaComplexContent) null;
      }
      xso.LineNumber = reader.LineNumber;
      xso.LinePosition = reader.LinePosition;
      xso.SourceUri = reader.BaseURI;
      while (reader.MoveToNextAttribute())
      {
        if (reader.Name == "id")
          xso.Id = reader.Value;
        else if (reader.Name == "mixed")
        {
          Exception innerExcpetion;
          xso.isMixed = XmlSchemaUtil.ReadBoolAttribute((XmlReader) reader, out innerExcpetion);
          if (innerExcpetion != null)
            XmlSchemaObject.error(h, reader.Value + " is an invalid value for mixed", innerExcpetion);
        }
        else if (reader.NamespaceURI == string.Empty && reader.Name != "xmlns" || reader.NamespaceURI == "http://www.w3.org/2001/XMLSchema")
          XmlSchemaObject.error(h, reader.Name + " is not a valid attribute for complexContent", (Exception) null);
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
          if (reader.LocalName != "complexContent")
          {
            XmlSchemaObject.error(h, "Should not happen :2: XmlSchemaComplexContent.Read, name=" + reader.Name, (Exception) null);
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
            if (reader.LocalName == "restriction")
            {
              num = 3;
              XmlSchemaComplexContentRestriction contentRestriction = XmlSchemaComplexContentRestriction.Read(reader, h);
              if (contentRestriction != null)
              {
                xso.content = (XmlSchemaContent) contentRestriction;
                continue;
              }
              continue;
            }
            if (reader.LocalName == "extension")
            {
              num = 3;
              XmlSchemaComplexContentExtension contentExtension = XmlSchemaComplexContentExtension.Read(reader, h);
              if (contentExtension != null)
              {
                xso.content = (XmlSchemaContent) contentExtension;
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
