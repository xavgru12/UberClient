// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Xsl.Attribute
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace Mono.Xml.Xsl
{
  internal struct Attribute
  {
    public string Prefix;
    public string Namespace;
    public string LocalName;
    public string Value;

    public Attribute(string prefix, string namespaceUri, string localName, string value)
    {
      this.Prefix = prefix;
      this.Namespace = namespaceUri;
      this.LocalName = localName;
      this.Value = value;
    }
  }
}
