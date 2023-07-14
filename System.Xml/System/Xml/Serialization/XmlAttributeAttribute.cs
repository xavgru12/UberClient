// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.XmlAttributeAttribute
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Text;
using System.Xml.Schema;

namespace System.Xml.Serialization
{
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
  public class XmlAttributeAttribute : Attribute
  {
    private string attributeName;
    private string dataType;
    private Type type;
    private XmlSchemaForm form;
    private string ns;

    public XmlAttributeAttribute()
    {
    }

    public XmlAttributeAttribute(string attributeName) => this.attributeName = attributeName;

    public XmlAttributeAttribute(Type type) => this.type = type;

    public XmlAttributeAttribute(string attributeName, Type type)
    {
      this.attributeName = attributeName;
      this.type = type;
    }

    public string AttributeName
    {
      get => this.attributeName == null ? string.Empty : this.attributeName;
      set => this.attributeName = value;
    }

    public string DataType
    {
      get => this.dataType == null ? string.Empty : this.dataType;
      set => this.dataType = value;
    }

    public XmlSchemaForm Form
    {
      get => this.form;
      set => this.form = value;
    }

    public string Namespace
    {
      get => this.ns;
      set => this.ns = value;
    }

    public Type Type
    {
      get => this.type;
      set => this.type = value;
    }

    internal void AddKeyHash(StringBuilder sb)
    {
      sb.Append("XAA ");
      KeyHelper.AddField(sb, 1, this.ns);
      KeyHelper.AddField(sb, 2, this.attributeName);
      KeyHelper.AddField(sb, 3, this.form.ToString(), XmlSchemaForm.None.ToString());
      KeyHelper.AddField(sb, 4, this.dataType);
      KeyHelper.AddField(sb, 5, this.type);
      sb.Append('|');
    }
  }
}
