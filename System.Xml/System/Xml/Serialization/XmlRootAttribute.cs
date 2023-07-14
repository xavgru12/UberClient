// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.XmlRootAttribute
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Text;

namespace System.Xml.Serialization
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface | AttributeTargets.ReturnValue)]
  public class XmlRootAttribute : Attribute
  {
    private string dataType;
    private string elementName;
    private bool isNullable = true;
    private bool isNullableSpecified;
    private string ns;

    public XmlRootAttribute()
    {
    }

    public XmlRootAttribute(string elementName) => this.elementName = elementName;

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
      set
      {
        this.isNullableSpecified = true;
        this.isNullable = value;
      }
    }

    public bool IsNullableSpecified => this.isNullableSpecified;

    public string Namespace
    {
      get => this.ns;
      set => this.ns = value;
    }

    internal void AddKeyHash(StringBuilder sb)
    {
      sb.Append("XRA ");
      KeyHelper.AddField(sb, 1, this.ns);
      KeyHelper.AddField(sb, 2, this.elementName);
      KeyHelper.AddField(sb, 3, this.dataType);
      KeyHelper.AddField(sb, 4, this.isNullable);
      sb.Append('|');
    }

    internal string Key
    {
      get
      {
        StringBuilder sb = new StringBuilder();
        this.AddKeyHash(sb);
        return sb.ToString();
      }
    }
  }
}
