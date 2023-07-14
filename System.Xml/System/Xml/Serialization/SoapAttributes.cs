// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.SoapAttributes
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace System.Xml.Serialization
{
  public class SoapAttributes
  {
    private SoapAttributeAttribute soapAttribute;
    private object soapDefaultValue = (object) DBNull.Value;
    private SoapElementAttribute soapElement;
    private SoapEnumAttribute soapEnum;
    private bool soapIgnore;
    private SoapTypeAttribute soapType;

    public SoapAttributes()
    {
    }

    public SoapAttributes(ICustomAttributeProvider provider)
    {
      foreach (object customAttribute in provider.GetCustomAttributes(false))
      {
        switch (customAttribute)
        {
          case SoapAttributeAttribute _:
            this.soapAttribute = (SoapAttributeAttribute) customAttribute;
            break;
          case DefaultValueAttribute _:
            this.soapDefaultValue = ((DefaultValueAttribute) customAttribute).Value;
            break;
          case SoapElementAttribute _:
            this.soapElement = (SoapElementAttribute) customAttribute;
            break;
          case SoapEnumAttribute _:
            this.soapEnum = (SoapEnumAttribute) customAttribute;
            break;
          case SoapIgnoreAttribute _:
            this.soapIgnore = true;
            break;
          case SoapTypeAttribute _:
            this.soapType = (SoapTypeAttribute) customAttribute;
            break;
        }
      }
    }

    public SoapAttributeAttribute SoapAttribute
    {
      get => this.soapAttribute;
      set => this.soapAttribute = value;
    }

    public object SoapDefaultValue
    {
      get => this.soapDefaultValue;
      set => this.soapDefaultValue = value;
    }

    public SoapElementAttribute SoapElement
    {
      get => this.soapElement;
      set => this.soapElement = value;
    }

    public SoapEnumAttribute SoapEnum
    {
      get => this.soapEnum;
      set => this.soapEnum = value;
    }

    public bool SoapIgnore
    {
      get => this.soapIgnore;
      set => this.soapIgnore = value;
    }

    public SoapTypeAttribute SoapType
    {
      get => this.soapType;
      set => this.soapType = value;
    }

    internal void AddKeyHash(StringBuilder sb)
    {
      sb.Append("SA ");
      if (this.soapIgnore)
        sb.Append('i');
      if (this.soapAttribute != null)
        this.soapAttribute.AddKeyHash(sb);
      if (this.soapElement != null)
        this.soapElement.AddKeyHash(sb);
      if (this.soapEnum != null)
        this.soapEnum.AddKeyHash(sb);
      if (this.soapType != null)
        this.soapType.AddKeyHash(sb);
      if (this.soapDefaultValue == null)
        sb.Append("n");
      else if (!(this.soapDefaultValue is DBNull))
      {
        string xmlString = XmlCustomFormatter.ToXmlString(TypeTranslator.GetTypeData(this.soapDefaultValue.GetType()), this.soapDefaultValue);
        sb.Append("v" + xmlString);
      }
      sb.Append("|");
    }
  }
}
