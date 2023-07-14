// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.XmlElementAttributes
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.Text;

namespace System.Xml.Serialization
{
  public class XmlElementAttributes : CollectionBase
  {
    public XmlElementAttribute this[int index]
    {
      get => (XmlElementAttribute) this.List[index];
      set => this.List[index] = (object) value;
    }

    public int Add(XmlElementAttribute attribute) => this.List.Add((object) attribute);

    public bool Contains(XmlElementAttribute attribute) => this.List.Contains((object) attribute);

    public int IndexOf(XmlElementAttribute attribute) => this.List.IndexOf((object) attribute);

    public void Insert(int index, XmlElementAttribute attribute) => this.List.Insert(index, (object) attribute);

    public void Remove(XmlElementAttribute attribute) => this.List.Remove((object) attribute);

    public void CopyTo(XmlElementAttribute[] array, int index) => this.List.CopyTo((Array) array, index);

    internal void AddKeyHash(StringBuilder sb)
    {
      if (this.Count == 0)
        return;
      sb.Append("XEAS ");
      for (int index = 0; index < this.Count; ++index)
        this[index].AddKeyHash(sb);
      sb.Append('|');
    }
  }
}
