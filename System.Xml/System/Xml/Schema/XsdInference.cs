// Decompiled with JetBrains decompiler
// Type: System.Xml.Schema.XsdInference
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.Collections.Generic;

namespace System.Xml.Schema
{
  internal class XsdInference
  {
    public const string NamespaceXml = "http://www.w3.org/XML/1998/namespace";
    public const string NamespaceXmlns = "http://www.w3.org/2000/xmlns/";
    public const string XdtNamespace = "http://www.w3.org/2003/11/xpath-datatypes";
    private static readonly XmlQualifiedName QNameString = new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema");
    private static readonly XmlQualifiedName QNameBoolean = new XmlQualifiedName("boolean", "http://www.w3.org/2001/XMLSchema");
    private static readonly XmlQualifiedName QNameAnyType = new XmlQualifiedName("anyType", "http://www.w3.org/2001/XMLSchema");
    private static readonly XmlQualifiedName QNameByte = new XmlQualifiedName("byte", "http://www.w3.org/2001/XMLSchema");
    private static readonly XmlQualifiedName QNameUByte = new XmlQualifiedName("unsignedByte", "http://www.w3.org/2001/XMLSchema");
    private static readonly XmlQualifiedName QNameShort = new XmlQualifiedName("short", "http://www.w3.org/2001/XMLSchema");
    private static readonly XmlQualifiedName QNameUShort = new XmlQualifiedName("unsignedShort", "http://www.w3.org/2001/XMLSchema");
    private static readonly XmlQualifiedName QNameInt = new XmlQualifiedName("int", "http://www.w3.org/2001/XMLSchema");
    private static readonly XmlQualifiedName QNameUInt = new XmlQualifiedName("unsignedInt", "http://www.w3.org/2001/XMLSchema");
    private static readonly XmlQualifiedName QNameLong = new XmlQualifiedName("long", "http://www.w3.org/2001/XMLSchema");
    private static readonly XmlQualifiedName QNameULong = new XmlQualifiedName("unsignedLong", "http://www.w3.org/2001/XMLSchema");
    private static readonly XmlQualifiedName QNameDecimal = new XmlQualifiedName("decimal", "http://www.w3.org/2001/XMLSchema");
    private static readonly XmlQualifiedName QNameUDecimal = new XmlQualifiedName("unsignedDecimal", "http://www.w3.org/2001/XMLSchema");
    private static readonly XmlQualifiedName QNameDouble = new XmlQualifiedName("double", "http://www.w3.org/2001/XMLSchema");
    private static readonly XmlQualifiedName QNameFloat = new XmlQualifiedName("float", "http://www.w3.org/2001/XMLSchema");
    private static readonly XmlQualifiedName QNameDateTime = new XmlQualifiedName("dateTime", "http://www.w3.org/2001/XMLSchema");
    private static readonly XmlQualifiedName QNameDuration = new XmlQualifiedName("duration", "http://www.w3.org/2001/XMLSchema");
    private XmlReader source;
    private XmlSchemaSet schemas;
    private bool laxOccurrence;
    private bool laxTypeInference;
    private Hashtable newElements = new Hashtable();
    private Hashtable newAttributes = new Hashtable();

    private XsdInference(
      XmlReader xmlReader,
      XmlSchemaSet schemas,
      bool laxOccurrence,
      bool laxTypeInference)
    {
      this.source = xmlReader;
      this.schemas = schemas;
      this.laxOccurrence = laxOccurrence;
      this.laxTypeInference = laxTypeInference;
    }

    public static XmlSchemaSet Process(
      XmlReader xmlReader,
      XmlSchemaSet schemas,
      bool laxOccurrence,
      bool laxTypeInference)
    {
      XsdInference xsdInference = new XsdInference(xmlReader, schemas, laxOccurrence, laxTypeInference);
      xsdInference.Run();
      return xsdInference.schemas;
    }

    private void Run()
    {
      this.schemas.Compile();
      int content = (int) this.source.MoveToContent();
      if (this.source.NodeType != XmlNodeType.Element)
        throw new ArgumentException("Argument XmlReader content is expected to be an element.");
      XmlQualifiedName name = new XmlQualifiedName(this.source.LocalName, this.source.NamespaceURI);
      XmlSchemaElement globalElement = this.GetGlobalElement(name);
      if (globalElement == null)
        this.InferElement(this.CreateGlobalElement(name), name.Namespace, true);
      else
        this.InferElement(globalElement, name.Namespace, false);
    }

    private void AddImport(string current, string import)
    {
      foreach (XmlSchema schema in (IEnumerable) this.schemas.Schemas(current))
      {
        bool flag = false;
        foreach (XmlSchemaExternal include in schema.Includes)
        {
          if (include is XmlSchemaImport xmlSchemaImport && xmlSchemaImport.Namespace == import)
            flag = true;
        }
        if (!flag)
          schema.Includes.Add((XmlSchemaObject) new XmlSchemaImport()
          {
            Namespace = import
          });
      }
    }

    private void IncludeXmlAttributes()
    {
      if (this.schemas.Schemas("http://www.w3.org/XML/1998/namespace").Count != 0)
        return;
      this.schemas.Add("http://www.w3.org/XML/1998/namespace", "http://www.w3.org/2001/xml.xsd");
    }

    private void InferElement(XmlSchemaElement el, string ns, bool isNew)
    {
      if (el.RefName != XmlQualifiedName.Empty)
      {
        XmlSchemaElement globalElement = this.GetGlobalElement(el.RefName);
        if (globalElement == null)
          this.InferElement(this.CreateElement(el.RefName), ns, true);
        else
          this.InferElement(globalElement, ns, isNew);
      }
      else
      {
        if (this.source.MoveToFirstAttribute())
        {
          this.InferAttributes(el, ns, isNew);
          this.source.MoveToElement();
        }
        if (this.source.IsEmptyElement)
        {
          this.InferAsEmptyElement(el, ns, isNew);
          this.source.Read();
          int content = (int) this.source.MoveToContent();
        }
        else
        {
          this.InferContent(el, ns, isNew);
          this.source.ReadEndElement();
        }
        if (el.SchemaType != null || !(el.SchemaTypeName == XmlQualifiedName.Empty))
          return;
        el.SchemaTypeName = XsdInference.QNameString;
      }
    }

    private Hashtable CollectAttrTable(XmlSchemaObjectCollection attList)
    {
      Hashtable hashtable = new Hashtable();
      foreach (XmlSchemaObject att in attList)
      {
        if (!(att is XmlSchemaAttribute xmlSchemaAttribute))
          throw this.Error(att, string.Format("Attribute inference only supports direct attribute definition. {0} is not supported.", (object) att.GetType()));
        if (xmlSchemaAttribute.RefName != XmlQualifiedName.Empty)
          hashtable.Add((object) xmlSchemaAttribute.RefName, (object) xmlSchemaAttribute);
        else
          hashtable.Add((object) new XmlQualifiedName(xmlSchemaAttribute.Name, string.Empty), (object) xmlSchemaAttribute);
      }
      return hashtable;
    }

    private void InferAttributes(XmlSchemaElement el, string ns, bool isNew)
    {
      XmlSchemaComplexType ct = (XmlSchemaComplexType) null;
      XmlSchemaObjectCollection attList = (XmlSchemaObjectCollection) null;
      Hashtable hashtable = (Hashtable) null;
      do
      {
        string namespaceUri = this.source.NamespaceURI;
        if (namespaceUri != null)
        {
          // ISSUE: reference to a compiler-generated field
          if (XsdInference.\u003C\u003Ef__switch\u0024map30 == null)
          {
            // ISSUE: reference to a compiler-generated field
            XsdInference.\u003C\u003Ef__switch\u0024map30 = new Dictionary<string, int>(3)
            {
              {
                "http://www.w3.org/XML/1998/namespace",
                0
              },
              {
                "http://www.w3.org/2001/XMLSchema-instance",
                1
              },
              {
                "http://www.w3.org/2000/xmlns/",
                2
              }
            };
          }
          int num;
          // ISSUE: reference to a compiler-generated field
          if (XsdInference.\u003C\u003Ef__switch\u0024map30.TryGetValue(namespaceUri, out num))
          {
            switch (num)
            {
              case 0:
                if (this.schemas.Schemas("http://www.w3.org/XML/1998/namespace").Count == 0)
                {
                  this.IncludeXmlAttributes();
                  break;
                }
                break;
              case 1:
                if (this.source.LocalName == "nil")
                {
                  el.IsNillable = true;
                  goto label_17;
                }
                else
                  goto label_17;
              case 2:
                goto label_17;
            }
          }
        }
        if (ct == null)
        {
          ct = this.ToComplexType(el);
          attList = this.GetAttributes(ct);
          hashtable = this.CollectAttrTable(attList);
        }
        XmlQualifiedName xmlQualifiedName = new XmlQualifiedName(this.source.LocalName, this.source.NamespaceURI);
        if (!(hashtable[(object) xmlQualifiedName] is XmlSchemaAttribute attr))
        {
          attList.Add((XmlSchemaObject) this.InferNewAttribute(xmlQualifiedName, isNew, ns));
        }
        else
        {
          hashtable.Remove((object) xmlQualifiedName);
          if (!(attr.RefName != (XmlQualifiedName) null) || !(attr.RefName != XmlQualifiedName.Empty))
            this.InferMergedAttribute(attr);
        }
label_17:;
      }
      while (this.source.MoveToNextAttribute());
      if (hashtable == null)
        return;
      foreach (XmlSchemaAttribute xmlSchemaAttribute in (IEnumerable) hashtable.Values)
        xmlSchemaAttribute.Use = XmlSchemaUse.Optional;
    }

    private XmlSchemaAttribute InferNewAttribute(
      XmlQualifiedName attrName,
      bool isNewTypeDefinition,
      string ns)
    {
      bool flag = false;
      XmlSchemaAttribute xmlSchemaAttribute;
      if (attrName.Namespace.Length > 0)
      {
        XmlSchemaAttribute globalAttribute = this.GetGlobalAttribute(attrName);
        if (globalAttribute == null)
        {
          this.CreateGlobalAttribute(attrName).SchemaTypeName = this.InferSimpleType(this.source.Value);
        }
        else
        {
          this.InferMergedAttribute(globalAttribute);
          flag = globalAttribute.Use == XmlSchemaUse.Required;
        }
        xmlSchemaAttribute = new XmlSchemaAttribute();
        xmlSchemaAttribute.RefName = attrName;
        this.AddImport(ns, attrName.Namespace);
      }
      else
      {
        xmlSchemaAttribute = new XmlSchemaAttribute();
        xmlSchemaAttribute.Name = attrName.Name;
        xmlSchemaAttribute.SchemaTypeName = this.InferSimpleType(this.source.Value);
      }
      xmlSchemaAttribute.Use = this.laxOccurrence || !isNewTypeDefinition && !flag ? XmlSchemaUse.Optional : XmlSchemaUse.Required;
      return xmlSchemaAttribute;
    }

    private void InferMergedAttribute(XmlSchemaAttribute attr)
    {
      attr.SchemaTypeName = this.InferMergedType(this.source.Value, attr.SchemaTypeName);
      attr.SchemaType = (XmlSchemaSimpleType) null;
    }

    private XmlQualifiedName InferMergedType(string value, XmlQualifiedName typeName)
    {
      schemaSimpleType = XmlSchemaType.GetBuiltInSimpleType(typeName);
      if (schemaSimpleType == null)
        return XsdInference.QNameString;
      do
      {
        try
        {
          schemaSimpleType.Datatype.ParseValue(value, this.source.NameTable, this.source as IXmlNamespaceResolver);
          return typeName;
        }
        catch
        {
          typeName = !(schemaSimpleType.BaseXmlSchemaType is XmlSchemaSimpleType schemaSimpleType) ? XmlQualifiedName.Empty : schemaSimpleType.QualifiedName;
        }
      }
      while (typeName != XmlQualifiedName.Empty);
      return XsdInference.QNameString;
    }

    private XmlSchemaObjectCollection GetAttributes(XmlSchemaComplexType ct)
    {
      if (ct.ContentModel == null)
        return ct.Attributes;
      if (ct.ContentModel is XmlSchemaSimpleContent contentModel1)
      {
        if (contentModel1.Content is XmlSchemaSimpleContentExtension content1)
          return content1.Attributes;
        if (contentModel1.Content is XmlSchemaSimpleContentRestriction content2)
          return content2.Attributes;
        throw this.Error((XmlSchemaObject) contentModel1, "Invalid simple content model.");
      }
      if (!(ct.ContentModel is XmlSchemaComplexContent contentModel2))
        throw this.Error((XmlSchemaObject) contentModel2, "Invalid complexType. Should not happen.");
      if (contentModel2.Content is XmlSchemaComplexContentExtension content3)
        return content3.Attributes;
      if (contentModel2.Content is XmlSchemaComplexContentRestriction content4)
        return content4.Attributes;
      throw this.Error((XmlSchemaObject) contentModel2, "Invalid simple content model.");
    }

    private XmlSchemaComplexType ToComplexType(XmlSchemaElement el)
    {
      XmlQualifiedName schemaTypeName = el.SchemaTypeName;
      XmlSchemaType schemaType = el.SchemaType;
      if (schemaType is XmlSchemaComplexType complexType1)
        return complexType1;
      XmlSchemaType globalType = this.schemas.GlobalTypes[schemaTypeName] as XmlSchemaType;
      if (globalType is XmlSchemaComplexType complexType2)
        return complexType2;
      XmlSchemaComplexType complexType3 = new XmlSchemaComplexType();
      el.SchemaType = (XmlSchemaType) complexType3;
      el.SchemaTypeName = XmlQualifiedName.Empty;
      if (schemaTypeName == XsdInference.QNameAnyType || schemaType == null && schemaTypeName == XmlQualifiedName.Empty)
        return complexType3;
      XmlSchemaSimpleContent schemaSimpleContent = new XmlSchemaSimpleContent();
      complexType3.ContentModel = (XmlSchemaContentModel) schemaSimpleContent;
      if (schemaType is XmlSchemaSimpleType schemaSimpleType)
      {
        schemaSimpleContent.Content = (XmlSchemaContent) new XmlSchemaSimpleContentRestriction()
        {
          BaseType = schemaSimpleType
        };
        return complexType3;
      }
      XmlSchemaSimpleContentExtension contentExtension = new XmlSchemaSimpleContentExtension();
      schemaSimpleContent.Content = (XmlSchemaContent) contentExtension;
      if (XmlSchemaType.GetBuiltInSimpleType(schemaTypeName) != null)
      {
        contentExtension.BaseTypeName = schemaTypeName;
        return complexType3;
      }
      if (!(globalType is XmlSchemaSimpleType))
        throw this.Error((XmlSchemaObject) el, "Unexpected schema component that contains simpleTypeName that could not be resolved.");
      contentExtension.BaseTypeName = schemaTypeName;
      return complexType3;
    }

    private void InferAsEmptyElement(XmlSchemaElement el, string ns, bool isNew)
    {
      if (el.SchemaType is XmlSchemaComplexType schemaType1)
      {
        if (schemaType1.ContentModel is XmlSchemaSimpleContent contentModel2)
          this.ToEmptiableSimpleContent(contentModel2, isNew);
        else if (schemaType1.ContentModel is XmlSchemaComplexContent contentModel1)
        {
          this.ToEmptiableComplexContent(contentModel1, isNew);
        }
        else
        {
          if (schemaType1.Particle == null)
            return;
          schemaType1.Particle.MinOccurs = 0M;
        }
      }
      else
      {
        if (!(el.SchemaType is XmlSchemaSimpleType schemaType))
          return;
        XmlSchemaSimpleType schemaSimpleType = this.MakeBaseTypeAsEmptiable(schemaType);
        string key = schemaSimpleType.QualifiedName.Namespace;
        if (key != null)
        {
          // ISSUE: reference to a compiler-generated field
          if (XsdInference.\u003C\u003Ef__switch\u0024map31 == null)
          {
            // ISSUE: reference to a compiler-generated field
            XsdInference.\u003C\u003Ef__switch\u0024map31 = new Dictionary<string, int>(2)
            {
              {
                "http://www.w3.org/2001/XMLSchema",
                0
              },
              {
                "http://www.w3.org/2003/11/xpath-datatypes",
                0
              }
            };
          }
          int num;
          // ISSUE: reference to a compiler-generated field
          if (XsdInference.\u003C\u003Ef__switch\u0024map31.TryGetValue(key, out num) && num == 0)
          {
            el.SchemaTypeName = schemaSimpleType.QualifiedName;
            return;
          }
        }
        el.SchemaType = (XmlSchemaType) schemaSimpleType;
      }
    }

    private XmlSchemaSimpleType MakeBaseTypeAsEmptiable(XmlSchemaSimpleType st)
    {
      string key = st.QualifiedName.Namespace;
      if (key != null)
      {
        // ISSUE: reference to a compiler-generated field
        if (XsdInference.\u003C\u003Ef__switch\u0024map32 == null)
        {
          // ISSUE: reference to a compiler-generated field
          XsdInference.\u003C\u003Ef__switch\u0024map32 = new Dictionary<string, int>(2)
          {
            {
              "http://www.w3.org/2001/XMLSchema",
              0
            },
            {
              "http://www.w3.org/2003/11/xpath-datatypes",
              0
            }
          };
        }
        int num;
        // ISSUE: reference to a compiler-generated field
        if (XsdInference.\u003C\u003Ef__switch\u0024map32.TryGetValue(key, out num) && num == 0)
          return XmlSchemaType.GetBuiltInSimpleType(XmlTypeCode.String);
      }
      if (st.Content is XmlSchemaSimpleTypeRestriction content)
      {
        ArrayList arrayList = (ArrayList) null;
        foreach (XmlSchemaFacet facet in content.Facets)
        {
          if (facet is XmlSchemaLengthFacet || facet is XmlSchemaMinLengthFacet)
          {
            if (arrayList == null)
              arrayList = new ArrayList();
            arrayList.Add((object) facet);
          }
        }
        foreach (XmlSchemaFacet xmlSchemaFacet in arrayList)
          content.Facets.Remove((XmlSchemaObject) xmlSchemaFacet);
        if (content.BaseType != null)
          content.BaseType = this.MakeBaseTypeAsEmptiable(st);
        else
          content.BaseTypeName = XsdInference.QNameString;
      }
      return st;
    }

    private void ToEmptiableSimpleContent(XmlSchemaSimpleContent sm, bool isNew)
    {
      if (sm.Content is XmlSchemaSimpleContentExtension content1)
      {
        content1.BaseTypeName = XsdInference.QNameString;
      }
      else
      {
        if (!(sm.Content is XmlSchemaSimpleContentRestriction content))
          throw this.Error((XmlSchemaObject) sm, "Invalid simple content model was passed.");
        content.BaseTypeName = XsdInference.QNameString;
        content.BaseType = (XmlSchemaSimpleType) null;
      }
    }

    private void ToEmptiableComplexContent(XmlSchemaComplexContent cm, bool isNew)
    {
      if (cm.Content is XmlSchemaComplexContentExtension content1)
      {
        if (content1.Particle != null)
          content1.Particle.MinOccurs = 0M;
        else if (content1.BaseTypeName != (XmlQualifiedName) null && content1.BaseTypeName != XmlQualifiedName.Empty && content1.BaseTypeName != XsdInference.QNameAnyType)
          throw this.Error((XmlSchemaObject) content1, "Complex type content extension has a reference to an external component that is not supported.");
      }
      else
      {
        if (!(cm.Content is XmlSchemaComplexContentRestriction content))
          throw this.Error((XmlSchemaObject) cm, "Invalid complex content model was passed.");
        if (content.Particle != null)
          content.Particle.MinOccurs = 0M;
        else if (content.BaseTypeName != (XmlQualifiedName) null && content.BaseTypeName != XmlQualifiedName.Empty && content.BaseTypeName != XsdInference.QNameAnyType)
          throw this.Error((XmlSchemaObject) content, "Complex type content extension has a reference to an external component that is not supported.");
      }
    }

    private void InferContent(XmlSchemaElement el, string ns, bool isNew)
    {
      this.source.Read();
      int content1 = (int) this.source.MoveToContent();
      XmlNodeType nodeType = this.source.NodeType;
      switch (nodeType)
      {
        case XmlNodeType.Element:
          this.InferComplexContent(el, ns, isNew);
          break;
        case XmlNodeType.Text:
        case XmlNodeType.CDATA:
label_4:
          this.InferTextContent(el, isNew);
          int content2 = (int) this.source.MoveToContent();
          if (this.source.NodeType != XmlNodeType.Element)
            break;
          goto case XmlNodeType.Element;
        default:
          switch (nodeType - 13)
          {
            case XmlNodeType.None:
              this.InferContent(el, ns, isNew);
              return;
            case XmlNodeType.Element:
              goto label_4;
            case XmlNodeType.Attribute:
              this.InferAsEmptyElement(el, ns, isNew);
              return;
            default:
              return;
          }
      }
    }

    private void InferComplexContent(XmlSchemaElement el, string ns, bool isNew)
    {
      XmlSchemaComplexType complexType = this.ToComplexType(el);
      this.ToComplexContentType(complexType);
      int position = 0;
      bool consumed = false;
      while (true)
      {
        XmlNodeType nodeType;
        do
        {
          nodeType = this.source.NodeType;
          switch (nodeType)
          {
            case XmlNodeType.None:
              goto label_9;
            case XmlNodeType.Element:
              goto label_4;
            case XmlNodeType.Text:
            case XmlNodeType.CDATA:
              goto label_8;
            default:
              if (nodeType != XmlNodeType.SignificantWhitespace)
                continue;
              goto label_8;
          }
        }
        while (nodeType != XmlNodeType.EndElement);
        break;
label_4:
        XmlSchemaSequence s = this.PopulateSequence(complexType);
        XmlSchemaChoice c = s.Items.Count <= 0 ? (XmlSchemaChoice) null : s.Items[0] as XmlSchemaChoice;
        if (c != null)
          this.ProcessLax(c, ns);
        else
          this.ProcessSequence(complexType, s, ns, ref position, ref consumed, isNew);
        int content1 = (int) this.source.MoveToContent();
        continue;
label_8:
        this.MarkAsMixed(complexType);
        this.source.ReadString();
        int content2 = (int) this.source.MoveToContent();
      }
      return;
label_9:
      throw new NotImplementedException("Internal Error: Should not happen.");
    }

    private void InferTextContent(XmlSchemaElement el, bool isNew)
    {
      string str = this.source.ReadString();
      if (el.SchemaType == null)
      {
        if (el.SchemaTypeName == XmlQualifiedName.Empty)
        {
          if (isNew)
            el.SchemaTypeName = this.InferSimpleType(str);
          else
            el.SchemaTypeName = XsdInference.QNameString;
        }
        else
        {
          string key = el.SchemaTypeName.Namespace;
          if (key != null)
          {
            // ISSUE: reference to a compiler-generated field
            if (XsdInference.\u003C\u003Ef__switch\u0024map33 == null)
            {
              // ISSUE: reference to a compiler-generated field
              XsdInference.\u003C\u003Ef__switch\u0024map33 = new Dictionary<string, int>(2)
              {
                {
                  "http://www.w3.org/2001/XMLSchema",
                  0
                },
                {
                  "http://www.w3.org/2003/11/xpath-datatypes",
                  0
                }
              };
            }
            int num;
            // ISSUE: reference to a compiler-generated field
            if (XsdInference.\u003C\u003Ef__switch\u0024map33.TryGetValue(key, out num) && num == 0)
            {
              el.SchemaTypeName = this.InferMergedType(str, el.SchemaTypeName);
              return;
            }
          }
          if (this.schemas.GlobalTypes[el.SchemaTypeName] is XmlSchemaComplexType globalType)
            this.MarkAsMixed(globalType);
          else
            el.SchemaTypeName = XsdInference.QNameString;
        }
      }
      else if (el.SchemaType is XmlSchemaSimpleType)
      {
        el.SchemaType = (XmlSchemaType) null;
        el.SchemaTypeName = XsdInference.QNameString;
      }
      else
      {
        XmlSchemaComplexType schemaType = el.SchemaType as XmlSchemaComplexType;
        if (!(schemaType.ContentModel is XmlSchemaSimpleContent contentModel))
        {
          this.MarkAsMixed(schemaType);
        }
        else
        {
          if (contentModel.Content is XmlSchemaSimpleContentExtension content1)
            content1.BaseTypeName = this.InferMergedType(str, content1.BaseTypeName);
          if (!(contentModel.Content is XmlSchemaSimpleContentRestriction content2))
            return;
          content2.BaseTypeName = this.InferMergedType(str, content2.BaseTypeName);
          content2.BaseType = (XmlSchemaSimpleType) null;
        }
      }
    }

    private void MarkAsMixed(XmlSchemaComplexType ct)
    {
      if (ct.ContentModel is XmlSchemaComplexContent contentModel)
        contentModel.IsMixed = true;
      else
        ct.IsMixed = true;
    }

    private void ProcessLax(XmlSchemaChoice c, string ns)
    {
      foreach (XmlSchemaParticle xmlSchemaParticle in c.Items)
      {
        if (!(xmlSchemaParticle is XmlSchemaElement el))
          throw this.Error((XmlSchemaObject) c, string.Format("Target schema item contains unacceptable particle {0}. Only element is allowed here."));
        if (this.ElementMatches(el, ns))
        {
          this.InferElement(el, ns, false);
          return;
        }
      }
      XmlSchemaElement el1 = new XmlSchemaElement();
      if (this.source.NamespaceURI == ns)
      {
        el1.Name = this.source.LocalName;
      }
      else
      {
        el1.RefName = new XmlQualifiedName(this.source.LocalName, this.source.NamespaceURI);
        this.AddImport(ns, this.source.NamespaceURI);
      }
      this.InferElement(el1, this.source.NamespaceURI, true);
      c.Items.Add((XmlSchemaObject) el1);
    }

    private bool ElementMatches(XmlSchemaElement el, string ns)
    {
      bool flag = false;
      if (el.RefName != XmlQualifiedName.Empty)
      {
        if (el.RefName.Name == this.source.LocalName && el.RefName.Namespace == this.source.NamespaceURI)
          flag = true;
      }
      else if (el.Name == this.source.LocalName && ns == this.source.NamespaceURI)
        flag = true;
      return flag;
    }

    private void ProcessSequence(
      XmlSchemaComplexType ct,
      XmlSchemaSequence s,
      string ns,
      ref int position,
      ref bool consumed,
      bool isNew)
    {
      for (int index = 0; index < position; ++index)
      {
        if (this.ElementMatches(s.Items[index] as XmlSchemaElement, ns))
        {
          this.ProcessLax(this.ToSequenceOfChoice(s), ns);
          return;
        }
      }
      if (s.Items.Count <= position)
      {
        XmlQualifiedName name = new XmlQualifiedName(this.source.LocalName, this.source.NamespaceURI);
        XmlSchemaElement element = this.CreateElement(name);
        if (this.laxOccurrence)
          element.MinOccurs = 0M;
        this.InferElement(element, ns, true);
        if (ns == name.Namespace)
        {
          s.Items.Add((XmlSchemaObject) element);
        }
        else
        {
          XmlSchemaElement xmlSchemaElement = new XmlSchemaElement();
          if (this.laxOccurrence)
            xmlSchemaElement.MinOccurs = 0M;
          xmlSchemaElement.RefName = name;
          this.AddImport(ns, name.Namespace);
          s.Items.Add((XmlSchemaObject) xmlSchemaElement);
        }
        consumed = true;
      }
      else
      {
        if (!(s.Items[position] is XmlSchemaElement el))
          throw this.Error((XmlSchemaObject) s, string.Format("Target complex type content sequence has an unacceptable type of particle {0}", (object) s.Items[position]));
        if (this.ElementMatches(el, ns))
        {
          if (consumed)
            el.MaxOccursString = "unbounded";
          this.InferElement(el, this.source.NamespaceURI, false);
          int content = (int) this.source.MoveToContent();
          XmlNodeType nodeType = this.source.NodeType;
          switch (nodeType)
          {
            case XmlNodeType.None:
label_21:
              if (this.source.NodeType != XmlNodeType.Element)
              {
                if (this.source.NodeType != XmlNodeType.EndElement)
                  break;
                break;
              }
              goto case XmlNodeType.Element;
            case XmlNodeType.Element:
              this.ProcessSequence(ct, s, ns, ref position, ref consumed, isNew);
              break;
            case XmlNodeType.Text:
            case XmlNodeType.CDATA:
label_25:
              this.MarkAsMixed(ct);
              this.source.ReadString();
              goto case XmlNodeType.None;
            default:
              switch (nodeType - 13)
              {
                case XmlNodeType.None:
                  this.source.ReadString();
                  goto label_21;
                case XmlNodeType.Element:
                  goto label_25;
                case XmlNodeType.Attribute:
                  return;
                default:
                  this.source.Read();
                  return;
              }
          }
        }
        else if (consumed)
        {
          ++position;
          consumed = false;
          this.ProcessSequence(ct, s, ns, ref position, ref consumed, isNew);
        }
        else
          this.ProcessLax(this.ToSequenceOfChoice(s), ns);
      }
    }

    private XmlSchemaChoice ToSequenceOfChoice(XmlSchemaSequence s)
    {
      XmlSchemaChoice sequenceOfChoice = new XmlSchemaChoice();
      if (this.laxOccurrence)
        sequenceOfChoice.MinOccurs = 0M;
      sequenceOfChoice.MaxOccursString = "unbounded";
      foreach (XmlSchemaParticle xmlSchemaParticle in s.Items)
        sequenceOfChoice.Items.Add((XmlSchemaObject) xmlSchemaParticle);
      s.Items.Clear();
      s.Items.Add((XmlSchemaObject) sequenceOfChoice);
      return sequenceOfChoice;
    }

    private void ToComplexContentType(XmlSchemaComplexType type)
    {
      if (!(type.ContentModel is XmlSchemaSimpleContent))
        return;
      foreach (XmlSchemaObject attribute in this.GetAttributes(type))
        type.Attributes.Add(attribute);
      type.ContentModel = (XmlSchemaContentModel) null;
      type.IsMixed = true;
    }

    private XmlSchemaSequence PopulateSequence(XmlSchemaComplexType ct)
    {
      XmlSchemaParticle xmlSchemaParticle = this.PopulateParticle(ct);
      return xmlSchemaParticle is XmlSchemaSequence xmlSchemaSequence ? xmlSchemaSequence : throw this.Error((XmlSchemaObject) ct, string.Format("Target complexType contains unacceptable type of particle {0}", (object) xmlSchemaParticle));
    }

    private XmlSchemaSequence CreateSequence()
    {
      XmlSchemaSequence sequence = new XmlSchemaSequence();
      if (this.laxOccurrence)
        sequence.MinOccurs = 0M;
      return sequence;
    }

    private XmlSchemaParticle PopulateParticle(XmlSchemaComplexType ct)
    {
      if (ct.ContentModel == null)
      {
        if (ct.Particle == null)
          ct.Particle = (XmlSchemaParticle) this.CreateSequence();
        return ct.Particle;
      }
      if (ct.ContentModel is XmlSchemaComplexContent contentModel)
      {
        if (contentModel.Content is XmlSchemaComplexContentExtension content1)
        {
          if (content1.Particle == null)
            content1.Particle = (XmlSchemaParticle) this.CreateSequence();
          return content1.Particle;
        }
        if (contentModel.Content is XmlSchemaComplexContentRestriction content2)
        {
          if (content2.Particle == null)
            content2.Particle = (XmlSchemaParticle) this.CreateSequence();
          return content2.Particle;
        }
      }
      throw this.Error((XmlSchemaObject) ct, "Schema inference internal error. The complexType should have been converted to have a complex content.");
    }

    private XmlQualifiedName InferSimpleType(string value)
    {
      if (this.laxTypeInference)
        return XsdInference.QNameString;
      string key = value;
      if (key != null)
      {
        // ISSUE: reference to a compiler-generated field
        if (XsdInference.\u003C\u003Ef__switch\u0024map34 == null)
        {
          // ISSUE: reference to a compiler-generated field
          XsdInference.\u003C\u003Ef__switch\u0024map34 = new Dictionary<string, int>(2)
          {
            {
              "true",
              0
            },
            {
              "false",
              0
            }
          };
        }
        int num;
        // ISSUE: reference to a compiler-generated field
        if (XsdInference.\u003C\u003Ef__switch\u0024map34.TryGetValue(key, out num))
        {
          if (num == 0)
            return XsdInference.QNameBoolean;
        }
      }
      try
      {
        long int64 = XmlConvert.ToInt64(value);
        if (0L <= int64 && int64 <= (long) byte.MaxValue)
          return XsdInference.QNameUByte;
        if ((long) sbyte.MinValue <= int64 && int64 <= (long) sbyte.MaxValue)
          return XsdInference.QNameByte;
        if (0L <= int64 && int64 <= (long) ushort.MaxValue)
          return XsdInference.QNameUShort;
        if ((long) short.MinValue <= int64 && int64 <= (long) short.MaxValue)
          return XsdInference.QNameShort;
        if (0L <= int64 && int64 <= (long) uint.MaxValue)
          return XsdInference.QNameUInt;
        return (long) int.MinValue <= int64 && int64 <= (long) int.MaxValue ? XsdInference.QNameInt : XsdInference.QNameLong;
      }
      catch (Exception ex)
      {
      }
      try
      {
        long uint64 = (long) XmlConvert.ToUInt64(value);
        return XsdInference.QNameULong;
      }
      catch (Exception ex)
      {
      }
      try
      {
        XmlConvert.ToDecimal(value);
        return XsdInference.QNameDecimal;
      }
      catch (Exception ex)
      {
      }
      try
      {
        double num = XmlConvert.ToDouble(value);
        return -3.4028234663852886E+38 <= num && num <= 3.4028234663852886E+38 ? XsdInference.QNameFloat : XsdInference.QNameDouble;
      }
      catch (Exception ex)
      {
      }
      try
      {
        XmlConvert.ToDateTime(value);
        return XsdInference.QNameDateTime;
      }
      catch (Exception ex)
      {
      }
      try
      {
        XmlConvert.ToTimeSpan(value);
        return XsdInference.QNameDuration;
      }
      catch (Exception ex)
      {
      }
      return XsdInference.QNameString;
    }

    private XmlSchemaElement GetGlobalElement(XmlQualifiedName name)
    {
      if (!(this.newElements[(object) name] is XmlSchemaElement globalElement))
        globalElement = this.schemas.GlobalElements[name] as XmlSchemaElement;
      return globalElement;
    }

    private XmlSchemaAttribute GetGlobalAttribute(XmlQualifiedName name)
    {
      if (!(this.newElements[(object) name] is XmlSchemaAttribute globalAttribute))
        globalAttribute = this.schemas.GlobalAttributes[name] as XmlSchemaAttribute;
      return globalAttribute;
    }

    private XmlSchemaElement CreateElement(XmlQualifiedName name) => new XmlSchemaElement()
    {
      Name = name.Name
    };

    private XmlSchemaElement CreateGlobalElement(XmlQualifiedName name)
    {
      XmlSchemaElement element = this.CreateElement(name);
      this.PopulateSchema(name.Namespace).Items.Add((XmlSchemaObject) element);
      this.newElements.Add((object) name, (object) element);
      return element;
    }

    private XmlSchemaAttribute CreateGlobalAttribute(XmlQualifiedName name)
    {
      XmlSchemaAttribute globalAttribute = new XmlSchemaAttribute();
      XmlSchema xmlSchema = this.PopulateSchema(name.Namespace);
      globalAttribute.Name = name.Name;
      xmlSchema.Items.Add((XmlSchemaObject) globalAttribute);
      this.newAttributes.Add((object) name, (object) globalAttribute);
      return globalAttribute;
    }

    private XmlSchema PopulateSchema(string ns)
    {
      ICollection collection = this.schemas.Schemas(ns);
      if (collection.Count > 0)
      {
        IEnumerator enumerator = collection.GetEnumerator();
        enumerator.MoveNext();
        return (XmlSchema) enumerator.Current;
      }
      XmlSchema schema = new XmlSchema();
      if (ns != null && ns.Length > 0)
        schema.TargetNamespace = ns;
      schema.ElementFormDefault = XmlSchemaForm.Qualified;
      schema.AttributeFormDefault = XmlSchemaForm.Unqualified;
      this.schemas.Add(schema);
      return schema;
    }

    private XmlSchemaInferenceException Error(XmlSchemaObject sourceObj, string message) => this.Error(sourceObj, false, message);

    private XmlSchemaInferenceException Error(
      XmlSchemaObject sourceObj,
      bool useReader,
      string message)
    {
      string message1 = message + (sourceObj == null ? string.Empty : string.Format(". Related schema component is {0}", (object) sourceObj.SourceUri, (object) sourceObj.LineNumber, (object) sourceObj.LinePosition)) + (!useReader ? string.Empty : string.Format(". {0}", (object) this.source.BaseURI));
      IXmlLineInfo source = this.source as IXmlLineInfo;
      return useReader && source != null ? new XmlSchemaInferenceException(message1, (Exception) null, source.LineNumber, source.LinePosition) : new XmlSchemaInferenceException(message1);
    }
  }
}
