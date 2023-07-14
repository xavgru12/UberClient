// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.XmlTextAttribute
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Text;

namespace System.Xml.Serialization
{
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
  public class XmlTextAttribute : Attribute
  {
    private string dataType;
    private Type type;

    public XmlTextAttribute()
    {
    }

    public XmlTextAttribute(Type type) => this.type = type;

    public string DataType
    {
      get => this.dataType == null ? string.Empty : this.dataType;
      set => this.dataType = value;
    }

    public Type Type
    {
      get => this.type;
      set => this.type = value;
    }

    internal void AddKeyHash(StringBuilder sb)
    {
      sb.Append("XTXA ");
      KeyHelper.AddField(sb, 1, this.type);
      KeyHelper.AddField(sb, 2, this.dataType);
      sb.Append('|');
    }
  }
}
