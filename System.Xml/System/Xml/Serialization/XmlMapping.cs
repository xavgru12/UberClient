// Decompiled with JetBrains decompiler
// Type: System.Xml.Serialization.XmlMapping
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;

namespace System.Xml.Serialization
{
  public abstract class XmlMapping
  {
    private ObjectMap map;
    private ArrayList relatedMaps;
    private SerializationFormat format;
    private SerializationSource source;
    internal string _elementName;
    internal string _namespace;
    private string key;

    internal XmlMapping()
    {
    }

    internal XmlMapping(string elementName, string ns)
    {
      this._elementName = elementName;
      this._namespace = ns;
    }

    [MonoTODO]
    public string XsdElementName => this._elementName;

    public string ElementName => this._elementName;

    public string Namespace => this._namespace;

    public void SetKey(string key) => this.key = key;

    internal string GetKey() => this.key;

    internal ObjectMap ObjectMap
    {
      get => this.map;
      set => this.map = value;
    }

    internal ArrayList RelatedMaps
    {
      get => this.relatedMaps;
      set => this.relatedMaps = value;
    }

    internal SerializationFormat Format
    {
      get => this.format;
      set => this.format = value;
    }

    internal SerializationSource Source
    {
      get => this.source;
      set => this.source = value;
    }
  }
}
