// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.XmlTypeMapElementInfoList
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;

namespace System.Xml.Serialization
{
  internal class XmlTypeMapElementInfoList : ArrayList
  {
    public int IndexOfElement(string name, string namspace)
    {
      for (int index = 0; index < this.Count; ++index)
      {
        XmlTypeMapElementInfo typeMapElementInfo = (XmlTypeMapElementInfo) this[index];
        if (typeMapElementInfo.ElementName == name && typeMapElementInfo.Namespace == namspace)
          return index;
      }
      return -1;
    }
  }
}
