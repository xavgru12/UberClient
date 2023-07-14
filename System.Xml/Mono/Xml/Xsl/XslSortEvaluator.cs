// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.XslSortEvaluator
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.Xml.XPath;

namespace Mono.Xml.Xsl
{
  internal class XslSortEvaluator
  {
    private XPathExpression select;
    private Sort[] sorterTemplates;
    private XPathSorter[] sorters;
    private XPathSorters sortRunner;
    private bool isSorterContextDependent;

    public XslSortEvaluator(XPathExpression select, Sort[] sorterTemplates)
    {
      this.select = select;
      this.sorterTemplates = sorterTemplates;
      this.PopulateConstantSorters();
      this.sortRunner = new XPathSorters();
    }

    private void PopulateConstantSorters()
    {
      this.sorters = new XPathSorter[this.sorterTemplates.Length];
      for (int index = 0; index < this.sorterTemplates.Length; ++index)
      {
        Sort sorterTemplate = this.sorterTemplates[index];
        if (sorterTemplate.IsContextDependent)
          this.isSorterContextDependent = true;
        else
          this.sorters[index] = sorterTemplate.ToXPathSorter((XslTransformProcessor) null);
      }
    }

    public BaseIterator SortedSelect(XslTransformProcessor p)
    {
      if (this.isSorterContextDependent)
      {
        for (int index = 0; index < this.sorters.Length; ++index)
        {
          if (this.sorterTemplates[index].IsContextDependent)
            this.sorters[index] = this.sorterTemplates[index].ToXPathSorter(p);
        }
      }
      BaseIterator baseIterator = (BaseIterator) p.Select(this.select);
      p.PushNodeset((XPathNodeIterator) baseIterator);
      p.PushForEachContext();
      ArrayList rgElts = new ArrayList(baseIterator.Count);
      while (baseIterator.MoveNext())
      {
        XPathSortElement xpathSortElement = new XPathSortElement();
        xpathSortElement.Navigator = baseIterator.Current.Clone();
        xpathSortElement.Values = new object[this.sorters.Length];
        for (int index = 0; index < this.sorters.Length; ++index)
          xpathSortElement.Values[index] = this.sorters[index].Evaluate(baseIterator);
        rgElts.Add((object) xpathSortElement);
      }
      p.PopForEachContext();
      p.PopNodeset();
      this.sortRunner.CopyFrom(this.sorters);
      return this.sortRunner.Sort(rgElts, baseIterator.NamespaceManager);
    }
  }
}
