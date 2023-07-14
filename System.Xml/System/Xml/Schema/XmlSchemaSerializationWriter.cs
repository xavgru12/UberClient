// Decompiled with JetBrains decompiler
// Type: System.Xml.Schema.XmlSchemaSerializationWriter
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
  internal class XmlSchemaSerializationWriter : XmlSerializationWriter
  {
    private const string xmlNamespace = "http://www.w3.org/2000/xmlns/";

    public void WriteRoot_XmlSchema(object o)
    {
      this.WriteStartDocument();
      XmlSchema ob = (XmlSchema) o;
      this.TopLevelElement();
      this.WriteObject_XmlSchema(ob, "schema", "http://www.w3.org/2001/XMLSchema", true, false, true);
    }

    private void WriteObject_XmlSchema(
      XmlSchema ob,
      string element,
      string namesp,
      bool isNullable,
      bool needType,
      bool writeWrappingElem)
    {
      if (ob == null)
      {
        if (!isNullable)
          return;
        this.WriteNullTagLiteral(element, namesp);
      }
      else
      {
        if (ob.GetType() != typeof (XmlSchema))
          throw this.CreateUnknownTypeException((object) ob);
        if (writeWrappingElem)
          this.WriteStartElement(element, namesp, (object) ob);
        if (needType)
          this.WriteXsiType("XmlSchema", "http://www.w3.org/2001/XMLSchema");
        this.WriteNamespaceDeclarations(ob.Namespaces);
        ICollection unhandledAttributes = (ICollection) ob.UnhandledAttributes;
        if (unhandledAttributes != null)
        {
          foreach (XmlAttribute node in (IEnumerable) unhandledAttributes)
          {
            if (node.NamespaceURI != "http://www.w3.org/2000/xmlns/")
              this.WriteXmlAttribute((XmlNode) node, (object) ob);
          }
        }
        if (ob.AttributeFormDefault != XmlSchemaForm.None)
          this.WriteAttribute("attributeFormDefault", string.Empty, this.GetEnumValue_XmlSchemaForm(ob.AttributeFormDefault));
        if (ob.BlockDefault != XmlSchemaDerivationMethod.None)
          this.WriteAttribute("blockDefault", string.Empty, this.GetEnumValue_XmlSchemaDerivationMethod(ob.BlockDefault));
        if (ob.FinalDefault != XmlSchemaDerivationMethod.None)
          this.WriteAttribute("finalDefault", string.Empty, this.GetEnumValue_XmlSchemaDerivationMethod(ob.FinalDefault));
        if (ob.ElementFormDefault != XmlSchemaForm.None)
          this.WriteAttribute("elementFormDefault", string.Empty, this.GetEnumValue_XmlSchemaForm(ob.ElementFormDefault));
        this.WriteAttribute("targetNamespace", string.Empty, ob.TargetNamespace);
        this.WriteAttribute("version", string.Empty, ob.Version);
        this.WriteAttribute("id", string.Empty, ob.Id);
        if (ob.Includes != null)
        {
          for (int index = 0; index < ob.Includes.Count; ++index)
          {
            if (ob.Includes[index] != null)
            {
              if (ob.Includes[index].GetType() == typeof (XmlSchemaInclude))
                this.WriteObject_XmlSchemaInclude((XmlSchemaInclude) ob.Includes[index], "include", "http://www.w3.org/2001/XMLSchema", false, false, true);
              else if (ob.Includes[index].GetType() == typeof (XmlSchemaImport))
              {
                this.WriteObject_XmlSchemaImport((XmlSchemaImport) ob.Includes[index], "import", "http://www.w3.org/2001/XMLSchema", false, false, true);
              }
              else
              {
                if (ob.Includes[index].GetType() != typeof (XmlSchemaRedefine))
                  throw this.CreateUnknownTypeException((object) ob.Includes[index]);
                this.WriteObject_XmlSchemaRedefine((XmlSchemaRedefine) ob.Includes[index], "redefine", "http://www.w3.org/2001/XMLSchema", false, false, true);
              }
            }
          }
        }
        if (ob.Items != null)
        {
          for (int index = 0; index < ob.Items.Count; ++index)
          {
            if (ob.Items[index] != null)
            {
              if (ob.Items[index].GetType() == typeof (XmlSchemaElement))
                this.WriteObject_XmlSchemaElement((XmlSchemaElement) ob.Items[index], nameof (element), "http://www.w3.org/2001/XMLSchema", false, false, true);
              else if (ob.Items[index].GetType() == typeof (XmlSchemaSimpleType))
                this.WriteObject_XmlSchemaSimpleType((XmlSchemaSimpleType) ob.Items[index], "simpleType", "http://www.w3.org/2001/XMLSchema", false, false, true);
              else if (ob.Items[index].GetType() == typeof (XmlSchemaAttribute))
                this.WriteObject_XmlSchemaAttribute((XmlSchemaAttribute) ob.Items[index], "attribute", "http://www.w3.org/2001/XMLSchema", false, false, true);
              else if (ob.Items[index].GetType() == typeof (XmlSchemaAnnotation))
                this.WriteObject_XmlSchemaAnnotation((XmlSchemaAnnotation) ob.Items[index], "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
              else if (ob.Items[index].GetType() == typeof (XmlSchemaAttributeGroup))
                this.WriteObject_XmlSchemaAttributeGroup((XmlSchemaAttributeGroup) ob.Items[index], "attributeGroup", "http://www.w3.org/2001/XMLSchema", false, false, true);
              else if (ob.Items[index].GetType() == typeof (XmlSchemaGroup))
                this.WriteObject_XmlSchemaGroup((XmlSchemaGroup) ob.Items[index], "group", "http://www.w3.org/2001/XMLSchema", false, false, true);
              else if (ob.Items[index].GetType() == typeof (XmlSchemaComplexType))
              {
                this.WriteObject_XmlSchemaComplexType((XmlSchemaComplexType) ob.Items[index], "complexType", "http://www.w3.org/2001/XMLSchema", false, false, true);
              }
              else
              {
                if (ob.Items[index].GetType() != typeof (XmlSchemaNotation))
                  throw this.CreateUnknownTypeException((object) ob.Items[index]);
                this.WriteObject_XmlSchemaNotation((XmlSchemaNotation) ob.Items[index], "notation", "http://www.w3.org/2001/XMLSchema", false, false, true);
              }
            }
          }
        }
        if (!writeWrappingElem)
          return;
        this.WriteEndElement((object) ob);
      }
    }

    private string GetEnumValue_XmlSchemaForm(XmlSchemaForm val)
    {
      switch (val)
      {
        case XmlSchemaForm.Qualified:
          return "qualified";
        case XmlSchemaForm.Unqualified:
          return "unqualified";
        default:
          return ((long) val).ToString((IFormatProvider) CultureInfo.InvariantCulture);
      }
    }

    private string GetEnumValue_XmlSchemaDerivationMethod(XmlSchemaDerivationMethod val)
    {
      XmlSchemaDerivationMethod derivationMethod = val;
      switch (derivationMethod)
      {
        case XmlSchemaDerivationMethod.Empty:
          return string.Empty;
        case XmlSchemaDerivationMethod.Substitution:
          return "substitution";
        case XmlSchemaDerivationMethod.Extension:
          return "extension";
        case XmlSchemaDerivationMethod.Restriction:
          return "restriction";
        case XmlSchemaDerivationMethod.List:
          return "list";
        default:
          if (derivationMethod == XmlSchemaDerivationMethod.Union)
            return "union";
          if (derivationMethod == XmlSchemaDerivationMethod.All)
            return "#all";
          StringBuilder stringBuilder = new StringBuilder();
          string str1 = val.ToString();
          char[] chArray = new char[1]{ ',' };
          foreach (string str2 in str1.Split(chArray))
          {
            string key = str2.Trim();
            if (key != null)
            {
              // ISSUE: reference to a compiler-generated field
              if (XmlSchemaSerializationWriter.\u003C\u003Ef__switch\u0024map35 == null)
              {
                // ISSUE: reference to a compiler-generated field
                XmlSchemaSerializationWriter.\u003C\u003Ef__switch\u0024map35 = new Dictionary<string, int>(7)
                {
                  {
                    "Empty",
                    0
                  },
                  {
                    "Substitution",
                    1
                  },
                  {
                    "Extension",
                    2
                  },
                  {
                    "Restriction",
                    3
                  },
                  {
                    "List",
                    4
                  },
                  {
                    "Union",
                    5
                  },
                  {
                    "All",
                    6
                  }
                };
              }
              int num;
              // ISSUE: reference to a compiler-generated field
              if (XmlSchemaSerializationWriter.\u003C\u003Ef__switch\u0024map35.TryGetValue(key, out num))
              {
                switch (num)
                {
                  case 0:
                    stringBuilder.Append(string.Empty).Append(' ');
                    continue;
                  case 1:
                    stringBuilder.Append("substitution").Append(' ');
                    continue;
                  case 2:
                    stringBuilder.Append("extension").Append(' ');
                    continue;
                  case 3:
                    stringBuilder.Append("restriction").Append(' ');
                    continue;
                  case 4:
                    stringBuilder.Append("list").Append(' ');
                    continue;
                  case 5:
                    stringBuilder.Append("union").Append(' ');
                    continue;
                  case 6:
                    stringBuilder.Append("#all").Append(' ');
                    continue;
                }
              }
            }
            stringBuilder.Append(str2.Trim()).Append(' ');
          }
          return stringBuilder.ToString().Trim();
      }
    }

    private void WriteObject_XmlSchemaInclude(
      XmlSchemaInclude ob,
      string element,
      string namesp,
      bool isNullable,
      bool needType,
      bool writeWrappingElem)
    {
      if (ob == null)
      {
        if (!isNullable)
          return;
        this.WriteNullTagLiteral(element, namesp);
      }
      else
      {
        if (ob.GetType() != typeof (XmlSchemaInclude))
          throw this.CreateUnknownTypeException((object) ob);
        if (writeWrappingElem)
          this.WriteStartElement(element, namesp, (object) ob);
        if (needType)
          this.WriteXsiType("XmlSchemaInclude", "http://www.w3.org/2001/XMLSchema");
        this.WriteNamespaceDeclarations(ob.Namespaces);
        ICollection unhandledAttributes = (ICollection) ob.UnhandledAttributes;
        if (unhandledAttributes != null)
        {
          foreach (XmlAttribute node in (IEnumerable) unhandledAttributes)
          {
            if (node.NamespaceURI != "http://www.w3.org/2000/xmlns/")
              this.WriteXmlAttribute((XmlNode) node, (object) ob);
          }
        }
        this.WriteAttribute("schemaLocation", string.Empty, ob.SchemaLocation);
        this.WriteAttribute("id", string.Empty, ob.Id);
        this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
        if (!writeWrappingElem)
          return;
        this.WriteEndElement((object) ob);
      }
    }

    private void WriteObject_XmlSchemaImport(
      XmlSchemaImport ob,
      string element,
      string namesp,
      bool isNullable,
      bool needType,
      bool writeWrappingElem)
    {
      if (ob == null)
      {
        if (!isNullable)
          return;
        this.WriteNullTagLiteral(element, namesp);
      }
      else
      {
        if (ob.GetType() != typeof (XmlSchemaImport))
          throw this.CreateUnknownTypeException((object) ob);
        if (writeWrappingElem)
          this.WriteStartElement(element, namesp, (object) ob);
        if (needType)
          this.WriteXsiType("XmlSchemaImport", "http://www.w3.org/2001/XMLSchema");
        this.WriteNamespaceDeclarations(ob.Namespaces);
        ICollection unhandledAttributes = (ICollection) ob.UnhandledAttributes;
        if (unhandledAttributes != null)
        {
          foreach (XmlAttribute node in (IEnumerable) unhandledAttributes)
          {
            if (node.NamespaceURI != "http://www.w3.org/2000/xmlns/")
              this.WriteXmlAttribute((XmlNode) node, (object) ob);
          }
        }
        this.WriteAttribute("schemaLocation", string.Empty, ob.SchemaLocation);
        this.WriteAttribute("id", string.Empty, ob.Id);
        this.WriteAttribute("namespace", string.Empty, ob.Namespace);
        this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
        if (!writeWrappingElem)
          return;
        this.WriteEndElement((object) ob);
      }
    }

    private void WriteObject_XmlSchemaRedefine(
      XmlSchemaRedefine ob,
      string element,
      string namesp,
      bool isNullable,
      bool needType,
      bool writeWrappingElem)
    {
      if (ob == null)
      {
        if (!isNullable)
          return;
        this.WriteNullTagLiteral(element, namesp);
      }
      else
      {
        if (ob.GetType() != typeof (XmlSchemaRedefine))
          throw this.CreateUnknownTypeException((object) ob);
        if (writeWrappingElem)
          this.WriteStartElement(element, namesp, (object) ob);
        if (needType)
          this.WriteXsiType("XmlSchemaRedefine", "http://www.w3.org/2001/XMLSchema");
        this.WriteNamespaceDeclarations(ob.Namespaces);
        ICollection unhandledAttributes = (ICollection) ob.UnhandledAttributes;
        if (unhandledAttributes != null)
        {
          foreach (XmlAttribute node in (IEnumerable) unhandledAttributes)
          {
            if (node.NamespaceURI != "http://www.w3.org/2000/xmlns/")
              this.WriteXmlAttribute((XmlNode) node, (object) ob);
          }
        }
        this.WriteAttribute("schemaLocation", string.Empty, ob.SchemaLocation);
        this.WriteAttribute("id", string.Empty, ob.Id);
        if (ob.Items != null)
        {
          for (int index = 0; index < ob.Items.Count; ++index)
          {
            if (ob.Items[index] != null)
            {
              if (ob.Items[index].GetType() == typeof (XmlSchemaGroup))
                this.WriteObject_XmlSchemaGroup((XmlSchemaGroup) ob.Items[index], "group", "http://www.w3.org/2001/XMLSchema", false, false, true);
              else if (ob.Items[index].GetType() == typeof (XmlSchemaComplexType))
                this.WriteObject_XmlSchemaComplexType((XmlSchemaComplexType) ob.Items[index], "complexType", "http://www.w3.org/2001/XMLSchema", false, false, true);
              else if (ob.Items[index].GetType() == typeof (XmlSchemaSimpleType))
                this.WriteObject_XmlSchemaSimpleType((XmlSchemaSimpleType) ob.Items[index], "simpleType", "http://www.w3.org/2001/XMLSchema", false, false, true);
              else if (ob.Items[index].GetType() == typeof (XmlSchemaAnnotation))
              {
                this.WriteObject_XmlSchemaAnnotation((XmlSchemaAnnotation) ob.Items[index], "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
              }
              else
              {
                if (ob.Items[index].GetType() != typeof (XmlSchemaAttributeGroup))
                  throw this.CreateUnknownTypeException((object) ob.Items[index]);
                this.WriteObject_XmlSchemaAttributeGroup((XmlSchemaAttributeGroup) ob.Items[index], "attributeGroup", "http://www.w3.org/2001/XMLSchema", false, false, true);
              }
            }
          }
        }
        if (!writeWrappingElem)
          return;
        this.WriteEndElement((object) ob);
      }
    }

    private void WriteObject_XmlSchemaElement(
      XmlSchemaElement ob,
      string element,
      string namesp,
      bool isNullable,
      bool needType,
      bool writeWrappingElem)
    {
      if (ob == null)
      {
        if (!isNullable)
          return;
        this.WriteNullTagLiteral(element, namesp);
      }
      else
      {
        if (ob.GetType() != typeof (XmlSchemaElement))
          throw this.CreateUnknownTypeException((object) ob);
        if (writeWrappingElem)
          this.WriteStartElement(element, namesp, (object) ob);
        if (needType)
          this.WriteXsiType("XmlSchemaElement", "http://www.w3.org/2001/XMLSchema");
        this.WriteNamespaceDeclarations(ob.Namespaces);
        ICollection unhandledAttributes = (ICollection) ob.UnhandledAttributes;
        if (unhandledAttributes != null)
        {
          foreach (XmlAttribute node in (IEnumerable) unhandledAttributes)
          {
            if (node.NamespaceURI != "http://www.w3.org/2000/xmlns/")
              this.WriteXmlAttribute((XmlNode) node, (object) ob);
          }
        }
        this.WriteAttribute("id", string.Empty, ob.Id);
        this.WriteAttribute("minOccurs", string.Empty, ob.MinOccursString);
        this.WriteAttribute("maxOccurs", string.Empty, ob.MaxOccursString);
        if (ob.IsAbstract)
          this.WriteAttribute("abstract", string.Empty, !ob.IsAbstract ? "false" : "true");
        if (ob.Block != XmlSchemaDerivationMethod.None)
          this.WriteAttribute("block", string.Empty, this.GetEnumValue_XmlSchemaDerivationMethod(ob.Block));
        if (ob.DefaultValue != null)
          this.WriteAttribute("default", string.Empty, ob.DefaultValue);
        if (ob.Final != XmlSchemaDerivationMethod.None)
          this.WriteAttribute("final", string.Empty, this.GetEnumValue_XmlSchemaDerivationMethod(ob.Final));
        if (ob.FixedValue != null)
          this.WriteAttribute("fixed", string.Empty, ob.FixedValue);
        if (ob.Form != XmlSchemaForm.None)
          this.WriteAttribute("form", string.Empty, this.GetEnumValue_XmlSchemaForm(ob.Form));
        if (ob.Name != null)
          this.WriteAttribute("name", string.Empty, ob.Name);
        if (ob.IsNillable)
          this.WriteAttribute("nillable", string.Empty, !ob.IsNillable ? "false" : "true");
        this.WriteAttribute("ref", string.Empty, this.FromXmlQualifiedName(ob.RefName));
        this.WriteAttribute("substitutionGroup", string.Empty, this.FromXmlQualifiedName(ob.SubstitutionGroup));
        this.WriteAttribute("type", string.Empty, this.FromXmlQualifiedName(ob.SchemaTypeName));
        this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
        if (ob.SchemaType is XmlSchemaSimpleType)
          this.WriteObject_XmlSchemaSimpleType((XmlSchemaSimpleType) ob.SchemaType, "simpleType", "http://www.w3.org/2001/XMLSchema", false, false, true);
        else if (ob.SchemaType is XmlSchemaComplexType)
          this.WriteObject_XmlSchemaComplexType((XmlSchemaComplexType) ob.SchemaType, "complexType", "http://www.w3.org/2001/XMLSchema", false, false, true);
        if (ob.Constraints != null)
        {
          for (int index = 0; index < ob.Constraints.Count; ++index)
          {
            if (ob.Constraints[index] != null)
            {
              if (ob.Constraints[index].GetType() == typeof (XmlSchemaKeyref))
                this.WriteObject_XmlSchemaKeyref((XmlSchemaKeyref) ob.Constraints[index], "keyref", "http://www.w3.org/2001/XMLSchema", false, false, true);
              else if (ob.Constraints[index].GetType() == typeof (XmlSchemaKey))
              {
                this.WriteObject_XmlSchemaKey((XmlSchemaKey) ob.Constraints[index], "key", "http://www.w3.org/2001/XMLSchema", false, false, true);
              }
              else
              {
                if (ob.Constraints[index].GetType() != typeof (XmlSchemaUnique))
                  throw this.CreateUnknownTypeException((object) ob.Constraints[index]);
                this.WriteObject_XmlSchemaUnique((XmlSchemaUnique) ob.Constraints[index], "unique", "http://www.w3.org/2001/XMLSchema", false, false, true);
              }
            }
          }
        }
        if (!writeWrappingElem)
          return;
        this.WriteEndElement((object) ob);
      }
    }

    private void WriteObject_XmlSchemaSimpleType(
      XmlSchemaSimpleType ob,
      string element,
      string namesp,
      bool isNullable,
      bool needType,
      bool writeWrappingElem)
    {
      if (ob == null)
      {
        if (!isNullable)
          return;
        this.WriteNullTagLiteral(element, namesp);
      }
      else
      {
        if (ob.GetType() != typeof (XmlSchemaSimpleType))
          throw this.CreateUnknownTypeException((object) ob);
        if (writeWrappingElem)
          this.WriteStartElement(element, namesp, (object) ob);
        if (needType)
          this.WriteXsiType("XmlSchemaSimpleType", "http://www.w3.org/2001/XMLSchema");
        this.WriteNamespaceDeclarations(ob.Namespaces);
        ICollection unhandledAttributes = (ICollection) ob.UnhandledAttributes;
        if (unhandledAttributes != null)
        {
          foreach (XmlAttribute node in (IEnumerable) unhandledAttributes)
          {
            if (node.NamespaceURI != "http://www.w3.org/2000/xmlns/")
              this.WriteXmlAttribute((XmlNode) node, (object) ob);
          }
        }
        this.WriteAttribute("id", string.Empty, ob.Id);
        this.WriteAttribute("name", string.Empty, ob.Name);
        if (ob.Final != XmlSchemaDerivationMethod.None)
          this.WriteAttribute("final", string.Empty, this.GetEnumValue_XmlSchemaDerivationMethod(ob.Final));
        this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
        if (ob.Content is XmlSchemaSimpleTypeUnion)
          this.WriteObject_XmlSchemaSimpleTypeUnion((XmlSchemaSimpleTypeUnion) ob.Content, "union", "http://www.w3.org/2001/XMLSchema", false, false, true);
        else if (ob.Content is XmlSchemaSimpleTypeList)
          this.WriteObject_XmlSchemaSimpleTypeList((XmlSchemaSimpleTypeList) ob.Content, "list", "http://www.w3.org/2001/XMLSchema", false, false, true);
        else if (ob.Content is XmlSchemaSimpleTypeRestriction)
          this.WriteObject_XmlSchemaSimpleTypeRestriction((XmlSchemaSimpleTypeRestriction) ob.Content, "restriction", "http://www.w3.org/2001/XMLSchema", false, false, true);
        if (!writeWrappingElem)
          return;
        this.WriteEndElement((object) ob);
      }
    }

    private void WriteObject_XmlSchemaAttribute(
      XmlSchemaAttribute ob,
      string element,
      string namesp,
      bool isNullable,
      bool needType,
      bool writeWrappingElem)
    {
      if (ob == null)
      {
        if (!isNullable)
          return;
        this.WriteNullTagLiteral(element, namesp);
      }
      else
      {
        if (ob.GetType() != typeof (XmlSchemaAttribute))
          throw this.CreateUnknownTypeException((object) ob);
        if (writeWrappingElem)
          this.WriteStartElement(element, namesp, (object) ob);
        if (needType)
          this.WriteXsiType("XmlSchemaAttribute", "http://www.w3.org/2001/XMLSchema");
        this.WriteNamespaceDeclarations(ob.Namespaces);
        ICollection unhandledAttributes = (ICollection) ob.UnhandledAttributes;
        if (unhandledAttributes != null)
        {
          foreach (XmlAttribute node in (IEnumerable) unhandledAttributes)
          {
            if (node.NamespaceURI != "http://www.w3.org/2000/xmlns/")
              this.WriteXmlAttribute((XmlNode) node, (object) ob);
          }
        }
        this.WriteAttribute("id", string.Empty, ob.Id);
        if (ob.DefaultValue != null)
          this.WriteAttribute("default", string.Empty, ob.DefaultValue);
        if (ob.FixedValue != null)
          this.WriteAttribute("fixed", string.Empty, ob.FixedValue);
        if (ob.Form != XmlSchemaForm.None)
          this.WriteAttribute("form", string.Empty, this.GetEnumValue_XmlSchemaForm(ob.Form));
        this.WriteAttribute("name", string.Empty, ob.Name);
        this.WriteAttribute("ref", string.Empty, this.FromXmlQualifiedName(ob.RefName));
        this.WriteAttribute("type", string.Empty, this.FromXmlQualifiedName(ob.SchemaTypeName));
        if (ob.Use != XmlSchemaUse.None)
          this.WriteAttribute("use", string.Empty, this.GetEnumValue_XmlSchemaUse(ob.Use));
        this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
        this.WriteObject_XmlSchemaSimpleType(ob.SchemaType, "simpleType", "http://www.w3.org/2001/XMLSchema", false, false, true);
        if (!writeWrappingElem)
          return;
        this.WriteEndElement((object) ob);
      }
    }

    private void WriteObject_XmlSchemaAnnotation(
      XmlSchemaAnnotation ob,
      string element,
      string namesp,
      bool isNullable,
      bool needType,
      bool writeWrappingElem)
    {
      if (ob == null)
      {
        if (!isNullable)
          return;
        this.WriteNullTagLiteral(element, namesp);
      }
      else
      {
        if (ob.GetType() != typeof (XmlSchemaAnnotation))
          throw this.CreateUnknownTypeException((object) ob);
        if (writeWrappingElem)
          this.WriteStartElement(element, namesp, (object) ob);
        if (needType)
          this.WriteXsiType("XmlSchemaAnnotation", "http://www.w3.org/2001/XMLSchema");
        this.WriteNamespaceDeclarations(ob.Namespaces);
        ICollection unhandledAttributes = (ICollection) ob.UnhandledAttributes;
        if (unhandledAttributes != null)
        {
          foreach (XmlAttribute node in (IEnumerable) unhandledAttributes)
          {
            if (node.NamespaceURI != "http://www.w3.org/2000/xmlns/")
              this.WriteXmlAttribute((XmlNode) node, (object) ob);
          }
        }
        this.WriteAttribute("id", string.Empty, ob.Id);
        if (ob.Items != null)
        {
          for (int index = 0; index < ob.Items.Count; ++index)
          {
            if (ob.Items[index] != null)
            {
              if (ob.Items[index].GetType() == typeof (XmlSchemaAppInfo))
              {
                this.WriteObject_XmlSchemaAppInfo((XmlSchemaAppInfo) ob.Items[index], "appinfo", "http://www.w3.org/2001/XMLSchema", false, false, true);
              }
              else
              {
                if (ob.Items[index].GetType() != typeof (XmlSchemaDocumentation))
                  throw this.CreateUnknownTypeException((object) ob.Items[index]);
                this.WriteObject_XmlSchemaDocumentation((XmlSchemaDocumentation) ob.Items[index], "documentation", "http://www.w3.org/2001/XMLSchema", false, false, true);
              }
            }
          }
        }
        if (!writeWrappingElem)
          return;
        this.WriteEndElement((object) ob);
      }
    }

    private void WriteObject_XmlSchemaAttributeGroup(
      XmlSchemaAttributeGroup ob,
      string element,
      string namesp,
      bool isNullable,
      bool needType,
      bool writeWrappingElem)
    {
      if (ob == null)
      {
        if (!isNullable)
          return;
        this.WriteNullTagLiteral(element, namesp);
      }
      else
      {
        if (ob.GetType() != typeof (XmlSchemaAttributeGroup))
          throw this.CreateUnknownTypeException((object) ob);
        if (writeWrappingElem)
          this.WriteStartElement(element, namesp, (object) ob);
        if (needType)
          this.WriteXsiType("XmlSchemaAttributeGroup", "http://www.w3.org/2001/XMLSchema");
        this.WriteNamespaceDeclarations(ob.Namespaces);
        ICollection unhandledAttributes = (ICollection) ob.UnhandledAttributes;
        if (unhandledAttributes != null)
        {
          foreach (XmlAttribute node in (IEnumerable) unhandledAttributes)
          {
            if (node.NamespaceURI != "http://www.w3.org/2000/xmlns/")
              this.WriteXmlAttribute((XmlNode) node, (object) ob);
          }
        }
        this.WriteAttribute("id", string.Empty, ob.Id);
        this.WriteAttribute("name", string.Empty, ob.Name);
        this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
        if (ob.Attributes != null)
        {
          for (int index = 0; index < ob.Attributes.Count; ++index)
          {
            if (ob.Attributes[index] != null)
            {
              if (ob.Attributes[index].GetType() == typeof (XmlSchemaAttribute))
              {
                this.WriteObject_XmlSchemaAttribute((XmlSchemaAttribute) ob.Attributes[index], "attribute", "http://www.w3.org/2001/XMLSchema", false, false, true);
              }
              else
              {
                if (ob.Attributes[index].GetType() != typeof (XmlSchemaAttributeGroupRef))
                  throw this.CreateUnknownTypeException((object) ob.Attributes[index]);
                this.WriteObject_XmlSchemaAttributeGroupRef((XmlSchemaAttributeGroupRef) ob.Attributes[index], "attributeGroup", "http://www.w3.org/2001/XMLSchema", false, false, true);
              }
            }
          }
        }
        this.WriteObject_XmlSchemaAnyAttribute(ob.AnyAttribute, "anyAttribute", "http://www.w3.org/2001/XMLSchema", false, false, true);
        if (!writeWrappingElem)
          return;
        this.WriteEndElement((object) ob);
      }
    }

    private void WriteObject_XmlSchemaGroup(
      XmlSchemaGroup ob,
      string element,
      string namesp,
      bool isNullable,
      bool needType,
      bool writeWrappingElem)
    {
      if (ob == null)
      {
        if (!isNullable)
          return;
        this.WriteNullTagLiteral(element, namesp);
      }
      else
      {
        if (ob.GetType() != typeof (XmlSchemaGroup))
          throw this.CreateUnknownTypeException((object) ob);
        if (writeWrappingElem)
          this.WriteStartElement(element, namesp, (object) ob);
        if (needType)
          this.WriteXsiType("XmlSchemaGroup", "http://www.w3.org/2001/XMLSchema");
        this.WriteNamespaceDeclarations(ob.Namespaces);
        ICollection unhandledAttributes = (ICollection) ob.UnhandledAttributes;
        if (unhandledAttributes != null)
        {
          foreach (XmlAttribute node in (IEnumerable) unhandledAttributes)
          {
            if (node.NamespaceURI != "http://www.w3.org/2000/xmlns/")
              this.WriteXmlAttribute((XmlNode) node, (object) ob);
          }
        }
        this.WriteAttribute("id", string.Empty, ob.Id);
        this.WriteAttribute("name", string.Empty, ob.Name);
        this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
        if (ob.Particle is XmlSchemaSequence)
          this.WriteObject_XmlSchemaSequence((XmlSchemaSequence) ob.Particle, "sequence", "http://www.w3.org/2001/XMLSchema", false, false, true);
        else if (ob.Particle is XmlSchemaChoice)
          this.WriteObject_XmlSchemaChoice((XmlSchemaChoice) ob.Particle, "choice", "http://www.w3.org/2001/XMLSchema", false, false, true);
        else if (ob.Particle is XmlSchemaAll)
          this.WriteObject_XmlSchemaAll((XmlSchemaAll) ob.Particle, "all", "http://www.w3.org/2001/XMLSchema", false, false, true);
        if (!writeWrappingElem)
          return;
        this.WriteEndElement((object) ob);
      }
    }

    private void WriteObject_XmlSchemaComplexType(
      XmlSchemaComplexType ob,
      string element,
      string namesp,
      bool isNullable,
      bool needType,
      bool writeWrappingElem)
    {
      if (ob == null)
      {
        if (!isNullable)
          return;
        this.WriteNullTagLiteral(element, namesp);
      }
      else
      {
        if (ob.GetType() != typeof (XmlSchemaComplexType))
          throw this.CreateUnknownTypeException((object) ob);
        if (writeWrappingElem)
          this.WriteStartElement(element, namesp, (object) ob);
        if (needType)
          this.WriteXsiType("XmlSchemaComplexType", "http://www.w3.org/2001/XMLSchema");
        this.WriteNamespaceDeclarations(ob.Namespaces);
        ICollection unhandledAttributes = (ICollection) ob.UnhandledAttributes;
        if (unhandledAttributes != null)
        {
          foreach (XmlAttribute node in (IEnumerable) unhandledAttributes)
          {
            if (node.NamespaceURI != "http://www.w3.org/2000/xmlns/")
              this.WriteXmlAttribute((XmlNode) node, (object) ob);
          }
        }
        this.WriteAttribute("id", string.Empty, ob.Id);
        this.WriteAttribute("name", string.Empty, ob.Name);
        if (ob.Final != XmlSchemaDerivationMethod.None)
          this.WriteAttribute("final", string.Empty, this.GetEnumValue_XmlSchemaDerivationMethod(ob.Final));
        if (ob.IsAbstract)
          this.WriteAttribute("abstract", string.Empty, !ob.IsAbstract ? "false" : "true");
        if (ob.Block != XmlSchemaDerivationMethod.None)
          this.WriteAttribute("block", string.Empty, this.GetEnumValue_XmlSchemaDerivationMethod(ob.Block));
        if (ob.IsMixed)
          this.WriteAttribute("mixed", string.Empty, !ob.IsMixed ? "false" : "true");
        this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
        if (ob.ContentModel is XmlSchemaComplexContent)
          this.WriteObject_XmlSchemaComplexContent((XmlSchemaComplexContent) ob.ContentModel, "complexContent", "http://www.w3.org/2001/XMLSchema", false, false, true);
        else if (ob.ContentModel is XmlSchemaSimpleContent)
          this.WriteObject_XmlSchemaSimpleContent((XmlSchemaSimpleContent) ob.ContentModel, "simpleContent", "http://www.w3.org/2001/XMLSchema", false, false, true);
        if (ob.Particle is XmlSchemaAll)
          this.WriteObject_XmlSchemaAll((XmlSchemaAll) ob.Particle, "all", "http://www.w3.org/2001/XMLSchema", false, false, true);
        else if (ob.Particle is XmlSchemaGroupRef)
          this.WriteObject_XmlSchemaGroupRef((XmlSchemaGroupRef) ob.Particle, "group", "http://www.w3.org/2001/XMLSchema", false, false, true);
        else if (ob.Particle is XmlSchemaSequence)
          this.WriteObject_XmlSchemaSequence((XmlSchemaSequence) ob.Particle, "sequence", "http://www.w3.org/2001/XMLSchema", false, false, true);
        else if (ob.Particle is XmlSchemaChoice)
          this.WriteObject_XmlSchemaChoice((XmlSchemaChoice) ob.Particle, "choice", "http://www.w3.org/2001/XMLSchema", false, false, true);
        if (ob.Attributes != null)
        {
          for (int index = 0; index < ob.Attributes.Count; ++index)
          {
            if (ob.Attributes[index] != null)
            {
              if (ob.Attributes[index].GetType() == typeof (XmlSchemaAttributeGroupRef))
              {
                this.WriteObject_XmlSchemaAttributeGroupRef((XmlSchemaAttributeGroupRef) ob.Attributes[index], "attributeGroup", "http://www.w3.org/2001/XMLSchema", false, false, true);
              }
              else
              {
                if (ob.Attributes[index].GetType() != typeof (XmlSchemaAttribute))
                  throw this.CreateUnknownTypeException((object) ob.Attributes[index]);
                this.WriteObject_XmlSchemaAttribute((XmlSchemaAttribute) ob.Attributes[index], "attribute", "http://www.w3.org/2001/XMLSchema", false, false, true);
              }
            }
          }
        }
        this.WriteObject_XmlSchemaAnyAttribute(ob.AnyAttribute, "anyAttribute", "http://www.w3.org/2001/XMLSchema", false, false, true);
        if (!writeWrappingElem)
          return;
        this.WriteEndElement((object) ob);
      }
    }

    private void WriteObject_XmlSchemaNotation(
      XmlSchemaNotation ob,
      string element,
      string namesp,
      bool isNullable,
      bool needType,
      bool writeWrappingElem)
    {
      if (ob == null)
      {
        if (!isNullable)
          return;
        this.WriteNullTagLiteral(element, namesp);
      }
      else
      {
        if (ob.GetType() != typeof (XmlSchemaNotation))
          throw this.CreateUnknownTypeException((object) ob);
        if (writeWrappingElem)
          this.WriteStartElement(element, namesp, (object) ob);
        if (needType)
          this.WriteXsiType("XmlSchemaNotation", "http://www.w3.org/2001/XMLSchema");
        this.WriteNamespaceDeclarations(ob.Namespaces);
        ICollection unhandledAttributes = (ICollection) ob.UnhandledAttributes;
        if (unhandledAttributes != null)
        {
          foreach (XmlAttribute node in (IEnumerable) unhandledAttributes)
          {
            if (node.NamespaceURI != "http://www.w3.org/2000/xmlns/")
              this.WriteXmlAttribute((XmlNode) node, (object) ob);
          }
        }
        this.WriteAttribute("id", string.Empty, ob.Id);
        this.WriteAttribute("name", string.Empty, ob.Name);
        this.WriteAttribute("public", string.Empty, ob.Public);
        this.WriteAttribute("system", string.Empty, ob.System);
        this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
        if (!writeWrappingElem)
          return;
        this.WriteEndElement((object) ob);
      }
    }

    private void WriteObject_XmlSchemaKeyref(
      XmlSchemaKeyref ob,
      string element,
      string namesp,
      bool isNullable,
      bool needType,
      bool writeWrappingElem)
    {
      if (ob == null)
      {
        if (!isNullable)
          return;
        this.WriteNullTagLiteral(element, namesp);
      }
      else
      {
        if (ob.GetType() != typeof (XmlSchemaKeyref))
          throw this.CreateUnknownTypeException((object) ob);
        if (writeWrappingElem)
          this.WriteStartElement(element, namesp, (object) ob);
        if (needType)
          this.WriteXsiType("XmlSchemaKeyref", "http://www.w3.org/2001/XMLSchema");
        this.WriteNamespaceDeclarations(ob.Namespaces);
        ICollection unhandledAttributes = (ICollection) ob.UnhandledAttributes;
        if (unhandledAttributes != null)
        {
          foreach (XmlAttribute node in (IEnumerable) unhandledAttributes)
          {
            if (node.NamespaceURI != "http://www.w3.org/2000/xmlns/")
              this.WriteXmlAttribute((XmlNode) node, (object) ob);
          }
        }
        this.WriteAttribute("id", string.Empty, ob.Id);
        this.WriteAttribute("name", string.Empty, ob.Name);
        this.WriteAttribute("refer", string.Empty, this.FromXmlQualifiedName(ob.Refer));
        this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
        this.WriteObject_XmlSchemaXPath(ob.Selector, "selector", "http://www.w3.org/2001/XMLSchema", false, false, true);
        if (ob.Fields != null)
        {
          for (int index = 0; index < ob.Fields.Count; ++index)
            this.WriteObject_XmlSchemaXPath((XmlSchemaXPath) ob.Fields[index], "field", "http://www.w3.org/2001/XMLSchema", false, false, true);
        }
        if (!writeWrappingElem)
          return;
        this.WriteEndElement((object) ob);
      }
    }

    private void WriteObject_XmlSchemaKey(
      XmlSchemaKey ob,
      string element,
      string namesp,
      bool isNullable,
      bool needType,
      bool writeWrappingElem)
    {
      if (ob == null)
      {
        if (!isNullable)
          return;
        this.WriteNullTagLiteral(element, namesp);
      }
      else
      {
        if (ob.GetType() != typeof (XmlSchemaKey))
          throw this.CreateUnknownTypeException((object) ob);
        if (writeWrappingElem)
          this.WriteStartElement(element, namesp, (object) ob);
        if (needType)
          this.WriteXsiType("XmlSchemaKey", "http://www.w3.org/2001/XMLSchema");
        this.WriteNamespaceDeclarations(ob.Namespaces);
        ICollection unhandledAttributes = (ICollection) ob.UnhandledAttributes;
        if (unhandledAttributes != null)
        {
          foreach (XmlAttribute node in (IEnumerable) unhandledAttributes)
          {
            if (node.NamespaceURI != "http://www.w3.org/2000/xmlns/")
              this.WriteXmlAttribute((XmlNode) node, (object) ob);
          }
        }
        this.WriteAttribute("id", string.Empty, ob.Id);
        this.WriteAttribute("name", string.Empty, ob.Name);
        this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
        this.WriteObject_XmlSchemaXPath(ob.Selector, "selector", "http://www.w3.org/2001/XMLSchema", false, false, true);
        if (ob.Fields != null)
        {
          for (int index = 0; index < ob.Fields.Count; ++index)
            this.WriteObject_XmlSchemaXPath((XmlSchemaXPath) ob.Fields[index], "field", "http://www.w3.org/2001/XMLSchema", false, false, true);
        }
        if (!writeWrappingElem)
          return;
        this.WriteEndElement((object) ob);
      }
    }

    private void WriteObject_XmlSchemaUnique(
      XmlSchemaUnique ob,
      string element,
      string namesp,
      bool isNullable,
      bool needType,
      bool writeWrappingElem)
    {
      if (ob == null)
      {
        if (!isNullable)
          return;
        this.WriteNullTagLiteral(element, namesp);
      }
      else
      {
        if (ob.GetType() != typeof (XmlSchemaUnique))
          throw this.CreateUnknownTypeException((object) ob);
        if (writeWrappingElem)
          this.WriteStartElement(element, namesp, (object) ob);
        if (needType)
          this.WriteXsiType("XmlSchemaUnique", "http://www.w3.org/2001/XMLSchema");
        this.WriteNamespaceDeclarations(ob.Namespaces);
        ICollection unhandledAttributes = (ICollection) ob.UnhandledAttributes;
        if (unhandledAttributes != null)
        {
          foreach (XmlAttribute node in (IEnumerable) unhandledAttributes)
          {
            if (node.NamespaceURI != "http://www.w3.org/2000/xmlns/")
              this.WriteXmlAttribute((XmlNode) node, (object) ob);
          }
        }
        this.WriteAttribute("id", string.Empty, ob.Id);
        this.WriteAttribute("name", string.Empty, ob.Name);
        this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
        this.WriteObject_XmlSchemaXPath(ob.Selector, "selector", "http://www.w3.org/2001/XMLSchema", false, false, true);
        if (ob.Fields != null)
        {
          for (int index = 0; index < ob.Fields.Count; ++index)
            this.WriteObject_XmlSchemaXPath((XmlSchemaXPath) ob.Fields[index], "field", "http://www.w3.org/2001/XMLSchema", false, false, true);
        }
        if (!writeWrappingElem)
          return;
        this.WriteEndElement((object) ob);
      }
    }

    private void WriteObject_XmlSchemaSimpleTypeUnion(
      XmlSchemaSimpleTypeUnion ob,
      string element,
      string namesp,
      bool isNullable,
      bool needType,
      bool writeWrappingElem)
    {
      if (ob == null)
      {
        if (!isNullable)
          return;
        this.WriteNullTagLiteral(element, namesp);
      }
      else
      {
        if (ob.GetType() != typeof (XmlSchemaSimpleTypeUnion))
          throw this.CreateUnknownTypeException((object) ob);
        if (writeWrappingElem)
          this.WriteStartElement(element, namesp, (object) ob);
        if (needType)
          this.WriteXsiType("XmlSchemaSimpleTypeUnion", "http://www.w3.org/2001/XMLSchema");
        this.WriteNamespaceDeclarations(ob.Namespaces);
        ICollection unhandledAttributes = (ICollection) ob.UnhandledAttributes;
        if (unhandledAttributes != null)
        {
          foreach (XmlAttribute node in (IEnumerable) unhandledAttributes)
          {
            if (node.NamespaceURI != "http://www.w3.org/2000/xmlns/")
              this.WriteXmlAttribute((XmlNode) node, (object) ob);
          }
        }
        this.WriteAttribute("id", string.Empty, ob.Id);
        string str = (string) null;
        if (ob.MemberTypes != null)
        {
          StringBuilder stringBuilder = new StringBuilder();
          for (int index = 0; index < ob.MemberTypes.Length; ++index)
            stringBuilder.Append(this.FromXmlQualifiedName(ob.MemberTypes[index])).Append(" ");
          str = stringBuilder.ToString().Trim();
        }
        this.WriteAttribute("memberTypes", string.Empty, str);
        this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
        if (ob.BaseTypes != null)
        {
          for (int index = 0; index < ob.BaseTypes.Count; ++index)
            this.WriteObject_XmlSchemaSimpleType((XmlSchemaSimpleType) ob.BaseTypes[index], "simpleType", "http://www.w3.org/2001/XMLSchema", false, false, true);
        }
        if (!writeWrappingElem)
          return;
        this.WriteEndElement((object) ob);
      }
    }

    private void WriteObject_XmlSchemaSimpleTypeList(
      XmlSchemaSimpleTypeList ob,
      string element,
      string namesp,
      bool isNullable,
      bool needType,
      bool writeWrappingElem)
    {
      if (ob == null)
      {
        if (!isNullable)
          return;
        this.WriteNullTagLiteral(element, namesp);
      }
      else
      {
        if (ob.GetType() != typeof (XmlSchemaSimpleTypeList))
          throw this.CreateUnknownTypeException((object) ob);
        if (writeWrappingElem)
          this.WriteStartElement(element, namesp, (object) ob);
        if (needType)
          this.WriteXsiType("XmlSchemaSimpleTypeList", "http://www.w3.org/2001/XMLSchema");
        this.WriteNamespaceDeclarations(ob.Namespaces);
        ICollection unhandledAttributes = (ICollection) ob.UnhandledAttributes;
        if (unhandledAttributes != null)
        {
          foreach (XmlAttribute node in (IEnumerable) unhandledAttributes)
          {
            if (node.NamespaceURI != "http://www.w3.org/2000/xmlns/")
              this.WriteXmlAttribute((XmlNode) node, (object) ob);
          }
        }
        this.WriteAttribute("id", string.Empty, ob.Id);
        this.WriteAttribute("itemType", string.Empty, this.FromXmlQualifiedName(ob.ItemTypeName));
        this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
        this.WriteObject_XmlSchemaSimpleType(ob.ItemType, "simpleType", "http://www.w3.org/2001/XMLSchema", false, false, true);
        if (!writeWrappingElem)
          return;
        this.WriteEndElement((object) ob);
      }
    }

    private void WriteObject_XmlSchemaSimpleTypeRestriction(
      XmlSchemaSimpleTypeRestriction ob,
      string element,
      string namesp,
      bool isNullable,
      bool needType,
      bool writeWrappingElem)
    {
      if (ob == null)
      {
        if (!isNullable)
          return;
        this.WriteNullTagLiteral(element, namesp);
      }
      else
      {
        if (ob.GetType() != typeof (XmlSchemaSimpleTypeRestriction))
          throw this.CreateUnknownTypeException((object) ob);
        if (writeWrappingElem)
          this.WriteStartElement(element, namesp, (object) ob);
        if (needType)
          this.WriteXsiType("XmlSchemaSimpleTypeRestriction", "http://www.w3.org/2001/XMLSchema");
        this.WriteNamespaceDeclarations(ob.Namespaces);
        ICollection unhandledAttributes = (ICollection) ob.UnhandledAttributes;
        if (unhandledAttributes != null)
        {
          foreach (XmlAttribute node in (IEnumerable) unhandledAttributes)
          {
            if (node.NamespaceURI != "http://www.w3.org/2000/xmlns/")
              this.WriteXmlAttribute((XmlNode) node, (object) ob);
          }
        }
        this.WriteAttribute("id", string.Empty, ob.Id);
        this.WriteAttribute("base", string.Empty, this.FromXmlQualifiedName(ob.BaseTypeName));
        this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
        this.WriteObject_XmlSchemaSimpleType(ob.BaseType, "simpleType", "http://www.w3.org/2001/XMLSchema", false, false, true);
        if (ob.Facets != null)
        {
          for (int index = 0; index < ob.Facets.Count; ++index)
          {
            if (ob.Facets[index] != null)
            {
              if (ob.Facets[index].GetType() == typeof (XmlSchemaMaxLengthFacet))
                this.WriteObject_XmlSchemaMaxLengthFacet((XmlSchemaMaxLengthFacet) ob.Facets[index], "maxLength", "http://www.w3.org/2001/XMLSchema", false, false, true);
              else if (ob.Facets[index].GetType() == typeof (XmlSchemaMinLengthFacet))
                this.WriteObject_XmlSchemaMinLengthFacet((XmlSchemaMinLengthFacet) ob.Facets[index], "minLength", "http://www.w3.org/2001/XMLSchema", false, false, true);
              else if (ob.Facets[index].GetType() == typeof (XmlSchemaLengthFacet))
                this.WriteObject_XmlSchemaLengthFacet((XmlSchemaLengthFacet) ob.Facets[index], "length", "http://www.w3.org/2001/XMLSchema", false, false, true);
              else if (ob.Facets[index].GetType() == typeof (XmlSchemaFractionDigitsFacet))
                this.WriteObject_XmlSchemaFractionDigitsFacet((XmlSchemaFractionDigitsFacet) ob.Facets[index], "fractionDigits", "http://www.w3.org/2001/XMLSchema", false, false, true);
              else if (ob.Facets[index].GetType() == typeof (XmlSchemaMaxInclusiveFacet))
                this.WriteObject_XmlSchemaMaxInclusiveFacet((XmlSchemaMaxInclusiveFacet) ob.Facets[index], "maxInclusive", "http://www.w3.org/2001/XMLSchema", false, false, true);
              else if (ob.Facets[index].GetType() == typeof (XmlSchemaMaxExclusiveFacet))
                this.WriteObject_XmlSchemaMaxExclusiveFacet((XmlSchemaMaxExclusiveFacet) ob.Facets[index], "maxExclusive", "http://www.w3.org/2001/XMLSchema", false, false, true);
              else if (ob.Facets[index].GetType() == typeof (XmlSchemaMinExclusiveFacet))
                this.WriteObject_XmlSchemaMinExclusiveFacet((XmlSchemaMinExclusiveFacet) ob.Facets[index], "minExclusive", "http://www.w3.org/2001/XMLSchema", false, false, true);
              else if (ob.Facets[index].GetType() == typeof (XmlSchemaEnumerationFacet))
                this.WriteObject_XmlSchemaEnumerationFacet((XmlSchemaEnumerationFacet) ob.Facets[index], "enumeration", "http://www.w3.org/2001/XMLSchema", false, false, true);
              else if (ob.Facets[index].GetType() == typeof (XmlSchemaTotalDigitsFacet))
                this.WriteObject_XmlSchemaTotalDigitsFacet((XmlSchemaTotalDigitsFacet) ob.Facets[index], "totalDigits", "http://www.w3.org/2001/XMLSchema", false, false, true);
              else if (ob.Facets[index].GetType() == typeof (XmlSchemaMinInclusiveFacet))
                this.WriteObject_XmlSchemaMinInclusiveFacet((XmlSchemaMinInclusiveFacet) ob.Facets[index], "minInclusive", "http://www.w3.org/2001/XMLSchema", false, false, true);
              else if (ob.Facets[index].GetType() == typeof (XmlSchemaWhiteSpaceFacet))
              {
                this.WriteObject_XmlSchemaWhiteSpaceFacet((XmlSchemaWhiteSpaceFacet) ob.Facets[index], "whiteSpace", "http://www.w3.org/2001/XMLSchema", false, false, true);
              }
              else
              {
                if (ob.Facets[index].GetType() != typeof (XmlSchemaPatternFacet))
                  throw this.CreateUnknownTypeException((object) ob.Facets[index]);
                this.WriteObject_XmlSchemaPatternFacet((XmlSchemaPatternFacet) ob.Facets[index], "pattern", "http://www.w3.org/2001/XMLSchema", false, false, true);
              }
            }
          }
        }
        if (!writeWrappingElem)
          return;
        this.WriteEndElement((object) ob);
      }
    }

    private string GetEnumValue_XmlSchemaUse(XmlSchemaUse val)
    {
      switch (val)
      {
        case XmlSchemaUse.Optional:
          return "optional";
        case XmlSchemaUse.Prohibited:
          return "prohibited";
        case XmlSchemaUse.Required:
          return "required";
        default:
          return ((long) val).ToString((IFormatProvider) CultureInfo.InvariantCulture);
      }
    }

    private void WriteObject_XmlSchemaAppInfo(
      XmlSchemaAppInfo ob,
      string element,
      string namesp,
      bool isNullable,
      bool needType,
      bool writeWrappingElem)
    {
      if (ob == null)
      {
        if (!isNullable)
          return;
        this.WriteNullTagLiteral(element, namesp);
      }
      else
      {
        if (ob.GetType() != typeof (XmlSchemaAppInfo))
          throw this.CreateUnknownTypeException((object) ob);
        if (writeWrappingElem)
          this.WriteStartElement(element, namesp, (object) ob);
        if (needType)
          this.WriteXsiType("XmlSchemaAppInfo", "http://www.w3.org/2001/XMLSchema");
        this.WriteNamespaceDeclarations(ob.Namespaces);
        this.WriteAttribute("source", string.Empty, ob.Source);
        if (ob.Markup != null)
        {
          foreach (XmlNode node in ob.Markup)
          {
            if (node is XmlElement)
              this.WriteElementLiteral(node, string.Empty, string.Empty, false, true);
            else
              node.WriteTo(this.Writer);
          }
        }
        if (!writeWrappingElem)
          return;
        this.WriteEndElement((object) ob);
      }
    }

    private void WriteObject_XmlSchemaDocumentation(
      XmlSchemaDocumentation ob,
      string element,
      string namesp,
      bool isNullable,
      bool needType,
      bool writeWrappingElem)
    {
      if (ob == null)
      {
        if (!isNullable)
          return;
        this.WriteNullTagLiteral(element, namesp);
      }
      else
      {
        if (ob.GetType() != typeof (XmlSchemaDocumentation))
          throw this.CreateUnknownTypeException((object) ob);
        if (writeWrappingElem)
          this.WriteStartElement(element, namesp, (object) ob);
        if (needType)
          this.WriteXsiType("XmlSchemaDocumentation", "http://www.w3.org/2001/XMLSchema");
        this.WriteNamespaceDeclarations(ob.Namespaces);
        this.WriteAttribute("source", string.Empty, ob.Source);
        this.WriteAttribute("xml:lang", string.Empty, ob.Language);
        if (ob.Markup != null)
        {
          foreach (XmlNode node in ob.Markup)
          {
            if (node is XmlElement)
              this.WriteElementLiteral(node, string.Empty, string.Empty, false, true);
            else
              node.WriteTo(this.Writer);
          }
        }
        if (!writeWrappingElem)
          return;
        this.WriteEndElement((object) ob);
      }
    }

    private void WriteObject_XmlSchemaAttributeGroupRef(
      XmlSchemaAttributeGroupRef ob,
      string element,
      string namesp,
      bool isNullable,
      bool needType,
      bool writeWrappingElem)
    {
      if (ob == null)
      {
        if (!isNullable)
          return;
        this.WriteNullTagLiteral(element, namesp);
      }
      else
      {
        if (ob.GetType() != typeof (XmlSchemaAttributeGroupRef))
          throw this.CreateUnknownTypeException((object) ob);
        if (writeWrappingElem)
          this.WriteStartElement(element, namesp, (object) ob);
        if (needType)
          this.WriteXsiType("XmlSchemaAttributeGroupRef", "http://www.w3.org/2001/XMLSchema");
        this.WriteNamespaceDeclarations(ob.Namespaces);
        ICollection unhandledAttributes = (ICollection) ob.UnhandledAttributes;
        if (unhandledAttributes != null)
        {
          foreach (XmlAttribute node in (IEnumerable) unhandledAttributes)
          {
            if (node.NamespaceURI != "http://www.w3.org/2000/xmlns/")
              this.WriteXmlAttribute((XmlNode) node, (object) ob);
          }
        }
        this.WriteAttribute("id", string.Empty, ob.Id);
        this.WriteAttribute("ref", string.Empty, this.FromXmlQualifiedName(ob.RefName));
        this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
        if (!writeWrappingElem)
          return;
        this.WriteEndElement((object) ob);
      }
    }

    private void WriteObject_XmlSchemaAnyAttribute(
      XmlSchemaAnyAttribute ob,
      string element,
      string namesp,
      bool isNullable,
      bool needType,
      bool writeWrappingElem)
    {
      if (ob == null)
      {
        if (!isNullable)
          return;
        this.WriteNullTagLiteral(element, namesp);
      }
      else
      {
        if (ob.GetType() != typeof (XmlSchemaAnyAttribute))
          throw this.CreateUnknownTypeException((object) ob);
        if (writeWrappingElem)
          this.WriteStartElement(element, namesp, (object) ob);
        if (needType)
          this.WriteXsiType("XmlSchemaAnyAttribute", "http://www.w3.org/2001/XMLSchema");
        this.WriteNamespaceDeclarations(ob.Namespaces);
        ICollection unhandledAttributes = (ICollection) ob.UnhandledAttributes;
        if (unhandledAttributes != null)
        {
          foreach (XmlAttribute node in (IEnumerable) unhandledAttributes)
          {
            if (node.NamespaceURI != "http://www.w3.org/2000/xmlns/")
              this.WriteXmlAttribute((XmlNode) node, (object) ob);
          }
        }
        this.WriteAttribute("id", string.Empty, ob.Id);
        this.WriteAttribute("namespace", string.Empty, ob.Namespace);
        if (ob.ProcessContents != XmlSchemaContentProcessing.None)
          this.WriteAttribute("processContents", string.Empty, this.GetEnumValue_XmlSchemaContentProcessing(ob.ProcessContents));
        this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
        if (!writeWrappingElem)
          return;
        this.WriteEndElement((object) ob);
      }
    }

    private void WriteObject_XmlSchemaSequence(
      XmlSchemaSequence ob,
      string element,
      string namesp,
      bool isNullable,
      bool needType,
      bool writeWrappingElem)
    {
      if (ob == null)
      {
        if (!isNullable)
          return;
        this.WriteNullTagLiteral(element, namesp);
      }
      else
      {
        if (ob.GetType() != typeof (XmlSchemaSequence))
          throw this.CreateUnknownTypeException((object) ob);
        if (writeWrappingElem)
          this.WriteStartElement(element, namesp, (object) ob);
        if (needType)
          this.WriteXsiType("XmlSchemaSequence", "http://www.w3.org/2001/XMLSchema");
        this.WriteNamespaceDeclarations(ob.Namespaces);
        ICollection unhandledAttributes = (ICollection) ob.UnhandledAttributes;
        if (unhandledAttributes != null)
        {
          foreach (XmlAttribute node in (IEnumerable) unhandledAttributes)
          {
            if (node.NamespaceURI != "http://www.w3.org/2000/xmlns/")
              this.WriteXmlAttribute((XmlNode) node, (object) ob);
          }
        }
        this.WriteAttribute("id", string.Empty, ob.Id);
        this.WriteAttribute("minOccurs", string.Empty, ob.MinOccursString);
        this.WriteAttribute("maxOccurs", string.Empty, ob.MaxOccursString);
        this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
        if (ob.Items != null)
        {
          for (int index = 0; index < ob.Items.Count; ++index)
          {
            if (ob.Items[index] != null)
            {
              if (ob.Items[index].GetType() == typeof (XmlSchemaSequence))
                this.WriteObject_XmlSchemaSequence((XmlSchemaSequence) ob.Items[index], "sequence", "http://www.w3.org/2001/XMLSchema", false, false, true);
              else if (ob.Items[index].GetType() == typeof (XmlSchemaChoice))
                this.WriteObject_XmlSchemaChoice((XmlSchemaChoice) ob.Items[index], "choice", "http://www.w3.org/2001/XMLSchema", false, false, true);
              else if (ob.Items[index].GetType() == typeof (XmlSchemaGroupRef))
                this.WriteObject_XmlSchemaGroupRef((XmlSchemaGroupRef) ob.Items[index], "group", "http://www.w3.org/2001/XMLSchema", false, false, true);
              else if (ob.Items[index].GetType() == typeof (XmlSchemaElement))
              {
                this.WriteObject_XmlSchemaElement((XmlSchemaElement) ob.Items[index], nameof (element), "http://www.w3.org/2001/XMLSchema", false, false, true);
              }
              else
              {
                if (ob.Items[index].GetType() != typeof (XmlSchemaAny))
                  throw this.CreateUnknownTypeException((object) ob.Items[index]);
                this.WriteObject_XmlSchemaAny((XmlSchemaAny) ob.Items[index], "any", "http://www.w3.org/2001/XMLSchema", false, false, true);
              }
            }
          }
        }
        if (!writeWrappingElem)
          return;
        this.WriteEndElement((object) ob);
      }
    }

    private void WriteObject_XmlSchemaChoice(
      XmlSchemaChoice ob,
      string element,
      string namesp,
      bool isNullable,
      bool needType,
      bool writeWrappingElem)
    {
      if (ob == null)
      {
        if (!isNullable)
          return;
        this.WriteNullTagLiteral(element, namesp);
      }
      else
      {
        if (ob.GetType() != typeof (XmlSchemaChoice))
          throw this.CreateUnknownTypeException((object) ob);
        if (writeWrappingElem)
          this.WriteStartElement(element, namesp, (object) ob);
        if (needType)
          this.WriteXsiType("XmlSchemaChoice", "http://www.w3.org/2001/XMLSchema");
        this.WriteNamespaceDeclarations(ob.Namespaces);
        ICollection unhandledAttributes = (ICollection) ob.UnhandledAttributes;
        if (unhandledAttributes != null)
        {
          foreach (XmlAttribute node in (IEnumerable) unhandledAttributes)
          {
            if (node.NamespaceURI != "http://www.w3.org/2000/xmlns/")
              this.WriteXmlAttribute((XmlNode) node, (object) ob);
          }
        }
        this.WriteAttribute("id", string.Empty, ob.Id);
        this.WriteAttribute("minOccurs", string.Empty, ob.MinOccursString);
        this.WriteAttribute("maxOccurs", string.Empty, ob.MaxOccursString);
        this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
        if (ob.Items != null)
        {
          for (int index = 0; index < ob.Items.Count; ++index)
          {
            if (ob.Items[index] != null)
            {
              if (ob.Items[index].GetType() == typeof (XmlSchemaGroupRef))
                this.WriteObject_XmlSchemaGroupRef((XmlSchemaGroupRef) ob.Items[index], "group", "http://www.w3.org/2001/XMLSchema", false, false, true);
              else if (ob.Items[index].GetType() == typeof (XmlSchemaElement))
                this.WriteObject_XmlSchemaElement((XmlSchemaElement) ob.Items[index], nameof (element), "http://www.w3.org/2001/XMLSchema", false, false, true);
              else if (ob.Items[index].GetType() == typeof (XmlSchemaAny))
                this.WriteObject_XmlSchemaAny((XmlSchemaAny) ob.Items[index], "any", "http://www.w3.org/2001/XMLSchema", false, false, true);
              else if (ob.Items[index].GetType() == typeof (XmlSchemaSequence))
              {
                this.WriteObject_XmlSchemaSequence((XmlSchemaSequence) ob.Items[index], "sequence", "http://www.w3.org/2001/XMLSchema", false, false, true);
              }
              else
              {
                if (ob.Items[index].GetType() != typeof (XmlSchemaChoice))
                  throw this.CreateUnknownTypeException((object) ob.Items[index]);
                this.WriteObject_XmlSchemaChoice((XmlSchemaChoice) ob.Items[index], "choice", "http://www.w3.org/2001/XMLSchema", false, false, true);
              }
            }
          }
        }
        if (!writeWrappingElem)
          return;
        this.WriteEndElement((object) ob);
      }
    }

    private void WriteObject_XmlSchemaAll(
      XmlSchemaAll ob,
      string element,
      string namesp,
      bool isNullable,
      bool needType,
      bool writeWrappingElem)
    {
      if (ob == null)
      {
        if (!isNullable)
          return;
        this.WriteNullTagLiteral(element, namesp);
      }
      else
      {
        if (ob.GetType() != typeof (XmlSchemaAll))
          throw this.CreateUnknownTypeException((object) ob);
        if (writeWrappingElem)
          this.WriteStartElement(element, namesp, (object) ob);
        if (needType)
          this.WriteXsiType("XmlSchemaAll", "http://www.w3.org/2001/XMLSchema");
        this.WriteNamespaceDeclarations(ob.Namespaces);
        ICollection unhandledAttributes = (ICollection) ob.UnhandledAttributes;
        if (unhandledAttributes != null)
        {
          foreach (XmlAttribute node in (IEnumerable) unhandledAttributes)
          {
            if (node.NamespaceURI != "http://www.w3.org/2000/xmlns/")
              this.WriteXmlAttribute((XmlNode) node, (object) ob);
          }
        }
        this.WriteAttribute("id", string.Empty, ob.Id);
        this.WriteAttribute("minOccurs", string.Empty, ob.MinOccursString);
        this.WriteAttribute("maxOccurs", string.Empty, ob.MaxOccursString);
        this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
        if (ob.Items != null)
        {
          for (int index = 0; index < ob.Items.Count; ++index)
            this.WriteObject_XmlSchemaElement((XmlSchemaElement) ob.Items[index], nameof (element), "http://www.w3.org/2001/XMLSchema", false, false, true);
        }
        if (!writeWrappingElem)
          return;
        this.WriteEndElement((object) ob);
      }
    }

    private void WriteObject_XmlSchemaComplexContent(
      XmlSchemaComplexContent ob,
      string element,
      string namesp,
      bool isNullable,
      bool needType,
      bool writeWrappingElem)
    {
      if (ob == null)
      {
        if (!isNullable)
          return;
        this.WriteNullTagLiteral(element, namesp);
      }
      else
      {
        if (ob.GetType() != typeof (XmlSchemaComplexContent))
          throw this.CreateUnknownTypeException((object) ob);
        if (writeWrappingElem)
          this.WriteStartElement(element, namesp, (object) ob);
        if (needType)
          this.WriteXsiType("XmlSchemaComplexContent", "http://www.w3.org/2001/XMLSchema");
        this.WriteNamespaceDeclarations(ob.Namespaces);
        ICollection unhandledAttributes = (ICollection) ob.UnhandledAttributes;
        if (unhandledAttributes != null)
        {
          foreach (XmlAttribute node in (IEnumerable) unhandledAttributes)
          {
            if (node.NamespaceURI != "http://www.w3.org/2000/xmlns/")
              this.WriteXmlAttribute((XmlNode) node, (object) ob);
          }
        }
        this.WriteAttribute("id", string.Empty, ob.Id);
        this.WriteAttribute("mixed", string.Empty, !ob.IsMixed ? "false" : "true");
        this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
        if (ob.Content is XmlSchemaComplexContentExtension)
          this.WriteObject_XmlSchemaComplexContentExtension((XmlSchemaComplexContentExtension) ob.Content, "extension", "http://www.w3.org/2001/XMLSchema", false, false, true);
        else if (ob.Content is XmlSchemaComplexContentRestriction)
          this.WriteObject_XmlSchemaComplexContentRestriction((XmlSchemaComplexContentRestriction) ob.Content, "restriction", "http://www.w3.org/2001/XMLSchema", false, false, true);
        if (!writeWrappingElem)
          return;
        this.WriteEndElement((object) ob);
      }
    }

    private void WriteObject_XmlSchemaSimpleContent(
      XmlSchemaSimpleContent ob,
      string element,
      string namesp,
      bool isNullable,
      bool needType,
      bool writeWrappingElem)
    {
      if (ob == null)
      {
        if (!isNullable)
          return;
        this.WriteNullTagLiteral(element, namesp);
      }
      else
      {
        if (ob.GetType() != typeof (XmlSchemaSimpleContent))
          throw this.CreateUnknownTypeException((object) ob);
        if (writeWrappingElem)
          this.WriteStartElement(element, namesp, (object) ob);
        if (needType)
          this.WriteXsiType("XmlSchemaSimpleContent", "http://www.w3.org/2001/XMLSchema");
        this.WriteNamespaceDeclarations(ob.Namespaces);
        ICollection unhandledAttributes = (ICollection) ob.UnhandledAttributes;
        if (unhandledAttributes != null)
        {
          foreach (XmlAttribute node in (IEnumerable) unhandledAttributes)
          {
            if (node.NamespaceURI != "http://www.w3.org/2000/xmlns/")
              this.WriteXmlAttribute((XmlNode) node, (object) ob);
          }
        }
        this.WriteAttribute("id", string.Empty, ob.Id);
        this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
        if (ob.Content is XmlSchemaSimpleContentExtension)
          this.WriteObject_XmlSchemaSimpleContentExtension((XmlSchemaSimpleContentExtension) ob.Content, "extension", "http://www.w3.org/2001/XMLSchema", false, false, true);
        else if (ob.Content is XmlSchemaSimpleContentRestriction)
          this.WriteObject_XmlSchemaSimpleContentRestriction((XmlSchemaSimpleContentRestriction) ob.Content, "restriction", "http://www.w3.org/2001/XMLSchema", false, false, true);
        if (!writeWrappingElem)
          return;
        this.WriteEndElement((object) ob);
      }
    }

    private void WriteObject_XmlSchemaGroupRef(
      XmlSchemaGroupRef ob,
      string element,
      string namesp,
      bool isNullable,
      bool needType,
      bool writeWrappingElem)
    {
      if (ob == null)
      {
        if (!isNullable)
          return;
        this.WriteNullTagLiteral(element, namesp);
      }
      else
      {
        if (ob.GetType() != typeof (XmlSchemaGroupRef))
          throw this.CreateUnknownTypeException((object) ob);
        if (writeWrappingElem)
          this.WriteStartElement(element, namesp, (object) ob);
        if (needType)
          this.WriteXsiType("XmlSchemaGroupRef", "http://www.w3.org/2001/XMLSchema");
        this.WriteNamespaceDeclarations(ob.Namespaces);
        ICollection unhandledAttributes = (ICollection) ob.UnhandledAttributes;
        if (unhandledAttributes != null)
        {
          foreach (XmlAttribute node in (IEnumerable) unhandledAttributes)
          {
            if (node.NamespaceURI != "http://www.w3.org/2000/xmlns/")
              this.WriteXmlAttribute((XmlNode) node, (object) ob);
          }
        }
        this.WriteAttribute("id", string.Empty, ob.Id);
        this.WriteAttribute("minOccurs", string.Empty, ob.MinOccursString);
        this.WriteAttribute("maxOccurs", string.Empty, ob.MaxOccursString);
        this.WriteAttribute("ref", string.Empty, this.FromXmlQualifiedName(ob.RefName));
        this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
        if (!writeWrappingElem)
          return;
        this.WriteEndElement((object) ob);
      }
    }

    private void WriteObject_XmlSchemaXPath(
      XmlSchemaXPath ob,
      string element,
      string namesp,
      bool isNullable,
      bool needType,
      bool writeWrappingElem)
    {
      if (ob == null)
      {
        if (!isNullable)
          return;
        this.WriteNullTagLiteral(element, namesp);
      }
      else
      {
        if (ob.GetType() != typeof (XmlSchemaXPath))
          throw this.CreateUnknownTypeException((object) ob);
        if (writeWrappingElem)
          this.WriteStartElement(element, namesp, (object) ob);
        if (needType)
          this.WriteXsiType("XmlSchemaXPath", "http://www.w3.org/2001/XMLSchema");
        this.WriteNamespaceDeclarations(ob.Namespaces);
        ICollection unhandledAttributes = (ICollection) ob.UnhandledAttributes;
        if (unhandledAttributes != null)
        {
          foreach (XmlAttribute node in (IEnumerable) unhandledAttributes)
          {
            if (node.NamespaceURI != "http://www.w3.org/2000/xmlns/")
              this.WriteXmlAttribute((XmlNode) node, (object) ob);
          }
        }
        this.WriteAttribute("id", string.Empty, ob.Id);
        if (ob.XPath != null)
          this.WriteAttribute("xpath", string.Empty, ob.XPath);
        this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
        if (!writeWrappingElem)
          return;
        this.WriteEndElement((object) ob);
      }
    }

    private void WriteObject_XmlSchemaMaxLengthFacet(
      XmlSchemaMaxLengthFacet ob,
      string element,
      string namesp,
      bool isNullable,
      bool needType,
      bool writeWrappingElem)
    {
      if (ob == null)
      {
        if (!isNullable)
          return;
        this.WriteNullTagLiteral(element, namesp);
      }
      else
      {
        if (ob.GetType() != typeof (XmlSchemaMaxLengthFacet))
          throw this.CreateUnknownTypeException((object) ob);
        if (writeWrappingElem)
          this.WriteStartElement(element, namesp, (object) ob);
        if (needType)
          this.WriteXsiType("XmlSchemaMaxLengthFacet", "http://www.w3.org/2001/XMLSchema");
        this.WriteNamespaceDeclarations(ob.Namespaces);
        ICollection unhandledAttributes = (ICollection) ob.UnhandledAttributes;
        if (unhandledAttributes != null)
        {
          foreach (XmlAttribute node in (IEnumerable) unhandledAttributes)
          {
            if (node.NamespaceURI != "http://www.w3.org/2000/xmlns/")
              this.WriteXmlAttribute((XmlNode) node, (object) ob);
          }
        }
        this.WriteAttribute("id", string.Empty, ob.Id);
        this.WriteAttribute("value", string.Empty, ob.Value);
        if (ob.IsFixed)
          this.WriteAttribute("fixed", string.Empty, !ob.IsFixed ? "false" : "true");
        this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
        if (!writeWrappingElem)
          return;
        this.WriteEndElement((object) ob);
      }
    }

    private void WriteObject_XmlSchemaMinLengthFacet(
      XmlSchemaMinLengthFacet ob,
      string element,
      string namesp,
      bool isNullable,
      bool needType,
      bool writeWrappingElem)
    {
      if (ob == null)
      {
        if (!isNullable)
          return;
        this.WriteNullTagLiteral(element, namesp);
      }
      else
      {
        if (ob.GetType() != typeof (XmlSchemaMinLengthFacet))
          throw this.CreateUnknownTypeException((object) ob);
        if (writeWrappingElem)
          this.WriteStartElement(element, namesp, (object) ob);
        if (needType)
          this.WriteXsiType("XmlSchemaMinLengthFacet", "http://www.w3.org/2001/XMLSchema");
        this.WriteNamespaceDeclarations(ob.Namespaces);
        ICollection unhandledAttributes = (ICollection) ob.UnhandledAttributes;
        if (unhandledAttributes != null)
        {
          foreach (XmlAttribute node in (IEnumerable) unhandledAttributes)
          {
            if (node.NamespaceURI != "http://www.w3.org/2000/xmlns/")
              this.WriteXmlAttribute((XmlNode) node, (object) ob);
          }
        }
        this.WriteAttribute("id", string.Empty, ob.Id);
        this.WriteAttribute("value", string.Empty, ob.Value);
        if (ob.IsFixed)
          this.WriteAttribute("fixed", string.Empty, !ob.IsFixed ? "false" : "true");
        this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
        if (!writeWrappingElem)
          return;
        this.WriteEndElement((object) ob);
      }
    }

    private void WriteObject_XmlSchemaLengthFacet(
      XmlSchemaLengthFacet ob,
      string element,
      string namesp,
      bool isNullable,
      bool needType,
      bool writeWrappingElem)
    {
      if (ob == null)
      {
        if (!isNullable)
          return;
        this.WriteNullTagLiteral(element, namesp);
      }
      else
      {
        if (ob.GetType() != typeof (XmlSchemaLengthFacet))
          throw this.CreateUnknownTypeException((object) ob);
        if (writeWrappingElem)
          this.WriteStartElement(element, namesp, (object) ob);
        if (needType)
          this.WriteXsiType("XmlSchemaLengthFacet", "http://www.w3.org/2001/XMLSchema");
        this.WriteNamespaceDeclarations(ob.Namespaces);
        ICollection unhandledAttributes = (ICollection) ob.UnhandledAttributes;
        if (unhandledAttributes != null)
        {
          foreach (XmlAttribute node in (IEnumerable) unhandledAttributes)
          {
            if (node.NamespaceURI != "http://www.w3.org/2000/xmlns/")
              this.WriteXmlAttribute((XmlNode) node, (object) ob);
          }
        }
        this.WriteAttribute("id", string.Empty, ob.Id);
        this.WriteAttribute("value", string.Empty, ob.Value);
        if (ob.IsFixed)
          this.WriteAttribute("fixed", string.Empty, !ob.IsFixed ? "false" : "true");
        this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
        if (!writeWrappingElem)
          return;
        this.WriteEndElement((object) ob);
      }
    }

    private void WriteObject_XmlSchemaFractionDigitsFacet(
      XmlSchemaFractionDigitsFacet ob,
      string element,
      string namesp,
      bool isNullable,
      bool needType,
      bool writeWrappingElem)
    {
      if (ob == null)
      {
        if (!isNullable)
          return;
        this.WriteNullTagLiteral(element, namesp);
      }
      else
      {
        if (ob.GetType() != typeof (XmlSchemaFractionDigitsFacet))
          throw this.CreateUnknownTypeException((object) ob);
        if (writeWrappingElem)
          this.WriteStartElement(element, namesp, (object) ob);
        if (needType)
          this.WriteXsiType("XmlSchemaFractionDigitsFacet", "http://www.w3.org/2001/XMLSchema");
        this.WriteNamespaceDeclarations(ob.Namespaces);
        ICollection unhandledAttributes = (ICollection) ob.UnhandledAttributes;
        if (unhandledAttributes != null)
        {
          foreach (XmlAttribute node in (IEnumerable) unhandledAttributes)
          {
            if (node.NamespaceURI != "http://www.w3.org/2000/xmlns/")
              this.WriteXmlAttribute((XmlNode) node, (object) ob);
          }
        }
        this.WriteAttribute("id", string.Empty, ob.Id);
        this.WriteAttribute("value", string.Empty, ob.Value);
        if (ob.IsFixed)
          this.WriteAttribute("fixed", string.Empty, !ob.IsFixed ? "false" : "true");
        this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
        if (!writeWrappingElem)
          return;
        this.WriteEndElement((object) ob);
      }
    }

    private void WriteObject_XmlSchemaMaxInclusiveFacet(
      XmlSchemaMaxInclusiveFacet ob,
      string element,
      string namesp,
      bool isNullable,
      bool needType,
      bool writeWrappingElem)
    {
      if (ob == null)
      {
        if (!isNullable)
          return;
        this.WriteNullTagLiteral(element, namesp);
      }
      else
      {
        if (ob.GetType() != typeof (XmlSchemaMaxInclusiveFacet))
          throw this.CreateUnknownTypeException((object) ob);
        if (writeWrappingElem)
          this.WriteStartElement(element, namesp, (object) ob);
        if (needType)
          this.WriteXsiType("XmlSchemaMaxInclusiveFacet", "http://www.w3.org/2001/XMLSchema");
        this.WriteNamespaceDeclarations(ob.Namespaces);
        ICollection unhandledAttributes = (ICollection) ob.UnhandledAttributes;
        if (unhandledAttributes != null)
        {
          foreach (XmlAttribute node in (IEnumerable) unhandledAttributes)
          {
            if (node.NamespaceURI != "http://www.w3.org/2000/xmlns/")
              this.WriteXmlAttribute((XmlNode) node, (object) ob);
          }
        }
        this.WriteAttribute("id", string.Empty, ob.Id);
        this.WriteAttribute("value", string.Empty, ob.Value);
        if (ob.IsFixed)
          this.WriteAttribute("fixed", string.Empty, !ob.IsFixed ? "false" : "true");
        this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
        if (!writeWrappingElem)
          return;
        this.WriteEndElement((object) ob);
      }
    }

    private void WriteObject_XmlSchemaMaxExclusiveFacet(
      XmlSchemaMaxExclusiveFacet ob,
      string element,
      string namesp,
      bool isNullable,
      bool needType,
      bool writeWrappingElem)
    {
      if (ob == null)
      {
        if (!isNullable)
          return;
        this.WriteNullTagLiteral(element, namesp);
      }
      else
      {
        if (ob.GetType() != typeof (XmlSchemaMaxExclusiveFacet))
          throw this.CreateUnknownTypeException((object) ob);
        if (writeWrappingElem)
          this.WriteStartElement(element, namesp, (object) ob);
        if (needType)
          this.WriteXsiType("XmlSchemaMaxExclusiveFacet", "http://www.w3.org/2001/XMLSchema");
        this.WriteNamespaceDeclarations(ob.Namespaces);
        ICollection unhandledAttributes = (ICollection) ob.UnhandledAttributes;
        if (unhandledAttributes != null)
        {
          foreach (XmlAttribute node in (IEnumerable) unhandledAttributes)
          {
            if (node.NamespaceURI != "http://www.w3.org/2000/xmlns/")
              this.WriteXmlAttribute((XmlNode) node, (object) ob);
          }
        }
        this.WriteAttribute("id", string.Empty, ob.Id);
        this.WriteAttribute("value", string.Empty, ob.Value);
        if (ob.IsFixed)
          this.WriteAttribute("fixed", string.Empty, !ob.IsFixed ? "false" : "true");
        this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
        if (!writeWrappingElem)
          return;
        this.WriteEndElement((object) ob);
      }
    }

    private void WriteObject_XmlSchemaMinExclusiveFacet(
      XmlSchemaMinExclusiveFacet ob,
      string element,
      string namesp,
      bool isNullable,
      bool needType,
      bool writeWrappingElem)
    {
      if (ob == null)
      {
        if (!isNullable)
          return;
        this.WriteNullTagLiteral(element, namesp);
      }
      else
      {
        if (ob.GetType() != typeof (XmlSchemaMinExclusiveFacet))
          throw this.CreateUnknownTypeException((object) ob);
        if (writeWrappingElem)
          this.WriteStartElement(element, namesp, (object) ob);
        if (needType)
          this.WriteXsiType("XmlSchemaMinExclusiveFacet", "http://www.w3.org/2001/XMLSchema");
        this.WriteNamespaceDeclarations(ob.Namespaces);
        ICollection unhandledAttributes = (ICollection) ob.UnhandledAttributes;
        if (unhandledAttributes != null)
        {
          foreach (XmlAttribute node in (IEnumerable) unhandledAttributes)
          {
            if (node.NamespaceURI != "http://www.w3.org/2000/xmlns/")
              this.WriteXmlAttribute((XmlNode) node, (object) ob);
          }
        }
        this.WriteAttribute("id", string.Empty, ob.Id);
        this.WriteAttribute("value", string.Empty, ob.Value);
        if (ob.IsFixed)
          this.WriteAttribute("fixed", string.Empty, !ob.IsFixed ? "false" : "true");
        this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
        if (!writeWrappingElem)
          return;
        this.WriteEndElement((object) ob);
      }
    }

    private void WriteObject_XmlSchemaEnumerationFacet(
      XmlSchemaEnumerationFacet ob,
      string element,
      string namesp,
      bool isNullable,
      bool needType,
      bool writeWrappingElem)
    {
      if (ob == null)
      {
        if (!isNullable)
          return;
        this.WriteNullTagLiteral(element, namesp);
      }
      else
      {
        if (ob.GetType() != typeof (XmlSchemaEnumerationFacet))
          throw this.CreateUnknownTypeException((object) ob);
        if (writeWrappingElem)
          this.WriteStartElement(element, namesp, (object) ob);
        if (needType)
          this.WriteXsiType("XmlSchemaEnumerationFacet", "http://www.w3.org/2001/XMLSchema");
        this.WriteNamespaceDeclarations(ob.Namespaces);
        ICollection unhandledAttributes = (ICollection) ob.UnhandledAttributes;
        if (unhandledAttributes != null)
        {
          foreach (XmlAttribute node in (IEnumerable) unhandledAttributes)
          {
            if (node.NamespaceURI != "http://www.w3.org/2000/xmlns/")
              this.WriteXmlAttribute((XmlNode) node, (object) ob);
          }
        }
        this.WriteAttribute("id", string.Empty, ob.Id);
        this.WriteAttribute("value", string.Empty, ob.Value);
        if (ob.IsFixed)
          this.WriteAttribute("fixed", string.Empty, !ob.IsFixed ? "false" : "true");
        this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
        if (!writeWrappingElem)
          return;
        this.WriteEndElement((object) ob);
      }
    }

    private void WriteObject_XmlSchemaTotalDigitsFacet(
      XmlSchemaTotalDigitsFacet ob,
      string element,
      string namesp,
      bool isNullable,
      bool needType,
      bool writeWrappingElem)
    {
      if (ob == null)
      {
        if (!isNullable)
          return;
        this.WriteNullTagLiteral(element, namesp);
      }
      else
      {
        if (ob.GetType() != typeof (XmlSchemaTotalDigitsFacet))
          throw this.CreateUnknownTypeException((object) ob);
        if (writeWrappingElem)
          this.WriteStartElement(element, namesp, (object) ob);
        if (needType)
          this.WriteXsiType("XmlSchemaTotalDigitsFacet", "http://www.w3.org/2001/XMLSchema");
        this.WriteNamespaceDeclarations(ob.Namespaces);
        ICollection unhandledAttributes = (ICollection) ob.UnhandledAttributes;
        if (unhandledAttributes != null)
        {
          foreach (XmlAttribute node in (IEnumerable) unhandledAttributes)
          {
            if (node.NamespaceURI != "http://www.w3.org/2000/xmlns/")
              this.WriteXmlAttribute((XmlNode) node, (object) ob);
          }
        }
        this.WriteAttribute("id", string.Empty, ob.Id);
        this.WriteAttribute("value", string.Empty, ob.Value);
        if (ob.IsFixed)
          this.WriteAttribute("fixed", string.Empty, !ob.IsFixed ? "false" : "true");
        this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
        if (!writeWrappingElem)
          return;
        this.WriteEndElement((object) ob);
      }
    }

    private void WriteObject_XmlSchemaMinInclusiveFacet(
      XmlSchemaMinInclusiveFacet ob,
      string element,
      string namesp,
      bool isNullable,
      bool needType,
      bool writeWrappingElem)
    {
      if (ob == null)
      {
        if (!isNullable)
          return;
        this.WriteNullTagLiteral(element, namesp);
      }
      else
      {
        if (ob.GetType() != typeof (XmlSchemaMinInclusiveFacet))
          throw this.CreateUnknownTypeException((object) ob);
        if (writeWrappingElem)
          this.WriteStartElement(element, namesp, (object) ob);
        if (needType)
          this.WriteXsiType("XmlSchemaMinInclusiveFacet", "http://www.w3.org/2001/XMLSchema");
        this.WriteNamespaceDeclarations(ob.Namespaces);
        ICollection unhandledAttributes = (ICollection) ob.UnhandledAttributes;
        if (unhandledAttributes != null)
        {
          foreach (XmlAttribute node in (IEnumerable) unhandledAttributes)
          {
            if (node.NamespaceURI != "http://www.w3.org/2000/xmlns/")
              this.WriteXmlAttribute((XmlNode) node, (object) ob);
          }
        }
        this.WriteAttribute("id", string.Empty, ob.Id);
        this.WriteAttribute("value", string.Empty, ob.Value);
        if (ob.IsFixed)
          this.WriteAttribute("fixed", string.Empty, !ob.IsFixed ? "false" : "true");
        this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
        if (!writeWrappingElem)
          return;
        this.WriteEndElement((object) ob);
      }
    }

    private void WriteObject_XmlSchemaWhiteSpaceFacet(
      XmlSchemaWhiteSpaceFacet ob,
      string element,
      string namesp,
      bool isNullable,
      bool needType,
      bool writeWrappingElem)
    {
      if (ob == null)
      {
        if (!isNullable)
          return;
        this.WriteNullTagLiteral(element, namesp);
      }
      else
      {
        if (ob.GetType() != typeof (XmlSchemaWhiteSpaceFacet))
          throw this.CreateUnknownTypeException((object) ob);
        if (writeWrappingElem)
          this.WriteStartElement(element, namesp, (object) ob);
        if (needType)
          this.WriteXsiType("XmlSchemaWhiteSpaceFacet", "http://www.w3.org/2001/XMLSchema");
        this.WriteNamespaceDeclarations(ob.Namespaces);
        ICollection unhandledAttributes = (ICollection) ob.UnhandledAttributes;
        if (unhandledAttributes != null)
        {
          foreach (XmlAttribute node in (IEnumerable) unhandledAttributes)
          {
            if (node.NamespaceURI != "http://www.w3.org/2000/xmlns/")
              this.WriteXmlAttribute((XmlNode) node, (object) ob);
          }
        }
        this.WriteAttribute("id", string.Empty, ob.Id);
        this.WriteAttribute("value", string.Empty, ob.Value);
        if (ob.IsFixed)
          this.WriteAttribute("fixed", string.Empty, !ob.IsFixed ? "false" : "true");
        this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
        if (!writeWrappingElem)
          return;
        this.WriteEndElement((object) ob);
      }
    }

    private void WriteObject_XmlSchemaPatternFacet(
      XmlSchemaPatternFacet ob,
      string element,
      string namesp,
      bool isNullable,
      bool needType,
      bool writeWrappingElem)
    {
      if (ob == null)
      {
        if (!isNullable)
          return;
        this.WriteNullTagLiteral(element, namesp);
      }
      else
      {
        if (ob.GetType() != typeof (XmlSchemaPatternFacet))
          throw this.CreateUnknownTypeException((object) ob);
        if (writeWrappingElem)
          this.WriteStartElement(element, namesp, (object) ob);
        if (needType)
          this.WriteXsiType("XmlSchemaPatternFacet", "http://www.w3.org/2001/XMLSchema");
        this.WriteNamespaceDeclarations(ob.Namespaces);
        ICollection unhandledAttributes = (ICollection) ob.UnhandledAttributes;
        if (unhandledAttributes != null)
        {
          foreach (XmlAttribute node in (IEnumerable) unhandledAttributes)
          {
            if (node.NamespaceURI != "http://www.w3.org/2000/xmlns/")
              this.WriteXmlAttribute((XmlNode) node, (object) ob);
          }
        }
        this.WriteAttribute("id", string.Empty, ob.Id);
        this.WriteAttribute("value", string.Empty, ob.Value);
        if (ob.IsFixed)
          this.WriteAttribute("fixed", string.Empty, !ob.IsFixed ? "false" : "true");
        this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
        if (!writeWrappingElem)
          return;
        this.WriteEndElement((object) ob);
      }
    }

    private string GetEnumValue_XmlSchemaContentProcessing(XmlSchemaContentProcessing val)
    {
      switch (val)
      {
        case XmlSchemaContentProcessing.Skip:
          return "skip";
        case XmlSchemaContentProcessing.Lax:
          return "lax";
        case XmlSchemaContentProcessing.Strict:
          return "strict";
        default:
          return ((long) val).ToString((IFormatProvider) CultureInfo.InvariantCulture);
      }
    }

    private void WriteObject_XmlSchemaAny(
      XmlSchemaAny ob,
      string element,
      string namesp,
      bool isNullable,
      bool needType,
      bool writeWrappingElem)
    {
      if (ob == null)
      {
        if (!isNullable)
          return;
        this.WriteNullTagLiteral(element, namesp);
      }
      else
      {
        if (ob.GetType() != typeof (XmlSchemaAny))
          throw this.CreateUnknownTypeException((object) ob);
        if (writeWrappingElem)
          this.WriteStartElement(element, namesp, (object) ob);
        if (needType)
          this.WriteXsiType("XmlSchemaAny", "http://www.w3.org/2001/XMLSchema");
        this.WriteNamespaceDeclarations(ob.Namespaces);
        ICollection unhandledAttributes = (ICollection) ob.UnhandledAttributes;
        if (unhandledAttributes != null)
        {
          foreach (XmlAttribute node in (IEnumerable) unhandledAttributes)
          {
            if (node.NamespaceURI != "http://www.w3.org/2000/xmlns/")
              this.WriteXmlAttribute((XmlNode) node, (object) ob);
          }
        }
        this.WriteAttribute("id", string.Empty, ob.Id);
        this.WriteAttribute("minOccurs", string.Empty, ob.MinOccursString);
        this.WriteAttribute("maxOccurs", string.Empty, ob.MaxOccursString);
        this.WriteAttribute("namespace", string.Empty, ob.Namespace);
        if (ob.ProcessContents != XmlSchemaContentProcessing.None)
          this.WriteAttribute("processContents", string.Empty, this.GetEnumValue_XmlSchemaContentProcessing(ob.ProcessContents));
        this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
        if (!writeWrappingElem)
          return;
        this.WriteEndElement((object) ob);
      }
    }

    private void WriteObject_XmlSchemaComplexContentExtension(
      XmlSchemaComplexContentExtension ob,
      string element,
      string namesp,
      bool isNullable,
      bool needType,
      bool writeWrappingElem)
    {
      if (ob == null)
      {
        if (!isNullable)
          return;
        this.WriteNullTagLiteral(element, namesp);
      }
      else
      {
        if (ob.GetType() != typeof (XmlSchemaComplexContentExtension))
          throw this.CreateUnknownTypeException((object) ob);
        if (writeWrappingElem)
          this.WriteStartElement(element, namesp, (object) ob);
        if (needType)
          this.WriteXsiType("XmlSchemaComplexContentExtension", "http://www.w3.org/2001/XMLSchema");
        this.WriteNamespaceDeclarations(ob.Namespaces);
        ICollection unhandledAttributes = (ICollection) ob.UnhandledAttributes;
        if (unhandledAttributes != null)
        {
          foreach (XmlAttribute node in (IEnumerable) unhandledAttributes)
          {
            if (node.NamespaceURI != "http://www.w3.org/2000/xmlns/")
              this.WriteXmlAttribute((XmlNode) node, (object) ob);
          }
        }
        this.WriteAttribute("id", string.Empty, ob.Id);
        this.WriteAttribute("base", string.Empty, this.FromXmlQualifiedName(ob.BaseTypeName));
        this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
        if (ob.Particle is XmlSchemaGroupRef)
          this.WriteObject_XmlSchemaGroupRef((XmlSchemaGroupRef) ob.Particle, "group", "http://www.w3.org/2001/XMLSchema", false, false, true);
        else if (ob.Particle is XmlSchemaSequence)
          this.WriteObject_XmlSchemaSequence((XmlSchemaSequence) ob.Particle, "sequence", "http://www.w3.org/2001/XMLSchema", false, false, true);
        else if (ob.Particle is XmlSchemaChoice)
          this.WriteObject_XmlSchemaChoice((XmlSchemaChoice) ob.Particle, "choice", "http://www.w3.org/2001/XMLSchema", false, false, true);
        else if (ob.Particle is XmlSchemaAll)
          this.WriteObject_XmlSchemaAll((XmlSchemaAll) ob.Particle, "all", "http://www.w3.org/2001/XMLSchema", false, false, true);
        if (ob.Attributes != null)
        {
          for (int index = 0; index < ob.Attributes.Count; ++index)
          {
            if (ob.Attributes[index] != null)
            {
              if (ob.Attributes[index].GetType() == typeof (XmlSchemaAttribute))
              {
                this.WriteObject_XmlSchemaAttribute((XmlSchemaAttribute) ob.Attributes[index], "attribute", "http://www.w3.org/2001/XMLSchema", false, false, true);
              }
              else
              {
                if (ob.Attributes[index].GetType() != typeof (XmlSchemaAttributeGroupRef))
                  throw this.CreateUnknownTypeException((object) ob.Attributes[index]);
                this.WriteObject_XmlSchemaAttributeGroupRef((XmlSchemaAttributeGroupRef) ob.Attributes[index], "attributeGroup", "http://www.w3.org/2001/XMLSchema", false, false, true);
              }
            }
          }
        }
        this.WriteObject_XmlSchemaAnyAttribute(ob.AnyAttribute, "anyAttribute", "http://www.w3.org/2001/XMLSchema", false, false, true);
        if (!writeWrappingElem)
          return;
        this.WriteEndElement((object) ob);
      }
    }

    private void WriteObject_XmlSchemaComplexContentRestriction(
      XmlSchemaComplexContentRestriction ob,
      string element,
      string namesp,
      bool isNullable,
      bool needType,
      bool writeWrappingElem)
    {
      if (ob == null)
      {
        if (!isNullable)
          return;
        this.WriteNullTagLiteral(element, namesp);
      }
      else
      {
        if (ob.GetType() != typeof (XmlSchemaComplexContentRestriction))
          throw this.CreateUnknownTypeException((object) ob);
        if (writeWrappingElem)
          this.WriteStartElement(element, namesp, (object) ob);
        if (needType)
          this.WriteXsiType("XmlSchemaComplexContentRestriction", "http://www.w3.org/2001/XMLSchema");
        this.WriteNamespaceDeclarations(ob.Namespaces);
        ICollection unhandledAttributes = (ICollection) ob.UnhandledAttributes;
        if (unhandledAttributes != null)
        {
          foreach (XmlAttribute node in (IEnumerable) unhandledAttributes)
          {
            if (node.NamespaceURI != "http://www.w3.org/2000/xmlns/")
              this.WriteXmlAttribute((XmlNode) node, (object) ob);
          }
        }
        this.WriteAttribute("id", string.Empty, ob.Id);
        this.WriteAttribute("base", string.Empty, this.FromXmlQualifiedName(ob.BaseTypeName));
        this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
        if (ob.Particle is XmlSchemaSequence)
          this.WriteObject_XmlSchemaSequence((XmlSchemaSequence) ob.Particle, "sequence", "http://www.w3.org/2001/XMLSchema", false, false, true);
        else if (ob.Particle is XmlSchemaChoice)
          this.WriteObject_XmlSchemaChoice((XmlSchemaChoice) ob.Particle, "choice", "http://www.w3.org/2001/XMLSchema", false, false, true);
        else if (ob.Particle is XmlSchemaGroupRef)
          this.WriteObject_XmlSchemaGroupRef((XmlSchemaGroupRef) ob.Particle, "group", "http://www.w3.org/2001/XMLSchema", false, false, true);
        else if (ob.Particle is XmlSchemaAll)
          this.WriteObject_XmlSchemaAll((XmlSchemaAll) ob.Particle, "all", "http://www.w3.org/2001/XMLSchema", false, false, true);
        if (ob.Attributes != null)
        {
          for (int index = 0; index < ob.Attributes.Count; ++index)
          {
            if (ob.Attributes[index] != null)
            {
              if (ob.Attributes[index].GetType() == typeof (XmlSchemaAttributeGroupRef))
              {
                this.WriteObject_XmlSchemaAttributeGroupRef((XmlSchemaAttributeGroupRef) ob.Attributes[index], "attributeGroup", "http://www.w3.org/2001/XMLSchema", false, false, true);
              }
              else
              {
                if (ob.Attributes[index].GetType() != typeof (XmlSchemaAttribute))
                  throw this.CreateUnknownTypeException((object) ob.Attributes[index]);
                this.WriteObject_XmlSchemaAttribute((XmlSchemaAttribute) ob.Attributes[index], "attribute", "http://www.w3.org/2001/XMLSchema", false, false, true);
              }
            }
          }
        }
        this.WriteObject_XmlSchemaAnyAttribute(ob.AnyAttribute, "anyAttribute", "http://www.w3.org/2001/XMLSchema", false, false, true);
        if (!writeWrappingElem)
          return;
        this.WriteEndElement((object) ob);
      }
    }

    private void WriteObject_XmlSchemaSimpleContentExtension(
      XmlSchemaSimpleContentExtension ob,
      string element,
      string namesp,
      bool isNullable,
      bool needType,
      bool writeWrappingElem)
    {
      if (ob == null)
      {
        if (!isNullable)
          return;
        this.WriteNullTagLiteral(element, namesp);
      }
      else
      {
        if (ob.GetType() != typeof (XmlSchemaSimpleContentExtension))
          throw this.CreateUnknownTypeException((object) ob);
        if (writeWrappingElem)
          this.WriteStartElement(element, namesp, (object) ob);
        if (needType)
          this.WriteXsiType("XmlSchemaSimpleContentExtension", "http://www.w3.org/2001/XMLSchema");
        this.WriteNamespaceDeclarations(ob.Namespaces);
        ICollection unhandledAttributes = (ICollection) ob.UnhandledAttributes;
        if (unhandledAttributes != null)
        {
          foreach (XmlAttribute node in (IEnumerable) unhandledAttributes)
          {
            if (node.NamespaceURI != "http://www.w3.org/2000/xmlns/")
              this.WriteXmlAttribute((XmlNode) node, (object) ob);
          }
        }
        this.WriteAttribute("id", string.Empty, ob.Id);
        this.WriteAttribute("base", string.Empty, this.FromXmlQualifiedName(ob.BaseTypeName));
        this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
        if (ob.Attributes != null)
        {
          for (int index = 0; index < ob.Attributes.Count; ++index)
          {
            if (ob.Attributes[index] != null)
            {
              if (ob.Attributes[index].GetType() == typeof (XmlSchemaAttributeGroupRef))
              {
                this.WriteObject_XmlSchemaAttributeGroupRef((XmlSchemaAttributeGroupRef) ob.Attributes[index], "attributeGroup", "http://www.w3.org/2001/XMLSchema", false, false, true);
              }
              else
              {
                if (ob.Attributes[index].GetType() != typeof (XmlSchemaAttribute))
                  throw this.CreateUnknownTypeException((object) ob.Attributes[index]);
                this.WriteObject_XmlSchemaAttribute((XmlSchemaAttribute) ob.Attributes[index], "attribute", "http://www.w3.org/2001/XMLSchema", false, false, true);
              }
            }
          }
        }
        this.WriteObject_XmlSchemaAnyAttribute(ob.AnyAttribute, "anyAttribute", "http://www.w3.org/2001/XMLSchema", false, false, true);
        if (!writeWrappingElem)
          return;
        this.WriteEndElement((object) ob);
      }
    }

    private void WriteObject_XmlSchemaSimpleContentRestriction(
      XmlSchemaSimpleContentRestriction ob,
      string element,
      string namesp,
      bool isNullable,
      bool needType,
      bool writeWrappingElem)
    {
      if (ob == null)
      {
        if (!isNullable)
          return;
        this.WriteNullTagLiteral(element, namesp);
      }
      else
      {
        if (ob.GetType() != typeof (XmlSchemaSimpleContentRestriction))
          throw this.CreateUnknownTypeException((object) ob);
        if (writeWrappingElem)
          this.WriteStartElement(element, namesp, (object) ob);
        if (needType)
          this.WriteXsiType("XmlSchemaSimpleContentRestriction", "http://www.w3.org/2001/XMLSchema");
        this.WriteNamespaceDeclarations(ob.Namespaces);
        ICollection unhandledAttributes = (ICollection) ob.UnhandledAttributes;
        if (unhandledAttributes != null)
        {
          foreach (XmlAttribute node in (IEnumerable) unhandledAttributes)
          {
            if (node.NamespaceURI != "http://www.w3.org/2000/xmlns/")
              this.WriteXmlAttribute((XmlNode) node, (object) ob);
          }
        }
        this.WriteAttribute("id", string.Empty, ob.Id);
        this.WriteAttribute("base", string.Empty, this.FromXmlQualifiedName(ob.BaseTypeName));
        this.WriteObject_XmlSchemaAnnotation(ob.Annotation, "annotation", "http://www.w3.org/2001/XMLSchema", false, false, true);
        this.WriteObject_XmlSchemaSimpleType(ob.BaseType, "simpleType", "http://www.w3.org/2001/XMLSchema", false, false, true);
        if (ob.Facets != null)
        {
          for (int index = 0; index < ob.Facets.Count; ++index)
          {
            if (ob.Facets[index] != null)
            {
              if (ob.Facets[index].GetType() == typeof (XmlSchemaEnumerationFacet))
                this.WriteObject_XmlSchemaEnumerationFacet((XmlSchemaEnumerationFacet) ob.Facets[index], "enumeration", "http://www.w3.org/2001/XMLSchema", false, false, true);
              else if (ob.Facets[index].GetType() == typeof (XmlSchemaMaxLengthFacet))
                this.WriteObject_XmlSchemaMaxLengthFacet((XmlSchemaMaxLengthFacet) ob.Facets[index], "maxLength", "http://www.w3.org/2001/XMLSchema", false, false, true);
              else if (ob.Facets[index].GetType() == typeof (XmlSchemaMinLengthFacet))
                this.WriteObject_XmlSchemaMinLengthFacet((XmlSchemaMinLengthFacet) ob.Facets[index], "minLength", "http://www.w3.org/2001/XMLSchema", false, false, true);
              else if (ob.Facets[index].GetType() == typeof (XmlSchemaLengthFacet))
                this.WriteObject_XmlSchemaLengthFacet((XmlSchemaLengthFacet) ob.Facets[index], "length", "http://www.w3.org/2001/XMLSchema", false, false, true);
              else if (ob.Facets[index].GetType() == typeof (XmlSchemaFractionDigitsFacet))
                this.WriteObject_XmlSchemaFractionDigitsFacet((XmlSchemaFractionDigitsFacet) ob.Facets[index], "fractionDigits", "http://www.w3.org/2001/XMLSchema", false, false, true);
              else if (ob.Facets[index].GetType() == typeof (XmlSchemaTotalDigitsFacet))
                this.WriteObject_XmlSchemaTotalDigitsFacet((XmlSchemaTotalDigitsFacet) ob.Facets[index], "totalDigits", "http://www.w3.org/2001/XMLSchema", false, false, true);
              else if (ob.Facets[index].GetType() == typeof (XmlSchemaMaxInclusiveFacet))
                this.WriteObject_XmlSchemaMaxInclusiveFacet((XmlSchemaMaxInclusiveFacet) ob.Facets[index], "maxInclusive", "http://www.w3.org/2001/XMLSchema", false, false, true);
              else if (ob.Facets[index].GetType() == typeof (XmlSchemaMaxExclusiveFacet))
                this.WriteObject_XmlSchemaMaxExclusiveFacet((XmlSchemaMaxExclusiveFacet) ob.Facets[index], "maxExclusive", "http://www.w3.org/2001/XMLSchema", false, false, true);
              else if (ob.Facets[index].GetType() == typeof (XmlSchemaMinInclusiveFacet))
                this.WriteObject_XmlSchemaMinInclusiveFacet((XmlSchemaMinInclusiveFacet) ob.Facets[index], "minInclusive", "http://www.w3.org/2001/XMLSchema", false, false, true);
              else if (ob.Facets[index].GetType() == typeof (XmlSchemaMinExclusiveFacet))
                this.WriteObject_XmlSchemaMinExclusiveFacet((XmlSchemaMinExclusiveFacet) ob.Facets[index], "minExclusive", "http://www.w3.org/2001/XMLSchema", false, false, true);
              else if (ob.Facets[index].GetType() == typeof (XmlSchemaWhiteSpaceFacet))
              {
                this.WriteObject_XmlSchemaWhiteSpaceFacet((XmlSchemaWhiteSpaceFacet) ob.Facets[index], "whiteSpace", "http://www.w3.org/2001/XMLSchema", false, false, true);
              }
              else
              {
                if (ob.Facets[index].GetType() != typeof (XmlSchemaPatternFacet))
                  throw this.CreateUnknownTypeException((object) ob.Facets[index]);
                this.WriteObject_XmlSchemaPatternFacet((XmlSchemaPatternFacet) ob.Facets[index], "pattern", "http://www.w3.org/2001/XMLSchema", false, false, true);
              }
            }
          }
        }
        if (ob.Attributes != null)
        {
          for (int index = 0; index < ob.Attributes.Count; ++index)
          {
            if (ob.Attributes[index] != null)
            {
              if (ob.Attributes[index].GetType() == typeof (XmlSchemaAttributeGroupRef))
              {
                this.WriteObject_XmlSchemaAttributeGroupRef((XmlSchemaAttributeGroupRef) ob.Attributes[index], "attributeGroup", "http://www.w3.org/2001/XMLSchema", false, false, true);
              }
              else
              {
                if (ob.Attributes[index].GetType() != typeof (XmlSchemaAttribute))
                  throw this.CreateUnknownTypeException((object) ob.Attributes[index]);
                this.WriteObject_XmlSchemaAttribute((XmlSchemaAttribute) ob.Attributes[index], "attribute", "http://www.w3.org/2001/XMLSchema", false, false, true);
              }
            }
          }
        }
        this.WriteObject_XmlSchemaAnyAttribute(ob.AnyAttribute, "anyAttribute", "http://www.w3.org/2001/XMLSchema", false, false, true);
        if (!writeWrappingElem)
          return;
        this.WriteEndElement((object) ob);
      }
    }

    protected override void InitCallbacks()
    {
    }
  }
}
