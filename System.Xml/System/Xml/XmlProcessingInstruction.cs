// Decompiled with JetBrains decompiler
// Type: System.Xml.XmlProcessingInstruction
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Xml.XPath;

namespace System.Xml
{
  public class XmlProcessingInstruction : XmlLinkedNode
  {
    private string target;
    private string data;

    protected internal XmlProcessingInstruction(string target, string data, XmlDocument doc)
      : base(doc)
    {
      XmlConvert.VerifyName(target);
      if (data == null)
        data = string.Empty;
      this.target = target;
      this.data = data;
    }

    public string Data
    {
      get => this.data;
      set => this.data = value;
    }

    public override string InnerText
    {
      get => this.Data;
      set => this.data = value;
    }

    public override string LocalName => this.target;

    public override string Name => this.target;

    public override XmlNodeType NodeType => XmlNodeType.ProcessingInstruction;

    internal override XPathNodeType XPathNodeType => XPathNodeType.ProcessingInstruction;

    public string Target => this.target;

    public override string Value
    {
      get => this.data;
      set
      {
        if (this.IsReadOnly)
          throw new ArgumentException("This node is read-only.");
        this.data = value;
      }
    }

    public override XmlNode CloneNode(bool deep) => (XmlNode) new XmlProcessingInstruction(this.target, this.data, this.OwnerDocument);

    public override void WriteContentTo(XmlWriter w)
    {
    }

    public override void WriteTo(XmlWriter w) => w.WriteProcessingInstruction(this.target, this.data);
  }
}
