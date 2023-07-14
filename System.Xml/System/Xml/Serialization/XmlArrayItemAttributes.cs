// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.XmlArrayItemAttributes
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.Text;

namespace System.Xml.Serialization
{
  public class XmlArrayItemAttributes : CollectionBase
  {
    public XmlArrayItemAttribute this[int index]
    {
      get => (XmlArrayItemAttribute) this.List[index];
      set => this.List[index] = (object) value;
    }

    public int Add(XmlArrayItemAttribute attribute) => this.List.Add((object) attribute);

    public bool Contains(XmlArrayItemAttribute attribute) => this.List.Contains((object) attribute);

    public void CopyTo(XmlArrayItemAttribute[] array, int index) => this.List.CopyTo((Array) array, index);

    public int IndexOf(XmlArrayItemAttribute attribute) => this.List.IndexOf((object) attribute);

    public void Insert(int index, XmlArrayItemAttribute attribute) => this.List.Insert(index, (object) attribute);

    public void Remove(XmlArrayItemAttribute attribute) => this.List.Remove((object) attribute);

    internal void AddKeyHash(StringBuilder sb)
    {
      if (this.Count == 0)
        return;
      sb.Append("XAIAS ");
      for (int index = 0; index < this.Count; ++index)
        this[index].AddKeyHash(sb);
      sb.Append('|');
    }
  }
}
