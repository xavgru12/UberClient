// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.XmlTypeAttribute
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Text;

namespace System.Xml.Serialization
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface)]
  public class XmlTypeAttribute : Attribute
  {
    private bool includeInSchema = true;
    private string ns;
    private string typeName;
    private bool anonymousType;

    public XmlTypeAttribute()
    {
    }

    public XmlTypeAttribute(string typeName) => this.typeName = typeName;

    public bool AnonymousType
    {
      get => this.anonymousType;
      set => this.anonymousType = value;
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
      sb.Append("XTA ");
      KeyHelper.AddField(sb, 1, this.ns);
      KeyHelper.AddField(sb, 2, this.typeName);
      KeyHelper.AddField(sb, 4, this.includeInSchema);
      sb.Append('|');
    }
  }
}
