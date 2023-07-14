// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.SoapEnumAttribute
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Text;

namespace System.Xml.Serialization
{
  [AttributeUsage(AttributeTargets.Field)]
  public class SoapEnumAttribute : Attribute
  {
    private string name;

    public SoapEnumAttribute()
    {
    }

    public SoapEnumAttribute(string name) => this.name = name;

    public string Name
    {
      get => this.name == null ? string.Empty : this.name;
      set => this.name = value;
    }

    internal void AddKeyHash(StringBuilder sb)
    {
      sb.Append("SENA ");
      KeyHelper.AddField(sb, 1, this.name);
      sb.Append('|');
    }
  }
}
