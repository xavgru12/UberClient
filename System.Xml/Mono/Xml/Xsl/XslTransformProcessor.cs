// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.XslTransformProcessor
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using Mono.Xml.XPath;
using Mono.Xml.Xsl.Operations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.Xsl
{
  internal class XslTransformProcessor
  {
    private XsltDebuggerWrapper debugger;
    private CompiledStylesheet compiledStyle;
    private XslStylesheet style;
    private Stack currentTemplateStack = new Stack();
    private XPathNavigator root;
    private XsltArgumentList args;
    private XmlResolver resolver;
    private string currentOutputUri;
    internal readonly XsltCompiledContext XPathContext;
    internal Hashtable globalVariableTable = new Hashtable();
    private Hashtable docCache;
    private Stack outputStack = new Stack();
    private StringBuilder avtSB;
    private Stack paramPassingCache = new Stack();
    private ArrayList nodesetStack = new ArrayList();
    private Stack variableStack = new Stack();
    private object[] currentStack;
    private Hashtable busyTable = new Hashtable();
    private static object busyObject = new object();

    public XslTransformProcessor(CompiledStylesheet style, object debugger)
    {
      this.XPathContext = new XsltCompiledContext(this);
      this.compiledStyle = style;
      this.style = style.Style;
      if (debugger == null)
        return;
      this.debugger = new XsltDebuggerWrapper(debugger);
    }

    public void Process(
      XPathNavigator root,
      Outputter outputtter,
      XsltArgumentList args,
      XmlResolver resolver)
    {
      this.args = args;
      this.root = root;
      this.resolver = resolver == null ? (XmlResolver) new XmlUrlResolver() : resolver;
      this.currentOutputUri = string.Empty;
      this.PushNodeset((XPathNodeIterator) new SelfIterator(root, (IXmlNamespaceResolver) this.XPathContext));
      this.CurrentNodeset.MoveNext();
      if (args != null)
      {
        foreach (XslGlobalVariable xslGlobalVariable in (IEnumerable) this.CompiledStyle.Variables.Values)
        {
          if (xslGlobalVariable is XslGlobalParam)
          {
            object paramVal = args.GetParam(xslGlobalVariable.Name.Name, xslGlobalVariable.Name.Namespace);
            if (paramVal != null)
              ((XslGlobalParam) xslGlobalVariable).Override(this, paramVal);
            xslGlobalVariable.Evaluate(this);
          }
        }
      }
      foreach (XslGlobalVariable xslGlobalVariable in (IEnumerable) this.CompiledStyle.Variables.Values)
      {
        if (args == null || !(xslGlobalVariable is XslGlobalParam))
          xslGlobalVariable.Evaluate(this);
      }
      this.PopNodeset();
      this.PushOutput(outputtter);
      this.ApplyTemplates((XPathNodeIterator) new SelfIterator(root, (IXmlNamespaceResolver) this.XPathContext), XmlQualifiedName.Empty, (ArrayList) null);
      this.PopOutput();
    }

    public XsltDebuggerWrapper Debugger => this.debugger;

    public CompiledStylesheet CompiledStyle => this.compiledStyle;

    public XsltArgumentList Arguments => this.args;

    public XPathNavigator Root => this.root;

    public MSXslScriptManager ScriptManager => this.compiledStyle.ScriptManager;

    public XmlResolver Resolver => this.resolver;

    public XPathNavigator GetDocument(Uri uri)
    {
      if (this.docCache != null)
      {
        if (this.docCache[(object) uri] is XPathNavigator navigator)
          return navigator.Clone();
      }
      else
        this.docCache = new Hashtable();
      XmlReader reader = (XmlReader) null;
      try
      {
        reader = (XmlReader) new XmlTextReader(uri.ToString(), (Stream) this.resolver.GetEntity(uri, (string) null, (Type) null), this.root.NameTable);
        navigator = new XPathDocument((XmlReader) new XmlValidatingReader(reader)
        {
          ValidationType = ValidationType.None
        }, XmlSpace.Preserve).CreateNavigator();
      }
      finally
      {
        reader?.Close();
      }
      this.docCache[(object) uri] = (object) navigator.Clone();
      return navigator;
    }

    public Outputter Out => (Outputter) this.outputStack.Peek();

    public void PushOutput(Outputter newOutput) => this.outputStack.Push((object) newOutput);

    public Outputter PopOutput()
    {
      Outputter outputter = (Outputter) this.outputStack.Pop();
      outputter.Done();
      return outputter;
    }

    public Hashtable Outputs => this.compiledStyle.Outputs;

    public XslOutput Output => this.Outputs[(object) this.currentOutputUri] as XslOutput;

    public string CurrentOutputUri => this.currentOutputUri;

    public bool InsideCDataElement => this.XPathContext.IsCData;

    public StringBuilder GetAvtStringBuilder()
    {
      if (this.avtSB == null)
        this.avtSB = new StringBuilder();
      return this.avtSB;
    }

    public string ReleaseAvtStringBuilder()
    {
      string str = this.avtSB.ToString();
      this.avtSB.Length = 0;
      return str;
    }

    private Hashtable GetParams(ArrayList withParams)
    {
      if (withParams == null)
        return (Hashtable) null;
      Hashtable hashtable;
      if (this.paramPassingCache.Count != 0)
      {
        hashtable = (Hashtable) this.paramPassingCache.Pop();
        hashtable.Clear();
      }
      else
        hashtable = new Hashtable();
      int count = withParams.Count;
      for (int index = 0; index < count; ++index)
      {
        XslVariableInformation withParam = (XslVariableInformation) withParams[index];
        hashtable.Add((object) withParam.Name, withParam.Evaluate(this));
      }
      return hashtable;
    }

    public void ApplyTemplates(
      XPathNodeIterator nodes,
      XmlQualifiedName mode,
      ArrayList withParams)
    {
      Hashtable withParams1 = this.GetParams(withParams);
      while (this.NodesetMoveNext(nodes))
      {
        this.PushNodeset(nodes);
        XslTemplate template = this.FindTemplate(this.CurrentNode, mode);
        this.currentTemplateStack.Push((object) template);
        template.Evaluate(this, withParams1);
        this.currentTemplateStack.Pop();
        this.PopNodeset();
      }
      if (withParams1 == null)
        return;
      this.paramPassingCache.Push((object) withParams1);
    }

    public void CallTemplate(XmlQualifiedName name, ArrayList withParams)
    {
      Hashtable withParams1 = this.GetParams(withParams);
      XslTemplate template = this.FindTemplate(name);
      this.currentTemplateStack.Push((object) null);
      template.Evaluate(this, withParams1);
      this.currentTemplateStack.Pop();
      if (withParams1 == null)
        return;
      this.paramPassingCache.Push((object) withParams1);
    }

    public void ApplyImports()
    {
      XslTemplate xslTemplate1 = (XslTemplate) this.currentTemplateStack.Peek();
      if (xslTemplate1 == null)
        throw new XsltException("Invalid context for apply-imports", (Exception) null, this.CurrentNode);
      for (int index = xslTemplate1.Parent.Imports.Count - 1; index >= 0; --index)
      {
        XslTemplate match = ((XslStylesheet) xslTemplate1.Parent.Imports[index]).Templates.FindMatch(this.CurrentNode, xslTemplate1.Mode, this);
        if (match != null)
        {
          this.currentTemplateStack.Push((object) match);
          match.Evaluate(this);
          this.currentTemplateStack.Pop();
          return;
        }
      }
      XslTemplate xslTemplate2;
      switch (this.CurrentNode.NodeType)
      {
        case XPathNodeType.Root:
        case XPathNodeType.Element:
          xslTemplate2 = !(xslTemplate1.Mode == XmlQualifiedName.Empty) ? (XslTemplate) new XslDefaultNodeTemplate(xslTemplate1.Mode) : XslDefaultNodeTemplate.Instance;
          break;
        case XPathNodeType.Attribute:
        case XPathNodeType.Text:
        case XPathNodeType.SignificantWhitespace:
        case XPathNodeType.Whitespace:
          xslTemplate2 = XslDefaultTextTemplate.Instance;
          break;
        case XPathNodeType.ProcessingInstruction:
        case XPathNodeType.Comment:
          xslTemplate2 = XslEmptyTemplate.Instance;
          break;
        default:
          xslTemplate2 = XslEmptyTemplate.Instance;
          break;
      }
      this.currentTemplateStack.Push((object) xslTemplate2);
      xslTemplate2.Evaluate(this);
      this.currentTemplateStack.Pop();
    }

    internal void OutputLiteralNamespaceUriNodes(
      Hashtable nsDecls,
      ArrayList excludedPrefixes,
      string localPrefixInCopy)
    {
      if (nsDecls == null)
        return;
      foreach (DictionaryEntry nsDecl in nsDecls)
      {
        string key1 = (string) nsDecl.Key;
        string nsUri = (string) nsDecl.Value;
        if (!(localPrefixInCopy == key1) && (localPrefixInCopy == null || key1.Length != 0 || this.XPathContext.ElementNamespace.Length != 0))
        {
          bool flag = false;
          if (this.style.ExcludeResultPrefixes != null)
          {
            foreach (XmlQualifiedName excludeResultPrefix in this.style.ExcludeResultPrefixes)
            {
              if (excludeResultPrefix.Namespace == nsUri)
                flag = true;
            }
          }
          if (!flag && this.style.NamespaceAliases[key1] == null)
          {
            string key2 = nsUri;
            if (key2 != null)
            {
              // ISSUE: reference to a compiler-generated field
              if (XslTransformProcessor.\u003C\u003Ef__switch\u0024map27 == null)
              {
                // ISSUE: reference to a compiler-generated field
                XslTransformProcessor.\u003C\u003Ef__switch\u0024map27 = new Dictionary<string, int>(3)
                {
                  {
                    "http://www.w3.org/1999/XSL/Transform",
                    0
                  },
                  {
                    "http://www.w3.org/XML/1998/namespace",
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
              if (XslTransformProcessor.\u003C\u003Ef__switch\u0024map27.TryGetValue(key2, out num))
              {
                switch (num)
                {
                  case 0:
                    continue;
                  case 1:
                    if (!("xml" == key1))
                      break;
                    continue;
                  case 2:
                    if (!("xmlns" == key1))
                      break;
                    continue;
                }
              }
            }
            if (excludedPrefixes == null || !excludedPrefixes.Contains((object) key1))
              this.Out.WriteNamespaceDecl(key1, nsUri);
          }
        }
      }
    }

    private XslTemplate FindTemplate(XPathNavigator node, XmlQualifiedName mode)
    {
      XslTemplate match = this.style.Templates.FindMatch(this.CurrentNode, mode, this);
      if (match != null)
        return match;
      switch (node.NodeType)
      {
        case XPathNodeType.Root:
        case XPathNodeType.Element:
          return mode == XmlQualifiedName.Empty ? XslDefaultNodeTemplate.Instance : (XslTemplate) new XslDefaultNodeTemplate(mode);
        case XPathNodeType.Attribute:
        case XPathNodeType.Text:
        case XPathNodeType.SignificantWhitespace:
        case XPathNodeType.Whitespace:
          return XslDefaultTextTemplate.Instance;
        case XPathNodeType.ProcessingInstruction:
        case XPathNodeType.Comment:
          return XslEmptyTemplate.Instance;
        default:
          return XslEmptyTemplate.Instance;
      }
    }

    private XslTemplate FindTemplate(XmlQualifiedName name) => this.style.Templates.FindTemplate(name) ?? throw new XsltException("Could not resolve named template " + (object) name, (Exception) null, this.CurrentNode);

    public void PushForEachContext() => this.currentTemplateStack.Push((object) null);

    public void PopForEachContext() => this.currentTemplateStack.Pop();

    public XPathNodeIterator CurrentNodeset => (XPathNodeIterator) this.nodesetStack[this.nodesetStack.Count - 1];

    public XPathNavigator CurrentNode
    {
      get
      {
        XPathNavigator current1 = this.CurrentNodeset.Current;
        if (current1 != null)
          return current1;
        for (int index = this.nodesetStack.Count - 2; index >= 0; --index)
        {
          XPathNavigator current2 = ((XPathNodeIterator) this.nodesetStack[index]).Current;
          if (current2 != null)
            return current2;
        }
        return (XPathNavigator) null;
      }
    }

    public bool NodesetMoveNext() => this.NodesetMoveNext(this.CurrentNodeset);

    public bool NodesetMoveNext(XPathNodeIterator iter)
    {
      if (!iter.MoveNext())
        return false;
      return iter.Current.NodeType != XPathNodeType.Whitespace || this.XPathContext.PreserveWhitespace(iter.Current) || this.NodesetMoveNext(iter);
    }

    public void PushNodeset(XPathNodeIterator itr)
    {
      BaseIterator baseIterator1 = !(itr is BaseIterator baseIterator2) ? (BaseIterator) new WrapperIterator(itr, (IXmlNamespaceResolver) null) : baseIterator2;
      baseIterator1.NamespaceManager = (IXmlNamespaceResolver) this.XPathContext;
      this.nodesetStack.Add((object) baseIterator1);
    }

    public void PopNodeset() => this.nodesetStack.RemoveAt(this.nodesetStack.Count - 1);

    public bool Matches(Pattern p, XPathNavigator n) => p.Matches(n, (XsltContext) this.XPathContext);

    public object Evaluate(XPathExpression expr)
    {
      BaseIterator currentNodeset = (BaseIterator) this.CurrentNodeset;
      CompiledExpression compiledExpression = (CompiledExpression) expr;
      if (currentNodeset.NamespaceManager == null)
        currentNodeset.NamespaceManager = compiledExpression.NamespaceManager;
      return compiledExpression.Evaluate(currentNodeset);
    }

    public string EvaluateString(XPathExpression expr)
    {
      XPathNodeIterator currentNodeset = this.CurrentNodeset;
      return currentNodeset.Current.EvaluateString(expr, currentNodeset, (IXmlNamespaceResolver) this.XPathContext);
    }

    public bool EvaluateBoolean(XPathExpression expr)
    {
      XPathNodeIterator currentNodeset = this.CurrentNodeset;
      return currentNodeset.Current.EvaluateBoolean(expr, currentNodeset, (IXmlNamespaceResolver) this.XPathContext);
    }

    public double EvaluateNumber(XPathExpression expr)
    {
      XPathNodeIterator currentNodeset = this.CurrentNodeset;
      return currentNodeset.Current.EvaluateNumber(expr, currentNodeset, (IXmlNamespaceResolver) this.XPathContext);
    }

    public XPathNodeIterator Select(XPathExpression expr) => this.CurrentNodeset.Current.Select(expr, (IXmlNamespaceResolver) this.XPathContext);

    public XslAttributeSet ResolveAttributeSet(XmlQualifiedName name) => this.CompiledStyle.ResolveAttributeSet(name);

    public int StackItemCount
    {
      get
      {
        if (this.currentStack == null)
          return 0;
        for (int stackItemCount = 0; stackItemCount < this.currentStack.Length; ++stackItemCount)
        {
          if (this.currentStack[stackItemCount] == null)
            return stackItemCount;
        }
        return this.currentStack.Length;
      }
    }

    public object GetStackItem(int slot) => this.currentStack[slot];

    public void SetStackItem(int slot, object o) => this.currentStack[slot] = o;

    public void PushStack(int stackSize)
    {
      this.variableStack.Push((object) this.currentStack);
      this.currentStack = new object[stackSize];
    }

    public void PopStack() => this.currentStack = (object[]) this.variableStack.Pop();

    public void SetBusy(object o) => this.busyTable[o] = XslTransformProcessor.busyObject;

    public void SetFree(object o) => this.busyTable.Remove(o);

    public bool IsBusy(object o) => this.busyTable[o] == XslTransformProcessor.busyObject;

    public bool PushElementState(string prefix, string name, string ns, bool preserveWhitespace)
    {
      bool flag1 = this.IsCData(name, ns);
      this.XPathContext.PushScope();
      Outputter outputter = this.Out;
      bool flag2 = flag1;
      this.XPathContext.IsCData = flag2;
      int num = flag2 ? 1 : 0;
      outputter.InsideCDataSection = num != 0;
      this.XPathContext.WhitespaceHandling = true;
      this.XPathContext.ElementPrefix = prefix;
      this.XPathContext.ElementNamespace = ns;
      return flag1;
    }

    private bool IsCData(string name, string ns)
    {
      for (int index = 0; index < this.Output.CDataSectionElements.Length; ++index)
      {
        XmlQualifiedName cdataSectionElement = this.Output.CDataSectionElements[index];
        if (cdataSectionElement.Name == name && cdataSectionElement.Namespace == ns)
          return true;
      }
      return false;
    }

    public void PopCDataState(bool isCData)
    {
      this.XPathContext.PopScope();
      this.Out.InsideCDataSection = this.XPathContext.IsCData;
    }

    public bool PreserveOutputWhitespace => this.XPathContext.Whitespace;
  }
}
