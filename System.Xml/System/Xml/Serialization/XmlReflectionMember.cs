// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.XmlReflectionMember
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Text;

namespace System.Xml.Serialization
{
  public class XmlReflectionMember
  {
    private bool isReturnValue;
    private string memberName;
    private Type memberType;
    private bool overrideIsNullable;
    private SoapAttributes soapAttributes;
    private XmlAttributes xmlAttributes;
    private Type declaringType;

    public XmlReflectionMember()
    {
    }

    internal XmlReflectionMember(string name, Type type, XmlAttributes attributes)
    {
      this.memberName = name;
      this.memberType = type;
      this.xmlAttributes = attributes;
    }

    internal XmlReflectionMember(string name, Type type, SoapAttributes attributes)
    {
      this.memberName = name;
      this.memberType = type;
      this.soapAttributes = attributes;
    }

    public bool IsReturnValue
    {
      get => this.isReturnValue;
      set => this.isReturnValue = value;
    }

    public string MemberName
    {
      get => this.memberName;
      set => this.memberName = value;
    }

    public Type MemberType
    {
      get => this.memberType;
      set => this.memberType = value;
    }

    public bool OverrideIsNullable
    {
      get => this.overrideIsNullable;
      set => this.overrideIsNullable = value;
    }

    public SoapAttributes SoapAttributes
    {
      get
      {
        if (this.soapAttributes == null)
          this.soapAttributes = new SoapAttributes();
        return this.soapAttributes;
      }
      set => this.soapAttributes = value;
    }

    public XmlAttributes XmlAttributes
    {
      get
      {
        if (this.xmlAttributes == null)
          this.xmlAttributes = new XmlAttributes();
        return this.xmlAttributes;
      }
      set => this.xmlAttributes = value;
    }

    internal Type DeclaringType
    {
      get => this.declaringType;
      set => this.declaringType = value;
    }

    internal void AddKeyHash(StringBuilder sb)
    {
      sb.Append("XRM ");
      KeyHelper.AddField(sb, 1, this.isReturnValue);
      KeyHelper.AddField(sb, 1, this.memberName);
      KeyHelper.AddField(sb, 1, this.memberType);
      KeyHelper.AddField(sb, 1, this.overrideIsNullable);
      if (this.soapAttributes != null)
        this.soapAttributes.AddKeyHash(sb);
      if (this.xmlAttributes != null)
        this.xmlAttributes.AddKeyHash(sb);
      sb.Append('|');
    }
  }
}
