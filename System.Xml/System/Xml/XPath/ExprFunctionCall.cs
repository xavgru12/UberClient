// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.ExprFunctionCall
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.Collections.Generic;
using System.Xml.Xsl;

namespace System.Xml.XPath
{
  internal class ExprFunctionCall : Expression
  {
    protected readonly XmlQualifiedName _name;
    protected readonly bool resolvedName;
    protected readonly ArrayList _args = new ArrayList();

    public ExprFunctionCall(XmlQualifiedName name, FunctionArguments args, IStaticXsltContext ctx)
    {
      if (ctx != null)
      {
        name = ctx.LookupQName(name.ToString());
        this.resolvedName = true;
      }
      this._name = name;
      args?.ToArrayList(this._args);
    }

    public static Expression Factory(
      XmlQualifiedName name,
      FunctionArguments args,
      IStaticXsltContext ctx)
    {
      if (name.Namespace != null && name.Namespace != string.Empty)
        return (Expression) new ExprFunctionCall(name, args, ctx);
      string name1 = name.Name;
      if (name1 != null)
      {
        // ISSUE: reference to a compiler-generated field
        if (ExprFunctionCall.\u003C\u003Ef__switch\u0024map41 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ExprFunctionCall.\u003C\u003Ef__switch\u0024map41 = new Dictionary<string, int>(27)
          {
            {
              "last",
              0
            },
            {
              "position",
              1
            },
            {
              "count",
              2
            },
            {
              "id",
              3
            },
            {
              "local-name",
              4
            },
            {
              "namespace-uri",
              5
            },
            {
              nameof (name),
              6
            },
            {
              "string",
              7
            },
            {
              "concat",
              8
            },
            {
              "starts-with",
              9
            },
            {
              "contains",
              10
            },
            {
              "substring-before",
              11
            },
            {
              "substring-after",
              12
            },
            {
              "substring",
              13
            },
            {
              "string-length",
              14
            },
            {
              "normalize-space",
              15
            },
            {
              "translate",
              16
            },
            {
              "boolean",
              17
            },
            {
              "not",
              18
            },
            {
              "true",
              19
            },
            {
              "false",
              20
            },
            {
              "lang",
              21
            },
            {
              "number",
              22
            },
            {
              "sum",
              23
            },
            {
              "floor",
              24
            },
            {
              "ceiling",
              25
            },
            {
              "round",
              26
            }
          };
        }
        int num;
        // ISSUE: reference to a compiler-generated field
        if (ExprFunctionCall.\u003C\u003Ef__switch\u0024map41.TryGetValue(name1, out num))
        {
          switch (num)
          {
            case 0:
              return (Expression) new XPathFunctionLast(args);
            case 1:
              return (Expression) new XPathFunctionPosition(args);
            case 2:
              return (Expression) new XPathFunctionCount(args);
            case 3:
              return (Expression) new XPathFunctionId(args);
            case 4:
              return (Expression) new XPathFunctionLocalName(args);
            case 5:
              return (Expression) new XPathFunctionNamespaceUri(args);
            case 6:
              return (Expression) new XPathFunctionName(args);
            case 7:
              return (Expression) new XPathFunctionString(args);
            case 8:
              return (Expression) new XPathFunctionConcat(args);
            case 9:
              return (Expression) new XPathFunctionStartsWith(args);
            case 10:
              return (Expression) new XPathFunctionContains(args);
            case 11:
              return (Expression) new XPathFunctionSubstringBefore(args);
            case 12:
              return (Expression) new XPathFunctionSubstringAfter(args);
            case 13:
              return (Expression) new XPathFunctionSubstring(args);
            case 14:
              return (Expression) new XPathFunctionStringLength(args);
            case 15:
              return (Expression) new XPathFunctionNormalizeSpace(args);
            case 16:
              return (Expression) new XPathFunctionTranslate(args);
            case 17:
              return (Expression) new XPathFunctionBoolean(args);
            case 18:
              return (Expression) new XPathFunctionNot(args);
            case 19:
              return (Expression) new XPathFunctionTrue(args);
            case 20:
              return (Expression) new XPathFunctionFalse(args);
            case 21:
              return (Expression) new XPathFunctionLang(args);
            case 22:
              return (Expression) new XPathFunctionNumber(args);
            case 23:
              return (Expression) new XPathFunctionSum(args);
            case 24:
              return (Expression) new XPathFunctionFloor(args);
            case 25:
              return (Expression) new XPathFunctionCeil(args);
            case 26:
              return (Expression) new XPathFunctionRound(args);
          }
        }
      }
      return (Expression) new ExprFunctionCall(name, args, ctx);
    }

    public override string ToString()
    {
      string empty = string.Empty;
      for (int index = 0; index < this._args.Count; ++index)
      {
        Expression expression = (Expression) this._args[index];
        if (empty != string.Empty)
          empty += ", ";
        empty += expression.ToString();
      }
      return this._name.ToString() + (object) '(' + empty + (object) ')';
    }

    public override XPathResultType ReturnType => XPathResultType.Any;

    public override XPathResultType GetReturnType(BaseIterator iter) => XPathResultType.Any;

    private XPathResultType[] GetArgTypes(BaseIterator iter)
    {
      XPathResultType[] argTypes = new XPathResultType[this._args.Count];
      for (int index = 0; index < this._args.Count; ++index)
        argTypes[index] = ((Expression) this._args[index]).GetReturnType(iter);
      return argTypes;
    }

    public override object Evaluate(BaseIterator iter)
    {
      XPathResultType[] argTypes1 = this.GetArgTypes(iter);
      IXsltContextFunction xsltContextFunction = (IXsltContextFunction) null;
      if (iter.NamespaceManager is XsltContext namespaceManager)
        xsltContextFunction = !this.resolvedName ? namespaceManager.ResolveFunction(this._name.Namespace, this._name.Name, argTypes1) : namespaceManager.ResolveFunction(this._name, argTypes1);
      if (xsltContextFunction == null)
        throw new XPathException("function " + this._name.ToString() + " not found");
      object[] args = new object[this._args.Count];
      if (xsltContextFunction.Maxargs != 0)
      {
        XPathResultType[] argTypes2 = xsltContextFunction.ArgTypes;
        for (int index = 0; index < this._args.Count; ++index)
        {
          XPathResultType type = argTypes2 != null ? (index >= argTypes2.Length ? argTypes2[argTypes2.Length - 1] : argTypes2[index]) : XPathResultType.Any;
          object obj = ((Expression) this._args[index]).EvaluateAs(iter, type);
          args[index] = obj;
        }
      }
      return xsltContextFunction.Invoke(namespaceManager, args, iter.Current);
    }

    internal override bool Peer => false;
  }
}
