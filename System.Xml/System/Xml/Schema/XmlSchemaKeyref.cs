// Decompiled with JetBrains decompiler
// Type: System.Xml.Schema.XmlSchemaKeyref
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Xml.Serialization;

namespace System.Xml.Schema
{
  public class XmlSchemaKeyref : XmlSchemaIdentityConstraint
  {
    private const string xmlname = "keyref";
    private XmlQualifiedName refer;
    private XmlSchemaIdentityConstraint target;

    public XmlSchemaKeyref() => this.refer = XmlQualifiedName.Empty;

    [XmlAttribute("refer")]
    public XmlQualifiedName Refer
    {
      get => this.refer;
      set => this.refer = value;
    }

    internal XmlSchemaIdentityConstraint Target => this.target;

    internal override int Compile(ValidationEventHandler h, XmlSchema schema)
    {
      base.Compile(h, schema);
      if (this.refer == (XmlQualifiedName) null || this.refer.IsEmpty)
        this.error(h, "refer must be present");
      else if (!XmlSchemaUtil.CheckQName(this.refer))
        this.error(h, "Refer is not a valid XmlQualifiedName");
      return this.errorCount;
    }

    internal override int Validate(ValidationEventHandler h, XmlSchema schema)
    {
      if (!(schema.NamedIdentities[this.Refer] is XmlSchemaIdentityConstraint namedIdentity))
        this.error(h, "Target key was not found.");
      else if (namedIdentity is XmlSchemaKeyref)
        this.error(h, "Target identity constraint was keyref.");
      else if (namedIdentity.Fields.Count != this.Fields.Count)
        this.error(h, "Target identity constraint has different number of fields.");
      else
        this.target = namedIdentity;
      return this.errorCount;
    }

    internal static XmlSchemaKeyref Read(XmlSchemaReader reader, ValidationEventHandler h)
    {
      XmlSchemaKeyref xso = new XmlSchemaKeyref();
      reader.MoveToElement();
      if (reader.NamespaceURI != "http://www.w3.org/2001/XMLSchema" || reader.LocalName != "keyref")
      {
        XmlSchemaObject.error(h, "Should not happen :1: XmlSchemaKeyref.Read, name=" + reader.Name, (Exception) null);
        reader.Skip();
        return (XmlSchemaKeyref) null;
      }
      xso.LineNumber = reader.LineNumber;
      xso.LinePosition = reader.LinePosition;
      xso.SourceUri = reader.BaseURI;
      while (reader.MoveToNextAttribute())
      {
        if (reader.Name == "id")
          xso.Id = reader.Value;
        else if (reader.Name == "name")
          xso.Name = reader.Value;
        else if (reader.Name == "refer")
        {
          Exception innerEx;
          xso.refer = XmlSchemaUtil.ReadQNameAttribute((XmlReader) reader, out innerEx);
          if (innerEx != null)
            XmlSchemaObject.error(h, reader.Value + " is not a valid value for refer attribute", innerEx);
        }
        else if (reader.NamespaceURI == string.Empty && reader.Name != "xmlns" || reader.NamespaceURI == "http://www.w3.org/2001/XMLSchema")
          XmlSchemaObject.error(h, reader.Name + " is not a valid attribute for keyref", (Exception) null);
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
          if (reader.LocalName != "keyref")
          {
            XmlSchemaObject.error(h, "Should not happen :2: XmlSchemaKeyref.Read, name=" + reader.Name, (Exception) null);
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
        else if (num <= 2 && reader.LocalName == "selector")
        {
          num = 3;
          XmlSchemaXPath xmlSchemaXpath = XmlSchemaXPath.Read(reader, h, "selector");
          if (xmlSchemaXpath != null)
            xso.Selector = xmlSchemaXpath;
        }
        else if (num <= 3 && reader.LocalName == "field")
        {
          num = 3;
          if (xso.Selector == null)
            XmlSchemaObject.error(h, "selector must be defined before field declarations", (Exception) null);
          XmlSchemaXPath xmlSchemaXpath = XmlSchemaXPath.Read(reader, h, "field");
          if (xmlSchemaXpath != null)
            xso.Fields.Add((XmlSchemaObject) xmlSchemaXpath);
        }
        else
          reader.RaiseInvalidElementError();
      }
      return xso;
    }
  }
}
