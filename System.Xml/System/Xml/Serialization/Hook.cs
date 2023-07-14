// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.Hook
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.Serialization
{
  [XmlType("hook")]
  internal class Hook
  {
    [XmlAttribute("type")]
    public HookType HookType;
    [XmlElement("select")]
    public Select Select;
    [XmlElement("insertBefore")]
    public string InsertBefore;
    [XmlElement("insertAfter")]
    public string InsertAfter;
    [XmlElement("replace")]
    public string Replace;

    public string GetCode(HookAction action)
    {
      if (action == HookAction.InsertBefore)
        return this.InsertBefore;
      return action == HookAction.InsertAfter ? this.InsertAfter : this.Replace;
    }
  }
}
