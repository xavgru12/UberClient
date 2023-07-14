// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.XmlChoiceIdentifierAttribute
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Reflection;
using System.Text;

namespace System.Xml.Serialization
{
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
  public class XmlChoiceIdentifierAttribute : Attribute
  {
    private string memberName;

    public XmlChoiceIdentifierAttribute()
    {
    }

    public XmlChoiceIdentifierAttribute(string name) => this.memberName = name;

    public string MemberName
    {
      get => this.memberName == null ? string.Empty : this.memberName;
      set => this.memberName = value;
    }

    internal MemberInfo MemberInfo { get; set; }

    internal void AddKeyHash(StringBuilder sb)
    {
      sb.Append("XCA ");
      KeyHelper.AddField(sb, 1, this.memberName);
      sb.Append('|');
    }
  }
}
