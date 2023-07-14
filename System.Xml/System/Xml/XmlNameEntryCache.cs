// Decompiled with JetBrains decompiler
// Type: System.Xml.XmlNameEntryCache
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;

namespace System.Xml
{
  internal class XmlNameEntryCache
  {
    private Hashtable table = new Hashtable();
    private XmlNameTable nameTable;
    private XmlNameEntry dummy = new XmlNameEntry(string.Empty, string.Empty, string.Empty);
    private char[] cacheBuffer;

    public XmlNameEntryCache(XmlNameTable nameTable) => this.nameTable = nameTable;

    public string GetAtomizedPrefixedName(string prefix, string local)
    {
      if (prefix == null || prefix.Length == 0)
        return local;
      if (this.cacheBuffer == null)
        this.cacheBuffer = new char[20];
      if (this.cacheBuffer.Length < prefix.Length + local.Length + 1)
        this.cacheBuffer = new char[Math.Max(prefix.Length + local.Length + 1, this.cacheBuffer.Length << 1)];
      prefix.CopyTo(0, this.cacheBuffer, 0, prefix.Length);
      this.cacheBuffer[prefix.Length] = ':';
      local.CopyTo(0, this.cacheBuffer, prefix.Length + 1, local.Length);
      return this.nameTable.Add(this.cacheBuffer, 0, prefix.Length + local.Length + 1);
    }

    public XmlNameEntry Add(string prefix, string local, string ns, bool atomic)
    {
      if (!atomic)
      {
        prefix = this.nameTable.Add(prefix);
        local = this.nameTable.Add(local);
        ns = this.nameTable.Add(ns);
      }
      XmlNameEntry key = this.GetInternal(prefix, local, ns, true);
      if (key == null)
      {
        key = new XmlNameEntry(prefix, local, ns);
        this.table[(object) key] = (object) key;
      }
      return key;
    }

    public XmlNameEntry Get(string prefix, string local, string ns, bool atomic) => this.GetInternal(prefix, local, ns, atomic);

    private XmlNameEntry GetInternal(string prefix, string local, string ns, bool atomic)
    {
      if (!atomic && (this.nameTable.Get(prefix) == null || this.nameTable.Get(local) == null || this.nameTable.Get(ns) == null))
        return (XmlNameEntry) null;
      this.dummy.Update(prefix, local, ns);
      return this.table[(object) this.dummy] as XmlNameEntry;
    }
  }
}
