// Decompiled with JetBrains decompiler
// Type: System.Xml.XmlNameEntry
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml
{
  internal class XmlNameEntry
  {
    public string Prefix;
    public string LocalName;
    public string NS;
    public int Hash;
    private string prefixed_name_cache;

    public XmlNameEntry(string prefix, string local, string ns) => this.Update(prefix, local, ns);

    public void Update(string prefix, string local, string ns)
    {
      this.Prefix = prefix;
      this.LocalName = local;
      this.NS = ns;
      this.Hash = local.GetHashCode() + (prefix.Length <= 0 ? 0 : prefix.GetHashCode());
    }

    public override bool Equals(object other) => other is XmlNameEntry xmlNameEntry && xmlNameEntry.Hash == this.Hash && object.ReferenceEquals((object) xmlNameEntry.LocalName, (object) this.LocalName) && object.ReferenceEquals((object) xmlNameEntry.NS, (object) this.NS) && object.ReferenceEquals((object) xmlNameEntry.Prefix, (object) this.Prefix);

    public override int GetHashCode() => this.Hash;

    public string GetPrefixedName(XmlNameEntryCache owner)
    {
      if (this.prefixed_name_cache == null)
        this.prefixed_name_cache = owner.GetAtomizedPrefixedName(this.Prefix, this.LocalName);
      return this.prefixed_name_cache;
    }
  }
}
