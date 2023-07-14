// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.XsltCompiledContext
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using Mono.Xml.XPath;
using System;
using System.Collections;
using System.Reflection;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.Xsl
{
  internal class XsltCompiledContext : XsltContext
  {
    private Hashtable keyNameCache = new Hashtable();
    private Hashtable keyIndexTables = new Hashtable();
    private Hashtable patternNavCaches = new Hashtable();
    private XslTransformProcessor p;
    private XsltCompiledContext.XsltContextInfo[] scopes;
    private int scopeAt;

    public XsltCompiledContext(XslTransformProcessor p)
      : base(new System.Xml.NameTable())
    {
      this.p = p;
      this.scopes = new XsltCompiledContext.XsltContextInfo[10];
      for (int index = 0; index < 10; ++index)
        this.scopes[index] = new XsltCompiledContext.XsltContextInfo();
    }

    public XslTransformProcessor Processor => this.p;

    public override string DefaultNamespace => string.Empty;

    public XPathNavigator GetNavCache(Pattern p, XPathNavigator node)
    {
      if (!(this.patternNavCaches[(object) p] is XPathNavigator navCache) || !navCache.MoveTo(node))
      {
        navCache = node.Clone();
        this.patternNavCaches[(object) p] = (object) navCache;
      }
      return navCache;
    }

    public object EvaluateKey(
      IStaticXsltContext staticContext,
      BaseIterator iter,
      Expression nameExpr,
      Expression valueExpr)
    {
      return (object) this.GetIndexTable(this.GetKeyName(staticContext, iter, nameExpr)).Evaluate(iter, valueExpr);
    }

    public bool MatchesKey(
      XPathNavigator nav,
      IStaticXsltContext staticContext,
      string name,
      string value)
    {
      return this.GetIndexTable(XslNameUtil.FromString(name, staticContext)).Matches(nav, value, (XsltContext) this);
    }

    private XmlQualifiedName GetKeyName(
      IStaticXsltContext staticContext,
      BaseIterator iter,
      Expression nameExpr)
    {
      XmlQualifiedName keyName;
      if (nameExpr.HasStaticValue)
      {
        keyName = (XmlQualifiedName) this.keyNameCache[(object) nameExpr];
        if (keyName == (XmlQualifiedName) null)
        {
          keyName = XslNameUtil.FromString(nameExpr.EvaluateString(iter), staticContext);
          this.keyNameCache[(object) nameExpr] = (object) keyName;
        }
      }
      else
        keyName = XslNameUtil.FromString(nameExpr.EvaluateString(iter), (XmlNamespaceManager) this);
      return keyName;
    }

    private KeyIndexTable GetIndexTable(XmlQualifiedName name)
    {
      if (!(this.keyIndexTables[(object) name] is KeyIndexTable indexTable))
      {
        indexTable = new KeyIndexTable(this, this.p.CompiledStyle.ResolveKey(name));
        this.keyIndexTables[(object) name] = (object) indexTable;
      }
      return indexTable;
    }

    public override string LookupNamespace(string prefix) => throw new InvalidOperationException("we should never get here");

    internal override IXsltContextFunction ResolveFunction(
      XmlQualifiedName name,
      XPathResultType[] argTypes)
    {
      string str = name.Namespace;
      if (str == null)
        return (IXsltContextFunction) null;
      object extension = (object) null;
      if (this.p.Arguments != null)
        extension = this.p.Arguments.GetExtensionObject(str);
      bool isScript = false;
      if (extension == null)
      {
        extension = this.p.ScriptManager.GetExtensionObject(str);
        if (extension == null)
          return (IXsltContextFunction) null;
        isScript = true;
      }
      MethodInfo bestMethod = this.FindBestMethod(extension.GetType(), name.Name, argTypes, isScript);
      return bestMethod != null ? (IXsltContextFunction) new XsltExtensionFunction(extension, bestMethod, this.p.CurrentNode) : (IXsltContextFunction) null;
    }

    private MethodInfo FindBestMethod(
      Type t,
      string name,
      XPathResultType[] argTypes,
      bool isScript)
    {
      MethodInfo[] methods = t.GetMethods((BindingFlags) ((!isScript ? 16 : 48) | 4 | 8));
      if (methods.Length == 0)
        return (MethodInfo) null;
      if (argTypes == null)
        return methods[0];
      int num1 = 0;
      int length = argTypes.Length;
      for (int index = 0; index < methods.Length; ++index)
      {
        if (methods[index].Name == name && methods[index].GetParameters().Length == length)
          methods[num1++] = methods[index];
      }
      int num2 = num1;
      switch (num2)
      {
        case 0:
          return (MethodInfo) null;
        case 1:
          return methods[0];
        default:
          for (int index1 = 0; index1 < num2; ++index1)
          {
            bool flag = true;
            ParameterInfo[] parameters = methods[index1].GetParameters();
            for (int index2 = 0; index2 < parameters.Length; ++index2)
            {
              XPathResultType argType = argTypes[index2];
              if (argType != XPathResultType.Any)
              {
                XPathResultType xpathType = XPFuncImpl.GetXPathType(parameters[index2].ParameterType, this.p.CurrentNode);
                if (xpathType != argType && xpathType != XPathResultType.Any)
                {
                  flag = false;
                  break;
                }
                if (xpathType == XPathResultType.Any && argType != XPathResultType.NodeSet && parameters[index2].ParameterType != typeof (object))
                {
                  flag = false;
                  break;
                }
              }
            }
            if (flag)
              return methods[index1];
          }
          return (MethodInfo) null;
      }
    }

    public override IXsltContextVariable ResolveVariable(string prefix, string name) => throw new InvalidOperationException("shouldn't get here");

    public override IXsltContextFunction ResolveFunction(
      string prefix,
      string name,
      XPathResultType[] ArgTypes)
    {
      throw new InvalidOperationException("XsltCompiledContext exception: shouldn't get here.");
    }

    internal override IXsltContextVariable ResolveVariable(XmlQualifiedName q) => (IXsltContextVariable) this.p.CompiledStyle.ResolveVariable(q);

    public override int CompareDocument(string baseUri, string nextBaseUri) => baseUri.GetHashCode().CompareTo(nextBaseUri.GetHashCode());

    public override bool PreserveWhitespace(XPathNavigator nav) => this.p.CompiledStyle.Style.GetPreserveWhitespace(nav);

    public override bool Whitespace => this.WhitespaceHandling;

    public bool IsCData
    {
      get => this.scopes[this.scopeAt].IsCData;
      set => this.scopes[this.scopeAt].IsCData = value;
    }

    public bool WhitespaceHandling
    {
      get => this.scopes[this.scopeAt].PreserveWhitespace;
      set => this.scopes[this.scopeAt].PreserveWhitespace = value;
    }

    public string ElementPrefix
    {
      get => this.scopes[this.scopeAt].ElementPrefix;
      set => this.scopes[this.scopeAt].ElementPrefix = value;
    }

    public string ElementNamespace
    {
      get => this.scopes[this.scopeAt].ElementNamespace;
      set => this.scopes[this.scopeAt].ElementNamespace = value;
    }

    private void ExtendScope()
    {
      XsltCompiledContext.XsltContextInfo[] scopes = this.scopes;
      this.scopes = new XsltCompiledContext.XsltContextInfo[this.scopeAt * 2 + 1];
      if (this.scopeAt <= 0)
        return;
      Array.Copy((Array) scopes, 0, (Array) this.scopes, 0, this.scopeAt);
    }

    public override bool PopScope()
    {
      base.PopScope();
      if (this.scopeAt == -1)
        return false;
      --this.scopeAt;
      return true;
    }

    public override void PushScope()
    {
      base.PushScope();
      ++this.scopeAt;
      if (this.scopeAt == this.scopes.Length)
        this.ExtendScope();
      if (this.scopes[this.scopeAt] == null)
        this.scopes[this.scopeAt] = new XsltCompiledContext.XsltContextInfo();
      else
        this.scopes[this.scopeAt].Clear();
    }

    private class XsltContextInfo
    {
      public bool IsCData;
      public bool PreserveWhitespace = true;
      public string ElementPrefix;
      public string ElementNamespace;

      public void Clear()
      {
        this.IsCData = false;
        this.PreserveWhitespace = true;
        this.ElementPrefix = this.ElementNamespace = (string) null;
      }
    }
  }
}
