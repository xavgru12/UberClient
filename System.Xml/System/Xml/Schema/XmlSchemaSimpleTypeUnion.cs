// Decompiled with JetBrains decompiler
// Type: System.Xml.Schema.XmlSchemaSimpleTypeUnion
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
  public class XmlSchemaSimpleTypeUnion : XmlSchemaSimpleTypeContent
  {
    private const string xmlname = "union";
    private XmlSchemaObjectCollection baseTypes;
    private XmlQualifiedName[] memberTypes;
    private object[] validatedTypes;
    private XmlSchemaSimpleType[] validatedSchemaTypes;

    public XmlSchemaSimpleTypeUnion() => this.baseTypes = new XmlSchemaObjectCollection();

    [XmlElement("simpleType", typeof (XmlSchemaSimpleType))]
    public XmlSchemaObjectCollection BaseTypes => this.baseTypes;

    [XmlAttribute("memberTypes")]
    public XmlQualifiedName[] MemberTypes
    {
      get => this.memberTypes;
      set => this.memberTypes = value;
    }

    [XmlIgnore]
    public XmlSchemaSimpleType[] BaseMemberTypes => this.validatedSchemaTypes;

    internal object[] ValidatedTypes => this.validatedTypes;

    internal override void SetParent(XmlSchemaObject parent)
    {
      base.SetParent(parent);
      foreach (XmlSchemaObject baseType in this.BaseTypes)
        baseType.SetParent((XmlSchemaObject) this);
    }

    internal override int Compile(ValidationEventHandler h, XmlSchema schema)
    {
      if (this.CompilationId == schema.CompilationId)
        return 0;
      this.errorCount = 0;
      int count = this.BaseTypes.Count;
      foreach (XmlSchemaObject baseType in this.baseTypes)
      {
        if (baseType != null && baseType is XmlSchemaSimpleType)
          this.errorCount += ((XmlSchemaSimpleType) baseType).Compile(h, schema);
        else
          this.error(h, "baseTypes can't have objects other than a simpletype");
      }
      if (this.memberTypes != null)
      {
        for (int index = 0; index < this.memberTypes.Length; ++index)
        {
          if (this.memberTypes[index] == (XmlQualifiedName) null || !XmlSchemaUtil.CheckQName(this.MemberTypes[index]))
          {
            this.error(h, "Invalid membertype");
            this.memberTypes[index] = XmlQualifiedName.Empty;
          }
          else
            count += this.MemberTypes.Length;
        }
      }
      if (count == 0)
        this.error(h, "Atleast one simpletype or membertype must be present");
      XmlSchemaUtil.CompileID(this.Id, (XmlSchemaObject) this, schema.IDCollection, h);
      this.CompilationId = schema.CompilationId;
      return this.errorCount;
    }

    internal override int Validate(ValidationEventHandler h, XmlSchema schema)
    {
      if (this.IsValidated(schema.ValidationId))
        return this.errorCount;
      ArrayList arrayList = new ArrayList();
      if (this.MemberTypes != null)
      {
        foreach (XmlQualifiedName memberType in this.MemberTypes)
        {
          object obj = (object) null;
          XmlSchemaType schemaType = (XmlSchemaType) (schema.FindSchemaType(memberType) as XmlSchemaSimpleType);
          if (schemaType != null)
          {
            this.errorCount += schemaType.Validate(h, schema);
            obj = (object) schemaType;
          }
          else if (memberType == XmlSchemaComplexType.AnyTypeName)
            obj = (object) XmlSchemaSimpleType.AnySimpleType;
          else if (memberType.Namespace == "http://www.w3.org/2001/XMLSchema" || memberType.Namespace == "http://www.w3.org/2003/11/xpath-datatypes")
          {
            obj = (object) XmlSchemaDatatype.FromName(memberType);
            if (obj == null)
              this.error(h, "Invalid schema type name was specified: " + (object) memberType);
          }
          else if (!schema.IsNamespaceAbsent(memberType.Namespace))
            this.error(h, "Referenced base schema type " + (object) memberType + " was not found in the corresponding schema.");
          arrayList.Add(obj);
        }
      }
      if (this.BaseTypes != null)
      {
        foreach (XmlSchemaSimpleType baseType in this.BaseTypes)
        {
          baseType.Validate(h, schema);
          arrayList.Add((object) baseType);
        }
      }
      this.validatedTypes = arrayList.ToArray();
      if (this.validatedTypes != null)
      {
        this.validatedSchemaTypes = new XmlSchemaSimpleType[this.validatedTypes.Length];
        for (int index = 0; index < this.validatedTypes.Length; ++index)
        {
          object validatedType = this.validatedTypes[index];
          switch (validatedType)
          {
            case XmlSchemaSimpleType builtInSimpleType:
            case null:
              this.validatedSchemaTypes[index] = builtInSimpleType;
              continue;
            default:
              builtInSimpleType = XmlSchemaType.GetBuiltInSimpleType(((XmlSchemaDatatype) validatedType).TypeCode);
              goto case null;
          }
        }
      }
      this.ValidationId = schema.ValidationId;
      return this.errorCount;
    }

    internal static XmlSchemaSimpleTypeUnion Read(XmlSchemaReader reader, ValidationEventHandler h)
    {
      XmlSchemaSimpleTypeUnion xso = new XmlSchemaSimpleTypeUnion();
      reader.MoveToElement();
      if (reader.NamespaceURI != "http://www.w3.org/2001/XMLSchema" || reader.LocalName != "union")
      {
        XmlSchemaObject.error(h, "Should not happen :1: XmlSchemaSimpleTypeUnion.Read, name=" + reader.Name, (Exception) null);
        reader.Skip();
        return (XmlSchemaSimpleTypeUnion) null;
      }
      xso.LineNumber = reader.LineNumber;
      xso.LinePosition = reader.LinePosition;
      xso.SourceUri = reader.BaseURI;
      while (reader.MoveToNextAttribute())
      {
        if (reader.Name == "id")
          xso.Id = reader.Value;
        else if (reader.Name == "memberTypes")
        {
          string[] strArray = XmlSchemaUtil.SplitList(reader.Value);
          xso.memberTypes = new XmlQualifiedName[strArray.Length];
          for (int index = 0; index < strArray.Length; ++index)
          {
            Exception innerEx;
            xso.memberTypes[index] = XmlSchemaUtil.ToQName((XmlReader) reader, strArray[index], out innerEx);
            if (innerEx != null)
              XmlSchemaObject.error(h, "'" + strArray[index] + "' is not a valid memberType", innerEx);
          }
        }
        else if (reader.NamespaceURI == string.Empty && reader.Name != "xmlns" || reader.NamespaceURI == "http://www.w3.org/2001/XMLSchema")
          XmlSchemaObject.error(h, reader.Name + " is not a valid attribute for union", (Exception) null);
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
          if (reader.LocalName != "union")
          {
            XmlSchemaObject.error(h, "Should not happen :2: XmlSchemaSimpleTypeUnion.Read, name=" + reader.Name, (Exception) null);
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
        else if (num <= 2 && reader.LocalName == "simpleType")
        {
          num = 2;
          XmlSchemaSimpleType schemaSimpleType = XmlSchemaSimpleType.Read(reader, h);
          if (schemaSimpleType != null)
            xso.baseTypes.Add((XmlSchemaObject) schemaSimpleType);
        }
        else
          reader.RaiseInvalidElementError();
      }
      return xso;
    }
  }
}
