// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.EnumMap
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Globalization;
using System.Text;

namespace System.Xml.Serialization
{
  internal class EnumMap : ObjectMap
  {
    private readonly EnumMap.EnumMapMember[] _members;
    private readonly bool _isFlags;
    private readonly string[] _enumNames;
    private readonly string[] _xmlNames;
    private readonly long[] _values;

    public EnumMap(EnumMap.EnumMapMember[] members, bool isFlags)
    {
      this._members = members;
      this._isFlags = isFlags;
      this._enumNames = new string[this._members.Length];
      this._xmlNames = new string[this._members.Length];
      this._values = new long[this._members.Length];
      for (int index = 0; index < this._members.Length; ++index)
      {
        EnumMap.EnumMapMember member = this._members[index];
        this._enumNames[index] = member.EnumName;
        this._xmlNames[index] = member.XmlName;
        this._values[index] = member.Value;
      }
    }

    public bool IsFlags => this._isFlags;

    public EnumMap.EnumMapMember[] Members => this._members;

    public string[] EnumNames => this._enumNames;

    public string[] XmlNames => this._xmlNames;

    public long[] Values => this._values;

    public string GetXmlName(string typeName, object enumValue)
    {
      if (enumValue is string)
        throw new InvalidCastException();
      long int64;
      try
      {
        int64 = ((IConvertible) enumValue).ToInt64((IFormatProvider) CultureInfo.CurrentCulture);
      }
      catch (FormatException ex)
      {
        throw new InvalidCastException();
      }
      for (int index = 0; index < this.Values.Length; ++index)
      {
        if (this.Values[index] == int64)
          return this.XmlNames[index];
      }
      if (this.IsFlags && int64 == 0L)
        return string.Empty;
      string str = string.Empty;
      if (this.IsFlags)
        str = XmlCustomFormatter.FromEnum(int64, this.XmlNames, this.Values, typeName);
      return str.Length != 0 ? str : throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, "'{0}' is not a valid value for {1}.", (object) int64, (object) typeName));
    }

    public string GetEnumName(string typeName, string xmlName)
    {
      if (this._isFlags)
      {
        xmlName = xmlName.Trim();
        if (xmlName.Length == 0)
          return "0";
        StringBuilder stringBuilder = new StringBuilder();
        foreach (string str1 in xmlName.Split((char[]) null))
        {
          if (!(str1 == string.Empty))
          {
            string str2 = (string) null;
            for (int index = 0; index < this.XmlNames.Length; ++index)
            {
              if (this.XmlNames[index] == str1)
              {
                str2 = this.EnumNames[index];
                break;
              }
            }
            if (str2 != null)
            {
              if (stringBuilder.Length > 0)
                stringBuilder.Append(',');
              stringBuilder.Append(str2);
            }
            else
              throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, "'{0}' is not a valid value for {1}.", (object) str1, (object) typeName));
          }
        }
        return stringBuilder.ToString();
      }
      foreach (EnumMap.EnumMapMember member in this._members)
      {
        if (member.XmlName == xmlName)
          return member.EnumName;
      }
      return (string) null;
    }

    public class EnumMapMember
    {
      private readonly string _xmlName;
      private readonly string _enumName;
      private readonly long _value;
      private string _documentation;

      public EnumMapMember(string xmlName, string enumName)
        : this(xmlName, enumName, 0L)
      {
      }

      public EnumMapMember(string xmlName, string enumName, long value)
      {
        this._xmlName = xmlName;
        this._enumName = enumName;
        this._value = value;
      }

      public string XmlName => this._xmlName;

      public string EnumName => this._enumName;

      public long Value => this._value;

      public string Documentation
      {
        get => this._documentation;
        set => this._documentation = value;
      }
    }
  }
}
