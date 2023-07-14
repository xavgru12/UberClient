// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.XmlArrayAttribute
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Text;
using System.Xml.Schema;

namespace System.Xml.Serialization
{
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
  public class XmlArrayAttribute : Attribute
  {
    private string elementName;
    private XmlSchemaForm form;
    private bool isNullable;
    private string ns;
    private int order = -1;

    public XmlArrayAttribute()
    {
    }

    public XmlArrayAttribute(string elementName) => this.elementName = elementName;

    public string ElementName
    {
      get => this.elementName == null ? string.Empty : this.elementName;
      set => this.elementName = value;
    }

    public XmlSchemaForm Form
    {
      get => this.form;
      set => this.form = value;
    }

    public bool IsNullable
    {
      get => this.isNullable;
      set => this.isNullable = value;
    }

    public string Namespace
    {
      get => this.ns;
      set => this.ns = value;
    }

    [MonoTODO]
    public int Order
    {
      get => this.order;
      set => this.order = value;
    }

    internal void AddKeyHash(StringBuilder sb)
    {
      sb.Append("XAAT ");
      KeyHelper.AddField(sb, 1, this.ns);
      KeyHelper.AddField(sb, 2, this.elementName);
      KeyHelper.AddField(sb, 3, this.form.ToString(), XmlSchemaForm.None.ToString());
      KeyHelper.AddField(sb, 4, this.isNullable);
      sb.Append('|');
    }
  }
}
