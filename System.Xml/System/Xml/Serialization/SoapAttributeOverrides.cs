// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.SoapAttributeOverrides
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.Text;

namespace System.Xml.Serialization
{
  public class SoapAttributeOverrides
  {
    private Hashtable overrides;

    public SoapAttributeOverrides() => this.overrides = new Hashtable();

    public SoapAttributes this[Type type] => this[type, string.Empty];

    public SoapAttributes this[Type type, string member] => (SoapAttributes) this.overrides[(object) this.GetKey(type, member)];

    public void Add(Type type, SoapAttributes attributes) => this.Add(type, string.Empty, attributes);

    public void Add(Type type, string member, SoapAttributes attributes)
    {
      if (this.overrides[(object) this.GetKey(type, member)] != null)
        throw new Exception("The attributes for the given type and Member already exist in the collection");
      this.overrides.Add((object) this.GetKey(type, member), (object) attributes);
    }

    private TypeMember GetKey(Type type, string member) => new TypeMember(type, member);

    internal void AddKeyHash(StringBuilder sb)
    {
      sb.Append("SAO ");
      foreach (DictionaryEntry dictionaryEntry in this.overrides)
      {
        SoapAttributes soapAttributes = (SoapAttributes) this.overrides[dictionaryEntry.Key];
        sb.Append(dictionaryEntry.Key.ToString()).Append(' ');
        soapAttributes.AddKeyHash(sb);
      }
      sb.Append("|");
    }
  }
}
