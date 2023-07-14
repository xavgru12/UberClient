// Decompiled with JetBrains decompiler
// Type: System.Xml.XmlLinkedNode
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml
{
  public abstract class XmlLinkedNode : XmlNode
  {
    private XmlLinkedNode nextSibling;

    internal XmlLinkedNode(XmlDocument doc)
      : base(doc)
    {
    }

    internal bool IsRooted
    {
      get
      {
        for (XmlNode parentNode = this.ParentNode; parentNode != null; parentNode = parentNode.ParentNode)
        {
          if (parentNode.NodeType == XmlNodeType.Document)
            return true;
        }
        return false;
      }
    }

    public override XmlNode NextSibling => this.ParentNode == null || this.ParentNode.LastChild == this ? (XmlNode) null : (XmlNode) this.nextSibling;

    internal XmlLinkedNode NextLinkedSibling
    {
      get => this.nextSibling;
      set => this.nextSibling = value;
    }

    public override XmlNode PreviousSibling
    {
      get
      {
        if (this.ParentNode != null)
        {
          XmlNode previousSibling = this.ParentNode.FirstChild;
          if (previousSibling != this)
          {
            while (previousSibling.NextSibling != this)
            {
              if ((previousSibling = previousSibling.NextSibling) == null)
                goto label_5;
            }
            return previousSibling;
          }
        }
label_5:
        return (XmlNode) null;
      }
    }
  }
}
