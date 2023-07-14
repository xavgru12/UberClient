// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.KeyIndexTable
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.Xsl
{
  internal class KeyIndexTable
  {
    private XsltCompiledContext ctx;
    private ArrayList keys;
    private Hashtable mappedDocuments;

    public KeyIndexTable(XsltCompiledContext ctx, ArrayList keys)
    {
      this.ctx = ctx;
      this.keys = keys;
    }

    public ArrayList Keys => this.keys;

    private void CollectTable(XPathNavigator doc, XsltContext ctx, Hashtable map)
    {
      for (int index = 0; index < this.keys.Count; ++index)
        this.CollectTable(doc, ctx, map, (XslKey) this.keys[index]);
    }

    private void CollectTable(XPathNavigator doc, XsltContext ctx, Hashtable map, XslKey key)
    {
      XPathNavigator xpathNavigator = doc.Clone();
      xpathNavigator.MoveToRoot();
      XPathNavigator target = doc.Clone();
      bool matchesAttributes = false;
      switch (key.Match.EvaluatedNodeType)
      {
        case XPathNodeType.Attribute:
        case XPathNodeType.All:
          matchesAttributes = true;
          break;
      }
      do
      {
        if (key.Match.Matches(xpathNavigator, ctx))
        {
          target.MoveTo(xpathNavigator);
          this.CollectIndex(xpathNavigator, target, map);
        }
      }
      while (this.MoveNavigatorToNext(xpathNavigator, matchesAttributes));
      if (map == null)
        return;
      foreach (ArrayList arrayList in (IEnumerable) map.Values)
        arrayList.Sort((IComparer) XPathNavigatorComparer.Instance);
    }

    private bool MoveNavigatorToNext(XPathNavigator nav, bool matchesAttributes)
    {
      if (matchesAttributes)
      {
        if (nav.NodeType != XPathNodeType.Attribute && nav.MoveToFirstAttribute())
          return true;
        if (nav.NodeType == XPathNodeType.Attribute)
        {
          if (nav.MoveToNextAttribute())
            return true;
          nav.MoveToParent();
        }
      }
      if (nav.MoveToFirstChild())
        return true;
      while (!nav.MoveToNext())
      {
        if (!nav.MoveToParent())
          return false;
      }
      return true;
    }

    private void CollectIndex(XPathNavigator nav, XPathNavigator target, Hashtable map)
    {
      for (int index = 0; index < this.keys.Count; ++index)
        this.CollectIndex(nav, target, map, (XslKey) this.keys[index]);
    }

    private void CollectIndex(
      XPathNavigator nav,
      XPathNavigator target,
      Hashtable map,
      XslKey key)
    {
      switch (key.Use.ReturnType)
      {
        case XPathResultType.NodeSet:
          XPathNodeIterator xpathNodeIterator1 = nav.Select((XPathExpression) key.Use);
          while (xpathNodeIterator1.MoveNext())
            this.AddIndex(xpathNodeIterator1.Current.Value, target, map);
          break;
        case XPathResultType.Any:
          object obj = nav.Evaluate((XPathExpression) key.Use);
          if (obj is XPathNodeIterator xpathNodeIterator2)
          {
            while (xpathNodeIterator2.MoveNext())
              this.AddIndex(xpathNodeIterator2.Current.Value, target, map);
            break;
          }
          this.AddIndex(XPathFunctions.ToString(obj), target, map);
          break;
        default:
          this.AddIndex(nav.EvaluateString((XPathExpression) key.Use, (XPathNodeIterator) null, (IXmlNamespaceResolver) null), target, map);
          break;
      }
    }

    private void AddIndex(string key, XPathNavigator target, Hashtable map)
    {
      if (!(map[(object) key] is ArrayList arrayList))
      {
        arrayList = new ArrayList();
        map[(object) key] = (object) arrayList;
      }
      for (int index = 0; index < arrayList.Count; ++index)
      {
        if (((XPathNavigator) arrayList[index]).IsSamePosition(target))
          return;
      }
      arrayList.Add((object) target.Clone());
    }

    private ArrayList GetNodesByValue(XPathNavigator nav, string value, XsltContext ctx)
    {
      if (this.mappedDocuments == null)
        this.mappedDocuments = new Hashtable();
      Hashtable map = (Hashtable) this.mappedDocuments[(object) nav.BaseURI];
      if (map == null)
      {
        map = new Hashtable();
        this.mappedDocuments.Add((object) nav.BaseURI, (object) map);
        this.CollectTable(nav, ctx, map);
      }
      return map[(object) value] as ArrayList;
    }

    public bool Matches(XPathNavigator nav, string value, XsltContext ctx)
    {
      ArrayList nodesByValue = this.GetNodesByValue(nav, value, ctx);
      if (nodesByValue == null)
        return false;
      for (int index = 0; index < nodesByValue.Count; ++index)
      {
        if (((XPathNavigator) nodesByValue[index]).IsSamePosition(nav))
          return true;
      }
      return false;
    }

    public BaseIterator Evaluate(BaseIterator iter, Expression valueExpr)
    {
      XPathNodeIterator xpathNodeIterator1 = (XPathNodeIterator) iter;
      if (iter.CurrentPosition == 0)
      {
        xpathNodeIterator1 = iter.Clone();
        xpathNodeIterator1.MoveNext();
      }
      XPathNavigator current = xpathNodeIterator1.Current;
      object obj = valueExpr.Evaluate(iter);
      XPathNodeIterator xpathNodeIterator2 = obj as XPathNodeIterator;
      XsltContext namespaceManager = iter.NamespaceManager as XsltContext;
      BaseIterator left = (BaseIterator) null;
      if (xpathNodeIterator2 != null)
      {
        while (xpathNodeIterator2.MoveNext())
        {
          ArrayList nodesByValue = this.GetNodesByValue(current, xpathNodeIterator2.Current.Value, namespaceManager);
          if (nodesByValue != null)
          {
            ListIterator right = new ListIterator((IList) nodesByValue, (IXmlNamespaceResolver) namespaceManager);
            left = left != null ? (BaseIterator) new UnionIterator(iter, left, (BaseIterator) right) : (BaseIterator) right;
          }
        }
      }
      else if (current != null)
      {
        ArrayList nodesByValue = this.GetNodesByValue(current, XPathFunctions.ToString(obj), namespaceManager);
        if (nodesByValue != null)
          left = (BaseIterator) new ListIterator((IList) nodesByValue, (IXmlNamespaceResolver) namespaceManager);
      }
      return left ?? (BaseIterator) new NullIterator(iter);
    }
  }
}
