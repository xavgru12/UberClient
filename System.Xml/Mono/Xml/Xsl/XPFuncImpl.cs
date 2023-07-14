// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.XPFuncImpl
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.Xsl
{
  internal abstract class XPFuncImpl : IXsltContextFunction
  {
    private int minargs;
    private int maxargs;
    private XPathResultType returnType;
    private XPathResultType[] argTypes;

    public XPFuncImpl()
    {
    }

    public XPFuncImpl(
      int minArgs,
      int maxArgs,
      XPathResultType returnType,
      XPathResultType[] argTypes)
    {
      this.Init(minArgs, maxArgs, returnType, argTypes);
    }

    protected void Init(
      int minArgs,
      int maxArgs,
      XPathResultType returnType,
      XPathResultType[] argTypes)
    {
      this.minargs = minArgs;
      this.maxargs = maxArgs;
      this.returnType = returnType;
      this.argTypes = argTypes;
    }

    public int Minargs => this.minargs;

    public int Maxargs => this.maxargs;

    public XPathResultType ReturnType => this.returnType;

    public XPathResultType[] ArgTypes => this.argTypes;

    public object Invoke(XsltContext xsltContext, object[] args, XPathNavigator docContext) => this.Invoke((XsltCompiledContext) xsltContext, args, docContext);

    public abstract object Invoke(
      XsltCompiledContext xsltContext,
      object[] args,
      XPathNavigator docContext);

    public static XPathResultType GetXPathType(Type type, XPathNavigator node)
    {
      TypeCode typeCode = Type.GetTypeCode(type);
      switch (typeCode)
      {
        case TypeCode.Object:
          if (typeof (XPathNavigator).IsAssignableFrom(type) || typeof (IXPathNavigable).IsAssignableFrom(type))
            return XPathResultType.Navigator;
          return typeof (XPathNodeIterator).IsAssignableFrom(type) ? XPathResultType.NodeSet : XPathResultType.Any;
        case TypeCode.Boolean:
          return XPathResultType.Boolean;
        default:
          switch (typeCode - 16)
          {
            case TypeCode.Empty:
              throw new XsltException("Invalid type DateTime was specified.", (Exception) null, node);
            case TypeCode.DBNull:
              return XPathResultType.String;
            default:
              return XPathResultType.Number;
          }
      }
    }
  }
}
