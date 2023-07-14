// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Schema.XsdIdentityField
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Xml.Schema;

namespace Mono.Xml.Schema
{
  internal class XsdIdentityField
  {
    private XsdIdentityPath[] fieldPaths;
    private int index;

    public XsdIdentityField(XmlSchemaXPath field, int index)
    {
      this.index = index;
      this.fieldPaths = field.CompiledExpression;
    }

    public XsdIdentityPath[] Paths => this.fieldPaths;

    public int Index => this.index;
  }
}
