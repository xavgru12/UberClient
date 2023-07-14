// Decompiled with JetBrains decompiler
// Type: System.Xml.XPath.XPathSorters
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;

namespace System.Xml.XPath
{
  internal class XPathSorters : IComparer
  {
    private readonly ArrayList _rgSorters = new ArrayList();

    int IComparer.Compare(object o1, object o2)
    {
      XPathSortElement xpathSortElement1 = (XPathSortElement) o1;
      XPathSortElement xpathSortElement2 = (XPathSortElement) o2;
      for (int index = 0; index < this._rgSorters.Count; ++index)
      {
        int num = ((XPathSorter) this._rgSorters[index]).Compare(xpathSortElement1.Values[index], xpathSortElement2.Values[index]);
        if (num != 0)
          return num;
      }
      switch (xpathSortElement1.Navigator.ComparePosition(xpathSortElement2.Navigator))
      {
        case XmlNodeOrder.After:
          return 1;
        case XmlNodeOrder.Same:
          return 0;
        default:
          return -1;
      }
    }

    public void Add(object expr, IComparer cmp) => this._rgSorters.Add((object) new XPathSorter(expr, cmp));

    public void Add(
      object expr,
      XmlSortOrder orderSort,
      XmlCaseOrder orderCase,
      string lang,
      XmlDataType dataType)
    {
      this._rgSorters.Add((object) new XPathSorter(expr, orderSort, orderCase, lang, dataType));
    }

    public void CopyFrom(XPathSorter[] sorters)
    {
      this._rgSorters.Clear();
      this._rgSorters.AddRange((ICollection) sorters);
    }

    public BaseIterator Sort(BaseIterator iter) => this.Sort(this.ToSortElementList(iter), iter.NamespaceManager);

    private ArrayList ToSortElementList(BaseIterator iter)
    {
      ArrayList sortElementList = new ArrayList();
      int count = this._rgSorters.Count;
      while (iter.MoveNext())
      {
        XPathSortElement xpathSortElement = new XPathSortElement();
        xpathSortElement.Navigator = iter.Current.Clone();
        xpathSortElement.Values = new object[count];
        for (int index = 0; index < this._rgSorters.Count; ++index)
        {
          XPathSorter rgSorter = (XPathSorter) this._rgSorters[index];
          xpathSortElement.Values[index] = rgSorter.Evaluate(iter);
        }
        sortElementList.Add((object) xpathSortElement);
      }
      return sortElementList;
    }

    public BaseIterator Sort(ArrayList rgElts, IXmlNamespaceResolver nsm)
    {
      rgElts.Sort((IComparer) this);
      XPathNavigator[] xpathNavigatorArray = new XPathNavigator[rgElts.Count];
      for (int index = 0; index < rgElts.Count; ++index)
      {
        XPathSortElement rgElt = (XPathSortElement) rgElts[index];
        xpathNavigatorArray[index] = rgElt.Navigator;
      }
      return (BaseIterator) new ListIterator((IList) xpathNavigatorArray, nsm);
    }
  }
}
