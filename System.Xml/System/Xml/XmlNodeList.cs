// Decompiled with JetBrains decompiler
// Type: System.Xml.XmlNodeList
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.Runtime.CompilerServices;

namespace System.Xml
{
  public abstract class XmlNodeList : IEnumerable
  {
    public abstract int Count { get; }

    [IndexerName("ItemOf")]
    public virtual XmlNode this[int i] => this.Item(i);

    public abstract IEnumerator GetEnumerator();

    public abstract XmlNode Item(int index);
  }
}
