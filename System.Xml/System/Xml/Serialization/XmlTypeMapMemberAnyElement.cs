// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.XmlTypeMapMemberAnyElement
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;

namespace System.Xml.Serialization
{
  internal class XmlTypeMapMemberAnyElement : XmlTypeMapMemberExpandable
  {
    public bool IsElementDefined(string name, string ns)
    {
      foreach (XmlTypeMapElementInfo typeMapElementInfo in (ArrayList) this.ElementInfo)
      {
        if (typeMapElementInfo.IsUnnamedAnyElement || typeMapElementInfo.ElementName == name && typeMapElementInfo.Namespace == ns)
          return true;
      }
      return false;
    }

    public bool IsDefaultAny
    {
      get
      {
        foreach (XmlTypeMapElementInfo typeMapElementInfo in (ArrayList) this.ElementInfo)
        {
          if (typeMapElementInfo.IsUnnamedAnyElement)
            return true;
        }
        return false;
      }
    }

    public bool CanBeText => this.ElementInfo.Count > 0 && ((XmlTypeMapElementInfo) this.ElementInfo[0]).IsTextElement;
  }
}
