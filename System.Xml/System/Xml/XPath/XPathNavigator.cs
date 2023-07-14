// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.XPathNavigator
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using Mono.Xml.XPath;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml.Schema;
using System.Xml.Xsl;

namespace System.Xml.XPath
{
  public abstract class XPathNavigator : 
    XPathItem,
    ICloneable,
    IXPathNavigable,
    IXmlNamespaceResolver
  {
    private static readonly char[] escape_text_chars = new char[3]
    {
      '&',
      '<',
      '>'
    };
    private static readonly char[] escape_attr_chars = new char[6]
    {
      '"',
      '&',
      '<',
      '>',
      '\r',
      '\n'
    };

    object ICloneable.Clone() => (object) this.Clone();

    public static IEqualityComparer NavigatorComparer => (IEqualityComparer) XPathNavigatorComparer.Instance;

    public abstract string BaseURI { get; }

    public virtual bool CanEdit => false;

    public virtual bool HasAttributes
    {
      get
      {
        if (!this.MoveToFirstAttribute())
          return false;
        this.MoveToParent();
        return true;
      }
    }

    public virtual bool HasChildren
    {
      get
      {
        if (!this.MoveToFirstChild())
          return false;
        this.MoveToParent();
        return true;
      }
    }

    public abstract bool IsEmptyElement { get; }

    public abstract string LocalName { get; }

    public abstract string Name { get; }

    public abstract string NamespaceURI { get; }

    public abstract XmlNameTable NameTable { get; }

    public abstract XPathNodeType NodeType { get; }

    public abstract string Prefix { get; }

    public virtual string XmlLang
    {
      get
      {
        XPathNavigator xpathNavigator = this.Clone();
        switch (xpathNavigator.NodeType)
        {
          case XPathNodeType.Attribute:
          case XPathNodeType.Namespace:
            xpathNavigator.MoveToParent();
            break;
        }
        while (!xpathNavigator.MoveToAttribute("lang", "http://www.w3.org/XML/1998/namespace"))
        {
          if (!xpathNavigator.MoveToParent())
            return string.Empty;
        }
        return xpathNavigator.Value;
      }
    }

    public abstract XPathNavigator Clone();

    public virtual XmlNodeOrder ComparePosition(XPathNavigator nav)
    {
      if (this.IsSamePosition(nav))
        return XmlNodeOrder.Same;
      if (this.IsDescendant(nav))
        return XmlNodeOrder.Before;
      if (nav.IsDescendant(this))
        return XmlNodeOrder.After;
      XPathNavigator xpathNavigator = this.Clone();
      XPathNavigator other = nav.Clone();
      xpathNavigator.MoveToRoot();
      other.MoveToRoot();
      if (!xpathNavigator.IsSamePosition(other))
        return XmlNodeOrder.Unknown;
      xpathNavigator.MoveTo(this);
      other.MoveTo(nav);
      int num1 = 0;
      while (xpathNavigator.MoveToParent())
        ++num1;
      xpathNavigator.MoveTo(this);
      int num2 = 0;
      while (other.MoveToParent())
        ++num2;
      other.MoveTo(nav);
      int num3;
      for (num3 = num1; num3 > num2; --num3)
        xpathNavigator.MoveToParent();
      for (int index = num2; index > num3; --index)
        other.MoveToParent();
      while (!xpathNavigator.IsSamePosition(other))
      {
        xpathNavigator.MoveToParent();
        other.MoveToParent();
        --num3;
      }
      xpathNavigator.MoveTo(this);
      for (int index = num1; index > num3 + 1; --index)
        xpathNavigator.MoveToParent();
      other.MoveTo(nav);
      for (int index = num2; index > num3 + 1; --index)
        other.MoveToParent();
      if (xpathNavigator.NodeType == XPathNodeType.Namespace)
      {
        if (other.NodeType != XPathNodeType.Namespace)
          return XmlNodeOrder.Before;
        while (xpathNavigator.MoveToNextNamespace())
        {
          if (xpathNavigator.IsSamePosition(other))
            return XmlNodeOrder.Before;
        }
        return XmlNodeOrder.After;
      }
      if (other.NodeType == XPathNodeType.Namespace)
        return XmlNodeOrder.After;
      if (xpathNavigator.NodeType == XPathNodeType.Attribute)
      {
        if (other.NodeType != XPathNodeType.Attribute)
          return XmlNodeOrder.Before;
        while (xpathNavigator.MoveToNextAttribute())
        {
          if (xpathNavigator.IsSamePosition(other))
            return XmlNodeOrder.Before;
        }
        return XmlNodeOrder.After;
      }
      while (xpathNavigator.MoveToNext())
      {
        if (xpathNavigator.IsSamePosition(other))
          return XmlNodeOrder.Before;
      }
      return XmlNodeOrder.After;
    }

    public virtual XPathExpression Compile(string xpath) => XPathExpression.Compile(xpath);

    internal virtual XPathExpression Compile(string xpath, IStaticXsltContext ctx) => XPathExpression.Compile(xpath, (IXmlNamespaceResolver) null, ctx);

    public virtual object Evaluate(string xpath) => this.Evaluate(this.Compile(xpath));

    public virtual object Evaluate(XPathExpression expr) => this.Evaluate(expr, (XPathNodeIterator) null);

    public virtual object Evaluate(XPathExpression expr, XPathNodeIterator context) => this.Evaluate(expr, context, (IXmlNamespaceResolver) null);

    private BaseIterator ToBaseIterator(XPathNodeIterator iter, IXmlNamespaceResolver ctx)
    {
      if (!(iter is BaseIterator baseIterator))
        baseIterator = (BaseIterator) new WrapperIterator(iter, ctx);
      return baseIterator;
    }

    private object Evaluate(
      XPathExpression expr,
      XPathNodeIterator context,
      IXmlNamespaceResolver ctx)
    {
      CompiledExpression compiledExpression = (CompiledExpression) expr;
      if (ctx == null)
        ctx = compiledExpression.NamespaceManager;
      if (context == null)
        context = (XPathNodeIterator) new NullIterator(this, ctx);
      BaseIterator baseIterator = this.ToBaseIterator(context, ctx);
      baseIterator.NamespaceManager = ctx;
      return compiledExpression.Evaluate(baseIterator);
    }

    internal XPathNodeIterator EvaluateNodeSet(
      XPathExpression expr,
      XPathNodeIterator context,
      IXmlNamespaceResolver ctx)
    {
      CompiledExpression compiledExpression = (CompiledExpression) expr;
      if (ctx == null)
        ctx = compiledExpression.NamespaceManager;
      if (context == null)
        context = (XPathNodeIterator) new NullIterator(this, compiledExpression.NamespaceManager);
      BaseIterator baseIterator = this.ToBaseIterator(context, ctx);
      baseIterator.NamespaceManager = ctx;
      return compiledExpression.EvaluateNodeSet(baseIterator);
    }

    internal string EvaluateString(
      XPathExpression expr,
      XPathNodeIterator context,
      IXmlNamespaceResolver ctx)
    {
      CompiledExpression compiledExpression = (CompiledExpression) expr;
      if (ctx == null)
        ctx = compiledExpression.NamespaceManager;
      if (context == null)
        context = (XPathNodeIterator) new NullIterator(this, compiledExpression.NamespaceManager);
      BaseIterator baseIterator = this.ToBaseIterator(context, ctx);
      return compiledExpression.EvaluateString(baseIterator);
    }

    internal double EvaluateNumber(
      XPathExpression expr,
      XPathNodeIterator context,
      IXmlNamespaceResolver ctx)
    {
      CompiledExpression compiledExpression = (CompiledExpression) expr;
      if (ctx == null)
        ctx = compiledExpression.NamespaceManager;
      if (context == null)
        context = (XPathNodeIterator) new NullIterator(this, compiledExpression.NamespaceManager);
      BaseIterator baseIterator = this.ToBaseIterator(context, ctx);
      baseIterator.NamespaceManager = ctx;
      return compiledExpression.EvaluateNumber(baseIterator);
    }

    internal bool EvaluateBoolean(
      XPathExpression expr,
      XPathNodeIterator context,
      IXmlNamespaceResolver ctx)
    {
      CompiledExpression compiledExpression = (CompiledExpression) expr;
      if (ctx == null)
        ctx = compiledExpression.NamespaceManager;
      if (context == null)
        context = (XPathNodeIterator) new NullIterator(this, compiledExpression.NamespaceManager);
      BaseIterator baseIterator = this.ToBaseIterator(context, ctx);
      baseIterator.NamespaceManager = ctx;
      return compiledExpression.EvaluateBoolean(baseIterator);
    }

    public virtual string GetAttribute(string localName, string namespaceURI)
    {
      if (!this.MoveToAttribute(localName, namespaceURI))
        return string.Empty;
      string attribute = this.Value;
      this.MoveToParent();
      return attribute;
    }

    public virtual string GetNamespace(string name)
    {
      if (!this.MoveToNamespace(name))
        return string.Empty;
      string str = this.Value;
      this.MoveToParent();
      return str;
    }

    public virtual bool IsDescendant(XPathNavigator nav)
    {
      if (nav != null)
      {
        nav = nav.Clone();
        while (nav.MoveToParent())
        {
          if (this.IsSamePosition(nav))
            return true;
        }
      }
      return false;
    }

    public abstract bool IsSamePosition(XPathNavigator other);

    public virtual bool Matches(string xpath) => this.Matches(this.Compile(xpath));

    public virtual bool Matches(XPathExpression expr)
    {
      Expression expression = ((CompiledExpression) expr).ExpressionNode;
      switch (expression)
      {
        case ExprRoot _:
          return this.NodeType == XPathNodeType.Root;
        case NodeTest nodeTest:
          switch (nodeTest.Axis.Axis)
          {
            case Axes.Attribute:
            case Axes.Child:
              return nodeTest.Match(((CompiledExpression) expr).NamespaceManager, this);
            default:
              throw new XPathException("Only child and attribute pattern are allowed for a pattern.");
          }
        case ExprFilter _:
          do
          {
            expression = ((ExprFilter) expression).LeftHandSide;
          }
          while (expression is ExprFilter);
          if (expression is NodeTest && !((NodeTest) expression).Match(((CompiledExpression) expr).NamespaceManager, this))
            return false;
          break;
      }
      switch (expression.ReturnType)
      {
        case XPathResultType.NodeSet:
        case XPathResultType.Any:
          switch (expression.EvaluatedNodeType)
          {
            case XPathNodeType.Attribute:
            case XPathNodeType.Namespace:
              if (this.NodeType != expression.EvaluatedNodeType)
                return false;
              break;
          }
          XPathNodeIterator xpathNodeIterator1 = this.Select(expr);
          while (xpathNodeIterator1.MoveNext())
          {
            if (this.IsSamePosition(xpathNodeIterator1.Current))
              return true;
          }
          XPathNavigator xpathNavigator = this.Clone();
          while (xpathNavigator.MoveToParent())
          {
            XPathNodeIterator xpathNodeIterator2 = xpathNavigator.Select(expr);
            while (xpathNodeIterator2.MoveNext())
            {
              if (this.IsSamePosition(xpathNodeIterator2.Current))
                return true;
            }
          }
          return false;
        default:
          return false;
      }
    }

    public abstract bool MoveTo(XPathNavigator other);

    public virtual bool MoveToAttribute(string localName, string namespaceURI)
    {
      if (this.MoveToFirstAttribute())
      {
        while (!(this.LocalName == localName) || !(this.NamespaceURI == namespaceURI))
        {
          if (!this.MoveToNextAttribute())
          {
            this.MoveToParent();
            goto label_5;
          }
        }
        return true;
      }
label_5:
      return false;
    }

    public virtual bool MoveToNamespace(string name)
    {
      if (this.MoveToFirstNamespace())
      {
        while (!(this.LocalName == name))
        {
          if (!this.MoveToNextNamespace())
          {
            this.MoveToParent();
            goto label_5;
          }
        }
        return true;
      }
label_5:
      return false;
    }

    public virtual bool MoveToFirst() => this.MoveToFirstImpl();

    public virtual void MoveToRoot()
    {
      do
        ;
      while (this.MoveToParent());
    }

    internal bool MoveToFirstImpl()
    {
      switch (this.NodeType)
      {
        case XPathNodeType.Attribute:
        case XPathNodeType.Namespace:
          return false;
        default:
          if (!this.MoveToParent())
            return false;
          this.MoveToFirstChild();
          return true;
      }
    }

    public abstract bool MoveToFirstAttribute();

    public abstract bool MoveToFirstChild();

    public bool MoveToFirstNamespace() => this.MoveToFirstNamespace(XPathNamespaceScope.All);

    public abstract bool MoveToFirstNamespace(XPathNamespaceScope namespaceScope);

    public abstract bool MoveToId(string id);

    public abstract bool MoveToNext();

    public abstract bool MoveToNextAttribute();

    public bool MoveToNextNamespace() => this.MoveToNextNamespace(XPathNamespaceScope.All);

    public abstract bool MoveToNextNamespace(XPathNamespaceScope namespaceScope);

    public abstract bool MoveToParent();

    public abstract bool MoveToPrevious();

    public virtual XPathNodeIterator Select(string xpath) => this.Select(this.Compile(xpath));

    public virtual XPathNodeIterator Select(XPathExpression expr) => this.Select(expr, (IXmlNamespaceResolver) null);

    internal XPathNodeIterator Select(XPathExpression expr, IXmlNamespaceResolver ctx)
    {
      CompiledExpression compiledExpression = (CompiledExpression) expr;
      if (ctx == null)
        ctx = compiledExpression.NamespaceManager;
      BaseIterator iter = (BaseIterator) new NullIterator(this, ctx);
      return compiledExpression.EvaluateNodeSet(iter);
    }

    public virtual XPathNodeIterator SelectAncestors(XPathNodeType type, bool matchSelf) => this.SelectTest((NodeTest) new NodeTypeTest(!matchSelf ? Axes.Ancestor : Axes.AncestorOrSelf, type));

    public virtual XPathNodeIterator SelectAncestors(
      string name,
      string namespaceURI,
      bool matchSelf)
    {
      if (name == null)
        throw new ArgumentNullException(nameof (name));
      if (namespaceURI == null)
        throw new ArgumentNullException(nameof (namespaceURI));
      return this.SelectTest((NodeTest) new NodeNameTest(!matchSelf ? Axes.Ancestor : Axes.AncestorOrSelf, new XmlQualifiedName(name, namespaceURI), true));
    }

    [DebuggerHidden]
    private static IEnumerable EnumerateChildren(XPathNavigator n, XPathNodeType type)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      XPathNavigator.\u003CEnumerateChildren\u003Ec__Iterator0 childrenCIterator0 = new XPathNavigator.\u003CEnumerateChildren\u003Ec__Iterator0()
      {
        n = n,
        type = type,
        \u003C\u0024\u003En = n,
        \u003C\u0024\u003Etype = type
      };
      // ISSUE: reference to a compiler-generated field
      childrenCIterator0.\u0024PC = -2;
      return (IEnumerable) childrenCIterator0;
    }

    public virtual XPathNodeIterator SelectChildren(XPathNodeType type) => (XPathNodeIterator) new WrapperIterator((XPathNodeIterator) new XPathNavigator.EnumerableIterator(XPathNavigator.EnumerateChildren(this, type), 0), (IXmlNamespaceResolver) null);

    [DebuggerHidden]
    private static IEnumerable EnumerateChildren(XPathNavigator n, string name, string ns)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      XPathNavigator.\u003CEnumerateChildren\u003Ec__Iterator1 childrenCIterator1 = new XPathNavigator.\u003CEnumerateChildren\u003Ec__Iterator1()
      {
        n = n,
        name = name,
        ns = ns,
        \u003C\u0024\u003En = n,
        \u003C\u0024\u003Ename = name,
        \u003C\u0024\u003Ens = ns
      };
      // ISSUE: reference to a compiler-generated field
      childrenCIterator1.\u0024PC = -2;
      return (IEnumerable) childrenCIterator1;
    }

    public virtual XPathNodeIterator SelectChildren(string name, string namespaceURI)
    {
      if (name == null)
        throw new ArgumentNullException(nameof (name));
      return namespaceURI != null ? (XPathNodeIterator) new WrapperIterator((XPathNodeIterator) new XPathNavigator.EnumerableIterator(XPathNavigator.EnumerateChildren(this, name, namespaceURI), 0), (IXmlNamespaceResolver) null) : throw new ArgumentNullException(nameof (namespaceURI));
    }

    public virtual XPathNodeIterator SelectDescendants(XPathNodeType type, bool matchSelf) => this.SelectTest((NodeTest) new NodeTypeTest(!matchSelf ? Axes.Descendant : Axes.DescendantOrSelf, type));

    public virtual XPathNodeIterator SelectDescendants(
      string name,
      string namespaceURI,
      bool matchSelf)
    {
      if (name == null)
        throw new ArgumentNullException(nameof (name));
      if (namespaceURI == null)
        throw new ArgumentNullException(nameof (namespaceURI));
      return this.SelectTest((NodeTest) new NodeNameTest(!matchSelf ? Axes.Descendant : Axes.DescendantOrSelf, new XmlQualifiedName(name, namespaceURI), true));
    }

    internal XPathNodeIterator SelectTest(NodeTest test) => (XPathNodeIterator) test.EvaluateNodeSet((BaseIterator) new NullIterator(this));

    public override string ToString() => this.Value;

    public virtual bool CheckValidity(XmlSchemaSet schemas, ValidationEventHandler handler)
    {
      XmlReaderSettings settings = new XmlReaderSettings();
      settings.NameTable = this.NameTable;
      settings.SetSchemas(schemas);
      settings.ValidationEventHandler += handler;
      settings.ValidationType = ValidationType.Schema;
      try
      {
        XmlReader xmlReader = XmlReader.Create(this.ReadSubtree(), settings);
        while (!xmlReader.EOF)
          xmlReader.Read();
      }
      catch (XmlSchemaValidationException ex)
      {
        return false;
      }
      return true;
    }

    public virtual XPathNavigator CreateNavigator() => this.Clone();

    public virtual object Evaluate(string xpath, IXmlNamespaceResolver nsResolver) => this.Evaluate(this.Compile(xpath), (XPathNodeIterator) null, nsResolver);

    public virtual IDictionary<string, string> GetNamespacesInScope(XmlNamespaceScope scope)
    {
      IDictionary<string, string> namespacesInScope = (IDictionary<string, string>) new Dictionary<string, string>();
      int num;
      switch (scope)
      {
        case XmlNamespaceScope.ExcludeXml:
          num = 1;
          break;
        case XmlNamespaceScope.Local:
          num = 2;
          break;
        default:
          num = 0;
          break;
      }
      XPathNamespaceScope namespaceScope = (XPathNamespaceScope) num;
      XPathNavigator xpathNavigator = this.Clone();
      if (xpathNavigator.NodeType != XPathNodeType.Element)
        xpathNavigator.MoveToParent();
      if (!xpathNavigator.MoveToFirstNamespace(namespaceScope))
        return namespacesInScope;
      do
      {
        namespacesInScope.Add(xpathNavigator.Name, xpathNavigator.Value);
      }
      while (xpathNavigator.MoveToNextNamespace(namespaceScope));
      return namespacesInScope;
    }

    public virtual string LookupNamespace(string prefix)
    {
      XPathNavigator xpathNavigator = this.Clone();
      if (xpathNavigator.NodeType != XPathNodeType.Element)
        xpathNavigator.MoveToParent();
      return xpathNavigator.MoveToNamespace(prefix) ? xpathNavigator.Value : (string) null;
    }

    public virtual string LookupPrefix(string namespaceUri)
    {
      XPathNavigator xpathNavigator = this.Clone();
      if (xpathNavigator.NodeType != XPathNodeType.Element)
        xpathNavigator.MoveToParent();
      if (!xpathNavigator.MoveToFirstNamespace())
        return (string) null;
      while (!(xpathNavigator.Value == namespaceUri))
      {
        if (!xpathNavigator.MoveToNextNamespace())
          return (string) null;
      }
      return xpathNavigator.Name;
    }

    private bool MoveTo(XPathNodeIterator iter)
    {
      if (!iter.MoveNext())
        return false;
      this.MoveTo(iter.Current);
      return true;
    }

    public virtual bool MoveToChild(XPathNodeType type) => this.MoveTo(this.SelectChildren(type));

    public virtual bool MoveToChild(string localName, string namespaceURI) => this.MoveTo(this.SelectChildren(localName, namespaceURI));

    public virtual bool MoveToNext(string localName, string namespaceURI)
    {
      XPathNavigator other = this.Clone();
      while (other.MoveToNext())
      {
        if (other.LocalName == localName && other.NamespaceURI == namespaceURI)
        {
          this.MoveTo(other);
          return true;
        }
      }
      return false;
    }

    public virtual bool MoveToNext(XPathNodeType type)
    {
      XPathNavigator other = this.Clone();
      while (other.MoveToNext())
      {
        if (type == XPathNodeType.All || other.NodeType == type)
        {
          this.MoveTo(other);
          return true;
        }
      }
      return false;
    }

    public virtual bool MoveToFollowing(string localName, string namespaceURI) => this.MoveToFollowing(localName, namespaceURI, (XPathNavigator) null);

    public virtual bool MoveToFollowing(string localName, string namespaceURI, XPathNavigator end)
    {
      if (localName == null)
        throw new ArgumentNullException(nameof (localName));
      if (namespaceURI == null)
        throw new ArgumentNullException(nameof (namespaceURI));
      localName = this.NameTable.Get(localName);
      if (localName == null)
        return false;
      namespaceURI = this.NameTable.Get(namespaceURI);
      if (namespaceURI == null)
        return false;
      XPathNavigator other = this.Clone();
      switch (other.NodeType)
      {
        case XPathNodeType.Attribute:
        case XPathNodeType.Namespace:
          other.MoveToParent();
          break;
      }
      do
      {
        if (!other.MoveToFirstChild())
        {
          while (!other.MoveToNext())
          {
            if (!other.MoveToParent())
              return false;
          }
        }
        if (end != null && end.IsSamePosition(other))
          return false;
      }
      while (!object.ReferenceEquals((object) localName, (object) other.LocalName) || !object.ReferenceEquals((object) namespaceURI, (object) other.NamespaceURI));
      this.MoveTo(other);
      return true;
    }

    public virtual bool MoveToFollowing(XPathNodeType type) => this.MoveToFollowing(type, (XPathNavigator) null);

    public virtual bool MoveToFollowing(XPathNodeType type, XPathNavigator end)
    {
      if (type == XPathNodeType.Root)
        return false;
      XPathNavigator other = this.Clone();
      switch (other.NodeType)
      {
        case XPathNodeType.Attribute:
        case XPathNodeType.Namespace:
          other.MoveToParent();
          break;
      }
      do
      {
        if (!other.MoveToFirstChild())
        {
          while (!other.MoveToNext())
          {
            if (!other.MoveToParent())
              return false;
          }
        }
        if (end != null && end.IsSamePosition(other))
          return false;
      }
      while (type != XPathNodeType.All && other.NodeType != type);
      this.MoveTo(other);
      return true;
    }

    public virtual XmlReader ReadSubtree()
    {
      switch (this.NodeType)
      {
        case XPathNodeType.Root:
        case XPathNodeType.Element:
          return (XmlReader) new XPathNavigatorReader(this);
        default:
          throw new InvalidOperationException(string.Format("NodeType {0} is not supported to read as a subtree of an XPathNavigator.", (object) this.NodeType));
      }
    }

    public virtual XPathNodeIterator Select(string xpath, IXmlNamespaceResolver nsResolver) => this.Select(this.Compile(xpath), nsResolver);

    public virtual XPathNavigator SelectSingleNode(string xpath) => this.SelectSingleNode(xpath, (IXmlNamespaceResolver) null);

    public virtual XPathNavigator SelectSingleNode(string xpath, IXmlNamespaceResolver nsResolver)
    {
      XPathExpression expression = this.Compile(xpath);
      expression.SetContext(nsResolver);
      return this.SelectSingleNode(expression);
    }

    public virtual XPathNavigator SelectSingleNode(XPathExpression expression)
    {
      XPathNodeIterator xpathNodeIterator = this.Select(expression);
      return xpathNodeIterator.MoveNext() ? xpathNodeIterator.Current : (XPathNavigator) null;
    }

    public override object ValueAs(Type type, IXmlNamespaceResolver nsResolver) => new XmlAtomicValue(this.Value, (XmlSchemaType) XmlSchemaSimpleType.XsString).ValueAs(type, nsResolver);

    public virtual void WriteSubtree(XmlWriter writer) => writer.WriteNode(this, false);

    private static string EscapeString(string value, bool attr)
    {
      char[] anyOf = !attr ? XPathNavigator.escape_text_chars : XPathNavigator.escape_attr_chars;
      if (value.IndexOfAny(anyOf) < 0)
        return value;
      StringBuilder stringBuilder = new StringBuilder(value, value.Length + 10);
      if (attr)
        stringBuilder.Replace("\"", "&quot;");
      stringBuilder.Replace("<", "&lt;");
      stringBuilder.Replace(">", "&gt;");
      if (attr)
      {
        stringBuilder.Replace("\r\n", "&#10;");
        stringBuilder.Replace("\r", "&#10;");
        stringBuilder.Replace("\n", "&#10;");
      }
      return stringBuilder.ToString();
    }

    public virtual string InnerXml
    {
      get
      {
        switch (this.NodeType)
        {
          case XPathNodeType.Attribute:
          case XPathNodeType.Namespace:
            return XPathNavigator.EscapeString(this.Value, true);
          case XPathNodeType.Text:
          case XPathNodeType.SignificantWhitespace:
          case XPathNodeType.Whitespace:
            return string.Empty;
          case XPathNodeType.ProcessingInstruction:
          case XPathNodeType.Comment:
            return this.Value;
          default:
            XmlReader reader = this.ReadSubtree();
            reader.Read();
            int num = reader.Depth;
            if (this.NodeType != XPathNodeType.Root)
              reader.Read();
            else
              num = -1;
            StringWriter writer = new StringWriter();
            XmlWriter xmlWriter = XmlWriter.Create((TextWriter) writer, new XmlWriterSettings()
            {
              Indent = true,
              ConformanceLevel = ConformanceLevel.Fragment,
              OmitXmlDeclaration = true
            });
            while (!reader.EOF && reader.Depth > num)
              xmlWriter.WriteNode(reader, false);
            return writer.ToString();
        }
      }
      set
      {
        this.DeleteChildren();
        if (this.NodeType == XPathNodeType.Attribute)
          this.SetValue(value);
        else
          this.AppendChild(value);
      }
    }

    public override sealed bool IsNode => true;

    public virtual string OuterXml
    {
      get
      {
        switch (this.NodeType)
        {
          case XPathNodeType.Attribute:
            return this.Prefix + (this.Prefix.Length <= 0 ? string.Empty : ":") + this.LocalName + "=\"" + XPathNavigator.EscapeString(this.Value, true) + "\"";
          case XPathNodeType.Namespace:
            return "xmlns" + (this.LocalName.Length <= 0 ? string.Empty : ":") + this.LocalName + "=\"" + XPathNavigator.EscapeString(this.Value, true) + "\"";
          case XPathNodeType.Text:
            return XPathNavigator.EscapeString(this.Value, false);
          case XPathNodeType.SignificantWhitespace:
          case XPathNodeType.Whitespace:
            return this.Value;
          default:
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;
            settings.ConformanceLevel = ConformanceLevel.Fragment;
            StringBuilder builder = new StringBuilder();
            using (XmlWriter writer = XmlWriter.Create(builder, settings))
              this.WriteSubtree(writer);
            return builder.ToString();
        }
      }
      set
      {
        switch (this.NodeType)
        {
          case XPathNodeType.Root:
          case XPathNodeType.Attribute:
          case XPathNodeType.Namespace:
            throw new XmlException("Setting OuterXml Root, Attribute and Namespace is not supported.");
          default:
            this.DeleteSelf();
            this.AppendChild(value);
            this.MoveToFirstChild();
            break;
        }
      }
    }

    public virtual IXmlSchemaInfo SchemaInfo => (IXmlSchemaInfo) null;

    public override object TypedValue
    {
      get
      {
        switch (this.NodeType)
        {
          case XPathNodeType.Element:
          case XPathNodeType.Attribute:
            if (this.XmlType != null)
            {
              XmlSchemaDatatype datatype = this.XmlType.Datatype;
              if (datatype != null)
                return datatype.ParseValue(this.Value, this.NameTable, (IXmlNamespaceResolver) this);
              break;
            }
            break;
        }
        return (object) this.Value;
      }
    }

    public virtual object UnderlyingObject => (object) null;

    public override bool ValueAsBoolean => XQueryConvert.StringToBoolean(this.Value);

    public override DateTime ValueAsDateTime => XmlConvert.ToDateTime(this.Value);

    public override double ValueAsDouble => XQueryConvert.StringToDouble(this.Value);

    public override int ValueAsInt => XQueryConvert.StringToInt(this.Value);

    public override long ValueAsLong => XQueryConvert.StringToInteger(this.Value);

    public override Type ValueType => this.SchemaInfo != null && this.SchemaInfo.SchemaType != null && this.SchemaInfo.SchemaType.Datatype != null ? this.SchemaInfo.SchemaType.Datatype.ValueType : (Type) null;

    public override XmlSchemaType XmlType => this.SchemaInfo != null ? this.SchemaInfo.SchemaType : (XmlSchemaType) null;

    private XmlReader CreateFragmentReader(string fragment)
    {
      XmlReaderSettings settings = new XmlReaderSettings();
      settings.ConformanceLevel = ConformanceLevel.Fragment;
      XmlNamespaceManager nsMgr = new XmlNamespaceManager(this.NameTable);
      foreach (KeyValuePair<string, string> keyValuePair in (IEnumerable<KeyValuePair<string, string>>) this.GetNamespacesInScope(XmlNamespaceScope.All))
        nsMgr.AddNamespace(keyValuePair.Key, keyValuePair.Value);
      return XmlReader.Create((TextReader) new StringReader(fragment), settings, new XmlParserContext(this.NameTable, nsMgr, (string) null, XmlSpace.None));
    }

    public virtual XmlWriter AppendChild() => throw new NotSupportedException();

    public virtual void AppendChild(string xmlFragments) => this.AppendChild(this.CreateFragmentReader(xmlFragments));

    public virtual void AppendChild(XmlReader reader)
    {
      XmlWriter xmlWriter = this.AppendChild();
      while (!reader.EOF)
        xmlWriter.WriteNode(reader, false);
      xmlWriter.Close();
    }

    public virtual void AppendChild(XPathNavigator nav) => this.AppendChild((XmlReader) new XPathNavigatorReader(nav));

    public virtual void AppendChildElement(string prefix, string name, string ns, string value)
    {
      XmlWriter xmlWriter = this.AppendChild();
      xmlWriter.WriteStartElement(prefix, name, ns);
      xmlWriter.WriteString(value);
      xmlWriter.WriteEndElement();
      xmlWriter.Close();
    }

    public virtual void CreateAttribute(
      string prefix,
      string localName,
      string namespaceURI,
      string value)
    {
      using (XmlWriter attributes = this.CreateAttributes())
        attributes.WriteAttributeString(prefix, localName, namespaceURI, value);
    }

    public virtual XmlWriter CreateAttributes() => throw new NotSupportedException();

    public virtual void DeleteSelf() => throw new NotSupportedException();

    public virtual void DeleteRange(XPathNavigator nav) => throw new NotSupportedException();

    public virtual XmlWriter ReplaceRange(XPathNavigator nav) => throw new NotSupportedException();

    public virtual XmlWriter InsertAfter()
    {
      switch (this.NodeType)
      {
        case XPathNodeType.Root:
        case XPathNodeType.Attribute:
        case XPathNodeType.Namespace:
          throw new InvalidOperationException(string.Format("Insertion after {0} is not allowed.", (object) this.NodeType));
        default:
          XPathNavigator xpathNavigator = this.Clone();
          if (xpathNavigator.MoveToNext())
            return xpathNavigator.InsertBefore();
          return xpathNavigator.MoveToParent() ? xpathNavigator.AppendChild() : throw new InvalidOperationException("Could not move to parent to insert sibling node");
      }
    }

    public virtual void InsertAfter(string xmlFragments) => this.InsertAfter(this.CreateFragmentReader(xmlFragments));

    public virtual void InsertAfter(XmlReader reader)
    {
      using (XmlWriter xmlWriter = this.InsertAfter())
        xmlWriter.WriteNode(reader, false);
    }

    public virtual void InsertAfter(XPathNavigator nav) => this.InsertAfter((XmlReader) new XPathNavigatorReader(nav));

    public virtual XmlWriter InsertBefore() => throw new NotSupportedException();

    public virtual void InsertBefore(string xmlFragments) => this.InsertBefore(this.CreateFragmentReader(xmlFragments));

    public virtual void InsertBefore(XmlReader reader)
    {
      using (XmlWriter xmlWriter = this.InsertBefore())
        xmlWriter.WriteNode(reader, false);
    }

    public virtual void InsertBefore(XPathNavigator nav) => this.InsertBefore((XmlReader) new XPathNavigatorReader(nav));

    public virtual void InsertElementAfter(
      string prefix,
      string localName,
      string namespaceURI,
      string value)
    {
      using (XmlWriter xmlWriter = this.InsertAfter())
        xmlWriter.WriteElementString(prefix, localName, namespaceURI, value);
    }

    public virtual void InsertElementBefore(
      string prefix,
      string localName,
      string namespaceURI,
      string value)
    {
      using (XmlWriter xmlWriter = this.InsertBefore())
        xmlWriter.WriteElementString(prefix, localName, namespaceURI, value);
    }

    public virtual XmlWriter PrependChild()
    {
      XPathNavigator xpathNavigator = this.Clone();
      return xpathNavigator.MoveToFirstChild() ? xpathNavigator.InsertBefore() : this.AppendChild();
    }

    public virtual void PrependChild(string xmlFragments) => this.PrependChild(this.CreateFragmentReader(xmlFragments));

    public virtual void PrependChild(XmlReader reader)
    {
      using (XmlWriter xmlWriter = this.PrependChild())
        xmlWriter.WriteNode(reader, false);
    }

    public virtual void PrependChild(XPathNavigator nav) => this.PrependChild((XmlReader) new XPathNavigatorReader(nav));

    public virtual void PrependChildElement(
      string prefix,
      string localName,
      string namespaceURI,
      string value)
    {
      using (XmlWriter xmlWriter = this.PrependChild())
        xmlWriter.WriteElementString(prefix, localName, namespaceURI, value);
    }

    public virtual void ReplaceSelf(string xmlFragment) => this.ReplaceSelf(this.CreateFragmentReader(xmlFragment));

    public virtual void ReplaceSelf(XmlReader reader) => throw new NotSupportedException();

    public virtual void ReplaceSelf(XPathNavigator navigator) => this.ReplaceSelf((XmlReader) new XPathNavigatorReader(navigator));

    [MonoTODO]
    public virtual void SetTypedValue(object value) => throw new NotSupportedException();

    public virtual void SetValue(string value) => throw new NotSupportedException();

    private void DeleteChildren()
    {
      switch (this.NodeType)
      {
        case XPathNodeType.Attribute:
          break;
        case XPathNodeType.Namespace:
          throw new InvalidOperationException("Removing namespace node content is not supported.");
        case XPathNodeType.Text:
        case XPathNodeType.SignificantWhitespace:
        case XPathNodeType.Whitespace:
        case XPathNodeType.ProcessingInstruction:
        case XPathNodeType.Comment:
          this.DeleteSelf();
          break;
        default:
          if (!this.HasChildren)
            break;
          XPathNavigator xpathNavigator = this.Clone();
          xpathNavigator.MoveToFirstChild();
          while (!xpathNavigator.IsSamePosition(this))
            xpathNavigator.DeleteSelf();
          break;
      }
    }

    private class EnumerableIterator : XPathNodeIterator
    {
      private IEnumerable source;
      private IEnumerator e;
      private int pos;

      public EnumerableIterator(IEnumerable source, int pos)
      {
        this.source = source;
        for (int index = 0; index < pos; ++index)
          this.MoveNext();
      }

      public override XPathNodeIterator Clone() => (XPathNodeIterator) new XPathNavigator.EnumerableIterator(this.source, this.pos);

      public override bool MoveNext()
      {
        if (this.e == null)
          this.e = this.source.GetEnumerator();
        if (!this.e.MoveNext())
          return false;
        ++this.pos;
        return true;
      }

      public override int CurrentPosition => this.pos;

      public override XPathNavigator Current => this.pos == 0 ? (XPathNavigator) null : (XPathNavigator) this.e.Current;
    }
  }
}
