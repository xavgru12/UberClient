// Decompiled with JetBrains decompiler
// Type: System.Xml.XmlNodeArrayList
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;

namespace System.Xml
{
  internal class XmlNodeArrayList : XmlNodeList
  {
    private ArrayList _rgNodes;

    public XmlNodeArrayList(ArrayList rgNodes) => this._rgNodes = rgNodes;

    public override int Count => this._rgNodes.Count;

    public override IEnumerator GetEnumerator() => this._rgNodes.GetEnumerator();

    public override XmlNode Item(int index) => index < 0 || this._rgNodes.Count <= index ? (XmlNode) null : (XmlNode) this._rgNodes[index];
  }
}
