// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Schema.XmlSchemaUri
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System;

namespace Mono.Xml.Schema
{
  internal class XmlSchemaUri : Uri
  {
    public string value;

    public XmlSchemaUri(string src)
      : this(src, XmlSchemaUri.HasValidScheme(src))
    {
    }

    private XmlSchemaUri(string src, bool formal)
      : base(!formal ? "anyuri:" + src : src, !formal)
    {
      this.value = src;
    }

    private static bool HasValidScheme(string src)
    {
      int num = src.IndexOf(':');
      if (num < 0)
        return false;
      for (int index = 0; index < num; ++index)
      {
        switch (src[index])
        {
          case '+':
          case '-':
          case '.':
            continue;
          default:
            if (!char.IsLetterOrDigit(src[index]))
              return false;
            continue;
        }
      }
      return true;
    }

    public override bool Equals(object obj) => (object) (obj as XmlSchemaUri) != null && (XmlSchemaUri) obj == this;

    public override int GetHashCode() => this.value.GetHashCode();

    public override string ToString() => this.value;

    public static bool operator ==(XmlSchemaUri v1, XmlSchemaUri v2) => v1.value == v2.value;

    public static bool operator !=(XmlSchemaUri v1, XmlSchemaUri v2) => v1.value != v2.value;
  }
}
