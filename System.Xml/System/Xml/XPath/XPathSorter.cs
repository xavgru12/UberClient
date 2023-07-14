// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.XPathSorter
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using Mono.Xml.XPath;
using System.Collections;
using System.Globalization;

namespace System.Xml.XPath
{
  internal class XPathSorter
  {
    private readonly Expression _expr;
    private readonly IComparer _cmp;
    private readonly XmlDataType _type;

    public XPathSorter(object expr, IComparer cmp)
    {
      this._expr = XPathSorter.ExpressionFromObject(expr);
      this._cmp = cmp;
      this._type = XmlDataType.Text;
    }

    public XPathSorter(
      object expr,
      XmlSortOrder orderSort,
      XmlCaseOrder orderCase,
      string lang,
      XmlDataType dataType)
    {
      this._expr = XPathSorter.ExpressionFromObject(expr);
      this._type = dataType;
      if (dataType == XmlDataType.Number)
        this._cmp = (IComparer) new XPathSorter.XPathNumberComparer(orderSort);
      else
        this._cmp = (IComparer) new XPathSorter.XPathTextComparer(orderSort, orderCase, lang);
    }

    private static Expression ExpressionFromObject(object expr)
    {
      switch (expr)
      {
        case CompiledExpression _:
          return ((CompiledExpression) expr).ExpressionNode;
        case string _:
          return new XPathParser().Compile((string) expr);
        default:
          throw new XPathException("Invalid query object");
      }
    }

    public object Evaluate(BaseIterator iter) => this._type == XmlDataType.Number ? (object) this._expr.EvaluateNumber(iter) : (object) this._expr.EvaluateString(iter);

    public int Compare(object o1, object o2) => this._cmp.Compare(o1, o2);

    private class XPathNumberComparer : IComparer
    {
      private int _nMulSort;

      public XPathNumberComparer(XmlSortOrder orderSort) => this._nMulSort = orderSort != XmlSortOrder.Ascending ? -1 : 1;

      int IComparer.Compare(object o1, object o2)
      {
        double d1 = (double) o1;
        double d2 = (double) o2;
        if (d1 < d2)
          return -this._nMulSort;
        if (d1 > d2)
          return this._nMulSort;
        if (d1 == d2)
          return 0;
        if (!double.IsNaN(d1))
          return this._nMulSort;
        return double.IsNaN(d2) ? 0 : -this._nMulSort;
      }
    }

    private class XPathTextComparer : IComparer
    {
      private int _nMulSort;
      private int _nMulCase;
      private XmlCaseOrder _orderCase;
      private CultureInfo _ci;

      public XPathTextComparer(XmlSortOrder orderSort, XmlCaseOrder orderCase, string strLang)
      {
        this._orderCase = orderCase;
        this._nMulCase = orderCase != XmlCaseOrder.UpperFirst ? 1 : -1;
        this._nMulSort = orderSort != XmlSortOrder.Ascending ? -1 : 1;
        if (strLang == null || strLang == string.Empty)
          this._ci = CultureInfo.CurrentCulture;
        else
          this._ci = new CultureInfo(strLang);
      }

      int IComparer.Compare(object o1, object o2)
      {
        string strA = (string) o1;
        string strB = (string) o2;
        int num = string.Compare(strA, strB, true, this._ci);
        return num != 0 || this._orderCase == XmlCaseOrder.None ? num * this._nMulSort : this._nMulSort * this._nMulCase * string.Compare(strA, strB, false, this._ci);
      }
    }
  }
}
