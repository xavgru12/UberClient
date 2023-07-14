// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.PredicateIterator
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.XPath
{
  internal class PredicateIterator : BaseIterator
  {
    private BaseIterator _iter;
    private Expression _pred;
    private XPathResultType resType;
    private bool finished;

    public PredicateIterator(BaseIterator iter, Expression pred)
      : base(iter.NamespaceManager)
    {
      this._iter = iter;
      this._pred = pred;
      this.resType = pred.GetReturnType(iter);
    }

    private PredicateIterator(PredicateIterator other)
      : base((BaseIterator) other)
    {
      this._iter = (BaseIterator) other._iter.Clone();
      this._pred = other._pred;
      this.resType = other.resType;
      this.finished = other.finished;
    }

    public override XPathNodeIterator Clone() => (XPathNodeIterator) new PredicateIterator(this);

    public override bool MoveNextCore()
    {
      if (this.finished)
        return false;
      while (this._iter.MoveNext())
      {
        switch (this.resType)
        {
          case XPathResultType.Number:
            if (this._pred.EvaluateNumber(this._iter) == (double) this._iter.ComparablePosition)
              break;
            continue;
          case XPathResultType.Any:
            object obj = this._pred.Evaluate(this._iter);
            if (obj is double num)
            {
              if (num == (double) this._iter.ComparablePosition)
                break;
              continue;
            }
            if (XPathFunctions.ToBoolean(obj))
              break;
            continue;
          default:
            if (this._pred.EvaluateBoolean(this._iter))
              break;
            continue;
        }
        return true;
      }
      this.finished = true;
      return false;
    }

    public override XPathNavigator Current => this.CurrentPosition == 0 ? (XPathNavigator) null : this._iter.Current;

    public override bool ReverseAxis => this._iter.ReverseAxis;

    public override string ToString() => this._iter.GetType().FullName;
  }
}
