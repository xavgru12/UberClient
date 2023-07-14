// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.XPathExpression
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using Mono.Xml.XPath;
using System.Collections;
using System.Xml.Xsl;

namespace System.Xml.XPath
{
  public abstract class XPathExpression
  {
    internal XPathExpression()
    {
    }

    public abstract string Expression { get; }

    public abstract XPathResultType ReturnType { get; }

    public abstract void AddSort(object expr, IComparer comparer);

    public abstract void AddSort(
      object expr,
      XmlSortOrder order,
      XmlCaseOrder caseOrder,
      string lang,
      XmlDataType dataType);

    public abstract XPathExpression Clone();

    public abstract void SetContext(XmlNamespaceManager nsManager);

    public static XPathExpression Compile(string xpath) => XPathExpression.Compile(xpath, (IXmlNamespaceResolver) null, (IStaticXsltContext) null);

    public static XPathExpression Compile(string xpath, IXmlNamespaceResolver nsmgr) => XPathExpression.Compile(xpath, nsmgr, (IStaticXsltContext) null);

    internal static XPathExpression Compile(
      string xpath,
      IXmlNamespaceResolver nsmgr,
      IStaticXsltContext ctx)
    {
      XPathParser xpathParser = new XPathParser(ctx);
      CompiledExpression compiledExpression = new CompiledExpression(xpath, xpathParser.Compile(xpath));
      compiledExpression.SetContext(nsmgr);
      return (XPathExpression) compiledExpression;
    }

    public abstract void SetContext(IXmlNamespaceResolver nsResolver);
  }
}
