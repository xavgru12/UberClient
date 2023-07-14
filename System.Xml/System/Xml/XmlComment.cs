// Decompiled with JetBrains decompiler
// Type: System.Xml.XmlComment
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Xml.XPath;

namespace System.Xml
{
  public class XmlComment : XmlCharacterData
  {
    protected internal XmlComment(string comment, XmlDocument doc)
      : base(comment, doc)
    {
    }

    public override string LocalName => "#comment";

    public override string Name => "#comment";

    public override XmlNodeType NodeType => XmlNodeType.Comment;

    internal override XPathNodeType XPathNodeType => XPathNodeType.Comment;

    public override XmlNode CloneNode(bool deep) => (XmlNode) new XmlComment(this.Value, this.OwnerDocument);

    public override void WriteContentTo(XmlWriter w)
    {
    }

    public override void WriteTo(XmlWriter w) => w.WriteComment(this.Data);
  }
}
