// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Schema.XsdKeyEntryCollection
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;

namespace Mono.Xml.Schema
{
  internal class XsdKeyEntryCollection : CollectionBase
  {
    public void Add(XsdKeyEntry entry) => this.List.Add((object) entry);

    public XsdKeyEntry this[int i]
    {
      get => (XsdKeyEntry) this.List[i];
      set => this.List[i] = (object) value;
    }
  }
}
