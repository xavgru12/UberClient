// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Schema.XsdKeyEntryFieldCollection
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;

namespace Mono.Xml.Schema
{
  internal class XsdKeyEntryFieldCollection : CollectionBase
  {
    public XsdKeyEntryField this[int i]
    {
      get => (XsdKeyEntryField) this.List[i];
      set => this.List[i] = (object) value;
    }

    public int Add(XsdKeyEntryField value) => this.List.Add((object) value);
  }
}
