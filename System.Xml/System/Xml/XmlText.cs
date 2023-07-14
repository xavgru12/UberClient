// Decompiled with JetBrains decompiler
// Type: System.Xml.XmlText
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Xml.XPath;

namespace System.Xml
{
  public class XmlText : XmlCharacterData
  {
    protected internal XmlText(string strData, XmlDocument doc)
      : base(strData, doc)
    {
    }

    public override string LocalName => "#text";

    public override string Name => "#text";

    public override XmlNodeType NodeType => XmlNodeType.Text;

    internal override XPathNodeType XPathNodeType => XPathNodeType.Text;

    public override string Value
    {
      get => this.Data;
      set => this.Data = value;
    }

    public override XmlNode ParentNode => base.ParentNode;

    public override XmlNode CloneNode(bool deep) => (XmlNode) this.OwnerDocument.CreateTextNode(this.Data);

    public virtual XmlText SplitText(int offset)
    {
      XmlText textNode = this.OwnerDocument.CreateTextNode(this.Data.Substring(offset));
      this.DeleteData(offset, this.Data.Length - offset);
      this.ParentNode.InsertAfter((XmlNode) textNode, (XmlNode) this);
      return textNode;
    }

    public override void WriteContentTo(XmlWriter w)
    {
    }

    public override void WriteTo(XmlWriter w) => w.WriteString(this.Data);
  }
}
