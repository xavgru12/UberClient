// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.XmlAnyElementAttribute
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Text;

namespace System.Xml.Serialization
{
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue, AllowMultiple = true)]
  public class XmlAnyElementAttribute : Attribute
  {
    private string elementName;
    private string ns;
    private bool isNamespaceSpecified;
    private int order = -1;

    public XmlAnyElementAttribute()
    {
    }

    public XmlAnyElementAttribute(string name) => this.elementName = name;

    public XmlAnyElementAttribute(string name, string ns)
    {
      this.elementName = name;
      this.ns = ns;
    }

    public string Name
    {
      get => this.elementName == null ? string.Empty : this.elementName;
      set => this.elementName = value;
    }

    public string Namespace
    {
      get => this.ns;
      set
      {
        this.isNamespaceSpecified = true;
        this.ns = value;
      }
    }

    internal bool NamespaceSpecified => this.isNamespaceSpecified;

    [MonoTODO]
    public int Order
    {
      get => this.order;
      set => this.order = value;
    }

    internal void AddKeyHash(StringBuilder sb)
    {
      sb.Append("XAEA ");
      KeyHelper.AddField(sb, 1, this.ns);
      KeyHelper.AddField(sb, 2, this.elementName);
      sb.Append('|');
    }
  }
}
