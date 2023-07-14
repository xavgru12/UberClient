// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.SoapElementAttribute
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Text;

namespace System.Xml.Serialization
{
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
  public class SoapElementAttribute : Attribute
  {
    private string dataType;
    private string elementName;
    private bool isNullable;

    public SoapElementAttribute()
    {
    }

    public SoapElementAttribute(string elementName) => this.elementName = elementName;

    public string DataType
    {
      get => this.dataType == null ? string.Empty : this.dataType;
      set => this.dataType = value;
    }

    public string ElementName
    {
      get => this.elementName == null ? string.Empty : this.elementName;
      set => this.elementName = value;
    }

    public bool IsNullable
    {
      get => this.isNullable;
      set => this.isNullable = value;
    }

    internal void AddKeyHash(StringBuilder sb)
    {
      sb.Append("SEA ");
      KeyHelper.AddField(sb, 1, this.elementName);
      KeyHelper.AddField(sb, 2, this.dataType);
      KeyHelper.AddField(sb, 3, this.isNullable);
      sb.Append('|');
    }
  }
}
