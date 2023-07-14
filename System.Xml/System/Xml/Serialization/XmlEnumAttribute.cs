// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.XmlEnumAttribute
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Text;

namespace System.Xml.Serialization
{
  [AttributeUsage(AttributeTargets.Field)]
  public class XmlEnumAttribute : Attribute
  {
    private string name;

    public XmlEnumAttribute()
    {
    }

    public XmlEnumAttribute(string name) => this.name = name;

    public string Name
    {
      get => this.name;
      set => this.name = value;
    }

    internal void AddKeyHash(StringBuilder sb)
    {
      sb.Append("XENA ");
      KeyHelper.AddField(sb, 1, this.name);
      sb.Append('|');
    }
  }
}
