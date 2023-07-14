// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.ExprRoot
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.XPath
{
  internal class ExprRoot : NodeSet
  {
    public override string ToString() => string.Empty;

    public override object Evaluate(BaseIterator iter)
    {
      if (iter.CurrentPosition == 0)
      {
        iter = (BaseIterator) iter.Clone();
        iter.MoveNext();
      }
      XPathNavigator nav = iter.Current.Clone();
      nav.MoveToRoot();
      return (object) new SelfIterator(nav, iter.NamespaceManager);
    }

    internal override XPathNodeType EvaluatedNodeType => XPathNodeType.Root;

    internal override bool Peer => true;

    internal override bool Subtree => false;
  }
}
