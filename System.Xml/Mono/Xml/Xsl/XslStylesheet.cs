// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.XslStylesheet
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using Mono.Xml.Xsl.Operations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.Xsl
{
  internal class XslStylesheet
  {
    public const string XsltNamespace = "http://www.w3.org/1999/XSL/Transform";
    public const string MSXsltNamespace = "urn:schemas-microsoft-com:xslt";
    private ArrayList imports = new ArrayList();
    private Hashtable spaceControls = new Hashtable();
    private NameValueCollection namespaceAliases = new NameValueCollection();
    private Hashtable parameters = new Hashtable();
    private Hashtable keys = new Hashtable();
    private Hashtable variables = new Hashtable();
    private XslTemplateTable templates;
    private string baseURI;
    private string version;
    private XmlQualifiedName[] extensionElementPrefixes;
    private XmlQualifiedName[] excludeResultPrefixes;
    private ArrayList stylesheetNamespaces = new ArrayList();
    private Hashtable inProcessIncludes = new Hashtable();
    private bool countedSpaceControlExistence;
    private bool cachedHasSpaceControls;
    private static readonly XmlQualifiedName allMatchName = new XmlQualifiedName("*");
    private bool countedNamespaceAliases;
    private bool cachedHasNamespaceAliases;

    public XmlQualifiedName[] ExtensionElementPrefixes => this.extensionElementPrefixes;

    public XmlQualifiedName[] ExcludeResultPrefixes => this.excludeResultPrefixes;

    public ArrayList StylesheetNamespaces => this.stylesheetNamespaces;

    public ArrayList Imports => this.imports;

    public Hashtable SpaceControls => this.spaceControls;

    public NameValueCollection NamespaceAliases => this.namespaceAliases;

    public Hashtable Parameters => this.parameters;

    public XslTemplateTable Templates => this.templates;

    public string BaseURI => this.baseURI;

    public string Version => this.version;

    internal void Compile(Compiler c)
    {
      c.PushStylesheet(this);
      this.templates = new XslTemplateTable(this);
      this.baseURI = c.Input.BaseURI;
      while (c.Input.NodeType != XPathNodeType.Element)
      {
        if (!c.Input.MoveToNext())
          throw new XsltCompileException("Stylesheet root element must be either \"stylesheet\" or \"transform\" or any literal element", (Exception) null, c.Input);
      }
      if (c.Input.NamespaceURI != "http://www.w3.org/1999/XSL/Transform")
      {
        if (c.Input.GetAttribute("version", "http://www.w3.org/1999/XSL/Transform") == string.Empty)
          throw new XsltCompileException("Mandatory global attribute version is missing", (Exception) null, c.Input);
        this.templates.Add(new XslTemplate(c));
      }
      else
      {
        if (c.Input.LocalName != "stylesheet" && c.Input.LocalName != "transform")
          throw new XsltCompileException("Stylesheet root element must be either \"stylesheet\" or \"transform\" or any literal element", (Exception) null, c.Input);
        this.version = c.Input.GetAttribute("version", string.Empty);
        if (this.version == string.Empty)
          throw new XsltCompileException("Mandatory attribute version is missing", (Exception) null, c.Input);
        this.extensionElementPrefixes = this.ParseMappedPrefixes(c.GetAttribute("extension-element-prefixes"), c.Input);
        this.excludeResultPrefixes = this.ParseMappedPrefixes(c.GetAttribute("exclude-result-prefixes"), c.Input);
        if (c.Input.MoveToFirstNamespace(XPathNamespaceScope.Local))
        {
          do
          {
            if (!(c.Input.Value == "http://www.w3.org/1999/XSL/Transform"))
              this.stylesheetNamespaces.Insert(0, (object) new XmlQualifiedName(c.Input.Name, c.Input.Value));
          }
          while (c.Input.MoveToNextNamespace(XPathNamespaceScope.Local));
          c.Input.MoveToParent();
        }
        this.ProcessTopLevelElements(c);
      }
      foreach (XslGlobalVariable var in (IEnumerable) this.variables.Values)
        c.AddGlobalVariable(var);
      foreach (ArrayList arrayList in (IEnumerable) this.keys.Values)
      {
        for (int index = 0; index < arrayList.Count; ++index)
          c.AddKey((XslKey) arrayList[index]);
      }
      c.PopStylesheet();
      this.inProcessIncludes = (Hashtable) null;
    }

    private XmlQualifiedName[] ParseMappedPrefixes(string list, XPathNavigator nav)
    {
      if (list == null)
        return (XmlQualifiedName[]) null;
      ArrayList arrayList = new ArrayList();
      foreach (string name in list.Split(XmlChar.WhitespaceChars))
      {
        switch (name)
        {
          case "":
            continue;
          case "#default":
            arrayList.Add((object) new XmlQualifiedName(string.Empty, string.Empty));
            continue;
          default:
            string ns = nav.GetNamespace(name);
            if (ns != string.Empty)
            {
              arrayList.Add((object) new XmlQualifiedName(name, ns));
              continue;
            }
            continue;
        }
      }
      return (XmlQualifiedName[]) arrayList.ToArray(typeof (XmlQualifiedName));
    }

    public bool HasSpaceControls
    {
      get
      {
        if (!this.countedSpaceControlExistence)
        {
          this.countedSpaceControlExistence = true;
          this.cachedHasSpaceControls = this.ComputeHasSpaceControls();
        }
        return this.cachedHasSpaceControls;
      }
    }

    private bool ComputeHasSpaceControls()
    {
      if (this.spaceControls.Count > 0 && this.HasStripSpace((IDictionary) this.spaceControls))
        return true;
      if (this.imports.Count == 0)
        return false;
      for (int index = 0; index < this.imports.Count; ++index)
      {
        XslStylesheet import = (XslStylesheet) this.imports[index];
        if (import.spaceControls.Count > 0 && this.HasStripSpace((IDictionary) import.spaceControls))
          return true;
      }
      return false;
    }

    private bool HasStripSpace(IDictionary table)
    {
      foreach (int num in (IEnumerable) table.Values)
      {
        if (num == 1)
          return true;
      }
      return false;
    }

    public bool GetPreserveWhitespace(XPathNavigator nav)
    {
      if (!this.HasSpaceControls)
        return true;
      nav = nav.Clone();
      if (!nav.MoveToParent() || nav.NodeType != XPathNodeType.Element)
      {
        object defaultXmlSpace = this.GetDefaultXmlSpace();
        return defaultXmlSpace == null || (int) defaultXmlSpace == 2;
      }
      string localName = nav.LocalName;
      string namespaceUri = nav.NamespaceURI;
      XmlQualifiedName key1 = new XmlQualifiedName(localName, namespaceUri);
      object obj = this.spaceControls[(object) key1];
      if (obj == null)
      {
        for (int index = 0; index < this.imports.Count; ++index)
        {
          obj = ((XslStylesheet) this.imports[index]).SpaceControls[(object) key1];
          if (obj != null)
            break;
        }
      }
      if (obj == null)
      {
        XmlQualifiedName key2 = new XmlQualifiedName("*", namespaceUri);
        obj = this.spaceControls[(object) key2];
        if (obj == null)
        {
          for (int index = 0; index < this.imports.Count; ++index)
          {
            obj = ((XslStylesheet) this.imports[index]).SpaceControls[(object) key2];
            if (obj != null)
              break;
          }
        }
      }
      if (obj == null)
        obj = this.GetDefaultXmlSpace();
      if (obj != null)
      {
        switch ((XmlSpace) obj)
        {
          case XmlSpace.Default:
            return false;
          case XmlSpace.Preserve:
            return true;
        }
      }
      throw new SystemException("Mono BUG: should not reach here");
    }

    private object GetDefaultXmlSpace()
    {
      object spaceControl = this.spaceControls[(object) XslStylesheet.allMatchName];
      if (spaceControl == null)
      {
        for (int index = 0; index < this.imports.Count; ++index)
        {
          spaceControl = ((XslStylesheet) this.imports[index]).SpaceControls[(object) XslStylesheet.allMatchName];
          if (spaceControl != null)
            break;
        }
      }
      return spaceControl;
    }

    public bool HasNamespaceAliases
    {
      get
      {
        if (!this.countedNamespaceAliases)
        {
          this.countedNamespaceAliases = true;
          if (this.namespaceAliases.Count > 0)
            this.cachedHasNamespaceAliases = true;
          else if (this.imports.Count == 0)
          {
            this.cachedHasNamespaceAliases = false;
          }
          else
          {
            for (int index = 0; index < this.imports.Count; ++index)
            {
              if (((XslStylesheet) this.imports[index]).namespaceAliases.Count > 0)
                this.countedNamespaceAliases = true;
            }
            this.cachedHasNamespaceAliases = false;
          }
        }
        return this.cachedHasNamespaceAliases;
      }
    }

    public string GetActualPrefix(string prefix)
    {
      if (!this.HasNamespaceAliases)
        return prefix;
      string namespaceAlias = this.namespaceAliases[prefix];
      if (namespaceAlias == null)
      {
        for (int index = 0; index < this.imports.Count; ++index)
        {
          namespaceAlias = ((XslStylesheet) this.imports[index]).namespaceAliases[prefix];
          if (namespaceAlias != null)
            break;
        }
      }
      return namespaceAlias ?? prefix;
    }

    private void StoreInclude(Compiler c)
    {
      XPathNavigator key = c.Input.Clone();
      c.PushInputDocument(c.Input.GetAttribute("href", string.Empty));
      this.inProcessIncludes[(object) key] = (object) c.Input;
      this.HandleImportsInInclude(c);
      c.PopInputDocument();
    }

    private void HandleImportsInInclude(Compiler c)
    {
      if (c.Input.NamespaceURI != "http://www.w3.org/1999/XSL/Transform")
      {
        if (c.Input.GetAttribute("version", "http://www.w3.org/1999/XSL/Transform") == string.Empty)
          throw new XsltCompileException("Mandatory global attribute version is missing", (Exception) null, c.Input);
      }
      else if (!c.Input.MoveToFirstChild())
        c.Input.MoveToRoot();
      else
        this.HandleIncludesImports(c);
    }

    private void HandleInclude(Compiler c)
    {
      XPathNavigator nav = (XPathNavigator) null;
      foreach (XPathNavigator key in (IEnumerable) this.inProcessIncludes.Keys)
      {
        if (key.IsSamePosition(c.Input))
        {
          nav = (XPathNavigator) this.inProcessIncludes[(object) key];
          break;
        }
      }
      if (nav == null)
        throw new Exception("Should not happen. Current input is " + c.Input.BaseURI + " / " + c.Input.Name + ", " + (object) this.inProcessIncludes.Count);
      if (nav.NodeType == XPathNodeType.Root)
        return;
      c.PushInputDocument(nav);
      do
        ;
      while (c.Input.NodeType != XPathNodeType.Element && c.Input.MoveToNext());
      if (c.Input.NamespaceURI != "http://www.w3.org/1999/XSL/Transform" && c.Input.NodeType == XPathNodeType.Element)
      {
        this.templates.Add(new XslTemplate(c));
      }
      else
      {
        do
        {
          if (c.Input.NodeType == XPathNodeType.Element)
            this.HandleTopLevelElement(c);
        }
        while (c.Input.MoveToNext());
      }
      c.Input.MoveToParent();
      c.PopInputDocument();
    }

    private void HandleImport(Compiler c, string href)
    {
      c.PushInputDocument(href);
      XslStylesheet xslStylesheet = new XslStylesheet();
      xslStylesheet.Compile(c);
      this.imports.Add((object) xslStylesheet);
      c.PopInputDocument();
    }

    private void HandleTopLevelElement(Compiler c)
    {
      XPathNavigator input = c.Input;
      string namespaceUri = input.NamespaceURI;
      if (namespaceUri == null)
        return;
      // ISSUE: reference to a compiler-generated field
      if (XslStylesheet.\u003C\u003Ef__switch\u0024map26 == null)
      {
        // ISSUE: reference to a compiler-generated field
        XslStylesheet.\u003C\u003Ef__switch\u0024map26 = new Dictionary<string, int>(2)
        {
          {
            "http://www.w3.org/1999/XSL/Transform",
            0
          },
          {
            "urn:schemas-microsoft-com:xslt",
            1
          }
        };
      }
      int num1;
      // ISSUE: reference to a compiler-generated field
      if (!XslStylesheet.\u003C\u003Ef__switch\u0024map26.TryGetValue(namespaceUri, out num1))
        return;
      switch (num1)
      {
        case 0:
          string localName1 = input.LocalName;
          if (localName1 != null)
          {
            // ISSUE: reference to a compiler-generated field
            if (XslStylesheet.\u003C\u003Ef__switch\u0024map24 == null)
            {
              // ISSUE: reference to a compiler-generated field
              XslStylesheet.\u003C\u003Ef__switch\u0024map24 = new Dictionary<string, int>(11)
              {
                {
                  "include",
                  0
                },
                {
                  "preserve-space",
                  1
                },
                {
                  "strip-space",
                  2
                },
                {
                  "namespace-alias",
                  3
                },
                {
                  "attribute-set",
                  4
                },
                {
                  "key",
                  5
                },
                {
                  "output",
                  6
                },
                {
                  "decimal-format",
                  7
                },
                {
                  "template",
                  8
                },
                {
                  "variable",
                  9
                },
                {
                  "param",
                  10
                }
              };
            }
            int num2;
            // ISSUE: reference to a compiler-generated field
            if (XslStylesheet.\u003C\u003Ef__switch\u0024map24.TryGetValue(localName1, out num2))
            {
              switch (num2)
              {
                case 0:
                  this.HandleInclude(c);
                  return;
                case 1:
                  this.AddSpaceControls(c.ParseQNameListAttribute("elements"), XmlSpace.Preserve, input);
                  return;
                case 2:
                  this.AddSpaceControls(c.ParseQNameListAttribute("elements"), XmlSpace.Default, input);
                  return;
                case 3:
                  return;
                case 4:
                  c.AddAttributeSet(new XslAttributeSet(c));
                  return;
                case 5:
                  XslKey xslKey = new XslKey(c);
                  if (this.keys[(object) xslKey.Name] == null)
                    this.keys[(object) xslKey.Name] = (object) new ArrayList();
                  ((ArrayList) this.keys[(object) xslKey.Name]).Add((object) xslKey);
                  return;
                case 6:
                  c.CompileOutput();
                  return;
                case 7:
                  c.CompileDecimalFormat();
                  return;
                case 8:
                  this.templates.Add(new XslTemplate(c));
                  return;
                case 9:
                  XslGlobalVariable xslGlobalVariable = new XslGlobalVariable(c);
                  this.variables[(object) xslGlobalVariable.Name] = (object) xslGlobalVariable;
                  return;
                case 10:
                  XslGlobalParam xslGlobalParam = new XslGlobalParam(c);
                  this.variables[(object) xslGlobalParam.Name] = (object) xslGlobalParam;
                  return;
              }
            }
          }
          if (!(this.version == "1.0"))
            break;
          throw new XsltCompileException("Unrecognized top level element after imports", (Exception) null, c.Input);
        case 1:
          string localName2 = input.LocalName;
          if (localName2 == null)
            break;
          // ISSUE: reference to a compiler-generated field
          if (XslStylesheet.\u003C\u003Ef__switch\u0024map25 == null)
          {
            // ISSUE: reference to a compiler-generated field
            XslStylesheet.\u003C\u003Ef__switch\u0024map25 = new Dictionary<string, int>(1)
            {
              {
                "script",
                0
              }
            };
          }
          int num3;
          // ISSUE: reference to a compiler-generated field
          if (!XslStylesheet.\u003C\u003Ef__switch\u0024map25.TryGetValue(localName2, out num3) || num3 != 0)
            break;
          c.ScriptManager.AddScript(c);
          break;
      }
    }

    private XPathNavigator HandleIncludesImports(Compiler c)
    {
      do
      {
        if (c.Input.NodeType == XPathNodeType.Element)
        {
          if (!(c.Input.LocalName != "import") && !(c.Input.NamespaceURI != "http://www.w3.org/1999/XSL/Transform"))
            this.HandleImport(c, c.GetAttribute("href"));
          else
            break;
        }
      }
      while (c.Input.MoveToNext());
      XPathNavigator other = c.Input.Clone();
      do
      {
        if (c.Input.NodeType == XPathNodeType.Element && !(c.Input.LocalName != "include") && !(c.Input.NamespaceURI != "http://www.w3.org/1999/XSL/Transform"))
          this.StoreInclude(c);
      }
      while (c.Input.MoveToNext());
      c.Input.MoveTo(other);
      return other;
    }

    private void ProcessTopLevelElements(Compiler c)
    {
      if (!c.Input.MoveToFirstChild())
        return;
      XPathNavigator other = this.HandleIncludesImports(c);
      do
      {
        if (c.Input.NodeType == XPathNodeType.Element && !(c.Input.LocalName != "namespace-alias") && !(c.Input.NamespaceURI != "http://www.w3.org/1999/XSL/Transform"))
        {
          string name = c.GetAttribute("stylesheet-prefix", string.Empty);
          if (name == "#default")
            name = string.Empty;
          string str = c.GetAttribute("result-prefix", string.Empty);
          if (str == "#default")
            str = string.Empty;
          this.namespaceAliases.Set(name, str);
        }
      }
      while (c.Input.MoveToNext());
      c.Input.MoveTo(other);
      do
      {
        if (c.Input.NodeType == XPathNodeType.Element)
          this.HandleTopLevelElement(c);
      }
      while (c.Input.MoveToNext());
      c.Input.MoveToParent();
    }

    private void AddSpaceControls(
      XmlQualifiedName[] names,
      XmlSpace result,
      XPathNavigator styleElem)
    {
      foreach (object name in names)
        this.spaceControls[name] = (object) result;
    }
  }
}
