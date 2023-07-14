// Decompiled with JetBrains decompiler
// Type: System.Xml.XmlNotation
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml
{
  public class XmlNotation : XmlNode
  {
    private string localName;
    private string publicId;
    private string systemId;
    private string prefix;

    internal XmlNotation(
      string localName,
      string prefix,
      string publicId,
      string systemId,
      XmlDocument doc)
      : base(doc)
    {
      this.localName = doc.NameTable.Add(localName);
      this.prefix = doc.NameTable.Add(prefix);
      this.publicId = publicId;
      this.systemId = systemId;
    }

    public override string InnerXml
    {
      get => string.Empty;
      set => throw new InvalidOperationException("This operation is not allowed.");
    }

    public override bool IsReadOnly => true;

    public override string LocalName => this.localName;

    public override string Name => this.prefix != string.Empty ? this.prefix + ":" + this.localName : this.localName;

    public override XmlNodeType NodeType => XmlNodeType.Notation;

    public override string OuterXml => string.Empty;

    public string PublicId => this.publicId != null ? this.publicId : (string) null;

    public string SystemId => this.systemId != null ? this.systemId : (string) null;

    public override XmlNode CloneNode(bool deep) => throw new InvalidOperationException("This operation is not allowed.");

    public override void WriteContentTo(XmlWriter w)
    {
    }

    public override void WriteTo(XmlWriter w)
    {
    }
  }
}
