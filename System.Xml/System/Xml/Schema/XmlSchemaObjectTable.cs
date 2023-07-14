// Decompiled with JetBrains decompiler
// Type: System.Xml.Schema.XmlSchemaObjectTable
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.Collections.Specialized;

namespace System.Xml.Schema
{
  public class XmlSchemaObjectTable
  {
    private HybridDictionary table;

    internal XmlSchemaObjectTable() => this.table = new HybridDictionary();

    public int Count => this.table.Count;

    public XmlSchemaObject this[XmlQualifiedName name] => (XmlSchemaObject) this.table[(object) name];

    public ICollection Names => this.table.Keys;

    public ICollection Values => this.table.Values;

    public bool Contains(XmlQualifiedName name) => this.table.Contains((object) name);

    public IDictionaryEnumerator GetEnumerator() => (IDictionaryEnumerator) new XmlSchemaObjectTable.XmlSchemaObjectTableEnumerator(this);

    internal void Add(XmlQualifiedName name, XmlSchemaObject value) => this.table[(object) name] = (object) value;

    internal void Clear() => this.table.Clear();

    internal void Set(XmlQualifiedName name, XmlSchemaObject value) => this.table[(object) name] = (object) value;

    internal class XmlSchemaObjectTableEnumerator : IEnumerator, IDictionaryEnumerator
    {
      private IDictionaryEnumerator xenum;
      private IEnumerable tmp;

      internal XmlSchemaObjectTableEnumerator(XmlSchemaObjectTable table)
      {
        this.tmp = (IEnumerable) table.table;
        this.xenum = (IDictionaryEnumerator) this.tmp.GetEnumerator();
      }

      bool IEnumerator.MoveNext() => this.xenum.MoveNext();

      void IEnumerator.Reset() => this.xenum.Reset();

      object IEnumerator.Current => (object) this.xenum.Entry;

      DictionaryEntry IDictionaryEnumerator.Entry => this.xenum.Entry;

      object IDictionaryEnumerator.Key => (object) (XmlQualifiedName) this.xenum.Key;

      object IDictionaryEnumerator.Value => (object) (XmlSchemaObject) this.xenum.Value;

      public XmlSchemaObject Current => (XmlSchemaObject) this.xenum.Value;

      public DictionaryEntry Entry => this.xenum.Entry;

      public XmlQualifiedName Key => (XmlQualifiedName) this.xenum.Key;

      public XmlSchemaObject Value => (XmlSchemaObject) this.xenum.Value;

      public bool MoveNext() => this.xenum.MoveNext();
    }
  }
}
