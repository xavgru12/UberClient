// Decompiled with JetBrains decompiler
// Type: System.Xml.XmlCharacterData
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Xml.XPath;

namespace System.Xml
{
  public abstract class XmlCharacterData : XmlLinkedNode
  {
    private string data;

    protected internal XmlCharacterData(string data, XmlDocument doc)
      : base(doc)
    {
      if (data == null)
        data = string.Empty;
      this.data = data;
    }

    public virtual string Data
    {
      get => this.data;
      set
      {
        string data = this.data;
        this.OwnerDocument.onNodeChanging((XmlNode) this, this.ParentNode, data, value);
        this.data = value;
        this.OwnerDocument.onNodeChanged((XmlNode) this, this.ParentNode, data, value);
      }
    }

    public override string InnerText
    {
      get => this.data;
      set => this.Data = value;
    }

    public virtual int Length => this.data != null ? this.data.Length : 0;

    public override string Value
    {
      get => this.data;
      set => this.Data = value;
    }

    internal override XPathNodeType XPathNodeType => XPathNodeType.Text;

    public virtual void AppendData(string strData)
    {
      string data = this.data;
      string newValue = this.data += strData;
      this.OwnerDocument.onNodeChanging((XmlNode) this, this.ParentNode, data, newValue);
      this.data = newValue;
      this.OwnerDocument.onNodeChanged((XmlNode) this, this.ParentNode, data, newValue);
    }

    public virtual void DeleteData(int offset, int count)
    {
      if (offset < 0)
        throw new ArgumentOutOfRangeException(nameof (offset), "Must be non-negative and must not be greater than the length of this instance.");
      int count1 = this.data.Length - offset;
      if (offset + count < this.data.Length)
        count1 = count;
      string data = this.data;
      string newValue = this.data.Remove(offset, count1);
      this.OwnerDocument.onNodeChanging((XmlNode) this, this.ParentNode, data, newValue);
      this.data = newValue;
      this.OwnerDocument.onNodeChanged((XmlNode) this, this.ParentNode, data, newValue);
    }

    public virtual void InsertData(int offset, string strData)
    {
      if (offset < 0 || offset > this.data.Length)
        throw new ArgumentOutOfRangeException(nameof (offset), "Must be non-negative and must not be greater than the length of this instance.");
      string data = this.data;
      string newValue = this.data.Insert(offset, strData);
      this.OwnerDocument.onNodeChanging((XmlNode) this, this.ParentNode, data, newValue);
      this.data = newValue;
      this.OwnerDocument.onNodeChanged((XmlNode) this, this.ParentNode, data, newValue);
    }

    public virtual void ReplaceData(int offset, int count, string strData)
    {
      if (offset < 0 || offset > this.data.Length)
        throw new ArgumentOutOfRangeException(nameof (offset), "Must be non-negative and must not be greater than the length of this instance.");
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count), "Must be non-negative.");
      if (strData == null)
        throw new ArgumentNullException(nameof (strData), "Must be non-null.");
      string data = this.data;
      string newValue = this.data.Substring(0, offset) + strData;
      if (offset + count < this.data.Length)
        newValue += this.data.Substring(offset + count);
      this.OwnerDocument.onNodeChanging((XmlNode) this, this.ParentNode, data, newValue);
      this.data = newValue;
      this.OwnerDocument.onNodeChanged((XmlNode) this, this.ParentNode, data, newValue);
    }

    public virtual string Substring(int offset, int count) => this.data.Length < offset + count ? this.data.Substring(offset) : this.data.Substring(offset, count);
  }
}
