// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.SoapAttributeAttribute
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Text;

namespace System.Xml.Serialization
{
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
  public class SoapAttributeAttribute : Attribute
  {
    private string attrName;
    private string dataType;
    private string ns;

    public SoapAttributeAttribute()
    {
    }

    public SoapAttributeAttribute(string attrName) => this.attrName = attrName;

    public string AttributeName
    {
      get => this.attrName == null ? string.Empty : this.attrName;
      set => this.attrName = value;
    }

    public string DataType
    {
      get => this.dataType == null ? string.Empty : this.dataType;
      set => this.dataType = value;
    }

    public string Namespace
    {
      get => this.ns;
      set => this.ns = value;
    }

    internal void AddKeyHash(StringBuilder sb)
    {
      sb.Append("SAA ");
      KeyHelper.AddField(sb, 1, this.attrName);
      KeyHelper.AddField(sb, 2, this.dataType);
      KeyHelper.AddField(sb, 3, this.ns);
      sb.Append("|");
    }
  }
}
