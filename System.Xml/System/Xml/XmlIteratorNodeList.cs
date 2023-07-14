// Decompiled with JetBrains decompiler
// Type: System.Xml.XmlIteratorNodeList
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.Xml.XPath;

namespace System.Xml
{
  internal class XmlIteratorNodeList : XmlNodeList
  {
    private XPathNodeIterator source;
    private XPathNodeIterator iterator;
    private ArrayList list;
    private bool finished;

    public XmlIteratorNodeList(XPathNodeIterator iter)
    {
      this.source = iter;
      this.iterator = iter.Clone();
      this.list = new ArrayList();
    }

    public override int Count => this.iterator.Count;

    public override IEnumerator GetEnumerator() => this.finished ? this.list.GetEnumerator() : (IEnumerator) new XmlIteratorNodeList.XPathNodeIteratorNodeListIterator(this.source);

    public override XmlNode Item(int index)
    {
      if (index < 0)
        return (XmlNode) null;
      if (index < this.list.Count)
        return (XmlNode) this.list[index];
      ++index;
      while (this.iterator.CurrentPosition < index)
      {
        if (!this.iterator.MoveNext())
        {
          this.finished = true;
          return (XmlNode) null;
        }
        this.list.Add((object) ((IHasXmlNode) this.iterator.Current).GetNode());
      }
      return (XmlNode) this.list[index - 1];
    }

    private class XPathNodeIteratorNodeListIterator : IEnumerator
    {
      private XPathNodeIterator iter;
      private XPathNodeIterator source;

      public XPathNodeIteratorNodeListIterator(XPathNodeIterator source)
      {
        this.source = source;
        this.Reset();
      }

      public bool MoveNext() => this.iter.MoveNext();

      public object Current => (object) ((IHasXmlNode) this.iter.Current).GetNode();

      public void Reset() => this.iter = this.source.Clone();
    }
  }
}
