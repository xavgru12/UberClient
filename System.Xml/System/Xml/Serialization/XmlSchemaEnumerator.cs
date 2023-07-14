// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.XmlSchemaEnumerator
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;

namespace System.Xml.Serialization
{
  [MonoTODO]
  public class XmlSchemaEnumerator : IEnumerator<XmlSchema>, IDisposable, IEnumerator
  {
    private IEnumerator e;

    public XmlSchemaEnumerator(XmlSchemas list) => this.e = list.GetEnumerator();

    object IEnumerator.Current => (object) this.Current;

    void IEnumerator.Reset() => this.e.Reset();

    public XmlSchema Current => (XmlSchema) this.e.Current;

    public void Dispose()
    {
    }

    public bool MoveNext() => this.e.MoveNext();
  }
}
