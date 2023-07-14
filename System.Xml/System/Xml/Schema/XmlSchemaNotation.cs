// Decompiled with JetBrains decompiler
// Type: System.Xml.Schema.XmlSchemaNotation
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Xml.Serialization;

namespace System.Xml.Schema
{
  public class XmlSchemaNotation : XmlSchemaAnnotated
  {
    private const string xmlname = "notation";
    private string name;
    private string pub;
    private string system;
    private XmlQualifiedName qualifiedName;

    [XmlAttribute("name")]
    public string Name
    {
      get => this.name;
      set => this.name = value;
    }

    [XmlAttribute("public")]
    public string Public
    {
      get => this.pub;
      set => this.pub = value;
    }

    [XmlAttribute("system")]
    public string System
    {
      get => this.system;
      set => this.system = value;
    }

    [XmlIgnore]
    internal XmlQualifiedName QualifiedName => this.qualifiedName;

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
      if (this.Public == null)
        this.error(h, "public must be present");
      else if (!XmlSchemaUtil.CheckAnyUri(this.Public))
        this.error(h, "public must be anyURI");
      if (this.system != null && !XmlSchemaUtil.CheckAnyUri(this.system))
        this.error(h, "system must be present and of Type anyURI");
      XmlSchemaUtil.CompileID(this.Id, (XmlSchemaObject) this, schema.IDCollection, h);
      return this.errorCount;
    }

    internal override int Validate(ValidationEventHandler h, XmlSchema schema) => this.errorCount;

    internal static XmlSchemaNotation Read(XmlSchemaReader reader, ValidationEventHandler h)
    {
      XmlSchemaNotation xso = new XmlSchemaNotation();
      reader.MoveToElement();
      if (reader.NamespaceURI != "http://www.w3.org/2001/XMLSchema" || reader.LocalName != "notation")
      {
        XmlSchemaObject.error(h, "Should not happen :1: XmlSchemaInclude.Read, name=" + reader.Name, (Exception) null);
        reader.Skip();
        return (XmlSchemaNotation) null;
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
        else if (reader.Name == "public")
          xso.pub = reader.Value;
        else if (reader.Name == "system")
          xso.system = reader.Value;
        else if (reader.NamespaceURI == string.Empty && reader.Name != "xmlns" || reader.NamespaceURI == "http://www.w3.org/2001/XMLSchema")
          XmlSchemaObject.error(h, reader.Name + " is not a valid attribute for notation", (Exception) null);
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
          if (reader.LocalName != "notation")
          {
            XmlSchemaObject.error(h, "Should not happen :2: XmlSchemaNotation.Read, name=" + reader.Name, (Exception) null);
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
