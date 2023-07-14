// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.XPathNavigatorComparer
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;

namespace System.Xml.XPath
{
  internal class XPathNavigatorComparer : IComparer, IEqualityComparer
  {
    public static XPathNavigatorComparer Instance = new XPathNavigatorComparer();

    private XPathNavigatorComparer()
    {
    }

    bool IEqualityComparer.Equals(object o1, object o2)
    {
      XPathNavigator xpathNavigator = o1 as XPathNavigator;
      XPathNavigator other = o2 as XPathNavigator;
      return xpathNavigator != null && other != null && xpathNavigator.IsSamePosition(other);
    }

    int IEqualityComparer.GetHashCode(object obj) => obj.GetHashCode();

    public int Compare(object o1, object o2)
    {
      XPathNavigator xpathNavigator = o1 as XPathNavigator;
      XPathNavigator nav = o2 as XPathNavigator;
      if (xpathNavigator == null)
        return -1;
      if (nav == null)
        return 1;
      switch (xpathNavigator.ComparePosition(nav))
      {
        case XmlNodeOrder.After:
          return 1;
        case XmlNodeOrder.Same:
          return 0;
        default:
          return -1;
      }
    }
  }
}
