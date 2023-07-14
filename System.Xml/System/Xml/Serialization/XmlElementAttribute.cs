// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.XmlElementAttribute
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Text;
using System.Xml.Schema;

namespace System.Xml.Serialization
{
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue, AllowMultiple = true)]
  public class XmlElementAttribute : Attribute
  {
    private string dataType;
    private string elementName;
    private XmlSchemaForm form;
    private string ns;
    private bool isNullable;
    private bool isNullableSpecified;
    private Type type;
    private int order = -1;

    public XmlElementAttribute()
    {
    }

    public XmlElementAttribute(string elementName) => this.elementName = elementName;

    public XmlElementAttribute(Type type) => this.type = type;

    public XmlElementAttribute(string elementName, Type type)
    {
      this.elementName = elementName;
      this.type = type;
    }

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

    public bool IsNullable
    {
      get => this.isNullable;
      set
      {
        this.isNullableSpecified = true;
        this.isNullable = value;
      }
    }

    internal bool IsNullableSpecified => this.isNullableSpecified;

    [MonoTODO]
    public int Order
    {
      get => this.order;
      set => this.order = value;
    }

    public Type Type
    {
      get => this.type;
      set => this.type = value;
    }

    internal void AddKeyHash(StringBuilder sb)
    {
      sb.Append("XEA ");
      KeyHelper.AddField(sb, 1, this.ns);
      KeyHelper.AddField(sb, 2, this.elementName);
      KeyHelper.AddField(sb, 3, this.form.ToString(), XmlSchemaForm.None.ToString());
      KeyHelper.AddField(sb, 4, this.dataType);
      KeyHelper.AddField(sb, 5, this.type);
      KeyHelper.AddField(sb, 6, this.isNullable);
      sb.Append('|');
    }
  }
}
