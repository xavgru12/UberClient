// Decompiled with JetBrains decompiler
// Type: System.Xml.Schema.XmlSchemaObjectCollection
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;

namespace System.Xml.Schema
{
  public class XmlSchemaObjectCollection : CollectionBase
  {
    public XmlSchemaObjectCollection()
    {
    }

    public XmlSchemaObjectCollection(XmlSchemaObject parent)
    {
    }

    public virtual XmlSchemaObject this[int index]
    {
      get => (XmlSchemaObject) this.List[index];
      set => this.List[index] = (object) value;
    }

    public int Add(XmlSchemaObject item) => this.List.Add((object) item);

    public bool Contains(XmlSchemaObject item) => this.List.Contains((object) item);

    public void CopyTo(XmlSchemaObject[] array, int index) => this.List.CopyTo((Array) array, index);

    public XmlSchemaObjectEnumerator GetEnumerator() => new XmlSchemaObjectEnumerator(this.List);

    public int IndexOf(XmlSchemaObject item) => this.List.IndexOf((object) item);

    public void Insert(int index, XmlSchemaObject item) => this.List.Insert(index, (object) item);

    protected override void OnClear()
    {
    }

    protected override void OnInsert(int index, object item)
    {
    }

    protected override void OnRemove(int index, object item)
    {
    }

    protected override void OnSet(int index, object oldValue, object newValue)
    {
    }

    public void Remove(XmlSchemaObject item) => this.List.Remove((object) item);
  }
}
