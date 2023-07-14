// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.EqualityExpr
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;

namespace System.Xml.XPath
{
  internal abstract class EqualityExpr : ExprBoolean
  {
    private bool trueVal;

    public EqualityExpr(Expression left, Expression right, bool trueVal)
      : base(left, right)
    {
      this.trueVal = trueVal;
    }

    public override bool StaticValueAsBoolean
    {
      get
      {
        if (!this.HasStaticValue)
          return false;
        if ((this._left.ReturnType == XPathResultType.Navigator || this._right.ReturnType == XPathResultType.Navigator) && this._left.ReturnType == this._right.ReturnType)
          return this._left.StaticValueAsNavigator.IsSamePosition(this._right.StaticValueAsNavigator) == this.trueVal;
        if (this._left.ReturnType == XPathResultType.Boolean | this._right.ReturnType == XPathResultType.Boolean)
          return this._left.StaticValueAsBoolean == this._right.StaticValueAsBoolean == this.trueVal;
        if (this._left.ReturnType == XPathResultType.Number | this._right.ReturnType == XPathResultType.Number)
          return this._left.StaticValueAsNumber == this._right.StaticValueAsNumber == this.trueVal;
        return this._left.ReturnType == XPathResultType.String | this._right.ReturnType == XPathResultType.String ? this._left.StaticValueAsString == this._right.StaticValueAsString == this.trueVal : this._left.StaticValue == this._right.StaticValue == this.trueVal;
      }
    }

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
      if (xpathResultType1 == XPathResultType.NodeSet || xpathResultType2 == XPathResultType.NodeSet)
      {
        Expression expression1;
        Expression expression2;
        if (xpathResultType1 != XPathResultType.NodeSet)
        {
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
          return expression1.EvaluateBoolean(iter) == expression2.EvaluateBoolean(iter) == this.trueVal;
        BaseIterator nodeSet1 = expression1.EvaluateNodeSet(iter);
        if (xpathResultType2 == XPathResultType.Number)
        {
          double number = expression2.EvaluateNumber(iter);
          while (nodeSet1.MoveNext())
          {
            if (XPathFunctions.ToNumber(nodeSet1.Current.Value) == number == this.trueVal)
              return true;
          }
        }
        else if (xpathResultType2 == XPathResultType.String)
        {
          string str = expression2.EvaluateString(iter);
          while (nodeSet1.MoveNext())
          {
            if (nodeSet1.Current.Value == str == this.trueVal)
              return true;
          }
        }
        else if (xpathResultType2 == XPathResultType.NodeSet)
        {
          BaseIterator nodeSet2 = expression2.EvaluateNodeSet(iter);
          ArrayList arrayList = new ArrayList();
          while (nodeSet1.MoveNext())
            arrayList.Add((object) XPathFunctions.ToString((object) nodeSet1.Current.Value));
          while (nodeSet2.MoveNext())
          {
            string str = XPathFunctions.ToString((object) nodeSet2.Current.Value);
            for (int index = 0; index < arrayList.Count; ++index)
            {
              if (str == (string) arrayList[index] == this.trueVal)
                return true;
            }
          }
        }
        return false;
      }
      if (xpathResultType1 == XPathResultType.Boolean || xpathResultType2 == XPathResultType.Boolean)
        return this._left.EvaluateBoolean(iter) == this._right.EvaluateBoolean(iter) == this.trueVal;
      return xpathResultType1 == XPathResultType.Number || xpathResultType2 == XPathResultType.Number ? this._left.EvaluateNumber(iter) == this._right.EvaluateNumber(iter) == this.trueVal : this._left.EvaluateString(iter) == this._right.EvaluateString(iter) == this.trueVal;
    }
  }
}
