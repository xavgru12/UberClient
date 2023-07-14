// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.CompiledExpression
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.Xml.Xsl;

namespace System.Xml.XPath
{
  internal class CompiledExpression : XPathExpression
  {
    protected IXmlNamespaceResolver _nsm;
    protected System.Xml.XPath.Expression _expr;
    private XPathSorters _sorters;
    private string rawExpression;

    public CompiledExpression(string raw, System.Xml.XPath.Expression expr)
    {
      this._expr = expr.Optimize();
      this.rawExpression = raw;
    }

    private CompiledExpression(CompiledExpression other)
    {
      this._nsm = other._nsm;
      this._expr = other._expr;
      this.rawExpression = other.rawExpression;
    }

    public override XPathExpression Clone() => (XPathExpression) new CompiledExpression(this);

    public System.Xml.XPath.Expression ExpressionNode => this._expr;

    public override void SetContext(XmlNamespaceManager nsManager) => this._nsm = (IXmlNamespaceResolver) nsManager;

    public override void SetContext(IXmlNamespaceResolver nsResolver) => this._nsm = nsResolver;

    internal IXmlNamespaceResolver NamespaceManager => this._nsm;

    public override string Expression => this.rawExpression;

    public override XPathResultType ReturnType => this._expr.ReturnType;

    public object Evaluate(BaseIterator iter)
    {
      if (this._sorters != null)
        return (object) this.EvaluateNodeSet(iter);
      try
      {
        return this._expr.Evaluate(iter);
      }
      catch (XPathException ex)
      {
        throw;
      }
      catch (XsltException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        throw new XPathException("Error during evaluation", ex);
      }
    }

    public XPathNodeIterator EvaluateNodeSet(BaseIterator iter)
    {
      BaseIterator nodeSet = this._expr.EvaluateNodeSet(iter);
      return this._sorters != null ? (XPathNodeIterator) this._sorters.Sort(nodeSet) : (XPathNodeIterator) nodeSet;
    }

    public double EvaluateNumber(BaseIterator iter) => this._expr.EvaluateNumber(iter);

    public string EvaluateString(BaseIterator iter) => this._expr.EvaluateString(iter);

    public bool EvaluateBoolean(BaseIterator iter) => this._expr.EvaluateBoolean(iter);

    public override void AddSort(object obj, IComparer cmp)
    {
      if (this._sorters == null)
        this._sorters = new XPathSorters();
      this._sorters.Add(obj, cmp);
    }

    public override void AddSort(
      object expr,
      XmlSortOrder orderSort,
      XmlCaseOrder orderCase,
      string lang,
      XmlDataType dataType)
    {
      if (this._sorters == null)
        this._sorters = new XPathSorters();
      this._sorters.Add(expr, orderSort, orderCase, lang, dataType);
    }
  }
}
