// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.XmlAttributes
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace System.Xml.Serialization
{
  public class XmlAttributes
  {
    private XmlAnyAttributeAttribute xmlAnyAttribute;
    private XmlAnyElementAttributes xmlAnyElements = new XmlAnyElementAttributes();
    private XmlArrayAttribute xmlArray;
    private XmlArrayItemAttributes xmlArrayItems = new XmlArrayItemAttributes();
    private XmlAttributeAttribute xmlAttribute;
    private XmlChoiceIdentifierAttribute xmlChoiceIdentifier;
    private object xmlDefaultValue = (object) DBNull.Value;
    private XmlElementAttributes xmlElements = new XmlElementAttributes();
    private XmlEnumAttribute xmlEnum;
    private bool xmlIgnore;
    private bool xmlns;
    private XmlRootAttribute xmlRoot;
    private XmlTextAttribute xmlText;
    private XmlTypeAttribute xmlType;

    public XmlAttributes()
    {
    }

    public XmlAttributes(ICustomAttributeProvider provider)
    {
      foreach (object customAttribute in provider.GetCustomAttributes(false))
      {
        switch (customAttribute)
        {
          case XmlAnyAttributeAttribute _:
            this.xmlAnyAttribute = (XmlAnyAttributeAttribute) customAttribute;
            break;
          case XmlAnyElementAttribute _:
            this.xmlAnyElements.Add((XmlAnyElementAttribute) customAttribute);
            break;
          case XmlArrayAttribute _:
            this.xmlArray = (XmlArrayAttribute) customAttribute;
            break;
          case XmlArrayItemAttribute _:
            this.xmlArrayItems.Add((XmlArrayItemAttribute) customAttribute);
            break;
          case XmlAttributeAttribute _:
            this.xmlAttribute = (XmlAttributeAttribute) customAttribute;
            break;
          case XmlChoiceIdentifierAttribute _:
            this.xmlChoiceIdentifier = (XmlChoiceIdentifierAttribute) customAttribute;
            break;
          case DefaultValueAttribute _:
            this.xmlDefaultValue = ((DefaultValueAttribute) customAttribute).Value;
            break;
          case XmlElementAttribute _:
            this.xmlElements.Add((XmlElementAttribute) customAttribute);
            break;
          case XmlEnumAttribute _:
            this.xmlEnum = (XmlEnumAttribute) customAttribute;
            break;
          case XmlIgnoreAttribute _:
            this.xmlIgnore = true;
            break;
          case XmlNamespaceDeclarationsAttribute _:
            this.xmlns = true;
            break;
          case XmlRootAttribute _:
            this.xmlRoot = (XmlRootAttribute) customAttribute;
            break;
          case XmlTextAttribute _:
            this.xmlText = (XmlTextAttribute) customAttribute;
            break;
          case XmlTypeAttribute _:
            this.xmlType = (XmlTypeAttribute) customAttribute;
            break;
        }
      }
    }

    public XmlAnyAttributeAttribute XmlAnyAttribute
    {
      get => this.xmlAnyAttribute;
      set => this.xmlAnyAttribute = value;
    }

    public XmlAnyElementAttributes XmlAnyElements => this.xmlAnyElements;

    public XmlArrayAttribute XmlArray
    {
      get => this.xmlArray;
      set => this.xmlArray = value;
    }

    public XmlArrayItemAttributes XmlArrayItems => this.xmlArrayItems;

    public XmlAttributeAttribute XmlAttribute
    {
      get => this.xmlAttribute;
      set => this.xmlAttribute = value;
    }

    public XmlChoiceIdentifierAttribute XmlChoiceIdentifier => this.xmlChoiceIdentifier;

    public object XmlDefaultValue
    {
      get => this.xmlDefaultValue;
      set => this.xmlDefaultValue = value;
    }

    public XmlElementAttributes XmlElements => this.xmlElements;

    public XmlEnumAttribute XmlEnum
    {
      get => this.xmlEnum;
      set => this.xmlEnum = value;
    }

    public bool XmlIgnore
    {
      get => this.xmlIgnore;
      set => this.xmlIgnore = value;
    }

    public bool Xmlns
    {
      get => this.xmlns;
      set => this.xmlns = value;
    }

    public XmlRootAttribute XmlRoot
    {
      get => this.xmlRoot;
      set => this.xmlRoot = value;
    }

    public XmlTextAttribute XmlText
    {
      get => this.xmlText;
      set => this.xmlText = value;
    }

    public XmlTypeAttribute XmlType
    {
      get => this.xmlType;
      set => this.xmlType = value;
    }

    internal void AddKeyHash(StringBuilder sb)
    {
      sb.Append("XA ");
      KeyHelper.AddField(sb, 1, this.xmlIgnore);
      KeyHelper.AddField(sb, 2, this.xmlns);
      KeyHelper.AddField(sb, 3, this.xmlAnyAttribute != null);
      this.xmlAnyElements.AddKeyHash(sb);
      this.xmlArrayItems.AddKeyHash(sb);
      this.xmlElements.AddKeyHash(sb);
      if (this.xmlArray != null)
        this.xmlArray.AddKeyHash(sb);
      if (this.xmlAttribute != null)
        this.xmlAttribute.AddKeyHash(sb);
      if (this.xmlDefaultValue == null)
        sb.Append("n");
      else if (!(this.xmlDefaultValue is DBNull))
      {
        string xmlString = XmlCustomFormatter.ToXmlString(TypeTranslator.GetTypeData(this.xmlDefaultValue.GetType()), this.xmlDefaultValue);
        sb.Append("v" + xmlString);
      }
      if (this.xmlEnum != null)
        this.xmlEnum.AddKeyHash(sb);
      if (this.xmlRoot != null)
        this.xmlRoot.AddKeyHash(sb);
      if (this.xmlText != null)
        this.xmlText.AddKeyHash(sb);
      if (this.xmlType != null)
        this.xmlType.AddKeyHash(sb);
      if (this.xmlChoiceIdentifier != null)
        this.xmlChoiceIdentifier.AddKeyHash(sb);
      sb.Append("|");
    }
  }
}
