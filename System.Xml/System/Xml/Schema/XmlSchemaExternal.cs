// Decompiled with JetBrains decompiler
// Type: System.Xml.Schema.XmlSchemaExternal
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
  public abstract class XmlSchemaExternal : XmlSchemaObject
  {
    private string id;
    private XmlSchema schema;
    private string location;
    private XmlAttribute[] unhandledAttributes;

    [XmlAttribute("schemaLocation", DataType = "anyURI")]
    public string SchemaLocation
    {
      get => this.location;
      set => this.location = value;
    }

    [XmlIgnore]
    public XmlSchema Schema
    {
      get => this.schema;
      set => this.schema = value;
    }

    [XmlAttribute("id", DataType = "ID")]
    public string Id
    {
      get => this.id;
      set => this.id = value;
    }

    [XmlAnyAttribute]
    public XmlAttribute[] UnhandledAttributes
    {
      get
      {
        if (this.unhandledAttributeList != null)
        {
          this.unhandledAttributes = (XmlAttribute[]) this.unhandledAttributeList.ToArray(typeof (XmlAttribute));
          this.unhandledAttributeList = (ArrayList) null;
        }
        return this.unhandledAttributes;
      }
      set
      {
        this.unhandledAttributes = value;
        this.unhandledAttributeList = (ArrayList) null;
      }
    }
  }
}
