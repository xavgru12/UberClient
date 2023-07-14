// Decompiled with JetBrains decompiler
// Type: Mono.Xml.DTDAttributeDefinition
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System;
using System.Collections;
using System.Globalization;
using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace Mono.Xml
{
  internal class DTDAttributeDefinition : DTDNode
  {
    private string name;
    private XmlSchemaDatatype datatype;
    private ArrayList enumeratedLiterals;
    private string unresolvedDefault;
    private ArrayList enumeratedNotations;
    private DTDAttributeOccurenceType occurenceType;
    private string resolvedDefaultValue;
    private string resolvedNormalizedDefaultValue;

    internal DTDAttributeDefinition(DTDObjectModel root) => this.SetRoot(root);

    public string Name
    {
      get => this.name;
      set => this.name = value;
    }

    public XmlSchemaDatatype Datatype
    {
      get => this.datatype;
      set => this.datatype = value;
    }

    public DTDAttributeOccurenceType OccurenceType
    {
      get => this.occurenceType;
      set => this.occurenceType = value;
    }

    public ArrayList EnumeratedAttributeDeclaration
    {
      get
      {
        if (this.enumeratedLiterals == null)
          this.enumeratedLiterals = new ArrayList();
        return this.enumeratedLiterals;
      }
    }

    public ArrayList EnumeratedNotations
    {
      get
      {
        if (this.enumeratedNotations == null)
          this.enumeratedNotations = new ArrayList();
        return this.enumeratedNotations;
      }
    }

    public string DefaultValue
    {
      get
      {
        if (this.resolvedDefaultValue == null)
          this.resolvedDefaultValue = this.ComputeDefaultValue();
        return this.resolvedDefaultValue;
      }
    }

    public string NormalizedDefaultValue
    {
      get
      {
        if (this.resolvedNormalizedDefaultValue == null)
        {
          string defaultValue = this.ComputeDefaultValue();
          try
          {
            object obj = this.Datatype.ParseValue(defaultValue, (XmlNameTable) null, (IXmlNamespaceResolver) null);
            string str;
            switch (obj)
            {
              case string[] _:
                str = string.Join(" ", (string[]) obj);
                break;
              case IFormattable _:
                str = ((IFormattable) obj).ToString((string) null, (IFormatProvider) CultureInfo.InvariantCulture);
                break;
              default:
                str = obj.ToString();
                break;
            }
            this.resolvedNormalizedDefaultValue = str;
          }
          catch (Exception ex)
          {
            this.resolvedNormalizedDefaultValue = this.Datatype.Normalize(defaultValue);
          }
        }
        return this.resolvedNormalizedDefaultValue;
      }
    }

    public string UnresolvedDefaultValue
    {
      get => this.unresolvedDefault;
      set => this.unresolvedDefault = value;
    }

    public char QuoteChar => this.UnresolvedDefaultValue.Length > 0 ? this.UnresolvedDefaultValue[0] : '"';

    internal string ComputeDefaultValue()
    {
      if (this.UnresolvedDefaultValue == null)
        return (string) null;
      StringBuilder stringBuilder = new StringBuilder();
      int startIndex1 = 0;
      string unresolvedDefaultValue;
      int startIndex2;
      int num;
      for (unresolvedDefaultValue = this.UnresolvedDefaultValue; (startIndex2 = unresolvedDefaultValue.IndexOf('&', startIndex1)) >= 0; startIndex1 = num + 1)
      {
        num = unresolvedDefaultValue.IndexOf(';', startIndex2);
        if (unresolvedDefaultValue[startIndex2 + 1] == '#')
        {
          char ch = unresolvedDefaultValue[startIndex2 + 2];
          NumberStyles style = NumberStyles.Integer;
          string s;
          if (ch == 'x' || ch == 'X')
          {
            s = unresolvedDefaultValue.Substring(startIndex2 + 3, num - startIndex2 - 3);
            style |= NumberStyles.HexNumber;
          }
          else
            s = unresolvedDefaultValue.Substring(startIndex2 + 2, num - startIndex2 - 2);
          stringBuilder.Append((char) int.Parse(s, style, (IFormatProvider) CultureInfo.InvariantCulture));
        }
        else
        {
          stringBuilder.Append(unresolvedDefaultValue.Substring(startIndex1, startIndex2 - 1));
          string name = unresolvedDefaultValue.Substring(startIndex2 + 1, num - 2);
          int predefinedEntity = XmlChar.GetPredefinedEntity(name);
          if (predefinedEntity >= 0)
            stringBuilder.Append(predefinedEntity);
          else
            stringBuilder.Append(this.Root.ResolveEntity(name));
        }
      }
      stringBuilder.Append(unresolvedDefaultValue.Substring(startIndex1));
      string defaultValue = stringBuilder.ToString(1, stringBuilder.Length - 2);
      stringBuilder.Length = 0;
      return defaultValue;
    }
  }
}
