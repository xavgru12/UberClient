// Decompiled with JetBrains decompiler
// Type: System.Xml.XmlCDataSection
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml
{
  public class XmlCDataSection : XmlCharacterData
  {
    protected internal XmlCDataSection(string data, XmlDocument doc)
      : base(data, doc)
    {
    }

    public override string LocalName => "#cdata-section";

    public override string Name => "#cdata-section";

    public override XmlNodeType NodeType => XmlNodeType.CDATA;

    public override XmlNode ParentNode => base.ParentNode;

    public override XmlNode CloneNode(bool deep) => (XmlNode) new XmlCDataSection(this.Data, this.OwnerDocument);

    public override void WriteContentTo(XmlWriter w)
    {
    }

    public override void WriteTo(XmlWriter w) => w.WriteCData(this.Data);
  }
}
