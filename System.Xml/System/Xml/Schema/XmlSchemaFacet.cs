// Decompiled with JetBrains decompiler
// Type: System.Xml.Schema.XmlSchemaFacet
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.ComponentModel;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
  public abstract class XmlSchemaFacet : XmlSchemaAnnotated
  {
    internal static readonly XmlSchemaFacet.Facet AllFacets = XmlSchemaFacet.Facet.length | XmlSchemaFacet.Facet.minLength | XmlSchemaFacet.Facet.maxLength | XmlSchemaFacet.Facet.pattern | XmlSchemaFacet.Facet.enumeration | XmlSchemaFacet.Facet.whiteSpace | XmlSchemaFacet.Facet.maxInclusive | XmlSchemaFacet.Facet.maxExclusive | XmlSchemaFacet.Facet.minExclusive | XmlSchemaFacet.Facet.minInclusive | XmlSchemaFacet.Facet.totalDigits | XmlSchemaFacet.Facet.fractionDigits;
    private bool isFixed;
    private string val;

    internal virtual XmlSchemaFacet.Facet ThisFacet => XmlSchemaFacet.Facet.None;

    [XmlAttribute("value")]
    public string Value
    {
      get => this.val;
      set => this.val = value;
    }

    [DefaultValue(false)]
    [XmlAttribute("fixed")]
    public virtual bool IsFixed
    {
      get => this.isFixed;
      set => this.isFixed = value;
    }

    [Flags]
    protected internal enum Facet
    {
      None = 0,
      length = 1,
      minLength = 2,
      maxLength = 4,
      pattern = 8,
      enumeration = 16, // 0x00000010
      whiteSpace = 32, // 0x00000020
      maxInclusive = 64, // 0x00000040
      maxExclusive = 128, // 0x00000080
      minExclusive = 256, // 0x00000100
      minInclusive = 512, // 0x00000200
      totalDigits = 1024, // 0x00000400
      fractionDigits = 2048, // 0x00000800
    }
  }
}
