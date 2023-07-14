// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.BaseIterator
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.XPath
{
  internal abstract class BaseIterator : XPathNodeIterator
  {
    private IXmlNamespaceResolver _nsm;
    private int position;

    internal BaseIterator(BaseIterator other)
    {
      this._nsm = other._nsm;
      this.position = other.position;
    }

    internal BaseIterator(IXmlNamespaceResolver nsm) => this._nsm = nsm;

    public IXmlNamespaceResolver NamespaceManager
    {
      get => this._nsm;
      set => this._nsm = value;
    }

    public virtual bool ReverseAxis => false;

    public int ComparablePosition
    {
      get
      {
        if (!this.ReverseAxis)
          return this.CurrentPosition;
        int num = this.Count - this.CurrentPosition + 1;
        return num < 1 ? 1 : num;
      }
    }

    public override int CurrentPosition => this.position;

    internal void SetPosition(int pos) => this.position = pos;

    public override bool MoveNext()
    {
      if (!this.MoveNextCore())
        return false;
      ++this.position;
      return true;
    }

    public abstract bool MoveNextCore();

    internal XPathNavigator PeekNext()
    {
      XPathNodeIterator xpathNodeIterator = this.Clone();
      return xpathNodeIterator.MoveNext() ? xpathNodeIterator.Current : (XPathNavigator) null;
    }

    public override string ToString() => this.Current != null ? this.Current.NodeType.ToString() + "[" + (object) this.CurrentPosition + "] : " + this.Current.Name + " = " + this.Current.Value : this.GetType().ToString() + "[" + (object) this.CurrentPosition + "]";
  }
}
