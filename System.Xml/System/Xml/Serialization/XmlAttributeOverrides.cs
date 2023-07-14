// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.XmlAttributeOverrides
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.Globalization;
using System.Text;

namespace System.Xml.Serialization
{
  public class XmlAttributeOverrides
  {
    private Hashtable overrides;

    public XmlAttributeOverrides() => this.overrides = new Hashtable();

    public XmlAttributes this[Type type] => this[type, string.Empty];

    public XmlAttributes this[Type type, string member] => (XmlAttributes) this.overrides[(object) this.GetKey(type, member)];

    public void Add(Type type, XmlAttributes attributes) => this.Add(type, string.Empty, attributes);

    public void Add(Type type, string member, XmlAttributes attributes)
    {
      if (this.overrides[(object) this.GetKey(type, member)] != null)
        throw new Exception("The attributes for the given type and Member already exist in the collection");
      this.overrides.Add((object) this.GetKey(type, member), (object) attributes);
    }

    private TypeMember GetKey(Type type, string member) => new TypeMember(type, member);

    internal void AddKeyHash(StringBuilder sb)
    {
      sb.Append("XAO ");
      foreach (DictionaryEntry dictionaryEntry in this.overrides)
      {
        XmlAttributes xmlAttributes = (XmlAttributes) dictionaryEntry.Value;
        IFormattable key = dictionaryEntry.Key as IFormattable;
        sb.Append(key == null ? dictionaryEntry.Key.ToString() : key.ToString((string) null, (IFormatProvider) CultureInfo.InvariantCulture)).Append(' ');
        xmlAttributes.AddKeyHash(sb);
      }
      sb.Append("|");
    }
  }
}
