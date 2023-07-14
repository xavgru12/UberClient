// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.Expression
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Globalization;

namespace System.Xml.XPath
{
  internal abstract class Expression
  {
    public abstract XPathResultType ReturnType { get; }

    public virtual XPathResultType GetReturnType(BaseIterator iter) => this.ReturnType;

    public virtual Expression Optimize() => this;

    public virtual bool HasStaticValue => false;

    public virtual object StaticValue
    {
      get
      {
        switch (this.ReturnType)
        {
          case XPathResultType.Number:
            return (object) this.StaticValueAsNumber;
          case XPathResultType.String:
            return (object) this.StaticValueAsString;
          case XPathResultType.Boolean:
            return (object) this.StaticValueAsBoolean;
          default:
            return (object) null;
        }
      }
    }

    public virtual string StaticValueAsString => this.HasStaticValue ? XPathFunctions.ToString(this.StaticValue) : (string) null;

    public virtual double StaticValueAsNumber => this.HasStaticValue ? XPathFunctions.ToNumber(this.StaticValue) : 0.0;

    public virtual bool StaticValueAsBoolean => this.HasStaticValue && XPathFunctions.ToBoolean(this.StaticValue);

    public virtual XPathNavigator StaticValueAsNavigator => this.StaticValue as XPathNavigator;

    public abstract object Evaluate(BaseIterator iter);

    public virtual BaseIterator EvaluateNodeSet(BaseIterator iter)
    {
      XPathResultType returnType = this.GetReturnType(iter);
      switch (returnType)
      {
        case XPathResultType.NodeSet:
        case XPathResultType.Navigator:
        case XPathResultType.Any:
          object obj = this.Evaluate(iter);
          XPathNodeIterator iter1 = obj as XPathNodeIterator;
          nodeSet = (BaseIterator) null;
          if (iter1 != null)
          {
            if (!(iter1 is BaseIterator nodeSet))
              nodeSet = (BaseIterator) new WrapperIterator(iter1, iter.NamespaceManager);
            return nodeSet;
          }
          if (obj is XPathNavigator xpathNavigator)
          {
            XPathNodeIterator iter2 = xpathNavigator.SelectChildren(XPathNodeType.All);
            switch (iter2)
            {
              case BaseIterator nodeSet:
              case null:
                break;
              default:
                nodeSet = (BaseIterator) new WrapperIterator(iter2, iter.NamespaceManager);
                break;
            }
          }
          if (nodeSet != null)
            return nodeSet;
          if (obj == null)
            return (BaseIterator) new NullIterator(iter);
          returnType = Expression.GetReturnType(obj);
          break;
      }
      throw new XPathException(string.Format("expected nodeset but was {1}: {0}", (object) this.ToString(), (object) returnType));
    }

    protected static XPathResultType GetReturnType(object obj)
    {
      switch (obj)
      {
        case string _:
          return XPathResultType.String;
        case bool _:
          return XPathResultType.Boolean;
        case XPathNodeIterator _:
          return XPathResultType.NodeSet;
        case double _:
        case int _:
          return XPathResultType.Number;
        case XPathNavigator _:
          return XPathResultType.Navigator;
        default:
          throw new XPathException("invalid node type: " + obj.GetType().ToString());
      }
    }

    internal virtual XPathNodeType EvaluatedNodeType => XPathNodeType.All;

    internal virtual bool IsPositional => false;

    internal virtual bool Peer => false;

    public virtual double EvaluateNumber(BaseIterator iter)
    {
      XPathResultType xpathResultType = this.GetReturnType(iter);
      object number1;
      if (xpathResultType == XPathResultType.NodeSet)
      {
        number1 = (object) this.EvaluateString(iter);
        xpathResultType = XPathResultType.String;
      }
      else
        number1 = this.Evaluate(iter);
      if (xpathResultType == XPathResultType.Any)
        xpathResultType = Expression.GetReturnType(number1);
      switch (xpathResultType)
      {
        case XPathResultType.Number:
          switch (number1)
          {
            case double number2:
              return number2;
            case IConvertible _:
              return ((IConvertible) number1).ToDouble((IFormatProvider) CultureInfo.InvariantCulture);
            default:
              return (double) number1;
          }
        case XPathResultType.String:
          return XPathFunctions.ToNumber((string) number1);
        case XPathResultType.Boolean:
          return (bool) number1 ? 1.0 : 0.0;
        case XPathResultType.NodeSet:
          return XPathFunctions.ToNumber(this.EvaluateString(iter));
        case XPathResultType.Navigator:
          return XPathFunctions.ToNumber(((XPathItem) number1).Value);
        default:
          throw new XPathException("invalid node type");
      }
    }

    public virtual string EvaluateString(BaseIterator iter)
    {
      object d = this.Evaluate(iter);
      XPathResultType returnType = this.GetReturnType(iter);
      if (returnType == XPathResultType.Any)
        returnType = Expression.GetReturnType(d);
      switch (returnType)
      {
        case XPathResultType.Number:
          return XPathFunctions.ToString((double) d);
        case XPathResultType.String:
          return (string) d;
        case XPathResultType.Boolean:
          return (bool) d ? "true" : "false";
        case XPathResultType.NodeSet:
          BaseIterator baseIterator = (BaseIterator) d;
          return baseIterator == null || !baseIterator.MoveNext() ? string.Empty : baseIterator.Current.Value;
        case XPathResultType.Navigator:
          return ((XPathItem) d).Value;
        default:
          throw new XPathException("invalid node type");
      }
    }

    public virtual bool EvaluateBoolean(BaseIterator iter)
    {
      object boolean = this.Evaluate(iter);
      XPathResultType returnType = this.GetReturnType(iter);
      if (returnType == XPathResultType.Any)
        returnType = Expression.GetReturnType(boolean);
      switch (returnType)
      {
        case XPathResultType.Number:
          double d = Convert.ToDouble(boolean);
          return d != 0.0 && d != -0.0 && !double.IsNaN(d);
        case XPathResultType.String:
          return ((string) boolean).Length != 0;
        case XPathResultType.Boolean:
          return (bool) boolean;
        case XPathResultType.NodeSet:
          BaseIterator baseIterator = (BaseIterator) boolean;
          return baseIterator != null && baseIterator.MoveNext();
        case XPathResultType.Navigator:
          return ((XPathNavigator) boolean).HasChildren;
        default:
          throw new XPathException("invalid node type");
      }
    }

    public object EvaluateAs(BaseIterator iter, XPathResultType type)
    {
      switch (type)
      {
        case XPathResultType.Number:
          return (object) this.EvaluateNumber(iter);
        case XPathResultType.String:
          return (object) this.EvaluateString(iter);
        case XPathResultType.Boolean:
          return (object) this.EvaluateBoolean(iter);
        case XPathResultType.NodeSet:
          return (object) this.EvaluateNodeSet(iter);
        default:
          return this.Evaluate(iter);
      }
    }

    public virtual bool RequireSorting => false;
  }
}
