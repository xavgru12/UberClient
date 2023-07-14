// Decompiled with JetBrains decompiler
// Type: Mono.Xml.Schema.XsdIdentitySelector
// Assembly: System.Xml, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: A6093E4D-5C47-4D02-9BF3-E0EBDD0B6ACE
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\UberStrike_v4-3-10\UberStrike_Data\Managed\System.Xml.dll

using System.Collections;
using System.Xml.Schema;

namespace Mono.Xml.Schema
{
  internal class XsdIdentitySelector
  {
    private XsdIdentityPath[] selectorPaths;
    private ArrayList fields = new ArrayList();
    private XsdIdentityField[] cachedFields;

    public XsdIdentitySelector(XmlSchemaXPath selector) => this.selectorPaths = selector.CompiledExpression;

    public XsdIdentityPath[] Paths => this.selectorPaths;

    public void AddField(XsdIdentityField field)
    {
      this.cachedFields = (XsdIdentityField[]) null;
      this.fields.Add((object) field);
    }

    public XsdIdentityField[] Fields
    {
      get
      {
        if (this.cachedFields == null)
          this.cachedFields = this.fields.ToArray(typeof (XsdIdentityField)) as XsdIdentityField[];
        return this.cachedFields;
      }
    }
  }
}
