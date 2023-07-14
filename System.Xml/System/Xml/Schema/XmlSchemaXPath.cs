// Decompiled with JetBrains decompiler
// Type: System.Xml.Schema.XmlSchemaXPath
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using Mono.Xml.Schema;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
  public class XmlSchemaXPath : XmlSchemaAnnotated
  {
    private string xpath;
    private XmlNamespaceManager nsmgr;
    internal bool isSelector;
    private XsdIdentityPath[] compiledExpression;
    private XsdIdentityPath currentPath;

    [XmlAttribute("xpath")]
    [DefaultValue("")]
    public string XPath
    {
      get => this.xpath;
      set => this.xpath = value;
    }

    internal override int Compile(ValidationEventHandler h, XmlSchema schema)
    {
      if (this.CompilationId == schema.CompilationId)
        return 0;
      if (this.nsmgr == null)
      {
        this.nsmgr = new XmlNamespaceManager((XmlNameTable) new NameTable());
        if (this.Namespaces != null)
        {
          foreach (XmlQualifiedName xmlQualifiedName in this.Namespaces.ToArray())
            this.nsmgr.AddNamespace(xmlQualifiedName.Name, xmlQualifiedName.Namespace);
        }
      }
      this.currentPath = new XsdIdentityPath();
      this.ParseExpression(this.xpath, h, schema);
      XmlSchemaUtil.CompileID(this.Id, (XmlSchemaObject) this, schema.IDCollection, h);
      this.CompilationId = schema.CompilationId;
      return this.errorCount;
    }

    internal XsdIdentityPath[] CompiledExpression => this.compiledExpression;

    private void ParseExpression(string xpath, ValidationEventHandler h, XmlSchema schema)
    {
      ArrayList paths = new ArrayList();
      this.ParsePath(xpath, 0, paths, h, schema);
      this.compiledExpression = (XsdIdentityPath[]) paths.ToArray(typeof (XsdIdentityPath));
    }

    private void ParsePath(
      string xpath,
      int pos,
      ArrayList paths,
      ValidationEventHandler h,
      XmlSchema schema)
    {
      pos = this.SkipWhitespace(xpath, pos);
      if (xpath.Length >= pos + 3 && xpath[pos] == '.')
      {
        int num = pos;
        ++pos;
        pos = this.SkipWhitespace(xpath, pos);
        if (xpath.Length > pos + 2 && xpath.IndexOf("//", pos, 2) == pos)
        {
          this.currentPath.Descendants = true;
          pos += 2;
        }
        else
          pos = num;
      }
      ArrayList steps = new ArrayList();
      this.ParseStep(xpath, pos, steps, paths, h, schema);
    }

    private void ParseStep(
      string xpath,
      int pos,
      ArrayList steps,
      ArrayList paths,
      ValidationEventHandler h,
      XmlSchema schema)
    {
      pos = this.SkipWhitespace(xpath, pos);
      if (xpath.Length == pos)
      {
        this.error(h, "Empty xpath expression is specified");
      }
      else
      {
        XsdIdentityStep xsdIdentityStep = new XsdIdentityStep();
        char ch = xpath[pos];
        switch (ch)
        {
          case 'a':
            if (xpath.Length > pos + 9 && xpath.IndexOf("attribute", pos, 9) == pos)
            {
              int num = pos;
              pos += 9;
              pos = this.SkipWhitespace(xpath, pos);
              if (xpath.Length > pos && xpath[pos] == ':' && xpath[pos + 1] == ':')
              {
                if (this.isSelector)
                {
                  this.error(h, "Selector cannot include attribute axes.");
                  this.currentPath = (XsdIdentityPath) null;
                  return;
                }
                pos += 2;
                xsdIdentityStep.IsAttribute = true;
                if (xpath.Length > pos && xpath[pos] == '*')
                {
                  ++pos;
                  xsdIdentityStep.IsAnyName = true;
                  goto label_42;
                }
                else
                {
                  pos = this.SkipWhitespace(xpath, pos);
                  break;
                }
              }
              else
              {
                pos = num;
                break;
              }
            }
            else
              break;
          case 'c':
            if (xpath.Length > pos + 5 && xpath.IndexOf("child", pos, 5) == pos)
            {
              int num = pos;
              pos += 5;
              pos = this.SkipWhitespace(xpath, pos);
              if (xpath.Length > pos && xpath[pos] == ':' && xpath[pos + 1] == ':')
              {
                pos += 2;
                if (xpath.Length > pos && xpath[pos] == '*')
                {
                  ++pos;
                  xsdIdentityStep.IsAnyName = true;
                  goto label_42;
                }
                else
                {
                  pos = this.SkipWhitespace(xpath, pos);
                  break;
                }
              }
              else
              {
                pos = num;
                break;
              }
            }
            else
              break;
          default:
            switch (ch)
            {
              case '*':
                ++pos;
                xsdIdentityStep.IsAnyName = true;
                goto label_42;
              case '.':
                ++pos;
                xsdIdentityStep.IsCurrent = true;
                goto label_42;
              case '@':
                if (this.isSelector)
                {
                  this.error(h, "Selector cannot include attribute axes.");
                  this.currentPath = (XsdIdentityPath) null;
                  return;
                }
                ++pos;
                xsdIdentityStep.IsAttribute = true;
                pos = this.SkipWhitespace(xpath, pos);
                if (xpath.Length > pos && xpath[pos] == '*')
                {
                  ++pos;
                  xsdIdentityStep.IsAnyName = true;
                  goto label_42;
                }
                else
                  break;
            }
            break;
        }
        int startIndex1 = pos;
        while (xpath.Length > pos && XmlChar.IsNCNameChar((int) xpath[pos]))
          ++pos;
        if (pos == startIndex1)
        {
          this.error(h, "Invalid path format for a field.");
          this.currentPath = (XsdIdentityPath) null;
          return;
        }
        if (xpath.Length == pos || xpath[pos] != ':')
        {
          xsdIdentityStep.Name = xpath.Substring(startIndex1, pos - startIndex1);
        }
        else
        {
          string prefix = xpath.Substring(startIndex1, pos - startIndex1);
          ++pos;
          if (xpath.Length > pos && xpath[pos] == '*')
          {
            string str = this.nsmgr.LookupNamespace(prefix, false);
            if (str == null)
            {
              this.error(h, "Specified prefix '" + prefix + "' is not declared.");
              this.currentPath = (XsdIdentityPath) null;
              return;
            }
            xsdIdentityStep.NsName = str;
            ++pos;
          }
          else
          {
            int startIndex2 = pos;
            while (xpath.Length > pos && XmlChar.IsNCNameChar((int) xpath[pos]))
              ++pos;
            xsdIdentityStep.Name = xpath.Substring(startIndex2, pos - startIndex2);
            string str = this.nsmgr.LookupNamespace(prefix, false);
            if (str == null)
            {
              this.error(h, "Specified prefix '" + prefix + "' is not declared.");
              this.currentPath = (XsdIdentityPath) null;
              return;
            }
            xsdIdentityStep.Namespace = str;
          }
        }
label_42:
        if (!xsdIdentityStep.IsCurrent)
          steps.Add((object) xsdIdentityStep);
        pos = this.SkipWhitespace(xpath, pos);
        if (xpath.Length == pos)
        {
          this.currentPath.OrderedSteps = (XsdIdentityStep[]) steps.ToArray(typeof (XsdIdentityStep));
          paths.Add((object) this.currentPath);
        }
        else if (xpath[pos] == '/')
        {
          ++pos;
          if (xsdIdentityStep.IsAttribute)
          {
            this.error(h, "Unexpected xpath token after Attribute NameTest.");
            this.currentPath = (XsdIdentityPath) null;
          }
          else
          {
            this.ParseStep(xpath, pos, steps, paths, h, schema);
            if (this.currentPath == null)
              return;
            this.currentPath.OrderedSteps = (XsdIdentityStep[]) steps.ToArray(typeof (XsdIdentityStep));
          }
        }
        else if (xpath[pos] == '|')
        {
          ++pos;
          this.currentPath.OrderedSteps = (XsdIdentityStep[]) steps.ToArray(typeof (XsdIdentityStep));
          paths.Add((object) this.currentPath);
          this.currentPath = new XsdIdentityPath();
          this.ParsePath(xpath, pos, paths, h, schema);
        }
        else
        {
          this.error(h, "Unexpected xpath token after NameTest.");
          this.currentPath = (XsdIdentityPath) null;
        }
      }
    }

    private int SkipWhitespace(string xpath, int pos)
    {
      bool flag = true;
      while (flag && xpath.Length > pos)
      {
        char ch = xpath[pos];
        switch (ch)
        {
          case '\t':
          case '\n':
          case '\r':
            ++pos;
            continue;
          default:
            if (ch != ' ')
            {
              flag = false;
              continue;
            }
            goto case '\t';
        }
      }
      return pos;
    }

    internal static XmlSchemaXPath Read(
      XmlSchemaReader reader,
      ValidationEventHandler h,
      string name)
    {
      XmlSchemaXPath xso = new XmlSchemaXPath();
      reader.MoveToElement();
      if (reader.NamespaceURI != "http://www.w3.org/2001/XMLSchema" || reader.LocalName != name)
      {
        XmlSchemaObject.error(h, "Should not happen :1: XmlSchemaComplexContentRestriction.Read, name=" + reader.Name, (Exception) null);
        reader.Skip();
        return (XmlSchemaXPath) null;
      }
      xso.LineNumber = reader.LineNumber;
      xso.LinePosition = reader.LinePosition;
      xso.SourceUri = reader.BaseURI;
      XmlNamespaceManager namespaceManager = XmlSchemaUtil.GetParserContext(reader.Reader).NamespaceManager;
      if (namespaceManager != null)
      {
        xso.nsmgr = new XmlNamespaceManager(reader.NameTable);
        foreach (object obj in namespaceManager)
        {
          string prefix = obj as string;
          string key = prefix;
          if (key != null)
          {
            // ISSUE: reference to a compiler-generated field
            if (XmlSchemaXPath.\u003C\u003Ef__switch\u0024map3C == null)
            {
              // ISSUE: reference to a compiler-generated field
              XmlSchemaXPath.\u003C\u003Ef__switch\u0024map3C = new Dictionary<string, int>(2)
              {
                {
                  "xml",
                  0
                },
                {
                  "xmlns",
                  0
                }
              };
            }
            int num;
            // ISSUE: reference to a compiler-generated field
            if (XmlSchemaXPath.\u003C\u003Ef__switch\u0024map3C.TryGetValue(key, out num) && num == 0)
              continue;
          }
          xso.nsmgr.AddNamespace(prefix, namespaceManager.LookupNamespace(prefix, false));
        }
      }
      while (reader.MoveToNextAttribute())
      {
        if (reader.Name == "id")
          xso.Id = reader.Value;
        else if (reader.Name == "xpath")
          xso.xpath = reader.Value;
        else if (reader.NamespaceURI == string.Empty && reader.Name != "xmlns" || reader.NamespaceURI == "http://www.w3.org/2001/XMLSchema")
          XmlSchemaObject.error(h, reader.Name + " is not a valid attribute for " + name, (Exception) null);
        else
          XmlSchemaUtil.ReadUnhandledAttribute((XmlReader) reader, (XmlSchemaObject) xso);
      }
      reader.MoveToElement();
      if (reader.IsEmptyElement)
        return xso;
      int num1 = 1;
      while (reader.ReadNextElement())
      {
        if (reader.NodeType == XmlNodeType.EndElement)
        {
          if (reader.LocalName != name)
          {
            XmlSchemaObject.error(h, "Should not happen :2: XmlSchemaXPath.Read, name=" + reader.Name, (Exception) null);
            break;
          }
          break;
        }
        if (num1 <= 1 && reader.LocalName == "annotation")
        {
          num1 = 2;
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
