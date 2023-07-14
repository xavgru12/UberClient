// Decompiled with JetBrains decompiler
// Type: System.Xml.XmlWhitespace
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Xml.XPath;

namespace System.Xml
{
  public class XmlWhitespace : XmlCharacterData
  {
    protected internal XmlWhitespace(string strData, XmlDocument doc)
      : base(strData, doc)
    {
    }

    public override string LocalName => "#whitespace";

    public override string Name => "#whitespace";

    public override XmlNodeType NodeType => XmlNodeType.Whitespace;

    internal override XPathNodeType XPathNodeType => XPathNodeType.Whitespace;

    public override string Value
    {
      get => this.Data;
      set => this.Data = XmlChar.IsWhitespace(value) ? value : throw new ArgumentException("Invalid whitespace characters.");
    }

    public override XmlNode ParentNode => base.ParentNode;

    public override XmlNode CloneNode(bool deep) => (XmlNode) new XmlWhitespace(this.Data, this.OwnerDocument);

    public override void WriteContentTo(XmlWriter w)
    {
    }

    public override void WriteTo(XmlWriter w) => w.WriteWhitespace(this.Data);
  }
}
