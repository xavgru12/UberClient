// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.XmlTypeSerializationSource
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Text;

namespace System.Xml.Serialization
{
  internal class XmlTypeSerializationSource : SerializationSource
  {
    private string attributeOverridesHash;
    private Type type;
    private string rootHash;

    public XmlTypeSerializationSource(
      Type type,
      XmlRootAttribute root,
      XmlAttributeOverrides attributeOverrides,
      string namspace,
      Type[] includedTypes)
      : base(namspace, includedTypes)
    {
      if (attributeOverrides != null)
      {
        StringBuilder sb = new StringBuilder();
        attributeOverrides.AddKeyHash(sb);
        this.attributeOverridesHash = sb.ToString();
      }
      if (root != null)
      {
        StringBuilder sb = new StringBuilder();
        root.AddKeyHash(sb);
        this.rootHash = sb.ToString();
      }
      this.type = type;
    }

    public override bool Equals(object o) => o is XmlTypeSerializationSource other && this.type.Equals(other.type) && !(this.rootHash != other.rootHash) && !(this.attributeOverridesHash != other.attributeOverridesHash) && this.BaseEquals((SerializationSource) other);

    public override int GetHashCode() => this.type.GetHashCode();
  }
}
