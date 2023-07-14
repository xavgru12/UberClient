// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.RelationalExpr
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;

namespace System.Xml.XPath
{
  internal abstract class RelationalExpr : ExprBoolean
  {
    public RelationalExpr(Expression left, Expression right)
      : base(left, right)
    {
    }

    public override bool StaticValueAsBoolean => this.HasStaticValue && this.Compare(this._left.StaticValueAsNumber, this._right.StaticValueAsNumber);

    public override bool EvaluateBoolean(BaseIterator iter)
    {
      XPathResultType xpathResultType1 = this._left.GetReturnType(iter);
      XPathResultType xpathResultType2 = this._right.GetReturnType(iter);
      if (xpathResultType1 == XPathResultType.Any)
        xpathResultType1 = Expression.GetReturnType(this._left.Evaluate(iter));
      if (xpathResultType2 == XPathResultType.Any)
        xpathResultType2 = Expression.GetReturnType(this._right.Evaluate(iter));
      if (xpathResultType1 == XPathResultType.Navigator)
        xpathResultType1 = XPathResultType.String;
      if (xpathResultType2 == XPathResultType.Navigator)
        xpathResultType2 = XPathResultType.String;
      if (xpathResultType1 != XPathResultType.NodeSet && xpathResultType2 != XPathResultType.NodeSet)
        return this.Compare(this._left.EvaluateNumber(iter), this._right.EvaluateNumber(iter));
      bool fReverse = false;
      Expression expression1;
      Expression expression2;
      if (xpathResultType1 != XPathResultType.NodeSet)
      {
        fReverse = true;
        expression1 = this._right;
        expression2 = this._left;
        xpathResultType2 = xpathResultType1;
      }
      else
      {
        expression1 = this._left;
        expression2 = this._right;
      }
      if (xpathResultType2 == XPathResultType.Boolean)
      {
        bool boolean1 = expression1.EvaluateBoolean(iter);
        bool boolean2 = expression2.EvaluateBoolean(iter);
        return this.Compare(Convert.ToDouble(boolean1), Convert.ToDouble(boolean2), fReverse);
      }
      BaseIterator nodeSet1 = expression1.EvaluateNodeSet(iter);
      if (xpathResultType2 == XPathResultType.Number || xpathResultType2 == XPathResultType.String)
      {
        double number = expression2.EvaluateNumber(iter);
        while (nodeSet1.MoveNext())
        {
          if (this.Compare(XPathFunctions.ToNumber(nodeSet1.Current.Value), number, fReverse))
            return true;
        }
      }
      else if (xpathResultType2 == XPathResultType.NodeSet)
      {
        BaseIterator nodeSet2 = expression2.EvaluateNodeSet(iter);
        ArrayList arrayList = new ArrayList();
        while (nodeSet1.MoveNext())
          arrayList.Add((object) XPathFunctions.ToNumber(nodeSet1.Current.Value));
        while (nodeSet2.MoveNext())
        {
          double number = XPathFunctions.ToNumber(nodeSet2.Current.Value);
          for (int index = 0; index < arrayList.Count; ++index)
          {
            if (this.Compare((double) arrayList[index], number))
              return true;
          }
        }
      }
      return false;
    }

    public abstract bool Compare(double arg1, double arg2);

    public bool Compare(double arg1, double arg2, bool fReverse) => fReverse ? this.Compare(arg2, arg1) : this.Compare(arg1, arg2);
  }
}
