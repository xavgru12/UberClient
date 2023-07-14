// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.XmlDeserializationEvents
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.Serialization
{
  public struct XmlDeserializationEvents
  {
    private XmlAttributeEventHandler onUnknownAttribute;
    private XmlElementEventHandler onUnknownElement;
    private XmlNodeEventHandler onUnknownNode;
    private UnreferencedObjectEventHandler onUnreferencedObject;

    public XmlAttributeEventHandler OnUnknownAttribute
    {
      get => this.onUnknownAttribute;
      set => this.onUnknownAttribute = value;
    }

    public XmlElementEventHandler OnUnknownElement
    {
      get => this.onUnknownElement;
      set => this.onUnknownElement = value;
    }

    public XmlNodeEventHandler OnUnknownNode
    {
      get => this.onUnknownNode;
      set => this.onUnknownNode = value;
    }

    public UnreferencedObjectEventHandler OnUnreferencedObject
    {
      get => this.onUnreferencedObject;
      set => this.onUnreferencedObject = value;
    }
  }
}
