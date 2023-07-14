// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.XPathIteratorComparer
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;

namespace System.Xml.XPath
{
  internal class XPathIteratorComparer : IComparer
  {
    public static XPathIteratorComparer Instance = new XPathIteratorComparer();

    private XPathIteratorComparer()
    {
    }

    public int Compare(object o1, object o2)
    {
      XPathNodeIterator xpathNodeIterator1 = o1 as XPathNodeIterator;
      XPathNodeIterator xpathNodeIterator2 = o2 as XPathNodeIterator;
      if (xpathNodeIterator1 == null)
        return -1;
      if (xpathNodeIterator2 == null)
        return 1;
      switch (xpathNodeIterator1.Current.ComparePosition(xpathNodeIterator2.Current))
      {
        case XmlNodeOrder.After:
          return -1;
        case XmlNodeOrder.Same:
          return 0;
        default:
          return 1;
      }
    }
  }
}
