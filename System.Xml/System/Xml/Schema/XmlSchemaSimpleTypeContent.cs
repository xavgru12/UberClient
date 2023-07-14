// Decompiled with JetBrains decompiler
// Type: System.Xml.Schema.XmlSchemaSimpleTypeContent
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

namespace System.Xml.Schema
{
  public abstract class XmlSchemaSimpleTypeContent : XmlSchemaAnnotated
  {
    internal XmlSchemaSimpleType OwnerType;

    internal object ActualBaseSchemaType => this.OwnerType.BaseSchemaType;

    internal virtual string Normalize(string s, XmlNameTable nt, XmlNamespaceManager nsmgr) => s;
  }
}
