// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.Compiler
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using Mono.Xml.XPath;
using Mono.Xml.Xsl.Operations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Policy;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.Xsl
{
  internal class Compiler : IStaticXsltContext
  {
    public const string XsltNamespace = "http://www.w3.org/1999/XSL/Transform";
    private ArrayList inputStack = new ArrayList();
    private XPathNavigator currentInput;
    private Stack styleStack = new Stack();
    private XslStylesheet currentStyle;
    private Hashtable keys = new Hashtable();
    private Hashtable globalVariables = new Hashtable();
    private Hashtable attrSets = new Hashtable();
    private XmlNamespaceManager nsMgr = new XmlNamespaceManager((XmlNameTable) new NameTable());
    private XmlResolver res;
    private Evidence evidence;
    private XslStylesheet rootStyle;
    private Hashtable outputs = new Hashtable();
    private bool keyCompilationMode;
    private string stylesheetVersion;
    private XsltDebuggerWrapper debugger;
    private MSXslScriptManager msScripts = new MSXslScriptManager();
    internal XPathParser xpathParser;
    internal XsltPatternParser patternParser;
    private VariableScope curVarScope;
    private Hashtable decimalFormats = new Hashtable();

    public Compiler(object debugger)
    {
      if (debugger == null)
        return;
      this.debugger = new XsltDebuggerWrapper(debugger);
    }

    Expression IStaticXsltContext.TryGetVariable(string nm)
    {
      if (this.curVarScope == null)
        return (Expression) null;
      XslLocalVariable v = this.curVarScope.ResolveStatic(XslNameUtil.FromString(nm, this.Input));
      return v == null ? (Expression) null : (Expression) new XPathVariableBinding((XslGeneralVariable) v);
    }

    Expression IStaticXsltContext.TryGetFunction(XmlQualifiedName name, FunctionArguments args)
    {
      string str = this.LookupNamespace(name.Namespace);
      if (str == "urn:schemas-microsoft-com:xslt" && name.Name == "node-set")
        return (Expression) new MSXslNodeSet(args);
      if (str != string.Empty)
        return (Expression) null;
      string name1 = name.Name;
      if (name1 != null)
      {
        // ISSUE: reference to a compiler-generated field
        if (Compiler.\u003C\u003Ef__switch\u0024mapE == null)
        {
          // ISSUE: reference to a compiler-generated field
          Compiler.\u003C\u003Ef__switch\u0024mapE = new Dictionary<string, int>(9)
          {
            {
              "current",
              0
            },
            {
              "unparsed-entity-uri",
              1
            },
            {
              "element-available",
              2
            },
            {
              "system-property",
              3
            },
            {
              "function-available",
              4
            },
            {
              "generate-id",
              5
            },
            {
              "format-number",
              6
            },
            {
              "key",
              7
            },
            {
              "document",
              8
            }
          };
        }
        int num;
        // ISSUE: reference to a compiler-generated field
        if (Compiler.\u003C\u003Ef__switch\u0024mapE.TryGetValue(name1, out num))
        {
          switch (num)
          {
            case 0:
              return (Expression) new XsltCurrent(args);
            case 1:
              return (Expression) new XsltUnparsedEntityUri(args);
            case 2:
              return (Expression) new XsltElementAvailable(args, (IStaticXsltContext) this);
            case 3:
              return (Expression) new XsltSystemProperty(args, (IStaticXsltContext) this);
            case 4:
              return (Expression) new XsltFunctionAvailable(args, (IStaticXsltContext) this);
            case 5:
              return (Expression) new XsltGenerateId(args);
            case 6:
              return (Expression) new XsltFormatNumber(args, (IStaticXsltContext) this);
            case 7:
              if (this.KeyCompilationMode)
                throw new XsltCompileException("Cannot use key() function inside key definition", (Exception) null, this.Input);
              return (Expression) new XsltKey(args, (IStaticXsltContext) this);
            case 8:
              return (Expression) new XsltDocument(args, this);
          }
        }
      }
      return (Expression) null;
    }

    XmlQualifiedName IStaticXsltContext.LookupQName(string s) => XslNameUtil.FromString(s, this.Input);

    public XsltDebuggerWrapper Debugger => this.debugger;

    public void CheckExtraAttributes(string element, params string[] validNames)
    {
      if (!this.Input.MoveToFirstAttribute())
        return;
      do
      {
        if (this.Input.NamespaceURI.Length <= 0)
        {
          bool flag = false;
          foreach (string validName in validNames)
          {
            if (this.Input.LocalName == validName)
              flag = true;
          }
          if (!flag)
            throw new XsltCompileException(string.Format("Invalid attribute '{0}' on element '{1}'", (object) this.Input.LocalName, (object) element), (Exception) null, this.Input);
        }
      }
      while (this.Input.MoveToNextAttribute());
      this.Input.MoveToParent();
    }

    public CompiledStylesheet Compile(XPathNavigator nav, XmlResolver res, Evidence evidence)
    {
      this.xpathParser = new XPathParser((IStaticXsltContext) this);
      this.patternParser = new XsltPatternParser((IStaticXsltContext) this);
      this.res = res;
      if (res == null)
        this.res = (XmlResolver) new XmlUrlResolver();
      this.evidence = evidence;
      if (nav.NodeType == XPathNodeType.Root && !nav.MoveToFirstChild())
        throw new XsltCompileException("Stylesheet root element must be either \"stylesheet\" or \"transform\" or any literal element", (Exception) null, nav);
      while (nav.NodeType != XPathNodeType.Element)
        nav.MoveToNext();
      this.stylesheetVersion = nav.GetAttribute("version", !(nav.NamespaceURI != "http://www.w3.org/1999/XSL/Transform") ? string.Empty : "http://www.w3.org/1999/XSL/Transform");
      this.outputs[(object) string.Empty] = (object) new XslOutput(string.Empty, this.stylesheetVersion);
      this.PushInputDocument(nav);
      if (nav.MoveToFirstNamespace(XPathNamespaceScope.ExcludeXml))
      {
        do
        {
          this.nsMgr.AddNamespace(nav.LocalName, nav.Value);
        }
        while (nav.MoveToNextNamespace(XPathNamespaceScope.ExcludeXml));
        nav.MoveToParent();
      }
      try
      {
        this.rootStyle = new XslStylesheet();
        this.rootStyle.Compile(this);
      }
      catch (XsltCompileException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        throw new XsltCompileException("XSLT compile error. " + ex.Message, ex, this.Input);
      }
      return new CompiledStylesheet(this.rootStyle, this.globalVariables, this.attrSets, this.nsMgr, this.keys, this.outputs, this.decimalFormats, this.msScripts);
    }

    public MSXslScriptManager ScriptManager => this.msScripts;

    public bool KeyCompilationMode
    {
      get => this.keyCompilationMode;
      set => this.keyCompilationMode = value;
    }

    internal Evidence Evidence => this.evidence;

    public XPathNavigator Input => this.currentInput;

    public XslStylesheet CurrentStylesheet => this.currentStyle;

    public void PushStylesheet(XslStylesheet style)
    {
      if (this.currentStyle != null)
        this.styleStack.Push((object) this.currentStyle);
      this.currentStyle = style;
    }

    public void PopStylesheet()
    {
      if (this.styleStack.Count == 0)
        this.currentStyle = (XslStylesheet) null;
      else
        this.currentStyle = (XslStylesheet) this.styleStack.Pop();
    }

    public void PushInputDocument(string url)
    {
      Uri absoluteUri = this.res.ResolveUri(!(this.Input.BaseURI == string.Empty) ? new Uri(this.Input.BaseURI) : (Uri) null, url);
      string url1 = !(absoluteUri != (Uri) null) ? string.Empty : absoluteUri.ToString();
      using (Stream entity = (Stream) this.res.GetEntity(absoluteUri, (string) null, typeof (Stream)))
      {
        if (entity == null)
          throw new XsltCompileException("Can not access URI " + absoluteUri.ToString(), (Exception) null, this.Input);
        XmlValidatingReader reader = new XmlValidatingReader((XmlReader) new XmlTextReader(url1, entity, this.nsMgr.NameTable));
        reader.ValidationType = ValidationType.None;
        XPathNavigator navigator = new XPathDocument((XmlReader) reader, XmlSpace.Preserve).CreateNavigator();
        reader.Close();
        navigator.MoveToFirstChild();
        do
          ;
        while (navigator.NodeType != XPathNodeType.Element && navigator.MoveToNext());
        this.PushInputDocument(navigator);
      }
    }

    public void PushInputDocument(XPathNavigator nav)
    {
      bool flag = this.currentInput is IXmlLineInfo currentInput && !currentInput.HasLineInfo();
      for (int index = 0; index < this.inputStack.Count; ++index)
      {
        if (((XPathNavigator) this.inputStack[index]).BaseURI == nav.BaseURI)
          throw new XsltCompileException((Exception) null, this.currentInput.BaseURI, !flag ? 0 : currentInput.LineNumber, !flag ? 0 : currentInput.LinePosition);
      }
      if (this.currentInput != null)
        this.inputStack.Add((object) this.currentInput);
      this.currentInput = nav;
    }

    public void PopInputDocument()
    {
      int index = this.inputStack.Count - 1;
      this.currentInput = (XPathNavigator) this.inputStack[index];
      this.inputStack.RemoveAt(index);
    }

    public XmlQualifiedName ParseQNameAttribute(string localName) => this.ParseQNameAttribute(localName, string.Empty);

    public XmlQualifiedName ParseQNameAttribute(string localName, string ns) => XslNameUtil.FromString(this.Input.GetAttribute(localName, ns), this.Input);

    public XmlQualifiedName[] ParseQNameListAttribute(string localName) => this.ParseQNameListAttribute(localName, string.Empty);

    public XmlQualifiedName[] ParseQNameListAttribute(string localName, string ns)
    {
      string attribute = this.GetAttribute(localName, ns);
      if (attribute == null)
        return (XmlQualifiedName[]) null;
      string[] strArray = attribute.Split(' ', '\r', '\n', '\t');
      int length = 0;
      for (int index = 0; index < strArray.Length; ++index)
      {
        if (strArray[index].Length != 0)
          ++length;
      }
      XmlQualifiedName[] qnameListAttribute = new XmlQualifiedName[length];
      int index1 = 0;
      int num = 0;
      for (; index1 < strArray.Length; ++index1)
      {
        if (strArray[index1].Length != 0)
          qnameListAttribute[num++] = XslNameUtil.FromString(strArray[index1], this.Input);
      }
      return qnameListAttribute;
    }

    public bool ParseYesNoAttribute(string localName, bool defaultVal) => this.ParseYesNoAttribute(localName, string.Empty, defaultVal);

    public bool ParseYesNoAttribute(string localName, string ns, bool defaultVal)
    {
      string attribute = this.GetAttribute(localName, ns);
      if (attribute != null)
      {
        // ISSUE: reference to a compiler-generated field
        if (Compiler.\u003C\u003Ef__switch\u0024mapF == null)
        {
          // ISSUE: reference to a compiler-generated field
          Compiler.\u003C\u003Ef__switch\u0024mapF = new Dictionary<string, int>(2)
          {
            {
              "yes",
              0
            },
            {
              "no",
              1
            }
          };
        }
        int num;
        // ISSUE: reference to a compiler-generated field
        if (Compiler.\u003C\u003Ef__switch\u0024mapF.TryGetValue(attribute, out num))
        {
          if (num == 0)
            return true;
          if (num == 1)
            return false;
        }
        throw new XsltCompileException("Invalid value for " + localName, (Exception) null, this.Input);
      }
      return defaultVal;
    }

    public string GetAttribute(string localName) => this.GetAttribute(localName, string.Empty);

    public string GetAttribute(string localName, string ns)
    {
      if (!this.Input.MoveToAttribute(localName, ns))
        return (string) null;
      string attribute = this.Input.Value;
      this.Input.MoveToParent();
      return attribute;
    }

    public XslAvt ParseAvtAttribute(string localName) => this.ParseAvtAttribute(localName, string.Empty);

    public XslAvt ParseAvtAttribute(string localName, string ns) => this.ParseAvt(this.GetAttribute(localName, ns));

    public void AssertAttribute(string localName) => this.AssertAttribute(localName, string.Empty);

    public void AssertAttribute(string localName, string ns)
    {
      if (this.Input.GetAttribute(localName, ns) == null)
        throw new XsltCompileException("Was expecting the " + localName + " attribute", (Exception) null, this.Input);
    }

    public XslAvt ParseAvt(string s) => s == null ? (XslAvt) null : new XslAvt(s, this);

    public Pattern CompilePattern(string pattern, XPathNavigator loc)
    {
      if (pattern == null || pattern == string.Empty)
        return (Pattern) null;
      return Pattern.Compile(pattern, this) ?? throw new XsltCompileException(string.Format("Invalid pattern '{0}'", (object) pattern), (Exception) null, loc);
    }

    internal CompiledExpression CompileExpression(string expression) => this.CompileExpression(expression, false);

    internal CompiledExpression CompileExpression(string expression, bool isKey)
    {
      if (expression == null || expression == string.Empty)
        return (CompiledExpression) null;
      Expression expr = this.xpathParser.Compile(expression);
      if (isKey)
        expr = (Expression) new ExprKeyContainer(expr);
      return new CompiledExpression(expression, expr);
    }

    public XslOperation CompileTemplateContent() => this.CompileTemplateContent(XPathNodeType.All, false);

    public XslOperation CompileTemplateContent(XPathNodeType parentType) => this.CompileTemplateContent(parentType, false);

    public XslOperation CompileTemplateContent(XPathNodeType parentType, bool xslForEach) => (XslOperation) new XslTemplateContent(this, parentType, xslForEach);

    public void AddGlobalVariable(XslGlobalVariable var) => this.globalVariables[(object) var.Name] = (object) var;

    public void AddKey(XslKey key)
    {
      if (this.keys[(object) key.Name] == null)
        this.keys[(object) key.Name] = (object) new ArrayList();
      ((ArrayList) this.keys[(object) key.Name]).Add((object) key);
    }

    public void AddAttributeSet(XslAttributeSet set)
    {
      if (this.attrSets[(object) set.Name] is XslAttributeSet attrSet)
      {
        attrSet.Merge(set);
        this.attrSets[(object) set.Name] = (object) attrSet;
      }
      else
        this.attrSets[(object) set.Name] = (object) set;
    }

    public void PushScope() => this.curVarScope = new VariableScope(this.curVarScope);

    public VariableScope PopScope()
    {
      this.curVarScope.giveHighTideToParent();
      VariableScope curVarScope = this.curVarScope;
      this.curVarScope = this.curVarScope.Parent;
      return curVarScope;
    }

    public int AddVariable(XslLocalVariable v) => this.curVarScope != null ? this.curVarScope.AddVariable(v) : throw new XsltCompileException("Not initialized variable", (Exception) null, this.Input);

    public VariableScope CurrentVariableScope => this.curVarScope;

    public bool IsExtensionNamespace(string nsUri)
    {
      if (nsUri == "http://www.w3.org/1999/XSL/Transform")
        return true;
      XPathNavigator other = this.Input.Clone();
      XPathNavigator xpathNavigator = other.Clone();
      do
      {
        bool flag = other.NamespaceURI == "http://www.w3.org/1999/XSL/Transform";
        xpathNavigator.MoveTo(other);
        if (other.MoveToFirstAttribute())
        {
          do
          {
            if (other.LocalName == "extension-element-prefixes" && other.NamespaceURI == (!flag ? "http://www.w3.org/1999/XSL/Transform" : string.Empty))
            {
              string str1 = other.Value;
              char[] chArray = new char[1]{ ' ' };
              foreach (string str2 in str1.Split(chArray))
              {
                if (xpathNavigator.GetNamespace(!(str2 == "#default") ? str2 : string.Empty) == nsUri)
                  return true;
              }
            }
          }
          while (other.MoveToNextAttribute());
          other.MoveToParent();
        }
      }
      while (other.MoveToParent());
      return false;
    }

    public Hashtable GetNamespacesToCopy()
    {
      Hashtable namespacesToCopy = new Hashtable();
      XPathNavigator other = this.Input.Clone();
      XPathNavigator xpathNavigator = other.Clone();
      if (other.MoveToFirstNamespace(XPathNamespaceScope.ExcludeXml))
      {
        do
        {
          if (other.Value != "http://www.w3.org/1999/XSL/Transform" && !namespacesToCopy.Contains((object) other.Name))
            namespacesToCopy.Add((object) other.Name, (object) other.Value);
        }
        while (other.MoveToNextNamespace(XPathNamespaceScope.ExcludeXml));
        other.MoveToParent();
      }
      do
      {
        bool flag = other.NamespaceURI == "http://www.w3.org/1999/XSL/Transform";
        xpathNavigator.MoveTo(other);
        if (other.MoveToFirstAttribute())
        {
          do
          {
            if ((other.LocalName == "extension-element-prefixes" || other.LocalName == "exclude-result-prefixes") && other.NamespaceURI == (!flag ? "http://www.w3.org/1999/XSL/Transform" : string.Empty))
            {
              string str1 = other.Value;
              char[] chArray = new char[1]{ ' ' };
              foreach (string str2 in str1.Split(chArray))
              {
                string str3 = !(str2 == "#default") ? str2 : string.Empty;
                if ((string) namespacesToCopy[(object) str3] == xpathNavigator.GetNamespace(str3))
                  namespacesToCopy.Remove((object) str3);
              }
            }
          }
          while (other.MoveToNextAttribute());
          other.MoveToParent();
        }
      }
      while (other.MoveToParent());
      return namespacesToCopy;
    }

    public void CompileDecimalFormat()
    {
      XmlQualifiedName qnameAttribute = this.ParseQNameAttribute("name");
      try
      {
        if (qnameAttribute.Name != string.Empty)
          XmlConvert.VerifyNCName(qnameAttribute.Name);
      }
      catch (XmlException ex)
      {
        throw new XsltCompileException("Invalid qualified name", (Exception) ex, this.Input);
      }
      XslDecimalFormat other = new XslDecimalFormat(this);
      if (this.decimalFormats.Contains((object) qnameAttribute))
        ((XslDecimalFormat) this.decimalFormats[(object) qnameAttribute]).CheckSameAs(other);
      else
        this.decimalFormats[(object) qnameAttribute] = (object) other;
    }

    public string LookupNamespace(string prefix)
    {
      if (prefix == string.Empty || prefix == null)
        return string.Empty;
      XPathNavigator xpathNavigator = this.Input;
      if (this.Input.NodeType == XPathNodeType.Attribute)
      {
        xpathNavigator = this.Input.Clone();
        xpathNavigator.MoveToParent();
      }
      return xpathNavigator.GetNamespace(prefix);
    }

    public void CompileOutput()
    {
      XPathNavigator input = this.Input;
      string attribute = input.GetAttribute("href", string.Empty);
      if (!(this.outputs[(object) attribute] is XslOutput xslOutput))
      {
        xslOutput = new XslOutput(attribute, this.stylesheetVersion);
        this.outputs.Add((object) attribute, (object) xslOutput);
      }
      xslOutput.Fill(input);
    }
  }
}
