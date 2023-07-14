// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.SoapTypeAttribute
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Text;

namespace System.Xml.Serialization
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface)]
  public class SoapTypeAttribute : Attribute
  {
    private string ns;
    private string typeName;
    private bool includeInSchema = true;

    public SoapTypeAttribute()
    {
    }

    public SoapTypeAttribute(string typeName) => this.typeName = typeName;

    public SoapTypeAttribute(string typeName, string ns)
    {
      this.typeName = typeName;
      this.ns = ns;
    }

    public bool IncludeInSchema
    {
      get => this.includeInSchema;
      set => this.includeInSchema = value;
    }

    public string Namespace
    {
      get => this.ns;
      set => this.ns = value;
    }

    public string TypeName
    {
      get => this.typeName == null ? string.Empty : this.typeName;
      set => this.typeName = value;
    }

    internal void AddKeyHash(StringBuilder sb)
    {
      sb.Append("STA ");
      KeyHelper.AddField(sb, 1, this.ns);
      KeyHelper.AddField(sb, 2, this.typeName);
      KeyHelper.AddField(sb, 3, this.includeInSchema);
      sb.Append('|');
    }
  }
}
