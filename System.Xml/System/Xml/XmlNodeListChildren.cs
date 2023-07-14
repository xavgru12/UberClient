// Decompiled with JetBrains decompiler
// Type: System.Xml.XmlNodeListChildren
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;

namespace System.Xml
{
  internal class XmlNodeListChildren : XmlNodeList
  {
    private IHasXmlChildNode parent;

    public XmlNodeListChildren(IHasXmlChildNode parent) => this.parent = parent;

    public override int Count
    {
      get
      {
        int count = 0;
        if (this.parent.LastLinkedChild != null)
        {
          XmlLinkedNode nextLinkedSibling = this.parent.LastLinkedChild.NextLinkedSibling;
          count = 1;
          while (!object.ReferenceEquals((object) nextLinkedSibling, (object) this.parent.LastLinkedChild))
          {
            nextLinkedSibling = nextLinkedSibling.NextLinkedSibling;
            ++count;
          }
        }
        return count;
      }
    }

    public override IEnumerator GetEnumerator() => (IEnumerator) new XmlNodeListChildren.Enumerator(this.parent);

    public override XmlNode Item(int index)
    {
      XmlNode xmlNode = (XmlNode) null;
      if (this.Count <= index)
        return (XmlNode) null;
      if (index >= 0 && this.parent.LastLinkedChild != null)
      {
        XmlLinkedNode nextLinkedSibling = this.parent.LastLinkedChild.NextLinkedSibling;
        int num;
        for (num = 0; num < index && !object.ReferenceEquals((object) nextLinkedSibling, (object) this.parent.LastLinkedChild); ++num)
          nextLinkedSibling = nextLinkedSibling.NextLinkedSibling;
        if (num == index)
          xmlNode = (XmlNode) nextLinkedSibling;
      }
      return xmlNode;
    }

    private class Enumerator : IEnumerator
    {
      private IHasXmlChildNode parent;
      private XmlLinkedNode currentChild;
      private bool passedLastNode;

      internal Enumerator(IHasXmlChildNode parent)
      {
        this.currentChild = (XmlLinkedNode) null;
        this.parent = parent;
        this.passedLastNode = false;
      }

      public virtual object Current
      {
        get
        {
          if (this.currentChild == null || this.parent.LastLinkedChild == null || this.passedLastNode)
            throw new InvalidOperationException();
          return (object) this.currentChild;
        }
      }

      public virtual bool MoveNext()
      {
        bool flag = true;
        if (this.parent.LastLinkedChild == null)
          flag = false;
        else if (this.currentChild == null)
          this.currentChild = this.parent.LastLinkedChild.NextLinkedSibling;
        else if (object.ReferenceEquals((object) this.currentChild, (object) this.parent.LastLinkedChild))
        {
          flag = false;
          this.passedLastNode = true;
        }
        else
          this.currentChild = this.currentChild.NextLinkedSibling;
        return flag;
      }

      public virtual void Reset() => this.currentChild = (XmlLinkedNode) null;
    }
  }
}
