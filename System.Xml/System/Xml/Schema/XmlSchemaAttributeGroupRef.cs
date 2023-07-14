// Decompiled with JetBrains decompiler
// Type: System.Xml.Schema.XmlSchemaAttributeGroupRef
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Xml.Serialization;

namespace System.Xml.Schema
{
  public class XmlSchemaAttributeGroupRef : XmlSchemaAnnotated
  {
    private const string xmlname = "attributeGroup";
    private XmlQualifiedName refName;

    public XmlSchemaAttributeGroupRef() => this.refName = XmlQualifiedName.Empty;

    [XmlAttribute("ref")]
    public XmlQualifiedName RefName
    {
      get => this.refName;
      set => this.refName = value;
    }

    internal override int Compile(ValidationEventHandler h, XmlSchema schema)
    {
      if (this.CompilationId == schema.CompilationId)
        return 0;
      this.errorCount = 0;
      if (this.RefName == (XmlQualifiedName) null || this.RefName.IsEmpty)
        this.error(h, "ref must be present");
      else if (!XmlSchemaUtil.CheckQName(this.RefName))
        this.error(h, "ref must be a valid qname");
      XmlSchemaUtil.CompileID(this.Id, (XmlSchemaObject) this, schema.IDCollection, h);
      this.CompilationId = schema.CompilationId;
      return this.errorCount;
    }

    internal override int Validate(ValidationEventHandler h, XmlSchema schema) => this.errorCount;

    internal static XmlSchemaAttributeGroupRef Read(
      XmlSchemaReader reader,
      ValidationEventHandler h)
    {
      XmlSchemaAttributeGroupRef xso = new XmlSchemaAttributeGroupRef();
      reader.MoveToElement();
      if (reader.NamespaceURI != "http://www.w3.org/2001/XMLSchema" || reader.LocalName != "attributeGroup")
      {
        XmlSchemaObject.error(h, "Should not happen :1: XmlSchemaAttributeGroupRef.Read, name=" + reader.Name, (Exception) null);
        reader.SkipToEnd();
        return (XmlSchemaAttributeGroupRef) null;
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
        else if (reader.NamespaceURI == string.Empty && reader.Name != "xmlns" || reader.NamespaceURI == "http://www.w3.org/2001/XMLSchema")
          XmlSchemaObject.error(h, reader.Name + " is not a valid attribute for attributeGroup in this context", (Exception) null);
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
          if (reader.LocalName != "attributeGroup")
          {
            XmlSchemaObject.error(h, "Should not happen :2: XmlSchemaAttributeGroupRef.Read, name=" + reader.Name, (Exception) null);
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
